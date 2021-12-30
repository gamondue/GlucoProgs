using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal class Meal
    {
        List<Food> foodsEaten;
        DateTime timeBegin;
        DateTime timeEnd;
        Common.TypeOfMeal type;
        double accuracyOfChoEstimate;

		internal int IdMeal { get; set; }
        internal List<Food> FoodsEaten { get => foodsEaten; set => foodsEaten = value; }
        internal DateTime TimeBegin { get => timeBegin; set => timeBegin = value; }
        internal DateTime TimeEnd { get => timeEnd; set => timeEnd = value; }
        internal Common.TypeOfMeal Type { get => type; set => type = value; }
        internal double AccuracyOfChoEstimate { get => accuracyOfChoEstimate; set => accuracyOfChoEstimate = value; }
        internal Common.QualitativeAccuracy QualitativeAccuracyOfChoEstimate { get; set; }
        internal Meal()
        {
            // default type of meal is snack
            type = Common.TypeOfMeal.Snack;
        }
    }
}
