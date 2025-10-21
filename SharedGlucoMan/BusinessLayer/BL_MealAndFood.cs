using gamon;
using System.Collections;
using static GlucoMan.Common;

namespace GlucoMan.BusinessLayer
{
    public class BL_MealAndFood
    {
        DataLayer dl;
        public Meal Meal { get; set; }
        public List<Meal> Meals { get; set; }
        public FoodInMeal FoodInMeal { get; set; }
        public List<FoodInMeal> FoodsInMeal { get; set; }

        public BL_MealAndFood()
        {
            Meals = new List<Meal>();
            Meal = new Meal();
            FoodsInMeal = new List<FoodInMeal>();
            FoodInMeal = new FoodInMeal();

            dl = Common.Database;
        }
        #region Meals
        public Meal GetOneMeal(int? IdMeal)
        {
            Meal = dl.GetOneMeal(IdMeal);
            return Meal;
        }
        public List<Meal> GetMeals(DateTime? InitialTime, DateTime? FinalTime)
        {
            Meals = dl.GetMeals(InitialTime, FinalTime);
            return Meals;
        }
        public int? SaveOneMeal(Meal Meal, bool SaveWithNowAsTime)
        {
            if (SaveWithNowAsTime)
            {
                DateTime now = DateTime.Now;
                Meal.TimeBegin.DateTime = now;
                Meal.TimeEnd.DateTime = now;
            }
            return dl.SaveOneMeal(Meal);
        }
        internal void DeleteOneMeal(Meal Meal)
        {
            dl.DeleteOneMeal(Meal);
        }
        internal string[] GetAllTypesOfMeal()
        {
            return Enum.GetNames(typeof(Common.TypeOfMeal));
        }
        #endregion
        #region FoodsInMeals
        public List<FoodInMeal> GetFoodsInMeal(int? IdMeal)
        {
            FoodsInMeal = dl.GetFoodsInMeal(IdMeal);
            return FoodsInMeal;
        }
        public bool SaveFoodsInMeal(List<FoodInMeal> List)
        {
            return dl.SaveFoodsInMeal(List);
        }
        public int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            // if we don't have a meal, then we make one 
            if (Meal == null)
            {
                Meal = new Meal();
            }
            // if the meal hasn't been saved yet, we do save and obtain the key of the meal 
            if (Meal.IdMeal == null)
            {
                // if the meal has not a begin date, we save with now
                if (Meal.TimeBegin == null || Meal.TimeBegin.DateTime == General.DateNull)
                {
                    // if a Meal is created here, it must have Now as Time
                    // (true as the second parameter) 
                    Meal.IdMeal = SaveOneMeal(Meal, true);
                }
                else
                {   // if it has already a date, we save with that 
                    Meal.IdMeal = SaveOneMeal(Meal, false);
                }
            }
            // if the current Ingredient has not an IdMeal, we give it the Id of the current Meal 
            if (FoodToSave.IdMeal == null)
            {
                FoodToSave.IdMeal = Meal.IdMeal;
            }
            // so the new meal will be the one of the Ingredient we are saving
            return dl.SaveOneFoodInMeal(FoodToSave);
        }
        internal void SaveAllFoodsInMeal()
        {
            if (FoodsInMeal != null)
                foreach (FoodInMeal food in FoodsInMeal)
                {
                    dl.SaveOneFoodInMeal(food);
                }
        }
        internal void DeleteOneFoodInMeal(FoodInMeal Food)
        {
            dl.DeleteOneFoodInMeal(Food);
            RecalcAll();
        }
        #endregion
        #region Foods
        internal int? SaveOneFood(Food Food)
        {
            return dl.SaveOneFood(Food);
        }
        internal void DeleteOneFood(Food food)
        {
            // !!!! verify if  the row to delete has been used somewhere else in the database
            // !!!! if it is the case, don't delete and give notice to the caller 
            dl.DeleteOneFood(food);
        }
        internal Food GetOneFood(int? idFood)
        {
            return dl.GetOneFood(idFood);
        }
        internal List<Food> SearchFoods(string Name, string Description, int MinNoOfCharacters)
        {
            if ((Name != null && Name != "" && Name.Length >= MinNoOfCharacters) ||
                (Description != null && Description != "" && Description.Length >= MinNoOfCharacters))
            {
                // trim the strings from blanks coming from cut and paste 
                if (Name != null)
                    Name = Name.Trim();
                if (Description != null)
                    Description = Description.Trim();
                return dl.SearchFoods(Name, Description);
            }
            // just if both are null: find all the list of foods 
            else if (Name == "" && Description == "")
                return dl.SearchFoods(Name, Description);
            else
                return null;
        }
        internal List<Food> GetFoods()
        {
            return dl.GetFoods();
        }
        public void CalculateChoOfFoodGrams()
        {
            if (FoodInMeal.CarbohydratesPercent.Double != null && FoodInMeal.QuantityInUnits.Double != null)
            {
                if (FoodInMeal.GramsInOneUnit.Double == null || FoodInMeal.GramsInOneUnit.Double == 0)
                    FoodInMeal.GramsInOneUnit.Double = 1; 
                FoodInMeal.CarbohydratesGrams.Double = FoodInMeal.CarbohydratesPercent.Double / 100 *
                    FoodInMeal.QuantityInUnits.Double * FoodInMeal.GramsInOneUnit.Double;
            }
        }
        #endregion
        internal string[] GetAllQualitativeAccuracies()
        {
            return Enum.GetNames(typeof(Common.QualitativeAccuracy));
        }
        internal double? RecalcTotalCho()
        {
            double? total = 0;
            if (FoodsInMeal != null)
            {
                foreach (FoodInMeal f in FoodsInMeal)
                {
                    if (f.CarbohydratesGrams.Double != null)
                        total += f.CarbohydratesGrams.Double;
                }
                Meal.CarbohydratesGrams.Double = total;
            }
            return total;
        }
        /// <summary>
        /// Calculate accuracy from single Food accuracies
        /// </summary>
        /// <returns>Weighted quadratic average. Also modifies Meal. </returns>
        internal double? RecalcTotalAccuracy()
        {
            // calculate weighted quadratic average accuracy "components"
            // the weights are the values of CHO of the various foods eaten in this meal 
            double sumOfWeights = 0;
            double sumOfSquaredValuesByWeight = 0;
            double? WeightedQuadraticAverage = 0;
            bool IsValueCorrect = true;
            if (FoodsInMeal != null && FoodsInMeal.Count > 0)
            {
                foreach (FoodInMeal f in FoodsInMeal)
                {
                    // if we don't have CHO we can't calculate the propagation of uncertainty, 
                    // because CHO is the weight of the component
                    if (f == null || f.CarbohydratesGrams.Double == null)
                    {
                        IsValueCorrect = false;
                        break;
                    }
                    // if we don't have the accuracy of a component, we can't calculate the propagation of uncertainty, 
                    if (f.AccuracyOfChoEstimate.Double == null || f.AccuracyOfChoEstimate == null)
                    {
                        IsValueCorrect = false;
                        break;
                    }
                    sumOfWeights += (double)f.CarbohydratesGrams.Double; // fixed 2024-12-14 from sum of squared weights to sum of weights
                    double squaredValue = (double)f.AccuracyOfChoEstimate.Double * (double)f.AccuracyOfChoEstimate.Double;
                    // CarbohydratesGrams is the weight, that doesn't get squared
                    sumOfSquaredValuesByWeight += squaredValue * (double)f.CarbohydratesGrams.Double;
                }
                if (IsValueCorrect)
                {
                    // square of the weighted quadratic sum
                    WeightedQuadraticAverage = Math.Sqrt(sumOfSquaredValuesByWeight / sumOfWeights);
                    Meal.AccuracyOfChoEstimate.Double = WeightedQuadraticAverage;
                }
                // if the value in not correct, Meal.AccuracyOfChoEstimate remains unchanged
            }
            if (IsValueCorrect)
                return WeightedQuadraticAverage;
            else
            {
                Meal.AccuracyOfChoEstimate.Double = (int)QualitativeAccuracy.NotSet;
                return null;
            }
        }
        internal void FromFoodToFoodInMeal(Food SourceFood, FoodInMeal DestinationFoodInMeal)
        {
            DestinationFoodInMeal.IdFood = SourceFood.IdFood;
            DestinationFoodInMeal.CarbohydratesPercent.Double = SourceFood.CarbohydratesPercent.Double;
            DestinationFoodInMeal.Name = SourceFood.Name;
            DestinationFoodInMeal.UnitSymbol = SourceFood.UnitSymbol;
            DestinationFoodInMeal.GramsInOneUnit.Double = SourceFood.GramsInOneUnit.Double;
            DestinationFoodInMeal.Description = SourceFood.Description;
            DestinationFoodInMeal.SugarPercent = SourceFood.SugarPercent;
            DestinationFoodInMeal.FibersPercent = SourceFood.FibersPercent;
        }
        internal void FromRecipeToFoodInMeal(Recipe sourceRecipe, FoodInMeal DestinationFoodInMeal)
        {
            //DestinationFoodInMeal.IdFood = SourceFood.IdFood;
            //DestinationFoodInMeal.CarbohydratesPercent.Double =
            //    sourceRecipe.CarbohydratesGrams.Double * sourceRecipe.UnitSymbol.GramsInOneUnit.Double;
            DestinationFoodInMeal.Name = sourceRecipe.Name;
            DestinationFoodInMeal.Description = sourceRecipe.Description;
            DestinationFoodInMeal.AccuracyOfChoEstimate = sourceRecipe.AccuracyOfChoEstimate;
            DestinationFoodInMeal.CarbohydratesPercent = sourceRecipe.CarbohydratesPercent;
            //DestinationFoodInMeal.SugarPercent = SourceFood.SugarPercent;
            //DestinationFoodInMeal.FibersPercent = SourceFood.FibersPercent;
        }
        public void FromFoodInMealToFood(FoodInMeal SourceFoodInMeal, Food DestinationFood)
        {
            DestinationFood.IdFood = SourceFoodInMeal.IdFood;
            DestinationFood.Name = SourceFoodInMeal.Name;
            DestinationFood.Description = SourceFoodInMeal.Description;
            DestinationFood.CarbohydratesPercent = SourceFoodInMeal.CarbohydratesPercent;
            DestinationFood.GramsInOneUnit.Double = SourceFoodInMeal.GramsInOneUnit.Double;
            //DestinationFood.UnitSymbol = dl.GetUnitsOfOneFood( UnitSymbol(SourceFoodInMeal.UnitSymbol, SourceFoodInMeal.GramsInOneUnit.Double);
            DestinationFood.SugarPercent = SourceFoodInMeal.SugarPercent;
            DestinationFood.FibersPercent = SourceFoodInMeal.FibersPercent;
        }
        public void FromIngredientToFood(Ingredient SourceIngredient, Food DestinationFood)
        {
            if (SourceIngredient != null)
            { 
                DestinationFood.IdFood = SourceIngredient.IdFood;
                DestinationFood.Name = SourceIngredient.Name;
                DestinationFood.Description = SourceIngredient.Description;
                DestinationFood.CarbohydratesPercent = SourceIngredient.CarbohydratesPercent;
                //DestinationFood.SugarPercent = SourceIngredient.SugarPercent;
                //DestinationFood.FibersPercent = SourceIngredient.FibersPercent;
            }
        }
        internal TypeOfMeal SetTypeOfMealBasedOnTimeNow()
        {
            TypeOfMeal type = TypeOfMeal.NotSet;
            DateTime now = DateTime.Now;
            if (now.Hour > 6 && now.Hour < 9)
                Meal.IdTypeOfMeal = TypeOfMeal.Breakfast;
            else if (now.Hour > 12 && now.Hour < 14)
                Meal.IdTypeOfMeal = TypeOfMeal.Lunch;
            else if (now.Hour > 19 && now.Hour < 21)
                Meal.IdTypeOfMeal = TypeOfMeal.Dinner;
            else
                Meal.IdTypeOfMeal = TypeOfMeal.Snack;
            return Meal.IdTypeOfMeal;
        }
        internal void RecalcAll()
        {
            RecalcTotalCho();
            RecalcTotalAccuracy();
            SaveMealParameters();
        }
        internal void NewDefaults()
        {
            Meal = new Meal();
            DateTime now = DateTime.Now;
            Meal.TimeBegin.DateTime = now;
            Meal.TimeEnd.DateTime = now;
            Meal.AccuracyOfChoEstimate.Double = 0;
            Meal.IdTypeOfMeal = SetTypeOfMealBasedOnTimeNow();
        }
        public void SaveFoodInMealParameters()
        {
            dl.SaveParameter("FoodInMeal_ChoGrams", FoodInMeal.CarbohydratesGrams.Text);
            dl.SaveParameter("FoodInMeal_QuantityGrams", FoodInMeal.QuantityInUnits.Text);
            dl.SaveParameter("FoodInMeal_CarbohydratesPercent", FoodInMeal.CarbohydratesPercent.Text);
            dl.SaveParameter("FoodInMeal_Name", FoodInMeal.Name);
            dl.SaveParameter("FoodInMeal_AccuracyOfChoEstimate", FoodInMeal.AccuracyOfChoEstimate.Text);
        }
        public void RestoreFoodInMealParameters()
        {
            FoodInMeal.CarbohydratesGrams.Text = dl.RestoreParameter("FoodInMeal_ChoGrams");
            FoodInMeal.QuantityInUnits.Text = dl.RestoreParameter("FoodInMeal_QuantityGrams");
            FoodInMeal.CarbohydratesPercent.Text = dl.RestoreParameter("FoodInMeal_CarbohydratesPercent");
            FoodInMeal.Name = dl.RestoreParameter("FoodInMeal_Name");
            FoodInMeal.AccuracyOfChoEstimate.Text = dl.RestoreParameter("FoodInMeal_AccuracyOfChoEstimate");
        }
        public void SaveMealParameters()
        {
            dl.SaveParameter("Meal_ChoGrams", Meal.CarbohydratesGrams.Text);
        }
        public void RestoreMealParameters()
        {
            Meal.CarbohydratesGrams.Text = dl.RestoreParameter("Meal_ChoGrams");
        }
        internal int? AddUnit(UnitOfFood Unit)
        {
            //// the Name and the Description of a new Unit are set equal to the Symbol
            //// (currenlt)
            //Unit.Description = Unit.Symbol;
            //Unit.Description = Unit.Symbol;
            return dl.AddUnit(Unit);
        }
        internal int? AddManufacturerToFood(Manufacturer m, Food currentFood)
        {
            // !!!!!!!!!!!!!! fix !!!!!!!!
            if (dl.CheckIfManufacturerExists(m))
                return dl.UpdateManufacturer(m);
            else
                return dl.AddManufacturer(m);

            // this instruction must be in the business layer when you save the food
            // return dl.AddManufacturerToFood(m, currentFood);
        }
        internal int? AddCategoryToFood(CategoryOfFood c, Food currentFood)
        {
            return dl.AddCategoryToFood(c, currentFood);
        }
        internal int? AddManufacturerToCurrentFood(Manufacturer manufacturer, Food food)
        {
            return dl.AddManufacturer(manufacturer, food);
        }
        internal int? AddManufacturer(Manufacturer manufacturer)
        {
            return dl.AddManufacturer(manufacturer, null);
        }
        internal int? AddCategoryToCurrentFood(CategoryOfFood category, Food food)
        {
            return dl.AddCategoryOfFood(category, food);
        }
        internal int? AddCategoryOfFood(CategoryOfFood CategoryOfFood)
        {
            int? idCategory = null;
            if (!dl.CheckIfCategoryExists(CategoryOfFood))
                idCategory = dl.AddCategoryOfFood(CategoryOfFood);
            else
                idCategory = dl.UpdateCategoryOfFood(CategoryOfFood);
            return idCategory;
        }
        internal void RemoveUnitFromFood(UnitOfFood unit, Food Food)
        {
            dl.RemoveUnitFromFood(unit, Food);
        }
        internal void RemoveManufacturerFromFood(Manufacturer manufacturer, Food currentFood)
        {
            dl.RemoveManufacturerFromFood(currentFood);
        }
        internal void RemoveCategoryFromFood(CategoryOfFood category, Food currentFood)
        {
            dl.RemoveCategoryFromFood(currentFood);
        }
        internal List<UnitOfFood> GetAllUnitsOfOneFood(Food Food)
        {
            return dl.GetAllUnitsOfOneFood(Food);
        }
        internal void RemoveUnitFromFoodsUnits(Food currentFood)
        {
            dl.RemoveUnitFromFoodsUnits(currentFood);
        }
        internal List<Manufacturer> GetAllManufacturersOfOneFood(Food food)
        {
            return dl.GetAllManufacturersOfOneFood(food);
        }
        internal List<CategoryOfFood> GetAllCategoriesOfOneFood(Food food)
        {
            return dl.GetAllCategoriesOfOneFood(food);
        }
        internal static TypeOfMeal SelectTypeOfMealBasedOnTimeNow()
        {
            TypeOfMeal type;
            int hour = DateTime.Now.Hour;
            if (hour > breakfastStartHour && hour < breakfastEndHour)
                type = TypeOfMeal.Breakfast;
            else if (hour > lunchStartHour && hour < lunchEndHour)
                type = TypeOfMeal.Lunch;
            else if (hour > dinnerStartHour && hour < dinnerEndHour)
                type = TypeOfMeal.Dinner;
            else
                type = TypeOfMeal.Snack;
            return type;
        }
        internal bool CheckIfUnitSymbolExists(UnitOfFood unit, int? idFood)
        {
            return dl.CheckIfUnitSymbolExists(unit, idFood);
        }
        internal void RemoveCategoryFromFood(Food currentFood)
        {
            dl.RemoveCategoryFromFood(currentFood);
        }
        internal void UpdateDataAfterChoGramsChange(string choGrams)
        {
            FoodInMeal.CarbohydratesGrams.Text = choGrams;
            FoodInMeal.QuantityInUnits.Double = 0;
            FoodInMeal.CarbohydratesPercent.Double = 0;
            RecalcAll();
        }
        internal void UpdateDataAfterQuantityChange(string CarbohydratesPercent, 
            string FoodQuantity)
        {
            FoodInMeal.CarbohydratesPercent.Text = CarbohydratesPercent;
            FoodInMeal.QuantityInUnits.Text = FoodQuantity;
            RecalcAll();
            //CalculateChoOfFoodGrams();
        }
        internal void UpdateOldFoodInMealInList()
        {
            // if the current FoodInMeal has not a name, it doesn't deserve to be updated, hence saved
            if (FoodInMeal.Name == null || FoodInMeal.Name == "")
                return;
            dl.SaveOneFoodInMeal(FoodInMeal);
            return;
        }
    }
}
