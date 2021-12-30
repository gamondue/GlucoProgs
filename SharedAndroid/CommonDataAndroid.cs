using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal static partial class Common
    {
        internal static string PathUser = "/data/data/it.ingmonti.glucoman.mobile/files/GlucoMan";

        //internal static string PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile);
        internal static string PathApplication = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        internal static string PathUsersDownload = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        // fix the former using some hints from here: 
        // https://stackoverflow.com/questions/54591881/xamarin-forms-platform-android-does-not-exist-after-xamarin-update-from-2-5-to-3

        //internal static string PathConfigurationData = Path.Combine(PathApplication, @"Config");
        //internal static string PathProgramsData = Path.Combine(PathApplication, @"Data");
        //internal static string PathLogs = Path.Combine(PathApplication , @"Logs");
        internal static string PathConfigurationData = Path.Combine(PathUser, @"Config");
        internal static string PathProgramsData = Path.Combine(PathUser, @"Data");
        internal static string PathLogs = Path.Combine(PathUser , @"Logs");
    }
}
