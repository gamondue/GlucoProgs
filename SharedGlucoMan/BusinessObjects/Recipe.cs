using gamon;

namespace GlucoMan
{
    public class Recipe
    {
        public int IdRecipe { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DoubleAndText ChoPercent { get; set; }
        public Recipe()
        {
            ChoPercent = new DoubleAndText();
        }
    }
}
