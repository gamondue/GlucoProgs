using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public class Meal
    {
        List<Food> foodsEaten;

        private DoubleAndText carbohydrates;

        public int? IdGlucoseRecord;
        public int? IdBolusCalculation;
        DateTimeAndText timeBegin;
        DateTimeAndText timeEnd;

        TypeOfMeal typeOfMeal;
        
        [DisplayName("Meal Code")]
        public int? IdMeal { get; set; }

        [DisplayName("Start time")]
        public DateTimeAndText TimeStart { get => timeBegin; set => timeBegin = value; }

        [DisplayName("Type of meal")]
        public TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }

        [DisplayName("CHO of meal")]
        public DoubleAndText CarbohydratesGrams { get => carbohydrates; set => carbohydrates = value; }

        [DisplayName("Accuracy of CHO")]
        public DoubleAndText AccuracyOfChoEstimate { get; set; }

        [DisplayName("Qualitative accuracy")]
        public QualitativeAccuracy QualitativeAccuracyOfChoEstimate { get; set; }

        [DisplayName("End time")]
        public DateTimeAndText TimeFinish { get => timeEnd; set => timeEnd = value; }

        public  List<Food> FoodsEaten { get => foodsEaten; set => foodsEaten = value; }
        //[DisplayName("Food Code")]
        //public int? IdFoodInMeal { get; set; }

        public  Meal()
        {
            // default type of meal is snack
            TypeOfMeal = Common.TypeOfMeal.Snack;
            TimeStart = new DateTimeAndText();
            TimeFinish = new DateTimeAndText();

            CarbohydratesGrams = new DoubleAndText();
            AccuracyOfChoEstimate = new DoubleAndText(); 
        }
    }
}
