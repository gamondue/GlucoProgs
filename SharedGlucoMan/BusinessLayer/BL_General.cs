using gamon;

namespace GlucoMan.BusinessLayer
{
    // Business Layer, general part
    public class BL_General
    {
        DataLayer dl = Common.Database;
        public long? SaveParameter(string FieldName, string FieldValue)
        {
            return dl.SaveParameter(FieldName, FieldValue);
        }
        public string RestoreParameter(string FieldName)
        {
            return dl.RestoreParameter(FieldName);
        }
        public bool DeleteDatabase()
        {
            return dl.DeleteDatabase();
        }
        internal async Task<bool> ExportProgramsFiles()
        {
            try
            {
#if ANDROID
                // Ensure the method is marked as async and the return type is Task<bool>
                if (!await AndroidExternalFilesHelper.RequestStoragePermissionsAsync())
                    return false;
#endif
                // log of errors file
                string exportedLogOfProgram = Path.Combine(Common.PathImportExport, Path.GetFileName(General.LogOfProgram.ErrorsFile));
                if (File.Exists(General.LogOfProgram.ErrorsFile))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalPublicDirectoryAsync(General.LogOfProgram.ErrorsFile, exportedLogOfProgram);
#else
                    File.Copy(General.LogOfProgram.ErrorsFile, exportedLogOfProgram, true);
#endif
                }
                // database file
                // log of insulin correction parameters 
                string exportedLogOfParameters = Path.Combine(Common.PathImportExport, Common.LogOfParametersFileName);
                if (File.Exists(exportedLogOfParameters))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalPublicDirectoryAsync(Common.PathAndFileLogOfParameters, exportedLogOfParameters);
#else
                    File.Copy(Common.PathAndFileLogOfParameters, exportedLogOfParameters, true);
#endif
                }
                string exportedDatabase = Path.Combine(Common.PathImportExport, Common.DatabaseFileName);
                if (File.Exists(Common.PathAndFileDatabase))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalPublicDirectoryAsync(Common.PathAndFileDatabase, exportedDatabase);
#else
                    File.Copy(Common.PathAndFileDatabase, exportedDatabase, true);
#endif
                }
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("ExportProgramsFiles", ex);
                return false;
            }
        }
        internal bool ImportDatabaseFromExternal(string pathAndFileDatabase, string v)
        {
            // import the foods
            DL_Sqlite dlImport = new DL_Sqlite(Path.Combine(Common.PathImportExport, "import.sqlite"));

            List<Food> source = dlImport.GetFoods();
            List<Food> destination = dl.GetFoods();

            foreach (Food destFood in destination)
            {
                foreach (Food sourceFood in source)
                {
                    // calc similarities of all the new records 
                    // !!!! TODO !!!! 
                }
            }
            return false;
        }
        internal async Task<bool> ReadDatabaseFromExternal(string pathAndFileInternalDatabase, string pathAndFileExternalDatabase)
        {
            try
            {
                string backupFile = Path.Combine(Common.PathDatabase, Common.DatabaseFileName.Replace(".Sqlite", "_backup.Sqlite"));
                // backup the current database, in Android it is in internal storage, so we can use the class File 
                File.Copy(pathAndFileInternalDatabase, backupFile, true);
                File.Delete(pathAndFileInternalDatabase);
#if ANDROID
                // if the user has not given the permissions, then return with false
                if (!await AndroidExternalFilesHelper.RequestStoragePermissionsAsync())
                    return false;
                return await AndroidExternalFilesHelper.ReadFileFromExternalStorageAsync
                    (pathAndFileExternalDatabase, pathAndFileInternalDatabase);
#else
                File.Copy(pathAndFileExternalDatabase, pathAndFileInternalDatabase, true);
#endif
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("ReadDatabaseFromExternal", ex);
                return false;
            }
        }
        internal void CreateNewDatabase()
        {
            dl.CreateNewDatabase(Common.PathAndFileDatabase);
        }
        internal async Task<bool> ExportProgramsFilesAsync()
        {
            try
            {
                // export log of errors file
                string exportedLogOfProgram = Path.Combine(Common.PathImportExport, Path.GetFileName(General.LogOfProgram.ErrorsFile));
                if (File.Exists(General.LogOfProgram.ErrorsFile))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalPublicDirectoryAsync(General.LogOfProgram.ErrorsFile, exportedLogOfProgram);
#else
                    File.Copy(General.LogOfProgram.ErrorsFile, exportedLogOfProgram, true);
#endif
                }
                // database file
                // log of insulin correction parameters 
                string exportedLogOfParameters = Path.Combine(Common.PathImportExport, Common.LogOfParametersFileName);
                if (File.Exists(exportedLogOfParameters))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalPublicDirectoryAsync(Common.PathAndFileLogOfParameters, exportedLogOfParameters);
#else
                    File.Copy(Common.PathAndFileLogOfParameters, exportedLogOfParameters, true);
#endif
                }
                string exportedDatabase = Path.Combine(Common.PathImportExport, Common.DatabaseFileName);
                if (File.Exists(Common.PathAndFileDatabase))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalPublicDirectoryAsync(Common.PathAndFileDatabase, exportedDatabase);
#else
                    File.Copy(Common.PathAndFileDatabase, exportedDatabase, true);
#endif
                }
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("ExportProgramsFiles", ex);
                return false;
            }
        }
    }
}
