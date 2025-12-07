using gamon;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public class Meal : Event
    {      
        [DisplayName("Meal Code")]
        internal int? IdMeal { get; set; }
        [DisplayName("Type of meal")]
        internal TypeOfMeal IdTypeOfMeal { get; set; }
        [DisplayName("CHO of meal")]
        internal DoubleAndText CarbohydratesGrams { get; set; }

        // the following property is for string representation of carbohydrates, e.g. "2 slices of bread"
        // currently it is NOT STORED in the database, nor used in the code logic
        internal DoubleAndText CarbohydratesString { get; set; }

        [DisplayName("Start time")]
        internal new DateTimeAndText EventTime
        {
            get => base.EventTime;
            set => base.EventTime = value;
        }
        [DisplayName("Accuracy of CHO")]
        internal DoubleAndText AccuracyOfChoEstimate { get; set; }
        internal int? IdBolusCalculation { get; set; }
        internal int? IdGlucoseRecord { get; set; }
        internal int? IdInjection { get; set; }
        [DisplayName("End time")]
        internal DateTimeAndText TimeEnd { get; set; }
        internal List<Food> FoodsEaten { get; set; }
        internal Meal()
        {
            // default type of meal is NotSet
            IdTypeOfMeal = Common.TypeOfMeal.NotSet;
            EventTime = new DateTimeAndText();
            TimeEnd = new DateTimeAndText();

            CarbohydratesGrams = new DoubleAndText();
            AccuracyOfChoEstimate = new DoubleAndText(); 
        }
        private string _name;
    }
}
