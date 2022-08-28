using System.IO;

namespace GlucoMan
{
    public static partial class Common
    {
        public static void SetGeneralPaths()
        {
            // general paths to be used by the rest of the program 

            PathProgramsData = Path.Combine(Common.LocalApplicationPath, @"Data");
            PathLogs = Path.Combine(Common.LocalApplicationPath, @"Logs");
            PathDatabase = Path.Combine(Common.LocalApplicationPath, @"Data");
            PathAndFileDatabase = Path.Combine(Common.PathDatabase, Common.DatabaseFileName);
            PathImportExport = Path.Combine(Common.ExternalPublicPath, @"Glucoman");
            PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, LogOfParametersFileName);
        }
        public static void SetAndroidPaths()
        {
            PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
            CommonApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
            LocalApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            AppDataDirectoryPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
            CacheDirectoryPath = Xamarin.Essentials.FileSystem.CacheDirectory;

            myDocPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        }
        public static void PlatformSpecificInitializations()
        {

        }
    }
}
