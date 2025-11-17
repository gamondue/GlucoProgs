using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;
using Microsoft.Maui.Graphics;

namespace GlucoMan.Maui;

public partial class RecipePage : ContentPage
{
    private BL_Recipes bl; // business layer of recipes

    private UiAccuracy accuracyRecipe;
    private UiAccuracy accuracyIngredient;

    private FoodsPage foodsPage;

    // Flags to prevent recursive updates during text changes
    private bool ingredientModifications = false;
    private bool ingredientPercentOrQuantityChanging = false;
    private bool ingredientChoGramsChanging = false;
    private bool programmaticModification = true;

    public bool RecipeIsChosen { get; internal set; }

    public RecipePage(BL_Recipes BlRecipes)
    {
        InitializeComponent();

        bl = BlRecipes ?? new BL_Recipes();

        // Ensure Recipe and Ingredient are initialized
        bl.Recipe ??= new Recipe();
        bl.Ingredient ??= new Ingredient();

        // Pickers for qualitative accuracy
        cmbAccuracyRecipe.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        cmbAccuracyIngredient.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

        // Helper for tinting entries based on accuracy and keeping combo/text in sync visually
        accuracyRecipe = new UiAccuracy(txtAccuracyOfChoRecipe, cmbAccuracyRecipe);
        accuracyIngredient = new UiAccuracy(txtAccuracyOfChoIngredient, cmbAccuracyIngredient);

        // Bind sections once; controls use TwoWay bindings in XAML
        recipeSection.BindingContext = bl.Recipe;
        //ingredientSection.BindingContext = bl.Ingredient;

        // calculate non database values 
        // First load ingredients from DB into bl.Recipe, then recalc totals
        RefreshGrid();
        bl.RecalcAll();
        // ensure recipe section reflects recalculated totals
        try
        {
            recipeSection.BindingContext = null;
            recipeSection.BindingContext = bl.Recipe;
        }
        catch { }
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        // XAML bindings are active
    }

    private void RefreshGrid()
    {
        try
        {
            gridIngredients.BindingContext = null;
            gridIngredients.BindingContext = bl.GetAllIngredientsInThisRecipe();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("RecipePage - RefreshGrid", ex);
        }
    }

    private void CalculateChoOfIngredientGrams()
    {
        if (bl.Ingredient.CarbohydratesPercent.Double != null && bl.Ingredient.QuantityGrams.Double != null)
        {
            bl.Ingredient.CarbohydratesGrams.Double =
                bl.Ingredient.CarbohydratesPercent.Double / 100 * bl.Ingredient.QuantityGrams.Double;
        }
    }

    private async void btnSaveAllRecipe_Click(object sender, EventArgs e)
    {
        try
        {
            // Data already pushed to model via TwoWay bindings
            bl.Recipe.IdRecipe = bl.SaveOneRecipe(bl.Recipe);

            // Persist ingredients
            bl.Ingredient.IdRecipe = bl.Recipe.IdRecipe;
            bl.UpdateOldIngredientInList();
            bl.SaveAllIngredientsInRecipe();

            bl.RecalcAll();
            RefreshGrid();
            await RefreshAccuracyControls();

            General.LogOfProgram?.Event("Recipe saved successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - btnSaveAllRecipe_Click", ex);
            await DisplayAlert("Error", "Failed to save Recipe data. This might be due to database connectivity issues. Check logs for details.", "OK");
        }
    }

    private void btnAddIngredient_Click(object sender, EventArgs e)
    {
        // Ensure recipe exists in DB
        if (bl.Recipe.IdRecipe == null)
            bl.Recipe.IdRecipe = bl.SaveOneRecipe(bl.Recipe);

        // Current Ingredient is filled via TwoWay binding
        bl.Ingredient.IdIngredient = null;
        bl.Ingredient.IdRecipe = bl.Recipe.IdRecipe;

        var savedId = bl.SaveOneIngredient(bl.Ingredient);
        if (savedId != null)
        {
            bl.Ingredient.IdIngredient = savedId;

            bl.Recipe.Ingredients ??= new List<Ingredient>();
            if (!bl.Recipe.Ingredients.Any(i => i.IdIngredient == savedId))
            {
                // store a copy in the list (so editing the current field keeps working)
                bl.Recipe.Ingredients.Add(new Ingredient
                {
                    IdIngredient = bl.Ingredient.IdIngredient,
                    IdRecipe = bl.Ingredient.IdRecipe,
                    Name = bl.Ingredient.Name,
                    CarbohydratesPercent = bl.Ingredient.CarbohydratesPercent,
                    CarbohydratesGrams = bl.Ingredient.CarbohydratesGrams,
                    QuantityGrams = bl.Ingredient.QuantityGrams,
                    QuantityPercent = bl.Ingredient.QuantityPercent,
                    AccuracyOfChoEstimate = bl.Ingredient.AccuracyOfChoEstimate
                });
            }

            bl.RecalcAll();
            RefreshGrid();
        }
        else
        {
            DisplayAlert("Error", "Failed to add ingredient to recipe", "OK");
        }
    }

    private void btnRemoveIngredient_Click(object sender, EventArgs e)
    {
        try
        {
            if (bl.Ingredient?.IdIngredient != null)
            {
                bl.DeleteOneIngredient(bl.Ingredient);

                if (bl.Recipe.Ingredients != null)
                    bl.Recipe.Ingredients.RemoveAll(i => i.IdIngredient == bl.Ingredient.IdIngredient);

                bl.RecalcAll();
                RefreshGrid();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("RecipePage - btnRemoveIngredient_Click", ex);
        }
    }

    private async void btnFoods_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            if (txtIngredientName.Text == null || txtIngredientName.Text == "")
            {
                bl.Ingredient = new Ingredient();
            }
            else
            {
                FromBoxesIngredientToClass();
            }
            foodsPage = new FoodsPage(bl.Ingredient);
            await Navigation.PushModalAsync(foodsPage);
            // Wait for the page to be closed and get the result
            bool foodWasChosen = await foodsPage.PageClosedTask;
            if (foodWasChosen && foodsPage.FoodIsChosen)
            {
                bool newFoodIsDifferent = foodsPage.Food.IdFood != bl.Ingredient.IdFood;
                // Update the current FoodInMeal with the Food chosen from the called page
                bl.FromFoodToIngredient(foodsPage.Food, bl.Ingredient);
                // check if this ingredient is the same that is coming from the Food page
                if (newFoodIsDifferent)
                {
                    // the chosen food is different, 
                    bl.Ingredient.QuantityGrams.Double = 0;
                }
                //ingredientSection.BindingContext = bl.Ingredient; // ensure UI shows updated ingredient
                bl.UpdateOldIngredientInList();
                bl.RecalcAll();
                FromClassToBoxesIngredient();
                RefreshGrid();
                await RefreshAccuracyControls();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - btnFoods_ClickAsync", ex);
            await DisplayAlert("Error", $"Failed to import food: {ex.Message}", "OK");
        }
    }

    private async void btnWeighFood_Click(object sender, EventArgs e)
    {
        try
        {
            // Ensure Ingredient is initialized
            bl.Ingredient ??= new Ingredient();
            
            // Update ingredient from current UI values
            FromBoxesIngredientToClass();
            
            // Create WeighFoodPage with current ingredient data
            var weighFoodPage = new WeighFoodPage(bl.Ingredient);
            await Navigation.PushModalAsync(weighFoodPage);

            // Wait for the page to be closed and get the result
            bool dataWasModified = await weighFoodPage.PageClosedTask;
 
            if (dataWasModified && weighFoodPage.ResultFood != null)
         {
           // Update ingredient from result food
      bl.FromFoodToIngredient(weighFoodPage.ResultFood, bl.Ingredient);

   // Determine which weight value to use for QuantityGrams
           double weightToUse = 0;
    
   if (weighFoodPage.WeightOfPortion > 0)
{
            // Priority 1: Use WeightOfPortion if available
 weightToUse = weighFoodPage.WeightOfPortion;
     General.LogOfProgram?.Event($"RecipePage - Using WeightOfPortion: {weightToUse:F1}g");
        }
     else
     {
                  // Priority 2: Use TxtRawNet (raw net weight) as fallback
        // Access the business layer data that was saved
     var blFood = new BL_WeighFood();
         blFood.RestoreData();
               
        if (blFood.RawNet?.Double != null && blFood.RawNet.Double > 0)
  {
            weightToUse = blFood.RawNet.Double.Value;
       General.LogOfProgram?.Event($"RecipePage - Using RawNet weight: {weightToUse:F1}g");
        }
     else
        {
       General.LogOfProgram?.Event("RecipePage - No weight data available from WeighFoodPage");
         }
   }

       // Apply the weight to QuantityGrams
                if (weightToUse > 0)
    {
          bl.Ingredient.QuantityGrams ??= new DoubleAndText();
    bl.Ingredient.QuantityGrams.Double = weightToUse;
     bl.Ingredient.QuantityGrams.Text = weightToUse.ToString("F1");
    General.LogOfProgram?.Event($"RecipePage - Ingredient quantity set to {weightToUse:F1}g from WeighFoodPage");
   }

     // Recalculate carbohydrates in grams
       CalculateChoOfIngredientGrams();
     
             // Update the user interface with the new data
 FromClassToBoxesIngredient();
          
        // Update the ingredient in the list
              bl.UpdateOldIngredientInList();
        
  // Recalculate all values
       bl.RecalcAll();

                // Refresh the grid
      RefreshGrid();
                
           // Update accuracy controls
        await RefreshAccuracyControls();
         
             General.LogOfProgram?.Event("RecipePage - Ingredient data updated from WeighFoodPage successfully");
    }
        }
   catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - btnWeighFood_Click", ex);
            await DisplayAlert("Error", $"Failed to open weigh food page: {ex.Message}\n\nCheck logs for details.", "OK");
        }
    }

    private async void btnDefaults_Click(object sender, EventArgs e)
    {
        try
        {
            bl.Ingredient = new Ingredient();
            //ingredientSection.BindingContext = bl.Ingredient; // rebind to fresh ingredient
            await RefreshAccuracyControls();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("RecipePage - btnDefaults_Click", ex);
        }
    }

    private void btnCalc_Click(object sender, EventArgs e)
    {
        try
        {
            // Update the ingredient in the list before switching
            FromBoxesIngredientToClass();
            bl.UpdateOldIngredientInList();
            // calc riepilogative data
            bl.RecalcAll();
            FromClassToBoxesIngredient();
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - btnCalc_Click", ex);
            DisplayAlert("Error", "Failed to calculate recipe totals. Check logs for details.", "OK");
        }
    }

    private void ingredientsSection_Unfocused(object sender, FocusEventArgs e)
    {
        // No manual sync/save here. TwoWay bindings keep model updated.
    }

    private async Task FromClassesToUi()
    {
        // Keep bindings; only refresh the accuracy visuals
        await RefreshAccuracyControls();
    }

    private async void gridIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }
        try
        {
            var selectedIngredient = (Ingredient)e.SelectedItem;

            if (selectedIngredient != bl.Ingredient)
            {
                if (bl.Ingredient?.Name != null)
                {
                    // Update the ingredient in the list before switching
                    FromBoxesIngredientToClass();
                    bl.UpdateOldIngredientInList();
                    RefreshGrid();
                }
                // after refresh the selected ingredient becomes the current ingredient 
                bl.Ingredient = selectedIngredient;
                FromClassToBoxesIngredient();
            }
            if (gridIngredients.SelectedItem != selectedIngredient)
            {
                gridIngredients.SelectedItem = selectedIngredient;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("RecipePage - gridIngredients_ItemSelected", ex);
        }
    }

    private void FromClassToBoxesIngredient()
    {
        ingredientModifications = true;

        txtIngredientName.Text = bl.Ingredient.Name;
        // unformatted visulization for user's modifications
        txtAccuracyOfChoIngredient.Text = Convert.ToDouble(bl.Ingredient.AccuracyOfChoEstimate.Double).ToString();
        txtIngredientQuantityGrams.Text = Convert.ToDouble(bl.Ingredient.QuantityGrams.Double).ToString();
        txtIngredientCarbohydratesPercent.Text = Convert.ToDouble(bl.Ingredient.CarbohydratesPercent.Double).ToString();
        // formatted visualization for program's modifications
        txtIngredientQuantityPercent.Text = bl.Ingredient.QuantityPercent.Text;

        ingredientModifications = false;
    }

    private void FromBoxesIngredientToClass()
    {
        //bl.Ingredient.IdIngredient = Safe.Int(txtIdIngredient.Text);
        bl.Ingredient.Name = Safe.String(txtIngredientName.Text);
        bl.Ingredient.QuantityGrams.Text = txtIngredientQuantityGrams.Text;
        bl.Ingredient.QuantityPercent.Text = txtIngredientQuantityPercent.Text;
        bl.Ingredient.CarbohydratesPercent.Text = txtIngredientCarbohydratesPercent.Text;
        bl.Ingredient.AccuracyOfChoEstimate.Text = txtAccuracyOfChoIngredient.Text;
        CalculateChoOfIngredientGrams();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (foodsPage != null && foodsPage.FoodIsChosen)
        {
            bl.FromFoodToIngredient(foodsPage.Food, bl.Ingredient);
            await FromClassesToUi();
        }
    }

    // Accuracy refresh helpers
    private async Task RefreshAccuracyControls()
    {
        try
        {
            await Task.Delay(50); // allow bindings to settle
            await RefreshRecipeAccuracyControls();
            await RefreshIngredientAccuracyControls();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - RefreshAccuracyControls", ex);
        }
    }

    private async Task RefreshRecipeAccuracyControls()
    {
        try
        {
            if (accuracyRecipe != null && !string.IsNullOrEmpty(txtAccuracyOfChoRecipe.Text))
            {
                if (double.TryParse(txtAccuracyOfChoRecipe.Text, out double recipeAccuracy))
                {
                    var qualitativeAccuracy = accuracyRecipe.GetQualitativeAccuracyGivenQuantitavive(recipeAccuracy);
                    cmbAccuracyRecipe.SelectedItem = qualitativeAccuracy;
                    txtAccuracyOfChoRecipe.BackgroundColor = accuracyRecipe.AccuracyBackColor(recipeAccuracy);
                    txtAccuracyOfChoRecipe.TextColor = accuracyRecipe.AccuracyForeColor(recipeAccuracy);
                }
            }
            else
            {
                cmbAccuracyRecipe.SelectedItem = null;
                txtAccuracyOfChoRecipe.BackgroundColor = Colors.LightGreen;
                txtAccuracyOfChoRecipe.TextColor = Colors.Black;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - RefreshRecipeAccuracyControls", ex);
        }
    }

    private async Task RefreshIngredientAccuracyControls()
    {
        try
        {
            if (accuracyIngredient != null && !string.IsNullOrEmpty(txtAccuracyOfChoIngredient.Text))
            {
                if (double.TryParse(txtAccuracyOfChoIngredient.Text, out double ingredientAccuracy))
                {
                    var qualitativeAccuracy = accuracyIngredient.GetQualitativeAccuracyGivenQuantitavive(ingredientAccuracy);
                    cmbAccuracyIngredient.SelectedItem = qualitativeAccuracy;
                    txtAccuracyOfChoIngredient.BackgroundColor = accuracyIngredient.AccuracyBackColor(ingredientAccuracy);
                    txtAccuracyOfChoIngredient.TextColor = accuracyIngredient.AccuracyForeColor(ingredientAccuracy);
                }
            }
            else
            {
                cmbAccuracyIngredient.SelectedItem = null;
                txtAccuracyOfChoIngredient.BackgroundColor = Colors.LightGreen;
                txtAccuracyOfChoIngredient.TextColor = Colors.Black;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - RefreshIngredientAccuracyControls", ex);
        }
    }
    private void txtIngredientCarbohydratesGrams_Unfocused(object sender, FocusEventArgs e)
    {
        //////////try
        //////////{
        //////////    // Do heavy operations: recalculate all percentages and totals
        //////////    bl.RecalcAll();

        //////////    // Update only the calculated read-only field manually
        //////////    if (bl.Ingredient?.QuantityPercent?.Text != null)
        //////////    {
        //////////        txtIngredientQuantityPercent.Text = bl.Ingredient.QuantityPercent.Text;
        //////////    }

        //////////    // Refresh the grid to show updated values
        //////////    RefreshGrid();
        //////////}
        //////////catch (Exception ex)
        //////////{
        //////////    General.LogOfProgram?.Error("RecipePage - txtIngredientCarbohydratesGrams_Unfocused", ex);
        //////////}
    }

    private void txtAccuracyOfChoRecipe_TextChanged(object sender, TextChangedEventArgs e) { }
    private void txtAccuracyOfChoRecipe_Unfocused(object sender, FocusEventArgs e) { }
    private void cmbAccuracyRecipe_SelectedIndexChanged(object sender, EventArgs e) { }

    private async void Calculator_Click(object sender, TappedEventArgs e)
    {
        try
        {
            var focusedEntry = GetFocusedEntry();
            string sValue = focusedEntry?.Text ?? "0";
            double dValue = double.TryParse(sValue, out var val) ? val : 0;

            // Start the CalculatorPage passing to it the value
            // of the control that currently has the focus
            var calculator = new CalculatorPage(dValue);
            await Navigation.PushModalAsync(calculator);
            var result = await calculator.ResultSource.Task;

            // Check if the page has given back a result
            if (result.HasValue && focusedEntry != null)
            {
                // Update the focused entry with the calculator result
                focusedEntry.Text = result.Value.ToString("F1");

                General.LogOfProgram?.Event($"WeighFoodPage - Calculator result {result.Value:F1} applied to {focusedEntry.StyleId ?? "entry"}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Calculator_Click", ex);
            await DisplayAlert("Error", "Failed to open calculator. Check logs for details.", "OK");
        }
    }

    private Entry GetFocusedEntry()
    {
        if (txtIngredientQuantityGrams != null && txtIngredientQuantityGrams.IsFocused) return txtIngredientQuantityGrams;
        if (txtIngredientCarbohydratesPercent != null && txtIngredientCarbohydratesPercent.IsFocused) return txtIngredientCarbohydratesPercent;
        return null;
    }
    private async void btnRecipes_Click(object sender, EventArgs e)
    {
        try
        {
            // Ensure Ingredient is initialized
            if (bl.Ingredient == null)
            {
                bl.Ingredient = new Ingredient();
            }

            // Open RecipesPage to choose a recipe
            var recipesPage = new RecipesPage((string)null, (string)null);
            await Navigation.PushAsync(recipesPage);

            // Wait for the page to be closed and get the result
            bool recipeWasChosen = await recipesPage.PageClosedTask;

            // Check if the user chose a recipe in called page
            if (recipeWasChosen && recipesPage.RecipeIsChosen && recipesPage.CurrentRecipe != null)
            {
                // Update the current Ingredient with the Recipe data
                bl.Ingredient.Name = recipesPage.CurrentRecipe.Name;
                bl.Ingredient.Description = recipesPage.CurrentRecipe.Description;

                // Import CHO% from recipe to ingredient
                if (recipesPage.CurrentRecipe.CarbohydratesPercent != null &&
                           recipesPage.CurrentRecipe.CarbohydratesPercent.Double.HasValue)
                {
                    bl.Ingredient.CarbohydratesPercent ??= new DoubleAndText();
                    bl.Ingredient.CarbohydratesPercent.Double = recipesPage.CurrentRecipe.CarbohydratesPercent.Double;
                }

                // Initialize QuantityGrams to 0 for a new recipe
                bl.Ingredient.QuantityGrams ??= new DoubleAndText();
                bl.Ingredient.QuantityGrams.Double = 0;

                // Set unit to grams (recipes are measured in grams)
                // Note: Ingredient doesn't have UnitSymbol/GramsInOneUnit like FoodInMeal
                // but we can document this behavior

                // Recalculate the carbohydrates in grams
                CalculateChoOfIngredientGrams();

                // Update the user interface with the new data
                FromClassToBoxesIngredient();

                // Update the ingredient in the list
                bl.UpdateOldIngredientInList();

                // Recalculate all values
                bl.RecalcAll();

                // Refresh the grid
                RefreshGrid();

                // Update accuracy controls
                await RefreshAccuracyControls();

                General.LogOfProgram?.Event($"Recipe imported: Name={recipesPage.CurrentRecipe.Name}, CHO%={recipesPage.CurrentRecipe.CarbohydratesPercent?.Double ?? 0}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - btnRecipes_Click", ex);
            await DisplayAlert("Error", $"Failed to import recipe: {ex.Message}", "OK");
        }
    }
}
