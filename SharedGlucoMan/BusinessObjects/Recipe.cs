using gamon;

namespace GlucoMan
{
    public class Recipe
    {
        public int? IdRecipe { get; set; }
        public string? Name { get; set; }
        public DoubleAndText TotalChoPercent { get; set; }
        public Recipe()
        {
            TotalChoPercent = new DoubleAndText();
        }
    }
}
