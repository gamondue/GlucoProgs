using gamon;

namespace GlucoMan
{
    /// <summary>
    /// Business Layer for GPS Track management.
    /// Handles track creation, position recording, and statistics calculation.
    /// </summary>
    public class BL_GpsTracking
    {
        DataLayer dl = DatabaseService.Instance.Database;

        /// <summary>
        /// Current track being recorded.
        /// </summary>
        public Track CurrentTrack { get; set; }

        /// <summary>
        /// List of all tracks.
        /// </summary>
        public List<Track> Tracks { get; set; }

        public BL_GpsTracking()
        {
            CurrentTrack = new Track();
            Tracks = new List<Track>();
        }

        #region Track Operations

        /// <summary>
        /// Starts a new track recording session.
        /// </summary>
        /// <param name="name">Name of the track</param>
        /// <returns>The newly created track</returns>
        public Track StartNewTrack(string name = null)
        {
            try
            {
                CurrentTrack = new Track
                {
                    Name = name ?? $"Track {DateTime.Now:yyyy-MM-dd HH:mm}",
                    IsRecording = true
                };
                CurrentTrack.StartTime.DateTime = DateTime.Now;
                CurrentTrack.EventTime.DateTime = DateTime.Now;

                // Save to get an ID
                CurrentTrack.IdTrack = dl.SaveTrack(CurrentTrack);

                General.LogOfProgram?.Event($"Started new track: {CurrentTrack.Name}");
                return CurrentTrack;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - StartNewTrack", ex);
                return null;
            }
        }

        /// <summary>
        /// Stops the current track recording and saves final statistics.
        /// </summary>
        /// <returns>The completed track</returns>
        public Track StopCurrentTrack()
        {
            try
            {
                if (CurrentTrack == null || !CurrentTrack.IsRecording)
                    return CurrentTrack;

                CurrentTrack.IsRecording = false;
                CurrentTrack.EndTime.DateTime = DateTime.Now;
                CurrentTrack.UpdateStatistics();

                // Save final track Data
                SaveTrack(CurrentTrack);

                General.LogOfProgram?.Event($"Stopped track: {CurrentTrack.Name}, Distance: {CurrentTrack.TotalDistanceMeters:F0}m");
                return CurrentTrack;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - StopCurrentTrack", ex);
                return CurrentTrack;
            }
        }

        /// <summary>
        /// Saves a track to the database.
        /// </summary>
        public int? SaveTrack(Track track)
        {
            try
            {
                if (track == null)
                    return null;

                // Save to database
                var trackId = dl.SaveTrack(track);

                // Also export to GPX file in Tracks folder
                if (trackId.HasValue && track.Positions != null && track.Positions.Count > 0)
                {
                    try
                    {
                        ExportTrackToGpxFile(track);
                        General.LogOfProgram?.Event($"Track {trackId} saved to database and exported to GPX file");
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error($"Error exporting track {trackId} to GPX file", ex);
                        // Don't fail the save operation if GPX export fails
                    }
                }

                return trackId;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - SaveTrack", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets a track by ID with all its positions.
        /// </summary>
        public Track GetOneTrack(int? idTrack)
        {
            try
            {
                if (!idTrack.HasValue)
                    return null;

                return dl.GetOneTrack(idTrack);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - GetOneTrack", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets all tracks within a date range.
        /// </summary>
        public List<Track> GetTracks(DateTime? startTime = null, DateTime? endTime = null)
        {
            try
            {
                Tracks = dl.GetTracks(startTime, endTime);
                return Tracks;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - GetTracks", ex);
                return new List<Track>();
            }
        }

        /// <summary>
        /// Deletes a track and all its positions.
        /// </summary>
        public void DeleteTrack(int? idTrack)
        {
            try
            {
                if (!idTrack.HasValue)
                    return;

                dl.DeleteTrack(idTrack);
                General.LogOfProgram?.Event($"Deleted track ID: {idTrack}");
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - DeleteTrack", ex);
            }
        }

        #endregion

        #region Position Operations

        /// <summary>
        /// Adds a new GPS position to the current track.
        /// </summary>
        /// <param name="latitude">Latitude in decimal degrees</param>
        /// <param name="longitude">Longitude in decimal degrees</param>
        /// <param name="altitude">Altitude in meters (optional)</param>
        /// <param name="accuracy">Horizontal accuracy in meters (optional)</param>
        /// <param name="speed">Speed in m/s (optional)</param>
        /// <returns>The created position</returns>
        public GpsPosition AddPosition(double latitude, double longitude, 
            double? altitude = null, double? accuracy = null, double? speed = null)
        {
            try
            {
                if (CurrentTrack == null)
                {
                    General.LogOfProgram?.Event("BL_GpsTracking - AddPosition: No active track, creating new one");
                    StartNewTrack();
                }

                // Allow adding positions even if not recording (for syncing saved positions)
                // if (CurrentTrack == null || !CurrentTrack.IsRecording)
                // {
                //     General.LogOfProgram?.Event("BL_GpsTracking - AddPosition: No active track");
                //     return null;
                // }

                var position = new GpsPosition
                {
                    IdTrack = CurrentTrack.IdTrack,
                    Latitude = latitude,
                    Longitude = longitude,
                    Altitude = altitude,
                    Accuracy = accuracy,
                    Speed = speed
                };
                position.EventTime.DateTime = DateTime.Now;

                // Save position to database
                position.IdPosition = dl.SaveGpsPosition(position);

                // Add to current track's positions list
                CurrentTrack.AddPosition(position);

                return position;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - AddPosition", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets all positions for a specific track.
        /// </summary>
        public List<GpsPosition> GetPositions(int? idTrack)
        {
            try
            {
                if (!idTrack.HasValue)
                    return new List<GpsPosition>();

                return dl.GetGpsPositions(idTrack);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - GetPositions", ex);
                return new List<GpsPosition>();
            }
        }

        /// <summary>
        /// Saves a single GPS position.
        /// </summary>
        public int? SavePosition(GpsPosition position)
        {
            try
            {
                if (position == null)
                    return null;

                return dl.SaveGpsPosition(position);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - SavePosition", ex);
                return null;
            }
        }

        /// <summary>
        /// Bulk saves multiple positions for efficiency.
        /// </summary>
        public void SavePositions(List<GpsPosition> positions)
        {
            try
            {
                if (positions == null || positions.Count == 0)
                    return;

                dl.SaveGpsPositions(positions);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - SavePositions", ex);
            }
        }

        #endregion

        #region Statistics and Utilities

        /// <summary>
        /// Generates a JSON string of positions suitable for Google Maps JavaScript API.
        /// </summary>
        /// <param name="positions">List of positions</param>
        /// <returns>JSON array of coordinates</returns>
        public string GetPositionsAsJson(List<GpsPosition> positions)
        {
            if (positions == null || positions.Count == 0)
                return "[]";

            var coords = positions
                .Where(p => p.Latitude.HasValue && p.Longitude.HasValue)
                .Select(p => $"{{lat: {p.Latitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}, lng: {p.Longitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}}}")
                .ToList();

            return "[" + string.Join(",", coords) + "]";
        }

        /// <summary>
        /// Gets formatted duration string.
        /// </summary>
        public string GetFormattedDuration(Track track)
        {
            if (track?.DurationSeconds == null)
                return "N/A";

            return TimeSpan.FromSeconds(track.DurationSeconds.Value).ToString(@"hh\:mm\:ss");
        }

        /// <summary>
        /// Gets formatted distance string.
        /// </summary>
        public string GetFormattedDistance(Track track)
        {
            if (track?.TotalDistanceMeters == null)
                return "N/A";

            if (track.TotalDistanceMeters >= 1000)
                return $"{track.TotalDistanceMeters / 1000:F2} km";
            
            return $"{track.TotalDistanceMeters:F0} m";
        }

        /// <summary>
        /// Gets formatted average speed string.
        /// </summary>
        public string GetFormattedSpeed(Track track)
        {
            if (track?.AverageSpeedMps == null)
                return "N/A";

            // Convert m/s to km/h
            double kmh = track.AverageSpeedMps.Value * 3.6;
            return $"{kmh:F1} km/h";
        }

        #endregion

        #region Track File Export

        /// <summary>
        /// Exports a track to GPX format and saves it to the Tracks folder.
        /// </summary>
        /// <param name="track">Track to export</param>
        /// <returns>Path to the exported file, or null if export failed</returns>
        public string ExportTrackToGpxFile(Track track)
        {
            try
            {
                if (track == null || track.Positions == null || track.Positions.Count == 0)
                {
                    General.LogOfProgram?.Error("ExportTrackToGpxFile - Track is null or has no positions", null);
                    return null;
                }

                // Create Tracks folder if it doesn't exist
                string tracksFolder = GetTracksFolder();
                Directory.CreateDirectory(tracksFolder);

                // Generate filename
                string safeTrackName = GetSafeFileName(track.Name ?? $"Track_{track.IdTrack}");
                string fileName = $"{safeTrackName}_{track.StartTime.DateTime:yyyyMMdd_HHmmss}.gpx";
                string filePath = Path.Combine(tracksFolder, fileName);

                // Generate GPX content
                string gpxContent = GenerateGpxContent(track);

                // Save to file
                File.WriteAllText(filePath, gpxContent, System.Text.Encoding.UTF8);

                General.LogOfProgram?.Event($"Track exported to GPX: {filePath}");
                return filePath;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("BL_GpsTracking - ExportTrackToGpxFile", ex);
                return null;
            }
        }

        /// <summary>
        /// Gets the path to the Tracks folder (creates it if it doesn't exist).
        /// </summary>
        public static string GetTracksFolder()
        {
#if ANDROID || IOS
            string tracksFolder = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "Tracks");
#else
            // For Windows, use a Tracks folder in the app data directory
            string tracksFolder = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "Tracks");
#endif
            return tracksFolder;
        }

        /// <summary>
        /// Generates GPX XML content for a track.
        /// </summary>
        private string GenerateGpxContent(Track track)
        {
            var sb = new System.Text.StringBuilder();
            
            // GPX header
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<gpx version=\"1.1\" creator=\"GlucoMan\" xmlns=\"http://www.topografix.com/GPX/1/1\">");
            
            // Metadata
            sb.AppendLine("  <metadata>");
            sb.AppendLine($"    <name>{System.Security.SecurityElement.Escape(track.Name ?? "Unnamed Track")}</name>");
            sb.AppendLine($"    <time>{track.StartTime.DateTime:yyyy-MM-ddTHH:mm:ssZ}</time>");
            sb.AppendLine("  </metadata>");
            
            // Track
            sb.AppendLine("  <trk>");
            sb.AppendLine($"    <name>{System.Security.SecurityElement.Escape(track.Name ?? "Unnamed Track")}</name>");
            sb.AppendLine("    <trkseg>");

            // Track points
            foreach (var pos in track.Positions.OrderBy(p => p.EventTime.DateTime))
            {
                if (pos.Latitude.HasValue && pos.Longitude.HasValue)
                {
                    sb.Append($"      <trkpt lat=\"{pos.Latitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}\" ");
                    sb.AppendLine($"lon=\"{pos.Longitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}\">");
                    
                    if (pos.Altitude.HasValue)
                    {
                        sb.AppendLine($"        <ele>{pos.Altitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}</ele>");
                    }
                    
                    sb.AppendLine($"        <time>{pos.EventTime.DateTime:yyyy-MM-ddTHH:mm:ssZ}</time>");
                    
                    if (pos.Speed.HasValue)
                    {
                        sb.AppendLine($"        <speed>{pos.Speed.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)}</speed>");
                    }
                    
                    sb.AppendLine("      </trkpt>");
                }
            }

            sb.AppendLine("    </trkseg>");
            sb.AppendLine("  </trk>");
            sb.AppendLine("</gpx>");

            return sb.ToString();
        }

        /// <summary>
        /// Makes a filename safe by removing invalid characters.
        /// </summary>
        private string GetSafeFileName(string fileName)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        #endregion
    }
}
