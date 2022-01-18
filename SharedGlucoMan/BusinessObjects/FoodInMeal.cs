using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public class FoodInMeal
    {
        public int? IdFoodInMeal { get; set; }
        public int? IdFood { get; set; }
        public int? IdMeal { get; set; }
        public DoubleAndText Quantity { get; set; }// [g/100]
        public DoubleAndText CarbohydratesGrams { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText CarbohydratesPercent { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText AccuracyOfChoEstimate { get; set; } // [0..1]
        //public QualitativeAccuracy QualitativeAccuracyOfCho;
        public DoubleAndText SugarPercent { get; set; }        // [g/100]
        public DoubleAndText FibersPercent { get; set; }       // [g/100]
        public string Name { get; set; }

        public string Description;

        public FoodInMeal()
        {
            Quantity = new DoubleAndText(); // [g]
            CarbohydratesGrams = new DoubleAndText();   // [g]
            CarbohydratesPercent = new DoubleAndText(); // [%]
            SugarPercent = new DoubleAndText();         // [%]
            FibersPercent = new DoubleAndText();        // [%]
            AccuracyOfChoEstimate = new DoubleAndText();
            //QualitativeAccuracyOfCho = QualitativeAccuracy.Perfect;
        }
    }
}
