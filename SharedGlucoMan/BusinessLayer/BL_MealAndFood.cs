using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan.BusinessLayer
{
    public  class BL_MealAndFood
    {
        DataLayer dl = Common.Database;
        List<Meal> mealsList;

        public  DoubleAndText FoodChoGrams = new DoubleAndText();
        public  DoubleAndText FoodQuantityGrams = new DoubleAndText();
        public  DoubleAndText FoodChoPercent = new DoubleAndText(); 

        public  BL_MealAndFood()
        {
            FoodChoGrams.Format = "0.0";
            FoodQuantityGrams.Format = "0";
            FoodChoPercent.Format = "0.0";
        }
        public  void CalculateChoOfFoodGrams()
        {
            FoodChoGrams.Double = FoodChoPercent.Double / 100 * FoodQuantityGrams.Double;
        }
        public  void CalculateChoOfMeal()
        {
            Common.LogOfProgram.Error("Sqlite_MealAndFood | RestoreFoodsInMeal", 
                new NotImplementedException());
        }
        public  List<Meal> ReadMeals(DateTime? InitialTime, DateTime? FinalTime)
        {
            return dl.ReadMeals(InitialTime, FinalTime);
        }
        public  void ReadMeal()
        {
            dl.ReadMeal(this);
        }
        public  void SaveOneMeal(Meal ThisMeal)
        {
            dl.SaveOneMeal(ThisMeal);
        }
        public  List<FoodInMeal> ReadFoodsInMeal(int? IdMeal)
        {
            return dl.ReadFoodsInMeal(IdMeal);
        }
        public  void SaveFoodsInMeal(List<FoodInMeal> List)
        {
            dl.SaveFoodsInMeal(List);
        }
        public  int? SaveOneFoodInMeal(FoodInMeal FoodToSave)
        {
            return dl.SaveOneFoodInMeal(FoodToSave);
        }
        /// <summary>
        /// Changes numerical accuracy when qualitative accuracy changes
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        internal double QualitativeAccuracyChanged(QualitativeAccuracy QualitativeAccuracy)
        {
            return (double)QualitativeAccuracy;
        }
        /// <summary>
        /// Changes qualitative accuracy when numerical accuracy changes
        /// </summary>
        /// <param name="currentMeal"></param>
        /// <exception cref="NotImplementedException"></exception>
        internal QualitativeAccuracy NumericalAccuracyChanged(double NumericalAccuracy)
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
    }
}
