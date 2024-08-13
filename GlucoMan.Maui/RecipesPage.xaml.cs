
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class RecipesPage : ContentPage
{
    BL_Recipes bl = new BL_Recipes();
    public RecipesPage()
    {
        InitializeComponent();

        List<Recipe> list = bl.ReadSomeRecipes(""); 
        if(list.Count == 0)
        {
            // create new recipe
            Recipe recipe = new Recipe();
            recipe.Name = "Torta di mele";
            recipe.Description = "Una torta buonissima";
            recipe.ChoPercent.Double = 50;
            int? index = bl.SaveOneRecipe(recipe);
            // create another recipe
            recipe = new Recipe();
            recipe.Name = "Pizza";
            recipe.Description = "Una pizza buonissima";
            recipe.ChoPercent.Double = 75;
            index = bl.SaveOneRecipe(recipe);
        }
        list = bl.ReadSomeRecipes("");
    }
    private async void btnRecipeDetails_ClickedAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecipePage());
    }
}