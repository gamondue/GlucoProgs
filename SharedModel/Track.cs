using gamon;
using System.Collections.Generic;

namespace GlucoMan
{
    /// <summary>
    /// Represents a GPS track composed of multiple positions recorded during physical activity.
    /// </summary>
    public class Track : Event
    {
        /// <summary>
        /// Unique identifier for the track in database.
        /// </summary>
        public int? IdTrack { get; set; }

        /// <summary>
        /// Name or description of the track.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Start time of the track recording.
        /// </summary>
        public DateTimeAndText StartTime { get; set; }

        /// <summary>
        /// End time of the track recording.
        /// </summary>
        public DateTimeAndText EndTime { get; set; }

        /// <summary>
        /// Total distance covered in meters.
        /// </summary>
        public double? TotalDistanceMeters { get; set; }

        /// <summary>
        /// Total duration of the track in seconds.
        /// </summary>
        public double? DurationSeconds { get; set; }

        /// <summary>
        /// IntegralAverage speed during the track in meters per second.
        /// </summary>
        public double? AverageSpeedMps { get; set; }

        /// <summary>
        /// ID of the related physical activity (if any).
        /// </summary>
        public int? IdActivity { get; set; }

        /// <summary>
        /// List of GPS positions that compose this track.
        /// </summary>
        public List<GpsPosition> Positions { get; set; }

        /// <summary>
        /// Indicates if the track is currently being recorded.
        /// </summary>
        public bool IsRecording { get; set; }

        public Track()
        {
            EventTime = new DateTimeAndText();
            StartTime = new DateTimeAndText();
            EndTime = new DateTimeAndText();
            Positions = new List<GpsPosition>();
            IsRecording = false;
        }

        /// <summary>
        /// Adds a new position to the track and updates statistics.
        /// </summary>
        /// <param name="position">The GPS position to add.</param>
        public void AddPosition(GpsPosition position)
        {
            if (position == null) return;

            position.IdTrack = this.IdTrack;
            Positions.Add(position);

            // Update start time if this is the first position
            if (Positions.Count == 1)
            {
                StartTime.DateTime = position.EventTime?.DateTime;
            }

            // Always update end time to the latest position
            EndTime.DateTime = position.EventTime?.DateTime;

            // Recalculate statistics
            UpdateStatistics();
        }

        /// <summary>
        /// Calculates and updates track statistics (distance, duration, average speed).
        /// </summary>
        public void UpdateStatistics()
        {
            if (Positions == null || Positions.Count < 2) return;

            double totalDistance = 0;

            for (int i = 1; i < Positions.Count; i++)
            {
                var prevPos = Positions[i - 1];
                var currPos = Positions[i];

                if (prevPos.Latitude.HasValue && prevPos.Longitude.HasValue &&
                    currPos.Latitude.HasValue && currPos.Longitude.HasValue)
                {
                    totalDistance += CalculateDistanceMeters(
                        prevPos.Latitude.Value, prevPos.Longitude.Value,
                        currPos.Latitude.Value, currPos.Longitude.Value);
                }
            }

            TotalDistanceMeters = totalDistance;

            // Calculate duration
            if (StartTime?.DateTime != null && EndTime?.DateTime != null)
            {
                DurationSeconds = (EndTime.DateTime.Value - StartTime.DateTime.Value).TotalSeconds;

                // Calculate average speed
                if (DurationSeconds > 0)
                {
                    AverageSpeedMps = TotalDistanceMeters / DurationSeconds;
                }
            }
        }

        /// <summary>
        /// Calculates the distance between two GPS coordinates using the Haversine formula.
        /// </summary>
        /// <param name="lat1">Latitude of point 1</param>
        /// <param name="lon1">Longitude of point 1</param>
        /// <param name="lat2">Latitude of point 2</param>
        /// <param name="lon2">Longitude of point 2</param>
        /// <returns>Distance in meters</returns>
        private double CalculateDistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusMeters = 6371000;

            double dLat = DegreesToRadians(lat2 - lat1);
            double dLon = DegreesToRadians(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return EarthRadiusMeters * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public override string ToString()
        {
            string duration = DurationSeconds.HasValue 
                ? TimeSpan.FromSeconds(DurationSeconds.Value).ToString(@"hh\:mm\:ss") 
                : "N/A";
            
            string distance = TotalDistanceMeters.HasValue 
                ? $"{TotalDistanceMeters.Value:F0} m" 
                : "N/A";

            return $"{Name ?? "Track"} - Duration: {duration}, Distance: {distance}, Points: {Positions?.Count ?? 0}";
        }
    }
}
