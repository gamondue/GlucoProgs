using gamon;
using GlucoMan;
//using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int? SaveOneAlarm(Alarm Alarm)
        {
            int? IdAlarm = null;
            try
            {
                if (Alarm.IdAlarm == null || Alarm.IdAlarm == 0)
                {
                    Alarm.IdAlarm = GetTableNextPrimaryKey("Alarms", "IdAlarm");
                    if (Alarm.TimeStart == null)
                        Alarm.TimeStart = new DateTimeAndText();
                    if (Alarm.TimeStart.DateTime == null || Alarm.TimeStart.DateTime == default)
                        Alarm.TimeStart.DateTime = DateTime.Now;
                    InsertAlarm(Alarm);
                }
                else
                {   // update existing record 
                    UpdateAlarm(Alarm);
                }
                return Alarm.IdAlarm;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite Datalayer | SaveOneAlarm", ex);
                return null;
            }
        }
        
        private void UpdateAlarm(Alarm alarm)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    int? secondsOfStartupGrace = (int?)alarm.StartupGraceWindow?.TotalSeconds;
                    int? secondsOfInterval = (int?)alarm.Interval?.TotalSeconds;
                    int? secondsOfDuration = (int?)alarm.Duration?.TotalSeconds;
                    int? secondsOfRepetition = (int?)alarm.RepetitionTime?.TotalSeconds;

                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Alarms SET " +
                        "ReminderText = " + SqliteSafe.String(alarm.ReminderText) + ", " +
                        "TimeStart = " + SqliteSafe.Date(alarm.TimeStart.DateTime) + ", " +
                        "NextTriggerTime = " + SqliteSafe.Date(alarm.NextTriggerTime) + ", " +
                        "IsDisabled = " + SqliteSafe.Bool(alarm.IsDisabled) + ", " +
                        "StartupGraceWindow = " + SqliteSafe.Int(secondsOfStartupGrace) + ", " +
                        "RingingState = " + SqliteSafe.Int((int)alarm.RingingState) + ", " +
                        "Duration = " + SqliteSafe.Int(secondsOfDuration) + ", " +
                        "RepetitionTime = " + SqliteSafe.Int(secondsOfRepetition) + ", " +
                        "Interval = " + SqliteSafe.Int(secondsOfInterval) + ", " +
                        "IsPlaying = " + SqliteSafe.Bool(alarm.IsPlaying) + ", " +
                        "EnablePlaySoundFile = " + SqliteSafe.Bool(alarm.EnablePlaySoundFile) + ", " +
                        "SoundFilePath = " + SqliteSafe.String(alarm.SoundFilePath) + ", " +
                        "RepeatCount = " + SqliteSafe.Int(alarm.RepeatCount) + ", " +
                        "MaxRepeatCount = " + SqliteSafe.Int(alarm.MaxRepeatCount) + ", " +
                        "LastTriggerTime = " + SqliteSafe.Date(alarm.LastTriggerTime) + ", " +
                        "TriggeredCount = " + SqliteSafe.Int(alarm.TriggeredCount) + ", " +
                        "DoVibrate = " + SqliteSafe.Bool(alarm.DoVibrate) + " " +
                        "WHERE IdAlarm = " + SqliteSafe.Int(alarm.IdAlarm) + ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_AlarmManagement | UpdateAlarm", ex);
            }
        }

        private int? InsertAlarm(Alarm alarm)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    int? secondsOfStartupGrace = (int?)alarm.StartupGraceWindow?.TotalSeconds;
                    int? secondsOfInterval = (int?)alarm.Interval?.TotalSeconds;
                    int? secondsOfDuration = (int?)alarm.Duration?.TotalSeconds;
                    int? secondsOfRepetition = (int?)alarm.RepetitionTime?.TotalSeconds;

                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Alarms" +
                    "(" +
                    "IdAlarm, ReminderText, TimeStart, NextTriggerTime, IsDisabled, " +
                    "StartupGraceWindow, RingingState, Duration, RepetitionTime, Interval, " +
                    "IsPlaying, EnablePlaySoundFile, SoundFilePath, RepeatCount, MaxRepeatCount, " +
                    "LastTriggerTime, TriggeredCount, DoVibrate" +
                    ")VALUES(" +
                    SqliteSafe.Int(alarm.IdAlarm) + "," +
                    SqliteSafe.String(alarm.ReminderText) + "," +
                    SqliteSafe.Date(alarm.TimeStart.DateTime) + "," +
                    SqliteSafe.Date(alarm.NextTriggerTime) + "," +
                    SqliteSafe.Bool(alarm.IsDisabled) + "," +
                    SqliteSafe.Int(secondsOfStartupGrace) + "," +
                    SqliteSafe.Int((int)alarm.RingingState) + "," +
                    SqliteSafe.Int(secondsOfDuration) + "," +
                    SqliteSafe.Int(secondsOfRepetition) + "," +
                    SqliteSafe.Int(secondsOfInterval) + "," +
                    SqliteSafe.Bool(alarm.IsPlaying) + "," +
                    SqliteSafe.Bool(alarm.EnablePlaySoundFile) + "," +
                    SqliteSafe.String(alarm.SoundFilePath) + "," +
                    SqliteSafe.Int(alarm.RepeatCount) + "," +
                    SqliteSafe.Int(alarm.MaxRepeatCount) + "," +
                    SqliteSafe.Date(alarm.LastTriggerTime) + "," +
                    SqliteSafe.Int(alarm.TriggeredCount) + "," +
                    SqliteSafe.Bool(alarm.DoVibrate) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return alarm.IdAlarm;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_AlarmManagement | InsertAlarm", ex);
                return null;
            }
        }
        
        internal override List<Alarm> GetAllAlarms(DateTime? from = null, DateTime? to = null,
            bool all = false, bool expired = false, bool active = true)
        {
            List<Alarm> alarms = new List<Alarm>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT * FROM Alarms";

                    // Build WHERE clause based on optional parameters
                    List<string> whereConditions = new List<string>();

                    // RingingState values mirror Alarm.AlarmRingingState:
                    //   0=Waiting, 1=Disabled, 2=Ringing, 3=Dismissed, 4=Delayed, 5=AutoSuspended, 6=Expired
                    // IsDisabled in DB is always in sync with RingingState=1 (Disabled).
                    if (!all)
                    {
                        if (active && !expired)
                        {
                            // Active: RingingState is not Disabled(1), Dismissed(3) or Expired(6).
                            whereConditions.Add("(RingingState IS NULL OR RingingState NOT IN (1, 3, 6))");
                        }
                        else if (expired && !active)
                        {
                            // Inactive: Disabled(1), Dismissed(3), Expired(6), or max restarts reached
                            whereConditions.Add("( RingingState IN (1, 3, 6)" +
                                                 " OR (MaxRepeatCount IS NOT NULL AND MaxRepeatCount > 0 AND RepeatCount IS NOT NULL AND RepeatCount >= MaxRepeatCount) )");
                        }
                    }

                    if (from.HasValue)
                    {
                        whereConditions.Add("TimeStart >= " + SqliteSafe.Date(from.Value));
                    }

                    if (to.HasValue)
                    {
                        whereConditions.Add("TimeStart <= " + SqliteSafe.Date(to.Value));
                    }

                    if (whereConditions.Count > 0)
                    {
                        query += " WHERE " + string.Join(" AND ", whereConditions);
                    }

                    query += " ORDER BY IdAlarm DESC;";

                    cmd = conn.CreateCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = query;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Alarm f = GetAlarmFromRow(dRead);
                        alarms.Add(f);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Alarm | GetAllAlarms", ex);
            }
            return alarms;
        }
        
        private Alarm GetAlarmFromRow(DbDataReader Row)
        {
            Alarm m = new Alarm();
            try
            {
                m.IdAlarm = Safe.Int(Row["IdAlarm"]);
                m.ReminderText = Safe.String(Row["ReminderText"]);
                m.TimeStart.DateTime = Safe.DateTime(Row["TimeStart"]);
                m.NextTriggerTime = Safe.DateTime(Row["NextTriggerTime"]);
                m.StartupGraceWindow = Safe.TimeSpanFromSeconds(Row["StartupGraceWindow"]);

                int? ringingStateValue = Safe.Int(Row["RingingState"]);
                if (ringingStateValue.HasValue)
                    m.RingingState = (Alarm.AlarmRingingState)ringingStateValue.Value;

                // Migration: if the legacy IsDisabled column is true but RingingState is not yet
                // Disabled, promote RingingState so the two are in sync.
                bool? dbIsDisabled = Safe.Bool(Row["IsDisabled"]);
                if (dbIsDisabled == true && m.RingingState != Alarm.AlarmRingingState.Disabled)
                    m.RingingState = Alarm.AlarmRingingState.Disabled;

                m.Duration = Safe.TimeSpanFromSeconds(Row["Duration"]);
                m.RepetitionTime = Safe.TimeSpanFromSeconds(Row["RepetitionTime"]);
                m.Interval = Safe.TimeSpanFromSeconds(Row["Interval"]);
                m.IsPlaying = Safe.Bool(Row["IsPlaying"]);
                m.EnablePlaySoundFile = Safe.Bool(Row["EnablePlaySoundFile"]);
                m.SoundFilePath = Safe.String(Row["SoundFilePath"]);
                m.RepeatCount = Safe.Int(Row["RepeatCount"]);
                m.MaxRepeatCount = Safe.Int(Row["MaxRepeatCount"]);
                m.LastTriggerTime = Safe.DateTime(Row["LastTriggerTime"]);
                m.TriggeredCount = Safe.Int(Row["TriggeredCount"]);
                m.DoVibrate = Safe.Bool(Row["DoVibrate"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_Alarm | GetAlarmFromRow", ex);
            }
            return m;
        }
        
        internal override void DeleteOneAlarm(Alarm alarm)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Alarms WHERE IdAlarm = " + SqliteSafe.Int(alarm.IdAlarm) + ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_AlarmManagement | DeleteOneAlarm", ex);
            }
        }
        
        internal void DeleteAlarmById(int? alarmId)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM Alarms WHERE IdAlarm = " + SqliteSafe.Int(alarmId) + ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_AlarmManagement | DeleteAlarmById", ex);
            }
        }
    }
}