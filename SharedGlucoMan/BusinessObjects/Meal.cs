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
        public int? IdMeal { get; set; }
        [DisplayName("Type of meal")]
        public TypeOfMeal IdTypeOfMeal { get; set; }
        [DisplayName("CHO of meal")]
        public DoubleAndText CarbohydratesGrams { get; set; }
        
        // the following property is for string representation of carbohydrates, e.g. "2 slices of bread"
        // currently it is NOT STORED in the database, nor used in the code logic
        public DoubleAndText CarbohydratesString { get; set; }

        [DisplayName("Start time")]
        public new DateTimeAndText EventTime
        {
            get => base.EventTime;
            set => base.EventTime = value;
        }
        [DisplayName("Accuracy of CHO")]
        public DoubleAndText AccuracyOfChoEstimate { get; set; }
        public int? IdBolusCalculation { get; set; }
        public int? IdGlucoseRecord { get; set; }
        public int? IdInjection { get; internal set; }
        [DisplayName("End time")]
        public DateTimeAndText TimeEnd { get; set; }
        public List<Food> FoodsEaten { get; set; }
        public Meal()
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
