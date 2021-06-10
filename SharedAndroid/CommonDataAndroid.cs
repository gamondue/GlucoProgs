using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedData
{
    internal static class CommonData
    {
        // the next if we want to save in a place where user cannot see our files
        // internal static string PathConfigurationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        
        // download path of the user: herein the user can see the files
        internal static string PathUsersDownload = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
            , Android.OS.Environment.DirectoryDownloads);
        
        // I set this path for pages' status saving so the user can see the files that store previous values: 
        internal static string PathConfigurationData = PathUsersDownload + @"/GlucoMan";
        internal static string PathAndNameErrorLogFile = PathConfigurationData + @"./GlucoMan_ErrorLog.txt";
    }
}
