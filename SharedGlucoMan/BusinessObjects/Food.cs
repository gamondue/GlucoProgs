using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class Food
    {
        internal int IdFood { get; set; } 
        internal string Name { get; set; }
        internal double? Carbohydrates { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        internal double? Sugar { get; set; }        // [g/100]
        internal double? Fibers { get; set; }       // [g/100]
        internal double? kCalories { get; set; }     // [kcal]  
        internal double? TotalFats { get; set; }    // [g/100]
        internal double? SaturatedFats { get; set; }// [g/100]
        internal double? Proteins { get; set; }     // [g/100]
        internal double? Salt { get; set; }         // [g/100]
        internal double? Potassium { get; set; }    // [g/100]  
        internal double? GlicemicIndex { get; set; } // [n]  
        // fields used for the list of foods in meal, NOT in the database
        internal double AccuracyOfChoEstimate { get; set; } // [0..1]

    }
}
