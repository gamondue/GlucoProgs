using GlucoMan;
using GlucoMan.BusinessLayer;

namespace NUnitTests
{
    public class Tests
    {
        BL_Recipes bl;
        [SetUp]
        public void Setup()
        {
            Common.SetGlobalParameters();
            Common.GeneralInitializationsAsync(); // from CommonFunctions.cs
            bl = new();
        }

        [Test]
        public void CreateRowsOfRecipesAndRecipesIngredients()
        {
            // create new recipe
            Meal recipe = new Recipe();
            recipe.Name = "Torta di mele";
            recipe.Description = "Una torta buonissima";
            recipe.CarbohydratesPercent.Double = 50;
            int index = (int)bl.SaveOneRecipe(recipe);
            Assert.That(index, Is.EqualTo(1));
            // create another recipe
            recipe = new Recipe();
            recipe.Name = "Pizza";
            recipe.Description = "Una pizza buonissima";
            recipe.CarbohydratesPercent.Double = 75;
            index = (int)bl.SaveOneRecipe(recipe);
            Assert.That(index, Is.EqualTo(2));

            // create a recipe ingredient for the first recipe
            Ingredient ingredient = new Ingredient();
            ingredient.Name = "Zucchero";
            ingredient.QuantityGrams.Double = 10;
            ingredient.IdRecipe = 1;
            ingredient.QuantityGrams.Double = 10;
            ingredient.CarbohydratesPercent.Double = 100;
            ingredient.IdFood = 1;
            index = (int)bl.SaveOneIngredient(ingredient);
            Assert.That(index, Is.EqualTo(0));
            // create another recipe ingredient for the first recipe
            ingredient = new Ingredient();
            ingredient.Name = "Mele";
            ingredient.IdRecipe = 1;
            ingredient.QuantityGrams.Double = 500;
            ingredient.CarbohydratesPercent.Double = 15;
            ingredient.IdFood = 2;
            index = (int)bl.SaveOneIngredient(ingredient);
            Assert.That(index, Is.EqualTo(0));

            // create a recipe ingredient for the second recipe
            ingredient = new Ingredient();
            ingredient.Name = "Farina tipo 0";
            ingredient.QuantityGrams.Double = 100;
            ingredient.IdRecipe = 2;
            ingredient.QuantityGrams.Double = 70;
            ingredient.CarbohydratesPercent.Double = 80;
            ingredient.IdFood = 3;
            // create another recipe ingredient for the second recipe
            ingredient = new Ingredient();
            ingredient.Name = "Mozzarella";
            ingredient.QuantityGrams.Double = 100;
            ingredient.IdRecipe = 2;
            ingredient.QuantityPercent.Double = 30;
            ingredient.CarbohydratesPercent.Double = 0;
            ingredient.IdFood = 4;
        }
        [Test]
        public void ChangeRowsOfRecipesAndRecipesIngredients()
        {
            // change the first recipe
            Meal recipe = new Recipe();
            recipe.IdRecipe = 1;
            recipe.Name = "Torta di mele";
            recipe.Description = "Una torta buonissima";
            recipe.CarbohydratesPercent.Double = 50;
            int index = (int)bl.SaveOneRecipe(recipe);
            Assert.That(index, Is.EqualTo(1));
            // change the second recipe
            recipe = new Recipe();
            recipe.IdRecipe = 2;
            recipe.Name = "Pizza";
            recipe.Description = "Una pizza buonissima";
            recipe.CarbohydratesPercent.Double = 75;
            index = (int)bl.SaveOneRecipe(recipe);
            Assert.That(index, Is.EqualTo(2));

            // change the first recipe ingredient
            Ingredient ingredient = new Ingredient();
            ingredient.IdIngredient = 1;
            ingredient.Name = "Zucchero";
            ingredient.QuantityGrams.Double = 10;
            ingredient.IdRecipe = 1;
            ingredient.QuantityGrams.Double = 10;
            ingredient.CarbohydratesPercent.Double = 100;
            ingredient.IdFood = 1;
            index = (int)bl.SaveOneIngredient(ingredient);
            Assert.That(index, Is.EqualTo(0));
            // change the second recipe ingredient
            ingredient = new Ingredient();
            ingredient.IdIngredient = 2;
            ingredient.Name = "Mele";
            ingredient.IdRecipe = 1;
            ingredient.QuantityGrams.Double = 500;
            ingredient.CarbohydratesPercent.Double = 15;
            ingredient.IdFood = 2;
            index = (int)bl.SaveOneIngredient(ingredient);
            Assert.That(index, Is.EqualTo(0));

            // change the third recipe ingredient
            ingredient = new Ingredient();
            ingredient.IdIngredient = 3;
            ingredient.Name = "Farina tipo 0";
            ingredient.QuantityGrams.Double = 100;
            ingredient.IdRecipe = 2;
            ingredient.QuantityGrams.Double = 70;
            ingredient.CarbohydratesPercent.Double = 80;
            ingredient.IdFood = 3;
            // change the fourth recipe ingredient
            ingredient = new Ingredient();
            ingredient.IdIngredient = 4;
            ingredient.Name = "Mozzarella";
            ingredient.QuantityGrams.Double = 100;
            ingredient.IdRecipe = 2;
            ingredient.QuantityPercent.Double = 30;
            ingredient.CarbohydratesPercent.Double = 0;
            ingredient.IdFood = 4;
        }
    }
}