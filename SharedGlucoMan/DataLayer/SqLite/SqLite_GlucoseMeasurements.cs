using Microsoft.Data.Sqlite;
using SharedData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal  partial class DL_Sqlite : DataLayer
    {
        internal  override int GetNextPrimaryKey()
        {
            return GetNextTablePrimaryKey("GlucoseRecords", "IdGlucoseRecord");
        }
        internal  override List<GlucoseRecord> ReadGlucoseMeasurements(
            DateTime? InitialInstant, DateTime? FinalInstant)
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
                        query += " WHERE Timestamp BETWEEN " + ((DateTime)InitialInstant).ToString("YYYY-MM-DD") +
                            " AND " + ((DateTime)FinalInstant).ToString("YYYY-MM-DD"); 
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
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list; 
        }
        internal  override List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
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
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | ReadGlucoseMeasurements", ex);
            }
            return list;
        }
        internal GlucoseRecord GetGlucoseRecordFromRow(DbDataReader Row)
        {
            GlucoseRecord gr = new GlucoseRecord();
            try
            {
                gr.IdGlucoseRecord = Safe.Int(Row["IdGlucoseRecord"]);
                gr.GlucoseValue = Safe.Double(Row["GlucoseValue"]);
                gr.Timestamp = Safe.DateTime(Row["Timestamp"]);
                gr.GlucoseString = Safe.String(Row["GlucoseString"]);
                gr.IdDevice = Safe.String(Row["IdDevice"]);
                gr.IdDeviceType = Safe.String(Row["IdDeviceType"]);
                gr.Notes = Safe.String(Row["Notes"]);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | GetGlucoseRecordFromRow", ex);
            }
            return gr;
        }
        internal  override void SaveGlucoseMeasurements(List<GlucoseRecord> List)
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
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
            }
        }
        internal  override long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement)
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
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
                return null; 
            }
        }
        private long? UpdateGlucoseMeasurement(GlucoseRecord Measurement)
        {
            try { 
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE GlucoseRecords SET " +
                    "GlucoseValue=" + SqlDouble(Measurement.GlucoseValue) + "," +
                    "Timestamp=" + SqlDate(Measurement.Timestamp) + "," +
                    "GlucoseString=" + SqlString(Measurement.GlucoseString) + "," +
                    "IdDevice=" + SqlString(Measurement.IdDevice) + "," +
                    "IdDeviceType=" + SqlString(Measurement.IdDeviceType) + "," +
                    "Notes=" + SqlString(Measurement.Notes) + ""; 
                    query += " WHERE IdGlucoseRecord=" + SqlInt(Measurement.IdGlucoseRecord);
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Measurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | SaveGlucoseMeasurements", ex);
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
                    "IdDevice,IdDeviceType,Notes";
                    query += ")VALUES (" +
                    SqlInt(Measurement.IdGlucoseRecord) + "," +
                    SqlDouble(Measurement.GlucoseValue) + "," +
                    SqlDate(Measurement.Timestamp) + "," +
                    SqlString(Measurement.GlucoseString) + "," +
                    SqlString(Measurement.IdDevice) + "," +
                    SqlString(Measurement.IdDeviceType) + "," +
                    SqlString(Measurement.Notes) + ")";
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Measurement.IdGlucoseRecord;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | InsertIntoGlucoseMeasurement", ex);
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
                Common.LogOfProgram.Error("Sqlite_GlucoseMeasurement | DeleteOneGlucoseMeasurement", ex);
            }
        }
    }
}
