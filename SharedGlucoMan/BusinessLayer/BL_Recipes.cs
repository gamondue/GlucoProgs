
namespace GlucoMan.BusinessLayer
{
    internal class BL_Recipes
    {
        DataLayer dl = Common.Database;
        public Recipe Recipe { get; set; }
        public List<Recipe> Recipes { get; set; }
        public Ingredient Ingredient { get; set; }
        public List<Ingredient> Ingredients { get; set; }
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
            double? totalWeight = 0;
            double? recipeCHO = 0;
            double? weightedSumOfAccuracies = 0;
            if (Ingredients != null)
            {
                // sum of weights, weighted sum of recipe's CHO and weighted sum of squared accuracies
                foreach (Ingredient i in Ingredients)
                {
                    totalWeight += i.QuantityGrams.Double;
                    recipeCHO += i.CarbohydratesPercent.Double * i.QuantityGrams.Double / 100;
                    double? weightedAccuracy = i.AccuracyOfChoEstimate.Double * i.QuantityGrams.Double;
                    weightedSumOfAccuracies += weightedAccuracy * weightedAccuracy;
                }
                recipeCHO = recipeCHO / totalWeight;
                double? accuracyOfRecipe = 0;
                if (weightedSumOfAccuracies != null)
                    accuracyOfRecipe = Math.Sqrt((double)weightedSumOfAccuracies) / totalWeight;
                // update the Ingredients' percentages
                foreach (Ingredient i in Ingredients)
                {
                    i.CarbohydratesGrams.Double = i.CarbohydratesPercent.Double * i.QuantityGrams.Double;
                    i.QuantityPercent.Double = i.QuantityGrams.Double / totalWeight * 100;
                }
                Recipe.AccuracyOfChoEstimate.Double = accuracyOfRecipe;
                Recipe.TotalWeight.Double = totalWeight;
            }
            dl.SaveAllIngredientsInARecipe(Ingredients);
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
        public int? SaveOneIngredient(Ingredient Ingredient)
        {
            return dl.SaveOneIngredient(Ingredient);
        }
        public List<Ingredient> ReadAllIngredientsInARecipe(int? idRecipe)
        {
            return dl.ReadAllIngredientsInARecipe(idRecipe);
        }
        internal void SaveAllIngredientsInARecipe(List<Ingredient> Ingredients)
        {
            dl.SaveAllIngredientsInARecipe(Ingredients);
        }
        internal void DeleteOneIngredient(Ingredient Ingredient)
        {
            dl.DeleteOneIngredient(Ingredient);
        }
    }
}
