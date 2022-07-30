
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
        #region General
        internal abstract void DeleteDatabase();
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
        #endregion

        #region Glucose 
        internal abstract List<GlucoseRecord> GetLastTwoGlucoseMeasurements(); 
        internal abstract List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialInstant, DateTime? FinalInstant);
        //internal abstract void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        internal abstract void SaveGlucoseMeasurements(List<GlucoseRecord> List);      
        internal abstract long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement);
        #endregion

        #region Meals and Food in Meals
        internal abstract Meal GetOneMeal(int? IdMeal);
        internal abstract List<Meal> GetMeals(DateTime? initialTime, DateTime? finalTime);
        internal abstract void SaveMeals(List<Meal> List);
        internal abstract int? SaveOneMeal(Meal Meal);
        internal abstract void DeleteOneMeal(Meal meal);
        internal abstract void SaveFoodsInMeal(List<FoodInMeal> list);
        internal abstract List<FoodInMeal> GetFoodsInMeal(int? IdMeal);
        internal abstract int? SaveOneFoodInMeal(FoodInMeal FoodToSave);
        internal abstract void DeleteOneFoodInMeal(FoodInMeal Food);
        internal abstract List<Food> SearchFood(string Name, string Description);
        internal abstract int? SaveOneFood(Food currentFood);
        internal abstract void DeleteOneFood(Food food);
        internal abstract Food GetOneFood(int? IdFood);
        internal abstract List<Food> GetFoods();
        #endregion

        #region Alarms
        internal abstract int? SaveOneAlarm(Alarm currentAlarm);
        internal abstract List<Alarm> ReadAllAlarms();
        #endregion
    }
}
