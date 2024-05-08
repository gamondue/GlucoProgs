using System.Collections.Generic;

namespace GlucoMan.BusinessLayer
{
    internal class BL_Recipes
    {
        DataLayer dl = Common.Database;
        public int? SaveOneRecipe(Recipe recipe)
        {
            return dl.SaveOneRecipe(recipe);
        }
        public List<Recipe> ReadSomeRecipes(string? whereClause)
        {
            return dl.ReadSomeRecipes(whereClause);
        }
        public int? SaveOneRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            return dl.SaveOneRecipeIngredient(recipeIngredient);
        }
        public List<RecipeIngredient> ReadIngredientsOfARecipe(int idRecipe)
        {
            return dl.ReadAllIngredientsOfARecipe(idRecipe);
        }
    }
}
