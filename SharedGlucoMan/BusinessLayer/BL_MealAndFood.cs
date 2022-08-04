using static GlucoMan.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GlucoMan.BusinessLayer
{
    public class BL_MealAndFood
    {
        //DataLayer dl = Common.Database;
        List<Meal> currentMeals;
        public List<Meal> Meals { get => currentMeals; set => currentMeals = value; }
        
        List<FoodInMeal> currentFoodsInMeal;
        FoodInMeal currentFoodInMeal;
        public Meal Meal { get; set ; }
        public List<FoodInMeal> FoodsInMeal { get => currentFoodsInMeal; set => currentFoodsInMeal = value; }
        public BL_MealAndFood()
        {
            currentMeals = new List<Meal>();
            Meal = new Meal();
            currentFoodsInMeal = new List<FoodInMeal>();
            currentFoodInMeal = new FoodInMeal();
        }
        #region Meals
        public Meal GetOneMeal(int? IdMeal)
        {
            Meal = Common.Database.GetOneMeal(IdMeal);
            return Meal;
        }
        public List<Meal> GetMeals(DateTime? InitialTime, DateTime? FinalTime)
        {
            currentMeals = Common.Database.GetMeals(InitialTime, FinalTime);
            return currentMeals;
        }
        public int? SaveOneMeal(Meal Meal)
        {
            return Common.Database.SaveOneMeal(Meal);
        }
        internal void DeleteOneMeal(Meal Meal)
        {
            Common.Database.DeleteOneMeal(Meal);
        }
        internal string[] GetAllTypesOfMeal()
        {
            return Enum.GetNames(typeof(Common.TypeOfMeal));
        }
        #endregion
        #region FoodsInMeals
        public FoodInMeal FoodInMeal { get => currentFoodInMeal; set => currentFoodInMeal = value; }
        public List<FoodInMeal> GetFoodsInMeal(int? IdMeal)
        {
            currentFoodsInMeal = Common.Database.GetFoodsInMeal(IdMeal);
            return currentFoodsInMeal;
        }
        public void SaveFoodsInMeal(List<FoodInMeal> List)
        {
            Common.Database.SaveFoodsInMeal(List);
        }
        public int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            // if we don't have a meal, the we make one 
            if (Meal == null)
            {
                Meal = new Meal();
            }
            // if the meal has not a code, we save it to have one 
            if (Meal.IdMeal == null)
            {
                Meal.IdMeal = SaveOneMeal(Meal);
            }
            // if the FoodInMeal has not an IdMeal, we give it the Id of the current Meal 
            if (FoodToSave.IdMeal == null)
            {
                FoodToSave.IdMeal = Meal.IdMeal; 
            }
            // so the new meal will be the one of the FoodInMeal we are saving
            return Common.Database.SaveOneFoodInMeal(FoodToSave);
        }
        internal void SaveAllFoodsInMeal()
        {
            if (currentFoodsInMeal != null)
                foreach (FoodInMeal food in currentFoodsInMeal)
                {
                    // if it is necessary, the next method will create a new meal
                    Common.Database.SaveOneFoodInMeal(food);
                }
        }
        internal void DeleteOneFoodInMeal(FoodInMeal Food)
        {
            Common.Database.DeleteOneFoodInMeal(Food);
        }
        #endregion
        #region Foods
        internal int? SaveOneFood(Food food)
        {
            return Common.Database.SaveOneFood(food);
        }
        internal void DeleteOneFood(Food food)
        {
            // !!!! verify if  the row to delete has been used somewhere else in the database
            // !!!! if it is the case, don't delete and give notice to the caller 
            Common.Database.DeleteOneFood(food);
        }
        internal Food GetOneFood(int? idFood)
        {
            return Common.Database.GetOneFood(idFood);
        }
        internal List<Food> SearchFoods(string Name, string Description)
        {
            return Common.Database.SearchFoods(Name, Description);
        }
        internal List<Food> GetFoods()
        {
            return Common.Database.GetFoods();
        }
        public void CalculateChoOfFoodGrams(FoodInMeal Food)
        {
            if (Food.ChoPercent.Double != null && Food.QuantityGrams.Double != null)
            {
                Food.ChoGrams.Double = Food.ChoPercent.Double / 100 *
                    Food.QuantityGrams.Double;
            }
            RecalcTotalCho();
            RecalcTotalAccuracy();
        }
        #endregion
        internal string[] GetAllAccuracies()
        {
            return Enum.GetNames(typeof(Common.QualitativeAccuracy));
        }
        internal double? RecalcTotalCho()
        {
            double? total = 0;
            if (currentFoodsInMeal != null)
            {
                foreach (FoodInMeal f in currentFoodsInMeal)
                {
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
            if (currentFoodsInMeal != null && currentFoodsInMeal.Count > 0)
            {
                foreach (FoodInMeal f in currentFoodsInMeal)
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
                return null;
        }
        internal void FromFoodToFoodInMeal(Food SourceFood, FoodInMeal DestinationFoodInMeal)
        {
            DestinationFoodInMeal.IdFood = SourceFood.IdFood;
            DestinationFoodInMeal.ChoPercent = SourceFood.Cho; 
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
            DestinationFood.Cho = SourceFoodInMeal.ChoPercent;
            DestinationFood.Sugar = SourceFoodInMeal.SugarPercent;
            DestinationFood.Fibers = SourceFoodInMeal.FibersPercent;
        }
        internal TypeOfMeal SetTypeOfMealBasedOnTime()
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
        internal void NewDefaults()
        {
            Meal = new Meal();
            DateTime now = DateTime.Now;
            Meal.TimeBegin.DateTime = now;
            Meal.TimeEnd.DateTime = now;
            Meal.AccuracyOfChoEstimate.Double = 0;
            Meal.IdTypeOfMeal = SetTypeOfMealBasedOnTime();
        }
        public void SaveFoodInMealParameters()
        {
            Common.Database.SaveParameter("FoodInMeal_ChoGrams", currentFoodInMeal.ChoGrams.Text);
            Common.Database.SaveParameter("FoodInMeal_QuantityGrams", currentFoodInMeal.QuantityGrams.Text);
            Common.Database.SaveParameter("FoodInMeal_ChoPercent", currentFoodInMeal.ChoPercent.Text);
            Common.Database.SaveParameter("FoodInMeal_Name", currentFoodInMeal.Name);
            Common.Database.SaveParameter("FoodInMeal_AccuracyOfChoEstimate", currentFoodInMeal.AccuracyOfChoEstimate.Text);
        }
        public void RestoreFoodInMealParameters()
        {
            currentFoodInMeal.ChoGrams.Text = Common.Database.RestoreParameter("FoodInMeal_ChoGrams");
            currentFoodInMeal.QuantityGrams.Text = Common.Database.RestoreParameter("FoodInMeal_QuantityGrams");
            currentFoodInMeal.ChoPercent.Text = Common.Database.RestoreParameter("FoodInMeal_ChoPercent");
            currentFoodInMeal.Name = Common.Database.RestoreParameter("FoodInMeal_Name");
            currentFoodInMeal.AccuracyOfChoEstimate.Text = Common.Database.RestoreParameter("FoodInMeal_AccuracyOfChoEstimate");
        }
        public void SaveMealParameters()
        {
            Common.Database.SaveParameter("Meal_ChoGrams", Meal.Carbohydrates.Text);
        }
        public void RestoreMealParameters()
        {
            Meal.Carbohydrates.Text = Common.Database.RestoreParameter("Meal_ChoGrams");
        }
    }
}
