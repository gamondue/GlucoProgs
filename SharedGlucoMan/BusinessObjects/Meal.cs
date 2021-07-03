using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal class Meal
    {
        internal enum TypeOfMeal
        {
            Breakfast,
            Lunch,
            Dinner,
            Snack
        }

        List<Food> foodsEaten;
        DateTime timeBegin;
        DateTime timeEnd;
        TypeOfMeal type;
        int accuracyOfChoEstimate;

        int breakfastStartHour = 6;
        int breakfastEndHour = 10;
        int lunchStartHour = 11;
        int lunchEndHour = 15;
        int dinnerStartHour = 17;
        int dinnerEndHour = 21;

        internal List<Food> FoodsEaten { get => foodsEaten; set => foodsEaten = value; }
        internal DateTime TimeBegin { get => timeBegin; set => timeBegin = value; }
        internal DateTime TimeEnd { get => timeEnd; set => timeEnd = value; }
        internal TypeOfMeal Type { get => type; set => type = value; }
        internal int AccuracyOfChoEstimate { get => accuracyOfChoEstimate; set => accuracyOfChoEstimate = value; }
        internal Meal()
        {
            // default type of meal is snack
            type = TypeOfMeal.Snack;
        }
        internal TypeOfMeal SelectMealBasedOnTimeNow()
        {
            int hour = DateTime.Now.Hour;
            if (hour > breakfastStartHour && hour < breakfastEndHour)
                type = TypeOfMeal.Breakfast;
            else if (hour > lunchStartHour && hour < lunchEndHour)
                type = TypeOfMeal.Lunch;
            else if (hour > dinnerStartHour && hour < dinnerEndHour)
                type = TypeOfMeal.Lunch;
            else
                type = TypeOfMeal.Snack;
            return type;
        }
    }
}
