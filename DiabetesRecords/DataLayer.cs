using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using gamon;
using System.IO;

namespace DiabetesRecords
{
    internal class DataLayer
    {
        string creationScript = @"
BEGIN TRANSACTION;
DROP TABLE IF EXISTS 'DiabetesRecords';
CREATE TABLE IF NOT EXISTS 'DiabetesRecords' (
	'IdDiabetesRecord'	INTEGER NOT NULL,
	'Timestamp'	DATETIME,
	'GlucoseValue'	DOUBLE,
	'InsulinValue'	DOUBLE,
	'IdTypeOfInsulinSpeed'	INTEGER,
	'IdTypeOfMeal'	INTEGER,
	'Notes'	TEXT,
	PRIMARY KEY('IdDiabetesRecord')
);
COMMIT;
";
        string connectionString;
        string pathAndFileDatabase;
        public DataLayer (string PathAndFileDatabase)
        {
            connectionString = "Data Source=\"" + PathAndFileDatabase + "\"; Cache = Shared; Mode = ReadWriteCreate";
            pathAndFileDatabase = PathAndFileDatabase; 
        }
        internal SqliteConnection Connect()
        {
            SqliteConnection connection;
            try
            {
                connection = new SqliteConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                General.Log.Error("Error connecting to the database: " + ex.Message + "\r\nFile Sqlite>: DiabetesRecords.sqlite " + "\n", null);
                connection = null;
            }
            return connection;
        }
        internal void CreateNewDatabase(string dbName)
        {
            // making new, means erasing existent! 
            if (File.Exists(dbName))
                File.Delete(dbName);
            //when the file does not exist
            // Microsoft.Data.Sqlite creates the file at first connection
            DbConnection c = Connect();
            c.Close();
            c.Dispose();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();

                    cmd.CommandText = creationScript;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DataAndGeneral | CreateNewDatabase", ex);
            }
        }
        internal int GetNextTablePrimaryKey(string Table, string KeyName)
        {
            int nextId;
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(" + KeyName + ") FROM " + Table + ";";
                var firstColumn = cmd.ExecuteScalar();
                if (firstColumn != DBNull.Value)
                {
                    nextId = int.Parse(firstColumn.ToString()) + 1;
                }
                else
                {
                    nextId = 1;
                }
                cmd.Dispose();
            }
            return nextId;
        }
        internal DiabetesRecord GetOneDiabetesRecord(int? IdDiabetesRecord)
        {
            DiabetesRecord rec = new DiabetesRecord();
            if (IdDiabetesRecord == null)
                return rec; 
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT * " +
                        " FROM DiabetesRecords";
                    query += " WHERE IdDiabetesRecord=" + IdDiabetesRecord;
                    query += " ORDER BY Timestamp DESC, IdDiabetesRecord";
                    query += " LIMIT 1;";
                    
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    dRead.Read(); 
                    DiabetesRecord g = GetDiabetesRecordFromRow(dRead);
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("DataLayer | GetOneDiabetesRecord", ex);
            }
            return rec;
        }
        internal DiabetesRecord GetLastDiabetesRecord()
        {
            DiabetesRecord rec = new DiabetesRecord();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT * " +
                        " FROM DiabetesRecords"; 
                    query += " ORDER BY Timestamp DESC, IdDiabetesRecord;";
                    query += " LIMIT 1";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    dRead.Read(); 
                    rec = GetDiabetesRecordFromRow(dRead);
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("DataLayer | GetLastDiabetesRecord", ex);
            }
            return rec;
        }
        internal List<DiabetesRecord> GetDiabetesRecords(DateTime DateFrom, DateTime DateTo)
        {
            List<DiabetesRecord> list = new List<DiabetesRecord>();
            try
            {
                DbDataReader dRead;
                DbCommand cmd;
                using (DbConnection conn = Connect())
                {
                    string query = "SELECT *" +
                        " FROM DiabetesRecords";
                    if (DateFrom != null && DateTo != null)
                    {   // add WHERE clause
                        query += " WHERE Timestamp BETWEEN " + SqliteHelper.Date(((DateTime)DateFrom).ToString("yyyy-MM-dd")) +
                            " AND " + SqliteHelper.Date(((DateTime)DateTo).ToString("yyyy-MM-dd 23:59:29")) + "";
                    }
                    query += " ORDER BY Timestamp DESC, IdDiabetesRecord;";
                    cmd = new SqliteCommand(query);
                    cmd.Connection = conn;
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        DiabetesRecord g = GetDiabetesRecordFromRow(dRead);
                        list.Add(g);
                    }
                    dRead.Dispose();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("DataLayer | GetRecords", ex);
            }
            return list;
        }
        internal DiabetesRecord GetDiabetesRecordFromRow(DbDataReader Row)
        {
            DiabetesRecord dr = new DiabetesRecord();
            try
            {
                dr.IdDiabetesRecord = Safe.Int(Row["IdDiabetesRecord"]);
                dr.Timestamp = Safe.DateTime(Row["Timestamp"]);
                dr.GlucoseValue = Safe.Double(Row["GlucoseValue"]);
                dr.InsulinValue = Safe.Double(Row["InsulinValue"]);
                dr.IdTypeOfInsulinSpeed = Safe.Int(Row["IdTypeOfInsulinSpeed"]);
                dr.IdTypeOfMeal = Safe.Int(Row["IdTypeOfMeal"]);
                dr.Notes = Safe.String(Row["Notes"]);
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | GetDiabetesRecFromRow", ex);
            }
            return dr;
        }

        internal bool DeleteDatabase()
        {
            try
            {
                File.Delete(pathAndFileDatabase);
                return true;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | DeleteDatabase", ex);
                return false;
            }
        }
        internal void SaveDiabetesRecords(List<DiabetesRecord> List)
        {
            try
            {
                foreach (DiabetesRecord rec in List)
                {
                    SaveOneDiabetesRecord(rec);
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | SaveDiabetesRecords", ex);
            }
        }
        internal int? SaveOneDiabetesRecord(DiabetesRecord Record)
        {
            try
            {
                if (Record.IdDiabetesRecord == null || Record.IdDiabetesRecord == 0)
                {
                    Record.IdDiabetesRecord = GetNextTablePrimaryKey("DiabetesRecords", "IdDiabetesRecord");
                    // INSERT new record in the table
                    InsertDiabetesRecord(Record);
                }
                else
                {   // DiabetesRecord.IdDiabetesRecord exists
                    UpdateDiabetesRecord(Record);
                }
                return Record.IdDiabetesRecord;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | SaveDiabetesRecords", ex);
                return null;
            }
        }
        private long? UpdateDiabetesRecord(DiabetesRecord Record)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "UPDATE DiabetesRecords SET " +
                    "Timestamp=" + SqliteHelper.Date(Record.Timestamp) + "," +
                    "GlucoseValue=" + SqliteHelper.Double(Record.GlucoseValue) + "," +
                    "InsulinValue=" + SqliteHelper.Double(Record.InsulinValue) + "," +
                    "IdTypeOfInsulinSpeed=" + SqliteHelper.Int(Record.IdTypeOfInsulinSpeed) + "," +
                    "IdTypeOfMeal=" + SqliteHelper.Int(Record.IdTypeOfMeal) + "," +
                    "Notes=" + SqliteHelper.String(Record.Notes) + "";
                    query += " WHERE IdDiabetesRecord=" + SqliteHelper.Int(Record.IdDiabetesRecord);
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Record.IdDiabetesRecord;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | SaveDiabetesRecords", ex);
                return null;
            }
        }
        private long? InsertDiabetesRecord(DiabetesRecord Record)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "INSERT INTO DiabetesRecords" +
                    "(" +
                    "IdDiabetesRecord,Timestamp,GlucoseValue,InsulinValue," +
                    "IdTypeOfInsulinSpeed,IdTypeOfMeal,Notes";
                    query += ")VALUES (" +
                    SqliteHelper.Int(Record.IdDiabetesRecord) + "," +
                    SqliteHelper.Date(Record.Timestamp) + "," +
                    SqliteHelper.Double(Record.GlucoseValue) + "," +
                    SqliteHelper.Double(Record.InsulinValue) + "," +
                    SqliteHelper.Int(Record.IdTypeOfInsulinSpeed) + "," +
                    SqliteHelper.Int(Record.IdTypeOfMeal) + "," +
                    SqliteHelper.String(Record.Notes) + ")";
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
                return Record.IdDiabetesRecord;
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | InsertIntoDiabetesRecord", ex);
                return null;
            }
        }
        internal void DeleteOneDiabetesRecord(DiabetesRecord Record)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "DELETE FROM DiabetesRecords" +
                    " WHERE IdDiabetesRecord=" + Record.IdDiabetesRecord;
                    query += ";";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | DeleteOneDiabetesRecord", ex);
            }
        }
        internal void DeleteLastDiabetesRecord()
        {
            try
            {
                DiabetesRecord dr = GetLastDiabetesRecord();
                if (dr == null || dr.IdDiabetesRecord == null)
                    return;
                DeleteOneDiabetesRecord(dr); 
            }
            catch (Exception ex)
            {
                General.Log.Error("Sqlite_DiabetesRecord | DeleteLastDiabetesRecord", ex);
            }
        }
    }
}
