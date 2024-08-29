using gamon;
using System;
using System.Collections.Generic;
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
        public void SaveFoodsInMeal(List<FoodInMeal> List)
        {
            dl.SaveFoodsInMeal(List);
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
                    // (true as the second paramater) 
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
                    //// if it is necessary, the next method will create a new meal
                    //if (food.IdMeal == null)
                    //{   // if the food has not been saved jet, we add it to the list of Foods in this Meal
                    //    FoodsInMeal.Add(food);
                    //}
                    // now we save, if the food was new, when exiting the next function, it will have an IdFoodInMeal
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
        internal int? SaveOneFood(Food food)
        {
            return dl.SaveOneFood(food);
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
        public void CalculateChoOfFoodGrams(FoodInMeal ThisFood)
        {
            if (ThisFood.CarbohydratesPercent.Double != null && ThisFood.QuantityGrams.Double != null)
            {
                ThisFood.ChoGrams.Double = ThisFood.CarbohydratesPercent.Double / 100 *
                    ThisFood.QuantityGrams.Double;
            }
            //RecalcAll(); 
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
                    if (f.ChoGrams.Double != null)
                        total += f.ChoGrams.Double;
                }
                Meal.Carbohydrates.Double = total;
            }
            return total;
        }
        /// <summary>
        /// Calculate accuracy from single food accuracies
        /// </summary>
        /// <returns>Weighted quadratic average. Also modifies Meal. </returns>
        internal double? RecalcTotalAccuracy()
        {
            // calculate weighted quadratic sum of the quadratic weighted CHOs ("components" of accuracy estimation)
            // the weights are the values of CHO of the various foods eaten in this meal 
            double sumOfSquaredWeights = 0;
            double sumOfSquaredWeightedValues = 0;
            double? WeightedQuadraticAverage = 0;
            bool IsValueCorrect = true;
            if (FoodsInMeal != null && FoodsInMeal.Count > 0)
            {
                foreach (FoodInMeal f in FoodsInMeal)
                {
                    // if we don't have CHO we can't calculate the propagation of uncertainty, 
                    // because CHO is the weight of the component
                    if (f == null || f.ChoGrams.Double == null)
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
                    sumOfSquaredWeights += (double)f.ChoGrams.Double * (double)f.ChoGrams.Double;
                    double WeightedUncertaintyComponent = (double)f.AccuracyOfChoEstimate.Double / 100 * (double)f.ChoGrams.Double;
                    sumOfSquaredWeightedValues += WeightedUncertaintyComponent * WeightedUncertaintyComponent;
                }
                if (IsValueCorrect)
                {
                    // square of the weighted quadratic sum
                    WeightedQuadraticAverage = Math.Sqrt(sumOfSquaredWeightedValues / sumOfSquaredWeights) * 100;
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
            DestinationFoodInMeal.CarbohydratesPercent = SourceFood.Carbohydrates;
            DestinationFoodInMeal.Name = SourceFood.Name;
            DestinationFoodInMeal.Description = SourceFood.Description;
            DestinationFoodInMeal.SugarPercent = SourceFood.Sugar;
            DestinationFoodInMeal.FibersPercent = SourceFood.Fibers;
        }
        public void FromFoodInMealToFood(FoodInMeal SourceFoodInMeal, Food DestinationFood)
        {
            DestinationFood.IdFood = SourceFoodInMeal.IdFood;
            DestinationFood.Name = SourceFoodInMeal.Name;
            DestinationFood.Description = SourceFoodInMeal.Description;
            DestinationFood.Carbohydrates = SourceFoodInMeal.CarbohydratesPercent;
            DestinationFood.Sugar = SourceFoodInMeal.SugarPercent;
            DestinationFood.Fibers = SourceFoodInMeal.FibersPercent;
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
            dl.SaveParameter("FoodInMeal_ChoGrams", FoodInMeal.ChoGrams.Text);
            dl.SaveParameter("FoodInMeal_QuantityGrams", FoodInMeal.QuantityGrams.Text);
            dl.SaveParameter("FoodInMeal_CarbohydratesPercent", FoodInMeal.CarbohydratesPercent.Text);
            dl.SaveParameter("FoodInMeal_Name", FoodInMeal.Name);
            dl.SaveParameter("FoodInMeal_AccuracyOfChoEstimate", FoodInMeal.AccuracyOfChoEstimate.Text);
        }
        public void RestoreFoodInMealParameters()
        {
            FoodInMeal.ChoGrams.Text = dl.RestoreParameter("FoodInMeal_ChoGrams");
            FoodInMeal.QuantityGrams.Text = dl.RestoreParameter("FoodInMeal_QuantityGrams");
            FoodInMeal.CarbohydratesPercent.Text = dl.RestoreParameter("FoodInMeal_CarbohydratesPercent");
            FoodInMeal.Name = dl.RestoreParameter("FoodInMeal_Name");
            FoodInMeal.AccuracyOfChoEstimate.Text = dl.RestoreParameter("FoodInMeal_AccuracyOfChoEstimate");
        }
        public void SaveMealParameters()
        {
            dl.SaveParameter("Meal_ChoGrams", Meal.Carbohydrates.Text);
        }
        public void RestoreMealParameters()
        {
            Meal.Carbohydrates.Text = dl.RestoreParameter("Meal_ChoGrams");
        }
    }
}
