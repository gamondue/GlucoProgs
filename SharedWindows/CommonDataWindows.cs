using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal static partial class Common
    {
        // program's default path and files. 
        internal static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //internal static string PathExe = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        //internal static string PathAndFileExe = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(8);

        // save in user's documents folder
        internal static string PathConfigurationData = Path.Combine(PathUser , @"GlucoMan\Config\");
        internal static string PathProgramsData = Path.Combine(PathUser , @"GlucoMan\Data\");
        //internal static string PathUsersDownload = ; // !!!! TODO find how to save on the download folder of user; 
        internal static string PathLogs = Path.Combine(PathUser , @"GlucoMan\Logs\");
        internal static string PathDatabase = Path.Combine(PathUser, @"GlucoMan\Data\");
        internal static string PathAndFileDatabase = Path.Combine(PathDatabase, @"GlucoManData.sqlite");

        //internal static CommonObjects CommonObj;
    }
}
