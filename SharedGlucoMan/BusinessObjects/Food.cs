using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class Food
    {
        internal IntAndText IdFood { get; set; }
        internal string Name { get; set; }
        internal DoubleAndText Carbohydrates { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        internal DoubleAndText Sugar { get; set; }        // [g/100]
        internal DoubleAndText Fibers { get; set; }       // [g/100]
        internal DoubleAndText kCalories { get; set; }     // [kcal]  
        internal DoubleAndText TotalFats { get; set; }    // [g/100]
        internal DoubleAndText SaturatedFats { get; set; }// [g/100]
        internal DoubleAndText Proteins { get; set; }     // [g/100]
        internal DoubleAndText Salt { get; set; }         // [g/100]
        internal DoubleAndText Potassium { get; set; }    // [g/100]  
        internal DoubleAndText GlicemicIndex { get; set; } // [n]  
        // fields used for the list of foods in meal, NOT in the database
        internal DoubleAndText AccuracyOfChoEstimate { get; set; } // [0..1]

        public Food()
        {
            IdFood = new IntAndText(); 
            kCalories = new DoubleAndText();      // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            Fibers = new DoubleAndText();        // [g]
            Carbohydrates = new DoubleAndText(); // [g]
            Sugar = new DoubleAndText();         // [g]
            Proteins = new DoubleAndText();      // [g]
            Salt = new DoubleAndText();          // [g]
            Potassium = new DoubleAndText();     // [g]  
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
