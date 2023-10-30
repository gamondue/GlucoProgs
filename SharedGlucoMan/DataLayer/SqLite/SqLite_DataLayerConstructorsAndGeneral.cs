using gamon;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        /// <summary>
        /// Data Access Layer: abstracts the access to dbms using to transfer data 
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
            if (!System.IO.File.Exists(Common.PathAndFileDatabase))
            {
                // since the file doesn't exist yet, we create it:
                CreateNewDatabase(dbName);
            }
        }
        /// <summary>
        /// Constructor of DataLayer class that get from outside the databases to use
        /// Assumes that the file exists.
        /// </summary>
        internal DL_Sqlite(string PathAndFile)
        {
            // ???? is next if useful ????
            if (!System.IO.File.Exists(PathAndFile))
            {
                string err = @"[" + PathAndFile + " not in the current nor in the dev directory]";
                Common.LogOfProgram.Error(err, null);
                throw new System.IO.FileNotFoundException(err);
            }
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
                Common.LogOfProgram.Error("Error connecting to the database: " + ex.Message + "\r\nFile Sqlite>: " + dbName + " " + "\n", null);
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
        internal bool FieldExists(string TableName, string FieldName)
        {
            // watch if field isPopUp exist in the database
            DataTable table = new DataTable();
            bool fieldExists;
            using (DbConnection conn = Connect())
            {
                table = conn.GetSchema("Columns", new string[] { null, null, TableName, null });
                fieldExists = false;
                foreach (DataRow row in table.Rows)
                {
                    foreach (DataColumn col in table.Columns)
                    {
                        if (row["COLUMN_NAME"].ToString() == FieldName)
                        {
                            fieldExists = true;
                            break;
                        }
                    }
                }
            }
            return fieldExists;
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
                            types += "\"" + SqlSafe.String(dRead.GetDataTypeName(i)) + "\"\t";
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
                            values += "\"" + SqlSafe.String(dRead.GetValue(i).ToString()) + "\"\t";
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
                                };
                            case TypeCode.DateTime:
                                {
                                    DateTime? d = SqlSafe.DateTime(row[c.ColumnName]);
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
                File.Delete(dbName);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | DeleteDatabase", ex);
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
                                    query += FieldName + "='" + FieldValue + "'";
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
                Common.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | SaveParameter", ex);
                return null;
            }
        }
        internal override string RestoreParameter(string FieldName, int? Key = null)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    // read table Parameters, to see if it has rows
                    DbCommand cmd = conn.CreateCommand();
                    string query = "SELECT ";
                    query += FieldName;
                    query += " FROM Parameters";

                    string whereClause;
                    if (Key == null)
                    {
                        // no key between parameter: we use the highest key
                        cmd.CommandText = "SELECT MAX(IdParameters) FROM Parameters;";
                        int? maxKey = SqlSafe.Int(cmd.ExecuteScalar());
                        //int? maxKey = NextKey("Parameters", "IdParameters");
                        whereClause = " WHERE IdParameters=" + maxKey;
                    }
                    else
                    {
                        // the user passed a key; we use that
                        whereClause = " WHERE IdParameters=" + Key;
                    }
                    query += whereClause + ";";
                    cmd.CommandText = query;
                    string result = SqlSafe.String(cmd.ExecuteScalar());
                    cmd.Dispose();
                    return result;
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_DataLayerConstructorsAndGeneral | RestoreParameter", ex);
                return null;
            }
        }
    }
}
