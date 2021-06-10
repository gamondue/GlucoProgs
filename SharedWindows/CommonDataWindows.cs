using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SharedData
{
    internal static class CommonData
    {
        // program's default path and files. 
        internal static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        internal static string PathExe = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        internal static string PathAndFileExe = System.Reflection.Assembly.GetExecutingAssembly().CodeBase.Substring(8);

        internal static string PathConfigurationData = PathUser + "\\GlucoMan\\Config";  // save in user's documents folder
        internal static string PathAndNameLogFile = PathConfigurationData + @".\GlucoMan_ErrorLog.txt";
        internal static string PathUsersDownload = PathConfigurationData; // !!!! TODO find how to save on the download folder of user; 
        // not yet uded: 
        internal static string PathAndNameErrorLogFile = PathConfigurationData + @"./GlucoMan_ErrorLog.txt";
    }
}
