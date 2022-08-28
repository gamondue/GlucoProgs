using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        public static string NomeFileDatabase = @"GlucoManData.Sqlite";

        // program's default path and files. 
        public static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //public static string PathExe = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
        //public static string PathAndFileExe = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(8);
        // !!!! substitute the next with a more general way of finding the folder when MS will provide a Download folder
        //      in Environment paths 
        public static string PathUsersDownload = System.Convert.ToString(
                        Microsoft.Win32.Registry.GetValue(
                             @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                            , "{374DE290-123F-4565-9164-39C4925E467B}"
                            , String.Empty)); 

        // general paths to be used by the rest of the program 
        public static string PathConfigurationData = Path.Combine(PathUser , @"GlucoMan\Config\");
        public static string PathProgramsData = Path.Combine(PathUser , @"GlucoMan\Data\");
        public static string PathLogs = Path.Combine(PathUser , @"GlucoMan\Logs\");
        public static string PathDatabase = Path.Combine(PathUser, @"GlucoMan\Data\");
        public static string PathAndFileDatabase = Path.Combine(PathDatabase, NomeFileDatabase);
        public static string PathImportExport = Path.Combine(PathUser, @"GlucoMan\ImportExport\"); 
        public static string PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, Common.LogOfParametersFileName);
    }
}
