using System.Collections.Generic;

namespace GlucoMan.BusinessLayer
{
    internal class BL_Recipes
    {
        DataLayer dl = Common.Database;
        public void SaveOneRecipe(Recipe recipe)
        {
            dl.SaveOneRecipe(recipe);
        }
        public List<Recipe> ReadSomeRecipes(string? WhereClause)
        {
            return dl.ReadSomeRecipes(WhereClause);
        }
    }
}
