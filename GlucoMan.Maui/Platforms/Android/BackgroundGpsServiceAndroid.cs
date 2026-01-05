 using Android.Content;
using GlucoMan.Maui.Services;
using gamon;
using AndroidOS = Android.OS;

namespace GlucoMan.Maui.Platforms.Android;

/// <summary>
/// Android implementation of IBackgroundGpsService.
/// Uses GpsTrackingService foreground service for background location tracking.
/// Reconnects to existing service if app was closed and service is still running.
/// </summary>
public class BackgroundGpsServiceAndroid : IBackgroundGpsService
{
    private const string PREFS_NAME = "GpsTrackingPrefs";
    private const string KEY_IS_TRACKING = "IsTracking";
    private const string KEY_START_TIME = "StartTime";
    
    private bool? cachedIsTracking = null;
    private DateTime? cachedTrackingStartTime = null;
    
    public bool IsTracking
    {
        get
        {
            // If service static property says it's running, use that
            if (GpsTrackingService.IsRunning)
                return true;
            
            // Otherwise check SharedPreferences (in case app was restarted but service is still running)
            if (!cachedIsTracking.HasValue)
            {
                var context = Platform.AppContext;
                var prefs = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
                cachedIsTracking = prefs.GetBoolean(KEY_IS_TRACKING, false);
                
                General.LogOfProgram?.Event($"BackgroundGpsServiceAndroid - Read IsTracking from prefs: {cachedIsTracking.Value}");
            }
            
            return cachedIsTracking.Value;
        }
    }
    
    public DateTime? TrackingStartTime
    {
        get
        {
            // If service has the value, use it
            if (GpsTrackingService.TrackingStartTime.HasValue)
                return GpsTrackingService.TrackingStartTime;
            
            // Otherwise check SharedPreferences
            if (!cachedTrackingStartTime.HasValue && IsTracking)
            {
                var context = Platform.AppContext;
                var prefs = context.GetSharedPreferences(PREFS_NAME, FileCreationMode.Private);
                long startTimeTicks = prefs.GetLong(KEY_START_TIME, 0);
                
                if (startTimeTicks > 0)
                {
                    cachedTrackingStartTime = new DateTime(startTimeTicks);
                    General.LogOfProgram?.Event($"BackgroundGpsServiceAndroid - Read TrackingStartTime from prefs: {cachedTrackingStartTime.Value}");
                }
            }
            
            return cachedTrackingStartTime;
        }
    }
    
    public event EventHandler<GpsPositionRecord> OnPositionRecorded;
    
    public BackgroundGpsServiceAndroid()
    {
        // Subscribe to service events
        GpsTrackingService.OnPositionRecorded += HandlePositionRecorded;
    }
    
    private void HandlePositionRecorded(object sender, GpsTrackingService.GpsPositionData e)
    {
        OnPositionRecorded?.Invoke(this, new GpsPositionRecord
        {
            Latitude = e.Latitude,
            Longitude = e.Longitude,
            Altitude = e.Altitude,
            Accuracy = e.Accuracy,
            Speed = e.Speed,
            Timestamp = e.Timestamp
        });
    }
    
    public async Task<bool> StartTrackingAsync()
    {
        try
        {
            // Check and request permissions first
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                
                if (status != PermissionStatus.Granted)
                {
                    General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - Location permission denied", null);
                    return false;
                }
            }
            
            // For Android 10+, also need background location permission
            if (AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.Q)
            {
                var bgStatus = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
                if (bgStatus != PermissionStatus.Granted)
                {
                    General.LogOfProgram?.Event("BackgroundGpsServiceAndroid - Background location permission not granted, will use foreground only");
                }
            }
            
            // For Android 13+, also need POST_NOTIFICATIONS permission
            if (AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.Tiramisu)
            {
                var notifStatus = await Permissions.CheckStatusAsync<Permissions.PostNotifications>();
                if (notifStatus != PermissionStatus.Granted)
                {
                    notifStatus = await Permissions.RequestAsync<Permissions.PostNotifications>();
                }
            }
            
            // Start the foreground service
            var context = Platform.CurrentActivity ?? Platform.AppContext;
            var intent = new Intent(context, typeof(GpsTrackingService));
            intent.SetAction("START_TRACKING");
            
            if (AndroidOS.Build.VERSION.SdkInt >= AndroidOS.BuildVersionCodes.O)
            {
                context.StartForegroundService(intent);
            }
            else
            {
                context.StartService(intent);
            }
            
            // Wait a moment to let the service start
            await Task.Delay(500);
            
            // Update cache
            cachedIsTracking = true;
            cachedTrackingStartTime = DateTime.Now;
            
            General.LogOfProgram?.Event("BackgroundGpsServiceAndroid - Started background tracking service");
            return true;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - StartTrackingAsync", ex);
            return false;
        }
    }
    
    public Task StopTrackingAsync()
    {
        try
        {
            var context = Platform.CurrentActivity ?? Platform.AppContext;
            var intent = new Intent(context, typeof(GpsTrackingService));
            intent.SetAction("STOP_TRACKING");
            context.StartService(intent);
            
            // Clear cache
            cachedIsTracking = false;
            cachedTrackingStartTime = null;
            
            General.LogOfProgram?.Event("BackgroundGpsServiceAndroid - Stopped background tracking service");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - StopTrackingAsync", ex);
        }
        
        return Task.CompletedTask;
    }
    
    public List<GpsPositionRecord> GetRecordedPositions()
    {
        // Try to get from service first
        var positions = GpsTrackingService.PeekAllPositions();
        
        // If service doesn't have Data but we're tracking, try to load from file
        if (positions.Count == 0 && IsTracking)
        {
            General.LogOfProgram?.Event("BackgroundGpsServiceAndroid - Service has no positions in memory, attempting to load from persisted file");
            
            try
            {
                var context = Platform.AppContext;
                var filePath = System.IO.Path.Combine(context.FilesDir.AbsolutePath, "gps_positions_temp.json");
                
                if (System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);
                    var persistedPositions = System.Text.Json.JsonSerializer.Deserialize<List<GpsTrackingService.GpsPositionData>>(json);
                    
                    if (persistedPositions != null && persistedPositions.Count > 0)
                    {
                        General.LogOfProgram?.Event($"BackgroundGpsServiceAndroid - Loaded {persistedPositions.Count} positions from persisted file");
                        
                        return persistedPositions.Select(p => new GpsPositionRecord
                        {
                            Latitude = p.Latitude,
                            Longitude = p.Longitude,
                            Altitude = p.Altitude,
                            Accuracy = p.Accuracy,
                            Speed = p.Speed,
                            Timestamp = p.Timestamp
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - Error loading persisted positions", ex);
            }
        }
        
        return positions.Select(p => new GpsPositionRecord
        {
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            Altitude = p.Altitude,
            Accuracy = p.Accuracy,
            Speed = p.Speed,
            Timestamp = p.Timestamp
        }).ToList();
    }
    
    public List<GpsPositionRecord> GetAndClearPositions()
    {
        // First try to get from memory
        var positions = GpsTrackingService.GetAndClearPositions();
        
        // If no positions in memory but we're tracking, load from file
        if (positions.Count == 0 && IsTracking)
        {
            General.LogOfProgram?.Event("BackgroundGpsServiceAndroid - GetAndClearPositions: loading from file");
            
            try
            {
                var context = Platform.AppContext;
                var filePath = System.IO.Path.Combine(context.FilesDir.AbsolutePath, "gps_positions_temp.json");
                
                if (System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);
                    var persistedPositions = System.Text.Json.JsonSerializer.Deserialize<List<GpsTrackingService.GpsPositionData>>(json);
                    
                    if (persistedPositions != null)
                    {
                        positions = persistedPositions;
                        General.LogOfProgram?.Event($"BackgroundGpsServiceAndroid - Loaded and clearing {positions.Count} positions from file");
                        
                        // Delete file after reading
                        System.IO.File.Delete(filePath);
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - Error in GetAndClearPositions", ex);
            }
        }
        
        return positions.Select(p => new GpsPositionRecord
        {
            Latitude = p.Latitude,
            Longitude = p.Longitude,
            Altitude = p.Altitude,
            Accuracy = p.Accuracy,
            Speed = p.Speed,
            Timestamp = p.Timestamp
        }).ToList();
    }
    
    public int GetPositionsCount()
    {
        // Try memory first
        int count = GpsTrackingService.GetPositionsCount();
        
        // If zero but we're tracking, check file
        if (count == 0 && IsTracking)
        {
            try
            {
                var context = Platform.AppContext;
                var filePath = System.IO.Path.Combine(context.FilesDir.AbsolutePath, "gps_positions_temp.json");
                
                if (System.IO.File.Exists(filePath))
                {
                    var json = System.IO.File.ReadAllText(filePath);
                    var positions = System.Text.Json.JsonSerializer.Deserialize<List<GpsTrackingService.GpsPositionData>>(json);
                    count = positions?.Count ?? 0;
                    
                    General.LogOfProgram?.Event($"BackgroundGpsServiceAndroid - GetPositionsCount from file: {count}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - Error counting persisted positions", ex);
            }
        }
        
        return count;
    }
    
    public void ClearPositions()
    {
        GpsTrackingService.ClearPositions();
        
        // Also delete persisted file
        try
        {
            var context = Platform.AppContext;
            var filePath = System.IO.Path.Combine(context.FilesDir.AbsolutePath, "gps_positions_temp.json");
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
                General.LogOfProgram?.Event("BackgroundGpsServiceAndroid - Cleared persisted positions file");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("BackgroundGpsServiceAndroid - Error clearing persisted file", ex);
        }
    }
}
