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
        public DoubleAndText QuantityGrams { get; set; }// [g/100]
        public DoubleAndText ChoGrams { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText ChoPercent { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText AccuracyOfChoEstimate { get; set; } // [0..1]
        //public QualitativeAccuracy QualitativeAccuracyOfCho;
        public DoubleAndText SugarPercent { get; set; }        // [g/100]
        public DoubleAndText FibersPercent { get; set; }       // [g/100]
        public string Name { get; set; }
        public QualitativeAccuracy QualitativeAccuracyOfCho { get; internal set; }

        public string Description;

        public FoodInMeal()
        {
            QuantityGrams = new DoubleAndText(); // [g]
            ChoGrams = new DoubleAndText();   // [g]
            ChoPercent = new DoubleAndText(); // [%]
            SugarPercent = new DoubleAndText();         // [%]
            FibersPercent = new DoubleAndText();        // [%]
            AccuracyOfChoEstimate = new DoubleAndText();
            //QualitativeAccuracyOfCho = QualitativeAccuracy.Perfect;

            ChoGrams.Format = "0.0";
            QuantityGrams.Format = "0";
            ChoPercent.Format = "0.0";
        }
    }
}
