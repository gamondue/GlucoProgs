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
        internal static void NotifyError(string Error)
        {
            // TODO: make an error beep
            // TODO: save a log of errors
        }
        internal static void Initializations()
        {
            MakeFolderIfDontExist(CommonData.PathConfigurationData);
            MakeFolderIfDontExist(CommonData.PathProgramsData);
        }
        internal static async void TestSaving()
        {
            string fileName = "txtInsulinCalc.txt";
            string fileContent = "";
            using (var stream = await Xamarin.Essentials.FileSystem.OpenAppPackageFileAsync(fileName))
            {
                using (var reader = new StreamReader(stream))
                {
                    fileContent += await reader.ReadToEndAsync();
                }
            }
        }
    }
}
