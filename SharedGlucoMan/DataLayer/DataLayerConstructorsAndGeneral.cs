using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DataLayer
    {
        string persistentBolusCalculation = Path.Combine(Common.PathConfigurationData, @"BolusCalculation.txt");
        string persistentInsulinParameters= Path.Combine(Common.PathConfigurationData, @"InsulinParameters.txt");
        string persistentGlucoseMeasurements = Path.Combine(Common.PathConfigurationData, @"GlucoseMeasurements.txt");
        string persistentFoodToEatTarget = Path.Combine(Common.PathConfigurationData, @"FoodToHitTargetCarbs.txt");
        string persistentHypoPrediction = Path.Combine(Common.PathConfigurationData, @"HypoPrediction.txt");
        string persistentWeighFood = Path.Combine(Common.PathConfigurationData, @"WeighFood.txt");

        string logBolusCalculationsFile = Path.Combine(Common.PathConfigurationData, @"LogOfBolusCalculations.tsv");
    }
}
