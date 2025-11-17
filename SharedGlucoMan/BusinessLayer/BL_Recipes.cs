using gamon;

namespace GlucoMan.BusinessLayer
{
    public class BL_Recipes
    {
        DataLayer dl = Common.Database;
        public Recipe Recipe { get; set; }
        //public List<Recipe> Recipes { get; private set; }
        public Ingredient Ingredient { get; set; }
        //public List<Ingredient> AllIngredientsOfCurrentRecipe { get; private set; }
        internal Recipe GetOneRecipe(int? idRecipe)
        {
            return dl.GetOneRecipe(idRecipe);
        }
        public int? SaveOneRecipe(Recipe recipe)
        {
            int? key = dl.SaveOneRecipe(recipe);
            if (recipe.Ingredients == null || recipe.Ingredients.Count == 0)
                return key;
            // save all ingredients
            foreach (Ingredient i in recipe.Ingredients)
            {
                i.IdRecipe = recipe.IdRecipe;
                SaveOneIngredient(i);
            }
            return key;
        }
        public List<Recipe> GetSomeRecipes(string? WhereClause)
        {
            return dl.GetSomeRecipes(WhereClause);
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
        internal void CalculatePercentagesAndChoGramsOfIngredients()
        {
            // calculate the total weight ot the recipe
            Recipe.TotalWeight.Double = 0;
            foreach (Ingredient i in Recipe.Ingredients)
            {
                // sum of recipe's weights
                if (i.QuantityGrams.Double == null)
                {
                    // neglect null values
                    //Recipe.CarbohydratesPercent.Double = double.NaN;
                }
                else
                {
                    // sum of recipe's weights
                    Recipe.TotalWeight.Double += i.QuantityGrams.Double;
                }
            }
            // update the percentages and 
            foreach (Ingredient i in Recipe.Ingredients)
            {
                if (i.QuantityGrams.Double == null)
                {
                    // neglect null values
                    i.QuantityPercent.Double = double.NaN;
                    i.CarbohydratesGrams.Double = double.NaN;
                }
                else
                {
                    i.QuantityPercent.Double = i.QuantityGrams.Double / Recipe.TotalWeight.Double * 100;
                    i.CarbohydratesGrams.Double = i.CarbohydratesPercent.Double * i.QuantityGrams.Double / 100;
                }
            }
        }
        internal void RecalcAll()
        {
            if (Recipe.Ingredients != null && Recipe.Ingredients.Count > 0)
            {
                // update the ingredients for concurrent changes
                GetAllIngredientsInThisRecipe();
                // calculate the percentages of weights of the ingredients in the recipe
                CalculatePercentagesAndChoGramsOfIngredients();
                // save the new percentages
                SaveAllIngredientsInRecipe();
                // other survey data
                // TotalWeight is already calculated by CalculatePercentagesAndChoGramsOfIngredients();
                double? sumOfChoGrams = 0;
                double? sumOfWeightedAccuracies = 0;
                // sum of weights, weighted sum of recipe's CHO and weighted sum of squared accuracies
                foreach (Ingredient i in Recipe.Ingredients)
                {
                    if (i.CarbohydratesPercent.Double != null && i.AccuracyOfChoEstimate.Double != null)
                    {
                        // sum of carbohydrates weights
                        sumOfChoGrams += i.CarbohydratesGrams.Double;
                        // sum of squared accuracies weighted by the weight of the ingredient
                        sumOfWeightedAccuracies += i.AccuracyOfChoEstimate.Double * i.AccuracyOfChoEstimate.Double * i.QuantityGrams.Double;
                    }
                }
                // recipe CHO 
                // sumOfChoGrams is in grams; to get percent (g carbs per100 g recipe) multiply fraction by100
                if (Recipe.TotalWeight.Double != null && Recipe.TotalWeight.Double != 0)
                {
                    Recipe.CarbohydratesPercent.Double = (sumOfChoGrams / Recipe.TotalWeight.Double) * 100;
                }
                else
                {
                    Recipe.CarbohydratesPercent.Double = null;
                }
                // recipe CHO accuracy
                Recipe.AccuracyOfChoEstimate.Double = Math.Sqrt((double)(sumOfWeightedAccuracies / Recipe.TotalWeight.Double));
            }
        }
        internal void FromFoodToIngredient(Food sourceFood, Ingredient destinationIngredient)
        {
            if (destinationIngredient != null && sourceFood != null)
            {
                destinationIngredient.IdFood = sourceFood.IdFood;
                destinationIngredient.Name = sourceFood.Name;
                destinationIngredient.Description = sourceFood.Description;
                
                if (destinationIngredient.CarbohydratesPercent == null)
                    destinationIngredient.CarbohydratesPercent = new DoubleAndText();
                
                if (sourceFood.CarbohydratesPercent != null)
                {
                    destinationIngredient.CarbohydratesPercent.Double = sourceFood.CarbohydratesPercent.Double;
                }
            }
        }
        public int? SaveOneIngredient(Ingredient Ingredient)
        {
            return dl.SaveOneIngredient(Ingredient);
        }
        internal void SaveAllIngredientsInRecipe()
        {
            if (Recipe.Ingredients != null)
                foreach (Ingredient ingredient in Recipe.Ingredients)
                {
                    dl.SaveOneIngredient(ingredient);
                }
        }
        public List<Ingredient> GetAllIngredientsInThisRecipe()
        {
            Recipe.Ingredients = dl.GetAllIngredientsInARecipe(Recipe.IdRecipe);
            return (Recipe.Ingredients);
        }
        internal void SaveListOfIngredients()
        {
            dl.SaveListOfIngredients(Recipe.Ingredients);
        }
        internal void DeleteOneIngredient(Ingredient Ingredient)
        {
            dl.DeleteOneIngredient(Ingredient);
        }
        internal void CreateNewListOfIngredientsInRecipe()
        {
            Recipe.Ingredients = new List<Ingredient>();
        }
        internal void UpdateOldIngredientInList()
        {
            // if the current Ingredient has not a name, it doesn't deserve to be updated, hence saved
            if (Ingredient.Name == null || Ingredient.Name == "")
                return;
            dl.SaveOneIngredient(Ingredient);
            return;
        }
    }
}
