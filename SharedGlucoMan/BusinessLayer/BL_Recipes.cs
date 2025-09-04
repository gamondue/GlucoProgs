namespace GlucoMan.BusinessLayer
{
    public class BL_Recipes
    {
        DataLayer dl = Common.Database;
        public Recipe CurrentRecipe { get; set; }
        //public List<Recipe> Recipes { get; private set; }
        public Ingredient CurrentIngredient { get; set; }
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
        public List<Recipe> ReadSomeRecipes(string? WhereClause)
        {
            return dl.ReadSomeRecipes(WhereClause);
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
        internal void UpdatePercentages()
        {
            // calculate the total weight ot the recipe
            CurrentRecipe.TotalWeight.Double = 0;
            foreach (Ingredient i in CurrentRecipe.Ingredients)
            {
                // sum of recipe's weights
                if (i.QuantityGrams.Double == null)
                {
                    CurrentRecipe.CarbohydratesPercent.Double = double.NaN;
                }
                else
                {
                    // sum of recipe's weights
                    CurrentRecipe.TotalWeight.Double += i.QuantityGrams.Double;
                }
            }
            // update the percentages
            foreach (Ingredient i in CurrentRecipe.Ingredients)
            {
                if (i.QuantityGrams.Double == null)
                {
                    i.QuantityPercent.Double = double.NaN;
                }
                else
                {
                    i.QuantityPercent.Double = i.QuantityGrams.Double / CurrentRecipe.TotalWeight.Double * 100;
                }
            }
        }
        internal void RecalcAll()
        {
            if (CurrentRecipe.Ingredients != null && CurrentRecipe.Ingredients.Count > 0)
            {
                // calculate the percentages of weights of the ingredients in the recipe
                UpdatePercentages();
                // other survey data
                // TotalWeight is calculated by UpdatePercentages();
                double? sumOfWeightedCho = 0;
                double? sumOfWeightedAccuracies = 0;
                // sum of weights, weighted sum of recipe's CHO and weighted sum of squared accuracies
                foreach (Ingredient i in CurrentRecipe.Ingredients)
                {
                    if (i.CarbohydratesPercent.Double != null && i.AccuracyOfChoEstimate.Double != null)
                    {
                        // sum of recipe's CHO weighted by the weight of the ingredient
                        sumOfWeightedCho += i.CarbohydratesPercent.Double * i.QuantityGrams.Double;
                        // sum of squared accuracies weighted by the weight of the ingredient
                        sumOfWeightedAccuracies += i.AccuracyOfChoEstimate.Double * i.AccuracyOfChoEstimate.Double * i.QuantityGrams.Double;
                    }
                }
                // recipe CHO 
                CurrentRecipe.CarbohydratesPercent.Double = sumOfWeightedCho / CurrentRecipe.TotalWeight.Double;
                // recipe CHO accuracy
                CurrentRecipe.AccuracyOfChoEstimate.Double = Math.Sqrt((double)(sumOfWeightedAccuracies / CurrentRecipe.TotalWeight.Double));
            }
        }
        internal void SaveIngredientParameters()
        {
            throw new NotImplementedException();
        }
        internal void FromFoodToIngredient(Food sourceFood, Ingredient destinationIngredient)
        {
            destinationIngredient.IdFood = sourceFood.IdFood;
            destinationIngredient.Name = sourceFood.Name;
            destinationIngredient.Description = sourceFood.Description;
            destinationIngredient.CarbohydratesPercent = sourceFood.CarbohydratesPercent;
        }
        public int? SaveOneIngredient(Ingredient Ingredient)
        {
            return dl.SaveOneIngredient(Ingredient);
        }
        public void ReadAllIngredientsInThisRecipe()
        {
            CurrentRecipe.Ingredients = dl.ReadAllIngredientsInARecipe(CurrentRecipe.IdRecipe);
        }
        internal void SaveListOfIngredients()
        {
            dl.SaveListOfIngredients(CurrentRecipe.Ingredients);
        }
        internal void DeleteOneIngredient(Ingredient Ingredient)
        {
            dl.DeleteOneIngredient(Ingredient);
        }
        internal void CreateNewListOfIngredientsInRecipe()
        {
            CurrentRecipe.Ingredients = new List<Ingredient>();
        }
    }
}
