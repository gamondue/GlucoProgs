using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin;

namespace GlucoMan
{
    public static partial class Common
    {
        public static void PlatformSpecificInitializations()
        {
            // create the files that will be useful
            // (so they will not give problems of sharing violations when created on need)
            if (!File.Exists(Path.Combine(Common.PathConfigurationData, @"BolusCalculation.txt")))
            {
                File.Create(Path.Combine(Common.PathConfigurationData, @"BolusCalculation.txt"));
                File.Create(Path.Combine(Common.PathConfigurationData, @"InsulinParameters.txt"));
                File.Create(Path.Combine(Common.PathConfigurationData, @"GlucoseMeasurements.txt"));
                File.Create(Path.Combine(Common.PathConfigurationData, @"FoodToHitTargetCarbs.txt"));
                File.Create(Path.Combine(Common.PathConfigurationData, @"HypoPrediction.txt"));
                File.Create(Path.Combine(Common.PathConfigurationData, @"WeighFood.txt"));
                
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_log.txt"));
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_errors.txt"));
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_prompts.txt"));
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_data.txt"));
            }
        }
    }
}
