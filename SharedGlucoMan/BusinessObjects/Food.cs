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
        internal DoubleAndText Cho { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        internal DoubleAndText Sugar { get; set; }        // [g/100]
        internal DoubleAndText Fibers { get; set; }       // [g/100]
        internal DoubleAndText Proteins { get; set; }     // [g/100]
        internal DoubleAndText Salt { get; set; }         // [g/100]
        internal DoubleAndText Potassium { get; set; }    // [g/100]  
        internal DoubleAndText Cholesterol { get; set; }
        internal DoubleAndText GlycemicIndex { get; set; } // [n]

        public Food()
        {
            Energy = new DoubleAndText();      // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            Fibers = new DoubleAndText();        // [g]
            Cho = new DoubleAndText(); // [g]
            Sugar = new DoubleAndText();         // [g]
            Fibers = new DoubleAndText();      // [g]
            Proteins = new DoubleAndText();      // [g]
            Salt = new DoubleAndText();          // [g]
            Potassium = new DoubleAndText();     // [g]  
            GlycemicIndex = new DoubleAndText(); // [n]  
        }
    }
}
