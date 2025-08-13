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
            LocalApplicationPath = Path.Combine(PathUser, "GlucoMan");
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
    }
}
