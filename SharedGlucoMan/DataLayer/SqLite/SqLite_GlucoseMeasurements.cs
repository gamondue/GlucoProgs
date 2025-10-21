using gamon;
using Microsoft.Data.Sqlite;
using System.Data.Common;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override int GetNextPrimaryKey()
        {
            return GetTableNextPrimaryKey("GlucoseRecords", "IdGlucoseRecord");
        }
        internal override List<GlucoseRecord> GetGlucoseRecords(
            DateTime? InitialInstant = null, DateTime? FinalInstant = null)
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM GlucoseRecords";
                    if (InitialInstant != null && FinalInstant != null)
                    {   // add WHERE clause
                        query += " WHERE Timestamp BETWEEN " + SqliteSafe.Date(InitialInstant) +
                            " AND " + SqliteSafe.Date(FinalInstant);
                    }
                    query += " ORDER BY Timestamp DESC";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        GlucoseRecord g = GetGlucoseRecordFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list;
        }
        internal override GlucoseRecord GetOneGlucoseRecord(int? IdGlucoseRecord)
        {
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM GlucoseRecords";
                    query += " WHERE idGlucoseRecord=" + IdGlucoseRecord;
                    query += ";";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        gr = GetGlucoseRecordFromRow(dRead);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return gr;
        }
        internal override List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
        {
            List<GlucoseRecord> list = new List<GlucoseRecord>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM GlucoseRecords";
                    query += " ORDER BY Timestamp DESC LIMIT 2";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        GlucoseRecord g = GetGlucoseRecordFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list;
        }
        internal GlucoseRecord GetGlucoseRecordFromRow(DbDataReader Row)
        {
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                int? value = Safe.Int(Row["IdGlucoseRecord"]);
                gr.IdGlucoseRecord = value;
                gr.Timestamp.DateTime = Safe.DateTime(Row["Timestamp"]);
                gr.GlucoseValue.Double = Safe.Double(Row["GlucoseValue"]);
                gr.GlucoseString = Safe.String(Row["GlucoseString"]);
                gr.IdDevice = Safe.String(Row["IdDevice"]);
                value = Safe.Int(Row["IdTypeOfGlucoseMeasurement"]);
                if (value != null)
                    gr.TypeOfGlucoseMeasurement = (Common.TypeOfGlucoseMeasurement)value;
                value = Safe.Int(Row["IdTypeOfGlucoseMeasurementDevice"]);
                if (value != null)
                    gr.TypeOfGlucoseMeasurementDevice = (Common.TypeOfGlucoseMeasurementDevice)value;
                gr.IdModelOfMeasurementSystem = Safe.String(Row["IdModelOfMeasurementSystem"]);
                gr.IdDocumentType.Int = Safe.Int(Row["IdDocumentType"]);
                gr.Notes = Safe.String(Row["Notes"]);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetGlucoseRecordFromRow", ex);
            }
            return gr;
        }
        internal override void SaveGlucoseMeasurements(List<GlucoseRecord> List)
        {
            try
            {
                foreach (GlucoseRecord rec in List)
                {
                    SaveOneGlucoseMeasurement(rec);
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
            }
        }
        internal override long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement)
        {
            try
            {
                if (GlucoseMeasurement.IdGlucoseRecord == null || GlucoseMeasurement.IdGlucoseRecord == 0)
                {
                    GlucoseMeasurement.IdGlucoseRecord = GetNextPrimaryKey();
                    // INSERT new record in the table
                    InsertGlucoseMeasurement(GlucoseMeasurement);
                }
                else
                {   // GlucoseMeasurement.IdGlucoseRecord exists
                    UpdateGlucoseMeasurement(GlucoseMeasurement);
                }
                return GlucoseMeasurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
                return null;
            }
        }
        private long? UpdateGlucoseMeasurement(GlucoseRecord Measurement)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE GlucoseRecords SET " +
                    "GlucoseValue=" + SqliteSafe.Double(Measurement.GlucoseValue.Double) + "," +
                    "Timestamp=" + SqliteSafe.Date(Measurement.Timestamp.DateTime) + "," +
                    "GlucoseString=" + SqliteSafe.String(Measurement.GlucoseString) + "," +
                    "IdTypeOfGlucoseMeasurement=" + SqliteSafe.Int((int?)Measurement.TypeOfGlucoseMeasurement) + "," +
                    "IdTypeOfGlucoseMeasurementDevice=" + SqliteSafe.Int((int?)Measurement.TypeOfGlucoseMeasurementDevice) + "," +
                    "IdModelOfMeasurementSystem=" + SqliteSafe.String(Measurement.IdModelOfMeasurementSystem) + "," +
                    "IdDevice=" + SqliteSafe.String(Measurement.IdDevice) + "," +
                    "IdDocumentType=" + SqliteSafe.Int(Measurement.IdDocumentType.Int) + "," +
                    "Notes=" + SqliteSafe.String(Measurement.Notes) + "";
                    query += " WHERE IdGlucoseRecord=" + SqliteSafe.Int(Measurement.IdGlucoseRecord);
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Measurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
                return null;
            }
        }
        private long? InsertGlucoseMeasurement(GlucoseRecord Measurement)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO GlucoseRecords" +
                    "(" +
                    "IdGlucoseRecord,GlucoseValue,Timestamp,GlucoseString," +
                    "IdTypeOfGlucoseMeasurement,IdTypeOfGlucoseMeasurementDevice,IdModelOfMeasurementSystem," +
                    "IdDevice,IdDocumentType,Notes";
                    query += ")VALUES (" +
                    SqliteSafe.Int(Measurement.IdGlucoseRecord) + "," +
                    SqliteSafe.Double(Measurement.GlucoseValue.Double) + "," +
                    SqliteSafe.Date(Measurement.Timestamp.DateTime) + "," +
                    SqliteSafe.String(Measurement.GlucoseString) + "," +
                    SqliteSafe.Int((int?)Measurement.TypeOfGlucoseMeasurement) + "," +
                    SqliteSafe.Int((int?)Measurement.TypeOfGlucoseMeasurementDevice) + "," +
                    SqliteSafe.String(Measurement.IdModelOfMeasurementSystem) + "," +
                    SqliteSafe.String(Measurement.IdDevice) + "," +
                    SqliteSafe.Int(Measurement.IdDocumentType.Int) + "," +
                    SqliteSafe.String(Measurement.Notes) + ")";
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Measurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | InsertIntoGlucoseMeasurement", ex);
                return null;
            }
        }
        internal override void DeleteOneGlucoseMeasurement(GlucoseRecord gr)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM GlucoseRecords" +
                    " WHERE IdGlucoseRecord=" + gr.IdGlucoseRecord;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_GlucoseMeasurement | DeleteOneGlucoseMeasurement", ex);
            }
        }
    }
}
