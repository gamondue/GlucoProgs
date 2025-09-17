using gamon;
using GlucoMan.BusinessLayer;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.IO;
using CommunityToolkit.Maui.Storage;
using System.Text;
using CommunityToolkit.Maui.Alerts;

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
                DisplayAlert("", "Error in deleting database file. File NOT deleted", "OK");
            }
            blGeneral.CreateNewDatabase(); // re-create the database
            // close program
            btnStopApplication_Click(null, null);
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
                General.LogOfProgram.Error("No files found to export", null);
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

                    // Read file content
                    var fileBytes = await File.ReadAllBytesAsync(sourceFile, cancellationToken);
                    using var stream = new MemoryStream(fileBytes);

                    // Use Community Toolkit FileSaver
                    var result = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
                    
                    if (result.IsSuccessful)
                    {
                        successCount++;
                        General.LogOfProgram.Debug($"Successfully exported: {fileName} to {result.FilePath}");
                        
                        // Show individual success toast
                        var toast = Toast.Make($"Salvato: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                        await toast.Show(cancellationToken);
                    }
                    else
                    {
                        General.LogOfProgram.Error($"Failed to export {fileName}: {result.Exception?.Message}", result.Exception);
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error($"Error exporting file {fileName}", ex);
                }
            }

            General.LogOfProgram.Debug($"Export completed: {successCount}/{totalCount} files exported");
            return successCount > 0;
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ExportFilesUsingCommunityToolkit", ex);
            return false;
        }
    }

    private async Task<bool> TryEnhancedAndroidExport()
    {
        try
        {
#if ANDROID
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
#else
            return false;
#endif
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("TryEnhancedAndroidExport", ex);
            return false;
        }
    }

    private async Task<bool> ShareExportedFiles()
    {
        try
        {
#if ANDROID
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
#else
            return false;
#endif
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ShareExportedFiles", ex);
            return false;
        }
    }

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
    private async Task ImportFoodsFromExternalDatabase()
    {
        throw new NotImplementedException();

    }

    private async Task ImportDatabaseFromExternalFile()
    {
        try
        {
            // Use Community Toolkit file picker with better error handling
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                { DevicePlatform.Android, new[] { "application/x-sqlite3", "application/octet-stream", ".sqlite", ".db", "*/*" } },
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

            // Copy the selected file to internal storage
            string tempImportName = "GlucomanData.sqlite";
            string tempImportPath = Path.Combine(FileSystem.AppDataDirectory, tempImportName);

            try
            {
                Directory.CreateDirectory(FileSystem.AppDataDirectory);
                
                using var src = await picked.OpenReadAsync();
                using var dst = File.Create(tempImportPath);
                await src.CopyToAsync(dst);

                General.LogOfProgram.Debug($"File copied to internal storage: {tempImportPath}");
                
                // Verify the copied file
                if (!File.Exists(tempImportPath))
                {
                    throw new FileNotFoundException("Copied file not found in internal storage");
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
                General.LogOfProgram.Error("Error copying selected file to internal storage", ex);
                await DisplayAlert("Error!", $"Error copying selected file: {ex.Message}", "OK");
                return;
            }

            // Import from the internal file
            bool success = await blGeneral.ReadDatabaseFromExternal(Common.PathAndFileDatabase, tempImportPath);
            
            if (!success)
            {
                General.LogOfProgram.Error("ImportDatabaseFromExternal returned false", null);
                await DisplayAlert("", "Error in importing from selected file to app's database", "OK");
            }
            else
            {
                General.LogOfProgram.Debug("Database import completed successfully");
                await DisplayAlert("", "Import completed successfully.", "OK");
            }

            // Clean up temporary file
            try
            {
                if (File.Exists(tempImportPath))
                {
                    File.Delete(tempImportPath);
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error cleaning up temporary import file", ex);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ImportDatabaseFile", ex);
            await DisplayAlert("Error!", $"Error during import: {ex.Message}", "OK");
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

    private async void btnStopApplication_Click(object sender, EventArgs e)
    {
        // stops the application shutting all its processes
        ////Application.Current.MainPage = new AppPage();
        Application.Current.Quit();
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
