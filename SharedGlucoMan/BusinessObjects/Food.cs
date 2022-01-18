using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan
{
    public class Food
    {
        internal int? IdFood { get; set; }
        internal string Name { get; set; }
        internal string Description;
        internal DoubleAndText Energy { get; set; }    // [kcal]  
        internal DoubleAndText TotalFats { get; set; }    // [g/100]
        internal DoubleAndText SaturatedFats { get; set; }// [g/100]
        internal DoubleAndText Carbohydrates { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        internal DoubleAndText Sugar { get; set; }        // [g/100]
        internal DoubleAndText Fibers { get; set; }       // [g/100]
        internal DoubleAndText Proteins { get; set; }     // [g/100]
        internal DoubleAndText Salt { get; set; }         // [g/100]
        internal DoubleAndText Potassium { get; set; }    // [g/100]  
        internal DoubleAndText GlicemicIndex { get; set; } // [n]  
        internal DoubleAndText AccuracyOfChoEstimate { get; set; } // [0..1]
        internal QualitativeAccuracy QualitativeAccuracyOfCho; 

        public Food()
        {
            Energy = new DoubleAndText();      // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            Fibers = new DoubleAndText();        // [g]
            Carbohydrates = new DoubleAndText(); // [g]
            Sugar = new DoubleAndText();         // [g]
            Proteins = new DoubleAndText();      // [g]
            Salt = new DoubleAndText();          // [g]
            Potassium = new DoubleAndText();     // [g]  
            QualitativeAccuracyOfCho = QualitativeAccuracy.Perfect;
        }
        //public void Save(Food FoodToSave)
        //{
        //    dl.SaveSingleFood(FoodToSave); 
        //}
        // public List<Food> ReadAllFoods()
        //{
        //    return dl.ReadAllFoods(); 
        //}
    }
}
