using gamon;
using GlucoMan.BusinessObjects;
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
        internal abstract int? SaveOneInjection(Injection Injection);
        internal abstract void DeleteOneInjection(Injection Injection);
        internal abstract List<Injection> GetInjections(DateTime InitialInstant,
            DateTime FinalInstant,
            Common.TypeOfInsulinAction TypeOfInsulinAction = Common.TypeOfInsulinAction.NotSet,
            Common.ZoneOfPosition Zone = Common.ZoneOfPosition.NotSet,
            bool getFront = false, bool getBack = false, bool getHands = false, bool getSensors = false);
        // get all the injections of type Short, Rapid or Intermediate between InitialInstant and FinalInstant
        internal abstract List<Injection> GetQuickInjections(DateTime InitialInstant,
            DateTime FinalInstant);
        #endregion
        #region Meals and Food in Meals
        internal abstract Meal GetOneMeal(int? IdMeal);
        internal abstract List<Meal> GetMeals(DateTime? initialTime, DateTime? finalTime);
        internal abstract void SaveMeals(List<Meal> List);
        internal abstract int? SaveOneMeal(Meal Meal);
        internal abstract void DeleteOneMeal(Meal meal);
        internal abstract bool SaveFoodsInMeal(List<FoodInMeal> list);
        internal abstract List<FoodInMeal> GetFoodsInMeal(int? IdMeal);
        internal abstract int? SaveOneFoodInMeal(FoodInMeal FoodToSave);
        internal abstract void DeleteOneFoodInMeal(FoodInMeal Food);
        internal abstract List<Food> SearchFoods(string Name, string Description);
        internal abstract int? SaveOneFood(Food currentFood);
        internal abstract void DeleteOneFood(Food food);
        internal abstract Food GetOneFood(int? IdFood);
        internal abstract List<Food> GetFoods();
        internal abstract int? AddUnit(UnitOfFood unit);
        internal abstract int? AddManufacturer(Manufacturer manufacturer, Food food);
        internal abstract int? AddCategoryOfFood(CategoryOfFood category, Food food);
        internal abstract List<UnitOfFood> GetAllUnitsOfOneFood(Food Food);
        internal abstract List<Manufacturer> GetAllManufacturersOfOneFood(Food food);
        internal abstract List<CategoryOfFood> GetAllCategoriesOfOneFood(Food food);
        internal abstract bool CheckIfUnitSymbolExists(UnitOfFood unit, int? idFood);
        internal abstract void RemoveCategoryFromFood(Food currentFood);
        internal abstract void RemoveUnitFromFood(UnitOfFood unit, Food food);
        internal abstract void RemoveManufacturerFromFood(Food Food);

        internal abstract Injection GetOneInjection(int? idInjection);
        internal abstract InsulinDrug? GetOneInsulinDrug(int? idInsulinDrug);
        internal abstract int? SaveInsulinDrug(InsulinDrug InsulinDrug);
        #endregion
        #region Alarms
        internal abstract int? SaveOneAlarm(Alarm currentAlarm);
        internal abstract List<Alarm> GetAllAlarms(DateTime? from = null, DateTime? to = null, bool all = false, bool expired = false, bool active = true);
        internal abstract void DeleteOneAlarm(Alarm alarm);
        #endregion
        #region Recipes and Ingredients
        internal abstract int? InsertOneRecipe(Recipe Recipe);
        internal abstract List<Recipe> GetSomeRecipes(string WhereClause);
        internal abstract Recipe GetOneRecipe(int? IdRecipe);
        internal abstract int? SaveOneRecipe(Recipe Recipe);
        internal abstract void UpdateOneRecipe(Recipe RecipeToSave);
        internal abstract void DeleteOneRecipe(Recipe Recipe);
        internal abstract List<Recipe> SearchRecipes(string Name, string Description);
        internal abstract List<Ingredient> GetAllIngredientsInARecipe(int? idRecipe);
        internal abstract int? SaveOneIngredient(Ingredient Ingredient);
        internal abstract void SaveListOfIngredients(List<Ingredient> ingredients);
        internal abstract int? InsertOneIngredient(Ingredient ingredient);
        internal abstract void UpdateOneIngredient(Ingredient ingredient);
        internal abstract Ingredient GetIngredientFromRow(DbDataReader Row);
        internal abstract void DeleteOneIngredient(Ingredient ingredient);
        #endregion
        internal abstract void CreateNewDatabase(string pathAndFileDatabase);
        internal abstract void SaveOneReferenceCoordinate(PositionOfInjection position);
        internal abstract void DeleteAllReferenceCoordinates(Common.ZoneOfPosition zone);
        internal abstract List<PositionOfInjection> GetReferencePositions(Common.ZoneOfPosition Zone);
        internal abstract int? SaveAllParameters(Parameters parameters,
            bool saveInANewRowWithTimestamp);
        internal abstract void UpdateAllParameters(Parameters parameters);
        internal abstract void InsertAllParameters(Parameters parameters);
        internal abstract Parameters GetParameters();
        internal abstract List<InsulinDrug>? GetAllInsulinDrugs(Common.TypeOfInsulinAction shortActing);
        internal abstract int? AddManufacturerToFood(Manufacturer m, Food currentFood);
        internal abstract int? AddCategoryToFood(CategoryOfFood c, Food currentFood);
        internal abstract void RemoveUnitFromFoodsUnits(Food currentFood);
        internal abstract bool CheckIfManufacturerExists(Manufacturer m);
        internal abstract int? UpdateManufacturer(Manufacturer m);
        internal abstract int? AddManufacturer(Manufacturer m);
        internal abstract bool CheckIfCategoryExists(CategoryOfFood categoryOfFood);
        internal abstract int? AddCategoryOfFood(CategoryOfFood categoryOfFood);
        internal abstract int? UpdateCategoryOfFood(CategoryOfFood categoryOfFood);
    }
}
