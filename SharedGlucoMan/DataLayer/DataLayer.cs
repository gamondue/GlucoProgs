
using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public abstract partial class DataLayer
    {
        public string persistentBolusCalculation = Path.Combine(Common.PathConfigurationData, @"BolusCalculation.txt");
        public string persistentInsulinParameters = Path.Combine(Common.PathConfigurationData, @"InsulinParameters.txt");
        public string persistentGlucoseMeasurements = Path.Combine(Common.PathConfigurationData, @"GlucoseMeasurements.txt");
        public string persistentFoodToEatTarget = Path.Combine(Common.PathConfigurationData, @"FoodToHitTargetCarbs.txt");
        public string persistentHypoPrediction = Path.Combine(Common.PathConfigurationData, @"HypoPrediction.txt");
        public string persistentWeighFood = Path.Combine(Common.PathConfigurationData, @"WeighFood.txt");

        public string logBolusCalculationsFile = Path.Combine(Common.PathConfigurationData, @"LogOfBolusCalculations.tsv");

        #region General
        public abstract void PurgeDatabase();
        public abstract int GetNextPrimaryKey();
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
        public abstract long? SaveParameter(string FieldName, string FieldValue, int? Key = null);
        public abstract string RestoreParameter(string FieldName, int? Key = null);
        public abstract void RestoreInsulinCorrectionParameters(BL_BolusCalculation bL_BolusCalculation);
        public abstract void RestoreFoodToHitTargetCarbs(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        public abstract void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo);
        public abstract void SaveBolusParameters(BL_BolusCalculation Parameters);
        public abstract void RestoreBolusParameters(BL_BolusCalculation Parameters);
        public abstract void SaveInsulinCorrectionParameters(BL_BolusCalculation Parameters);
        #endregion

        #region Glucose 
        public abstract List<GlucoseRecord> GetLastTwoGlucoseMeasurements();
        public abstract List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialInstant, DateTime? FinalInstant);
        public abstract void RestoreWeighFood(BL_WeighFood WeighFood);
        public abstract void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        public abstract void SaveGlucoseMeasurements(List<GlucoseRecord> List);
        public abstract void SaveHypoPrediction(BL_HypoPrediction Hypo);
        public abstract void SaveLogOfBoluses(BL_BolusCalculation Bolus);
        public abstract long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement);
        public abstract void SaveWeighFood(BL_WeighFood WeighFood);
        #endregion

        #region Meals and Food in Meals
        public abstract void SaveMeals(List<Meal> List);
        public abstract long? SaveOneMeal(Meal Meal);
        public void ReadMeal(BL_MealAndFood bL_MealAndFood)
        {
            throw new NotImplementedException();
        }
        public abstract List<Meal> ReadMeals(DateTime? initialTime, DateTime? finalTime);
        public abstract void SaveFoodsInMeal(List<FoodInMeal> list);
        public abstract List<FoodInMeal> ReadFoodsInMeal(int? IdMeal);
        public abstract int? SaveOneFoodInMeal(FoodInMeal FoodToSave); 
        #endregion
    }
}
