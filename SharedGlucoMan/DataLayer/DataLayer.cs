
using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal abstract partial class DataLayer
    {
        internal string persistentBolusCalculation = Path.Combine(Common.PathConfigurationData, @"BolusCalculation.txt");
        internal string persistentInsulinParameters = Path.Combine(Common.PathConfigurationData, @"InsulinParameters.txt");
        internal string persistentGlucoseMeasurements = Path.Combine(Common.PathConfigurationData, @"GlucoseMeasurements.txt");
        internal string persistentFoodToEatTarget = Path.Combine(Common.PathConfigurationData, @"FoodToHitTargetCarbs.txt");
        internal string persistentHypoPrediction = Path.Combine(Common.PathConfigurationData, @"HypoPrediction.txt");
        internal string persistentWeighFood = Path.Combine(Common.PathConfigurationData, @"WeighFood.txt");

        internal string logBolusCalculationsFile = Path.Combine(Common.PathConfigurationData, @"LogOfBolusCalculations.tsv");

        internal abstract List<GlucoseRecord> GetFirstTwoGlucoseMeasurements();
        internal abstract List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialInstant, DateTime? FinalInstant);
        internal abstract void RestoreBolusCalculations(BL_BolusCalculation Bolus);
        internal abstract void RestoreFoodToHitTargetCarbs(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        internal abstract void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo);
        internal abstract void RestoreInsulinParameters(BL_BolusCalculation Bolus);
        internal abstract void RestoreWeighFood(GlucoMan.BusinessLayer.BL_WeighFood WeighFood);
        internal abstract void SaveBolusCalculations(BL_BolusCalculation Bolus);
        internal abstract void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        internal abstract void SaveGlucoseMeasurements(List<GlucoseRecord> List);
        internal abstract void SaveHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo);
        internal abstract void SaveInsulinParameters(BL_BolusCalculation Bolus);
        internal abstract void SaveLogOfBoluses(BL_BolusCalculation Bolus);
        internal abstract void SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement);
        internal abstract void SaveWeighFood(GlucoMan.BusinessLayer.BL_WeighFood WeighFood);
        internal abstract int FindNextIndex(List<GlucoseRecord> List);
        /// <summary>
        /// Saves the values of the parameters, passed as strings, in the fields whose name is passed
        /// Saving on a table with possibly a single row, whose name is managed by the implementation 
        /// Passing an optional key will save in the passed row. 
        /// If no key il passed the row with maximum key will be used 
        /// The method must take care of the types of the fields 
        /// </summary>
        /// <param name="Paramaters"></param>
        /// <param name="Key"></param>
        internal abstract int? SaveParameter(string FieldName, string FieldValue, int? Key = null); 
        internal abstract string RestoreParameter(string FieldName, int? Key = null); 
        internal abstract void PurgeDatabase(); 
    }
}
