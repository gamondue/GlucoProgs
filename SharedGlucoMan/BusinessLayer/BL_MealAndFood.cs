using static GlucoMan.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace GlucoMan.BusinessLayer
{
    public class BL_MealAndFood
    {
        DataLayer dl = Common.Database;

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
            Meal = dl.GetOneMeal(IdMeal);
            return Meal;
        }
        public List<Meal> GetMeals(DateTime? InitialTime, DateTime? FinalTime)
        {
            currentMeals = dl.GetMeals(InitialTime, FinalTime);
            return currentMeals;
        }
        public int? SaveOneMeal(Meal Meal)
        {
            this.Meal = Meal;
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
        internal void SaveAllFoodsInMeal()
        {
            if (currentFoodsInMeal != null)
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
        internal List<Food> SearchFoods(string Name, string Description)
        {
            return dl.SearchFood(Name, Description);
        }
        internal List<Food> ReadFoods()
        {
            return dl.GetFoods();
        }
        public void CalculateChoOfFoodGrams(FoodInMeal Food)
        {
            if (Food.ChoPercent.Double != null && Food.QuantityGrams.Double != null)
            {
                Food.ChoGrams.Double = Food.ChoPercent.Double / 100 *
                    Food.QuantityGrams.Double;
                RecalcTotalCho();
                RecalcTotalAccuracy();
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
            if (currentFoodsInMeal != null)
            {
                foreach (FoodInMeal f in currentFoodsInMeal)
                {
                    total += f.ChoGrams.Double;
                }
                Meal.ChoGrams.Double = total;
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
                    if (f.ChoGrams.Double == null)
                    {
                        IsValueCorrect = false;
                        break;
                    }
                    // if we don't have the accuracy of a component, we can't calculate the propagation of uncertainty, 
                    if (f.AccuracyOfChoEstimate == null)
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
            return WeightedQuadraticAverage;
        }
        internal void FromFoodToFoodInMeal(Food SourceFood, FoodInMeal DestinationFoodInMeal)
        {
            DestinationFoodInMeal.IdFood = SourceFood.IdFood;
            DestinationFoodInMeal.ChoPercent = SourceFood.Cho; 
            DestinationFoodInMeal.Name = SourceFood.Name;
            DestinationFoodInMeal.Description = SourceFood.Description;
            DestinationFoodInMeal.SugarPercent = SourceFood.Sugar;
            DestinationFoodInMeal.FibersPercent = SourceFood.Fibers;         }
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
            Meal.QualitativeAccuracyOfChoEstimate = QualitativeAccuracy.NotSet;
            Meal.IdTypeOfMeal = SetTypeOfMealBasedOnTime();
        }
        internal QualitativeAccuracy GetQualitativeAccuracyGivenQuantitavive(double? NumericalAccuracy)
        {
            if (NumericalAccuracy <= 0)
            {
                return QualitativeAccuracy.NotSet;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryBad)
            {
                return QualitativeAccuracy.Null;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Bad)
            {
                return QualitativeAccuracy.VeryBad;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Poor)
            {
                return QualitativeAccuracy.Bad;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.AlmostSufficient)
            {
                return QualitativeAccuracy.Poor;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Sufficient)
            {
                return QualitativeAccuracy.AlmostSufficient;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Satisfactory)
            {
                return QualitativeAccuracy.Sufficient;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Good)
            {
                return QualitativeAccuracy.Satisfactory;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryGood)
            {
                return QualitativeAccuracy.Good;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Outstanding)
            {
                return QualitativeAccuracy.VeryGood;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Perfect)
            {
                return QualitativeAccuracy.Outstanding;
            }
            else
            {
                return QualitativeAccuracy.Perfect;
            }
        }
        internal Color AccuracyBackColor(double NumericalAccuracy)
        {
            Color c; 
            if (NumericalAccuracy <= 0)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryBad)
            {
                c = Color.Red;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Bad)
            {
                c = Color.DarkRed;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Poor)
            {
                c = Color.OrangeRed;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.AlmostSufficient)
            {
                c = Color.Orange;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Sufficient)
            {
                c = Color.Yellow;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Satisfactory)
            {
                c = Color.YellowGreen;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Good)
            {
                c = Color.GreenYellow;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryGood)
            {
                c = Color.LawnGreen;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Outstanding)
            {
                c = Color.DarkSeaGreen;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Perfect)
            {
                c = Color.Green;
            }
            else
            {
                c = Color.Lime;
            }
            return c; 
        }
        internal Color AccuracyForeColor(double NumericalAccuracy)
        {
            Color c;
            if (NumericalAccuracy <= 0)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryBad)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Bad)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Poor)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.AlmostSufficient)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Sufficient)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Satisfactory)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Good)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.VeryGood)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Outstanding)
            {
                c = Color.White;
            }
            else if (NumericalAccuracy < (double)QualitativeAccuracy.Perfect)
            {
                c = Color.White;
            }
            else
            {
                c = Color.White;
            }
            return c;
        }
        public void SaveFoodInMealParameters()
        {
            dl.SaveParameter("FoodInMeal_ChoGrams", currentFoodInMeal.ChoGrams.Text);
            dl.SaveParameter("FoodInMeal_QuantityGrams", currentFoodInMeal.QuantityGrams.Text);
            dl.SaveParameter("FoodInMeal_ChoPercent", currentFoodInMeal.ChoPercent.Text);
            dl.SaveParameter("FoodInMeal_Name", currentFoodInMeal.Name);
            dl.SaveParameter("FoodInMeal_AccuracyOfChoEstimate", currentFoodInMeal.AccuracyOfChoEstimate.Text);
        }
        public void RestoreFoodInMealParameters()
        {
            currentFoodInMeal.ChoGrams.Text = dl.RestoreParameter("FoodInMeal_ChoGrams");
            currentFoodInMeal.QuantityGrams.Text = dl.RestoreParameter("FoodInMeal_QuantityGrams");
            currentFoodInMeal.ChoPercent.Text = dl.RestoreParameter("FoodInMeal_ChoPercent");
            currentFoodInMeal.Name = dl.RestoreParameter("FoodInMeal_Name");
            currentFoodInMeal.AccuracyOfChoEstimate.Text = dl.RestoreParameter("FoodInMeal_AccuracyOfChoEstimate");
        }
        public void SaveMealParameters()
        {
            dl.SaveParameter("Meal_ChoGrams", Meal.ChoGrams.Text);
        }
        public void RestoreMealParameters()
        {
            Meal.ChoGrams.Text = dl.RestoreParameter("Meal_ChoGrams");
        }
    }
}
