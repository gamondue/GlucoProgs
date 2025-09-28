using gamon;
using GlucoMan.BusinessLayer;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.IO;
using CommunityToolkit.Maui.Storage;
using System.Text;
using CommunityToolkit.Maui.Alerts;

#if ANDROID
using AndroidEnvironment = Android.OS.Environment;
#endif

namespace GlucoMan.Maui;

public partial class MiscellaneousFunctionsPage : ContentPage
{
    GlucoMan.BusinessLayer.BL_General blGeneral = new GlucoMan.BusinessLayer.BL_General();
    bool canModify = true;
    public MiscellaneousFunctionsPage()
    {
        InitializeComponent();
    }
    private void txt_mgPerdL_TextChanged(object sender, EventArgs e)
    {
        double value;
        double.TryParse(txt_mgPerdL.Text, out value);
        if (canModify)
        {
            canModify = false;
            txt_mmolPerL.Text = Common.mgPerdL_To_mmolPerL(value).ToString("0.00");
            canModify = true;
        }
        else
        {
            canModify = true;
        }
    }
    private void txt_mmolPerL_TextChanged(object sender, EventArgs e)
    {
        double value;
        double.TryParse(txt_mmolPerL.Text, out value);
        if (canModify)
        {
            canModify = false;
            txt_mgPerdL.Text = Common.mmolPerL_To_mgPerdL(value).ToString("0");
            canModify = true;
        }
        else
        {
            canModify = true;
        }
    }
    private async void btnResetDatabase_Click(object sender, EventArgs e)
    {
        bool remove = await DisplayAlert("Should I delete the WHOLE database, LOSING ALL DATA" +
            "\nAfter creation of the new database the program will shut down."
            ,"", "Yes", "No");
        if (remove)
        {
            // deleting the database file
            // after deletion the software will automatically re-create the database
            if (!blGeneral.DeleteDatabase())
            {
                await DisplayAlert("", "Error in deleting database file. File NOT deleted", "OK");
            }
            blGeneral.CreateNewDatabase(); // re-create the database
            // close program
            btnStopApplication_Click(this, EventArgs.Empty);
        }
    }
    
    private async void btnCopyProgramsFiles_Click(object sender, EventArgs e)
    {
        try
        {
            // Show loading indicator
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromMinutes(5)).Token;
            
            // Show progress dialog
            bool continueExport = await DisplayAlert(
                "Esportazione File", 
                "L'app proverà diversi metodi per esportare i file.\n" +
                "Su dispositivi Huawei/Xiaomi potrebbero essere necessari permessi aggiuntivi.\n\n" +
                "Continuare?", 
                "Sì", "No");
                
            if (!continueExport)
                return;
            
            // Try the enhanced file export method first
            bool success = await ExportFilesUsingCommunityToolkit(cancellationToken);
            
            if (!success)
            {
                // Try enhanced Android file helper
                success = await TryEnhancedAndroidExport();
            }
            
            if (!success)
            {
                // Fallback to traditional method
                success = await ExportFilesTraditionalMethod();
            }
            
            if (!success)
            {
                // Final fallback - offer to share files instead
                bool shareFiles = await DisplayAlert(
                    "Esportazione Fallita", 
                    "Impossibile salvare i file nelle cartelle pubbliche.\n" +
                    "Vuoi condividere i file tramite altre app invece?", 
                    "Condividi", "Annulla");
                    
                if (shareFiles)
                {
                    success = await ShareExportedFiles();
                }
            }
            
            if (success)
            {
                await DisplayAlert("Successo", "File esportati correttamente!", "OK");
            }
            else
            {
                await DisplayAlert("Errore", 
                    "Impossibile esportare i file.\n\n" +
                    "Su dispositivi Huawei/Xiaomi:\n" +
                    "1. Vai in Impostazioni > App > GlucoMan > Autorizzazioni\n" +
                    "2. Abilita tutte le autorizzazioni per 'Archiviazione'\n" +
                    "3. Disabilita l'ottimizzazione batteria per GlucoMan\n" +
                    "4. Riprova l'operazione", "OK");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("btnCopyProgramsFiles_Click", ex);
            await DisplayAlert("Errore", $"Errore durante l'esportazione: {ex.Message}", "OK");
        }
    }

    private async Task<bool> ExportFilesUsingCommunityToolkit(CancellationToken cancellationToken)
    {
        try
        {
            General.LogOfProgram.Debug("Starting export using Community Toolkit FileSaver");
            
            // Get files to export
            var filesToExport = GetFilesToExport();
            if (!filesToExport.Any())
            {
                General.LogOfProgram.Error("No files found to export", new Exception("No files found to export"));
                return false;
            }

            // Create GlucoMan folder in Downloads directory
            string downloadFolder = "";
            string glucoManExportFolder = "";

#if ANDROID
            // For Android, use the public Downloads directory
            downloadFolder = AndroidEnvironment.GetExternalStoragePublicDirectory(AndroidEnvironment.DirectoryDownloads)?.AbsolutePath ?? "";
            if (string.IsNullOrEmpty(downloadFolder))
            {
                // Fallback to standard Downloads path
                downloadFolder = Path.Combine("/storage/emulated/0", "Download");
            }
#elif WINDOWS
            // For Windows, use the user's Downloads folder
            downloadFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
#else
            // For other platforms, fallback to Documents
            downloadFolder = FileSystem.AppDataDirectory;
#endif

            glucoManExportFolder = Path.Combine(downloadFolder, "GlucoMan");
            
            try
            {
                // Create the GlucoMan directory in Downloads if it doesn't exist
                Directory.CreateDirectory(glucoManExportFolder);
                General.LogOfProgram.Debug($"Created/verified GlucoMan export folder: {glucoManExportFolder}");
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error($"Failed to create export directory: {glucoManExportFolder}", ex);
                return false;
            }

            int successCount = 0;
            int totalCount = filesToExport.Count;

            foreach (var (sourceFile, fileName) in filesToExport)
            {
                try
                {
                    if (!File.Exists(sourceFile))
                    {
                        General.LogOfProgram.Debug($"Skipping non-existent file: {sourceFile}");
                        continue;
                    }

                    // Create destination path in GlucoMan folder
                    string destinationPath = Path.Combine(glucoManExportFolder, fileName);

                    // Copy file directly to the destination
                    File.Copy(sourceFile, destinationPath, true);
                    
                    successCount++;
                    General.LogOfProgram.Debug($"Successfully exported: {fileName} to {destinationPath}");
                    
                    // Show individual success toast
                    var toast = Toast.Make($"Salvato: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show(cancellationToken);
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error($"Error exporting file {fileName}", ex);
                    
                    // Try using FileSaver as fallback for this file
                    try
                    {
                        var fileBytes = await File.ReadAllBytesAsync(sourceFile, cancellationToken);
                        using var stream = new MemoryStream(fileBytes);
                        var result = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
                        
                        if (result.IsSuccessful)
                        {
                            successCount++;
                            General.LogOfProgram.Debug($"Successfully exported via FileSaver fallback: {fileName} to {result.FilePath}");
                            
                            var toast = Toast.Make($"Salvato: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                            await toast.Show(cancellationToken);
                        }
                    }
                    catch (Exception fallbackEx)
                    {
                        General.LogOfProgram.Error($"FileSaver fallback also failed for {fileName}", fallbackEx);
                    }
                }
            }

            General.LogOfProgram.Debug($"Export completed: {successCount}/{totalCount} files exported to {glucoManExportFolder}");
            
            if (successCount > 0)
            {
                // Show final success message with location
                var finalToast = Toast.Make($"File esportati in Downloads/GlucoMan ({successCount}/{totalCount})", CommunityToolkit.Maui.Core.ToastDuration.Long);
                await finalToast.Show(cancellationToken);
            }
            
            return successCount > 0;
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ExportFilesUsingCommunityToolkit", ex);
            return false;
        }
    }

#if ANDROID
    private async Task<bool> TryEnhancedAndroidExport()
    {
        try
        {
            General.LogOfProgram.Debug("Trying enhanced Android export method");
            
            var filesToExport = GetFilesToExport();
            if (!filesToExport.Any())
            {
                return false;
            }

            int successCount = 0;
            var exportResults = new List<string>();

            foreach (var (sourceFile, fileName) in filesToExport)
            {
                if (!File.Exists(sourceFile))
                    continue;

                var result = await EnhancedFileHelper.SaveFileWithFallback(sourceFile, fileName);
                if (result.Success)
                {
                    successCount++;
                    exportResults.Add($"? {fileName} ? {result.Path}");
                    
                    // Show individual success toast
                    var toast = Toast.Make($"Salvato: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                }
                else
                {
                    exportResults.Add($"? {fileName} ? Fallito");
                }
            }

            // Show detailed results
            if (exportResults.Any())
            {
                string resultMessage = "Risultati esportazione:\n\n" + string.Join("\n", exportResults);
                General.LogOfProgram.Debug(resultMessage);
            }

            return successCount > 0;
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("TryEnhancedAndroidExport", ex);
            return false;
        }
    }
#else
    private Task<bool> TryEnhancedAndroidExport() => Task.FromResult(false);
#endif

#if ANDROID
    private async Task<bool> ShareExportedFiles()
    {
        try
        {
            General.LogOfProgram.Debug("Attempting to share exported files");
            
            var filesToShare = GetFilesToExport();
            if (!filesToShare.Any())
            {
                return false;
            }

            int shareCount = 0;
            
            foreach (var (sourceFile, fileName) in filesToShare)
            {
                if (!File.Exists(sourceFile))
                    continue;

                bool shared = await EnhancedFileHelper.ShareFile(sourceFile, fileName);
                if (shared)
                {
                    shareCount++;
                    General.LogOfProgram.Debug($"File shared successfully: {fileName}");
                    
                    // Small delay between shares to avoid overwhelming the system
                    await Task.Delay(1000);
                }
            }

            return shareCount > 0;
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ShareExportedFiles", ex);
            return false;
        }
    }
#else
    private Task<bool> ShareExportedFiles() => Task.FromResult(false);
#endif

    private async Task<bool> ExportFilesTraditionalMethod()
    {
        try
        {
            General.LogOfProgram.Debug("Fallback to traditional export method");
            
#if ANDROID
            if (!await AndroidExternalFilesHelper.ProgramHasPermissions())
            {
                return false;
            }       
#endif
            return await blGeneral.ExportProgramsFilesAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ExportFilesTraditionalMethod", ex);
            return false;
        }
    }

    private List<(string SourceFile, string FileName)> GetFilesToExport()
    {
        var files = new List<(string, string)>();
        
        try
        {
            // Add database file
            if (File.Exists(Common.PathAndFileDatabase))
            {
                files.Add((Common.PathAndFileDatabase, Path.GetFileName(Common.PathAndFileDatabase)));
            }

            // Add log files
            if (Directory.Exists(Common.PathLogs))
            {
                var logFiles = Directory.GetFiles(Common.PathLogs);
                foreach (var logFile in logFiles)
                {
                    files.Add((logFile, Path.GetFileName(logFile)));
                }
            }

            // Add parameters log if exists
            if (File.Exists(Common.PathAndFileLogOfParameters))
            {
                files.Add((Common.PathAndFileLogOfParameters, Path.GetFileName(Common.PathAndFileLogOfParameters)));
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("GetFilesToExport", ex);
        }

        return files;
    }

    private async void btnImport_Click(object sender, EventArgs e)
    {
        bool import = await DisplayAlert(
            "Import the foods from a database",
            "Select the SQLite database file to import.\n" +
            "ATTENTION: the selected file may REPLACE the current app database.\n" +
            "You should backup the current database before continuing!\n\nContinue?",
            "Yes", "No");

        if (!import)
            return;

        await ImportFoodsFromExternalDatabase ();
    }
    private Task ImportFoodsFromExternalDatabase()
    {
        return Task.FromException(new NotImplementedException());
    }

    private async Task ImportDatabaseFromExternalFile()
    {
        try
        {
            // Use Community Toolkit file picker with better error handling
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                //{ DevicePlatform.Android, new[] { "application/x-sqlite3", "application/octet-stream", ".sqlite", ".db", "*/*" } },
                { DevicePlatform.Android, new[] { "application/x-sqlite3", "application/octet-stream", ".sqlite", ".db" } },
                { DevicePlatform.iOS, new[] { "public.data", ".sqlite", ".db" } },
                { DevicePlatform.WinUI, new[] { ".sqlite", ".db" } },
                { DevicePlatform.MacCatalyst, new[] { ".sqlite", ".db" } }
            });

            var picked = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select GlucoMan database (.sqlite/.db)",
                FileTypes = customFileType
            });

            if (picked is null)
                return;

            General.LogOfProgram.Debug($"Selected file for database import: {picked.FileName} (Full path: {picked.FullPath})");

            // Create backup of current database first
            await CreateDatabaseBackup();

            // Copy the selected file to GlucoMan folder in app directory
            //string tempImportName = "readGlucomanData.sqlite";
            //string glucoManFolder = Path.Combine(FileSystem.AppDataDirectory, "GlucoMan");
            //string tempImportPath = Path.Combine(glucoManFolder, tempImportName);
            string glucoManFolder =picked.FullPath.Replace(picked.FileName, "");
            string tempImportPath = picked.FullPath;
            try
            {
                //// Create the GlucoMan directory if it doesn't exist
                //Directory.CreateDirectory(glucoManFolder);
                General.LogOfProgram.Debug($"Created/verified GlucoMan folder: {glucoManFolder}");
                
                using var src = await picked.OpenReadAsync();
                using var dst = File.Create(Common.PathAndFileDatabase);
                await src.CopyToAsync(dst);

                General.LogOfProgram.Debug($"File copied to GlucoMan folder: {tempImportPath}");
                
                // Verify the copied file
                if (!File.Exists(tempImportPath))
                {
                    throw new FileNotFoundException("Copied file not found in GlucoMan folder");
                }

                var fileInfo = new FileInfo(tempImportPath);
                General.LogOfProgram.Debug($"Copied file size: {fileInfo.Length} bytes");
                
                if (fileInfo.Length == 0)
                {
                    throw new InvalidDataException("Copied file is empty");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error copying selected file to GlucoMan folder", ex);
                await DisplayAlert("Error!", $"Error copying selected file: {ex.Message}", "OK");
                return;
            }

            // Import from the internal file
            bool success = await blGeneral.ReadDatabaseFromExternal(Common.PathAndFileDatabase, tempImportPath);
            
            if (!success)
            {
                General.LogOfProgram.Error("ImportDatabaseFromExternal returned false", new Exception("ImportDatabaseFromExternal returned false"));
                await DisplayAlert("", "Error in reading from selected file to app's database", "OK");
            }
            else
            {
                General.LogOfProgram.Debug("Database reading completed successfully");
                await DisplayAlert("", "Database reading completed successfully.", "OK");
            }

            // Clean up temporary file
            try
            {
                if (File.Exists(tempImportPath))
                {
                    File.Delete(tempImportPath);
                    General.LogOfProgram.Debug($"Temporary file cleaned up: {tempImportPath}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error cleaning up temporary file", ex);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ImportDatabaseFile", ex);
            await DisplayAlert("Error!", $"Error during database reading: {ex.Message}", "OK");
        }
    }

    private async Task CreateDatabaseBackup()
    {
        try
        {
            if (File.Exists(Common.PathAndFileDatabase))
            {
                string backupName = $"database_backup_{DateTime.Now:yyyyMMdd_HHmmss}GlucomanData.sqlite";
                string backupPath = Path.Combine(FileSystem.AppDataDirectory, backupName);
                
                File.Copy(Common.PathAndFileDatabase, backupPath, true);
                General.LogOfProgram.Debug($"Database backup created: {backupPath}");
                
                var toast = Toast.Make($"Backup creato: {backupName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                await toast.Show();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("CreateDatabaseBackup", ex);
        }
    }

    private void btnStopApplication_Click(object sender, EventArgs e)
    {
        // stops the application shutting all its processes
        Application.Current?.Quit();
        // Stops the application shutting all its processes
        Process.GetCurrentProcess().CloseMainWindow();
        Process.GetCurrentProcess().Close();
    }
    private async void btnShowErrorLog_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            string fileContent = File.ReadAllText(General.LogOfProgram.ErrorsFile);
            await Navigation.PushAsync(new ShowTextPage(fileContent));
        }
        catch
        {
            await DisplayAlert("Reading not possible", "File not existing or not accessible", "Ok");
        }
    }
    private async void btnDeleteErrorLog_ClickAsync(object sender, EventArgs e)
    {
        General.LogOfProgram.EraseContentOfAllLogs();
        await DisplayAlert("", "Done!", "Ok");
    }
    
    private async void btnReadDatabase_Click(object sender, EventArgs e)
    {
        bool read = await DisplayAlert("Read database from external storage",
            "Select the SQLite database file to import.\n" +
            "ATTENTION: the selected file will REPLACE the current app database.\n" +
            "You should backup the current database before continuing!\n\nContinue?",
            "Yes", "No");
        if (!read)
            return;
        await ImportDatabaseFromExternalFile();
    }
    
    private async void btnSettings_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
    private void btnMenu_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}
