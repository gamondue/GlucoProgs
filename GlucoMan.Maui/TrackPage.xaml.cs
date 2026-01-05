using gamon;
using GlucoMan;
using GlucoMan.Maui.Services;
using GlucoMan.Maui.Resources.Strings;
using System.Globalization;

namespace GlucoMan.Maui;

public partial class TrackPage : ContentPage
{
    private BL_GpsTracking bl;
    private IBackgroundGpsService backgroundGpsService;
    private bool isTracking = false;
    private DateTime trackingStartTime;
    private bool isViewOnlyMode = false; // True when viewing a saved track
    private int? loadedTrackId = null; // ID of the loaded track
    private bool mapIsReady = false; // Flag to track when map is ready
    private bool hasShownRecoveryDialog = false; // Prevent showing dialog multiple times

    // Google Maps API Key - should be configured in app settings
    private const string GOOGLE_MAPS_API_KEY = "YOUR_API_KEY_HERE";

    public TrackPage()
    {
        InitializeComponent();
        bl = new BL_GpsTracking();


        // Get background GPS service from DI
        backgroundGpsService = Application.Current.Handler.MauiContext.Services.GetService<IBackgroundGpsService>();

        // CRITICAL DIAGNOSTIC: Log if service was obtained
        if (backgroundGpsService == null)
        {
            General.LogOfProgram?.Error("TrackPage constructor - backgroundGpsService is NULL! GPS tracking will not work.", null);
            UpdateStatus("ERROR: GPS Service not available", Colors.Red);
        }
        else
        {
            // Subscribe to position updates from background service
            backgroundGpsService.OnPositionRecorded += OnBackgroundPositionRecorded;
        }

        // If a background tracking service is already running, DON'T set isTracking yet
        // Let OnAppearing handle recovery dialog first
        try
        {
            // Initialize current position label with localized text
            lblCurrentPosition.Text = AppStrings.TrackWaitingForGPS;

            if (backgroundGpsService != null && backgroundGpsService.IsTracking)
            {
                // Service is running but we don't know yet if this is a recovery scenario
                // Just set button states to "loading" state
                btnStartTracking.IsEnabled = false;
                btnStopTracking.IsEnabled = false;
                btnSaveTrack.IsEnabled = false;
                btnClearTrack.IsEnabled = false;

                UpdateStatus(AppStrings.TrackStatusChecking, Colors.Orange);
            }
            else
            {
                // Default initial state: Start enabled, others disabled
                isTracking = false;
                btnStartTracking.IsEnabled = true;
                btnStopTracking.IsEnabled = false;
                btnSaveTrack.IsEnabled = false;
                btnClearTrack.IsEnabled = false;

                UpdateStatus(AppStrings.TrackStatusReady, Colors.Gray);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage constructor - checking background service", ex);
        }

        InitializeMap();
    }

    /// <summary>
    /// Constructor that accepts a Track ID to load and display an existing track
    /// </summary>
    /// <param name="idTrack">ID of the track to load</param>
    public TrackPage(int idTrack) : this()
    {
        try
        {
            isViewOnlyMode = true;
            loadedTrackId = idTrack;

            // Load the track from database
            LoadAndDisplayTrack(idTrack);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error($"TrackPage - Constructor with IdTrack={idTrack}", ex);
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Wait for map to be ready before doing anything with it
        await WaitForMapReady();

        if (isViewOnlyMode)
        {
            // In view-only mode, disable tracking controls
            SetViewOnlyMode();
        }
        else
        {
            await CheckAndRequestLocationPermission();

            // Check if background service is running and has Data
            if (backgroundGpsService != null && backgroundGpsService.IsTracking)
            {
                int existingPositions = backgroundGpsService.GetPositionsCount();

                General.LogOfProgram?.Event($"TrackPage - OnAppearing: Service is tracking, has {existingPositions} positions, page isTracking={isTracking}, hasShownRecoveryDialog={hasShownRecoveryDialog}");

                // If we haven't shown recovery dialog yet AND we're not already tracking
                // (meaning app was closed/killed and service recovered)
                if (!hasShownRecoveryDialog && !isTracking && existingPositions > 0)
                {
                    // This is a recovery scenario - show dialog
                    hasShownRecoveryDialog = true;
                    await ShowRecoveryDialog(existingPositions);
                }
                else if (!isTracking)
                {
                    // Normal case: service is running, just resume tracking without dialog
                    isTracking = true;
                    trackingStartTime = backgroundGpsService.TrackingStartTime ?? DateTime.Now;

                    await SyncAndDisplayPositionsFromBackgroundService();

                    btnStartTracking.IsEnabled = false;
                    btnStopTracking.IsEnabled = true;
                    btnSaveTrack.IsEnabled = false;
                    btnClearTrack.IsEnabled = false;

                    UpdateStatus(AppStrings.TrackStatusBackground, Colors.Green);
                    UpdateStatistics();
                }
                else
                {
                    // Already tracking (page reappearing from background) - just refresh
                    await SyncAndDisplayPositionsFromBackgroundService();
                    UpdateStatistics();
                }
            }
        }
    }

    /// <summary>
    /// Show dialog when recovering a tracking session after app restart
    /// </summary>
    private async Task ShowRecoveryDialog(int positionsCount)
    {
        try
        {
            var action = await DisplayActionSheet(
                string.Format(AppStrings.TrackRecoveryTitle, positionsCount),
                AppStrings.TrackRecoveryCancel,
                null,
                AppStrings.TrackRecoveryContinue,
                AppStrings.TrackRecoverySaveStop);

            if (action == AppStrings.TrackRecoveryContinue)
            {
                // Resume tracking
                await SyncAndDisplayPositionsFromBackgroundService();

                isTracking = true;
                btnStartTracking.IsEnabled = false;
                btnStopTracking.IsEnabled = true;
                btnSaveTrack.IsEnabled = false;
                btnClearTrack.IsEnabled = false;

                UpdateStatus(AppStrings.TrackStatusResumed, Colors.Green);
                UpdateStatistics();

                General.LogOfProgram?.Event($"TrackPage - User chose to continue tracking {positionsCount} positions");
            }
            else if (action == AppStrings.TrackRecoverySaveStop)
            {
                // Save and stop
                await SyncAndDisplayPositionsFromBackgroundService();

                // Stop the background service
                if (backgroundGpsService != null)
                {
                    await backgroundGpsService.StopTrackingAsync();
                }

                isTracking = false;
                btnStartTracking.IsEnabled = true;
                btnStopTracking.IsEnabled = false;
                btnSaveTrack.IsEnabled = true;
                btnClearTrack.IsEnabled = true;

                UpdateStatus(AppStrings.TrackStatusReadyToSave, Colors.Orange);
                UpdateStatistics();

                // Optionally show save reminder
                await DisplayAlert(AppStrings.TrackReadyTitle,
                    string.Format(AppStrings.TrackReadyMessage, positionsCount),
                    AppStrings.OK);

                General.LogOfProgram?.Event($"TrackPage - User chose to save and stop {positionsCount} positions");
            }
            else // "Discard and Stop" or dismissed
            {
                // Discard and stop
                if (backgroundGpsService != null)
                {
                    await backgroundGpsService.StopTrackingAsync();
                    backgroundGpsService.ClearPositions();
                }

                isTracking = false;
                bl.CurrentTrack = new Track();

                btnStartTracking.IsEnabled = true;
                btnStopTracking.IsEnabled = false;
                btnSaveTrack.IsEnabled = false;
                btnClearTrack.IsEnabled = false;

                await mapWebView.EvaluateJavaScriptAsync("clearTrack()");

                UpdateStatus(AppStrings.TrackStatusDiscarded, Colors.Gray);

                General.LogOfProgram?.Event($"TrackPage - User chose to discard {positionsCount} positions");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - ShowRecoveryDialog", ex);
        }
    }

    /// <summary>
    /// Wait for the map WebView to be ready
    /// </summary>
    private async Task WaitForMapReady()
    {
        // Give map time to initialize (WebView needs time to load HTML/JS)
        int maxAttempts = 20;
        int attempt = 0;

        while (!mapIsReady && attempt < maxAttempts)
        {
            await Task.Delay(100);
            attempt++;

            // Try to check if map is ready by calling a simple JS function
            try
            {
                var result = await mapWebView.EvaluateJavaScriptAsync("typeof map !== 'undefined'");
                if (result == "true")
                {
                    mapIsReady = true;
                    General.LogOfProgram?.Event($"TrackPage - Map ready after {attempt * 100}ms");
                    break;
                }
            }
            catch
            {
                // Map not ready yet, continue waiting
            }
        }

        if (!mapIsReady)
        {
            // Fallback: assume it's ready after max wait time
            mapIsReady = true;
            General.LogOfProgram?.Event("TrackPage - Map assumed ready after timeout");
        }
    }

    /// <summary>
    /// Handle position updates from background service
    /// </summary>
    private async void OnBackgroundPositionRecorded(object sender, GpsPositionRecord e)
    {
        try
        {
            if (bl == null || bl.CurrentTrack == null)
            {
                General.LogOfProgram?.Error("TrackPage - OnBackgroundPositionRecorded: bl or bl.CurrentTrack is NULL!", null);
                return;
            }

            // Add position to business layer
            var addedPosition = bl.AddPosition(e.Latitude, e.Longitude, e.Altitude, e.Accuracy, e.Speed);

            if (addedPosition == null)
            {
                General.LogOfProgram?.Error("TrackPage - OnBackgroundPositionRecorded: bl.AddPosition returned NULL - track might not be recording!", null);
                return;
            }
            
            // Update UI on main thread
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    // Update map with new position
                    string script = $"updatePosition({e.Latitude.ToString(CultureInfo.InvariantCulture)}, {e.Longitude.ToString(CultureInfo.InvariantCulture)})";
                    await mapWebView.EvaluateJavaScriptAsync(script);

                    // Update current position label
                    lblCurrentPosition.Text = $"{e.Latitude:F6}, {e.Longitude:F6}";

                    // Update statistics
                    UpdateStatistics();
                }
                catch (Exception ex)
                {
                    General.LogOfProgram?.Error("TrackPage - OnBackgroundPositionRecorded UI update", ex);
                }
            });
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - OnBackgroundPositionRecorded", ex);
        }
    }

    /// <summary>
    /// Sync positions from background service to business layer AND display on map (when app resumes)
    /// This method reads positions WITHOUT clearing them from the service
    /// </summary>
    private async Task SyncAndDisplayPositionsFromBackgroundService()
    {
        try
        {
            if (backgroundGpsService == null) return;

            // Get positions WITHOUT clearing them (we'll clear only after explicit save or stop)
            var positions = backgroundGpsService.GetRecordedPositions();

            General.LogOfProgram?.Event($"TrackPage - Found {positions.Count} positions in background service");

            // Start new track if not already started
            if (bl.CurrentTrack == null || bl.CurrentTrack.Positions == null)
            {
                bl.StartNewTrack();
            }

            // Clear map first to avoid duplicates
            try
            {
                await mapWebView.EvaluateJavaScriptAsync("clearTrack()");
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("TrackPage - Error clearing map", ex);
            }

            // First, display any positions already in blMeal.CurrentTrack (from previous sessions)
            if (bl.CurrentTrack.Positions != null && bl.CurrentTrack.Positions.Count > 0)
            {
                foreach (var pos in bl.CurrentTrack.Positions)
                {
                    if (pos.Latitude.HasValue && pos.Longitude.HasValue)
                    {
                        string script = $"updatePosition({pos.Latitude.Value.ToString(CultureInfo.InvariantCulture)}, {pos.Longitude.Value.ToString(CultureInfo.InvariantCulture)})";
                        await mapWebView.EvaluateJavaScriptAsync(script);
                    }
                }
                General.LogOfProgram?.Event($"TrackPage - Displayed {bl.CurrentTrack.Positions.Count} existing positions on map");
            }

            // Then add new positions from background service (if any)
            int newPositionsAdded = 0;
            foreach (var pos in positions)
            {
                // Check if this position is already in the track (avoid duplicates based on lat/lon)
                bool alreadyExists = bl.CurrentTrack.Positions?.Any(p =>
                    p.Latitude == pos.Latitude &&
                    p.Longitude == pos.Longitude) ?? false;

                if (!alreadyExists)
                {
                    // Add to business layer
                    bl.AddPosition(pos.Latitude, pos.Longitude, pos.Altitude, pos.Accuracy, pos.Speed);
                    newPositionsAdded++;
                }

                // Always display on map (even if duplicate in BL, we cleared the map)
                string script = $"updatePosition({pos.Latitude.ToString(CultureInfo.InvariantCulture)}, {pos.Longitude.ToString(CultureInfo.InvariantCulture)})";
                await mapWebView.EvaluateJavaScriptAsync(script);
            }

            // Update last position label
            if (positions.Count > 0)
            {
                var lastPos = positions.Last();
                lblCurrentPosition.Text = $"{lastPos.Latitude:F6}, {lastPos.Longitude:F6}";
            }
            else if (bl.CurrentTrack.Positions?.Count > 0)
            {
                var lastPos = bl.CurrentTrack.Positions.Last();
                if (lastPos.Latitude.HasValue && lastPos.Longitude.HasValue)
                {
                    lblCurrentPosition.Text = $"{lastPos.Latitude.Value:F6}, {lastPos.Longitude.Value:F6}";
                }
            }

            UpdateStatistics();

            General.LogOfProgram?.Event($"TrackPage - Synced {newPositionsAdded} new positions, total displayed: {(bl.CurrentTrack.Positions?.Count ?? 0) + positions.Count}");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - SyncAndDisplayPositionsFromBackgroundService", ex);
        }
    }

    #region Track Loading and Display

    /// <summary>
    /// Loads a track from database and displays it on the map
    /// </summary>
    private async void LoadAndDisplayTrack(int idTrack)
    {
        try
        {
            // Load track from database
            var track = bl.GetOneTrack(idTrack);

            if (track == null)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    UpdateStatus(string.Format(AppStrings.TrackStatusNotFound, idTrack), Colors.Red);
                    await DisplayAlert(AppStrings.TrackNotFoundTitle,
                        string.Format(AppStrings.TrackNotFoundMessage, idTrack),
                        AppStrings.OK);
                });
                return;
            }

            // Set current track to the loaded track
            bl.CurrentTrack = track;

            // Wait for map to be ready, then display the track
            await WaitForMapReady();

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    // Display track on map
                    await DisplayTrackOnMap(track);

                    // Update statistics
                    UpdateStatistics();

                    // Update status
                    string trackName = !string.IsNullOrEmpty(track.Name) ? track.Name : $"Track {idTrack}";
                    UpdateStatus(string.Format(AppStrings.TrackStatusViewing, trackName), Colors.Blue);

                    General.LogOfProgram?.Event($"TrackPage - Loaded track ID {idTrack} with {track.Positions?.Count ?? 0} positions");
                }
                catch (Exception ex)
                {
                    General.LogOfProgram?.Error("TrackPage - LoadAndDisplayTrack UI update", ex);
                    UpdateStatus(AppStrings.TrackStatusDisplayError, Colors.Red);
                }
            });
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error($"TrackPage - LoadAndDisplayTrack ID={idTrack}", ex);
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                UpdateStatus(AppStrings.TrackStatusLoadError, Colors.Red);
                await DisplayAlert(AppStrings.TrackLoadErrorTitle,
                    AppStrings.TrackLoadErrorMessage,
                    AppStrings.OK);
            });
        }
    }

    /// <summary>
    /// Displays a track on the map by converting positions to JSON and calling JavaScript
    /// </summary>
    private async Task DisplayTrackOnMap(Track track)
    {
        if (track?.Positions == null || track.Positions.Count == 0)
        {
            UpdateStatus(AppStrings.TrackStatusNoPositions, Colors.Orange);
            return;
        }

        try
        {
            // Convert positions to JSON format for JavaScript
            var pointsJson = "[";
            for (int i = 0; i < track.Positions.Count; i++)
            {
                var pos = track.Positions[i];
                if (pos.Latitude.HasValue && pos.Longitude.HasValue)
                {
                    pointsJson += $"{{\"lat\":{pos.Latitude.Value.ToString(CultureInfo.InvariantCulture)},\"lng\":{pos.Longitude.Value.ToString(CultureInfo.InvariantCulture)}}}";
                    if (i < track.Positions.Count - 1)
                        pointsJson += ",";
                }
            }
            pointsJson += "]";

            // Call JavaScript function to load and display the track
            string script = $"loadTrack('{pointsJson}')";
            await mapWebView.EvaluateJavaScriptAsync(script);

            General.LogOfProgram?.Event($"TrackPage - Displayed {track.Positions.Count} positions on map");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - DisplayTrackOnMap", ex);
            throw;
        }
    }

    /// <summary>
    /// Sets the page to view-only mode (disables recording controls)
    /// </summary>
    private void SetViewOnlyMode()
    {
        try
        {
            btnStartTracking.IsEnabled = false;
            btnStopTracking.IsEnabled = false;
            btnSaveTrack.IsEnabled = false;
            btnClearTrack.IsEnabled = false;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - SetViewOnlyMode", ex);
        }
    }

    #endregion

    #region Permissions

    private async Task<bool> CheckAndRequestLocationPermission()
    {
        try
        {
            // Prima richiediamo il permesso base "When In Use"
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status != PermissionStatus.Granted)
            {
                UpdateStatus(AppStrings.TrackStatusPermissionDenied, Colors.Red);
                await DisplayAlert(AppStrings.TrackPermissionRequiredTitle,
                    AppStrings.TrackPermissionRequiredMessage,
                    AppStrings.OK);
                return false;
            }

            // Poi richiediamo il permesso Always (per tracking in background)
            // IMPORTANTE: Su Android questo deve essere richiesto DOPO il permesso WhenInUse
            var alwaysStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

            if (alwaysStatus != PermissionStatus.Granted)
            {
                // Mostra una spiegazione prima di richiedere il permesso Always
                bool requestAlways = await DisplayAlert(
                    AppStrings.TrackBackgroundPermissionTitle,
                    AppStrings.TrackBackgroundPermissionMessage,
                    AppStrings.TrackYesEnableButton,
                    AppStrings.TrackNoForegroundOnlyButton);

                if (requestAlways)
                {
                    alwaysStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();

                    if (alwaysStatus == PermissionStatus.Granted)
                    {
                        UpdateStatus(AppStrings.TrackStatusPermissionGrantedBackground, Colors.Green);
                        General.LogOfProgram?.Event("TrackPage - Background location permission granted");
                    }
                    else
                    {
                        UpdateStatus(AppStrings.TrackStatusPermissionGrantedForeground, Colors.Orange);
                        General.LogOfProgram?.Event("TrackPage - Background location permission denied, only foreground available");

                        await DisplayAlert(AppStrings.TrackLimitedGpsTitle,
                            AppStrings.TrackLimitedGpsMessage,
                            AppStrings.OK);
                    }
                }
                else
                {
                    UpdateStatus(AppStrings.TrackStatusPermissionGrantedForeground, Colors.Orange);
                    General.LogOfProgram?.Event("TrackPage - User declined background location permission");
                }
            }
            else
            {
                UpdateStatus(AppStrings.TrackStatusPermissionGrantedBackground, Colors.Green);
            }

            return true;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - CheckAndRequestLocationPermission", ex);
            UpdateStatus(AppStrings.TrackStatusPermissionCheckError, Colors.Red);
            return false;
        }
    }

    #endregion

    #region Map Initialization

    private void InitializeMap()
    {
        try
        {
            mapIsReady = false;
            string html = GetMapHtml();
            mapWebView.Source = new HtmlWebViewSource { Html = html };
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - InitializeMap", ex);
        }
    }

    private string GetMapHtml()
    {
        // Default center: Piazza del Quirinale, Rome (41.8992, 12.4872)
        return @"
<!DOCTYPE html>
<html>
<head>
    <meta charset='utf-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no'>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <style>
        html, body { height: 100%; margin: 0; padding: 0; }
        #map { height: 100%; width: 100%; }
    </style>
</head>
<body>
    <div id='map'></div>
    <script>
        var map = L.map('map').setView([41.899125, 12.486705], 15); // Default: Piazza del Quirinale, Rome
        
        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '© OpenStreetMap contributors'
        }).addTo(map);
        
        var trackLine = null;
        var trackPoints = [];
        var currentMarker = null;
        
        function updatePosition(lat, lng) {
            var latlng = L.latLng(lat, lng);
            trackPoints.push(latlng);
            
            if (trackLine) {
                trackLine.setLatLngs(trackPoints);
            } else {
                trackLine = L.polyline(trackPoints, {
                    color: 'blue',
                    weight: 4,
                    opacity: 0.7
                }).addTo(map);
            }
            
            if (currentMarker) {
                currentMarker.setLatLng(latlng);
            } else {
                currentMarker = L.circleMarker(latlng, {
                    radius: 8,
                    fillColor: 'red',
                    color: 'white',
                    weight: 2,
                    fillOpacity: 1
                }).addTo(map);
            }
            
            map.setView(latlng, map.getZoom());
        }
        
        function clearTrack() {
            trackPoints = [];
            if (trackLine) {
                map.removeLayer(trackLine);
                trackLine = null;
            }
            if (currentMarker) {
                map.removeLayer(currentMarker);
                currentMarker = null;
            }
        }
        
        function setCenter(lat, lng, zoom) {
            map.setView([lat, lng], zoom || 15);
        }
        
        function loadTrack(pointsJson) {
            clearTrack();
            try {
                var points = JSON.parse(pointsJson);
                points.forEach(function(p) {
                    updatePosition(p.lat, p.lng);
                });
                if (trackPoints.length > 0) {
                    map.fitBounds(trackLine.getBounds(), { padding: [20, 20] });
                }
            } catch(e) {
                console.error('Error loading track:', e);
            }
        }
    </script>
</body>
</html>";
    }

    #endregion

    #region GPS Tracking

    private async void BtnStartTracking_Clicked(object sender, EventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("TrackPage - BtnStartTracking: Button clicked");

            if (backgroundGpsService == null)
            {
                General.LogOfProgram?.Error("TrackPage - BtnStartTracking: backgroundGpsService is NULL!", null);
                await DisplayAlert("Error", "GPS service is not available. Please restart the app.", "OK");
                return;
            }

            General.LogOfProgram?.Event($"TrackPage - BtnStartTracking: Service type = {backgroundGpsService.GetType().Name}");

            if (!await CheckAndRequestLocationPermission())
            {
                General.LogOfProgram?.Event("TrackPage - BtnStartTracking: Permission denied");
                return;
            }

            General.LogOfProgram?.Event("TrackPage - BtnStartTracking: Permissions granted, starting new track in BL");

            // Start new track in business layer
            bl.StartNewTrack();

            isTracking = true;
            trackingStartTime = DateTime.Now;

            btnStartTracking.IsEnabled = false;
            btnStopTracking.IsEnabled = true;
            btnSaveTrack.IsEnabled = false;

            UpdateStatus(AppStrings.TrackStatusStartingBackground, Colors.Green);

            await mapWebView.EvaluateJavaScriptAsync("clearTrack()");

            General.LogOfProgram?.Event("TrackPage - BtnStartTracking: Calling backgroundGpsService.StartTrackingAsync()");

            bool started = await backgroundGpsService.StartTrackingAsync();

            General.LogOfProgram?.Event($"TrackPage - BtnStartTracking: StartTrackingAsync returned {started}");

            if (started)
            {
                UpdateStatus(AppStrings.TrackStatusBackground, Colors.Green);
                // Reset current position label to waiting state (it will be updated when first GPS fix arrives)
                lblCurrentPosition.Text = AppStrings.TrackWaitingForGPS;
                General.LogOfProgram?.Event("TrackPage - BtnStartTracking: GPS tracking started successfully");
            }
            else
            {
                UpdateStatus(AppStrings.TrackStatusBackgroundFailed, Colors.Orange);
                General.LogOfProgram?.Error("TrackPage - BtnStartTracking: StartTrackingAsync returned false", null);

                await DisplayAlert("GPS Error",
                    "Failed to start GPS tracking. Please check:\n" +
                    "1. Location services are enabled\n" +
                    "2. App has location permissions\n" +
                    "3. Check logs for details",
                    "OK");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - BtnStartTracking_Clicked", ex);
            UpdateStatus(AppStrings.TrackStatusStartError, Colors.Red);

            await DisplayAlert("Exception", $"Error starting GPS: {ex.Message}", "OK");
        }
    }

    private async void BtnStopTracking_Clicked(object sender, EventArgs e)
    {
        try
        {
            isTracking = false;

            if (backgroundGpsService != null)
            {
                // Get final positions and add to BL before stopping
                var finalPositions = backgroundGpsService.GetAndClearPositions();
                foreach (var pos in finalPositions)
                {
                    bool alreadyExists = bl.CurrentTrack?.Positions?.Any(p =>
                        p.Latitude == pos.Latitude &&
                        p.Longitude == pos.Longitude) ?? false;

                    if (!alreadyExists)
                    {
                        bl.AddPosition(pos.Latitude, pos.Longitude, pos.Altitude, pos.Accuracy, pos.Speed);
                    }
                }

                await backgroundGpsService.StopTrackingAsync();
            }

            bl.StopCurrentTrack();

            btnStartTracking.IsEnabled = true;
            btnStopTracking.IsEnabled = false;
            btnSaveTrack.IsEnabled = true;

            UpdateStatus(AppStrings.TrackStatusStopped, Colors.Orange);
            UpdateStatistics();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - BtnStopTracking_Clicked", ex);
            UpdateStatus(AppStrings.TrackStatusStopError, Colors.Red);
        }
    }

    #endregion

    #region UI Updates

    private void UpdateStatus(string message, Color color)
    {
        lblStatus.Text = message;
        lblStatus.TextColor = color;
    }

    private void UpdateStatistics()
    {
        try
        {
            if (bl.CurrentTrack != null)
            {
                lblPointsCount.Text = bl.CurrentTrack.Positions?.Count.ToString() ?? "0";
                lblDistance.Text = bl.GetFormattedDistance(bl.CurrentTrack);
                lblSpeed.Text = bl.GetFormattedSpeed(bl.CurrentTrack);

                if (isTracking)
                {
                    var elapsed = DateTime.Now - trackingStartTime;
                    lblDuration.Text = elapsed.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    lblDuration.Text = bl.GetFormattedDuration(bl.CurrentTrack);
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - UpdateStatistics", ex);
        }
    }

    #endregion

    #region Button Events

    private async void BtnSaveTrack_Clicked(object sender, EventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("TrackPage - BtnSaveTrack: Starting save process");

            // FIRST: Ensure we have a track initialized
            if (bl.CurrentTrack == null)
            {
                bl.StartNewTrack();
            }

            // Count what we have BEFORE merging
            int existingPositions = bl.CurrentTrack.Positions?.Count ?? 0;
            int servicePositions = backgroundGpsService?.GetPositionsCount() ?? 0;

            General.LogOfProgram?.Event($"TrackPage - BtnSaveTrack: Before merge - bl.CurrentTrack has {existingPositions} positions, background service has {servicePositions} positions");

            // SECOND: Merge positions from background service if available
            if (backgroundGpsService != null && servicePositions > 0)
            {
                try
                {
                    // Use GetRecordedPositions (peek) to see what we have
                    var allPositions = backgroundGpsService.GetRecordedPositions();

                    General.LogOfProgram?.Event($"TrackPage - BtnSaveTrack: Retrieved {allPositions?.Count ?? 0} positions from background service");

                    if (allPositions != null && allPositions.Count > 0)
                    {
                        // If blMeal.CurrentTrack already has positions, merge them
                        // Otherwise, rebuild from scratch
                        if (existingPositions > 0)
                        {
                            // Merge: add only new positions from service that aren't already in track
                            int addedCount = 0;
                            foreach (var pos in allPositions.OrderBy(p => p.Timestamp))
                            {
                                bool alreadyExists = bl.CurrentTrack.Positions?.Any(p =>
                                    Math.Abs(p.Latitude.GetValueOrDefault() - pos.Latitude) < 0.00001 &&
                                    Math.Abs(p.Longitude.GetValueOrDefault() - pos.Longitude) < 0.00001) ?? false;

                                if (!alreadyExists)
                                {
                                    bl.AddPosition(pos.Latitude, pos.Longitude, pos.Altitude, pos.Accuracy, pos.Speed);
                                    addedCount++;
                                }
                            }
                            General.LogOfProgram?.Event($"TrackPage - BtnSaveTrack: Merged {addedCount} new positions into existing track");
                        }
                        else
                        {
                            // Rebuild from scratch - track was empty
                            var oldTrack = bl.CurrentTrack;
                            bl.StartNewTrack();

                            // Copy metadata
                            if (oldTrack != null)
                            {
                                bl.CurrentTrack.Name = oldTrack.Name;
                                bl.CurrentTrack.StartTime = oldTrack.StartTime;
                            }

                            // Add all positions from background service
                            foreach (var pos in allPositions.OrderBy(p => p.Timestamp))
                            {
                                bl.AddPosition(pos.Latitude, pos.Longitude, pos.Altitude, pos.Accuracy, pos.Speed);
                            }

                            General.LogOfProgram?.Event($"TrackPage - BtnSaveTrack: Rebuilt track from scratch with {bl.CurrentTrack.Positions?.Count ?? 0} positions");
                        }
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram?.Error("TrackPage - BtnSaveTrack_Clicked merge background positions", ex);
                }
            }

            // THIRD: Final check - do we have anything to save?
            int finalPositionCount = bl.CurrentTrack?.Positions?.Count ?? 0;

            General.LogOfProgram?.Event($"TrackPage - BtnSaveTrack: Final position count before save: {finalPositionCount}");

            if (bl.CurrentTrack == null || bl.CurrentTrack.Positions == null || finalPositionCount == 0)
            {
                await DisplayAlert(AppStrings.TrackNoDataTitle,
                    string.Format(AppStrings.TrackNoDataMessage, existingPositions, servicePositions),
                    AppStrings.OK);
                return;
            }

            // FOURTH: Save the track (persist to database)
            bl.SaveTrack(bl.CurrentTrack);

            General.LogOfProgram?.Event($"TrackPage - BtnSaveTrack: Successfully saved track with {finalPositionCount} positions to database");

            await DisplayAlert(AppStrings.TrackSavedTitle,
                string.Format(AppStrings.TrackSavedMessage, finalPositionCount,
                    bl.GetFormattedDistance(bl.CurrentTrack),
                    bl.GetFormattedDuration(bl.CurrentTrack)),
                AppStrings.OK);

            // FIFTH: Clear the background service queue since we've saved everything
            try
            {
                backgroundGpsService?.ClearPositions();
                General.LogOfProgram?.Event("TrackPage - BtnSaveTrack: Cleared background service positions after save");
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("TrackPage - BtnSaveTrack: Error clearing background positions", ex);
            }

            btnSaveTrack.IsEnabled = false;
            UpdateStatus(AppStrings.TrackStatusSaved, Colors.Green);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - BtnSaveTrack_Clicked", ex);
            await DisplayAlert(AppStrings.TrackSaveErrorTitle,
                string.Format(AppStrings.TrackSaveErrorMessage, ex.Message),
                AppStrings.OK);
        }
    }

    private async void BtnClearTrack_Clicked(object sender, EventArgs e)
    {
        try
        {
            bool confirm = await DisplayAlert(AppStrings.TrackClearTitle,
                AppStrings.TrackClearMessage,
                AppStrings.Yes,
                AppStrings.No);

            if (confirm)
            {
                if (isTracking)
                {
                    isTracking = false;
                    if (backgroundGpsService != null)
                    {
                        await backgroundGpsService.StopTrackingAsync();
                    }
                    btnStartTracking.IsEnabled = true;
                    btnStopTracking.IsEnabled = false;
                }

                backgroundGpsService?.ClearPositions();
                bl.CurrentTrack = new Track();

                await mapWebView.EvaluateJavaScriptAsync("clearTrack()");

                lblPointsCount.Text = "0";
                lblDistance.Text = "0 m";
                lblDuration.Text = "00:00:00";
                lblSpeed.Text = "0 km/h";
                lblCurrentPosition.Text = AppStrings.TrackWaitingForGPS;
                btnSaveTrack.IsEnabled = false;

                UpdateStatus(AppStrings.TrackStatusCleared, Colors.Gray);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - BtnClearTrack_Clicked", ex);
        }
    }

    private async void BtnBack_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (isTracking && backgroundGpsService != null && backgroundGpsService.IsTracking)
            {
                bool continueInBackground = await DisplayAlert(AppStrings.TrackKeepTrackingTitle,
                    AppStrings.TrackKeepTrackingMessage,
                    AppStrings.TrackKeepTrackingAccept,
                    AppStrings.TrackKeepTrackingCancel);

                if (!continueInBackground)
                {
                    isTracking = false;
                    await backgroundGpsService.StopTrackingAsync();
                    bl.StopCurrentTrack();
                }
            }

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TrackPage - BtnBack_Clicked", ex);
            await Navigation.PopAsync();
        }
    }
    #endregion
}
