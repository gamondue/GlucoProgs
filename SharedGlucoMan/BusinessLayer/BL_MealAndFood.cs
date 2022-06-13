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
        Meal currentMeal;
        List<FoodInMeal> currentFoodsInMeal;
        FoodInMeal currentFoodInMeal;

        public Meal Meal { get => currentMeal; set => currentMeal = value; }
        public List<FoodInMeal> Foods { get => currentFoodsInMeal; set => currentFoodsInMeal = value; }
        public FoodInMeal FoodInMeal { get => currentFoodInMeal; set => currentFoodInMeal = value; }

        internal List<Food> SearchFoods(Food FoodToSearch)
        {
            return dl.SearchFood(FoodToSearch); 
        }

        public List<Meal> Meals { get => currentMeals; set => currentMeals = value; }

        public BL_MealAndFood()
        {
            currentMeals = new List<Meal>();
            currentMeal = new Meal();
            currentFoodsInMeal = new List<FoodInMeal>();
            currentFoodInMeal = new FoodInMeal();
        }
        public void CalculateChoOfFoodGrams(FoodInMeal Food)
        {
            Food.CarbohydratesGrams.Double = Food.CarbohydratesPercent.Double / 100 * 
                Food.Quantity.Double;
        }
        public void CalculateChoOfMeal()
        {
            Common.LogOfProgram.Error("Sqlite_MealAndFood | RestoreFoodsInMeal", 
                new Exception());
        }
        public void ReadMeals(DateTime? InitialTime, DateTime? FinalTime)
        {
            currentMeals = dl.ReadMeals(InitialTime, FinalTime);
        }
        public void ReadMeal(int? IdMeal)
        {
            currentMeal = dl.ReadOneMeal(IdMeal);
        }
        public int? SaveOneMeal()
        {
            return dl.SaveOneMeal(currentMeal);
        }
        internal void DeleteOneMeal(Meal Meal)
        {
            dl.DeleteOneMeal(Meal);
        }
        public void ReadFoodsInMeal(int? IdMeal)
        {
            currentFoodsInMeal = dl.ReadFoodsInMeal(IdMeal);
        }
        public void SaveFoodsInMeal(List<FoodInMeal> List)
        {
            dl.SaveFoodsInMeal(List);
        }
        public int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            return dl.SaveOneFoodInMeal(FoodToSave);
        }
        /// <summary>
        /// Changes numerical accuracy when qualitative accuracy changes
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal void DeleteOneFoodInMeal(FoodInMeal Food)
        {
            dl.DeleteOneFoodInMeal(Food);
        }
        internal double QualitativeAccuracyChanged(QualitativeAccuracy QualitativeAccuracy)
        {
            return (double)QualitativeAccuracy;
        }
        /// <summary>
        /// Changes qualitative accuracy when numerical accuracy changes
        /// </summary>
        /// <param name="currentMeal"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal QualitativeAccuracy NumericalAccuracyChanged(double? NumericalAccuracy)
        {
            if (NumericalAccuracy < 0)
                return QualitativeAccuracy.NotSet;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryBad)
                return QualitativeAccuracy.Null;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Bad)
                return QualitativeAccuracy.VeryBad;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Poor)
                return QualitativeAccuracy.Bad;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.AlmostSufficient)
                return QualitativeAccuracy.Poor;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Sufficient)
                return QualitativeAccuracy.AlmostSufficient;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Satisfactory)
                return QualitativeAccuracy.Sufficient;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Good)
                return QualitativeAccuracy.Satisfactory;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryGood)
                return QualitativeAccuracy.Good;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Outstanding)
                return QualitativeAccuracy.VeryGood;
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Perfect)
                return QualitativeAccuracy.Outstanding;
            else 
                return QualitativeAccuracy.Perfect;
        }
        internal double? RecalcTotalCho()
        {
            double? total = 0;
            foreach (FoodInMeal f in currentFoodsInMeal)
            {
                if (f.CarbohydratesGrams.Double != null)
                    // we don't sum the record that we are currently editing 
                    if (f.IdFoodInMeal != FoodInMeal.IdFoodInMeal)
                        total += f.CarbohydratesGrams.Double;
            }
            // we add the current editing value of CHO
            total += currentFoodInMeal.CarbohydratesGrams.Double; 
            currentMeal.Carbohydrates.Double = total; 
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
            foreach (FoodInMeal f in currentFoodsInMeal)
            {
                if (f.CarbohydratesGrams == null)
                    f.CarbohydratesGrams.Double = 0;
                sumOfWeights += (double) f.CarbohydratesGrams.Double; 
                if (f.AccuracyOfChoEstimate.Double != null)
                {
                    // we don't sum the record that we are currently editing 
                    if (f.IdFoodInMeal != FoodInMeal.IdFoodInMeal)
                        sumOfSquaredWeightedValues +=
                          Math.Pow((double) f.AccuracyOfChoEstimate.Double, 2) * (double) f.CarbohydratesGrams.Double;
                }
                else
                {
                    // if we have a null, we sum 0 
                }
            }
            // we add the food that is currently editing 
            sumOfSquaredWeightedValues +=
                Math.Pow((double) FoodInMeal.AccuracyOfChoEstimate.Double, 2) * (double)FoodInMeal.CarbohydratesGrams.Double;
            // square of the weighted quadratic average
            double? WeightedQuadraticAverage = Math.Sqrt(sumOfSquaredWeightedValues / sumOfWeights);
            currentMeal.AccuracyOfChoEstimate.Double = WeightedQuadraticAverage;
            return WeightedQuadraticAverage;
        }
    }
}
