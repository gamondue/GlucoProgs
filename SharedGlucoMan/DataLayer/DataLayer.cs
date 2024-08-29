using System;
using System.Collections.Generic;
using System.Data.Common;

namespace GlucoMan
{
    internal abstract class DataLayer
    {
        #region General
        internal abstract bool DeleteDatabase();
        internal abstract int GetNextPrimaryKey();
        #endregion
        #region Save and restore of program's parameters        
        /// <summary>
        /// Saves the values of the parameters, passed as strings, in the fields whose name is passed
        /// Saving on a table with possibly a single row, whose name is managed by the implementation 
        /// Passing an optional key will save in the passed row. 
        /// If no key il passed the row with maximum key will be used 
        /// The method must take care of the types of the fields 
        /// </summary>
        /// <param name="Paramaters"></param>
        /// <param name="Key"></param>
        internal abstract long? SaveParameter(string FieldName, string FieldValue, int? Key = null);
        internal abstract string RestoreParameter(string FieldName, int? Key = null);
        #endregion
        #region Glucose 
        internal abstract GlucoseRecord GetOneGlucoseRecord(int? idGlucoseRecord);
        internal abstract List<GlucoseRecord> GetLastTwoGlucoseMeasurements();
        internal abstract List<GlucoseRecord> GetGlucoseRecords(DateTime? InitialInstant, DateTime? FinalInstant);
        //internal abstract void SaveFoodToHitTarget(BL_FoodToHitTargetCarbs CalculationsOfChoMassToHitTarget);
        internal abstract void SaveGlucoseMeasurements(List<GlucoseRecord> List);
        internal abstract long? SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement);
        internal abstract void DeleteOneGlucoseMeasurement(GlucoseRecord gr);
        internal abstract int? SaveOneInjection(InsulinInjection Injection);
        internal abstract void DeleteOneInjection(InsulinInjection Injection);
        internal abstract List<InsulinInjection> GetInjections(DateTime InitialInstant,
            DateTime FinalInstant, 
            Common.TypeOfInsulinSpeed TypeOfInsulinSpeed = Common.TypeOfInsulinSpeed.NotSet, 
            Common.ZoneOfPosition Zone = Common.ZoneOfPosition.NotSet);
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
        internal abstract List<Food> SearchFoods(string Name, string Description);
        internal abstract int? SaveOneFood(Food currentFood);
        internal abstract void DeleteOneFood(Food food);
        internal abstract Food GetOneFood(int? IdFood);
        internal abstract List<Food> GetFoods();
        internal abstract InsulinInjection GetOneInjection(int? idInjection);
        #endregion
        #region Alarms
        internal abstract int? SaveOneAlarm(Alarm currentAlarm);
        internal abstract List<Alarm> ReadAllAlarms();
        #endregion
        #region Recipes and Ingredients
        internal abstract int? InsertOneRecipe(Recipe Recipe); 
        internal abstract List<Recipe> ReadSomeRecipes(string WhereClause);
        internal abstract Recipe GetOneRecipe(int? IdRecipe);
        internal abstract int? SaveOneRecipe(Recipe Recipe);
        internal abstract void UpdateOneRecipe(Recipe RecipeToSave);
        internal abstract void DeleteOneRecipe(Recipe Recipe);
        internal abstract List<Recipe> SearchRecipes(string Name, string Description);
        internal abstract List<Ingredient> ReadAllIngredientsOfARecipe(int? idRecipe);
        internal abstract int? SaveOneIngredient(Ingredient Ingredient);
        internal abstract int? InsertOneIngredient(Ingredient ingredient);
        internal abstract void UpdateOneIngredient(Ingredient ingredient);
        internal abstract Ingredient GetIngredientFromRow(DbDataReader Row);
        #endregion
        internal abstract void CreateNewDatabase(string pathAndFileDatabase);
    }
}
