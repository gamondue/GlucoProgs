using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan.BusinessLayer
{
    public class BL_MealAndFood
    {
        DataLayer dl = Common.Database;

        List<Meal> currentMeals;
        public List<Meal> Meals { get => currentMeals; set => currentMeals = value; }
        
        Meal currentMeal;
        List<FoodInMeal> currentFoodsInMeal;
        FoodInMeal currentFoodInMeal;
        public Meal Meal { get => currentMeal; set => currentMeal = value; }
        public List<FoodInMeal> Foods { get => currentFoodsInMeal; set => currentFoodsInMeal = value; }
        public BL_MealAndFood()
        {
            currentMeals = new List<Meal>();
            currentMeal = new Meal();
            currentFoodsInMeal = new List<FoodInMeal>();
            currentFoodInMeal = new FoodInMeal();
        }
        #region Meals
        public Meal GetOneMeal(int? IdMeal)
        {
            currentMeal = dl.GetOneMeal(IdMeal);
            return currentMeal;
        }
        public List<Meal> GetMeals(DateTime? InitialTime, DateTime? FinalTime)
        {
            currentMeals = dl.GetMeals(InitialTime, FinalTime);
            return currentMeals;
        }
        public int? SaveOneMeal(Meal Meal)
        {
            currentMeal = Meal;
            return dl.SaveOneMeal(currentMeal);
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

        public FoodInMeal FoodInMeal { get => currentFoodInMeal; set => currentFoodInMeal = value; }
        public List<FoodInMeal> GetFoodsInMeal(int? IdMeal)
        {
            currentFoodsInMeal = dl.GetFoodsInMeal(IdMeal);
            return currentFoodsInMeal;
        }
        public void SaveFoodsInMeal(List<FoodInMeal> List)
        {
            dl.SaveFoodsInMeal(List);
        }
        public int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            return dl.SaveOneFoodInMeal(FoodToSave);
        }
        internal void SaveAllFoodsInMeal(FoodInMeal foodInMeal)
        {
            foreach (FoodInMeal food in currentFoodsInMeal)
            {
                dl.SaveOneFoodInMeal(food);
            }
        }
        /// <summary>
        /// Changes numerical accuracy when qualitative accuracy changes
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void DeleteOneFoodInMeal(FoodInMeal Food)
        {
            dl.DeleteOneFoodInMeal(Food);
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
            // !!!! verify if  the row to delete has been used somewhere else in the database
            // !!!! if it is the case, don't delete and give notice to the caller 
            return dl.GetOneFood(idFood);
        }
        internal List<Food> SearchFoods(Food FoodToSearch)
        {
            return dl.SearchFood(FoodToSearch);
        }
        internal List<Food> ReadFoods()
        {
            return dl.GetFoods();
        }
        public void CalculateChoOfFoodGrams(FoodInMeal Food)
        {
            Food.CarbohydratesGrams.Double = Food.CarbohydratesPercent.Double / 100 *
                Food.Quantity.Double;
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
                    if (f.CarbohydratesGrams.Double != null)
                        // we don't sum the record that we are currently editing 
                        if (f.IdFoodInMeal != FoodInMeal.IdFoodInMeal)
                            total += f.CarbohydratesGrams.Double;
                }
                // we add the current editing value of CHO
                total += currentFoodInMeal.CarbohydratesGrams.Double;
                currentMeal.CarbohydratesGrams.Double = total;
            }
            return total; 
        }
        /// <summary>
        /// Calculate accuracy from single food accuracies
        /// </summary>
        /// <returns>Weighted quadratic average. Also modifies Meal. </returns>
        internal double? RecalcTotalAccuracy()
        {
            // calculate weighted quadratic sum 
            double sumOfWeights = 0;
            double sumOfSquaredWeightedValues = 0;
            double? WeightedQuadraticAverage = 0; 
            if (currentFoodsInMeal != null && currentFoodsInMeal.Count > 0)
            {
                foreach (FoodInMeal f in currentFoodsInMeal)
                {
                    if (f.CarbohydratesGrams == null)
                        f.CarbohydratesGrams.Double = 0;
                    sumOfWeights += (double)f.CarbohydratesGrams.Double;
                    if (f.AccuracyOfChoEstimate.Double != null)
                    {
                        // we don't sum the record that we are currently editing 
                        if (f.IdFoodInMeal != FoodInMeal.IdFoodInMeal)
                            sumOfSquaredWeightedValues +=
                              Math.Pow((double)f.AccuracyOfChoEstimate.Double, 2) * (double)f.CarbohydratesGrams.Double;
                    }
                    else
                    {
                        // if we have a null, we sum 0 
                    }
                }
                // we add the food that is currently edited
                if (FoodInMeal.AccuracyOfChoEstimate.Double != null && FoodInMeal.CarbohydratesGrams.Double != null)
                    sumOfSquaredWeightedValues +=
                    Math.Pow((double)FoodInMeal.AccuracyOfChoEstimate.Double, 2) * (double)FoodInMeal.CarbohydratesGrams.Double;
                else
                    // since we haven' enough data, the accuracy cannot be evaluated 
                    sumOfSquaredWeightedValues = double.MaxValue; 
                // square of the weighted quadratic average
                WeightedQuadraticAverage = Math.Sqrt(sumOfSquaredWeightedValues / sumOfWeights);
                currentMeal.AccuracyOfChoEstimate.Double = WeightedQuadraticAverage;
            }
            return WeightedQuadraticAverage;
        }
        internal TypeOfMeal SetTypeOfMealBasedOnTime()
        {
            TypeOfMeal type = TypeOfMeal.NotSet; 
            DateTime now = DateTime.Now;
            if (now.Hour > 6 && now.Hour < 9)
                currentMeal.IdTypeOfMeal = TypeOfMeal.Breakfast;
            else if (now.Hour > 12 && now.Hour < 14)
                currentMeal.IdTypeOfMeal = TypeOfMeal.Lunch;
            else if (now.Hour > 19 && now.Hour < 21)
                currentMeal.IdTypeOfMeal = TypeOfMeal.Dinner;
            else
                currentMeal.IdTypeOfMeal = TypeOfMeal.Snack;
            return currentMeal.IdTypeOfMeal; 
        }
        internal void NewDefaults()
        {
            currentMeal = new Meal();
            DateTime now = DateTime.Now;
            currentMeal.TimeBegin.DateTime = now;
            currentMeal.TimeEnd.DateTime = now;
            currentMeal.AccuracyOfChoEstimate.Double = 0;
            currentMeal.QualitativeAccuracyOfChoEstimate = QualitativeAccuracy.NotSet;
            currentMeal.IdTypeOfMeal = SetTypeOfMealBasedOnTime(); 
        }
    }
}
