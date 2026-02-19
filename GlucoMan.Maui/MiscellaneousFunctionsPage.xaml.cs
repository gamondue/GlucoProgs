using gamon;
using GlucoMan;
using GlucoMan.Maui.Services;
using System.Diagnostics;
using Microsoft.Maui.Storage;
using System.IO;
using CommunityToolkit.Maui.Storage;
using System.Text;
using CommunityToolkit.Maui.Alerts;
using GlucoMan.Maui.Resources.Strings;

#if ANDROID
using AndroidEnvironment = Android.OS.Environment;
#endif

namespace GlucoMan.Maui;

public partial class MiscellaneousFunctionsPage : ContentPage
{
    GlucoMan.BL_General blGeneral = new GlucoMan.BL_General();
    bool canModify = true;
    private readonly LocalizationService _localizationService;
    
    public MiscellaneousFunctionsPage()
    {
        InitializeComponent();
        
        // Get LocalizationService from DI
        _localizationService = Application.Current.Handler.MauiContext.Services.GetService<LocalizationService>();
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
        bool remove = await DisplayAlert(AppStrings.ResetDatabase,
            AppStrings.ConfirmAction,
            AppStrings.Yes, AppStrings.No);
        if (remove)
        {
            // deleting the database file
            // after deletion the software will automatically re-create the database
            if (!blGeneral.DeleteDatabase())
            {
                await DisplayAlert(AppStrings.Error, AppStrings.LoadError, AppStrings.OK);
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
                AppStrings.ExportData,
                AppStrings.ConfirmAction,
                AppStrings.Yes, AppStrings.No);
                
            if (!continueExport)
                return;
            
            // Try the enhanced file export method first
            bool success = await ExportFilesUsingCommunityToolkit(cancellationToken);
            
            if (!success)
            {
                // Try enhanced Android file helper
                success = await TryEnhancedAndroidExport();
            }
            
#if ANDROID
            // Su Android, prova fallback al metodo tradizionale
            if (!success)
            {
                // Fallback to traditional method
                success = await ExportFilesTraditionalMethod();
            }
#endif
            
            if (!success)
            {
                // Final fallback - offer to share files instead (solo Android)
#if ANDROID
                bool shareFiles = await DisplayAlert(
                    AppStrings.Error,
                    AppStrings.LoadError,
                    AppStrings.Share, AppStrings.Cancel);
                    
                if (shareFiles)
                {
                    success = await ShareExportedFiles();
                }
#endif
            }
            
            if (success)
            {
                await DisplayAlert(AppStrings.Success, "Files exported successfully!", AppStrings.OK);
            }
            else
            {
                await DisplayAlert(AppStrings.Error, 
                    "Unable to export files.\n\nOn Huawei/Xiaomi devices:\n1. Go to Settings > Apps > GlucoMan > Permissions\n2. Enable all 'Storage' permissions\n3. Disable battery optimization for GlucoMan\n4. Retry the operation", AppStrings.OK);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("btnCopyProgramsFiles_Click", ex);
            await DisplayAlert(AppStrings.Error, $"Error during export: {ex.Message}", AppStrings.OK);
        }
    }

    private async Task<bool> ExportFilesUsingCommunityToolkit(CancellationToken cancellationToken)
    {
        try
        {
            General.LogOfProgram.Debug("Starting export using Community Toolkit FileSaver");
            
            // Get files to export
            var logAndOtherFilesToExport = GetFilesToExport();
            if (!logAndOtherFilesToExport.Any())
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
            int totalCount = logAndOtherFilesToExport.Count;

            // Normalizza i percorsi delle cartelle interne per il confronto
            string containerPhotosInternal = Path.GetFullPath(Common.GetContainerPhotosPath());
            string containerPhotosExport = Path.Combine(glucoManExportFolder, "ContainerPhotos");
            // Crea la cartella ContainerPhotos solo se serve
            bool containerPhotosDirCreated = false;

            string tracksInternal = Path.GetFullPath(GlucoMan.BL_GpsTracking.GetTracksFolder());
            string tracksExport = Path.Combine(glucoManExportFolder, "Tracks");
            // Crea la cartella Tracks solo se serve
            bool tracksDirCreated = false;

            foreach (var (sourceFile, fileName) in logAndOtherFilesToExport)
            {
                try
                {
                    if (!File.Exists(sourceFile))
                    {
                        General.LogOfProgram.Debug($"Skipping non-existent file: {sourceFile}");
                        continue;
                    }

                    string destinationPath;
                    // Normalizza il percorso del file sorgente per il confronto
                    string normalizedSourceFile = Path.GetFullPath(sourceFile);
                    string sourceFileDirectory = Path.GetDirectoryName(normalizedSourceFile) ?? string.Empty;
                    
                    // Se il file proviene dalla cartella interna ContainerPhotos, esportalo nella sottocartella
                    if (sourceFileDirectory.Equals(containerPhotosInternal, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!containerPhotosDirCreated)
                        {
                            Directory.CreateDirectory(containerPhotosExport);
                            containerPhotosDirCreated = true;
                            General.LogOfProgram.Debug($"Created ContainerPhotos export folder: {containerPhotosExport}");
                        }
                        destinationPath = Path.Combine(containerPhotosExport, fileName);
                    }
                    // Se il file proviene dalla cartella interna Tracks, esportalo nella sottocartella
                    else if (sourceFileDirectory.Equals(tracksInternal, StringComparison.OrdinalIgnoreCase))
                    {
                        if (!tracksDirCreated)
                        {
                            Directory.CreateDirectory(tracksExport);
                            tracksDirCreated = true;
                            General.LogOfProgram.Debug($"Created Tracks export folder: {tracksExport}");
                        }
                        destinationPath = Path.Combine(tracksExport, fileName);
                    }
                    else
                    {
                        destinationPath = Path.Combine(glucoManExportFolder, fileName);
                    }

                    // Copia il file
                    File.Copy(sourceFile, destinationPath, true);
                    successCount++;
                    General.LogOfProgram.Debug($"Successfully exported: {fileName} to {destinationPath}");

                    // Mostra toast
                    var toast = Toast.Make($"Saved: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show(cancellationToken);
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error($"Error exporting file {fileName}", ex);
#if ANDROID
                    // Prova fallback FileSaver solo su Android
                    try
                    {
                        var fileBytes = await File.ReadAllBytesAsync(sourceFile, cancellationToken);
                        using var stream = new MemoryStream(fileBytes);
                        var result = await FileSaver.Default.SaveAsync(fileName, stream, cancellationToken);
                        if (result.IsSuccessful)
                        {
                            successCount++;
                            General.LogOfProgram.Debug($"Successfully exported via FileSaver fallback: {fileName} to {result.FilePath}");
                            var toast = Toast.Make($"Saved: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                            await toast.Show(cancellationToken);
                        }
                    }
                    catch (Exception fallbackEx)
                    {
                        General.LogOfProgram.Error($"FileSaver fallback also failed for {fileName}", fallbackEx);
                    }
#endif
                }
            }

            General.LogOfProgram.Debug($"Export completed: {successCount}/{totalCount} files exported to {glucoManExportFolder}");
            
            if (successCount > 0)
            {
                // Show final success message with location
                try
                {
                    var finalToast = Toast.Make($"Files exported to Downloads/GlucoMan ({successCount}/{totalCount})", CommunityToolkit.Maui.Core.ToastDuration.Long);
                    await finalToast.Show(cancellationToken);
                }
                catch (Exception ex)
                {
                    General.LogOfProgram?.Debug($"Error showing final toast: {ex.Message}");
                    // Continue anyway - files were exported successfully
                }
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
                    var toast = Toast.Make($"Saved: {fileName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
                    await toast.Show();
                }
                else
                {
                    exportResults.Add($"? {fileName} ? Failed");
                }
            }

            // Show detailed results
            if (exportResults.Any())
            {
                string resultMessage = "Export results:\n\n" + string.Join("\n", exportResults);
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

            // Add container photos if present
            try
            {
                string containerPhotosFolder = Common.GetContainerPhotosPath();
                if (Directory.Exists(containerPhotosFolder))
                {
                    var photoFiles = Directory.GetFiles(containerPhotosFolder);
                    foreach (var photo in photoFiles)
                    {
                        // Use filename only to place in export folder; avoid duplicate names by prefixing with folder name if necessary
                        string fileName = Path.GetFileName(photo);
                        files.Add((photo, fileName));
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("GetFilesToExport - adding container photos", ex);
            }

            // Add GPS tracks if present
            try
            {
                string tracksFolder = GlucoMan.BL_GpsTracking.GetTracksFolder();
                if (Directory.Exists(tracksFolder))
                {
                    var trackFiles = Directory.GetFiles(tracksFolder);
                    foreach (var trackFile in trackFiles)
                    {
                        string fileName = Path.GetFileName(trackFile);
                        files.Add((trackFile, fileName));
                    }
                    General.LogOfProgram?.Debug($"GetFilesToExport - added {trackFiles.Length} track files from {tracksFolder}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("GetFilesToExport - adding GPS tracks", ex);
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
            AppStrings.ImportFoods,
            AppStrings.ConfirmAction,
            AppStrings.Yes, AppStrings.No);

        if (!import)
            return;

        await ImportFoodsFromExternalDatabase ();
    }
    private Task ImportFoodsFromExternalDatabase()
    {
        return Task.FromException(new NotImplementedException());
    }
    private async Task ImportDatabaseAndFilesFromExternalFolder()
    {
        try
        {
            // Use Community Toolkit file picker with better error handling
            var customFileType = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
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

            // Ask user to confirm importing associated photos from external ContainerPhotos folder
            const string photosSubfolderName = "ContainerPhotos";
            bool importPhotos = await DisplayAlert(
                AppStrings.ImportImages,
                string.Format(AppStrings.ImportImages_Description, photosSubfolderName),
                AppStrings.Yes,
                AppStrings.No);

            if (!importPhotos)
            {
                General.LogOfProgram.Debug("User declined importing photos during database import");
            }

            // IMPORTANT: Close database connection BEFORE any file operations
            // This releases the file lock on the SQLite database
            CloseDatabaseConnection();

            // Small delay to ensure file handle is released
            await Task.Delay(200);

            // Create backup of current database (now that connection is closed)
            await CreateDatabaseBackup();

            // Copy the selected file to GlucoMan folder in app directory
            string glucoManFolder = picked.FullPath.Replace(picked.FileName, "");
            try
            {
                General.LogOfProgram.Debug($"Source folder: {glucoManFolder}");

                // Read from the picked file and write to the database location
                using (var src = await picked.OpenReadAsync())
                using (var dst = File.Create(Common.PathAndFileDatabase))
                {
                    await src.CopyToAsync(dst);
                }

                General.LogOfProgram.Debug($"File copied to: {Common.PathAndFileDatabase}");

                // Verify the copied file
                if (!File.Exists(Common.PathAndFileDatabase))
                {
                    throw new FileNotFoundException("Copied file not found in app folder");
                }

                var fileInfo = new FileInfo(Common.PathAndFileDatabase);
                General.LogOfProgram.Debug($"Copied file size: {fileInfo.Length} bytes");

                if (fileInfo.Length == 0)
                {
                    throw new InvalidDataException("Copied file is empty");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error copying selected file to app folder", ex);
                // Re-open database before returning with error
                ReopenDatabaseConnection();
                await DisplayAlert(AppStrings.Error, $"Error copying selected file: {ex.Message}", AppStrings.OK);
                return;
            }
            // If user agreed, import photos from external ContainerPhotos folder
            if (importPhotos)
            {
                try
                {
                    // IMPORTANT: On Android, FilePicker returns a temporary cache path, not the actual file location
                    // We need to look in the standard export folder (Downloads/GlucoMan/ContainerPhotos)
                    string externalPhotosFolder;

#if ANDROID
                    // For Android, use the standard Downloads/GlucoMan export folder
                    string downloadFolder = AndroidEnvironment.GetExternalStoragePublicDirectory(AndroidEnvironment.DirectoryDownloads)?.AbsolutePath ?? "";
                    if (string.IsNullOrEmpty(downloadFolder))
                    {
                        downloadFolder = Path.Combine("/storage/emulated/0", "Download");
                    }
                    externalPhotosFolder = Path.Combine(downloadFolder, "GlucoMan", photosSubfolderName);
#else
                    // For other platforms, look in the same folder as the selected database file
                    externalPhotosFolder = Path.Combine(Path.GetDirectoryName(picked.FullPath) ?? string.Empty, photosSubfolderName);
#endif

                    string internalPhotosFolder = Common.GetContainerPhotosPath();

                    // IMPORTANT: Create the internal photos folder BEFORE checking if external folder exists
                    // This ensures the destination exists even if there are no photos to import
                    Directory.CreateDirectory(internalPhotosFolder);

                    General.LogOfProgram.Debug($"Looking for photos in: {externalPhotosFolder}");
                    General.LogOfProgram.Debug($"Target internal folder: {internalPhotosFolder}");
                    General.LogOfProgram.Debug($"Internal folder created/verified: {Directory.Exists(internalPhotosFolder)}");

                    if (Directory.Exists(externalPhotosFolder))
                    {
                        var externalPhotos = Directory.GetFiles(externalPhotosFolder);
                        General.LogOfProgram.Debug($"Found {externalPhotos.Length} photos to import");

                        int copied = 0;
                        foreach (var externalFile in externalPhotos)
                        {
                            try
                            {
                                string destFile = Path.Combine(internalPhotosFolder, Path.GetFileName(externalFile));
                                File.Copy(externalFile, destFile, true);
                                copied++;
                                General.LogOfProgram.Debug($"Copied photo: {Path.GetFileName(externalFile)}");
                            }
                            catch (Exception ex)
                            {
                                General.LogOfProgram.Error($"Error copying photo {externalFile}", ex);
                            }
                        }

                        General.LogOfProgram.Debug($"Successfully imported {copied} of {externalPhotos.Length} photos from {externalPhotosFolder} into {internalPhotosFolder}");
                        if (copied > 0)
                        {
                            var toast = Toast.Make($"Imported {copied} container photos", CommunityToolkit.Maui.Core.ToastDuration.Short);
                            await toast.Show();
                        }
                        else
                        {
                            General.LogOfProgram.Debug("No photos found to import in external photos folder");
                            await DisplayAlert(AppStrings.Warning, $"No photos were found in:\n{externalPhotosFolder}", AppStrings.OK);
                        }
                    }
                    else
                    {
                        string errorMsg = $"External photos folder not found:\n{externalPhotosFolder}";
                        General.LogOfProgram.Debug(errorMsg);
                        await DisplayAlert(AppStrings.Warning, errorMsg, AppStrings.OK);
                    }
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("Error importing photos from external folder", ex);
                    await DisplayAlert(AppStrings.Warning, $"An error occurred while importing photos:\n{ex.Message}\nCheck logs for details.", AppStrings.OK);
                }
            }
            else
            {
                General.LogOfProgram.Debug("User declined importing photos during database import");
                await DisplayAlert(AppStrings.Info, "Database imported successfully, but photos were NOT imported.\n\nTo import photos, run import again and confirm photo import.", AppStrings.OK);
            }

            // Import GPS tracks from external Tracks folder
            try
            {
                const string tracksSubfolderName = "Tracks";
                string externalTracksFolder;

#if ANDROID
                // For Android, use the standard Downloads/GlucoMan export folder
                string downloadFolder = AndroidEnvironment.GetExternalStoragePublicDirectory(AndroidEnvironment.DirectoryDownloads)?.AbsolutePath ?? "";
                if (string.IsNullOrEmpty(downloadFolder))
                {
                    downloadFolder = Path.Combine("/storage/emulated/0", "Download");
                }
                externalTracksFolder = Path.Combine(downloadFolder, "GlucoMan", tracksSubfolderName);
#else
                // For other platforms, look in the same folder as the selected database file
                externalTracksFolder = Path.Combine(Path.GetDirectoryName(picked.FullPath) ?? string.Empty, tracksSubfolderName);
#endif

                string internalTracksFolder = GlucoMan.BL_GpsTracking.GetTracksFolder();

                if (Directory.Exists(externalTracksFolder))
                {
                    Directory.CreateDirectory(internalTracksFolder);
                    var externalTracks = Directory.GetFiles(externalTracksFolder);
                    int copiedTracks = 0;
                    foreach (var externalFile in externalTracks)
                    {
                        try
                        {
                            string destFile = Path.Combine(internalTracksFolder, Path.GetFileName(externalFile));
                            File.Copy(externalFile, destFile, true);
                            copiedTracks++;
                        }
                        catch (Exception ex)
                        {
                            General.LogOfProgram.Error($"Error copying track file {externalFile}", ex);
                        }
                    }

                    General.LogOfProgram.Debug($"Imported {copiedTracks} track files from {externalTracksFolder} into {internalTracksFolder}");
                    if (copiedTracks > 0)
                    {
                        var toast = Toast.Make($"Imported {copiedTracks} GPS tracks", CommunityToolkit.Maui.Core.ToastDuration.Short);
                        await toast.Show();
                    }
                }
                else
                {
                    General.LogOfProgram.Debug($"External tracks folder not found: {externalTracksFolder}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("Error importing tracks from external folder", ex);
                // Don't show alert, just log - tracks are optional
            }

            // Re-open the database with the new file
            ReopenDatabaseConnection();

            General.LogOfProgram.Debug("Database import completed successfully");
            await DisplayAlert(AppStrings.Success, "Database import completed successfully.", AppStrings.OK);
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("ImportDatabaseFile", ex);
            // Ensure database is re-opened even on error
            try { ReopenDatabaseConnection(); } catch { }
            await DisplayAlert(AppStrings.Error, $"Error during database import: {ex.Message}", AppStrings.OK);
        }
    }
    /// <summary>
    /// Closes all database connections to release file locks.
    /// Call this before any file operation on the database file.
    /// </summary>
    private void CloseDatabaseConnection()
    {
        try
        {
            General.LogOfProgram?.Debug("Closing database connection for file operation...");

            // Clear the singleton database reference
            Common.Database = null;

            // Force garbage collection to release any lingering handles
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // SQLite specific: clear the connection pool to release all file handles
            Microsoft.Data.Sqlite.SqliteConnection.ClearAllPools();

            General.LogOfProgram?.Debug("Database connection closed successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("CloseDatabaseConnection", ex);
        }
    }
    /// <summary>
    /// Re-opens the database connection after a file operation.
    /// </summary>
    private void ReopenDatabaseConnection()
    {
        try
        {
            General.LogOfProgram?.Debug("Re-opening database connection...");

            // Re-create the database connection
            Common.Database = new DL_Sqlite();

            // Also update the blGeneral reference
            blGeneral = new BL_General();

            General.LogOfProgram?.Debug("Database connection re-opened successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("ReopenDatabaseConnection", ex);
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
                
                var toast = Toast.Make($"Backup created: {backupName}", CommunityToolkit.Maui.Core.ToastDuration.Short);
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
            var path = General.LogOfProgram.ErrorsFile;
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                await DisplayAlert(AppStrings.Error, "File not existing or not accessible", AppStrings.OK);
                return;
            }

            var fi = new FileInfo(path);
            const long twoMb = 2 * 1024 * 1024;

            string fileContent;

            // If file is large, ask user to confirm and show only the last part to avoid freezing UI
            if (fi.Length > twoMb)
            {
                bool show = await DisplayAlert("Large file", "The error log is large and may take time to load. Show last 200 KB?", "Show", "Cancel");
                if (!show) return;

                const int tailSize = 200 * 1024; // 200 KB
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    long start = Math.Max(0, fs.Length - tailSize);
                    fs.Seek(start, SeekOrigin.Begin);
                    fileContent = await sr.ReadToEndAsync();
                    if (start > 0)
                        fileContent = "... (truncated head of file) ...\n" + fileContent;
                }
            }
            else
            {
                // Read asynchronously using FileStream with shared read so writer can continue writing
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.Default))
                {
                    fileContent = await sr.ReadToEndAsync();
                }
            }

            await Navigation.PushAsync(new ShowTextPage(fileContent));
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("btnShowErrorLog_ClickAsync", ex);
            await DisplayAlert(AppStrings.Error, AppStrings.FileNotExistingOrNotAccessible, AppStrings.OK);
        }
    }
    private async void btnDeleteErrorLog_ClickAsync(object sender, EventArgs e)
    {
        General.LogOfProgram.EraseContentOfAllLogs();
        await DisplayAlert(AppStrings.Success, AppStrings.Done, AppStrings.OK);
    }    
    private async void btnReadDatabase_Click(object sender, EventArgs e)
    {
        bool read = await DisplayAlert(AppStrings.ImportDatabase,
            AppStrings.ConfirmAction,
            AppStrings.Yes, AppStrings.No);
        if (!read)
            return;
        await ImportDatabaseAndFilesFromExternalFolder();
    } 
    private async void btnSettings_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage(_localizationService));
    }
    private void btnMenu_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }
}
