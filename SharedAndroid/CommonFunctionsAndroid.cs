using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedFunctions
{
    internal static partial class CommonFunctions
    {
        internal static void NotifyError(string Error)
        {
            // TODO: make an error beep
        }
        internal static void Initializations()
        {
            MakeFolderIfDontExist(CommonData.PathConfigurationData);
            MakeFolderIfDontExist(CommonData.PathProgramsData);
        }
    }
}
