using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedData
{
    internal static class CommonData
    {
        internal static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        internal static string PathApplication = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static string PathUsersDownload = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        // fix the former using some hints from here: 
        // https://stackoverflow.com/questions/54591881/xamarin-forms-platform-android-does-not-exist-after-xamarin-update-from-2-5-to-3

        //internal static string PathConfigurationData = Path.Combine(PathApplication, @"GlucoMan/Config");
        //internal static string PathProgramsData = Path.Combine(PathApplication, @"GlucoMan/Data");
        //internal static string PathAndNameLogFile = Path.Combine(PathApplication, @"GlucoMan/logs/GlucoMan_Log.txt");
        internal static string PathConfigurationData = Path.Combine(PathApplication, @"Config");
        internal static string PathProgramsData = Path.Combine(PathApplication, @"Data");
        internal static string PathAndNameLogFile = Path.Combine(PathApplication, @"Logs/GlucoMan_Log.txt");

        internal static CommonObjects CommonObj;
    }
}
