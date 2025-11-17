using gamon;

namespace GlucoMan
{
    public class Ingredient
    {
        public int? IdIngredient { get; set; }
        public int? IdRecipe { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DoubleAndText QuantityGrams { get; set; }
        public DoubleAndText QuantityPercent { get; set; }
        public DoubleAndText CarbohydratesPercent { get; set; }
        public DoubleAndText CarbohydratesGrams { get; internal set; }
        public DoubleAndText AccuracyOfChoEstimate { get; internal set; }
        public int? IdFood { get; set; }

        public Ingredient()
        {
            QuantityGrams = new DoubleAndText();
            QuantityPercent = new DoubleAndText();
            CarbohydratesPercent = new DoubleAndText();
            CarbohydratesGrams = new DoubleAndText();
            AccuracyOfChoEstimate = new DoubleAndText();
        }
    }
}
