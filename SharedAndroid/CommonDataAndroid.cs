using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        public static string NomeFileDatabase = @"GlucoManData.Sqlite";

        public static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        public static string CommonApplicationPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string LocalApplicationPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        public static string AppDataDirectoryPath = Xamarin.Essentials.FileSystem.AppDataDirectory;
        public static string CacheDirectoryPath = Xamarin.Essentials.FileSystem.CacheDirectory;
        public static string myDocPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        
        //public static string externalPublicPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Type: Environment.DirectoryDocuments)?.AbsolutePath; 
        
        // fix the former using some hints from here: 
        // https://stackoverflow.com/questions/54591881/xamarin-forms-platform-android-does-not-exist-after-xamarin-update-from-2-5-to-3

        //public static string PathConfigurationData = Path.Combine(PathApplication, @"Config");
        //public static string PathProgramsData = Path.Combine(PathApplication, @"Data");
        //public static string PathLogs = Path.Combine(PathApplication , @"Logs");
        public static string PathConfigurationData = Path.Combine(AppDataDirectoryPath, @"Config");
        public static string PathProgramsData = Path.Combine(PathUser, @"Data");
        public static string PathLogs = Path.Combine(PathUser , @"Logs");
        public static string PathDatabase = Path.Combine(LocalApplicationPath, @"GlucoMan/Data");
        public static string PathAndFileDatabase = Path.Combine(PathDatabase, NomeFileDatabase);
        public static string PathCommand = Path.Combine(PathDatabase, NomeFileDatabase);
    }
}
