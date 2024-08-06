using gamon;
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
                    Alarm.IdAlarm = GetNextTablePrimaryKey("Alarms", "IdAlarm");
                    Alarm.TimeStart.DateTime = DateTime.Now;
                    // INSERT new record in the table
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
                General.Log.Error("Sqlite Datalayer | SaveOneAlarm", ex);
                return null;
            }
            return IdAlarm;
        }
        private void UpdateAlarm(Alarm alarm)
        {
            throw new NotImplementedException();
        }
        private int? InsertAlarm(Alarm alarm)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    int? secondsOfInterval = (int?)((TimeSpan)alarm.Interval).TotalSeconds;
                    int? secondsOfDuration = (int?)((TimeSpan)alarm.Duration).TotalSeconds;

                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO Alarms" +
                    "(" +
                    "IdAlarm,TimeStart,TimeAlarm,Interval,Duration," +
                    "IsRepeated,IsEnabled";
                    query += ")VALUES(" +
                    SqliteSafe.Int(alarm.IdAlarm) + "," +
                    SqliteSafe.Date(alarm.TimeStart.DateTime) + "," +
                    SqliteSafe.Date(alarm.TimeAlarm.DateTime) + "," +
                    SqliteSafe.Int(secondsOfInterval) + "," +
                    SqliteSafe.Int(secondsOfDuration) + "," +
                    SqliteSafe.Bool(alarm.IsRepeated) + "," +
                    SqliteSafe.Bool(alarm.IsEnabled) + "";
                    query += ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return alarm.IdAlarm;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_AlarmMeasurement | InsertAlarm", ex);
                return null;
            }
        }
        internal override List<Alarm> ReadAllAlarms()
        {
            List<Alarm> alarms = new List<Alarm>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM Alarms";
                    query += " ORDER BY IdAlarm DESC";
                    query += ";";
                    cmd = conn.CreateCommand();
                    cmd.Connection = conn;
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
                General.Log.Error("Sqlite_Alarm | ReadAllAlarms", ex);
            }
            return alarms;
        }
        private Alarm GetAlarmFromRow(DbDataReader Row)
        {
            Alarm m = new Alarm();
            try
            {
                m.IdAlarm = SqlSafe.Int(Row["IdAlarm"]);
                m.TimeStart.DateTime = SqlSafe.DateTime(Row["TimeStart"]);
                m.TimeAlarm.DateTime = SqlSafe.DateTime(Row["TimeAlarm"]);
                m.Interval = SqlSafe.TimeSpanFromSeconds(Row["Interval"]);
                m.Duration = SqlSafe.TimeSpanFromMinutes(Row["Duration"]);
                m.IsEnabled = SqlSafe.Bool(Row["IsEnabled"]);
                m.IsRepeated = SqlSafe.Bool(Row["IsRepeated"]);
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_Alarm | GetAlarmFromRow", ex);
            }
            return m;
        }
    }
}