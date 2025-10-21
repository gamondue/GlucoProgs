using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class RecipePage : ContentPage
{
    BL_Recipes bl; // business layer of recipes

    private UiAccuracy accuracyRecipe;
    private UiAccuracy accuracyIngredient;

    private FoodsPage foodsPage;

    // List for storing the ingredients in this meal
    private List<Ingredient> ingredientsInRecipe;

    private bool ingredientModifications = false;
    private bool ingredientPercentOrQuantityChanging = false;
    private bool ingredientChoGramsChanging = false;

    public bool RecipeIsChosen { get; internal set; }

    public RecipePage(BL_Recipes BlRecipes)
    {
        InitializeComponent();

        bl = BlRecipes;
        if (bl == null)
        {
            bl = new BL_Recipes();
        }
        
        // Ensure Recipe is initialized
        if (bl.Recipe == null)
        {
  bl.Recipe = new Recipe();
      General.LogOfProgram?.Event("RecipePage - Initialized new Recipe");
  }
    
   // Ensure Ingredient is initialized
     if (bl.Ingredient == null)
    {
bl.Ingredient = new Ingredient();
 General.LogOfProgram?.Event("RecipePage - Initialized new Ingredient");
 }

        cmbAccuracyRecipe.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        cmbAccuracyIngredient.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

accuracyRecipe = new UiAccuracy(txtAccuracyOfChoRecipe, cmbAccuracyRecipe);
        accuracyIngredient = new UiAccuracy(txtAccuracyOfChoIngredient, cmbAccuracyIngredient);

    RefreshUi();

        //recipeSection.BindingContext = bl.Recipe;
    }
    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        // Initialize accuracy controls to sync combo boxes with current text values
        // InitializeAccuracyControls();
    }
    #region UI related methods
    private async Task RefreshUi()
    {
        RefreshRecipe();
        // the current Ingredient is unbound and not refreshed
        RefreshGrid();
    }
    private void RefreshRecipe()
    {
        try
        {
            if (recipeSection != null && bl?.Recipe != null)
            {
                recipeSection.BindingContext = null;
                recipeSection.BindingContext = bl.Recipe;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - RefreshMeal", ex);
        }
    }
    private void RefreshGrid()
    {
        try
        {
            //bl.Recipe.Ingredients = bl.ReadAllIngredientsInThisRecipe();
            bl.GetAllIngredientsInThisRecipe();
            gridIngredients.BindingContext = null;
            gridIngredients.BindingContext = bl.Recipe.Ingredients;
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - RefreshGrid", ex);
        }
    }
    private void FromClassToBoxesIngredient()
    {
        ingredientModifications = true;
        if (bl.Ingredient != null)
        {
            txtIngredientName.Text = bl.Ingredient.Name ?? "";
            txtAccuracyOfChoIngredient.Text = (bl.Ingredient.AccuracyOfChoEstimate?.Double ?? 0).ToString();
            txtIngredientQuantityInUnits.Text = (bl.Ingredient.QuantityGrams?.Double ?? 0).ToString();
            txtIngredientCarbohydratesGrams.Text = bl.Ingredient.CarbohydratesPercent?.Text
                ?? (bl.Ingredient.CarbohydratesPercent?.Double ?? 0).ToString();
            txtIdIngredient.Text = bl.Ingredient.IdIngredient?.ToString() ?? "";
        }
        ingredientModifications = false;
    }
    private void FromBoxesIngredientToClass()
    {
        // Ensure bl.Ingredient is initialized
    if (bl.Ingredient == null)
{
 bl.Ingredient = new Ingredient();
 }
    
        //bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdFoodInMeal.Text);
        bl.Ingredient.Name = Safe.String(txtIngredientName.Text);

  // Initialize DoubleAndText properties if null
  if (bl.Ingredient.AccuracyOfChoEstimate == null)
 bl.Ingredient.AccuracyOfChoEstimate = new DoubleAndText();
     if (bl.Ingredient.CarbohydratesPercent == null)
bl.Ingredient.CarbohydratesPercent = new DoubleAndText();
     if (bl.Ingredient.QuantityGrams == null)
            bl.Ingredient.QuantityGrams = new DoubleAndText();
   
  bl.Ingredient.AccuracyOfChoEstimate.Text = txtAccuracyOfChoIngredient.Text;
   bl.Ingredient.CarbohydratesPercent.Text = txtIngredientCarbohydratesGrams.Text;
    // in this page the unit is read only, taken from the Ingredient object
  bl.Ingredient.QuantityGrams.Text = txtIngredientQuantityInUnits.Text;
    }
    #endregion
    #region controls' events
    private async void btnSaveAllRecipe_Click(object sender, EventArgs e)
    {
        try
        {
            // Save the Recipe
            bl.Recipe.IdRecipe = bl.SaveOneRecipe(bl.Recipe);
            // Save the Ingredient object
            // Ensure the food is associated with the current Recipe (not necessary)
            bl.Ingredient.IdRecipe = bl.Recipe.IdRecipe;
            // Save the old food in Recipe 
            bl.UpdateOldIngredientInList();
            // copy the data from the UI into the business layer class
            FromBoxesIngredientToClass();
            // Save all the ingredients in the Recipe
            bl.SaveAllIngredientsInRecipe();
            // Refresh the UI to show updated data
            RefreshUi();
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
        // Ensure we have a valid recipe to add ingredient to
        if (bl.Recipe.IdRecipe == null)
        {
            // Save the recipe first if it doesn't exist
            bl.Recipe.IdRecipe = bl.SaveOneRecipe(bl.Recipe);
        }
        // Create new Ingredient entry
        bl.Ingredient.IdIngredient = null; // Reset ID for new entry
        bl.Ingredient.IdRecipe = bl.Recipe.IdRecipe; // Associate with current recipe

        if (bl.Recipe.Ingredients == null)
            bl.Recipe.Ingredients = new List<Ingredient>();

        // Save the ingredient
        var savedId = bl.SaveOneIngredient(bl.Ingredient);
        if (savedId != null)
        {
            bl.Ingredient.IdIngredient = savedId;

            // Add to business layer list if not already there
            if (!bl.Recipe.Ingredients.Any(f => f.IdIngredient == savedId))
            {
                // Create a copy of the current ingredient to add to the list
                var ingredientCopy = new Ingredient
                {
                    IdIngredient = bl.Ingredient.IdIngredient,
                    IdRecipe = bl.Ingredient.IdRecipe,
                    Name = bl.Ingredient.Name,
                    CarbohydratesPercent = bl.Ingredient.CarbohydratesPercent,
                    QuantityInUnits = bl.Ingredient.QuantityInUnits,
                    CarbohydratesGrams = bl.Ingredient.CarbohydratesGrams,
                    AccuracyOfChoEstimate = bl.Ingredient.AccuracyOfChoEstimate,
                    //UnitSymbol = bl.Ingredient.UnitSymbol,
                    //GramsInOneUnit = bl.Ingredient.GramsInOneUnit
                };
                bl.Recipe.Ingredients.Add(ingredientCopy);
            }
            bl.RecalcAll();
            FromClassToBoxesIngredient();
            General.LogOfProgram.Event("Ingredient added to recipe successfully");
        }
        else
        {
            DisplayAlert("Error", "Failed to add ingredient to recipe", "OK");
        }
        RefreshGrid();
    }
    private async void btnRemoveIngredient_Click(object sender, EventArgs e)
    {
        try
        {
            if (bl.Ingredient != null && bl.Ingredient.IdIngredient != null)
            {
                bl.DeleteOneIngredient(bl.Ingredient);

                // Remove from business layer list
                if (bl.Recipe.Ingredients != null)
                {
                    bl.Recipe.Ingredients.RemoveAll(f => f.IdIngredient == bl.Ingredient.IdIngredient);
                }

                // Update the ObservableCollection for UI binding
                //UpdateFoodsInMealCollection();

                bl.RecalcAll();
                FromClassToBoxesIngredient();
            }
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - btnRemoveFoodInMeal_Click", ex);
        }
    }
    private async void btnFoods_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            if (bl.Ingredient == null)
            {
                bl.Ingredient = new Ingredient();
            }
            
            foodsPage = new FoodsPage(bl.Ingredient);
            await Navigation.PushModalAsync(foodsPage);

            // Wait for the page to be closed and get the result
            bool foodWasChosen = await foodsPage.PageClosedTask;

            // check if the user chose a food in called page
            if (foodWasChosen && foodsPage.FoodIsChosen)
            {
                // Aggiorna l'Ingredient corrente con il Food scelto dalla pagina chiamata
                bl.FromFoodToIngredient(foodsPage.Food, bl.Ingredient);

                // DEBUG: Log per verificare i valori
                General.LogOfProgram?.Debug($"Food importato: Nome={bl.Ingredient.Name}, CHO%={bl.Ingredient.CarbohydratesPercent?.Double ?? 0}");

                // Aggiorna l'interfaccia utente con i nuovi dati
                FromClassToBoxesIngredient();

                // Ricalcola tutti i valori
                bl.RecalcAll();

                // Aggiorna l'UI della recipe
                RefreshRecipe();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - btnFoods_ClickAsync", ex);
            await DisplayAlert("Error", $"Failed to import food: {ex.Message}", "OK");
        }
    }
    private async void btnDefaults_Click(object sender, EventArgs e)
    {
        try
        {
            txtIngredientName.Text = "";
            txtAccuracyOfChoIngredient.Text = "";
            cmbAccuracyIngredient.SelectedItem = null;
            txtIngredientCarbohydratesGrams.Text = "";
            txtIngredientQuantityPercent.Text = "";
            txtIdIngredient.Text = "";
            //txtFoodCarbohydratesGrams.Text = "";
            //btnUnit.Text = "";
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - btnDefaults_Click", ex);
        }
    }
    private async void btnCalc_Click(object sender, EventArgs e)
    {
        try
        {
            // take the data from the UI controls and put it into the business layer class
            FromBoxesIngredientToClass();
            bl.RecalcAll();

            // Refresh the bound UI data related to the Meal, since it has changed
            if (recipeSection != null && bl?.Recipe != null)
            {
                recipeSection.BindingContext = null;
                recipeSection.BindingContext = bl.Recipe;
            }
            RefreshRecipe();
            // Also refresh the visualization of the grid
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnCalc_Click", ex);
            DisplayAlert("Error", "Failed to calculate meal totals. Check logs for details.", "OK");
        }
    }
    // ???? should we put the button of Weighing ????
    private void ingredientsSection_Unfocused(object sender, FocusEventArgs e)
    {
        // when finished with the current ingredient,
        // update the data in the bl and show the changes

        // update bl.FoodInMeal from the UI controls
        FromBoxesIngredientToClass();
        bl.RecalcAll();
        bl.SaveAllIngredientsInRecipe();
        // Refresh the bound UI data related to the Meal, since it has changed
        if (recipeSection != null && bl?.Recipe != null)
        {
            recipeSection.BindingContext = null;
            recipeSection.BindingContext = bl.Recipe;
        }
    }









    private async Task FromClassesToUi()
    {
        FromRecipeToUi();
        FromIngredientToUi();

        // Update accuracy controls after data is loaded and loadingUi is false
        await RefreshAccuracyControls();
    }

    private void FromUiToClasses()
    {
        if (bl.Ingredient == null)
            bl.Ingredient = new();
        // first the current ingredient, THEN the recipe!
        FromUiToIngredient(bl.Ingredient);
        FromUiToRecipe(bl.Recipe);
    }
    private void FromUiToIngredient(Ingredient Ingredient)
    {
        Ingredient.IdIngredient = Safe.Int(txtIdIngredient.Text);
        Ingredient.IdRecipe = Safe.Int(txtIdRecipe.Text);
        //////////Ingredient.QuantityGrams.Text = txtIngredientQuantityGrams.Text;
        //////////Ingredient.CarbohydratesPercent.Text = txtIngredientCarbohydratesPercent.Text;
        Ingredient.QuantityInUnits.Text = txtIngredientQuantityPercent.Text;
        Ingredient.Name = txtIngredientName.Text;
        Ingredient.AccuracyOfChoEstimate.Text = txtAccuracyOfChoIngredient.Text;
    }
    private void FromUiToRecipe(Recipe Recipe)
    {
        Recipe.IdRecipe = Safe.Int(txtIdRecipe.Text);
        Recipe.Name = txtRecipeName.Text;
        Recipe.Description = txtRecipeDescription.Text;
        Recipe.CarbohydratesPercent.Text = txtChoOfRecipePercent.Text;
        Recipe.AccuracyOfChoEstimate.Text = txtAccuracyOfChoRecipe.Text;
    }
    private void FromIngredientToUi()
    {
        //if (bl.Ingredient != null)
        //{
        ingredientSection.BindingContext = null;
        ingredientSection.BindingContext = bl.Ingredient;
        //}
    }
    private void FromRecipeToUi()
    {
        if (bl.Recipe != null)
        {
            recipeSection.BindingContext = null;
            recipeSection.BindingContext = bl.Recipe;
        }
    }
    private void txtIngredientCarbohydratesPercent_TextChanged(object sender, EventArgs e)
    {

        ////////        FromUiToIngredient(localIngredientForCalculations);
        ////////        bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
        ////////        txtIngredientChoGrams.Text = localIngredientForCalculations.CarbohydratesPercent.Text;
        ////////        bl.SaveRecipeParameters();

    }
    private void txtIngredientQuantityGrams_TextChanged(object sender, EventArgs e)
    {

        ////////FromUiToIngredient(localIngredientForCalculations);
        ////////bl.CalculateChoOfFoodGrams(localIngredientForCalculations);
        ////////txtIngredientChoGrams.Text = localIngredientForCalculations.CarbohydratesGrams.Text;
    }
    private void txtIngredientChoGrams_TextChanged(object sender, EventArgs e)
    {
        //if (!loadingUi)
        {
            //if (!txtIngredientQuantityGrams.IsFocused && !txtIngredientCarbohydratesPercent.IsFocused)
            //{
            //    //txtIngredientQuantityGrams.Text = "";
            //    //////////localIngredientForCalculations.QuantityInUnits.Double = 0;
            //    //txtIngredientCarbohydratesPercent.Text = "";
            //}
        }
        ////localIngredientForCalculations.CarbohydratesGrams.Text = txtIngredientChoGrams.Text;
        //bl.UpdateFoodInMealAndRecalc();
        //FromRecipeToUi();
        //txtChoOfRecipePercent.Text = bl.Recipe.CarbohydratesPercent.Text;
    }
    private void btnSaveAllFoods_Click(object sender, EventArgs e)
    {
        FromUiToClasses();
        bl.SaveOneIngredient(bl.Ingredient).ToString();
        bl.SaveListOfIngredients();
        RefreshGrid();
    }
    private async void btnSearchFood_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new FoodsPage(txtRecipeName.Text, ""));
    }
    private async void btnWeighFood_Click(object sender, EventArgs e)
    {
     try
 {
            // Ensure bl.Ingredient is initialized
       if (bl.Ingredient == null)
         {
  bl.Ingredient = new Ingredient();
    General.LogOfProgram?.Event("RecipePage - Initialized new Ingredient for weighing");
            }

   // Update the current ingredient from UI before opening WeighFoodPage
   FromBoxesIngredientToClass();
  
       // Open WeighFoodPage with current ingredient data
   var weighFoodPage = new WeighFoodPage(bl.Ingredient);
  await Navigation.PushModalAsync(weighFoodPage);
 
   // Wait for the page to be closed and get the result
    bool dataWasModified = await weighFoodPage.PageClosedTask;
          
// Check if the user modified food data in the WeighFoodPage
 if (dataWasModified && weighFoodPage.ResultFood != null)
      {
    // Update the current Ingredient with the modified Food data
 bl.FromFoodToIngredient(weighFoodPage.ResultFood, bl.Ingredient);
          
     // Set the QuantityGrams with the WeightOfPortion from WeighFoodPage
    // This updates the "Quantity" field in the RecipePage
 if (weighFoodPage.WeightOfPortion > 0)
   {
       bl.Ingredient.QuantityGrams.Double = weighFoodPage.WeightOfPortion;
  bl.Ingredient.QuantityGrams.Text = weighFoodPage.WeightOfPortion.ToString("F1");
 General.LogOfProgram?.Event($"RecipePage - Quantity set to {weighFoodPage.WeightOfPortion:F1}g from WeighFoodPage");
   }
     
   // Recalculate the carbohydrates in grams of this Ingredient
 // (similar to CalculateChoOfFoodGrams in MealPage)
     if (bl.Ingredient.CarbohydratesPercent.Double != null && bl.Ingredient.QuantityGrams.Double != null)
 {
 bl.Ingredient.CarbohydratesGrams.Double = 
       bl.Ingredient.CarbohydratesPercent.Double / 100 * bl.Ingredient.QuantityGrams.Double;
   }
  
       // Update the user interface with the new data
       FromClassToBoxesIngredient();
      
        // Recalculate all values
    bl.RecalcAll();
        
  // Update the recipe UI
      RefreshRecipe();
    
     General.LogOfProgram?.Event("Ingredient data updated from WeighFoodPage successfully");
    }
      }
 catch (Exception ex)
    {
   General.LogOfProgram?.Error("RecipePage - btnWeighFood_Click", ex);
     await DisplayAlert("Error", $"Failed to open weigh food page: {ex.Message}\n\nCheck logs for details.", "OK");
     }
  }
    private async void gridIngredients_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            if (e.SelectedItem != bl.Ingredient)
            {
                if (bl.Ingredient.Name != null)
                {
                    FromBoxesIngredientToClass();
                    bl.UpdateOldIngredientInList();
                    RefreshGrid();
                }
                bl.Ingredient = (Ingredient)e.SelectedItem;
                FromClassToBoxesIngredient();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - gridFoodsInMeal_ItemSelected", ex);
        }
    }
    protected override async void OnAppearing()
    {
        if (foodsPage != null && foodsPage.FoodIsChosen)
        {
            bl.FromFoodToIngredient(foodsPage.Food, bl.Ingredient);
            await FromClassesToUi();
            //////////txtIngredientCarbohydratesPercent.Focus();
        }
    }
    private void btnGetIngredientFood_Click(object sender, EventArgs e)
    {
        FoodsPage p = new FoodsPage(bl.Ingredient);
        Navigation.PushAsync(p);
    }
    private void txtIngredientQuantityGrams_TextChanged(object sender, TextChangedEventArgs e)
    {
        //////////bl.UpdateFoodInMealAndRecalc();
        //FromClassesToUi();
    }
    private void txtIngredientCarbohydratesPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        //if (!txtIngredientChoPercent.IsFocused && !txtFoodCarbohydratesPercent.IsFocused)
        //{
        //    //txtFoodQuantityGrams.Text = "";
        //    //localFoodInMealForCalculations.QuantityInUnits.Double = 0;
        //    //txtFoodCarbohydratesPercent.Text = "";
        //    //localFoodInMealForCalculations.CarbohydratesPercent.Double = 0;
        //}
        //localFoodInMealForCalculations.CarbohydratesGrams.Text = txtFoodChoGrams.Text;
    }
    private void txtIngredientQuantityPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        //bl.Ingredient.QuantityInUnits.Double = null;
        //txtIngredientQuantityGrams.Text = bl.Ingredient.QuantityInUnits.Text;
    }
    private void Calculator_Click(object sender, TappedEventArgs e)
    {
        // save the entries in the classes
        FromUiToClasses();

        var focusedEntry = GetFocusedEntry();
        string sValue = focusedEntry?.Text ?? "0";
        double dValue = double.TryParse(sValue, out var val) ? val : 0;

        // start the CalculatorPage passing to it the value of the 
        // controller that currently has the focus
        var calculator = new CalculatorPage(dValue);
        ////////await Navigation.PushModalAsync(calculator);
        ////////var result = await calculator.ResultSource.Task;

        // check if the page has given back a result
        ////////if (result.HasValue)
        ////////{
        ////////// update the data model
        ////////if (focusedEntry == txtMealCarbohydratesGrams)
        ////////{
        ////////    //bl.FoodInMeal.CarbohydratesGrams.Text = result.Value.ToString();
        ////////    txtMealCarbohydratesGrams.Text = result.Value.ToString();
        ////////    txtMealCarbohydratesGrams_TextChanged(null, null);
        ////////}
        ////////else if (focusedEntry == txtFoodCarbohydratesPercent)
        ////////{
        ////////    //bl.FoodInMeal.CarbohydratesPercent.Text = result.Value.ToString();
        ////////    txtFoodCarbohydratesPercent.Text = result.Value.ToString();
        ////////    //txtFoodChoOrQuantity_TextChanged(null, null);
        ////////}
        ////////else if (focusedEntry == txtFoodQuantityInUnits)
        ////////{
        ////////    //bl.FoodInMeal.QuantityInUnits.Text = result.Value.ToString();
        ////////    txtFoodQuantityInUnits.Text = result.Value.ToString();
        ////////    //txtFoodChoOrQuantity_TextChanged(null, null);
        ////////}
        ////////else if (focusedEntry == txtFoodCarbohydratesGrams)
        ////////{
        ////////    //bl.FoodInMeal.CarbohydratesPercent.Text = result.Value.ToString();
        ////////    txtFoodCarbohydratesGrams.Text = result.Value.ToString();
        ////////    //txtFoodCarbohydratesGrams_TextChanged(null, null);
        ////////}
        // show the UI starting from the classes
        //FromClassesToUi();
        ////////}
    }
    private Entry GetFocusedEntry()
    {
        if (txtAccuracyOfChoRecipe != null && txtAccuracyOfChoRecipe.IsFocused) return txtAccuracyOfChoRecipe;
        if (txtAccuracyOfChoIngredient != null && txtAccuracyOfChoIngredient.IsFocused) return txtAccuracyOfChoIngredient;
        ////////////if (txtIngredientQuantityGrams != null && txtIngredientQuantityGrams.IsFocused) return txtIngredientQuantityGrams;
        ////////////if (txtIngredientCarbohydratesPercent != null && txtIngredientCarbohydratesPercent.IsFocused) return txtIngredientCarbohydratesPercent;
        if (txtChoOfRecipePercent != null && txtChoOfRecipePercent.IsFocused) return txtChoOfRecipePercent;
        // aggiungi altri Entry se necessario
        return null;
    }

    private async void btnRecipes_Click(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecipesPage(null));
    }

    // ACCURACY MANAGEMENT METHODS (following MealPage pattern exactly)

    //private void InitializeAccuracyControls()
    //{
    //    try
    //    {
    //        // Initialize recipe accuracy combo box based on current text value
    //        if (accuracyRecipe != null && !string.IsNullOrEmpty(txtAccuracyOfChoRecipe.Text))
    //        {
    //            if (double.TryParse(txtAccuracyOfChoRecipe.Text, out double recipeAccuracy))
    //            {
    //                var qualitativeAccuracy = accuracyRecipe.GetQualitativeAccuracyGivenQuantitavive(recipeAccuracy);
    //                cmbAccuracyRecipe.SelectedItem = qualitativeAccuracy;
    //            }
    //        }

    //        // Initialize ingredient accuracy combo box based on current text value
    //        if (accuracyIngredient != null && !string.IsNullOrEmpty(txtAccuracyOfChoIngredient.Text))
    //        {
    //            if (double.TryParse(txtAccuracyOfChoIngredient.Text, out double ingredientAccuracy))
    //            {
    //                var qualitativeAccuracy = accuracyIngredient.GetQualitativeAccuracyGivenQuantitavive(ingredientAccuracy);
    //                cmbAccuracyIngredient.SelectedItem = qualitativeAccuracy;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        General.LogOfProgram?.Error("RecipePage - InitializeAccuracyControls", ex);
    //    }
    //}

    private async Task RefreshAccuracyControls()
    {
        try
        {
            // Small delay to ensure data binding has completed (like in MealPage)
            await Task.Delay(50);

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
            // Update recipe accuracy controls after data binding changes
            if (accuracyRecipe != null && !string.IsNullOrEmpty(txtAccuracyOfChoRecipe.Text))
            {
                if (double.TryParse(txtAccuracyOfChoRecipe.Text, out double recipeAccuracy))
                {
                    // Update combo box selection
                    var qualitativeAccuracy = accuracyRecipe.GetQualitativeAccuracyGivenQuantitavive(recipeAccuracy);
                    cmbAccuracyRecipe.SelectedItem = qualitativeAccuracy;

                    // Update text box colors using UiAccuracy logic
                    txtAccuracyOfChoRecipe.BackgroundColor = accuracyRecipe.AccuracyBackColor(recipeAccuracy);
                    txtAccuracyOfChoRecipe.TextColor = accuracyRecipe.AccuracyForeColor(recipeAccuracy);
                }
            }
            else
            {
                // Reset to default if no valid accuracy
                cmbAccuracyRecipe.SelectedItem = null;
                txtAccuracyOfChoRecipe.BackgroundColor = Colors.LightGreen; // Default from XAML
                txtAccuracyOfChoRecipe.TextColor = Colors.Black; // Default from XAML
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
            // Update ingredient accuracy controls after data binding changes
            if (accuracyIngredient != null && !string.IsNullOrEmpty(txtAccuracyOfChoIngredient.Text))
            {
                if (double.TryParse(txtAccuracyOfChoIngredient.Text, out double ingredientAccuracy))
                {
                    // Update combo box selection
                    var qualitativeAccuracy = accuracyIngredient.GetQualitativeAccuracyGivenQuantitavive(ingredientAccuracy);
                    cmbAccuracyIngredient.SelectedItem = qualitativeAccuracy;

                    // Update text box colors using UiAccuracy logic
                    txtAccuracyOfChoIngredient.BackgroundColor = accuracyIngredient.AccuracyBackColor(ingredientAccuracy);
                    txtAccuracyOfChoIngredient.TextColor = accuracyIngredient.AccuracyForeColor(ingredientAccuracy);
                }
            }
            else
            {
                // Reset to default if no valid accuracy
                cmbAccuracyIngredient.SelectedItem = null;
                txtAccuracyOfChoIngredient.BackgroundColor = Colors.LightGreen; // Default from XAML
                txtAccuracyOfChoIngredient.TextColor = Colors.Black; // Default from XAML
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - RefreshIngredientAccuracyControls", ex);
        }
    }

    // EVENT HANDLERS - These should be minimal and only update the data model
    // Let UiAccuracy handle all UI synchronization automatically

    // RECIPE ACCURACY EVENTS
    private void txtAccuracyOfChoRecipe_TextChanged(object sender, TextChangedEventArgs e)
    {
        // The UiAccuracy class handles all synchronization internally
        // Do not interfere with its operation
    }

    private void txtAccuracyOfChoRecipe_Unfocused(object sender, FocusEventArgs e)
    {
        // Let UiAccuracy handle the combo box update, we only update the data model
        try
        {
            //if (!loadingUi && bl?.Recipe != null && !string.IsNullOrEmpty(txtAccuracyOfChoRecipe.Text))
            //{
            //    if (double.TryParse(txtAccuracyOfChoRecipe.Text, out double accuracy))
            //    {
            //        // Update the recipe's accuracy in the data model
            //        bl.Recipe.AccuracyOfChoEstimate.Double = accuracy;
            //    }
            //}
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - txtAccuracyOfChoRecipe_Unfocused", ex);
        }
    }

    private void cmbAccuracyRecipe_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Let UiAccuracy handle the text box update, we only update the data model
        try
        {
            //if (!loadingUi && bl?.Recipe != null && cmbAccuracyRecipe.SelectedItem != null)
            //{
            //    var selectedAccuracy = (QualitativeAccuracy)cmbAccuracyRecipe.SelectedItem;
            //    double numericValue = (double)selectedAccuracy;

            //    // Update the recipe's accuracy in the data model
            //    bl.Recipe.AccuracyOfChoEstimate.Double = numericValue;
            //}
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("RecipePage - cmbAccuracyRecipe_SelectedIndexChanged", ex);
        }
    }

    //// INGREDIENT ACCURACY EVENTS
    //private void txtAccuracyOfChoIngredient_TextChanged(object sender, TextChangedEventArgs e)
    //{
    //    // The UiAccuracy class handles all synchronization internally
    //    // Do not interfere with its operation
    //}

    //private void txtAccuracyOfChoIngredient_Unfocused(object sender, FocusEventArgs e)
    //{
    //    // Let UiAccuracy handle the combo box update, we only update the data model
    //    try
    //    {
    //        //if (!loadingUi && bl?.Ingredient != null && !string.IsNullOrEmpty(txtAccuracyOfChoIngredient.Text))
    //        //{
    //        //    if (double.TryParse(txtAccuracyOfChoIngredient.Text, out double accuracy))
    //        //    {
    //        //        // Update the ingredient's accuracy in the data model
    //        //        bl.Ingredient.AccuracyOfChoEstimate.Double = accuracy;
    //        //    }
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        General.LogOfProgram?.Error("RecipePage - txtAccuracyOfChoIngredient_Unfocused", ex);
    //    }
    //}

    //private void cmbAccuracyIngredient_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    // Let UiAccuracy handle the text box update, we only update the data model
    //    try
    //    {
    //        //if (!loadingUi && bl?.Ingredient != null && cmbAccuracyIngredient.SelectedItem != null)
    //        //{
    //        //    var selectedAccuracy = (QualitativeAccuracy)cmbAccuracyIngredient.SelectedItem;
    //        //    double numericValue = (double)selectedAccuracy;

    //        //    // Update the ingredient's accuracy in the data model
    //        //    bl.Ingredient.AccuracyOfChoEstimate.Double = numericValue;
    //        //}
    //    }
    //    catch (Exception ex)
    //    {
    //        General.LogOfProgram?.Error("RecipePage - cmbAccuracyIngredient_SelectedIndexChanged", ex);
    //    }
    //}
    private void txtIngredientCarbohydratesGrams_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    #endregion

    private void btnFoods_Click(object sender, EventArgs e)
    {

    }
}
