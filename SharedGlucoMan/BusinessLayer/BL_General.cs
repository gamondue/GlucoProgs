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
                    
                // Get proper Android export path
                string exportBasePath = Path.Combine(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, 
                    "GlucoMan");
#else
                string exportBasePath = Common.PathImportExport;
#endif

                string fileName;
                // files in the logs path
                var filesInProgramsFolder = Directory.GetFiles(Common.PathLogs);
                foreach (string fileSource in filesInProgramsFolder)
                {
                    fileName = Path.GetFileName(fileSource);
                    if (File.Exists(fileSource)) // Check source file exists
                    {
#if ANDROID
                        string fileDestination = Path.Combine(exportBasePath, fileName);
                        await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(fileSource, fileDestination);
#else
                        string fileDestination = Path.Combine(Common.PathImportExport, fileName);
                        File.Copy(fileSource, fileDestination, true);
#endif
                    }
                }
                
                // log of insulin correction parameters 
                string logOfParametersFile = Common.PathAndFileLogOfParameters;
                if (File.Exists(logOfParametersFile))
                {
                    fileName = Path.GetFileName(logOfParametersFile);
#if ANDROID
                    string fileDestination = Path.Combine(exportBasePath, fileName);
                    await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(logOfParametersFile, fileDestination);
#else
                    string exportedLogOfParameters = Path.Combine(Common.PathImportExport, Common.LogOfParametersFileName);
                    File.Copy(logOfParametersFile, exportedLogOfParameters, true);
#endif
                }

                // database file
                if (File.Exists(Common.PathAndFileDatabase))
                {
                    fileName = Path.GetFileName(Common.PathAndFileDatabase);
#if ANDROID
                    string fileDestination = Path.Combine(exportBasePath, fileName);
                    bool success = await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(Common.PathAndFileDatabase, fileDestination);
                    if (!success)
                    {
                        General.LogOfProgram.Error("Fallimento nell'export del database", null);
                        return false;
                    }
#else
                    string exportedDatabase = Path.Combine(Common.PathImportExport, Common.DatabaseFileName);
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
        //internal async Task<bool> ReadDatabaseFromExternal(string pathAndFileInternalDatabase, 
        //    string pathAndFileExternalDatabase)
        internal async Task<bool> ReadDatabaseFromExternal(string pathAndFileInternalDatabase,
                    string pathAndFileExternalDatabase)
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
                    (pathAndFileInternalDatabase, pathAndFileExternalDatabase);
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
        internal bool ImportFoodsFromExternal(string pathAndFileDatabase, string v)
        {
            // import the whole database
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
#if ANDROID
                // Try the enhanced file helper first for better Huawei/Xiaomi compatibility
                bool enhancedSuccess = await TryEnhancedFileExport();
                if (enhancedSuccess)
                {
                    General.LogOfProgram.Debug("Export completed successfully using enhanced file helper");
                    return true;
                }

                // Check permissions first for traditional method
                if (!await AndroidExternalFilesHelper.RequestStoragePermissionsAsync())
                {
                    General.LogOfProgram.Error("Insufficient permissions for export", null);
                    return false;
                }
                
                // Get the Download/GlucoMan directory for Android
                string exportBasePath = Path.Combine(
                    Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, 
                    "GlucoMan");
#else
                string exportBasePath = Common.PathImportExport;
#endif

                // Export all files in the log folder
                var filesInLogFolder = Directory.GetFiles(Common.PathLogs);
                string fileName, fileDestination;
                foreach (string fileSource in filesInLogFolder)
                {
                    fileName = Path.GetFileName(fileSource);
                    fileDestination = Path.Combine(exportBasePath, fileName);
                    
                    if (File.Exists(fileSource)) // Check the actual source file, not the error log
                    {
                        General.LogOfProgram.Debug($"Exporting log file: {fileSource} -> {fileDestination}");
                        
                        if (File.Exists(fileDestination))
                            File.Delete(fileDestination);

#if ANDROID
                        bool success = await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(
                            fileSource, fileDestination);
                        if (!success)
                        {
                            General.LogOfProgram.Error($"Failed to export log file: {fileName}", null);
                        }
#else
                        File.Copy(fileSource, fileDestination, true);
#endif
                    }
                }

                // Export database file
                fileName = Path.GetFileName(Common.DatabaseFileName);
                string exportedDatabase = Path.Combine(exportBasePath, fileName);
                
                if (File.Exists(Common.PathAndFileDatabase))
                {
                    General.LogOfProgram.Debug($"Exporting database: {Common.PathAndFileDatabase} -> {exportedDatabase}");
                    
                    if (File.Exists(exportedDatabase))
                        File.Delete(exportedDatabase);

#if ANDROID
                    bool success = await AndroidExternalFilesHelper.SaveFileToExternalStoragePublicDirectoryAsync(
                        Common.PathAndFileDatabase, exportedDatabase);
                    if (!success)
                    {
                        General.LogOfProgram.Error("Failed to export database", null);
                        return false;
                    }
#else
                    File.Copy(Common.PathAndFileDatabase, exportedDatabase, true);
#endif
                }
                else
                {
                    General.LogOfProgram.Error($"Database file not found: {Common.PathAndFileDatabase}", null);
                }

                General.LogOfProgram.Debug("Export completed successfully");
                return true;
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("BL,ExportProgramsFilesAsync", ex);
                return false;
            }
        }
#if ANDROID
        private async Task<bool> TryEnhancedFileExport()
        {
            try
            {
                General.LogOfProgram.Debug("Starting enhanced file export for Huawei/Xiaomi compatibility");
                
                // Check if we have basic file permissions
                if (!EnhancedFileHelper.HasBasicFilePermissions())
                {
                    General.LogOfProgram.Debug("Basic file permissions not available, skipping enhanced export");
                    return false;
                }

                var exportedFiles = new List<string>();
                var failedFiles = new List<string>();

                // Export database file
                if (File.Exists(Common.PathAndFileDatabase))
                {
                    string dbFileName = Path.GetFileName(Common.PathAndFileDatabase);
                    var result = await EnhancedFileHelper.SaveFileWithFallback(Common.PathAndFileDatabase, dbFileName);
                    
                    if (result.Success)
                    {
                        exportedFiles.Add($"{dbFileName} -> {result.Path}");
                    }
                    else
                    {
                        failedFiles.Add(dbFileName);
                    }
                }

                // Export log files
                if (Directory.Exists(Common.PathLogs))
                {
                    var logFiles = Directory.GetFiles(Common.PathLogs);
                    foreach (string logFile in logFiles)
                    {
                        string logFileName = Path.GetFileName(logFile);
                        var result = await EnhancedFileHelper.SaveFileWithFallback(logFile, logFileName);
                        
                        if (result.Success)
                        {
                            exportedFiles.Add($"{logFileName} -> {result.Path}");
                        }
                        else
                        {
                            failedFiles.Add(logFileName);
                        }
                    }
                }

                // Export parameters log if exists
                if (File.Exists(Common.PathAndFileLogOfParameters))
                {
                    string paramFileName = Path.GetFileName(Common.PathAndFileLogOfParameters);
                    var result = await EnhancedFileHelper.SaveFileWithFallback(Common.PathAndFileLogOfParameters, paramFileName);
                    
                    if (result.Success)
                    {
                        exportedFiles.Add($"{paramFileName} -> {result.Path}");
                    }
                    else
                    {
                        failedFiles.Add(paramFileName);
                    }
                }

                General.LogOfProgram.Debug($"Enhanced export results: {exportedFiles.Count} succeeded, {failedFiles.Count} failed");
                
                foreach (var exported in exportedFiles)
                {
                    General.LogOfProgram.Debug($"Exported: {exported}");
                }

                foreach (var failed in failedFiles)
                {
                    General.LogOfProgram.Error($"Failed to export: {failed}", null);
                }

                // Consider successful if we exported at least the database
                return exportedFiles.Any(f => f.Contains(".sqlite") || f.Contains(".db"));
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("TryEnhancedFileExport", ex);
                return false;
            }
        }
#endif
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
