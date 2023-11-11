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
        public DoubleAndText MonoinsaturatedFats { get; set; }// [g/100]     
        public DoubleAndText PolinsaturatedFats { get; set; }// [g/100]     
        public DoubleAndText Cho { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText Sugar { get; set; }        // [g/100]
        public DoubleAndText Fibers { get; set; }       // [g/100]
        public DoubleAndText Proteins { get; set; }     // [g/100]
        public DoubleAndText Salt { get; set; }         // [g/100]
        public DoubleAndText Potassium { get; set; }    // [g/100]  
        public DoubleAndText Cholesterol { get; set; }
        public DoubleAndText GlycemicIndex { get; set; } // [n]
        public Food()
        {
            Energy = new DoubleAndText();      // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            MonoinsaturatedFats = new DoubleAndText(); // [g]
            PolinsaturatedFats = new DoubleAndText(); // [g]
            Cho = new DoubleAndText(); // [g]
            Sugar = new DoubleAndText();         // [g]
            Fibers = new DoubleAndText();      // [g]
            Proteins = new DoubleAndText();      // [g]
            Salt = new DoubleAndText();          // [g]
            Potassium = new DoubleAndText();     // [g]  
            Cholesterol = new DoubleAndText();     // [g]  
            GlycemicIndex = new DoubleAndText(); // [n]  
        }
    }
}
