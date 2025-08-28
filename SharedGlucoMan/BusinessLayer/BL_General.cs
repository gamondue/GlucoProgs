using gamon;
using GlucoMan.BusinessObjects;

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
                if (!await AndroidExternalFilesHelper.RequestStoragePermissionsAsync())
                    return false;
#endif
                string fileName;
                // files in the logs path
                var filesInProgramsFolder = Directory.GetFiles(Common.PathLogs);
                foreach (string f in filesInProgramsFolder)
                {
                    //string fileSource = Path.Combine(Common.PathImportExport, Path.GetFileName(General.LogOfProgram.ErrorsFile));
                    fileName = Path.GetFileName(f);
                    if (File.Exists(General.LogOfProgram.ErrorsFile))
                    {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(General.LogOfProgram.ErrorsFile
                    , fileName);
#else
                        File.Copy(General.LogOfProgram.ErrorsFile, f, true);
#endif
                    }
                }
                // database file
                // log of insulin correction parameters 
                string exportedLogOfParameters = Path.Combine(Common.PathImportExport, Common.LogOfParametersFileName);
                fileName = Path.GetFileName(exportedLogOfParameters);
                if (File.Exists(exportedLogOfParameters))
                {
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(Common.PathAndFileLogOfParameters
                    , fileName);
#else
                    File.Copy(Common.PathAndFileLogOfParameters, exportedLogOfParameters, true);
#endif
                }
                string exportedDatabase = Path.Combine(Common.PathImportExport, Common.DatabaseFileName);
                fileName = Path.GetFileName(exportedDatabase);
                if (File.Exists(Common.PathAndFileDatabase))
                {
#if ANDROID
                    bool success = await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(Common.PathAndFileDatabase, fileName);
                    if (!success)
                    {
                        General.LogOfProgram.Error("Fallimento nell'export del database", null);
                        return false;
                    }
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
        internal async Task<bool> ReadDatabaseFromExternal(string pathAndFileInternalDatabase, string pathAndFileExternalDatabase)
        {
            try
            {
                string backupFilename = Common.DatabaseFileName.Replace(".Sqlite", "_backup.Sqlite");
                if (File.Exists(pathAndFileInternalDatabase))
                {
                    string backupFile = Path.Combine(Common.PathDatabase, backupFilename);
                    // backup the current database, in Android it is in internal storage, so we can use the class File 
                    File.Copy(pathAndFileInternalDatabase, backupFile, true);
                    //File.Delete(pathAndFileInternalDatabase);
                }
#if ANDROID
                // if the user has not given the permissions, then return with false
                if (!await AndroidExternalFilesHelper.RequestStoragePermissionsAsync())
                    return false;
                 return await AndroidExternalFilesHelper.ReadFileFromExternalPublicDirectoryAsync
                    (Path.GetFileName((pathAndFileExternalDatabase)), pathAndFileInternalDatabase);
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
        internal void CreateNewDatabase()
        {
            dl.CreateNewDatabase(Common.PathAndFileDatabase);
        }
        internal async Task<bool> ExportProgramsFilesAsync()
        {
            try
            {
                // export all the files in the log folder
                var filesInLogFolder = Directory.GetFiles(Common.PathLogs);
                string fileName, fileDestination;
                foreach (string fileSource in filesInLogFolder)
                {
                    //string fileSource = Path.Combine(Common.PathImportExport, Path.GetFileName(General.LogOfProgram.ErrorsFile));
                    fileName = Path.GetFileName(fileSource);
                    fileDestination = Path.Combine(Common.PathImportExport, fileName);
                    if (File.Exists(General.LogOfProgram.ErrorsFile))
                    {
                        if (File.Exists(fileDestination))
                            File.Delete(fileDestination);
#if ANDROID
                        await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(
                            fileSource,
                            fileDestination);
#else
                        File.Copy(fileSource, fileDestination, true);
#endif
                    }
                }
                // export database file
                string exportedDatabase = Path.Combine(Common.PathImportExport, Common.DatabaseFileName);
                fileName = Path.GetFileName(exportedDatabase);
                //if (File.Exists(Common.PathAndFileDatabase))
                //{
                    if (File.Exists(exportedDatabase))
                        File.Delete(exportedDatabase);
#if ANDROID
                    await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(
                    Common.PathAndFileDatabase,
                    exportedDatabase);
#else
                    File.Copy(Common.PathAndFileDatabase, exportedDatabase, true);
#endif
                //}
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("BL,ExportProgramsFiles", ex);
                return false;
            }
        }
        internal Parameters GetSettingsPageParameters()
        {
            return dl.GetParameters();
        }
        internal void SaveAllParameters(Parameters p,
            InsulinDrug ShortActingInsulin, InsulinDrug LongActingInsulin)
        {
            // check if the parameters relative to the insulins are changed
            // if they are changed, then save a new row with timestamp in the parameters table
            // otherwise save the parameters in the current row
            bool SaveInANewRowWithTimestamp = false;
            if (ShortActingInsulin != null && LongActingInsulin != null)
            {
                InsulinDrug? oldShort = dl.GetOneInsulinDrug(p.IdInsulinDrug_Short);
                InsulinDrug? oldLong = dl.GetOneInsulinDrug(p.IdInsulinDrug_Long);
                if (oldShort == null || oldLong == null ||
                    oldShort.Name != ShortActingInsulin.Name ||
                    oldLong.Name != LongActingInsulin.Name ||
                    oldShort.DurationInHours != ShortActingInsulin.DurationInHours ||
                    oldLong.DurationInHours != LongActingInsulin.DurationInHours)
                {
                    p.IdInsulinDrug_Short = ShortActingInsulin.IdInsulinDrug;
                    p.IdInsulinDrug_Long = LongActingInsulin.IdInsulinDrug;
                    SaveInANewRowWithTimestamp = true;
                }
            }
            dl.SaveAllParameters(p, SaveInANewRowWithTimestamp);
            dl.SaveInsulinDrug(ShortActingInsulin);
            dl.SaveInsulinDrug(LongActingInsulin);
        }
    }
}
