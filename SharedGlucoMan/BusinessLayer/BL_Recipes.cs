
namespace GlucoMan.BusinessLayer
{
    internal class BL_Recipes
    {
        DataLayer dl = Common.Database;
        public List<Recipe> Recipes { get; set; }
        public Recipe Recipe { get; set; }

        public List<Ingredient> Ingredients { get; set; }
        public Ingredient Ingredient { get; set; }
        internal Recipe GetOneRecipe(int? idRecipe)
        {
            return dl.GetOneRecipe(idRecipe);
        }
        public int? SaveOneRecipe(Recipe recipe)
        {
            return dl.SaveOneRecipe(recipe);
        }
        public List<Recipe> ReadSomeRecipes(string? whereClause)
        {
            return dl.ReadSomeRecipes(whereClause);
        }
        public int? SaveOneIngredient(Ingredient Ingredient)
        {
            return dl.SaveOneIngredient(Ingredient);
        }
        public List<Ingredient> ReadIngredientsOfARecipe(int? idRecipe)
        {
            return dl.ReadAllIngredientsOfARecipe(idRecipe);
        }
        internal List<Recipe> SearchRecipes(string Name, string Description, int MinNoOfCharacters)
        {
            if ((Name != null && Name != "" && Name.Length >= MinNoOfCharacters) ||
                (Description != null && Description != "" && Description.Length >= MinNoOfCharacters))
            {
                // trim the strings from blanks coming from cut and paste 
                if (Name != null)
                    Name = Name.Trim();
                if (Description != null)
                    Description = Description.Trim();
                return dl.SearchRecipes(Name, Description);
            }
            // just if both are null: find all the list of Recipes 
            else if (Name == "" && Description == "")
                return dl.SearchRecipes(Name, Description);
            else
                return null;
        }
        internal void DeleteOneRecipe(Recipe Recipe)
        {
            dl.DeleteOneRecipe(Recipe);
        }
        internal void RecalcAll()
        {
            ////////throw new NotImplementedException();
        }
        internal void SaveAllIngredientsInRecipe()
        {
            throw new NotImplementedException();
        }
        internal void SaveRecipeParameters()
        {
            // throw new NotImplementedException();
        }
        internal void SaveIngredientParameters()
        {
            throw new NotImplementedException();
        }
        internal void FromFoodToIngredient(Food currentFood, Ingredient Ingredient)
        {
            throw new NotImplementedException();
        }
    }
}
