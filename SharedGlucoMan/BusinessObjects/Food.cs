
using gamon;

namespace GlucoMan
{
    public class Food
    {
        public int? IdFood { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DoubleAndText Energy { get; set; }    // [kcal]  
        public DoubleAndText TotalFatsPercent { get; set; } // [g/100]
        public DoubleAndText SaturatedFatsPercent { get; set; }// [g/100]
        public DoubleAndText MonounsaturatedFatsPercent { get; set; }// [g/100]
        public DoubleAndText PolyunsaturatedFatsPercent { get; set; }// [g/100]
        public DoubleAndText CarbohydratesPercent { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText SugarPercent { get; set; }        // [g/100]
        public DoubleAndText PotassiumPercent { get; set; }    // [g/100]  
        public DoubleAndText FibersPercent { get; set; }       // [g/100]
        public DoubleAndText ProteinsPercent { get; set; }     // [g/100]
        public DoubleAndText SaltPercent { get; set; }         // [g/100]
        public DoubleAndText Cholesterol { get; set; }  // [g/100]
        public DoubleAndText GlycemicIndex { get; set; } // [n]
        public string UnitSymbol { get; set; }
        public DoubleAndText GramsInOneUnit { get; set; } // [g]
        public string Manufacturer { get; set; }
        public string Category { get; set; }
        public List<UnitOfFood> Units { get; set; }
        // unit is mandative to set the "internal" values in grams
        public Food(UnitOfFood Unit)
        {
            Energy = new DoubleAndText();        // [kcal]  
            TotalFatsPercent = new DoubleAndText();     // [g]
            SaturatedFatsPercent = new DoubleAndText(); // [g]
            MonounsaturatedFatsPercent = new DoubleAndText(); // [g]
            PolyunsaturatedFatsPercent = new DoubleAndText(); // [g]
            CarbohydratesPercent = new DoubleAndText();       // [g]
            SugarPercent = new DoubleAndText();         // [g]
            FibersPercent = new DoubleAndText();        // [g]
            ProteinsPercent = new DoubleAndText();      // [g]
            SaltPercent = new DoubleAndText();          // [g]
            Cholesterol = new DoubleAndText();   // [g]  
            PotassiumPercent = new DoubleAndText();     // [g]  
            GlycemicIndex = new DoubleAndText();  // [n]
            GramsInOneUnit = new DoubleAndText(); // [g]
        }
    }
}
