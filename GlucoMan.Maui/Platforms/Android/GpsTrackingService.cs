using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Locations;
using AndroidLocation = Android.Locations.Location;
using AndroidOS = Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using gamon;
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
    private const int SAVE_INTERVAL_POSITIONS = 10; // Save every 10 positions
    
    private LocationManager locationManager;
    private AndroidOS.PowerManager.WakeLock wakeLock;
    private bool isTracking = false;
    private int positionsSinceLastSave = 0;
    
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
            
            // Clear previous positions
            while (positionsQueue.TryDequeue(out _)) { }
            DeletePositionsFile();
            
            // Record tracking start time
            TrackingStartTime = DateTime.Now;
            
            // Save tracking state to preferences
            SaveTrackingState(true, TrackingStartTime.Value);
            
            // Request location updates
            if (locationManager.IsProviderEnabled(LocationManager.GpsProvider))
            {
                locationManager.RequestLocationUpdates(
                    LocationManager.GpsProvider,
                    5000, // 5 seconds
                    8,     // 8 meters minimum distance
                    this);
            }
            
            if (locationManager.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locationManager.RequestLocationUpdates(
                    LocationManager.NetworkProvider,
                    15000, // 15 seconds
                    10,    // 10 meters
                    this);
            }
            
            isTracking = true;
            IsRunning = true;
            
            General.LogOfProgram?.Event("GpsTrackingService - Started background GPS tracking with WakeLock");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - StartTracking", ex);
        }
    }
    
    private void StopTracking()
    {
        if (!isTracking) return;
        
        try
        {
            locationManager?.RemoveUpdates(this);
            isTracking = false;
            IsRunning = false;
            
            // Release wake lock
            if (wakeLock?.IsHeld == true)
            {
                wakeLock.Release();
                General.LogOfProgram?.Event("GpsTrackingService - WakeLock released");
            }
            
            // Clear tracking state
            SaveTrackingState(false, null);
            
            StopForeground(StopForegroundFlags.Remove);
            
            General.LogOfProgram?.Event($"GpsTrackingService - Stopped. Recorded {positionsQueue.Count} positions");
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
            var positionData = new GpsPositionData
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                Altitude = location.HasAltitude ? location.Altitude : null,
                Accuracy = location.HasAccuracy ? location.Accuracy : null,
                Speed = location.HasSpeed ? location.Speed : null,
                Timestamp = DateTime.Now
            };
            
            // Store in memory queue
            positionsQueue.Enqueue(positionData);
            positionsSinceLastSave++;
            
            // Log position recording
            General.LogOfProgram?.Event($"GpsTrackingService - OnLocationChanged: Lat={positionData.Latitude:F6}, Lon={positionData.Longitude:F6}, QueueCount={positionsQueue.Count}");
            
            // Periodically save to file
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
            var json = JsonSerializer.Serialize(positions);
            var filePath = Path.Combine(FilesDir.AbsolutePath, POSITIONS_FILE);
            File.WriteAllText(filePath, json);
            
            General.LogOfProgram?.Event($"GpsTrackingService - Saved {positions.Count} positions to file");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("GpsTrackingService - SavePositionsToFile", ex);
        }
    }
    
    private void LoadPositionsFromFile()
    {
        try
        {
            var filePath = Path.Combine(FilesDir.AbsolutePath, POSITIONS_FILE);
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var positions = JsonSerializer.Deserialize<List<GpsPositionData>>(json);
                
                if (positions != null)
                {
                    while (positionsQueue.TryDequeue(out _)) { } // Clear existing
                    foreach (var pos in positions)
                    {
                        positionsQueue.Enqueue(pos);
                    }
                    
                    General.LogOfProgram?.Event($"GpsTrackingService - Loaded {positions.Count} positions from file");
                }
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
            .AddAction(0, "Stop Tracking", stopPendingIntent)
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
