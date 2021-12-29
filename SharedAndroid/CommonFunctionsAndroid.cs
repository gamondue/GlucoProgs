using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;

namespace SharedFunctions
{
    internal static partial class CommonFunctions
    {
        internal static void Initializations()
        {
            MakeFolderIfDontExist(CommonData.PathConfigurationData);
            MakeFolderIfDontExist(CommonData.PathProgramsData);
            MakeFolderIfDontExist(CommonData.PathAndNameLogFile);
        }
    }
}
