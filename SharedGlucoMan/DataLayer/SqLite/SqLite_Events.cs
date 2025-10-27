using gamon;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using GlucoMan.BusinessObjects;
using System.Collections.Generic;
using System.Data;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        /// <summary>
        /// Save or update a list of Events in the database
        /// (keeps existing behaviour: use INSERT for new and UPDATE for existing by delegating to SaveOneEvent)
        /// </summary>
        internal override void SaveEvents(List<Event> importedEvents)
        {
            if (importedEvents == null || importedEvents.Count == 0)
                return;

            try
            {
                foreach (var evt in importedEvents)
                {
                    try
                    {
                        SaveOneEvent(evt);
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error($"SqLite_Events - SaveEvents - item failed to save: {evt?.EventTime?.DateTime}", ex);
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Events | SaveEvents", ex);
            }
        }

        /// <summary>
        /// Insert-only bulk operation for Events. Does not perform updates; skips items that already have an IdEvent.
        /// </summary>
        internal override void InsertEvents(List<Event> importedEvents)
        {
            if (importedEvents == null || importedEvents.Count == 0)
                return;

            int currentKey = GetTableNextPrimaryKey("Events", "IdEvent");
            try
            {
                using (DbConnection conn = Connect())
                {
                    using (var tran = conn.BeginTransaction())
                    using (DbCommand insertCmd = conn.CreateCommand())
                    {
                        insertCmd.Transaction = tran;

                        insertCmd.CommandText = "INSERT INTO Events (IdEvent, EventTime, Notes) VALUES (@id,@ts,@notes);";

                        var pIdIns = insertCmd.CreateParameter(); pIdIns.ParameterName = "@id"; pIdIns.DbType = DbType.Int32; insertCmd.Parameters.Add(pIdIns);
                        var pTsIns = insertCmd.CreateParameter(); pTsIns.ParameterName = "@ts"; pTsIns.DbType = DbType.DateTime; insertCmd.Parameters.Add(pTsIns);
                        var pNotesIns = insertCmd.CreateParameter(); pNotesIns.ParameterName = "@notes"; pNotesIns.DbType = DbType.String; insertCmd.Parameters.Add(pNotesIns);

                        try { insertCmd.Prepare(); } catch { }

                        foreach (var evt in importedEvents)
                        {
                            if (evt == null)
                                continue;

                            // Only insert new events (no update here)
                            if (evt.IdEvent == null || evt.IdEvent == 0)
                            {
                                pIdIns.Value = currentKey;
                                pTsIns.Value = evt?.EventTime?.DateTime ?? (object)DBNull.Value;
                                pNotesIns.Value = string.IsNullOrEmpty(evt?.Notes) ? (object)DBNull.Value : evt.Notes;
                                insertCmd.ExecuteNonQuery();
                                evt.IdEvent = currentKey;
                                currentKey++;
                            }
                            else
                            {
                                // skip items that already have an id
                                continue;
                            }
                        }

                        tran.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("SqLite_Events | InsertEvents", ex);
            }
        }

        /// <summary>
        /// Save or update a single Event in the database
        /// </summary>
        private int? SaveOneEvent(Event evt)
        {
            try
            {
                // Check if event has an ID (update) or needs a new one (insert)
                if (evt.IdEvent == null || evt.IdEvent == 0)
                {
                    evt.IdEvent = GetTableNextPrimaryKey("Events", "IdEvent");
                    InsertOneEvent(evt);
                }
                else
                {
                    UpdateOneEvent(evt);
                }

                return evt.IdEvent;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - SaveOneEvent", ex);
                return null;
            }
        }

        /// <summary>
        /// Insert a new Event into the database
        /// </summary>
        private int? InsertOneEvent(Event evt)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Events " +
                "(IdEvent, EventTime, Notes) " +
                    "VALUES (" +
                     SqliteSafe.Int(evt.IdEvent) + "," +
                   SqliteSafe.Date(evt.EventTime?.DateTime) + "," +
                     SqliteSafe.String(evt.Notes) + ");";

                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                return evt.IdEvent;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - InsertOneEvent", ex);
                return null;
            }
        }

        /// <summary>
        /// Update an existing Event in the database
        /// </summary>
        private int? UpdateOneEvent(Event evt)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Events SET " +
                            "EventTime=" + SqliteSafe.Date(evt.EventTime?.DateTime) + "," +   
                            "Notes=" + SqliteSafe.String(evt.Notes) +
                            " WHERE IdEvent=" + SqliteSafe.Int(evt.IdEvent) + ";";

                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                return evt.IdEvent;
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - UpdateOneEvent", ex);
                return null;
            }
        }

        /// <summary>
        /// Get all Events from the database, optionally filtered by date range
        /// </summary>
        internal override List<Event> GetEvents(DateTime? startTime = null, DateTime? endTime = null)
        {
            List<Event> events = new List<Event>();
            try
            {
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT IdEvent, EventTime, Notes FROM Events";

                    if (startTime != null && endTime != null)
                    {
                        query += " WHERE EventTime BETWEEN " +
                        SqliteSafe.Date(startTime) + " AND " + SqliteSafe.Date(endTime);
                    }

                    query += " ORDER BY EventTime DESC;";

                    DbCommand cmd = new SqliteCommand(query);
                    cmd.Connection = conn;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Event evt = GetEventFromRow(reader);
                            events.Add(evt);
                        }
                    }

                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - GetEvents", ex);
            }
            return events;
        }

        /// <summary>
        /// Get a single Event by its ID
        /// </summary>
        internal Event GetOneEvent(int? idEvent)
        {
            Event evt = null;

            try
            {
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT IdEvent, EventTime, Notes FROM Events " +
                                "WHERE IdEvent=" + SqliteSafe.Int(idEvent) + ";";

                    DbCommand cmd = new SqliteCommand(query);
                    cmd.Connection = conn;

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            evt = GetEventFromRow(reader);
                        }
                    }

                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - GetOneEvent", ex);
            }

            return evt;
        }

        /// <summary>
        /// Delete an Event from the database
        /// </summary>
        internal void DeleteOneEvent(Event evt)
        {
            try
            {
                if (evt?.IdEvent == null)
                    return;

                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Events WHERE IdEvent=" + SqliteSafe.Int(evt.IdEvent) + ";";

                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }

                General.LogOfProgram?.Event($"SqLite_Events - Deleted event with ID: {evt.IdEvent}");
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - DeleteOneEvent", ex);
            }
        }

        /// <summary>
        /// Build an Event object from a database row
        /// </summary>
        private Event GetEventFromRow(DbDataReader row)
        {
            Event evt = new Event();

            try
            {
                evt.IdEvent = Safe.Int(row["IdEvent"]);
                evt.EventTime = new DateTimeAndText
                {
                    DateTime = Safe.DateTime(row["EventTime"])
                };
                evt.Notes = Safe.String(row["Notes"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("SqLite_Events - GetEventFromRow", ex);
            }

            return evt;
        }
    }
}
