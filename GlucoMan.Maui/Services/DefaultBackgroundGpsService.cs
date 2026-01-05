using GlucoMan.Maui.Services;
using gamon;
using System.Collections.Concurrent;

namespace GlucoMan.Maui.Services;

/// <summary>
/// Default implementation of IBackgroundGpsService for platforms without native background support.
/// Uses in-app timer-based tracking (works only while app is in foreground).
/// </summary>
public class DefaultBackgroundGpsService : IBackgroundGpsService
{
    private bool isTracking = false;
    private DateTime? trackingStartTime = null;
    private System.Timers.Timer gpsTimer;
    private readonly ConcurrentQueue<GpsPositionRecord> positions = new();
    
    public bool IsTracking => isTracking;
    public DateTime? TrackingStartTime => trackingStartTime;
    
    public event EventHandler<GpsPositionRecord> OnPositionRecorded;
    
    public async Task<bool> StartTrackingAsync()
    {
        try
        {
            // Check permissions
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                
                if (status != PermissionStatus.Granted)
                {
                    General.LogOfProgram?.Error("DefaultBackgroundGpsService - Permission denied, cannot start tracking", null);
                    return false;
                }
            }
            
            // Clear previous positions
            while (positions.TryDequeue(out _)) { }
            
            isTracking = true;
            trackingStartTime = DateTime.Now;
            
            // Start timer for periodic location updates
            gpsTimer = new System.Timers.Timer(10000); // 10 seconds
            gpsTimer.Elapsed += async (s, e) => await RecordPosition();
            gpsTimer.AutoReset = true;
            gpsTimer.Start();
            
            // Get first position immediately
            await RecordPosition();
            
            General.LogOfProgram?.Event("DefaultBackgroundGpsService - Started in-app tracking");
            return true;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("DefaultBackgroundGpsService - StartTrackingAsync", ex);
            return false;
        }
    }
    
    public Task StopTrackingAsync()
    {
        try
        {
            isTracking = false;
            
            if (gpsTimer != null)
            {
                gpsTimer.Stop();
                gpsTimer.Dispose();
                gpsTimer = null;
            }
            
            General.LogOfProgram?.Event($"DefaultBackgroundGpsService - Stopped. Recorded {positions.Count} positions");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("DefaultBackgroundGpsService - StopTrackingAsync", ex);
        }
        
        return Task.CompletedTask;
    }
    
    private async Task RecordPosition()
    {
        if (!isTracking) return;
        
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request);
            
            if (location != null)
            {
                var record = new GpsPositionRecord
                {
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    Altitude = location.Altitude,
                    Accuracy = (float?)location.Accuracy,
                    Speed = (float?)location.Speed,
                    Timestamp = DateTime.Now
                };
                
                positions.Enqueue(record);
                
                // Notify on main thread
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    OnPositionRecorded?.Invoke(this, record);
                });
            }
            else
            {
                General.LogOfProgram?.Error("DefaultBackgroundGpsService - RecordPosition: location is NULL", null);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("DefaultBackgroundGpsService - RecordPosition", ex);
        }
    }
    
    public List<GpsPositionRecord> GetRecordedPositions()
    {
        return positions.ToList();
    }
    
    public List<GpsPositionRecord> GetAndClearPositions()
    {
        var result = new List<GpsPositionRecord>();
        while (positions.TryDequeue(out var pos))
        {
            result.Add(pos);
        }
        return result;
    }
    
    public int GetPositionsCount() => positions.Count;
    
    public void ClearPositions()
    {
        while (positions.TryDequeue(out _)) { }
    }
}
