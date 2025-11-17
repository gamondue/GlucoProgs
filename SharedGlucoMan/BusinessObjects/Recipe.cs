using gamon;

namespace GlucoMan
{
    public class Recipe
    {
        public int? IdRecipe { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DoubleAndText CarbohydratesPercent { get; set; }
        public DoubleAndText AccuracyOfChoEstimate { get; internal set; }
        public bool IsCooked { get; internal set; } // If true, the carb value refers to the cooked recipe. 
        public DoubleAndText RawToCookedRatio { get; internal set; }
        public DoubleAndText TotalWeight { get; set; }
        public List<Ingredient> Ingredients { get; internal set; }

        public Recipe()
        {
            CarbohydratesPercent = new DoubleAndText();
            AccuracyOfChoEstimate = new DoubleAndText();
            RawToCookedRatio = new();
            RawToCookedRatio.Format = "0.000";
            TotalWeight = new();
        }
    }
}
