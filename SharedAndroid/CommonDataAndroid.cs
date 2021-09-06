using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedData
{
    internal static class CommonData
    {
        // the next if we want to save in a place where user cannot see our files


        // download path of the user: herein the user can see the files
        internal static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

        // I set this path for pages' status saving so the user can see the files that store previous values: 
        // internal static string PathUsersDownload = Environment.SpecialFolder.ApplicationData.ToString();
        //internal static string PathUsersDownload = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //internal static string PathUsersDownload = Path.Combine(Xamarin.Forms.PlatformConfiguration.Android.OS.Environment.ExternalStorageDirectory.AbsolutePath
        //, Android.OS.Environment.DirectoryDownloads);
        //internal static string PathUsersDownload = PathUser;
        // internal static string PathConfigurationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        internal static string PathConfigurationData = Path.Combine(PathUser, @"GlucoMan/Config");
        internal static string PathProgramsData = Path.Combine(PathUser, @"GlucoMan/Data");
        internal static string PathAndFileDatabase = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "glucoman.db3");

        internal static string PathAndNameErrorLogFile = Path.Combine(PathConfigurationData, @"./GlucoMan_ErrorLog.txt");
    }
}
