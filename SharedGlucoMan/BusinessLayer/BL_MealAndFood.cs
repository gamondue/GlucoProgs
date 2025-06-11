using gamon;
using static GlucoMan.Common;

namespace GlucoMan.BusinessLayer
{
    public class BL_MealAndFood
    {
        DataLayer dl;
        public List<Meal> Meals { get; set; }
        public Meal Meal { get; set; }
        public List<FoodInMeal> FoodsInMeal { get; set; }
        public FoodInMeal FoodInMeal { get; set; }

        //public Food Food { get; set; }

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
            // if the current CurrentIngredient has not an IdMeal, we give it the Id of the current Meal 
            if (FoodToSave.IdMeal == null)
            {
                FoodToSave.IdMeal = Meal.IdMeal;
            }
            // so the new meal will be the one of the CurrentIngredient we are saving
            return dl.SaveOneFoodInMeal(FoodToSave);
        }
        internal void SaveAllFoodsInMeal()
        {
            if (FoodsInMeal != null)
                foreach (FoodInMeal food in FoodsInMeal)
                {
                    //// if it is necessary, the next method will create a new meal
                    //if (Food.IdMeal == null)
                    //{   // if the Food has not been saved jet, we add it to the list of Foods in this Meal
                    //    FoodsInMeal.Add(Food);
                    //}
                    // now we save, if the Food was new, when exiting the next function, it will have an IdFoodInMeal
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
            if (FoodInMeal.CarbohydratesPerUnit.Double != null && FoodInMeal.QuantityInUnits.Double != null)
            {
                FoodInMeal.CarbohydratesGrams.Double = FoodInMeal.CarbohydratesPerUnit.Double / 100 *
                    FoodInMeal.QuantityInUnits.Double;
            }
        }
        #endregion
        internal string[] GetAllAccuracies()
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
                    sumOfSquaredValuesByWeight += squaredValue * (double)f.CarbohydratesGrams.Double;
                }
                if (IsValueCorrect)
                {
                    // square of the weighted quadratic sum
                    WeightedQuadraticAverage = Math.Sqrt(sumOfSquaredValuesByWeight / sumOfWeights) * 100;
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
            DestinationFoodInMeal.CarbohydratesPerUnit.Double =
                SourceFood.CarbohydratesPercent.Double * SourceFood.Unit.GramsInOneUnit.Double;
            DestinationFoodInMeal.Name = SourceFood.Name;
            DestinationFoodInMeal.Description = SourceFood.Description;
            DestinationFoodInMeal.SugarPercent = SourceFood.SugarPercent;
            DestinationFoodInMeal.FibersPercent = SourceFood.FibersPercent;
        }
        internal void FromRecipeToFoodInMeal(Recipe sourceRecipe, FoodInMeal DestinationFoodInMeal)
        {
            //DestinationFoodInMeal.IdFood = SourceFood.IdFood;
            //DestinationFoodInMeal.CarbohydratesPerUnit.Double =
            //    sourceRecipe.CarbohydratesGrams.Double * sourceRecipe.Unit.GramsInOneUnit.Double;
            DestinationFoodInMeal.Name = sourceRecipe.Name;
            DestinationFoodInMeal.Description = sourceRecipe.Description;
            DestinationFoodInMeal.AccuracyOfChoEstimate = sourceRecipe.AccuracyOfChoEstimate;
            // !!!! maybe the following should be different !!!!
            DestinationFoodInMeal.CarbohydratesPerUnit = sourceRecipe.CarbohydratesPercent;
            //DestinationFoodInMeal.SugarPercent = SourceFood.SugarPercent;
            //DestinationFoodInMeal.FibersPercent = SourceFood.FibersPercent;
        }
        public void FromFoodInMealToFood(FoodInMeal SourceFoodInMeal, Food DestinationFood)
        {
            DestinationFood.IdFood = SourceFoodInMeal.IdFood;
            DestinationFood.Name = SourceFoodInMeal.Name;
            DestinationFood.Description = SourceFoodInMeal.Description;
            DestinationFood.CarbohydratesPercent = SourceFoodInMeal.CarbohydratesPerUnit;
            DestinationFood.SugarPercent = SourceFoodInMeal.SugarPercent;
            DestinationFood.FibersPercent = SourceFoodInMeal.FibersPercent;
        }
        public void FromIngredientToFood(Ingredient SourceIngredient, Food DestinationFood)
        {
            DestinationFood.IdFood = SourceIngredient.IdFood;
            DestinationFood.Name = SourceIngredient.Name;
            DestinationFood.Description = SourceIngredient.Description;
            DestinationFood.CarbohydratesPercent = SourceIngredient.CarbohydratesPercent;
            //DestinationFood.SugarPercent = SourceIngredient.SugarPercent;
            //DestinationFood.FibersPercent = SourceIngredient.FibersPercent;
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
            dl.SaveParameter("FoodInMeal_CarbohydratesPercent", FoodInMeal.CarbohydratesPerUnit.Text);
            dl.SaveParameter("FoodInMeal_Name", FoodInMeal.Name);
            dl.SaveParameter("FoodInMeal_AccuracyOfChoEstimate", FoodInMeal.AccuracyOfChoEstimate.Text);
        }
        public void RestoreFoodInMealParameters()
        {
            FoodInMeal.CarbohydratesGrams.Text = dl.RestoreParameter("FoodInMeal_ChoGrams");
            FoodInMeal.QuantityInUnits.Text = dl.RestoreParameter("FoodInMeal_QuantityGrams");
            FoodInMeal.CarbohydratesPerUnit.Text = dl.RestoreParameter("FoodInMeal_CarbohydratesPercent");
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
        internal void AddUnitToFood(Food Food, UnitOfFood Unit)
        {
            dl.AddUnitToFood(Food, Unit);
        }
        internal void RemoveUnitFromFoodsUnits(Food Food)
        {
            dl.RemoveUnitFromFoodsUnits(Food);
        }
        internal List<UnitOfFood> GetAllUnitsOfOneFood(Food Food)
        {
            return dl.GetAllUnitsOfOneFood(Food);
        }
    }
}
