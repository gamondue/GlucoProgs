using Microsoft.Data.Sqlite;
using SharedData;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        /// <summary>
        /// Data Access Layer: abstracts the access to dbms using to transfer data 
        /// DbClasses and ADO db classes (ADO should be avoided, if possible) 
        /// </summary>
        private string dbName;

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
        private void CreateNewDatabase(string dbName)
        {
            // when the file does not exist 
            // Microsoft.Data.Sqlite creates the file at first connection
            Connect();

            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    string query = @"CREATE TABLE Parameters (" +
                    "IdParameters INTEGER NOT NULL," +
                    "TargetGlucose INTEGER," +
                    "GlucoseBeforeMeal INTEGER," +
                    "ChoToEat INTEGER," +
                    "ChoInsulinRatioBreakfast DOUBLE," +
                    "ChoInsulinRatioLunch DOUBLE," +
                    "ChoInsulinRatioDinner DOUBLE," +
                    "TypicalBolusMorning DOUBLE," +
                    "TypicalBolusMidday DOUBLE," +
                    "TypicalBolusEvening DOUBLE," +
                    "TypicalBolusNight DOUBLE," +
                    "TotalDailyDoseOfInsulin DOUBLE," +
                    "FactorOfInsulinCorrectionSensitivity DOUBLE," +
                    "InsulinCorrectionSensitivity DOUBLE," +
                    "PRIMARY KEY(IdParameters AUTOINCREMENT)" +
                    ");";
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_DataAndGeneral | CreateNewDatabase", ex);
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

            //string baseConnectionString = "Data Source=" + dbName + " Cache = Shared";
            //var connectionString = new SqliteConnectionStringBuilder(baseConnectionString)
            //{
            //    Mode = SqliteOpenMode.ReadWriteCreate,
            //    //Password = password
            //}.ToString();

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
        public int nFieldDbDataReader(string NomeCampo, DbDataReader dr)
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
        internal int NextKey(string Table, string KeyName)
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
                    " WHERE " + KeyName + "=" + SqlString(KeyValue) +
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
        private bool FieldExists(string TableName, string FieldName)
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
        internal void BackupAllStudentsDataTsv()
        {
            BackupTableTsv("Students");
            BackupTableTsv("StudentsPhotos");
            BackupTableTsv("StudentsPhotos_Students");
            BackupTableTsv("Classes_Students");
            BackupTableTsv("Grades");
        }
        internal void BackupAllStudentsDataXml()
        {
            BackupTableXml("Students");
            BackupTableXml("StudentsPhotos");
            BackupTableXml("StudentsPhotos_Students");
            BackupTableXml("Classes_Students");
            BackupTableXml("Grades");
        }
        internal void RestoreAllStudentsDataTsv(bool MustErase)
        {
            RestoreTableTsv("Students", MustErase);
            RestoreTableTsv("StudentsPhotos", MustErase);
            RestoreTableTsv("StudentsPhotos_Students", MustErase);
            RestoreTableTsv("Classes_Students", MustErase);
            RestoreTableTsv("Grades", MustErase);
        }
        internal void RestoreAllStudentsDataXml(bool MustErase)
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
                            types += "\"" + SafeRead.String(dRead.GetDataTypeName(i)) + "\"\t";
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
                            values += "\"" + SafeRead.String(dRead.GetValue(i).ToString()) + "\"\t";
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
                //                valuesString += "" + SqlString(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("INT") >= 0)
                //                valuesString +=  SqlInt(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("REAL") >= 0)
                //                valuesString += SqlFloat(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("FLOAT") >= 0)
                //                valuesString += SqlFloat(dati[row, col]) + ",";
                //            else if (fieldTypes[col].IndexOf("DATE") >= 0)
                //                valuesString += SqlDate(dati[row, col]) + ",";
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
                                    cmd.CommandText += "" + SqlString(row[c.ColumnName].ToString()) + ",";
                                    break;
                                };
                            case TypeCode.DateTime:
                                {
                                    DateTime? d = SafeRead.DateTime(row[c.ColumnName]);
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
        internal void CreateDemoDatabase(string newDatabaseFullName)
        {
            //    DbCommand cmd;

            //    File.Copy(Common.PathAndFileDatabase, newDatabaseFullName);

            //    // local instance of a DataLayer to operate on a second database 
            //    DataLayer newDatabaseDl = new DataLayer(newDatabaseFullName);

            //    // erase all the data of the students of other classes
            //    using (DbConnection conn = newDatabaseDl.Connect()) // connect to the new database, just copied
            //    {
            //        cmd = conn.CreateCommand();

            //        // erase all the other classes
            //        cmd.CommandText = "DELETE FROM Classes" +
            //        " WHERE idClass<>" + Class1.IdClass +
            //        " AND idClass<>" + Class2.IdClass + ";";
            //        cmd.ExecuteNonQuery();

            //        // erase all the lessons of other classes
            //        cmd.CommandText = "DELETE FROM Lessons" +
            //            " WHERE idClass<>" + Class1.IdClass +
            //            " AND idClass<>" + Class2.IdClass + ";";
            //        cmd.ExecuteNonQuery();

            //        // erase all the students of other classes from the link table
            //        cmd.CommandText = "DELETE FROM Classes_Students" +
            //         " WHERE idClass<>" + Class1.IdClass +
            //         " AND idClass<>" + Class2.IdClass + ";";
            //        cmd.ExecuteNonQuery();

            //        // erase all the students of other classes 
            //        cmd.CommandText = "DELETE FROM Students" +
            //            " WHERE idStudent NOT IN" +
            //            " (SELECT idStudent FROM Classes_Students" +
            //            " WHERE idClass<>" + Class1.IdClass +
            //            " OR idClass<>" + Class2.IdClass +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the annotation, of all classes
            //        cmd.CommandText = "DELETE FROM StudentsAnnotations" +
            //            ";";
            //        cmd.ExecuteNonQuery();

            //        // erase all the StartLinks of other classes
            //        cmd.CommandText = "DELETE FROM Classes_StartLinks" +
            //            " WHERE idClass<>" + Class1.IdClass +
            //            " AND idClass<>" + Class2.IdClass +
            //            ";";
            //        cmd.ExecuteNonQuery();

            //        // erase all the grades of other classes' students
            //        cmd.CommandText = "DELETE FROM Grades" +
            //            " WHERE idStudent NOT IN" +
            //            " (SELECT idStudent FROM Classes_Students" +
            //            " WHERE idClass<>" + Class1.IdClass +
            //            " OR idClass<>" + Class2.IdClass +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the links to photos of other classes' students
            //        cmd.CommandText = "DELETE FROM StudentsPhotos_Students" +
            //            " WHERE idStudent NOT IN" +
            //            " (SELECT idStudent FROM Classes_Students" +
            //            " WHERE idClass<>" + Class1.IdClass +
            //            " OR idClass<>" + Class2.IdClass +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the photos of other classes' students
            //        cmd.CommandText = "DELETE FROM StudentsPhotos WHERE StudentsPhotos.idStudentsPhoto NOT IN" +
            //            "(SELECT StudentsPhotos_Students.idStudentsPhoto" +
            //            " FROM StudentsPhotos, StudentsPhotos_Students, Classes_Students" +
            //            " WHERE StudentsPhotos_Students.idStudent = Classes_Students.idStudent" +
            //            " AND StudentsPhotos.idStudentsPhoto = StudentsPhotos_Students.idStudentsPhoto" +
            //            " AND (Classes_Students.idClass=" + Class1.IdClass +
            //            " OR Classes_Students.idClass=" + Class2.IdClass + ")" +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the images of other classes
            //        cmd.CommandText = "DELETE FROM Images WHERE Images.idImage NOT IN" +
            //            "(SELECT DISTINCT Lessons_Images.idImage" +
            //            " FROM Images, Lessons_Images, Lessons" +
            //            " WHERE Lessons_Images.idImage = Images.idImage" +
            //            " AND Lessons_Images.idLesson = Lessons.idLesson" +
            //            " AND (Lessons.idClass=" + Class1.IdClass +
            //            " OR Lessons.idClass=" + Class2.IdClass + ")" +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        //erase all links to the images of other classes
            //        cmd.CommandText = "DELETE FROM Lessons_Images WHERE Lessons_Images.idImage NOT IN" +
            //            "(SELECT DISTINCT Lessons_Images.idImage" +
            //            " FROM Images, Lessons_Images, Lessons" +
            //            " WHERE Lessons_Images.idImage = Images.idImage" +
            //            " AND Lessons_Images.idLesson = Lessons.idLesson" +
            //            " AND (Lessons.idClass=" + Class1.IdClass +
            //            " OR Lessons.idClass=" + Class2.IdClass + ")" +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the questions of the students of the other classes
            //        // !! StudentsQuestions currently not used !!
            //        cmd.CommandText = "DELETE FROM StudentsQuestions" +
            //            " WHERE idStudent NOT IN" +
            //            " (SELECT DISTINCT idStudent FROM Classes_Students" +
            //            " WHERE idClass=" + Class1.IdClass +
            //            " OR idClass=" + Class2.IdClass +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the answers  of the students of the other classes
            //        // !! StudentsAnswers currently not used !!
            //        cmd.CommandText = "DELETE FROM StudentsAnswers" +
            //        " WHERE idStudent NOT IN" +
            //        " (SELECT idStudent FROM Classes_Students" +
            //            " WHERE idClass=" + Class1.IdClass +
            //            " OR idClass=" + Class2.IdClass + ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the tests of students of the other classes
            //        // !! StudentsTests currently not used !!
            //        cmd.CommandText = "DELETE FROM StudentsTests" +
            //        " WHERE idStudent NOT IN" +
            //        " (SELECT idStudent FROM Classes_Students" +
            //        " WHERE idClass=" + Class1.IdClass +
            //            " OR idClass=" + Class2.IdClass +
            //        ");";
            //        cmd.ExecuteNonQuery();

            //        // erase all the topics of other classes' lessons
            //        cmd.CommandText = "DELETE FROM Lessons_Topics" +
            //            " WHERE idLesson NOT IN" +
            //            " (SELECT idLesson from Lessons" +
            //            " WHERE idClass=" + Class1.IdClass +
            //            " OR idClass=" + Class2.IdClass +
            //            ");";
            //        cmd.ExecuteNonQuery();

            //        // change the data of the classes
            //        Class1.Abbreviation = "DEMO1";
            //        Class1.Description = "SchoolGrades demo class 1";
            //        // Class1.IdSchool = ""; // left the existing code 
            //        Class1.PathRestrictedApplication = Common.PathExe + "\\demo1";
            //        // Class1.SchoolYear = // !!!! shift the data to the destination school year, to be done when year's shifting will be managed!!!!
            //        Class1.IdSchool = Common.IdSchool;
            //        Class1.UriWebApp = ""; // ???? decide what to put here ????

            //        // SaveClass Class1;
            //        string query = "UPDATE Classes" +
            //            " SET" +
            //            " idClass=" + Class1.IdClass + "" +
            //            ",idSchoolYear=" + SqlString(Class1.SchoolYear) + "" +
            //            ",idSchool=" + SqlString(Class1.IdSchool) + "" +
            //            ",abbreviation=" + SqlString(Class1.Abbreviation) + "" +
            //            ",desc=" + SqlString(Class1.Description) + "" +
            //            ",uriWebApp=" + Class1.UriWebApp + "" +
            //            ",pathRestrictedApplication=" + SqlString(Class1.PathRestrictedApplication) + "" +
            //            " WHERE idClass=" + Class1.IdClass +
            //            ";";
            //        cmd.CommandText = query;
            //        cmd.ExecuteNonQuery();

            //        Class2.Abbreviation = "DEMO2";
            //        Class2.Description = "SchoolGrades demo class 2";
            //        Class2.PathRestrictedApplication = Common.PathExe + "\\demo2";
            //        // Class2.SchoolYear = !!!! shift the data to the destination school year !!!!
            //        Class2.IdSchool = Common.IdSchool;
            //        Class2.UriWebApp = ""; // ???? decide what to put here ????
            //        // SaveClass Class2;
            //        query = "UPDATE Classes" +
            //            " SET" +
            //            " idClass=" + Class2.IdClass + "" +
            //            ",idSchoolYear=" + SqlString(Class2.SchoolYear) + "" +
            //            ",idSchool=" + SqlString(Class2.IdSchool) + "" +
            //            ",abbreviation=" + SqlString(Class2.Abbreviation) + "" +
            //            ",desc=" + SqlString(Class2.Description) + "" +
            //            ",uriWebApp=" + Class2.UriWebApp + "" +
            //            ",pathRestrictedApplication=" + SqlString(Class2.PathRestrictedApplication) + "" +
            //            " WHERE idClass=" + Class2.IdClass +
            //            ";";
            //        cmd.CommandText = query;
            //        cmd.ExecuteNonQuery();

            //        // erase all the users
            //        cmd = conn.CreateCommand();
            //        cmd.CommandText = "DELETE FROM Users" +
            //            ";";
            //        cmd.ExecuteNonQuery();

            //        // rename every student left in the database according to the names found in the pictures' filenames
            //        RenameStudentsNamesFromPictures(Class1, conn);
            //        RenameStudentsNamesFromPictures(Class2, conn);

            //        // change the paths of the images 
            //        ChangeImagesPath(Class1, conn);
            //        ChangeImagesPath(Class2, conn);

            //        // randomly change all grades 
            //        RandomizeGrades(conn);

            //        // change the lesson dates to this school year (when we implement year shift!) 
            //        // !!!! TODO !!!!

            //        // change the school year in StudentsPhotos_Students (when we implement year shift!) 
            //        // !!!! TODO !!!!

            //        // compact the database 
            //        cmd.CommandText = "VACUUM;";
            //        cmd.ExecuteNonQuery();

            //        cmd.Dispose();
            //    }
            //    return newDatabaseFullName;
        }
    }
}
