using gamon;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace GlucoMan
{
    public static partial class Common
    {
        public static void SetGlobalParameters()
        {
            // program's default path and files. 
            // general paths to be used by the rest of the program

            // conditional compilation for Windows or Android
            PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);            
#if WINDOWS
            // For Windows, use Documents folder for user-accessible data
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            LocalApplicationPath = Path.Combine(documentsPath, "GlucoMan");
            PathImportExport = Path.Combine(LocalApplicationPath, "ImportExport");
#elif ANDROID
            LocalApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            // Use the Downloads folder which is accessible to the user
            string ExternalPublicPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads)?.AbsolutePath;

            CommonApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            PathImportExport = Path.Combine(ExternalPublicPath, "GlucoMan");
#endif
            PathConfigurationData = Path.Combine(LocalApplicationPath, "Config");
            PathDatabase = Path.Combine(LocalApplicationPath, "Data");
            PathAndFileDatabase = Path.Combine(PathDatabase, DatabaseFileName);
            PathLogs = Path.Combine(LocalApplicationPath, "Logs");
            PathAndFileLogOfParameters = Path.Combine(PathLogs, LogOfParametersFileName);

            General.LogOfProgram = new Logger(PathLogs, true,
                @"GlucoMan_Log.txt",
                @"GlucoMan_Errors.txt",
                @"GlucoMan_Debug.txt",
                @"GlucoMan_Prompts.txt",
                @"GlucoMan_Data.txt");
        }

        /// <summary>
        /// Gets the container photos folder path and ensures it exists.
        /// On Windows: uses LocalApplicationPath (Documents\GlucoMan\ContainerPhotos)
        /// On Android: uses FileSystem.AppDataDirectory to ensure consistency
        /// </summary>
        public static string GetContainerPhotosPath()
        {
            string path;
#if WINDOWS
            path = Path.Combine(LocalApplicationPath, "ContainerPhotos");
#elif ANDROID
            // On Android, use FileSystem.AppDataDirectory for consistency with MAUI
            path = Path.Combine(FileSystem.AppDataDirectory, "ContainerPhotos");
#else
            path = Path.Combine(LocalApplicationPath, "ContainerPhotos");
#endif

            // Ensure the directory exists
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                    General.LogOfProgram?.Debug($"Common.GetContainerPhotosPath - Created folder: {path}");
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error($"Common.GetContainerPhotosPath - Failed to create folder: {path}", ex);
            }

            return path;
        }
    }
}
