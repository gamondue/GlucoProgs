using gamon;
using GlucoMan.BusinessObjects;
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
                    int? secondsOfTriggerInterval = (int?)alarm.TriggerInterval?.TotalSeconds;
                    int? secondsOfInterval = (int?)alarm.Interval?.TotalSeconds;
                    int? secondsOfDuration = (int?)alarm.Duration?.TotalSeconds;
                    int? secondsOfRepetition = (int?)alarm.RepetitionTime?.TotalSeconds;

                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE Alarms SET " +
                        "TimeStart = " + SqliteSafe.Date(alarm.TimeStart.DateTime) + ", " +
                        "ReminderText = " + SqliteSafe.String(alarm.ReminderText) + ", " +
                        "TriggerInterval = " + SqliteSafe.Int(secondsOfTriggerInterval) + ", " +
                        "Interval = " + SqliteSafe.Int(secondsOfInterval) + ", " +
                        "Duration = " + SqliteSafe.Int(secondsOfDuration) + ", " +
                        "RepeatCount = " + SqliteSafe.Int(alarm.RepeatCount) + ", " +
                        "MaxRepeatCount = " + SqliteSafe.Int(alarm.MaxRepeatCount) + ", " +
                        "NextTriggerTime = " + SqliteSafe.Date(alarm.NextTriggerTime) + ", " +
                        "LastTriggerTime = " + SqliteSafe.Date(alarm.LastTriggerTime) + ", " +
                        "TriggeredCount = " + SqliteSafe.Int(alarm.TriggeredCount) + ", " +
                        "Playing = " + SqliteSafe.Bool(alarm.Playing) + ", " +
                        "RepetitionTime = " + SqliteSafe.Int(secondsOfRepetition) + ", " +
                        "PlaySoundFile = " + SqliteSafe.Bool(alarm.PlaySoundFile) + ", " +
                        "SoundFilePath = " + SqliteSafe.String(alarm.SoundFilePath) + ", " +
                        "Vibrate = " + SqliteSafe.Bool(alarm.Vibrate) + " " +
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
                    int? secondsOfTriggerInterval = (int?)alarm.TriggerInterval?.TotalSeconds;
                    int? secondsOfInterval = (int?)alarm.Interval?.TotalSeconds;
                    int? secondsOfDuration = (int?)alarm.Duration?.TotalSeconds;
                    int? secondsOfRepetition = (int?)alarm.RepetitionTime?.TotalSeconds;

                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Alarms" +
                    "(" +
                    "IdAlarm,TimeStart,ReminderText,TriggerInterval,Interval,Duration," +
                    "RepeatCount,MaxRepeatCount,NextTriggerTime,LastTriggerTime,TriggeredCount," +
                    "Playing,RepetitionTime,PlaySoundFile,SoundFilePath,Vibrate";
                    query += ")VALUES(" +
                    SqliteSafe.Int(alarm.IdAlarm) + "," +
                    SqliteSafe.Date(alarm.TimeStart.DateTime) + "," +
                    SqliteSafe.String(alarm.ReminderText) + "," +
                    SqliteSafe.Int(secondsOfTriggerInterval) + "," +
                    SqliteSafe.Int(secondsOfInterval) + "," +
                    SqliteSafe.Int(secondsOfDuration) + "," +
                    SqliteSafe.Int(alarm.RepeatCount) + "," +
                    SqliteSafe.Int(alarm.MaxRepeatCount) + "," +
                    SqliteSafe.Date(alarm.NextTriggerTime) + "," +
                    SqliteSafe.Date(alarm.LastTriggerTime) + "," +
                    SqliteSafe.Int(alarm.TriggeredCount) + "," +
                    SqliteSafe.Bool(alarm.Playing) + "," +
                    SqliteSafe.Int(secondsOfRepetition) + "," +
                    SqliteSafe.Bool(alarm.PlaySoundFile) + "," +
                    SqliteSafe.String(alarm.SoundFilePath) + "," +
                    SqliteSafe.Bool(alarm.Vibrate) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return alarm.IdAlarm;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_AlarmMeasurement | InsertAlarm", ex);
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

                    // Active / expired logic based on TriggerInterval and repetition counters
                    if (!all)
                    {
                        if (active && !expired)
                        {
                            // Active: still inside trigger window OR scheduled by NextTriggerTime
                            whereConditions.Add("( (TriggerInterval IS NULL OR datetime(TimeStart, '+' || COALESCE(TriggerInterval,0) || ' seconds') > " + SqliteSafe.Date(DateTime.Now) + ")" +
                                                 " OR (NextTriggerTime IS NOT NULL AND NextTriggerTime > " + SqliteSafe.Date(DateTime.Now) + ") )");
                            // Not exceeded max repeat count
                            whereConditions.Add("(MaxRepeatCount IS NULL OR RepeatCount IS NULL OR RepeatCount < MaxRepeatCount)");
                        }
                        else if (expired && !active)
                        {
                            // Expired: outside trigger window OR max repeat count reached
                            whereConditions.Add("( (TriggerInterval IS NOT NULL AND datetime(TimeStart, '+' || TriggerInterval || ' seconds') <= " + SqliteSafe.Date(DateTime.Now) + ")" +
                                                 " OR (MaxRepeatCount IS NOT NULL AND RepeatCount IS NOT NULL AND RepeatCount >= MaxRepeatCount) )");
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
                m.TimeStart.DateTime = Safe.DateTime(Row["TimeStart"]);
                m.ReminderText = Safe.String(Row["ReminderText"]);
                m.TriggerInterval = Safe.TimeSpanFromSeconds(Row["TriggerInterval"]);
                m.Interval = Safe.TimeSpanFromSeconds(Row["Interval"]);
                m.Duration = Safe.TimeSpanFromSeconds(Row["Duration"]);
                m.RepeatCount = Safe.Int(Row["RepeatCount"]);
                m.MaxRepeatCount = Safe.Int(Row["MaxRepeatCount"]);
                m.NextTriggerTime = Safe.DateTime(Row["NextTriggerTime"]);
                m.LastTriggerTime = Safe.DateTime(Row["LastTriggerTime"]);
                m.TriggeredCount = Safe.Int(Row["TriggeredCount"]);
                m.Playing = Safe.Bool(Row["Playing"]);
                m.RepetitionTime = Safe.TimeSpanFromSeconds(Row["RepetitionTime"]);
                m.PlaySoundFile = Safe.Bool(Row["PlaySoundFile"]);
                m.SoundFilePath = Safe.String(Row["SoundFilePath"]);
                m.Vibrate = Safe.Bool(Row["Vibrate"]);
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