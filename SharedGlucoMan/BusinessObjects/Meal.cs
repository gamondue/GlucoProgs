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

        DateTimeAndText timeBegin;
        DateTimeAndText timeEnd;

        TypeOfMeal typeOfMeal;
        
        [DisplayName("Meal Code")]
        public int? IdMeal { get; set; }
        [DisplayName("Type of meal")]
        public TypeOfMeal IdTypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        [DisplayName("CHO of meal")]
        public DoubleAndText Carbohydrates { get => carbohydrates; set => carbohydrates = value; }
        [DisplayName("Start time")]
        public DateTimeAndText TimeBegin { get => timeBegin; set => timeBegin = value; }
        public string Notes { get; set; }
        [DisplayName("Accuracy of CHO")]
        public DoubleAndText AccuracyOfChoEstimate { get; set; }
        public int? IdBolusCalculation { get; set; }
        public int? IdGlucoseRecord { get; set; }
        [DisplayName("End time")]
        public DateTimeAndText TimeEnd { get => timeEnd; set => timeEnd = value; }
        public List<Food> FoodsEaten { get => foodsEaten; set => foodsEaten = value; }
        public Meal()
        {
            // default type of meal is NotSet
            IdTypeOfMeal = Common.TypeOfMeal.NotSet;
            TimeBegin = new DateTimeAndText();
            TimeEnd = new DateTimeAndText();

            Carbohydrates = new DoubleAndText();
            AccuracyOfChoEstimate = new DoubleAndText(); 
        }
    }
}
