using gamon;

namespace GlucoMan
{
    public class Food
    {
        public int? IdFood { get; set; }
        public string Name { get; set; }
        public string Description;
        public DoubleAndText Energy { get; set; }    // [kcal]  
        public DoubleAndText TotalFats { get; set; }    // [g/100]
        public DoubleAndText SaturatedFats { get; set; }// [g/100]
        public DoubleAndText MonoSaturatedFats { get; set; }// [g/100]
        public DoubleAndText PoliSaturatedFats { get; set; }// [g/100]
        public DoubleAndText Cho { get; set; }// [g/100 in database,g in the list of foods in a Meal] 
        public DoubleAndText Sugar { get; set; }        // [g/100]
        public DoubleAndText Fibers { get; set; }       // [g/100]
        public DoubleAndText Proteins { get; set; }     // [g/100]
        public DoubleAndText Salt { get; set; }         // [g/100]
        public DoubleAndText Cholesterol { get; set; }  // [g/100]
        public DoubleAndText Potassium { get; set; }    // [g/100]  
        public DoubleAndText GlycemicIndex { get; set; } // [n]
        public UnitOfFood Unit { get; set; }
        public ProducerOfFood Producer { get; set; }
        public CategoryOfFood Category { get; set; }

        public Food()
        {
            Energy = new DoubleAndText();        // [kcal]  
            TotalFats = new DoubleAndText();     // [g]
            SaturatedFats = new DoubleAndText(); // [g]
            MonoSaturatedFats = new DoubleAndText(); // [g]
            PoliSaturatedFats = new DoubleAndText(); // [g]
            Cho = new DoubleAndText();           // [g]
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
