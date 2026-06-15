 using gamon;
using GlucoMan;
using Microsoft.Data.Sqlite;
using System.Data;
using System.Data.Common;
using System.Text;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        /// <summary>
        /// Data Access Layer: abstracts the access to dbms using to transfer Data 
        /// DbClasses and ADO db classes (ADO should be avoided, if possible) 
        /// </summary>
        internal string dbName;

        #region constructors
        /// <summary> 
        /// Constructor of DataLayer class that uses the default database of the program
        /// Assumes that the file exists.
        /// </summary>
        internal DL_Sqlite()
        {
            dbName = Common.PathAndFileDatabase;
            SQLitePCL.Batteries.Init();
            if (!System.IO.File.Exists(Common.PathAndFileDatabase))
            {
                // since the file doesn't exist yet, we create it:
                CreateNewDatabase(dbName);
            }
            MigrateDatabaseIfNeeded();
        }
        /// <summary>
        /// Constructor of DataLayer class that get from outside the databases to use
        /// Assumes that the file exists.
        /// </summary>
        internal DL_Sqlite(string PathAndFile)
        {
            // ???? is next if useful ????
            //if (!System.IO.File.Exists(PathAndFile))
            //{
            //    string err = @"[" + PathAndFile + " not in the current nor in the dev directory]";
            //    General.LogOfProgram.Error(err, null);
            //    throw new System.IO.FileNotFoundException(err);
            //}
            dbName = PathAndFile;
        }
        #endregion
        #region properties
        internal string NameAndPathDatabase
        {
            get { return dbName; }
            //set { nomeEPathDatabase = value; }
        }
        #endregion
        internal SqliteConnection Connect()
        {
            SqliteConnection connection;

            string connectionString = "Data Source=\"" + dbName + "\"; Cache = Shared; Mode = ReadWriteCreate";
            try
            {
                connection = new SqliteConnection(connectionString);
                connection.Open();
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error connecting to the database: " + ex.Message + "\r\nFile Sqlite>: " + dbName + " " + "\n", ex);
                connection = null;
            }
            return connection;
        }
        internal int nFieldDbDataReader(string NomeCampo, DbDataReader dr)
        {
            for (int i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i) == NomeCampo)
                {
                    return i;
                }
            }
            return -1;
        }
        internal void CompactDatabase()
        {
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                // compact the database 
                cmd.CommandText = "VACUUM;";
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            //Application.Exit();
        }
        internal int GetTableNextPrimaryKey(string Table, string KeyName)
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
        internal int GetTwoTablesNextPrimaryKey(
            string TableMaster, string TableSlave, string KeyMaster, string KeySlave, string ValueKeyMaster)
        {
            int nextId;
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT MAX(" + KeySlave + ") FROM " + TableSlave +
                    " ," + TableMaster + "" +
                    " WHERE " + TableMaster + "." + KeyMaster + "=" + ValueKeyMaster +
                    ";";
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
        internal bool CheckKeyExistence
            (string TableName, string KeyName, string KeyValue)
        {
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT " + KeyName + " FROM " + TableName +
                    " WHERE " + KeyName + "=" + SqliteSafe.String(KeyValue) +
                    ";";
                var keyResult = cmd.ExecuteScalar();
                cmd.Dispose();
                if (keyResult != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        /// <summary>
        /// Applies incremental schema changes to existing databases without data loss.
        /// Safe to run on every startup: each ALTER TABLE is guarded by FieldExists.
        /// </summary>
        private void MigrateDatabaseIfNeeded()
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();

                    // --- Foods table migrations ---
                    try
                    {
                        if (!FieldExists("Foods", "Barcode"))
                        {
                            cmd.CommandText = "ALTER TABLE Foods ADD COLUMN Barcode TEXT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added Barcode column to Foods table");
                        }
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Debug($"Migration: Barcode column may already exist or error: {ex.Message}");
                    }

                    try
                    {
                        if (!FieldExists("Foods", "IsRaw"))
                        {
                            cmd.CommandText = "ALTER TABLE Foods ADD COLUMN IsRaw TINYINT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added IsRaw column to Foods table");
                        }
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Debug($"Migration: IsRaw column may already exist or error: {ex.Message}");
                    }

                    try
                    {
                        if (!FieldExists("Foods", "RawCookedRatio"))
                        {
                            cmd.CommandText = "ALTER TABLE Foods ADD COLUMN RawCookedRatio DOUBLE;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added RawCookedRatio column to Foods table");
                        }
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Debug($"Migration: RawCookedRatio column may already exist or error: {ex.Message}");
                    }

                    // --- CategoriesOfFood table (create if not exists) ---
                    try
                    {
                        cmd.CommandText = "CREATE TABLE IF NOT EXISTS CategoriesOfFood " +
                            "(IdCategoryOfFood INTEGER PRIMARY KEY, Name TEXT, Description TEXT);";
                        cmd.ExecuteNonQuery();
                        General.LogOfProgram?.Debug("Migration: CategoriesOfFood table exists or was created");
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Failed to create CategoriesOfFood table", ex);
                    }

                    // --- Manufacturers table (create if not exists) ---
                    try
                    {
                        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Manufacturers " +
                            "(IdManufacturer INTEGER PRIMARY KEY, Name TEXT, Description TEXT);";
                        cmd.ExecuteNonQuery();
                        General.LogOfProgram?.Debug("Migration: Manufacturers table exists or was created");
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Failed to create Manufacturers table", ex);
                    }

                    // --- Parameters table migrations ---
                    try
                    {
                        if (!FieldExists("Parameters", "FatSecret_ClientId"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_ClientId TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                        if (!FieldExists("Parameters", "FatSecret_ClientSecret"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_ClientSecret TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                        if (!FieldExists("Parameters", "FatSecret_Language"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_Language TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                        if (!FieldExists("Parameters", "FatSecret_Region"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_Region TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                        if (!FieldExists("Parameters", "FatSecret_ConsumerSecret"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_ConsumerSecret TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                        if (!FieldExists("Parameters", "FatSecret_OAuth1Token"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_OAuth1Token TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                        if (!FieldExists("Parameters", "FatSecret_OAuth1TokenSecret"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN FatSecret_OAuth1TokenSecret TEXT;";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Error adding Parameters columns", ex);
                    }

                    // --- TimeZone columns migrations ---
                    try
                    {
                        if (!FieldExists("Parameters", "Time_CurrentTimeZone"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN Time_CurrentTimeZone REAL;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added Time_CurrentTimeZone column to Parameters table");
                        }
                        // Legacy: add old TimeZone column if absent (databases created before rename)
                        if (!FieldExists("Parameters", "TimeZone") && !FieldExists("Parameters", "UtcOffset"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN TimeZone INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added TimeZone column to Parameters table");
                        }
                        if (!FieldExists("GlucoseRecords", "TimeZone") && !FieldExists("GlucoseRecords", "UtcOffset"))
                        {
                            cmd.CommandText = "ALTER TABLE GlucoseRecords ADD COLUMN TimeZone INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added TimeZone column to GlucoseRecords table");
                        }
                        if (!FieldExists("Injections", "TimeZone") && !FieldExists("Injections", "UtcOffset"))
                        {
                            cmd.CommandText = "ALTER TABLE Injections ADD COLUMN TimeZone INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added TimeZone column to Injections table");
                        }
                        if (!FieldExists("Meals", "TimeZone") && !FieldExists("Meals", "UtcOffset"))
                        {
                            cmd.CommandText = "ALTER TABLE Meals ADD COLUMN TimeZone INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added TimeZone column to Meals table");
                        }
                        // Rename TimeZone -> UtcOffset on all tables that still use the old name
                        if (FieldExists("Parameters", "TimeZone"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters RENAME COLUMN TimeZone TO UtcOffset;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Renamed TimeZone -> UtcOffset in Parameters table");
                        }
                        if (FieldExists("GlucoseRecords", "TimeZone"))
                        {
                            cmd.CommandText = "ALTER TABLE GlucoseRecords RENAME COLUMN TimeZone TO UtcOffset;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Renamed TimeZone -> UtcOffset in GlucoseRecords table");
                        }
                        if (FieldExists("Injections", "TimeZone"))
                        {
                            cmd.CommandText = "ALTER TABLE Injections RENAME COLUMN TimeZone TO UtcOffset;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Renamed TimeZone -> UtcOffset in Injections table");
                        }
                        if (FieldExists("Meals", "TimeZone"))
                        {
                            cmd.CommandText = "ALTER TABLE Meals RENAME COLUMN TimeZone TO UtcOffset;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Renamed TimeZone -> UtcOffset in Meals table");
                        }
                        // Convert UtcOffset from INTEGER to REAL on all tables to support fractional
                        // offsets (e.g. India +5.5, Nepal +5.75). SQLite has no ALTER COLUMN, so we
                        // add a REAL column, copy the data, drop the old INTEGER column, then rename.
                        foreach (var tbl in new[] { "GlucoseRecords", "Injections", "Meals", "Parameters" })
                        {
                            if (FieldExists(tbl, "UtcOffset") && !FieldExists(tbl, "UtcOffset_real"))
                            {
                                cmd.CommandText = $"ALTER TABLE {tbl} ADD COLUMN UtcOffset_real REAL;";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = $"UPDATE {tbl} SET UtcOffset_real = CAST(UtcOffset AS REAL);";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = $"ALTER TABLE {tbl} DROP COLUMN UtcOffset;";
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = $"ALTER TABLE {tbl} RENAME COLUMN UtcOffset_real TO UtcOffset;";
                                cmd.ExecuteNonQuery();
                                General.LogOfProgram?.Debug($"Migration: Converted UtcOffset INTEGER -> REAL in {tbl}");
                            }
                        }
                        // Add Time_IsDaylightSavingTime and Time_CountryName if missing
                        if (!FieldExists("Parameters", "Time_IsDaylightSavingTime"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN Time_IsDaylightSavingTime INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added Time_IsDaylightSavingTime column to Parameters table");
                        }
                        if (!FieldExists("Parameters", "Time_CountryName"))
                        {
                            cmd.CommandText = "ALTER TABLE Parameters ADD COLUMN Time_CountryName VARCHAR(64);";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added Time_CountryName column to Parameters table");
                        }
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Error adding TimeZone columns", ex);
                    }

                    // --- InsulinDrugs table (create if not exists) ---
                    try
                    {
                        cmd.CommandText = "CREATE TABLE IF NOT EXISTS InsulinDrugs " +
                            "(IdInsulinDrug INT NOT NULL, Name VARCHAR(32), Manufacturer VARCHAR(32), " +
                            "TypeOfInsulinAction INTEGER, DurationInHours DOUBLE, OnsetTimeInHours DOUBLE, " +
                            "PeakTimeInHours DOUBLE, PRIMARY KEY(IdInsulinDrug));";
                        cmd.ExecuteNonQuery();

                        // Seed default insulin drugs if the table is empty
                        cmd.CommandText = "SELECT COUNT(*) FROM InsulinDrugs;";
                        long count = (long)(cmd.ExecuteScalar() ?? 0L);
                        if (count == 0)
                        {
                            cmd.CommandText =
                                "INSERT INTO InsulinDrugs VALUES (1,'Humalog','Lilly',20,4.0,0.25,2.0);" +
                                "INSERT INTO InsulinDrugs VALUES (2,'Toujeo','Sanofi',40,36.0,3.5,0.0);" +
                                "INSERT INTO InsulinDrugs VALUES (3,'Lispro','Lilly',20,4.5,0.3333333,2.0);" +
                                "INSERT INTO InsulinDrugs VALUES (4,'Fiasp','Novo Nordisk',20,3.0,0.03,1.5);" +
                                "INSERT INTO InsulinDrugs VALUES (5,'Lantus','Sanofi',40,24.0,4.0,0.0);";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Seeded default InsulinDrugs");
                        }

                        General.LogOfProgram?.Debug("Migration: InsulinDrugs table exists or was created");
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Failed to create InsulinDrugs table", ex);
                    }

                    // --- Alarms table migrations (schema refactor 2026-05) ---
                    // Old schema: ValidTimeAfterStart INTEGER, no RepetitionTime, no RingingState
                    // New schema: StartupGraceWindow INTEGER, RepetitionTime INTEGER, RingingState TINYINT
                    try
                    {
                        // CREATE the Alarms table if it doesn't exist yet (fresh installs)
                        cmd.CommandText = "CREATE TABLE IF NOT EXISTS Alarms " +
                            "(IdAlarm INT NOT NULL, ReminderText TEXT, TimeStart DATETIME, " +
                            "NextTriggerTime DATETIME, IsDisabled TINYINT, StartupGraceWindow INTEGER, " +
                            "RingingState TINYINT, Duration INTEGER, RepetitionTime INTEGER, Interval INTEGER, " +
                            "IsPlaying TINYINT, EnablePlaySoundFile TINYINT, SoundFilePath TEXT, " +
                            "RepeatCount INTEGER, MaxRepeatCount INTEGER, LastTriggerTime DATETIME, " +
                            "TriggeredCount INTEGER, DoVibrate TINYINT, PRIMARY KEY (IdAlarm));";
                        cmd.ExecuteNonQuery();

                        // Add new columns when upgrading from old schema
                        if (!FieldExists("Alarms", "StartupGraceWindow"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN StartupGraceWindow INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added StartupGraceWindow column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "RepetitionTime"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN RepetitionTime INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added RepetitionTime column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "RingingState"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN RingingState TINYINT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added RingingState column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "ReminderText"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN ReminderText TEXT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added ReminderText column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "NextTriggerTime"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN NextTriggerTime DATETIME;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added NextTriggerTime column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "IsDisabled"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN IsDisabled TINYINT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added IsDisabled column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "IsPlaying"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN IsPlaying TINYINT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added IsPlaying column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "EnablePlaySoundFile"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN EnablePlaySoundFile TINYINT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added EnablePlaySoundFile column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "SoundFilePath"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN SoundFilePath TEXT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added SoundFilePath column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "RepeatCount"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN RepeatCount INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added RepeatCount column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "MaxRepeatCount"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN MaxRepeatCount INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added MaxRepeatCount column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "LastTriggerTime"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN LastTriggerTime DATETIME;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added LastTriggerTime column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "TriggeredCount"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN TriggeredCount INTEGER;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added TriggeredCount column to Alarms table");
                        }
                        if (!FieldExists("Alarms", "DoVibrate"))
                        {
                            cmd.CommandText = "ALTER TABLE Alarms ADD COLUMN DoVibrate TINYINT;";
                            cmd.ExecuteNonQuery();
                            General.LogOfProgram?.Debug("Migration: Added DoVibrate column to Alarms table");
                        }
                        // Note: SQLite does not support DROP COLUMN in older versions.
                        // ValidTimeAfterStart (old column) is left in place on old databases
                        // and simply ignored by the new code.
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Error migrating Alarms table", ex);
                    }

                    // --- Clean up duplicate global "g" units ---
                    // A bug created a new global "g" unit every time a new food was saved.
                    // Keep only the one with the smallest IdUnitOfFood.
                    try
                    {
                        cmd.CommandText = "DELETE FROM UnitsOfFood" +
                            " WHERE Symbol='g' AND (IdFood IS NULL OR IdFood=0)" +
                            " AND IdUnitOfFood NOT IN" +
                            " (SELECT MIN(IdUnitOfFood) FROM UnitsOfFood" +
                            "  WHERE Symbol='g' AND (IdFood IS NULL OR IdFood=0));";
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Error("Migration: Error cleaning duplicate g units", ex);
                    }

                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("DL_Sqlite | MigrateDatabaseIfNeeded", ex);
            }
        }

        internal bool FieldExists(string TableName, string FieldName)
        {
            // Use PRAGMA for SQLite-specific, reliable column detection
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = $"PRAGMA table_info({TableName});";
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // PRAGMA table_info returns: cid, name, type, notnull, dflt_value, pk
                            if (reader["name"].ToString() == FieldName)
                                return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        internal void BackupAllDataToTsv()
        {
            BackupTableTsv("Students");
            BackupTableTsv("StudentsPhotos");
            BackupTableTsv("StudentsPhotos_Students");
            BackupTableTsv("Classes_Students");
            BackupTableTsv("Grades");
        }
        internal void BackupAllDataToXml()
        {
            BackupTableXml("Students");
            BackupTableXml("StudentsPhotos");
            BackupTableXml("StudentsPhotos_Students");
            BackupTableXml("Classes_Students");
            BackupTableXml("Grades");
        }
        internal void RestoreAllDataFromTsv(bool MustErase)
        {
            RestoreTableTsv("Students", MustErase);
            RestoreTableTsv("StudentsPhotos", MustErase);
            RestoreTableTsv("StudentsPhotos_Students", MustErase);
            RestoreTableTsv("Classes_Students", MustErase);
            RestoreTableTsv("Grades", MustErase);
        }
        internal void RestoreAllDataFromXml(bool MustErase)
        {
            RestoreTableXml("Students", MustErase);
            RestoreTableXml("StudentsPhotos", MustErase);
            RestoreTableXml("StudentsPhotos_Students", MustErase);
            RestoreTableXml("Classes_Students", MustErase);
            RestoreTableXml("Grades", MustErase);
        }
        internal void BackupTableTsv(string TableName)
        {
            DbDataReader dRead;
            DbCommand cmd;
            string fileContent = "";

            using (DbConnection conn = Connect())
            {
                string query = "SELECT *" +
                    " FROM " + TableName + " ";
                cmd = new SqliteCommand(query);
                cmd.Connection = conn;
                dRead = cmd.ExecuteReader();
                int y = 0;
                while (dRead.Read())
                {
                    // field names only in first row 
                    if (y == 0)
                    {
                        string types = "";
                        for (int i = 0; i < dRead.FieldCount; i++)
                        {
                            fileContent += "\"" + dRead.GetName(i) + "\"\t";
                            types += "\"" + Safe.String(dRead.GetDataTypeName(i)) + "\"\t";
                        }
                        fileContent = fileContent.Substring(0, fileContent.Length - 1) + "\r\n";
                        fileContent += types.Substring(0, types.Length - 1) + "\r\n";
                    }
                    // field values
                    string values = "";
                    if (dRead.GetValue(0) != null)
                    {
                        Console.Write(dRead.GetValue(0));
                        for (int i = 0; i < dRead.FieldCount; i++)
                        {
                            values += "\"" + Safe.String(dRead.GetValue(i).ToString()) + "\"\t";
                        }
                        fileContent += values.Substring(0, values.Length - 1) + "\r\n";
                    }
                    else
                    {

                    }
                    y++;
                }
                TextFile.StringToFile(Common.PathDatabase + "\\" + TableName + ".tsv", fileContent, false);
                dRead.Dispose();
                cmd.Dispose();
            }
        }
        internal void BackupTableXml(string TableName)
        {
            DataAdapter dAdapt;
            DataSet dSet = new DataSet();
            DataTable t;
            string query = "SELECT *" +
                    " FROM " + TableName + ";";

            using (DbConnection conn = Connect())
            {
                // !!!! sunstitute DataAdapter

                //dAdapt = new DataAdapter (query, (SqliteConnection)conn);
                //dSet = new DataSet("GetTable");
                //dAdapt.Fill(dSet);
                //t = dSet.Tables[0];

                //t.WriteXml(Common.PathDatabase + "\\" + TableName + ".xml",
                //    XmlWriteMode.WriteSchema);

                //dAdapt.Dispose();
                //dSet.Dispose();
            }
        }
        internal void RestoreTableTsv(string TableName, bool EraseBefore)
        {
            List<string> fieldNames;
            List<string> fieldTypes = new List<string>();
            //string[,] dati = FileDiTesto.FileInMatrice(Common.PathDatabase +
            //    "\\" + TableName + ".tsv", '\t',
            //    out fieldsNames, out fieldTypes);
            string dati = TextFile.FileToString(Common.PathDatabase +
                "\\" + TableName + ".tsv");
            if (dati is null)
                return;
            using (DbConnection conn = Connect())
            {
                DbCommand cmd = conn.CreateCommand();
                if (EraseBefore)
                {
                    // first: erases existing rows in the table
                    cmd.CommandText += "DELETE FROM " + TableName + ";";
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw new Exception("Append of table records to an existing table id not implemented yet");
                    //return; 
                }
                string fieldsString = " (";
                string valuesString;
                int fieldsCount = 0;

                int index = 0;
                string fieldName = "";
                while (index < dati.Length)
                {
                    // parse first line: field names
                    fieldNames = new List<string>();
                    do
                    {
                        if (dati[index++] != '\"')
                            return; // error! 
                        fieldName = "";
                        while (dati[index] != '\"')
                        {
                            fieldName += dati[index++];
                        }
                        fieldNames.Add(fieldName);
                        fieldsString += fieldName + ",";
                        fieldsCount++;
                        if (dati[++index] != '\t' && dati[index] != '\r')
                            return; // ERROR!
                    } while (dati[++index] != '\r');
                    index++; // void line feed

                    // parse second line: field types
                    string fieldType = "";
                    while (dati[index] != '\r')
                    {
                        while (dati[index] != '\"')
                        {
                            fieldType += dati[index++];
                        }
                        fieldTypes.Add(fieldType);
                        fieldsString += fieldName + ",";
                        fieldsCount++;
                    }
                    index++; // void line feed

                    // parse the rest of the rows: values
                    string fieldValue = "";
                    while (dati[index] != '\r')
                    {
                        while (dati[index] != '\"')
                        {
                            fieldType += dati[index++];
                        }
                        fieldTypes.Add(fieldType);
                        fieldsString += fieldName + ",";
                        fieldsCount++;
                    }
                }
                //for (int col = 0; col < dati.GetLength(1); col++)
                //{
                //    if (fieldNames[col] != "")
                //    {
                //        fieldsString += fieldNames[col] + ",";
                //        fieldsCount++; 
                //    }
                //}
                //fieldsString = fieldsString.Substring(0, fieldsString.Length - 1);
                //fieldsString += ")";
                //for (int row = 0; row < dati.GetLength(0); row++)
                //{
                //    valuesString = " Values (";
                //    for (int col = 0; col < fieldsCount; col++)
                //    {
                //        if (fieldNames[col] != "")
                //        {
                //            if (fieldTypes[col].IndexOf("VARCHAR") >= 0)
                //                valuesString += "" + SqliteSafe.String(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("INT") >= 0)
                //                valuesString +=  SqliteSafe.Int(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("REAL") >= 0)
                //                valuesString += SqlFloat(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("FLOAT") >= 0)
                //                valuesString += SqlFloat(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("DATE") >= 0)
                //                valuesString += SqliteSafe.Date((dati[row, col]) + ",";
                //        }
                //    }
                //    valuesString = valuesString.Substring(0, valuesString.Length - 1);
                //    valuesString += ")";
                //    cmd.CommandText = "INSERT INTO " + TableName +
                //                fieldsString +
                //                valuesString;
                //    //" WHERE " + fieldsNames[0] + "=";
                //    //if (fieldTypes[0].IndexOf("VARCHAR") >= 0)
                //    //    cmd.CommandText += "'" + StringSql(dati[row, 0]) + "'";
                //    //else
                //    //    cmd.CommandText += StringSql(dati[row, 0]);
                //    cmd.CommandText += ";";
                //    cmd.ExecuteNonQuery();
                //}
                //cmd.Dispose();
            }
        }
        internal void RestoreTableXml(string TableName, bool EraseBefore)
        {
            DataSet dSet = new DataSet();
            DataTable t = null;
            dSet.ReadXml(Common.PathDatabase + "\\" + TableName + ".xml", XmlReadMode.ReadSchema);
            t = dSet.Tables[0];
            if (t.Rows.Count == 0)
                return;
            using (DbConnection conn = Connect())
            {
                DbCommand cmd;
                cmd = conn.CreateCommand();
                if (EraseBefore)
                {
                    cmd.CommandText = "DELETE FROM " + TableName + ";";
                    cmd.ExecuteNonQuery();
                }
                cmd.CommandText = "INSERT INTO " + TableName + "(";
                // column names
                DataRow r = t.Rows[0];
                foreach (DataColumn c in t.Columns)
                {
                    cmd.CommandText += c.ColumnName + ",";
                }
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1);
                cmd.CommandText += ")VALUES";
                // row values
                foreach (DataRow row in t.Rows)
                {
                    cmd.CommandText += "(";
                    foreach (DataColumn c in t.Columns)
                    {
                        switch (Type.GetTypeCode(c.DataType))
                        {
                            case TypeCode.String:
                            case TypeCode.Char:
                                {
                                    cmd.CommandText += "" + SqliteSafe.String(row[c.ColumnName].ToString()) + ",";
                                    break;
                                }
                                ;
                            case TypeCode.DateTime:
                                {
                                    DateTime? d = Safe.DateTime(row[c.ColumnName]);
                                    cmd.CommandText += "'" +
                                        ((DateTime)(d)).ToString("yyyy-MM-dd_HH.mm.ss") + "',";
                                    break;
                                }
                            default:
                                {
                                    if (!(row[c.ColumnName] is DBNull))
                                        cmd.CommandText += row[c.ColumnName] + ",";
                                    else
                                        cmd.CommandText += "0,";
                                    break;
                                }
                        }
                    }
                    cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1);
                    cmd.CommandText += "),";
                }
                cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1);
                cmd.CommandText += ";";
                cmd.ExecuteNonQuery();
                dSet.Dispose();
                t.Dispose();
                cmd.Dispose();
            }
        }
        internal override bool DeleteDatabase()
        {
            try
            {
                // simple deletion wasn't working for lock on file
                //File.Delete(dbName);

                string[] tablesNames;
                // erase all the tables of current database 
                using (DbConnection conn = Connect())
                {
                    // Disabilita temporaneamente i vincoli di chiave esterna
                    using (DbCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "PRAGMA foreign_keys = OFF;";
                        command.ExecuteNonQuery();
                    }
                    // Recupera i nomi delle tabelle
                    var tableNames = new StringBuilder();
                    using (DbCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name NOT LIKE 'sqlite_%';";
                        command.ExecuteNonQuery();
                        // add names to list od tables
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tableNames.AppendLine($"DROP TABLE IF EXISTS \"{reader["name"]}\";");
                            }
                        }
                    }
                    // Esegui i comandi DROP TABLE
                    using (DbCommand command = conn.CreateCommand())
                    {
                        command.CommandText = tableNames.ToString();
                        command.ExecuteNonQuery();
                    }
                    // abilita i vincoli di chiave esterna
                    using (DbCommand command = conn.CreateCommand())
                    {
                        command.CommandText = "PRAGMA foreign_keys = ON;";
                        command.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | DeleteDatabase", ex);
                return false;
            }
        }
        internal override long? SaveParameter(string FieldName, string FieldValue, int? Key = null)
        {
            long? idOfRecord = null;
            try
            {
                using (DbConnection conn = Connect())
                {
                    // read table Parameters, to see if it has rows
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT COUNT(*) FROM Parameters";
                    cmd.CommandText = query;
                    long? count = (long)cmd.ExecuteScalar();

                    string whereClause = "";
                    double dummy;
                    if (FieldValue != null)
                    {
                        if (count == 0)
                        {
                            // make a new first row with IdParameters = 1
                            query = "INSERT INTO Parameters (IdParameters,";
                            query += FieldName;
                            query += ")VALUES(1,";
                            if (double.TryParse(FieldValue, out dummy))
                            {
                                query += SqliteSafe.Double(dummy);
                            }
                            else
                            {
                                query += "'" + FieldValue + "'";
                            }
                            query += ");";
                            idOfRecord = 1;
                        }
                        // count > 0 
                        else
                        {
                            if (Key == null || Key == 0)
                            {
                                // no key given: we update the highest key already given 
                                cmd.CommandText = "SELECT MAX(IdParameters) FROM Parameters;";
                                long? maxKey = (long?)cmd.ExecuteScalar();
                                whereClause = " WHERE IdParameters=" + maxKey.ToString();
                                query = "UPDATE Parameters SET ";
                                if (double.TryParse(FieldValue, out dummy))
                                {
                                    query += FieldName + "=" + SqliteSafe.Double(dummy);
                                }
                                else
                                {
                                    query += FieldName + "=" + SqliteSafe.String(FieldValue);
                                }
                                idOfRecord = maxKey;
                            }
                            else
                            {   // user provided a key, if the key already has a row we must use that 
                                // we see if we already have a row with this id 
                                cmd.CommandText = "SELECT IdParameters FROM Parameters;";
                                int? index = (int?)cmd.ExecuteScalar();
                                if (index == Key) // if the index il already given 
                                {
                                    // updating existing row
                                    query = "UPDATE Parameters SET ";
                                    if (double.TryParse(FieldValue, out dummy))
                                    {
                                        query += FieldName + "=" + SqliteSafe.Double(dummy);
                                    }
                                    else
                                    {
                                        query += FieldName + "=" + "'" + FieldValue + "'";
                                    }
                                    whereClause = " WHERE IdParameters=" + Key;

                                    idOfRecord = Key;
                                }
                                else
                                {
                                    // making a new row with increased key
                                    query = "INSERT INTO Parameters (IdParameters,";
                                    query += FieldName;
                                    query += ")VALUES(1,";
                                    if (double.TryParse(FieldValue, out dummy))
                                    {
                                        query += SqliteSafe.Double(dummy);
                                    }
                                    else
                                    {
                                        query += "'" + FieldValue + "'";
                                    }
                                    query += ");";
                                    whereClause = " WHERE IdParameters=" + Key++;
                                    idOfRecord = Key;
                                }
                                idOfRecord = null;
                            }
                            query += whereClause + ";";
                        }
                        cmd.CommandText = query;
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                    return idOfRecord;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | SaveParameter", ex);
                return null;
            }
        }
        internal override string RestoreParameter(string FieldName, int? Key = null)
        {
            // read the parameter whose name is passed, in the LAST row of the table Parameters
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT ";
                    query += FieldName;
                    query += " FROM Parameters";
                    //string whereClause;
                    //if (Key == null)
                    //{
                    //    // no key between parameter: we use the highest key
                    //    cmd.CommandText = "SELECT MAX(IdParameters) FROM Parameters;";
                    //    int? maxKey = Safe.Int(cmd.ExecuteScalar());
                    //    //int? maxKey = NextKey("Parameters", "IdParameters");
                    //    whereClause = " WHERE IdParameters=" + maxKey;
                    //}
                    //else
                    //{
                    //    // the user passed a key; we use that
                    //    whereClause = " WHERE IdParameters=" + Key;
                    //}
                    //query += whereClause;
                    query += " ORDER BY IdParameters DESC LIMIT 1" +
                        ";";
                    cmd.CommandText = query;
                    string result = Safe.String(cmd.ExecuteScalar());
                    cmd.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | RestoreParameter", ex);
                return null;
            }
        }
        internal override Parameters GetParameters()
        {
            // read in parameters all the properties of the Parameters object
            // from THE LAST ROW of the table Parameters
            Parameters parameters = new Parameters();
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT * FROM Parameters ORDER BY IdParameters DESC LIMIT 1;";
                    using (DbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            parameters.Bolus_TargetGlucose = Safe.Int(dr["Bolus_TargetGlucose"]);
                            parameters.Bolus_GlucoseBeforeMeal = Safe.Int(dr["Bolus_GlucoseBeforeMeal"]);
                            parameters.Bolus_ChoToEat = Safe.Double(dr["Bolus_ChoToEat"]);
                            parameters.Bolus_ChoInsulinRatioBreakfast = Safe.Double(dr["Bolus_ChoInsulinRatioBreakfast"]);
                            parameters.Bolus_ChoInsulinRatioLunch = Safe.Double(dr["Bolus_ChoInsulinRatioLunch"]);
                            parameters.Bolus_ChoInsulinRatioDinner = Safe.Double(dr["Bolus_ChoInsulinRatioDinner"]);
                            parameters.Bolus_TotalDailyDoseOfInsulin = Safe.Double(dr["Bolus_TotalDailyDoseOfInsulin"]);
                            parameters.Bolus_InsulinCorrectionSensitivity = Safe.Double(dr["Bolus_InsulinCorrectionSensitivity"]);
                            parameters.Correction_TypicalBolusMorning = Safe.Double(dr["Correction_TypicalBolusMorning"]);
                            parameters.Correction_TypicalBolusMidday = Safe.Double(dr["Correction_TypicalBolusMidday"]);
                            parameters.Correction_TypicalBolusEvening = Safe.Double(dr["Correction_TypicalBolusEvening"]);
                            parameters.Correction_TypicalBolusNight = Safe.Double(dr["Correction_TypicalBolusNight"]);
                            parameters.Correction_FactorOfInsulinCorrectionSensitivity = Safe.Double(dr["Correction_FactorOfInsulinCorrectionSensitivity"]);
                            parameters.Hypo_GlucoseTarget = Safe.Double(dr["Hypo_GlucoseTarget"]);
                            parameters.Hypo_GlucoseLast = Safe.Double(dr["Hypo_GlucoseLast"]);
                            parameters.Hypo_GlucosePrevious = Safe.Double(dr["Hypo_GlucosePrevious"]);
                            parameters.Hypo_HourLast = Safe.Double(dr["Hypo_HourLast"]);
                            parameters.Hypo_HourPrevious = Safe.Double(dr["Hypo_HourPrevious"]);
                            parameters.Hypo_MinuteLast = Safe.Double(dr["Hypo_MinuteLast"]);
                            parameters.Hypo_MinutePrevious = Safe.Double(dr["Hypo_MinutePrevious"]);
                            parameters.Hypo_AlarmAdvanceTime = Safe.Double(dr["Hypo_AlarmAdvanceTime"]);
                            parameters.Hypo_FutureSpanMinutes = Safe.Double(dr["Hypo_FutureSpanMinutes"]);
                            parameters.Hit_ChoAlreadyTaken = Safe.Double(dr["Hit_ChoAlreadyTaken"]);
                            parameters.Hit_ChoOfFood = Safe.Double(dr["Hit_ChoOfFood"]);
                            parameters.Hit_TargetCho = Safe.Double(dr["Hit_TargetCho"]);
                            parameters.Hit_NameOfFood = Safe.String(dr["Hit_NameOfFood"]);
                            parameters.FoodInMeal_ChoGrams = Safe.Double(dr["FoodInMeal_ChoGrams"]);
                            parameters.FoodInMeal_QuantityGrams = Safe.Double(dr["FoodInMeal_QuantityGrams"]);
                            parameters.FoodInMeal_CarbohydratesPercent = Safe.Double(dr["FoodInMeal_CarbohydratesPercent"]);
                            parameters.FoodInMeal_Name = Safe.String(dr["FoodInMeal_Name"]);
                            parameters.FoodInMeal_AccuracyOfChoEstimate = Safe.Double(dr["FoodInMeal_AccuracyOfChoEstimate"]);
                            parameters.Meal_ChoGrams = Safe.Double(dr["Meal_ChoGrams"]);
                            // parameters managed by the SettingsPage
                            parameters.IdParameters = Safe.Int(dr["IdParameters"]);
                            parameters.Timestamp = Safe.DateTime(dr["Timestamp"]);
                            parameters.IdInsulinDrug_Short = Safe.Int(dr["Insulin_Short_Id"]);
                            parameters.IdInsulinDrug_Long = Safe.Int(dr["Insulin_Long_Id"]);
                            parameters.Meal_Breakfast_StartTime_Hours = Safe.Double(dr["Meal_Breakfast_StartTime_Hours"]);
                            parameters.Meal_Breakfast_EndTime_Hours = Safe.Double(dr["Meal_Breakfast_EndTime_Hours"]);
                            parameters.Meal_Lunch_StartTime_Hours = Safe.Double(dr["Meal_Lunch_StartTime_Hours"]);
                            parameters.Meal_Lunch_EndTime_Hours = Safe.Double(dr["Meal_Lunch_EndTime_Hours"]);
                            parameters.Meal_Dinner_StartTime_Hours = Safe.Double(dr["Meal_Dinner_StartTime_Hours"]);
                            parameters.Meal_Dinner_EndTime_Hours = Safe.Double(dr["Meal_Dinner_EndTime_Hours"]);
                            parameters.MonthsOfDataShownInTheGrids = Safe.Double(dr["MonthsOfDataShownInTheGrids"]);
                            parameters.Time_CurrentTimeZone = Safe.Double(dr["Time_CurrentTimeZone"]);
                            try { parameters.Time_IsDaylightSavingTime = Safe.Int(dr["Time_IsDaylightSavingTime"]) == 1; } catch { parameters.Time_IsDaylightSavingTime = false; }
                            try { parameters.Time_CountryName = Safe.String(dr["Time_CountryName"]); } catch { parameters.Time_CountryName = null; }
                            parameters.UtcOffset = Safe.Double(dr["UtcOffset"]);
                        }
                    }
                    cmd.Dispose();
                }
                return parameters;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | GetSettingsPageParameters", ex);
                return null;
            }
        }
        internal override int? SaveAllParameters(Parameters parameters,
            bool saveInANewRowWithTimestamp)
        {
            try
            {
                if (parameters.IdParameters == null || parameters.IdParameters == 0
                    || saveInANewRowWithTimestamp)
                {
                    parameters.IdParameters = GetTableNextPrimaryKey("Parameters", "IdParameters");
                    // INSERT new record in the table
                    parameters.Timestamp = DateTime.UtcNow;
                    parameters.UtcOffset = Common.CurrentTimeZone;
                    InsertAllParameters(parameters);
                }
                else
                    UpdateAllParameters(parameters);
                return parameters.IdParameters;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("DataLayer | SaveSettingsPageParameters", ex);
                return null;
            }
        }
        internal override void UpdateAllParameters(Parameters parameters)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "UPDATE Parameters SET " +
                        "Timestamp=@Timestamp, " +
                        "Insulin_Short_Id=@Insulin_Short_Id, " +
                        "Insulin_Long_Id=@Insulin_Long_Id, " +
                        "Meal_Breakfast_StartTime_Hours=@Meal_Breakfast_StartTime, " +
                        "Meal_Breakfast_EndTime_Hours=@Meal_Breakfast_EndTime, " +
                        "Meal_Lunch_StartTime_Hours=@Meal_Lunch_StartTime, " +
                        "Meal_Lunch_EndTime_Hours=@Meal_Lunch_EndTime, " +
                        "Meal_Dinner_StartTime_Hours=@Meal_Dinner_StartTime, " +
                        "Meal_Dinner_EndTime_Hours=@Meal_Dinner_EndTime, " +
                        "Bolus_TargetGlucose=@Bolus_TargetGlucose, " +
                        "Bolus_GlucoseBeforeMeal=@Bolus_GlucoseBeforeMeal, " +
                        "Bolus_ChoToEat=@Bolus_ChoToEat, " +
                        "Bolus_ChoInsulinRatioBreakfast=@Bolus_ChoInsulinRatioBreakfast, " +
                        "Bolus_ChoInsulinRatioLunch=@Bolus_ChoInsulinRatioLunch, " +
                        "Bolus_ChoInsulinRatioDinner=@Bolus_ChoInsulinRatioDinner, " +
                        "Bolus_TotalDailyDoseOfInsulin=@Bolus_TotalDailyDoseOfInsulin, " +
                        "Bolus_InsulinCorrectionSensitivity=@Bolus_InsulinCorrectionSensitivity, " +
                        "Correction_TypicalBolusMorning=@Correction_TypicalBolusMorning, " +
                        "Correction_TypicalBolusMidday=@Correction_TypicalBolusMidday, " +
                        "Correction_TypicalBolusEvening=@Correction_TypicalBolusEvening, " +
                        "Correction_TypicalBolusNight=@Correction_TypicalBolusNight, " +
                        "Correction_FactorOfInsulinCorrectionSensitivity=@Correction_FactorOfInsulinCorrectionSensitivity, " +
                        "Hypo_GlucoseTarget=@Hypo_GlucoseTarget, " +
                        "Hypo_GlucoseLast=@Hypo_GlucoseLast, " +
                        "Hypo_GlucosePrevious=@Hypo_GlucosePrevious, " +
                        "Hypo_HourLast=@Hypo_HourLast, " +
                        "Hypo_HourPrevious=@Hypo_HourPrevious, " +
                        "Hypo_MinuteLast=@Hypo_MinuteLast, " +
                        "Hypo_MinutePrevious=@Hypo_MinutePrevious, " +
                        "Hypo_AlarmAdvanceTime=@Hypo_AlarmAdvanceTime, " +
                        "Hypo_FutureSpanMinutes=@Hypo_FutureSpanMinutes, " +
                        "Hit_ChoAlreadyTaken=@Hit_ChoAlreadyTaken, " +
                        "Hit_ChoOfFood=@Hit_ChoOfFood, " +
                        "Hit_TargetCho=@Hit_TargetCho, " +
                        "Hit_NameOfFood=@Hit_NameOfFood, " +
                        "FoodInMeal_ChoGrams=@FoodInMeal_ChoGrams, " +
                        "FoodInMeal_QuantityGrams=@FoodInMeal_QuantityGrams, " +
                        "FoodInMeal_CarbohydratesPercent=@FoodInMeal_CarbohydratesPercent, " +
                        "FoodInMeal_Name=@FoodInMeal_Name, " +
                        "FoodInMeal_AccuracyOfChoEstimate=@FoodInMeal_AccuracyOfChoEstimate, " +
                        "Meal_ChoGrams=@Meal_ChoGrams, " +
                        "MonthsOfDataShownInTheGrids=@MonthsOfDataShownInTheGrids, " +
                        "Time_CurrentTimeZone=@Time_CurrentTimeZone, " +
                        "Time_IsDaylightSavingTime=@Time_IsDaylightSavingTime, " +
                        "Time_CountryName=@Time_CountryName, " +
                        "UtcOffset=@UtcOffset " +
                        $"WHERE IdParameters={parameters.IdParameters};";

                    cmd.Parameters.Add(new SqliteParameter("@Timestamp", parameters.Timestamp ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Insulin_Short_Id", parameters.IdInsulinDrug_Short ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Insulin_Long_Id", parameters.IdInsulinDrug_Long ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Breakfast_StartTime", parameters.Meal_Breakfast_StartTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Breakfast_EndTime", parameters.Meal_Breakfast_EndTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Lunch_StartTime", parameters.Meal_Lunch_StartTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Lunch_EndTime", parameters.Meal_Lunch_EndTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Dinner_StartTime", parameters.Meal_Dinner_StartTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Dinner_EndTime", parameters.Meal_Dinner_EndTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_TargetGlucose", parameters.Bolus_TargetGlucose ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_GlucoseBeforeMeal", parameters.Bolus_GlucoseBeforeMeal ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoToEat", parameters.Bolus_ChoToEat ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoInsulinRatioBreakfast", parameters.Bolus_ChoInsulinRatioBreakfast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoInsulinRatioLunch", parameters.Bolus_ChoInsulinRatioLunch ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoInsulinRatioDinner", parameters.Bolus_ChoInsulinRatioDinner ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_TotalDailyDoseOfInsulin", parameters.Bolus_TotalDailyDoseOfInsulin ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_InsulinCorrectionSensitivity", parameters.Bolus_InsulinCorrectionSensitivity ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusMorning", parameters.Correction_TypicalBolusMorning ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusMidday", parameters.Correction_TypicalBolusMidday ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusEvening", parameters.Correction_TypicalBolusEvening ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusNight", parameters.Correction_TypicalBolusNight ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_FactorOfInsulinCorrectionSensitivity", parameters.Correction_FactorOfInsulinCorrectionSensitivity ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_GlucoseTarget", parameters.Hypo_GlucoseTarget ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_GlucoseLast", parameters.Hypo_GlucoseLast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_GlucosePrevious", parameters.Hypo_GlucosePrevious ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_HourLast", parameters.Hypo_HourLast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_HourPrevious", parameters.Hypo_HourPrevious ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_MinuteLast", parameters.Hypo_MinuteLast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_MinutePrevious", parameters.Hypo_MinutePrevious ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_AlarmAdvanceTime", parameters.Hypo_AlarmAdvanceTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_FutureSpanMinutes", parameters.Hypo_FutureSpanMinutes ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_ChoAlreadyTaken", parameters.Hit_ChoAlreadyTaken ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_ChoOfFood", parameters.Hit_ChoOfFood ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_TargetCho", parameters.Hit_TargetCho ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_NameOfFood", parameters.Hit_NameOfFood ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_ChoGrams", parameters.FoodInMeal_ChoGrams ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_QuantityGrams", parameters.FoodInMeal_QuantityGrams ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_CarbohydratesPercent", parameters.FoodInMeal_CarbohydratesPercent ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_Name", parameters.FoodInMeal_Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_AccuracyOfChoEstimate", parameters.FoodInMeal_AccuracyOfChoEstimate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_ChoGrams", parameters.Meal_ChoGrams ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@MonthsOfDataShownInTheGrids", parameters.MonthsOfDataShownInTheGrids ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Time_CurrentTimeZone", parameters.Time_CurrentTimeZone ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Time_IsDaylightSavingTime", parameters.Time_IsDaylightSavingTime.HasValue ? (object)(parameters.Time_IsDaylightSavingTime.Value ? 1 : 0) : DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Time_CountryName", parameters.Time_CountryName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@UtcOffset", parameters.UtcOffset ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("DataLayer|SettingsPageParameters", ex);
            }
        }
        internal override void InsertAllParameters(Parameters parameters)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Parameters (" +
                        "IdParameters, Timestamp, Insulin_Short_Id, Insulin_Long_Id, " +
                        "Meal_Breakfast_StartTime_Hours, Meal_Breakfast_EndTime_Hours, " +
                        "Meal_Lunch_StartTime_Hours, Meal_Lunch_EndTime_Hours, " +
                        "Meal_Dinner_StartTime_Hours, Meal_Dinner_EndTime_Hours, " +
                        "Bolus_TargetGlucose, Bolus_GlucoseBeforeMeal, Bolus_ChoToEat, " +
                        "Bolus_ChoInsulinRatioBreakfast, Bolus_ChoInsulinRatioLunch, Bolus_ChoInsulinRatioDinner, " +
                        "Bolus_TotalDailyDoseOfInsulin, Bolus_InsulinCorrectionSensitivity, " +
                        "Correction_TypicalBolusMorning, Correction_TypicalBolusMidday, Correction_TypicalBolusEvening, " +
                        "Correction_TypicalBolusNight, Correction_FactorOfInsulinCorrectionSensitivity, " +
                        "Hypo_GlucoseTarget, Hypo_GlucoseLast, Hypo_GlucosePrevious, " +
                        "Hypo_HourLast, Hypo_HourPrevious, Hypo_MinuteLast, Hypo_MinutePrevious, " +
                        "Hypo_AlarmAdvanceTime, Hypo_FutureSpanMinutes, Hit_ChoAlreadyTaken, " +
                        "Hit_ChoOfFood, Hit_TargetCho, Hit_NameOfFood, FoodInMeal_ChoGrams, " +
                        "FoodInMeal_QuantityGrams, FoodInMeal_CarbohydratesPercent, FoodInMeal_Name, " +
                        "FoodInMeal_AccuracyOfChoEstimate, Meal_ChoGrams, MonthsOfDataShownInTheGrids, " +
                        "Time_CurrentTimeZone, Time_IsDaylightSavingTime, Time_CountryName, UtcOffset) VALUES (" +
                        "@IdParameters, @Timestamp, @Insulin_Short_Id, @Insulin_Long_Id, " +
                        "@Meal_Breakfast_StartTime, @Meal_Breakfast_EndTime, @Meal_Lunch_StartTime, " +
                        "@Meal_Lunch_EndTime, @Meal_Dinner_StartTime, @Meal_Dinner_EndTime, " +
                        "@Bolus_TargetGlucose, @Bolus_GlucoseBeforeMeal, @Bolus_ChoToEat, " +
                        "@Bolus_ChoInsulinRatioBreakfast, @Bolus_ChoInsulinRatioLunch, @Bolus_ChoInsulinRatioDinner, " +
                        "@Bolus_TotalDailyDoseOfInsulin, @Bolus_InsulinCorrectionSensitivity, " +
                        "@Correction_TypicalBolusMorning, @Correction_TypicalBolusMidday, @Correction_TypicalBolusEvening, " +
                        "@Correction_TypicalBolusNight, @Correction_FactorOfInsulinCorrectionSensitivity, " +
                        "@Hypo_GlucoseTarget, @Hypo_GlucoseLast, @Hypo_GlucosePrevious, " +
                        "@Hypo_HourLast, @Hypo_HourPrevious, @Hypo_MinuteLast, @Hypo_MinutePrevious, " +
                        "@Hypo_AlarmAdvanceTime, @Hypo_FutureSpanMinutes, @Hit_ChoAlreadyTaken, " +
                        "@Hit_ChoOfFood, @Hit_TargetCho, @Hit_NameOfFood, @FoodInMeal_ChoGrams, " +
                        "@FoodInMeal_QuantityGrams, @FoodInMeal_CarbohydratesPercent, @FoodInMeal_Name, " +
                        "@FoodInMeal_AccuracyOfChoEstimate, @Meal_ChoGrams, @MonthsOfDataShownInTheGrids, " +
                        "@Time_CurrentTimeZone, @Time_IsDaylightSavingTime, @Time_CountryName, @UtcOffset);";

                    cmd.Parameters.Add(new SqliteParameter("@IdParameters", parameters.IdParameters ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Timestamp", parameters.Timestamp ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Insulin_Short_Id", parameters.IdInsulinDrug_Short ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Insulin_Long_Id", parameters.IdInsulinDrug_Long ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Breakfast_StartTime", parameters.Meal_Breakfast_StartTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Breakfast_EndTime", parameters.Meal_Breakfast_EndTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Lunch_StartTime", parameters.Meal_Lunch_StartTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Lunch_EndTime", parameters.Meal_Lunch_EndTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Dinner_StartTime", parameters.Meal_Dinner_StartTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_Dinner_EndTime", parameters.Meal_Dinner_EndTime_Hours ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_TargetGlucose", parameters.Bolus_TargetGlucose ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_GlucoseBeforeMeal", parameters.Bolus_GlucoseBeforeMeal ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoToEat", parameters.Bolus_ChoToEat ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoInsulinRatioBreakfast", parameters.Bolus_ChoInsulinRatioBreakfast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoInsulinRatioLunch", parameters.Bolus_ChoInsulinRatioLunch ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_ChoInsulinRatioDinner", parameters.Bolus_ChoInsulinRatioDinner ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_TotalDailyDoseOfInsulin", parameters.Bolus_TotalDailyDoseOfInsulin ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Bolus_InsulinCorrectionSensitivity", parameters.Bolus_InsulinCorrectionSensitivity ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusMorning", parameters.Correction_TypicalBolusMorning ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusMidday", parameters.Correction_TypicalBolusMidday ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusEvening", parameters.Correction_TypicalBolusEvening ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_TypicalBolusNight", parameters.Correction_TypicalBolusNight ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Correction_FactorOfInsulinCorrectionSensitivity", parameters.Correction_FactorOfInsulinCorrectionSensitivity ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_GlucoseTarget", parameters.Hypo_GlucoseTarget ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_GlucoseLast", parameters.Hypo_GlucoseLast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_GlucosePrevious", parameters.Hypo_GlucosePrevious ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_HourLast", parameters.Hypo_HourLast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_HourPrevious", parameters.Hypo_HourPrevious ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_MinuteLast", parameters.Hypo_MinuteLast ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_MinutePrevious", parameters.Hypo_MinutePrevious ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_AlarmAdvanceTime", parameters.Hypo_AlarmAdvanceTime ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hypo_FutureSpanMinutes", parameters.Hypo_FutureSpanMinutes ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_ChoAlreadyTaken", parameters.Hit_ChoAlreadyTaken ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_ChoOfFood", parameters.Hit_ChoOfFood ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_TargetCho", parameters.Hit_TargetCho ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Hit_NameOfFood", parameters.Hit_NameOfFood ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_ChoGrams", parameters.FoodInMeal_ChoGrams ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_QuantityGrams", parameters.FoodInMeal_QuantityGrams ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_CarbohydratesPercent", parameters.FoodInMeal_CarbohydratesPercent ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_Name", parameters.FoodInMeal_Name ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@FoodInMeal_AccuracyOfChoEstimate", parameters.FoodInMeal_AccuracyOfChoEstimate ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Meal_ChoGrams", parameters.Meal_ChoGrams ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@MonthsOfDataShownInTheGrids", parameters.MonthsOfDataShownInTheGrids ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Time_CurrentTimeZone", parameters.Time_CurrentTimeZone ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Time_IsDaylightSavingTime", parameters.Time_IsDaylightSavingTime.HasValue ? (object)(parameters.Time_IsDaylightSavingTime.Value ? 1 : 0) : DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@Time_CountryName", parameters.Time_CountryName ?? (object)DBNull.Value));
                    cmd.Parameters.Add(new SqliteParameter("@UtcOffset", parameters.UtcOffset ?? (object)DBNull.Value));
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("DataLayer | SettingsPageParameters", ex);
            }
        }
    }
}
