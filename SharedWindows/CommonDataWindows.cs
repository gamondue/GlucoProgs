using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        // program's default path and files. 
        public static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //public static string PathExe = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        //public static string PathAndFileExe = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(8);
        public static string NameDatabase = @"GlucoManData.Sqlite"; 
        // save in user's documents folder
        public static string PathConfigurationData = Path.Combine(PathUser , @"GlucoMan\Config\");
        public static string PathProgramsData = Path.Combine(PathUser , @"GlucoMan\Data\");
        //public static string PathUsersDownload = ; // !!!! TODO find how to save on the download folder of user; 
        public static string PathLogs = Path.Combine(PathUser , @"GlucoMan\Logs\");
        public static string PathDatabase = Path.Combine(PathUser, @"GlucoMan\Data\");
        public static string PathAndFileDatabase = Path.Combine(PathDatabase, NameDatabase);

        //public static CommonObjects CommonObj;
    }
}
