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
        public Recipe()
        {
            CarbohydratesPercent = new DoubleAndText();
            AccuracyOfChoEstimate = new DoubleAndText();
        }
    }
}
