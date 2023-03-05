using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        public static string NomeFileDatabase = @"GlucoManData.Sqlite";
        public static string PathUsersDownload;
        // program's default path and files. 
        public static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        // PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        
        //public static string PathExe = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        //public static string PathAndFileExe = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(8);

        // general paths to be used by the rest of the program 
        public static string PathConfigurationData = Path.Combine(PathUser, @"GlucoMan\Config\");
        public static string PathProgramsData = Path.Combine(PathUser, @"GlucoMan\Data\");
        //PathProgramsData = Path.Combine(Common.LocalApplicationPath, @"Data");
        public static string PathLogs = Path.Combine(PathUser, @"GlucoMan\Logs\");
        // PathLogs = Path.Combine(Common.LocalApplicationPath, @"Logs");
        public static string PathDatabase = Path.Combine(PathUser, @"GlucoMan\Data\");
        // PathDatabase = Path.Combine(Common.LocalApplicationPath, @"Data");
        public static string PathAndFileDatabase = Path.Combine(PathDatabase, NomeFileDatabase);
        // PathAndFileDatabase = Path.Combine(Common.PathDatabase, Common.DatabaseFileName);
        public static string PathImportExport = Path.Combine(PathUser, @"GlucoMan\ImportExport\");
        // PathImportExport = Path.Combine(Common.ExternalPublicPath, @"Glucoman");
        public static string PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, Common.LogOfParametersFileName);
        // PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, LogOfParametersFileName);

        public static void SetPaths()
        {
//            CommonApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
//            LocalApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

//            AppDataDirectoryPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
//            CacheDirectoryPath = Xamarin.Essentials.FileSystem.CacheDirectory;

//            myDocPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
//            if (DeviceInfo.Current.Platform == DevicePlatform.WinUi)
//            {
//                PathUsersDownload = System.Convert.ToString(Microsoft.Win32.Registry.GetValue(
//                             @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
//                            , "{374DE290-123F-4565-9164-39C4925E467B}"
//                            , String.Empty));
//                    public static string PathConfigurationData = Path.Combine(PathUser, @"GlucoMan\Config\");
//        public static string PathProgramsData = Path.Combine(PathUser, @"GlucoMan\Data\");
//        public static string PathLogs = Path.Combine(PathUser, @"GlucoMan\Logs\");
//        public static string PathDatabase = Path.Combine(PathUser, @"GlucoMan\Data\");
//        public static string PathAndFileDatabase = Path.Combine(PathDatabase, NomeFileDatabase);
//        public static string PathImportExport = Path.Combine(PathUser, @"GlucoMan\ImportExport\");
//        public static string PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, Common.LogOfParametersFileName);
//    }
//        else
//{
//    //PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile); 
//    CommonApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
//    LocalApplicationPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

//    AppDataDirectoryPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
//    CacheDirectoryPath = Xamarin.Essentials.FileSystem.CacheDirectory;

//    myDocPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
//}
}
    }
}
