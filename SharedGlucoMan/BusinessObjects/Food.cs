using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    class Food
    {
        internal int IdFood { get; set; } 
        internal string Name { get; set; } 
        internal double? Quantity { get; set; }     // [n]  
        internal double? Calories { get; set; }     // [kcal]  
        internal double? TotalFats { get; set; }    // [g]
        internal double? SaturatedFats { get; set; }// [g]
        internal double? Fibers { get; set; }       // [g]
        internal double? Carbohydrates { get; set; }// [g]
        internal double? Sugar { get; set; }        // [g]
        internal double? Proteins { get; set; }     // [g]
        internal double? Salt { get; set; }         // [g]
        internal double? Potassium { get; set; }    // [g]  
    }
}
