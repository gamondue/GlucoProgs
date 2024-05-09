using GlucoMan;
using System.IO; 
using System;
using System.Collections.Generic;

namespace GlucoMan.BusinessLayer
{
    // Business Layer, general part
    public class BL_General
    {
        DataLayer dl = Common.Database;
        public  long? SaveParameter(string FieldName, string FieldValue)
        {
            return dl.SaveParameter(FieldName, FieldValue);
        }
        public  string RestoreParameter(string FieldName)
        {
            return dl.RestoreParameter(FieldName);
        }
        public bool DeleteDatabase()
        {
            return dl.DeleteDatabase();
        }
        internal bool ExportProgramsFiles()
        {
            try
            {
                // database file
                string exportedDatabase = Path.Combine(Common.PathImportExport, Common.DatabaseFileName);
                File.Copy(Common.PathAndFileDatabase, exportedDatabase, true);

                // log of errors 
                string exportedLogOfProgram = Path.Combine(Common.PathImportExport, 
                    Path.GetFileName (Common.LogOfProgram.ErrorsFile)); 
                File.Copy(Common.LogOfProgram.ErrorsFile, exportedLogOfProgram, true);

                // log of insulin correction parameters 
                string exportedLogOfParameters = Path.Combine(Common.PathImportExport,
                    Common.LogOfParametersFileName); 
                File.Copy(Common.PathAndFileLogOfParameters, exportedLogOfParameters, true);

                return true;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("ExportProgramsFiles", ex); 
                return false;
            }
        }
        internal bool ImportDatabaseFromExternal(string pathAndFileDatabase, string v)
        {
            // import the foods
            DL_Sqlite dlImport = new DL_Sqlite(Path.Combine(Common.PathImportExport, "import.sqlite"));

            List<Food> source = dlImport.GetFoods();
            List<Food> destination = dl.GetFoods();

            foreach(Food destFood in destination)
            {
                foreach (Food sourceFood in source)
                {
                    // calc similarities of all the new records 
                    // !!!! TODO !!!! 
                }
            }
            return false; 
        }
        internal bool ReadDatabaseFromExternal(string pathAndFileInternalDatabase, string pathAndFileExternalDatabase)
        {
            try
            {
                string backupFile = Path.Combine(Common.PathImportExport, Common.DatabaseFileName.Replace(".Sqlite", "_backup.Sqlite"));
                if (File.Exists(pathAndFileInternalDatabase))
                {
                    File.Copy(pathAndFileInternalDatabase, backupFile, true);
                    //File.Delete(pathAndFileInternalDatabase);
                    //File.Create(pathAndFileInternalDatabase); 
                }
                File.Copy(pathAndFileExternalDatabase, pathAndFileInternalDatabase, true);
                return true;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("ReadDatabaseFromExternal", ex);
                return false;
            }
        }
        internal void CreateNewDatabase()
        { 
            dl.CreateNewDatabase(Common.PathAndFileDatabase);
        }
    }
}
