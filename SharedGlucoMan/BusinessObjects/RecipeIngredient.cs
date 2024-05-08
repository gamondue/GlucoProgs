using gamon;

namespace GlucoMan
{
    internal class RecipeIngredient
    {
        public int IdIngredient { get; set; }
        public int? IdRecipe { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DoubleAndText QuantityGrams { get; set; }
        public DoubleAndText QuantityPercent { get; set; }
        public DoubleAndText ChoPercent { get; set; }
        public int? IdFood { get; set; }
        public RecipeIngredient()
        {
            QuantityGrams = new DoubleAndText();
            QuantityPercent = new DoubleAndText();
            ChoPercent = new DoubleAndText();
        }
    }
}
