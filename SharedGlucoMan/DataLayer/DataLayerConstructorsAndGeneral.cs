using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SQLite;

namespace GlucoMan
{
    internal partial class DataLayer
    {
        string persistentBolusCalculation = Path.Combine(CommonData.PathConfigurationData, @"BolusCalculation.txt");
        string persistentGlucoseMeasurements = Path.Combine(CommonData.PathConfigurationData, @"GlucoseMeasurements.txt");
        string persistentFoodToEatTarget = Path.Combine(CommonData.PathConfigurationData, @"FoodToHitTargetCarbs.txt");
        string persistentHypoPrediction = Path.Combine(CommonData.PathConfigurationData, @"HypoPrediction.txt");
        string persistentWeighFood = Path.Combine(CommonData.PathConfigurationData, @"WeighFood.txt");
        string persistentFoods = Path.Combine(CommonData.PathConfigurationData, @"Foods.txt");

        string logBolusCalculationsFile = Path.Combine(CommonData.PathConfigurationData, @"LogOfBolusCalculations.tsv");
        
        //[PrimaryKey, AutoIncrement]

    }
}
