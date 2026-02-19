using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using AndroidLocation = Android.Locations.Location;
using AndroidOS = Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using gamon;
using GlucoMan.Maui.Resources.Strings;
using System.Collections.Concurrent;
using System.Text.Json;

namespace GlucoMan.Maui.Platforms.Android;

/// <summary>
/// Android Foreground Service for GPS tracking in background.
/// Stores positions in memory (RAM) and periodically persists to disk.
/// Survives app termination and can be recovered when app resumes.
/// </summary>
[Service(ForegroundServiceType = ForegroundService.TypeLocation, Exported = false)]
public class GpsTrackingService : Service, ILocationListener
{
    private const int NOTIFICATION_ID = 9999;
    private const string CHANNEL_ID = "gps_tracking_channel";
    private const string CHANNEL_NAME = "GPS Tracking";
    private const string PREFS_NAME = "GpsTrackingPrefs";
    private const string KEY_IS_TRACKING = "IsTracking";
    private const string KEY_START_TIME = "StartTime";
    private const string POSITIONS_FILE = "gps_positions_temp.json";
    private const int SAVE_INTERVAL_POSITIONS = 1; // Save after EVERY position for reliability
    private const int KEEPALIVE_INTERVAL_MS = 30000; // 30 seconds keepalive timer
    
    private LocationManager locationManager;
    private AndroidOS.PowerManager.WakeLock wakeLock;
    private bool isTracking = false;
    private int positionsSinceLastSave = 0;
    private System.Timers.Timer keepAliveTimer; // Timer to periodically request location
    private DateTime lastPositionTime = DateTime.MinValue;
    
    // Thread-safe collection for storing positions in memory
    private static readonly ConcurrentQueue<GpsPositionData> positionsQueue = new();
    
    // Static event to notify UI when new position is recorded
    public static event EventHandler<GpsPositionData> OnPositionRecorded;
    
    // Static flag to check if service is running
    public static bool IsRunning { get; private set; } = false;
    
    // Track start time
    public static DateTime? TrackingStartTime { get; private set; }
    
    /// <summary>
    /// Data class for GPS position (lightweight, no database dependencies)
    /// </summary>
    public class GpsPositionData
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public float? Accuracy { get; set; }
        public float? Speed { get; set; }
        public DateTime Timestamp { get; set; }
    }
    
    public override AndroidOS.IBinder OnBind(Intent intent) => null;
    
    public override void OnCreate()
    {
        base.OnCreate();
        CreateNotificationChannel();
        locationManager = (LocationManager)GetSystemService(LocationService);
        
        // Acquire wake lock to prevent CPU sleep
        var powerManager = (AndroidOS.PowerManager)GetSystemService(PowerService);
        wakeLock = powerManager.NewWakeLock(AndroidOS.WakeLockFlags.Partial, "GlucoMan::GpsTrackingWakeLock");
    }
    
    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        try
        {
            string action = intent?.Action;
            
            if (action == "START_TRACKING")
            {
                StartTracking();
            }
            else if (action == "STOP_TRACKING")
            {
                StopTracking();
                StopSelf();
            }
            else if (action == null)
            {
                // Service restarted by system - check if we were tracking
                var prefs = GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
                bool wasTracking = prefs.GetBoolean(KEY_IS_TRACKING, false);
                
                if (wasTracking)
                {
                    RestoreTrackingState();
                }
            }
            
            return StartCommandResult.Sticky;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - OnStartCommand", ex);
            return StartCommandResult.Sticky; // Still try to restart
        }
    }
    
    private void StartTracking()
    {
        if (isTracking) return;
        
        try
        {
            // Acquire wake lock
            if (!wakeLock.IsHeld)
            {
                wakeLock.Acquire();
                General.LogOfProgram?.Event("GpsTrackingService - WakeLock acquired");
            }
            
            // Start as foreground service with notification
            var notification = CreateNotification("GPS Tracking Active", "Recording your activity...");
            
            if (AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.Q)
            {
                StartForeground(NOTIFICATION_ID, notification, ForegroundService.TypeLocation);
            }
            else
            {
                StartForeground(NOTIFICATION_ID, notification);
            }
            
            // DON'T clear previous positions - they may be from a recovery scenario
            // Only clear if this is a fresh start (no positions in file)
            var existingPositions = LoadPositionsFromFileInternal();
            if (existingPositions == null || existingPositions.Count == 0)
            {
                // Fresh start - clear queue
                while (positionsQueue.TryDequeue(out _)) { }
                DeletePositionsFile();
                TrackingStartTime = DateTime.Now;
                General.LogOfProgram?.Event("GpsTrackingService - Fresh start, cleared queue");
            }
            else
            {
                // Recovery - keep existing positions
                General.LogOfProgram?.Event($"GpsTrackingService - Resuming with {positionsQueue.Count} existing positions");
                if (!TrackingStartTime.HasValue)
                {
                    var prefs = GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
                    long startTimeTicks = prefs.GetLong(KEY_START_TIME, 0);
                    TrackingStartTime = startTimeTicks > 0 ? new DateTime(startTimeTicks) : DateTime.Now;
                }
            }
            
            // Save tracking state to preferences
            SaveTrackingState(true, TrackingStartTime ?? DateTime.Now);
            
            // Request location updates with aggressive settings for continuous tracking
            RequestLocationUpdates();
            
            // Start keepalive timer to periodically request location
            // This prevents Android from suspending GPS updates when stationary
            StartKeepAliveTimer();
            
            isTracking = true;
            IsRunning = true;
            lastPositionTime = DateTime.Now;
            
            General.LogOfProgram?.Event($"GpsTrackingService - Started background GPS tracking. Queue has {positionsQueue.Count} positions");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - StartTracking", ex);
        }
    }
    
    /// <summary>
    /// Request location updates from all available providers
    /// </summary>
    private void RequestLocationUpdates()
    {
        try
        {
            // Remove any existing listeners first
            locationManager?.RemoveUpdates(this);
            
            // GPS provider - most accurate
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                locationManager.RequestLocationUpdates(
                    LocationManager.GpsProvider,
                    3000, // 3 seconds (more frequent)
                    0,    // 0 meters - get ALL updates regardless of movement
                    this);
                General.LogOfProgram?.Event("GpsTrackingService - GPS provider: listening (3s, 0m)");
            }
            else
            {
                General.LogOfProgram?.Error("GpsTrackingService - GPS provider NOT enabled!", null);
            }
            
            // Network provider - backup
            if (locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locationManager.RequestLocationUpdates(
                    LocationManager.NetworkProvider,
                    10000, // 10 seconds
                    0,     // 0 meters
                    this);
                General.LogOfProgram?.Event("GpsTrackingService - Network provider: listening (10s, 0m)");
            }
            
            // Passive provider - receives updates from other apps
            if (locationManager.IsProviderEnabled(LocationManager.PassiveProvider))
            {
                locationManager.RequestLocationUpdates(
                    LocationManager.PassiveProvider,
                    0,
                    0,
                    this);
                General.LogOfProgram?.Event("GpsTrackingService - Passive provider: listening");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - RequestLocationUpdates", ex);
        }
    }
    
    /// <summary>
    /// Start a timer that periodically requests location to keep GPS active
    /// </summary>
    private void StartKeepAliveTimer()
    {
        try
        {
            StopKeepAliveTimer(); // Stop any existing timer
            
            keepAliveTimer = new System.Timers.Timer(KEEPALIVE_INTERVAL_MS);
            keepAliveTimer.Elapsed += OnKeepAliveTimerElapsed;
            keepAliveTimer.AutoReset = true;
            keepAliveTimer.Start();
            
            General.LogOfProgram?.Event($"GpsTrackingService - KeepAlive timer started ({KEEPALIVE_INTERVAL_MS}ms interval)");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - StartKeepAliveTimer", ex);
        }
    }
    
    private void StopKeepAliveTimer()
    {
        try
        {
            if (keepAliveTimer != null)
            {
                keepAliveTimer.Stop();
                keepAliveTimer.Dispose();
                keepAliveTimer = null;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - StopKeepAliveTimer", ex);
        }
    }
    
    /// <summary>
    /// Keepalive timer callback - check if we're still receiving updates
    /// </summary>
    private void OnKeepAliveTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        try
        {
            if (!isTracking) return;
            
            var timeSinceLastPosition = DateTime.Now - lastPositionTime;
            
            General.LogOfProgram?.Event($"GpsTrackingService - KeepAlive check: {timeSinceLastPosition.TotalSeconds:F0}s since last position, queue has {positionsQueue.Count} positions");
            
            // If we haven't received a position in more than 60 seconds, try to wake up GPS
            if (timeSinceLastPosition.TotalSeconds > 60)
            {
                General.LogOfProgram?.Event("GpsTrackingService - GPS appears stalled, requesting single update to wake it up");
                
                // Request a single immediate location update to wake up GPS
                try
                {
                    if (locationManager.IsProviderEnabled(LocationManager.GpsProvider))
                    {
                        var lastKnown = locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
                        if (lastKnown != null)
                        {
                            General.LogOfProgram?.Event($"GpsTrackingService - Last known GPS: Lat={lastKnown.Latitude:F6}, Lon={lastKnown.Longitude:F6}, Age={(DateTime.Now - DateTimeOffset.FromUnixTimeMilliseconds(lastKnown.Time).DateTime).TotalSeconds:F0}s");
                        }
                    }
                    
                    // Re-request location updates (this can help restart stalled GPS)
                    RequestLocationUpdates();
                }
                catch (Exception ex)
                {
                    General.LogOfProgram?.Error("GpsTrackingService - Error requesting wake-up location", ex);
                }
            }
            
            // Also save positions periodically as a safety measure
            if (positionsQueue.Count > 0)
            {
                SavePositionsToFile();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - OnKeepAliveTimerElapsed", ex);
        }
    }
    
    private void StopTracking()
    {
        if (!isTracking) return;
        
        try
        {
            // Stop keepalive timer first
            StopKeepAliveTimer();
            
            // IMPORTANT: Save positions to file BEFORE stopping, so they can be recovered
            SavePositionsToFile();
            General.LogOfProgram?.Event($"GpsTrackingService - StopTracking: Saved {positionsQueue.Count} positions to file before stopping");
            
            locationManager?.RemoveUpdates(this);
            isTracking = false;
            IsRunning = false;
            
            // Release wake lock
            if (wakeLock?.IsHeld == true)
            {
                wakeLock.Release();
                General.LogOfProgram?.Event("GpsTrackingService - WakeLock released");
            }
            
            // Clear tracking state but DON'T delete positions file yet
            // The positions file will be deleted when positions are consumed by the UI
            SaveTrackingState(false, null);
            
            StopForeground(StopForegroundFlags.Remove);
            
            General.LogOfProgram?.Event($"GpsTrackingService - Stopped. Queue still has {positionsQueue.Count} positions");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - StopTracking", ex);
        }
    }
    
    private void RestoreTrackingState()
    {
        try
        {
            var prefs = GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
            bool wasTracking = prefs.GetBoolean(KEY_IS_TRACKING, false);
            
            if (wasTracking)
            {
                long startTimeTicks = prefs.GetLong(KEY_START_TIME, 0);
                TrackingStartTime = startTimeTicks > 0 ? new DateTime(startTimeTicks) : DateTime.Now;
                
                // Load positions from file
                LoadPositionsFromFile();
                
                // Restart tracking
                General.LogOfProgram?.Event($"GpsTrackingService - Restoring tracking session started at {TrackingStartTime}, recovered {positionsQueue.Count} positions");
                
                // Re-acquire wake lock and restart GPS
                if (!wakeLock.IsHeld)
                {
                    wakeLock.Acquire();
                }
                
                var notification = CreateNotification("GPS Tracking Recovered", $"Restored {positionsQueue.Count} positions");
                StartForeground(NOTIFICATION_ID, notification, ForegroundService.TypeLocation);
                
                if (locationManager.IsProviderEnabled(LocationManager.GpsProvider))
                {
                    locationManager.RequestLocationUpdates(LocationManager.GpsProvider, 5000, 8, this);
                }
                
                if (locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
                {
                    locationManager.RequestLocationUpdates(LocationManager.NetworkProvider, 15000, 10, this);
                }
                
                isTracking = true;
                IsRunning = true;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - RestoreTrackingState", ex);
        }
    }
    
    public override void OnDestroy()
    {
        StopKeepAliveTimer();
        StopTracking();
        
        // Release wake lock if still held
        if (wakeLock?.IsHeld == true)
        {
            wakeLock.Release();
        }
        
        base.OnDestroy();
    }
    
    #region ILocationListener Implementation
    
    public void OnLocationChanged(AndroidLocation location)
    {
        if (location == null) return;
        
        try
        {
            // Update last position time
            lastPositionTime = DateTime.Now;
            
            var positionData = new GpsPositionData
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Altitude = location.HasAltitude ? location.Altitude : null,
                Accuracy = location.HasAccuracy ? location.Accuracy : null,
                Speed = location.HasSpeed ? location.Speed : null,
                Timestamp = DateTime.Now
            };
            
            // Check for duplicate position (same lat/lon as last)
            var lastPosition = positionsQueue.LastOrDefault();
            if (lastPosition != null)
            {
                double latDiff = Math.Abs(lastPosition.Latitude - positionData.Latitude);
                double lonDiff = Math.Abs(lastPosition.Longitude - positionData.Longitude);
                
                // If position hasn't changed significantly (less than ~1 meter), skip it
                // but still update lastPositionTime to show GPS is active
                if (latDiff < 0.00001 && lonDiff < 0.00001)
                {
                    // Position hasn't changed, but GPS is still working
                    return;
                }
            }
            
            // Store in memory queue
            positionsQueue.Enqueue(positionData);
            positionsSinceLastSave++;
            
            // Log position recording
            General.LogOfProgram?.Event($"GpsTrackingService - OnLocationChanged: Lat={positionData.Latitude:F6}, Lon={positionData.Longitude:F6}, Acc={positionData.Accuracy:F1}m, QueueCount={positionsQueue.Count}");
            
            // Save to file after every position for reliability
            if (positionsSinceLastSave >= SAVE_INTERVAL_POSITIONS)
            {
                SavePositionsToFile();
                positionsSinceLastSave = 0;
            }
            
            // Notify listeners (UI)
            OnPositionRecorded?.Invoke(this, positionData);
            
            // Update notification with position count
            UpdateNotification($"GPS Tracking Active", $"Recorded {positionsQueue.Count} positions");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - OnLocationChanged", ex);
        }
    }
    
    public void OnProviderDisabled(string provider)
    {
        General.LogOfProgram?.Event($"GpsTrackingService - Provider disabled: {provider}");
    }
    
    public void OnProviderEnabled(string provider)
    {
        General.LogOfProgram?.Event($"GpsTrackingService - Provider enabled: {provider}");
    }
    
    public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, AndroidOS.Bundle extras)
    {
        // Deprecated in API 29, but still called on older devices
    }
    
    #endregion
    
    #region Persistence
    
    private void SaveTrackingState(bool isTracking, DateTime? startTime)
    {
        try
        {
            var prefs = GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
            var editor = prefs.Edit();
            editor.PutBoolean(KEY_IS_TRACKING, isTracking);
            editor.PutLong(KEY_START_TIME, startTime?.Ticks ?? 0);
            editor.Apply();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - SaveTrackingState", ex);
        }
    }
    
    private void SavePositionsToFile()
    {
        try
        {
            var positions = positionsQueue.ToList();
            if (positions.Count == 0)
            {
                General.LogOfProgram?.Event("GpsTrackingService - SavePositionsToFile: No positions to save");
                return;
            }
            
            var json = JsonSerializer.Serialize(positions);
            var filePath = Path.Combine(FilesDir.AbsolutePath, POSITIONS_FILE);
            File.WriteAllText(filePath, json);
            
            General.LogOfProgram?.Event($"GpsTrackingService - Saved {positions.Count} positions to file: {filePath}");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - SavePositionsToFile", ex);
        }
    }
    
    /// <summary>
    /// Internal method that returns the loaded positions without modifying the queue
    /// </summary>
    private List<GpsPositionData> LoadPositionsFromFileInternal()
    {
        try
        {
            var filePath = Path.Combine(FilesDir.AbsolutePath, POSITIONS_FILE);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var positions = JsonSerializer.Deserialize<List<GpsPositionData>>(json);
                return positions ?? new List<GpsPositionData>();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - LoadPositionsFromFileInternal", ex);
        }
        return new List<GpsPositionData>();
    }
    
    private void LoadPositionsFromFile()
    {
        try
        {
            var positions = LoadPositionsFromFileInternal();
            
            if (positions != null && positions.Count > 0)
            {
                // Merge with existing queue (avoid duplicates based on timestamp)
                var existingTimestamps = positionsQueue.Select(p => p.Timestamp).ToHashSet();
                
                foreach (var pos in positions)
                {
                    if (!existingTimestamps.Contains(pos.Timestamp))
                    {
                        positionsQueue.Enqueue(pos);
                    }
                }
                
                General.LogOfProgram?.Event($"GpsTrackingService - Loaded {positions.Count} positions from file, queue now has {positionsQueue.Count}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - LoadPositionsFromFile", ex);
        }
    }
    
    private void DeletePositionsFile()
    {
        try
        {
            var filePath = Path.Combine(FilesDir.AbsolutePath, POSITIONS_FILE);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - DeletePositionsFile", ex);
        }
    }
    
    #endregion
    
    #region Notification
    
    private void CreateNotificationChannel()
    {
        if (AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(
                CHANNEL_ID,
                CHANNEL_NAME,
                NotificationImportance.Low)
            {
                Description = "GPS tracking notification for activity recording"
            };
            
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager?.CreateNotificationChannel(channel);
        }
    }
    
    private Notification CreateNotification(string title, string content)
    {
        var intent = new Intent(this, typeof(MainActivity));
        intent.SetFlags(ActivityFlags.SingleTop);
        
        var pendingIntentFlags = AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.S
            ? PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable
            : PendingIntentFlags.UpdateCurrent;
            
        var pendingIntent = PendingIntent.GetActivity(this, 0, intent, pendingIntentFlags);
        
        var stopIntent = new Intent(this, typeof(GpsTrackingService));
        stopIntent.SetAction("STOP_TRACKING");
        var stopPendingIntent = PendingIntent.GetService(this, 1, stopIntent, pendingIntentFlags);
        
        var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
            .SetContentTitle(title)
            .SetContentText(content)
            .SetSmallIcon(global::Android.Resource.Drawable.IcMenuMyLocation)
            .SetOngoing(true)
            .SetContentIntent(pendingIntent)
            .AddAction(0, AppStrings.StopTracking, stopPendingIntent)
            .SetPriority(NotificationCompat.PriorityLow);
        
        return builder.Build();
    }
    
    private void UpdateNotification(string title, string content)
    {
        try
        {
            var notification = CreateNotification(title, content);
            var notificationManager = (NotificationManager)GetSystemService(NotificationService);
            notificationManager?.Notify(NOTIFICATION_ID, notification);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - UpdateNotification", ex);
        }
    }
    
    #endregion
    
    #region Static Methods for accessing recorded positions
    
    public static List<GpsPositionData> GetAndClearPositions()
    {
        var positions = new List<GpsPositionData>();
        while (positionsQueue.TryDequeue(out var position))
        {
            positions.Add(position);
        }
        return positions;
    }
    
    public static List<GpsPositionData> PeekAllPositions()
    {
        return positionsQueue.ToList();
    }
    
    public static int GetPositionsCount() => positionsQueue.Count;
    
    public static void ClearPositions()
    {
        while (positionsQueue.TryDequeue(out _)) { }
    }
    
    #endregion
}
