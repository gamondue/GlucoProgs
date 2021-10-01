using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal class Food
    {
        DataLayer dl = new DataLayer();
        internal IntAndText IdFood { get; set; }
        internal string Name { get; set; }
        internal DoubleAndText Quantity { get; set; }     // [n]  
        internal DoubleAndText Calories { get; set; }     // [kcal]  
        internal DoubleAndText TotalFats { get; set; }    // [g]
        internal DoubleAndText SaturatedFats { get; set; }// [g]
        internal DoubleAndText Fibers { get; set; }       // [g]
        internal DoubleAndText Carbohydrates { get; set; }// [g]
        internal DoubleAndText Sugar { get; set; }        // [g]
        internal DoubleAndText Proteins { get; set; }     // [g]
        internal DoubleAndText Salt { get; set; }         // [g]
        internal DoubleAndText Potassium { get; set; }    // [g]  

        internal Food()
        {
            IdFood = new IntAndText(); 

            Quantity = new DoubleAndText();      // [n]  
            Calories = new DoubleAndText();      // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            Fibers = new DoubleAndText();        // [g]
            Carbohydrates = new DoubleAndText(); // [g]
            Sugar = new DoubleAndText();         // [g]
            Proteins = new DoubleAndText();      // [g]
            Salt = new DoubleAndText();          // [g]
            Potassium = new DoubleAndText();     // [g]  

            Quantity.Double = 100; 
        }
        internal void Save(Food FoodToSave)
        {
            dl.SaveSingleFood(FoodToSave); 
        }
        internal List<Food> ReadAllFoods()
        {
            return dl.ReadAllFoods(); 
        }
    }
}
