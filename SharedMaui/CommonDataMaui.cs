using gamon;

namespace GlucoMan
{
    public static partial class Common
    {
        // constructor
        public static void SetGlobalParameters()
        {
            // program's default path and files. 

            // general paths to be used by the rest of the program  
            // conditional compilation for Windows or Android
#if WINDOWS
            PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            LocalApplicationPath = Path.Combine(PathUser, "GlucoMan");
            
            PathConfigurationData = Path.Combine(LocalApplicationPath, "Config");
            PathDatabase = Path.Combine(LocalApplicationPath, "Data");
            PathAndFileDatabase = Path.Combine(PathDatabase, DatabaseFileName);
            PathLogs = Path.Combine(LocalApplicationPath, "Logs");
            PathImportExport = Path.Combine(LocalApplicationPath, "ImportExport");
            PathAndFileLogOfParameters = Path.Combine(PathLogs, LogOfParametersFileName);
#elif ANDROID
            PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            LocalApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            ExternalPublicPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDocuments)?.AbsolutePath;

            PathConfigurationData = Path.Combine(LocalApplicationPath, "");
            PathDatabase = Path.Combine(LocalApplicationPath, "");
            PathAndFileDatabase = Path.Combine(PathDatabase, DatabaseFileName);
            PathLogs = Path.Combine(LocalApplicationPath, "");
            PathImportExport = Path.Combine(ExternalPublicPath, "GlucoMan");
            PathAndFileLogOfParameters = Path.Combine(PathLogs, LogOfParametersFileName);

            CommonApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);

            //AppDataDirectoryPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
            //CacheDirectoryPath = Xamarin.Essentials.FileSystem.CacheDirectory;
            //myDocPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
#else
            PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            LocalApplicationPath = Path.Combine(PathUser, "GlucoMan");
#endif
            General.LogOfProgram = new Logger(PathLogs, true,
                @"DiabetesRecords_Log.txt",
                @"DiabetesRecords_Errors.txt",
                @"DiabetesRecords_Debug.txt",
                @"DiabetesRecords_Prompts.txt",
                @"DiabetesRecords_Data.txt");
        }
    }
}
