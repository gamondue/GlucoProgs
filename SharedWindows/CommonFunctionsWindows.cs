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
            Console.Beep(); 
        }
        internal static void Initializations()
        {
            MakeFolderIfDontExist(CommonData.PathConfigurationData);
            MakeFolderIfDontExist(CommonData.PathProgramsData);
        }
    }
}
