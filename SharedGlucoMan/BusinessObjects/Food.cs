using gamon;

namespace GlucoMan
{
    public class Food
    {
        public int? IdFood { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DoubleAndText Energy { get; set; }    // [kcal]  
        public DoubleAndText TotalFats { get; set; }    // [g/100]
        public DoubleAndText SaturatedFats { get; set; }// [g/100]
        public DoubleAndText MonounsaturatedFats { get; set; }// [g/100]
        public DoubleAndText PolyunsaturatedFats { get; set; }// [g/100]
        public DoubleAndText Carbohydrates { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText Sugar { get; set; }        // [g/100]
        public DoubleAndText Fibers { get; set; }       // [g/100]
        public DoubleAndText Proteins { get; set; }     // [g/100]
        public DoubleAndText Salt { get; set; }         // [g/100]
        public DoubleAndText Cholesterol { get; set; }  // [g/100]
        public DoubleAndText Potassium { get; set; }    // [g/100]  
        public DoubleAndText GlycemicIndex { get; set; } // [n]
        public UnitOfFood Unit { get; set; }
        public ManufacturerOfFood Manufacturer { get; set; }
        public CategoryOfFood Category { get; set; }
        public Food()
        {
            Energy = new DoubleAndText();        // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            MonounsaturatedFats = new DoubleAndText(); // [g]
            PolyunsaturatedFats = new DoubleAndText(); // [g]
            Carbohydrates = new DoubleAndText();           // [g]
            Sugar = new DoubleAndText();         // [g]
            Fibers = new DoubleAndText();        // [g]
            Proteins = new DoubleAndText();      // [g]
            Salt = new DoubleAndText();          // [g]
            Cholesterol = new DoubleAndText();   // [g]  
            Potassium = new DoubleAndText();     // [g]  
            GlycemicIndex = new DoubleAndText(); // [n]  
        }
    }
}
