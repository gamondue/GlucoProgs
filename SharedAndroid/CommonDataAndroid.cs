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
        // fix the former using some hints from here: 
        // https://stackoverflow.com/questions/54591881/xamarin-forms-platform-android-does-not-exist-after-xamarin-update-from-2-5-to-3

        // I set this path for pages' status saving so the user can see the files that store previous values: 
        internal static string PathConfigurationData = Path.Combine(PathUsersDownload, @"/GlucoMan/Config");
        internal static string PathProgramsData = Path.Combine(PathUsersDownload, @"/GlucoMan/Data");

        internal static string PathAndNameErrorLogFile = Path.Combine(PathConfigurationData, @"./GlucoMan_ErrorLog.txt");
    }
}
