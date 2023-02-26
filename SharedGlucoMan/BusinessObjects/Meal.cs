using gamon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public class Meal
    {      
        [DisplayName("Meal Code")]
        public int? IdMeal { get; set; }
        [DisplayName("Type of meal")]
        public TypeOfMeal IdTypeOfMeal { get; set; }
        [DisplayName("CHO of meal")]
        public DoubleAndText Carbohydrates { get; set; }
        [DisplayName("Start time")]
        public DateTimeAndText TimeBegin { get; set ; }
        public string Notes { get; set; }
        [DisplayName("Accuracy of CHO")]
        public DoubleAndText AccuracyOfChoEstimate { get; set; }
        public int? IdBolusCalculation { get; set; }
        public int? IdGlucoseRecord { get; set; }
        public int? IdInsulinInjection { get; internal set; }
        [DisplayName("End time")]
        public DateTimeAndText TimeEnd { get; set; }
        public List<Food> FoodsEaten { get; set; }
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
