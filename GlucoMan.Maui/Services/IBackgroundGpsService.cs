namespace GlucoMan.Maui.Services;

/// <summary>
/// Cross-platform interface for background GPS tracking service.
/// Android implementation uses a foreground service.
/// Windows/other platforms use in-app tracking only.
/// </summary>
public interface IBackgroundGpsService
{
    /// <summary>
    /// Indicates whether background tracking is currently active
    /// </summary>
    bool IsTracking { get; }
    
    /// <summary>
    /// Starts background GPS tracking
    /// </summary>
    /// <returns>True if started successfully</returns>
    Task<bool> StartTrackingAsync();
    
    /// <summary>
    /// Stops background GPS tracking
    /// </summary>
    Task StopTrackingAsync();
    
    /// <summary>
    /// Gets all recorded positions since tracking started
    /// </summary>
    List<GpsPositionRecord> GetRecordedPositions();
    
    /// <summary>
    /// Gets and clears all recorded positions (for saving to database)
    /// </summary>
    List<GpsPositionRecord> GetAndClearPositions();
    
    /// <summary>
    /// Gets the number of recorded positions
    /// </summary>
    int GetPositionsCount();
    
    /// <summary>
    /// Clears all recorded positions
    /// </summary>
    void ClearPositions();
    
    /// <summary>
    /// Gets the tracking start time
    /// </summary>
    DateTime? TrackingStartTime { get; }
    
    /// <summary>
    /// Event fired when a new position is recorded
    /// </summary>
    event EventHandler<GpsPositionRecord> OnPositionRecorded;
}

/// <summary>
/// Lightweight GPS position record for cross-platform use
/// </summary>
public class GpsPositionRecord
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public float? Accuracy { get; set; }
    public float? Speed { get; set; }
    public DateTime Timestamp { get; set; }

    // ── Timezone info resolved from coordinates ──────────────────────────
    /// <summary>IANA timezone id at this position, e.g. "Europe/Rome".</summary>
    public string IanaTimeZoneId { get; set; }

    /// <summary>UTC offset in hours at this position and time (includes DST). May be fractional.</summary>
    public double? UtcOffsetHours { get; set; }

    /// <summary>True when Daylight Saving Time was in effect at this position and time.</summary>
    public bool? IsDaylightSavingTime { get; set; }
}
