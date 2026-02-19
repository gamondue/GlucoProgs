using gamon;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        // PhysicalActivities CRUD moved to its own partial class file
        internal override List<PhysicalActivity> GetPhysicalActivities(DateTime? InitialInstant = null, DateTime? FinalInstant = null)
        {
            List<PhysicalActivity> list = new List<PhysicalActivity>();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT * FROM PhysicalActivities";
                    if (InitialInstant != null && FinalInstant != null)
                    {
                        query += " WHERE EventTime BETWEEN " + SqliteSafe.Date(InitialInstant) + " AND " + SqliteSafe.Date(FinalInstant);
                    }
                    query += " ORDER BY EventTime DESC;";
                    cmd.CommandText = query;
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var a = new PhysicalActivity();
                            a.IdActivity = Safe.Int(reader["IdActivity"]);
                            a.EventTime = Safe.DateTime(reader["EventTime"]);
                            a.ActivityLevel = Safe.Int(reader["ActivityLevel"]);
                            a.DurationMinutes = Safe.Int(reader["DurationMinutes"]);
                            a.Intensity = Safe.String(reader["Intensity"]);
                            a.Accuracy = Safe.Double(reader["Accuracy"]);
                            a.Notes = Safe.String(reader["Notes"]);
                            a.IdTrack = Safe.Int(reader["IdTrack"]);
                            list.Add(a);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_PhysicalActivities | GetPhysicalActivities", ex);
            }
            return list;
        }
        
        internal override PhysicalActivity GetOnePhysicalActivity(int? IdActivity)
        {
            PhysicalActivity a = null;
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM PhysicalActivities WHERE IdActivity=" + SqliteSafe.Int(IdActivity) + ";";
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            a = new PhysicalActivity();
                            a.IdActivity = Safe.Int(reader["IdActivity"]);
                            a.EventTime = Safe.DateTime(reader["EventTime"]);
                            a.ActivityLevel = Safe.Int(reader["ActivityLevel"]);
                            a.DurationMinutes = Safe.Int(reader["DurationMinutes"]);
                            a.Intensity = Safe.String(reader["Intensity"]);
                            a.Accuracy = Safe.Double(reader["Accuracy"]);
                            a.Notes = Safe.String(reader["Notes"]);
                            a.IdTrack = Safe.Int(reader["IdTrack"]);
                        }
                    }
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_PhysicalActivities | GetOnePhysicalActivity", ex);
            }
            return a;
        }

        internal override int? SaveOnePhysicalActivity(PhysicalActivity activity)
        {
            try
            {
                if (activity == null)
                    return null;

                if (activity.IdActivity == null || activity.IdActivity == 0)
                {
                    activity.IdActivity = GetTableNextPrimaryKey("PhysicalActivities", "IdActivity");
                    using (DbConnection conn = Connect())
                    {
                        DbCommand cmd = conn.CreateCommand();
                        string query = "INSERT INTO PhysicalActivities (IdActivity, EventTime, ActivityLevel, " +
                            "DurationMinutes, Intensity, Accuracy, Notes, IdTrack) VALUES (" + 
                             SqliteSafe.Int(activity.IdActivity) + "," + SqliteSafe.Date(activity.EventTime) + "," + 
                             SqliteSafe.Int(activity.ActivityLevel) + "," + SqliteSafe.Int(activity.DurationMinutes) + "," + 
                             SqliteSafe.String(activity.Intensity) + "," + SqliteSafe.Double(activity.Accuracy) + "," +
                             SqliteSafe.String(activity.Notes) + "," + SqliteSafe.Int(activity.IdTrack) + ");";
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                else
                {
                    using (DbConnection conn = Connect())
                    {
                        DbCommand cmd = conn.CreateCommand();
                        string query = "UPDATE PhysicalActivities SET EventTime=" + SqliteSafe.Date(activity.EventTime) + 
                            ", ActivityLevel=" + SqliteSafe.Int(activity.ActivityLevel) + 
                            ", DurationMinutes=" + SqliteSafe.Int(activity.DurationMinutes) + 
                            ", Intensity=" + SqliteSafe.String(activity.Intensity) + 
                            ", Accuracy=" + SqliteSafe.Double(activity.Accuracy) + 
                            ", Notes=" + SqliteSafe.String(activity.Notes) + 
                            ", IdTrack=" + SqliteSafe.Int(activity.IdTrack) + 
                            " WHERE IdActivity=" + SqliteSafe.Int(activity.IdActivity) + ";";
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                return activity.IdActivity;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_PhysicalActivities | SavePhysicalActivity", ex);
                return null;
            }
        }

        internal override int? DeleteOnePhysicalActivity(PhysicalActivity activity)
        {
            throw new NotImplementedException();
        }
    }
}
