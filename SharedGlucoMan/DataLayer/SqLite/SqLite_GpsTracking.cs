using gamon;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        #region GPS Tracks

        internal override int? SaveTrack(Track track)
        {
            try
            {
                if (track.IdTrack == null || track.IdTrack == 0)
                {
                    track.IdTrack = GetTableNextPrimaryKey("GpsTracks", "IdTrack");
                    InsertTrack(track);
                }
                else
                {
                    UpdateTrack(track);
                }
                return track.IdTrack;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | SaveTrack", ex);
                return null;
            }
        }

        private void InsertTrack(Track track)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO GpsTracks 
                        (IdTrack, Name, StartTime, EndTime, TotalDistanceMeters, DurationSeconds, AverageSpeedMps, IdActivity, Notes)
                        VALUES (@IdTrack, @Name, @StartTime, @EndTime, @TotalDistanceMeters, @DurationSeconds, @AverageSpeedMps, @IdActivity, @Notes);";

                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", track.IdTrack ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Name", track.Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@StartTime", track.StartTime?.DateTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@EndTime", track.EndTime?.DateTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@TotalDistanceMeters", track.TotalDistanceMeters ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@DurationSeconds", track.DurationSeconds ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@AverageSpeedMps", track.AverageSpeedMps ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@IdActivity", track.IdActivity ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Notes", track.Notes ?? (object)DBNull.Value));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | InsertTrack", ex);
            }
        }

        private void UpdateTrack(Track track)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        UPDATE GpsTracks SET 
                            Name = @Name,
                            StartTime = @StartTime,
                            EndTime = @EndTime,
                            TotalDistanceMeters = @TotalDistanceMeters,
                            DurationSeconds = @DurationSeconds,
                            AverageSpeedMps = @AverageSpeedMps,
                            IdActivity = @IdActivity,
                            Notes = @Notes
                        WHERE IdTrack = @IdTrack;";

                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", track.IdTrack ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Name", track.Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@StartTime", track.StartTime?.DateTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@EndTime", track.EndTime?.DateTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@TotalDistanceMeters", track.TotalDistanceMeters ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@DurationSeconds", track.DurationSeconds ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@AverageSpeedMps", track.AverageSpeedMps ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@IdActivity", track.IdActivity ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Notes", track.Notes ?? (object)DBNull.Value));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | UpdateTrack", ex);
            }
        }

        internal override Track GetOneTrack(int? idTrack)
        {
            Track track = null;
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM GpsTracks WHERE IdTrack = @IdTrack;";
                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", idTrack ?? (object)DBNull.Value));

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            track = GetTrackFromRow(reader);
                            // Load positions for this track
                            track.Positions = GetGpsPositions(track.IdTrack);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | GetOneTrack", ex);
            }
            return track;
        }

        internal override List<Track> GetTracks(DateTime? startTime = null, DateTime? endTime = null)
        {
            List<Track> tracks = new List<Track>();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT * FROM GpsTracks";

                    if (startTime.HasValue && endTime.HasValue)
                    {
                        query += " WHERE StartTime BETWEEN @StartTime AND @EndTime";
                        cmd.Parameters.Add(new SqliteParameter("@StartTime", startTime.Value));
                        cmd.Parameters.Add(new SqliteParameter("@EndTime", endTime.Value));
                    }
                    
                    query += " ORDER BY StartTime DESC;";
                    cmd.CommandText = query;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Track track = GetTrackFromRow(reader);
                            tracks.Add(track);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | GetTracks", ex);
            }
            return tracks;
        }

        internal override void DeleteTrack(int? idTrack)
        {
            try
            {
                // First delete all positions belonging to this track
                DeleteGpsPositions(idTrack);

                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM GpsTracks WHERE IdTrack = @IdTrack;";
                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", idTrack ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | DeleteTrack", ex);
            }
        }

        private Track GetTrackFromRow(DbDataReader row)
        {
            Track track = new Track();
            try
            {
                track.IdTrack = Safe.Int(row["IdTrack"]);
                track.Name = Safe.String(row["Name"]);
                track.StartTime.DateTime = Safe.DateTime(row["StartTime"]);
                track.EndTime.DateTime = Safe.DateTime(row["EndTime"]);
                track.TotalDistanceMeters = Safe.Double(row["TotalDistanceMeters"]);
                track.DurationSeconds = Safe.Double(row["DurationSeconds"]);
                track.AverageSpeedMps = Safe.Double(row["AverageSpeedMps"]);
                track.IdActivity = Safe.Int(row["IdActivity"]);
                track.Notes = Safe.String(row["Notes"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | GetTrackFromRow", ex);
            }
            return track;
        }

        #endregion

        #region GPS Positions

        internal override int? SaveGpsPosition(GpsPosition position)
        {
            try
            {
                if (position.IdPosition == null || position.IdPosition == 0)
                {
                    position.IdPosition = GetTableNextPrimaryKey("GpsPositions", "IdPosition");
                    InsertGpsPositionInternal(position);
                }
                else
                {
                    UpdateGpsPositionInternal(position);
                }
                return position.IdPosition;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | SaveGpsPosition", ex);
                return null;
            }
        }

        private void InsertGpsPositionInternal(GpsPosition position)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        INSERT INTO GpsPositions 
                        (IdPosition, IdTrack, Timestamp, Latitude, Longitude, Altitude, Accuracy, Speed, Notes)
                        VALUES (@IdPosition, @IdTrack, @Timestamp, @Latitude, @Longitude, @Altitude, @Accuracy, @Speed, @Notes);";

                    cmd.Parameters.Add(new SqliteParameter("@IdPosition", position.IdPosition ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", position.IdTrack ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Timestamp", position.EventTime?.DateTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Latitude", position.Latitude ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Longitude", position.Longitude ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Altitude", position.Altitude ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Accuracy", position.Accuracy ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Speed", position.Speed ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Notes", position.Notes ?? (object)DBNull.Value));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | InsertGpsPositionInternal", ex);
            }
        }

        private void UpdateGpsPositionInternal(GpsPosition position)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = @"
                        UPDATE GpsPositions SET 
                            IdTrack = @IdTrack,
                            Timestamp = @Timestamp,
                            Latitude = @Latitude,
                            Longitude = @Longitude,
                            Altitude = @Altitude,
                            Accuracy = @Accuracy,
                            Speed = @Speed,
                            Notes = @Notes
                        WHERE IdPosition = @IdPosition;";

                    cmd.Parameters.Add(new SqliteParameter("@IdPosition", position.IdPosition ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", position.IdTrack ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Timestamp", position.EventTime?.DateTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Latitude", position.Latitude ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Longitude", position.Longitude ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Altitude", position.Altitude ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Accuracy", position.Accuracy ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Speed", position.Speed ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Notes", position.Notes ?? (object)DBNull.Value));

                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | UpdateGpsPositionInternal", ex);
            }
        }

        internal override void UpdateGpsPosition(GpsPosition position)
        {
            UpdateGpsPositionInternal(position);
        }

        internal override void SaveGpsPositions(List<GpsPosition> positions)
        {
            if (positions == null || positions.Count == 0) return;

            int currentKey = GetTableNextPrimaryKey("GpsPositions", "IdPosition");
            try
            {
                using (DbConnection conn = Connect())
                {
                    using (var tran = conn.BeginTransaction())
                    using (DbCommand cmd = conn.CreateCommand())
                    {
                        cmd.Transaction = tran;
                        cmd.CommandText = @"
                            INSERT INTO GpsPositions 
                            (IdPosition, IdTrack, Timestamp, Latitude, Longitude, Altitude, Accuracy, Speed, Notes)
                            VALUES (@id, @track, @ts, @lat, @lon, @alt, @acc, @speed, @notes);";

                        var pId = cmd.CreateParameter(); pId.ParameterName = "@id"; pId.DbType = DbType.Int32; cmd.Parameters.Add(pId);
                        var pTrack = cmd.CreateParameter(); pTrack.ParameterName = "@track"; pTrack.DbType = DbType.Int32; cmd.Parameters.Add(pTrack);
                        var pTs = cmd.CreateParameter(); pTs.ParameterName = "@ts"; pTs.DbType = DbType.DateTime; cmd.Parameters.Add(pTs);
                        var pLat = cmd.CreateParameter(); pLat.ParameterName = "@lat"; pLat.DbType = DbType.Double; cmd.Parameters.Add(pLat);
                        var pLon = cmd.CreateParameter(); pLon.ParameterName = "@lon"; pLon.DbType = DbType.Double; cmd.Parameters.Add(pLon);
                        var pAlt = cmd.CreateParameter(); pAlt.ParameterName = "@alt"; pAlt.DbType = DbType.Double; cmd.Parameters.Add(pAlt);
                        var pAcc = cmd.CreateParameter(); pAcc.ParameterName = "@acc"; pAcc.DbType = DbType.Double; cmd.Parameters.Add(pAcc);
                        var pSpeed = cmd.CreateParameter(); pSpeed.ParameterName = "@speed"; pSpeed.DbType = DbType.Double; cmd.Parameters.Add(pSpeed);
                        var pNotes = cmd.CreateParameter(); pNotes.ParameterName = "@notes"; pNotes.DbType = DbType.String; cmd.Parameters.Add(pNotes);

                        try { cmd.Prepare(); } catch { /* ignore */ }

                        foreach (var pos in positions)
                        {
                            pId.Value = currentKey;
                            pTrack.Value = pos?.IdTrack ?? (object)DBNull.Value;
                            pTs.Value = pos?.EventTime?.DateTime ?? (object)DBNull.Value;
                            pLat.Value = pos?.Latitude ?? (object)DBNull.Value;
                            pLon.Value = pos?.Longitude ?? (object)DBNull.Value;
                            pAlt.Value = pos?.Altitude ?? (object)DBNull.Value;
                            pAcc.Value = pos?.Accuracy ?? (object)DBNull.Value;
                            pSpeed.Value = pos?.Speed ?? (object)DBNull.Value;
                            pNotes.Value = string.IsNullOrEmpty(pos?.Notes) ? (object)DBNull.Value : pos.Notes;

                            cmd.ExecuteNonQuery();
                            pos.IdPosition = currentKey;
                            currentKey++;
                        }

                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | SaveGpsPositions", ex);
            }
        }

        internal override List<GpsPosition> GetGpsPositions(int? idTrack)
        {
            List<GpsPosition> positions = new List<GpsPosition>();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM GpsPositions WHERE IdTrack = @IdTrack ORDER BY Timestamp ASC;";
                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", idTrack ?? (object)DBNull.Value));

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GpsPosition position = GetGpsPositionFromRow(reader);
                            positions.Add(position);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | GetGpsPositions", ex);
            }
            return positions;
        }

        internal override void DeleteGpsPositions(int? idTrack)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM GpsPositions WHERE IdTrack = @IdTrack;";
                    cmd.Parameters.Add(new SqliteParameter("@IdTrack", idTrack ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | DeleteGpsPositions", ex);
            }
        }

        internal override int? InsertGpsPosition(GpsPosition position)
        {
            try
            {
                if (position.IdPosition == null || position.IdPosition == 0)
                {
                    position.IdPosition = GetTableNextPrimaryKey("GpsPositions", "IdPosition");
                }

                InsertGpsPositionInternal(position);
                return position.IdPosition;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | InsertGpsPosition", ex);
                return null;
            }
        }

        internal override List<GpsPosition> GetGpsPositionsForTrack(int? idTrack)
        {
            // Alias for GetGpsPositions to match DataLayer interface
            return GetGpsPositions(idTrack);
        }

        internal override void DeleteGpsPositionsForTrack(int? idTrack)
        {
            // Alias for DeleteGpsPositions to match DataLayer interface
            DeleteGpsPositions(idTrack);
        }

        private GpsPosition GetGpsPositionFromRow(DbDataReader row)
        {
            GpsPosition position = new GpsPosition();
            try
            {
                position.IdPosition = Safe.Int(row["IdPosition"]);
                position.IdTrack = Safe.Int(row["IdTrack"]);
                position.EventTime.DateTime = Safe.DateTime(row["Timestamp"]);
                position.Latitude = Safe.Double(row["Latitude"]);
                position.Longitude = Safe.Double(row["Longitude"]);
                position.Altitude = Safe.Double(row["Altitude"]);
                position.Accuracy = Safe.Double(row["Accuracy"]);
                position.Speed = Safe.Double(row["Speed"]);
                position.Notes = Safe.String(row["Notes"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_GpsTracking | GetGpsPositionFromRow", ex);
            }
            return position;
        }

        #endregion
    }
}
