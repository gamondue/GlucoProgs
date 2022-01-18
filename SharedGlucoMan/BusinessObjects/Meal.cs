using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public  class Meal
    {
        List<Food> foodsEaten;

        DateTimeAndText timeBegin;
        DateTimeAndText timeEnd;

        TypeOfMeal typeOfMeal;
        double? accuracyOfChoEstimate;

        [DisplayName("Meal Code")]
        public  int? IdMeal { get; set; }
        public  TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        [DisplayName("Start time")]
        public DateTimeAndText TimeStart { get => timeBegin; set => timeBegin = value; }
        [DisplayName("End time")]
        public DateTimeAndText TimeEnd { get => timeEnd; set => timeEnd = value; }
        //[DisplayName("CHO of meal")]
        public DoubleAndText Carbohydrates;
        [DisplayName("Accuracy of CHO")]
        public double? AccuracyOfChoEstimate { get => accuracyOfChoEstimate; set => accuracyOfChoEstimate = value; }
        public  QualitativeAccuracy QualitativeAccuracyOfChoEstimate { get; set; }
        public  int? IdGlucoseRecord;
        public  int? IdBolusCalculation;
        public  TypeOfInsulinInjection TypeOfInsulineInjection;
        public  List<Food> FoodsEaten { get => foodsEaten; set => foodsEaten = value; }
        public int? IdInsulineInjection { get; set; }
        public  Meal()
        {
            // default type of meal is snack
            TypeOfMeal = Common.TypeOfMeal.Snack;
            TimeStart = new DateTimeAndText();
            TimeEnd = new DateTimeAndText();

            Carbohydrates = new DoubleAndText();
        }
    }
}
