using System;
using System.Collections.Generic;
using System.Text;

namespace SharedData
{
    internal static class CommonData
    {
        internal static string PathConfigurationData = @".\";           // ???? save in user's documents folder ???? 
        internal static string PathAndNameLogFile = PathConfigurationData + @".\GlucoMan_ErrorLog.txt";
        internal static string PathUsersDownload = PathConfigurationData; // !!!! TODO find how to save on the download folder of user; 
    }
}
