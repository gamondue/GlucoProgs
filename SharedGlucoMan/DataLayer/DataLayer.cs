
using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
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

        #region General
        internal abstract void PurgeDatabase();
        internal abstract int GetNextPrimaryKey();
        internal abstract void DeleteOneGlucoseMeasurement(GlucoseRecord gr);

        #endregion
        /// <summary>
        /// Saves the values of the parameters, passed as strings, in the fields whose name is passed
        /// Saving on a table with possibly a single row, whose name is managed by the implementation 
        /// Passing an optional key will save in the passed row. 
        /// If no key il passed the row with maximum key will be used 
        /// The method must take care of the types of the fields 
        /// </summary>
        /// <param name="Paramaters"></param>
        /// <param name="Key"></param>
        #region Save and restore of program's parameters
        internal abstract long? SaveParameter(string FieldName, string FieldValue, int? Key = null);
        internal abstract string RestoreParameter(string FieldName, int? Key = null);
        internal abstract void RestoreInsulinCorrectionParameters(BL_BolusCalculation bL_BolusCalculation);
        internal abstract void RestoreFoodToHitTargetCarbs(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        internal abstract void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo);
        internal abstract void SaveBolusParameters(BL_BolusCalculation Parameters);
        internal abstract void RestoreBolusParameters(BL_BolusCalculation Parameters);
        internal abstract void SaveInsulinCorrectionParameters(BL_BolusCalculation Parameters);
        internal abstract void SaveRatioChoInsulinParameters(BL_BolusCalculation Parameters);
        internal abstract void RestoreRatioChoInsulinParameters(BL_BolusCalculation Parameters);
        #endregion

        #region Glucose 
        internal abstract List<GlucoseRecord> GetLastTwoGlucoseMeasurements(); 
        internal abstract List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialInstant, DateTime? FinalInstant);
        internal abstract void RestoreWeighFood(BL_WeighFood WeighFood);
        internal abstract void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        internal abstract void SaveGlucoseMeasurements(List<GlucoseRecord> List);
        internal abstract void SaveHypoPrediction(BL_HypoPrediction Hypo);
        internal abstract void SaveLogOfBoluses(BL_BolusCalculation Bolus);
        internal abstract long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement);
        internal abstract void SaveWeighFood(BL_WeighFood WeighFood);
        #endregion

        #region Meals and Food in Meals
        internal abstract Meal ReadOneMeal(long? IdMeal);
        internal abstract List<Meal> ReadMeals(DateTime? initialTime, DateTime? finalTime);
        internal abstract void SaveMeals(List<Meal> List);
        internal abstract int? SaveOneMeal(Meal Meal);
        internal abstract void DeleteOneMeal(Meal meal);
        internal abstract void SaveFoodsInMeal(List<FoodInMeal> list);
        internal abstract List<FoodInMeal> ReadFoodsInMeal(int? IdMeal);
        internal abstract int? SaveOneFoodInMeal(FoodInMeal FoodToSave);
        internal abstract void DeleteOneFoodInMeal(FoodInMeal Food);
        internal abstract List<Food> SearchFood(Food Food);
        #endregion
    }
}
