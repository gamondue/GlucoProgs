using System;
using System.IO;

namespace GlucoMan
{
    public static partial class Common
    {
        public static string DatabaseFileName = @"GlucoManData.Sqlite";

        public static string PathUser;
        public static string CommonApplicationPath;
        public static string LocalApplicationPath;
        public static string AppDataDirectoryPath;
        public static string CacheDirectoryPath;
        public static string myDocPath;

        public static string ExternalPublicPath; 

        // general Paths of the program 
        public static string PathConfigurationData;
        public static string PathProgramsData;
        public static string PathLogs;
        public static string PathDatabase;
        public static string PathAndFileDatabase;
        public static string PathExport;
    }
}
