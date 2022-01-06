using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    internal class Meal
    {
        List<Food> foodsEaten;

        DateTimeAndText timeBegin;
        DateTimeAndText timeEnd;

        TypeOfMeal TypeOfMeal;
        double accuracyOfChoEstimate;

		internal int IdMeal { get; set; }
        internal TypeOfMeal Type { get => TypeOfMeal; set => TypeOfMeal = value; }
        internal DateTimeAndText TimeBegin { get => timeBegin; set => timeBegin = value; }
        internal DateTimeAndText TimeEnd { get => timeEnd; set => timeEnd = value; }
        internal DoubleAndText Carbohydrates;
        internal double AccuracyOfChoEstimate { get => accuracyOfChoEstimate; set => accuracyOfChoEstimate = value; }
        internal QualitativeAccuracy QualitativeAccuracyOfChoEstimate { get; set; }
        internal int? IdGlucoseRecord;
        internal int? IdBolusCalculation;
        internal QualitativeAccuracy QualitativeAccuracyCHO;
        internal TypeOfInsulinInjection InsulineInjection;

        internal List<Food> FoodsEaten { get => foodsEaten; set => foodsEaten = value; }

        internal Meal()
        {
            // default type of meal is snack
            TypeOfMeal = Common.TypeOfMeal.Snack;
        }
    }
}
