using gamon;

namespace GlucoMan
{
    /// <summary>
    /// Represents a GPS position recorded during physical activity tracking.
    /// </summary>
    public class GpsPosition : Event
    {
        /// <summary>
        /// Unique identifier for the GPS position record in database.
        /// </summary>
        public int? IdPosition { get; set; }

        /// <summary>
        /// Latitude coordinate in decimal degrees (WGS84).
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Longitude coordinate in decimal degrees (WGS84).
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Altitude in meters above sea level (if available).
        /// </summary>
        public double? Altitude { get; set; }

        /// <summary>
        /// Horizontal accuracy of the position in meters.
        /// </summary>
        public double? Accuracy { get; set; }

        /// <summary>
        /// Speed at the time of recording in meters per second.
        /// </summary>
        public double? Speed { get; set; }

        /// <summary>
        /// ID of the track this position belongs to.
        /// </summary>
        public int? IdTrack { get; set; }

        public GpsPosition()
        {
            EventTime = new DateTimeAndText();
        }

        public override string ToString()
        {
            if (EventTime?.DateTime == null)
                return "No timestamp";
            
            return $"{EventTime.Text} - Lat: {Latitude:F6}, Lon: {Longitude:F6}";
        }
    }
}
