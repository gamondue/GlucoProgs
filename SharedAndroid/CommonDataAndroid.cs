using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        public static string NomeFileDatabase = @"GlucoManData_1.Sqlite";

        //public static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        public static string PathUser = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string PathApplication = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        public static string PathUsersDownload = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // fix the former using some hints from here: 
        // https://stackoverflow.com/questions/54591881/xamarin-forms-platform-android-does-not-exist-after-xamarin-update-from-2-5-to-3

        //public static string PathConfigurationData = Path.Combine(PathApplication, @"Config");
        //public static string PathProgramsData = Path.Combine(PathApplication, @"Data");
        //public static string PathLogs = Path.Combine(PathApplication , @"Logs");
        public static string PathConfigurationData = Path.Combine(PathUser, @"Config");
        public static string PathProgramsData = Path.Combine(PathUser, @"Data");
        public static string PathLogs = Path.Combine(PathUser , @"Logs");
        public static string PathDatabase = Path.Combine(PathUsersDownload, @"Glucoman/Data");
        public static string PathAndFileDatabase = Path.Combine(PathDatabase, NomeFileDatabase);
    }
}
