using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class RecipePage : ContentPage
{
    BL_Recipes bl = new BL_Recipes();
    public RecipePage()
	{
		InitializeComponent();

		List<RecipeIngredient> ingredients = bl.ReadIngredientsOfARecipe(1);
        if (ingredients.Count == 0)
        {
            RecipeIngredient ingredient = new RecipeIngredient();
            ingredient.Name = "Zucchero";
            ingredient.QuantityGrams.Double = 10;
            ingredient.IdRecipe = 1;
            ingredient.QuantityGrams.Double = 10;
            ingredient.ChoPercent.Double = 100;
            ingredient.IdFood = 1;
            int? index = bl.SaveOneRecipeIngredient(ingredient);
            // create another recipe ingredient for the first recipe
            ingredient = new RecipeIngredient();
            ingredient.Name = "Mele";
            ingredient.IdRecipe = 1;
            ingredient.QuantityGrams.Double = 500;
            ingredient.ChoPercent.Double = 15;
            ingredient.IdFood = 2;
            index = bl.SaveOneRecipeIngredient(ingredient);

            // create a recipe ingredient for the second recipe
            ingredient = new RecipeIngredient();
            ingredient.Name = "Farina tipo 0";
            ingredient.QuantityGrams.Double = 100;
            ingredient.IdRecipe = 2;
            ingredient.QuantityGrams.Double = 70;
            ingredient.ChoPercent.Double = 80;
            ingredient.IdFood = 3;
            index = bl.SaveOneRecipeIngredient(ingredient);
            // create another recipe ingredient for the second recipe
            ingredient = new RecipeIngredient();
            ingredient.Name = "Mozzarella";
            ingredient.QuantityGrams.Double = 100;
            ingredient.IdRecipe = 2;
            ingredient.QuantityPercent.Double = 30;
            ingredient.ChoPercent.Double = 0;
            ingredient.IdFood = 4;
            index = bl.SaveOneRecipeIngredient(ingredient);
        }
        ingredients = bl.ReadIngredientsOfARecipe(1);
    }
}