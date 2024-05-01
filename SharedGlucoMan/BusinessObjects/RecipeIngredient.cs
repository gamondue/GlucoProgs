using gamon;

namespace GlucoMan
{
    internal class RecipeIngredient
    {
        public int IdIngredient { get; set; }
        public int IdFood { get; set; }
        public string? Name { get; set; }
        public DoubleAndText WeightInRecipe { get; set; }
        public DoubleAndText WeightPercent { get; set; }
        public DoubleAndText ChoPercent { get; set; }
        public RecipeIngredient()
        {
            WeightInRecipe = new DoubleAndText();
            WeightPercent = new DoubleAndText();
            ChoPercent = new DoubleAndText();
        }
    }
}
