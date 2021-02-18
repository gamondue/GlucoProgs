using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedData
{
    internal static class CommonData
    {
        internal static string PathConfigurationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string PathUsersDownload = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
            , Android.OS.Environment.DirectoryDownloads);
        internal static string PathAndNameLogFile = PathConfigurationData + @".\GlucoMan_ErrorLog.txt";
    }
}
