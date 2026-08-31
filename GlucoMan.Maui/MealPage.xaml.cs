using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using System.ComponentModel;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class MealPage : ContentPage, INotifyPropertyChanged
{
    // since it is accessed by several pages, we use a common business
    // layer beetween different pages
    private BL_MealAndFood bl = new();
    private BL_General BlGeneral = new();

    private UiAccuracy accuracyMeal;
    private UiAccuracy accuracyFoodInMeal;

    FoodsPage foodsPage;
    RecipesPage recipesPage;
    InsulinCalcPage insulinCalcPage;
    InjectionsPage injectionsPage;
    GlucoseMeasurementsPage measurementPage;

    private Color initialButtonBackground;
    private Color initialButtonTextColor;

    // List for storing foods in this meal
    private List<FoodInMeal> foodsInMeal;

    private bool foodInMealModifications = false;
    private bool foodInMealPercentOrQuantityChanging = false;
    private bool foodInMealChoGramsChanging = false;
    private bool programmaticModification = true;
    private bool _programmaticChoUpdate = false;
    private bool _choManuallyModified = false;
    private bool _backNavigationInProgress = false;
    private double? _storedCho;

    public MealPage(Meal Meal)
    {
        InitializeComponent();

        bl.Meal = Meal;

        initialButtonBackground = btnStartMeal.BackgroundColor;
        initialButtonTextColor = btnStartMeal.TextColor;

        if (Meal == null)
        {
            Meal = new Meal();
            btnDefaults_Click(null, null);
        }
        bl.Meal = Meal;

        _storedCho = bl.Meal?.CarbohydratesGrams?.Double;

        if (bl.Meal.IdMeal == null || (bl.Meal.EventTime.DateTime + new TimeSpan(0, 15, 0) > Common.LocalNow))
        {
            btnStartMeal.BackgroundColor = Colors.Red;
            btnStartMeal.TextColor = Colors.Yellow;
        }
        // fill the combos
        cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        cmbAccuracyFoodInMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

        // create the objects that manage the accuracies 
        accuracyMeal = new UiAccuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);
        accuracyFoodInMeal = new UiAccuracy(txtAccuracyOfChoFoodInMeal, cmbAccuracyFoodInMeal);

        if (bl.Meal.IdTypeOfMeal == null || bl.Meal.IdTypeOfMeal == TypeOfMeal.NotSet)
        {
            bl.Meal.IdTypeOfMeal = Common.SelectTypeOfMealBasedOnTimeNow();
        }
        RefreshUi();

        // Set the page as its own BindingContext for property binding
        this.BindingContext = this;

        if (bl.FoodInMeal == null)
        {
            bl.FoodInMeal = new FoodInMeal();
        }
    }
    #region UI related methods
    private void RefreshUi()
    {
        RefreshMeal();
        // the current FoodIn Meal is unbound and not refreshed
        RefreshGrid();
    }
    private void RefreshMeal()
    {
        try
        {
            if (mealSection != null && bl?.Meal != null)
            {
                mealSection.BindingContext = null;
                mealSection.BindingContext = bl.Meal;
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
            bl.FoodsInMeal = bl.GetFoodsInMeal(bl.Meal.IdMeal);
            gridFoodsInMeal.BindingContext = null;
            gridFoodsInMeal.BindingContext = bl.FoodsInMeal;
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - RefreshGrid", ex);
        }
    }
    private void FromClassToBoxesFoodInMeal()
    {
        foodInMealModifications = true;

        txtFoodInMealName.Text = bl.FoodInMeal.Name;
        btnUnit.Text = bl.FoodInMeal.UnitSymbol;
        if (!programmaticModification)
        {
            // unformatted visulization for user's modifications
            txtAccuracyOfChoFoodInMeal.Text = Convert.ToDouble(bl.FoodInMeal.AccuracyOfChoEstimate.Double).ToString();
            txtFoodCarbohydratesPerUnit.Text = Convert.ToDouble(bl.FoodInMeal.CarbohydratesPercent.Double).ToString();
            txtFoodQuantityInUnits.Text = Convert.ToDouble(bl.FoodInMeal.QuantityInUnits.Double).ToString();
            txtFoodCarbohydratesGrams.Text = Convert.ToDouble(bl.FoodInMeal.CarbohydratesGrams.Double).ToString();
            Common.SetCursorToStart(txtFoodInMealName);
        }
        else
        {
            // formatted visualization for program's modifications
            txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
            txtFoodCarbohydratesPerUnit.Text = bl.FoodInMeal.CarbohydratesPercent.Text;
            txtFoodQuantityInUnits.Text = bl.FoodInMeal.QuantityInUnits.Text;
            txtFoodCarbohydratesGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
            Common.SetCursorToStart(txtFoodInMealName);
        }
        foodInMealModifications = false;
    }
    private void UpdateMealTotalChoInUI()
    {
        bl.RecalcTotalCho();
        _programmaticChoUpdate = true;
        txtMealCarbohydratesGrams.Text = bl.Meal.CarbohydratesGrams.Text;
        _programmaticChoUpdate = false;
    }
    private void FromBoxesFoodInMealToClass()
    {
        //blMeal.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdFoodInMeal.Text);
        bl.FoodInMeal.Name = Safe.String(txtFoodInMealName.Text);
        bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
        bl.FoodInMeal.CarbohydratesPercent.Text = txtFoodCarbohydratesPerUnit.Text;
        // in this page the unit is read only, taken from the FoodInMeal object
        bl.FoodInMeal.QuantityInUnits.Text = txtFoodQuantityInUnits.Text;
        bl.FoodInMeal.CarbohydratesGrams.Text = txtFoodCarbohydratesGrams.Text;
    }
    #endregion
    #region controls' events    
    private async Task CheckDiscrepancyAndSaveAsync()
    {
        if (bl.FoodInMeal != null)
        {
            bl.FoodInMeal.IdMeal = bl.Meal.IdMeal;
            FromBoxesFoodInMealToClass();
            bl.UpdateOldFoodInMealInList();
        }
        bl.SaveAllFoodsInMeal();

        double? displayedCho = Safe.Double(txtMealCarbohydratesGrams.Text);
        double? displayedAccuracy = Safe.Double(txtAccuracyOfChoMeal.Text);

        // Store values BEFORE recalculation to detect actual changes
        double? oldCho = bl.Meal.CarbohydratesGrams.Double;
        double? oldAccuracy = bl.Meal.AccuracyOfChoEstimate.Double;

        bl.RecalcTotalCho();
        bl.RecalcTotalAccuracy();

        double? calculatedCho = bl.Meal.CarbohydratesGrams.Double;
        double? calculatedAccuracy = bl.Meal.AccuracyOfChoEstimate.Double;

        // Check if displayed values differ from calculated values (NOT from stored values)
        bool choChanged = Math.Abs((displayedCho ?? 0) - (calculatedCho ?? 0)) > 0.01;
        bool accuracyChanged = Math.Abs((displayedAccuracy ?? 0) - (calculatedAccuracy ?? 0)) > 0.01;

        // Only prompt if there are foods AND the displayed values genuinely differ from calculated
        if ((bl.FoodsInMeal != null && bl.FoodsInMeal.Count != 0) && (choChanged || accuracyChanged))
        {
            bool useCalculatedValues = await DisplayAlert(
                AppStrings.ValueDiscrepancy,
                AppStrings.ValueDiscrepancyMessage,
                AppStrings.UseCalculated,
                AppStrings.KeepDisplayed);

            if (!useCalculatedValues)
            {
                bl.Meal.CarbohydratesGrams.Double = displayedCho;
                bl.Meal.AccuracyOfChoEstimate.Double = displayedAccuracy;
            }
        }
        else
        {
            bl.Meal.CarbohydratesGrams.Double = displayedCho;
            bl.Meal.AccuracyOfChoEstimate.Double = displayedAccuracy;
        }
        SaveOrCreateMealData();
    }
    private async void btnSaveAllMeal_Click(object sender, EventArgs e)
    {
        try
        {
            await Services.TimeZoneCheckService.Instance.CheckAndPromptIfChangedAsync(this);
            await CheckDiscrepancyAndSaveAsync();
            _choManuallyModified = false;
            _storedCho = bl.Meal.CarbohydratesGrams.Double;
            RefreshUi();
            General.LogOfProgram?.Event("Meal saved successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnSaveAllMeal_Click", ex);
            await DisplayAlert(AppStrings.Error, AppStrings.FailedToSaveMealData, AppStrings.OK);
        }
    }
    private void SaveOrCreateMealData()
    {
        if (bl.Meal.IdMeal == null)
        {
            // save the meal with new Id and current time if it doesn't have an ID
            bl.Meal.IdMeal = bl.SaveOneMeal(bl.Meal, true);
        }
        else
        {   // save the meal without changing the time
            bl.SaveOneMeal(bl.Meal, false);
        }
    }
    private void btnAddFoodInMeal_Click(object sender, EventArgs e)
    {
        try
        {
            // Ensure we have a valid meal to add food to
            if (bl.Meal.IdMeal == null)
            {
                // Save the meal first if it doesn't exist
                bl.Meal.IdMeal = bl.SaveOneMeal(bl.Meal, true);
            }
            // Create new FoodInMeal entry
            bl.FoodInMeal.IdFoodInMeal = null; // Reset ID for new entry
            bl.FoodInMeal.IdMeal = bl.Meal.IdMeal; // Associate with current meal

            if (bl.FoodsInMeal == null)
                bl.FoodsInMeal = new List<FoodInMeal>();

            FromBoxesFoodInMealToClass();
            // Save the food in meal
            var savedId = bl.SaveOneFoodInMeal(bl.FoodInMeal);
            if (savedId != null)
            {
                bl.FoodInMeal.IdFoodInMeal = savedId;

                // Add to business layer list if not already there
                if (!bl.FoodsInMeal.Any(f => f.IdFoodInMeal == savedId))
                {
                    // Create a copy of the current food to add to the list
                    var foodCopy = new FoodInMeal
                    {
                        IdFoodInMeal = bl.FoodInMeal.IdFoodInMeal,
                        IdMeal = bl.FoodInMeal.IdMeal,
                        IdFood = bl.FoodInMeal.IdFood,
                        Name = bl.FoodInMeal.Name,
                        CarbohydratesPercent = bl.FoodInMeal.CarbohydratesPercent,
                        QuantityInUnits = bl.FoodInMeal.QuantityInUnits,
                        CarbohydratesGrams = bl.FoodInMeal.CarbohydratesGrams,
                        AccuracyOfChoEstimate = bl.FoodInMeal.AccuracyOfChoEstimate,
                        UnitSymbol = bl.FoodInMeal.UnitSymbol,
                        GramsInOneUnit = bl.FoodInMeal.GramsInOneUnit
                    };
                    bl.FoodsInMeal.Add(foodCopy);
                }
                bl.RecalcAll();
                FromClassToBoxesFoodInMeal();
                General.LogOfProgram.Event("Food added to meal successfully");
            }
            else
            {
                DisplayAlert(AppStrings.Error, AppStrings.FailedToAddFoodDetails, AppStrings.OK);
            }
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - btnAddFoodInMeal_Click", ex);
            DisplayAlert("Error", "Failed to add food to meal. Check logs for details.", "OK");
        }
    }
    private void btnRemoveFoodInMeal_Click(object sender, EventArgs e)
    {
        try
        {
            if (bl.FoodInMeal != null && bl.FoodInMeal.IdFoodInMeal != null)
            {
                bl.DeleteOneFoodInMeal(bl.FoodInMeal);

                // Remove from business layer list
                if (bl.FoodsInMeal != null)
                {
                    bl.FoodsInMeal.RemoveAll(f => f.IdFoodInMeal == bl.FoodInMeal.IdFoodInMeal);
                }

                // Update the ObservableCollection for UI binding
                //UpdateFoodsInMealCollection();

                bl.RecalcAll();
                FromClassToBoxesFoodInMeal();
            }
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - btnRemoveFoodInMeal_Click", ex);
        }
    }
    private async void btnSearchFoodInMeal_Click(object sender, EventArgs e)
    {
        try
        {
            // Get current data from UI
            FromBoxesFoodInMealToClass();

            // Open the search page passing current FoodInMeal
            var searchPage = new FoodsInMealSearchResultsPage(bl.FoodInMeal);
            await Navigation.PushModalAsync(searchPage);

            // Wait for the page to be closed and get the result
            FoodInMeal? result = await searchPage.PageClosedTask;

            // Check if the user chose a food (result is not null)
            if (result != null)
            {
                // Update only Name and CarbohydratesPercent from the search result
                bl.FoodInMeal.Name = result.Name;
                bl.FoodInMeal.CarbohydratesPercent = result.CarbohydratesPercent;

                // Recalculate the carbohydrates in grams of this FoodInMeal
                bl.CalculateChoOfFoodGrams();

                // Update the user interface with the new Data
                FromClassToBoxesFoodInMeal();

                // Recalculate all values
                bl.RecalcAll();

                // Update the meal UI
                RefreshMeal();

                General.LogOfProgram?.Event($"Food search completed: Name={result.Name}, CHO%={result.CarbohydratesPercent?.Double ?? 0}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnSearchFoodInMeal_Click", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }
    private async void btnFoods_ClickAsync(object sender, EventArgs e)
    {
        if (txtFoodInMealName.Text == null || txtFoodInMealName.Text == "")
        {
            bl.FoodInMeal = new FoodInMeal();
        }
        else
        {
            FromBoxesFoodInMealToClass();
        }
        foodsPage = new FoodsPage(bl.FoodInMeal);
        await Navigation.PushModalAsync(foodsPage);
        // Wait for the page to be closed and get the result
        bool foodWasChosen = await foodsPage.PageClosedTask;
        if (foodWasChosen && foodsPage.FoodIsChosen)
        {
            bool newFoodIsDifferent = foodsPage.Food.IdFood != bl.FoodInMeal.IdFood;
            // Update the current FoodInMeal with the Food chosen from the called page
            bl.FromFoodToFoodInMeal(foodsPage.Food, bl.FoodInMeal);
            // check if this food in meal is the same that is coming from the Food page
            if (newFoodIsDifferent)
            {
                // the chosen food is different, 
                bl.FoodInMeal.QuantityInUnits.Double = 0;
            }
            // recalculate the carbohydrates in grams of this FoodInMeal
            bl.CalculateChoOfFoodGrams();
            // Update the user interface with the new Data
            FromClassToBoxesFoodInMeal();
            // Recalculate all values
            bl.RecalcAll();
            // Update the meal UI
            RefreshMeal();
        }
    }
    private async void btnRecipes_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            // Ensure FoodInMeal is initialized
            if (bl.FoodInMeal == null)
            {
                bl.FoodInMeal = new FoodInMeal();
            }

            recipesPage = new RecipesPage(null);
            await Navigation.PushAsync(recipesPage);

            // Wait for the page to be closed and get the result
            bool recipeWasChosen = await recipesPage.PageClosedTask;

            // Check if the user chose a recipe in called page
            if (recipeWasChosen && recipesPage.RecipeIsChosen && recipesPage.CurrentRecipe != null)
            {
                // Update the current FoodInMeal with the Recipe Data
                bl.FoodInMeal.Name = recipesPage.CurrentRecipe.Name;

                // Import CHO% from recipe
                if (recipesPage.CurrentRecipe.CarbohydratesPercent != null &&
                   recipesPage.CurrentRecipe.CarbohydratesPercent.Double.HasValue)
                {
                    bl.FoodInMeal.CarbohydratesPercent.Double = recipesPage.CurrentRecipe.CarbohydratesPercent.Double;
                    bl.FoodInMeal.CarbohydratesPercent.Text = recipesPage.CurrentRecipe.CarbohydratesPercent.Text;
                }

                // Initialize QuantityInUnits to 0 for a new recipe
                bl.FoodInMeal.QuantityInUnits.Double = 0;
                bl.FoodInMeal.QuantityInUnits.Text = "0";

                // Set unit to grams
                bl.FoodInMeal.UnitSymbol = "g";
                bl.FoodInMeal.GramsInOneUnit.Double = 1;

                // Recalculate the carbohydrates in grams
                bl.CalculateChoOfFoodGrams();

                // Update the user interface with the new Data
                FromClassToBoxesFoodInMeal();

                // Recalculate all values
                bl.RecalcAll();

                // Update the meal UI
                RefreshMeal();

                General.LogOfProgram?.Event($"Recipe imported: Name={recipesPage.CurrentRecipe.Name}, CHO%={recipesPage.CurrentRecipe.CarbohydratesPercent?.Double ?? 0}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnRecipes_ClickAsync", ex);
            await DisplayAlert(AppStrings.Error, string.Format(AppStrings.FailedToImportRecipe, ex.Message), AppStrings.OK);
        }
    }
    private void btnDefaults_Click(object sender, EventArgs e)
    {
        try
        {

            txtFoodInMealName.Text = "";
            txtAccuracyOfChoFoodInMeal.Text = "";
            cmbAccuracyFoodInMeal.SelectedItem = null;
            txtFoodCarbohydratesPerUnit.Text = "";
            txtFoodQuantityInUnits.Text = "";
            txtFoodCarbohydratesGrams.Text = "";
            btnUnit.Text = "g";
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - btnDefaults_Click", ex);
        }
    }
    private void btnCalc_Click(object sender, EventArgs e)
    {
        try
        {
            // take the Data from the UI controls and put it into the business layer class
            FromBoxesFoodInMealToClass();
            bl.UpdateOldFoodInMealInList();
            // Refresh the bound UI Data related to the Meal, since it has changed
            if (mealSection != null && bl?.Meal != null)
            {
                mealSection.BindingContext = null;
                mealSection.BindingContext = bl.Meal;
            }
            bl.RecalcAll();
            RefreshMeal();
            // Also refresh the visualization of the grid
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnCalc_Click", ex);
            DisplayAlert(AppStrings.Error, AppStrings.FailedToCalculateTotals, AppStrings.OK);
        }
    }
    private async void btnInsulinCalc_ClickAsync(object sender, EventArgs e)
    {
        //insulinCalcPage = new InsulinCalcPage(blMeal.Meal.IdBolusCalculation);
        insulinCalcPage = new InsulinCalcPage();
        await Navigation.PushAsync(insulinCalcPage);
    }
    private async void btnGlucose_ClickAsync(object sender, EventArgs e)
    {
        measurementPage = new GlucoseMeasurementsPage(bl.Meal.IdGlucoseRecord);
        await Navigation.PushAsync(measurementPage);
    }
    private async void btnWeighFood_Click(object sender, EventArgs e)
    {
        try
        {
            // Update the current food from UI before opening WeighFoodPage
            FromBoxesFoodInMealToClass();

            // Open WeighFoodPage with current food Data
            var weighFoodPage = new WeighFoodPage(bl.FoodInMeal);
            await Navigation.PushModalAsync(weighFoodPage);

            // Wait for the page to be closed and get the result
            bool dataWasModified = await weighFoodPage.PageClosedTask;

            // Check if the user modified food Data in the WeighFoodPage
            if (dataWasModified)
            {
                // Update food name from WeighFoodPage
                if (!string.IsNullOrEmpty(weighFoodPage.FoodName))
                {
                    bl.FoodInMeal.Name = weighFoodPage.FoodName;
                }
                
                // Update CHO% and weight
                bl.FoodInMeal.CarbohydratesPercent = weighFoodPage.ResultCarbohydratesPercent;
                bl.FoodInMeal.QuantityInUnits = weighFoodPage.ResultWeightOfPortion;

                // Recalculate the carbohydrates in grams of this FoodInMeal
                bl.CalculateChoOfFoodGrams();

                // Update the user interface with the new Data
                FromClassToBoxesFoodInMeal();

                // Recalculate all values
                bl.RecalcAll();

                // Update the meal UI
                RefreshMeal();

                General.LogOfProgram?.Event($"Food data updated from WeighFoodPage: Name={bl.FoodInMeal.Name}, CHO%={bl.FoodInMeal.CarbohydratesPercent?.Double:F1}, Weight={bl.FoodInMeal.QuantityInUnits?.Double:F1}g");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnWeighFood_Click", ex);
            await DisplayAlert(AppStrings.Error, AppStrings.FailedToOpenWeighFood, AppStrings.OK);
        }
    }
    private async void btnFoodCalcAsync_Click(object sender, EventArgs e)
    {
        // update the Data from the record modified
        FromBoxesFoodInMealToClass();
        bl.UpdateOldFoodInMealInList();
        // save the parameters that have to be read by the page we are opening
        BlGeneral.SaveParameter("Hit_ChoAlreadyTaken", bl.Meal.CarbohydratesGrams.Text);
        BlGeneral.SaveParameter("Hit_ChoOfFood", bl.FoodInMeal.CarbohydratesPercent.Text);
        BlGeneral.SaveParameter("Hit_NameOfFood", bl.FoodInMeal.Name);
        await Navigation.PushAsync(new FoodToHitTargetCarbsPage());
    }
    private async void btnInjection_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            injectionsPage = new InjectionsPage(bl.Meal.IdInjection);
            await Navigation.PushAsync(injectionsPage);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnInjection_ClickAsync", ex);
            DisplayAlert(AppStrings.Error, AppStrings.FailedToOpenInjections, AppStrings.OK);
        }
    }
    private async void btnStartMeal_Click(object sender, EventArgs e)
    {
        //FromUiToClasses();
        if (bl.Meal.IdMeal != null)
            bl.SaveOneMeal(bl.Meal, true); // saves with time now 
        btnStartMeal.BackgroundColor = initialButtonBackground;
        btnStartMeal.TextColor = initialButtonTextColor;
        btnStartMeal.ImageSource = "chronograph_started.png";
        RefreshUi();
    }
    private void foodSection_Unfocused(object sender, FocusEventArgs e)
    {
        // when finished with the current food, update the Data in the blMeal 
        // and show the changes

        // update blMeal.FoodInMeal from the UI controls
        FromBoxesFoodInMealToClass();
        bl.RecalcAll();
        bl.SaveAllFoodsInMeal();
        // Refresh the bound UI Data related to the Meal, since it has changed
        if (mealSection != null && bl?.Meal != null)
        {
            mealSection.BindingContext = null;
            mealSection.BindingContext = bl.Meal;
        }
    }
    private void gridFoodsInMeal_SelectionChanged(object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
        {
            return;
        }
        try
        {
            var selectedFood = (FoodInMeal)e.CurrentSelection[0];

            if (selectedFood != bl.FoodInMeal)
            {
                // Deseleziona il precedente FoodInMeal
                if (bl.FoodInMeal?.Name != null)
                {
                    bl.FoodInMeal.IsSelectedInList = false;
                    FromBoxesFoodInMealToClass();
                    bl.UpdateOldFoodInMealInList();
                }
                // Update the current FoodInMeal 
                bl.FoodInMeal = selectedFood;
                FromClassToBoxesFoodInMeal();
                UpdateMealTotalChoInUI();
            }

            // Deseleziona tutti gli altri elementi nella lista
            if (bl.FoodsInMeal != null)
            {
                foreach (var food in bl.FoodsInMeal)
                {
                    food.IsSelectedInList = false;
                }
            }

            // Seleziona l'elemento corrente
            selectedFood.IsSelectedInList = true;

            // Mantieni la selezione visibile
            if (gridFoodsInMeal.SelectedItem != selectedFood)
            {
                gridFoodsInMeal.SelectedItem = selectedFood;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - gridFoodsInMeal_SelectionChanged", ex);
        }
    }
    // Support a direct tap on the item Frame to set selection (helps on Android)
    private void OnItemTapped(object? sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is FoodInMeal tapped)
        {
            gridFoodsInMeal.SelectedItem = tapped;
            try
            {
                var selectedFood = tapped;

                if (selectedFood != bl.FoodInMeal)
                {
                    // Deselect previous FoodInMeal
                    if (bl.FoodInMeal?.Name != null)
                    {
                        bl.FoodInMeal.IsSelectedInList = false;
                        FromBoxesFoodInMealToClass();
                        bl.UpdateOldFoodInMealInList();
                    }

                    // Update the current FoodInMeal
                    bl.FoodInMeal = selectedFood;
                    FromClassToBoxesFoodInMeal();
                    UpdateMealTotalChoInUI();
                }
                // Deselect all other items in the list
                if (bl.FoodsInMeal != null)
                {
                    foreach (var food in bl.FoodsInMeal)
                    {
                        food.IsSelectedInList = false;
                    }
                }

                // Select the current item
                selectedFood.IsSelectedInList = true;
            }
            catch
            {
                // ignore any errors from manual invocation
            }
        }
    }
    private void txtFoodCarbohydratesGrams_TextChanged(object sender, TextChangedEventArgs e)
    {
        foodInMealChoGramsChanging = true;
        if (txtFoodCarbohydratesGrams.IsLoaded && !foodInMealModifications && !foodInMealPercentOrQuantityChanging)
        {
            // the user is changing manually
            programmaticModification = false;

            bl.UpdateDataAfterChoGramsChange(txtFoodCarbohydratesGrams.Text);
                    txtFoodCarbohydratesPerUnit.Text = "";
                    txtFoodQuantityInUnits.Text = "";
                    txtMealCarbohydratesGrams.Text = bl.Meal.CarbohydratesGrams.Text;
            //FromClassToBoxesFoodInMeal();
            programmaticModification = true;
        }
        foodInMealChoGramsChanging = false;
    }
    private void txtFoodChoOrQuantity_TextChanged(object sender, TextChangedEventArgs e)
    {
        foodInMealPercentOrQuantityChanging = true;
        if (txtFoodCarbohydratesPerUnit.IsLoaded && !foodInMealModifications)
        {
            // the user is changing manually
            programmaticModification = false;

            // aggiorna blMeal.FoodInMeal con i dati dell'interfaccia
            FromBoxesFoodInMealToClass();
            // ricalcola il totale dei carboidrati della voce del cibo corrente
            bl.CalculateChoOfFoodGrams();
            // aggiorna solo la visualizzazione dei grammi di carboidrati
            txtFoodCarbohydratesGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
            // aggiorna istantaneamente il totale CHO del pasto
            UpdateMealTotalChoInUI();

            programmaticModification = true;
        }
        foodInMealPercentOrQuantityChanging = false;
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
    // In MAUI Shell, OnBackButtonPressed intercepts both the hardware back button (Android)
    // and the Shell navigation bar back button on all platforms.
    // DisplayAlert cannot be used from OnDisappearing because the page is no longer the active presenter.
    protected override bool OnBackButtonPressed()
    {
        if (_backNavigationInProgress) return true;
        // Avoid locale-sensitive text parsing when the user has not typed manually:
        // use the business-object Double directly (no CultureInfo issues).
        // Only fall back to text parsing when the user has manually edited the field.
        double? currentCho = _choManuallyModified
            ? (Safe.Double(txtMealCarbohydratesGrams.Text) ?? bl.Meal?.CarbohydratesGrams?.Double)
            : bl.Meal?.CarbohydratesGrams?.Double;
        bool choChanged = Math.Abs((currentCho ?? 0) - (_storedCho ?? 0)) > 0.01;
        if (choChanged && bl.Meal?.IdMeal != null)
        {
            _backNavigationInProgress = true;
            _ = HandleChoChangeOnBackAsync(currentCho);
            return true;
        }
        return false;
    }
    private async Task HandleChoChangeOnBackAsync(double? currentCho)
    {
        try
        {
            string message = string.Format(AppStrings.ChoChangedOnExitMessage,
                _storedCho?.ToString("0.0") ?? "—",
                currentCho?.ToString("0.0") ?? "—");
            bool saveNew = await DisplayAlert(
                AppStrings.UnsavedChanges,
                message,
                AppStrings.SaveNewValue,
                AppStrings.KeepStoredValue);
            if (saveNew)
            {
                bl.Meal.CarbohydratesGrams.Double = currentCho;
                SaveOrCreateMealData();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - HandleChoChangeOnBackAsync", ex);
        }
        finally
        {
            _backNavigationInProgress = false;
            await Navigation.PopAsync();
        }
    }
    private void txtMealCarbohydratesGrams_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (txtMealCarbohydratesGrams.IsLoaded && !_programmaticChoUpdate)
        {
            _choManuallyModified = true;
        }
    }
    private void txtAccuracyOfChoMeal_TextChanged(object sender, TextChangedEventArgs e)
    {
        // The UiAccuracy class handles all synchronization internally
        // Do not interfere with its operation
    }
    private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Let UiAccuracy handle the text box update, we only update the Data model
        try
        {
            if (!cmbAccuracyMeal.IsLoaded && bl.Meal != null && cmbAccuracyMeal.SelectedItem != null)
            {
                var selectedAccuracy = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
                double numericValue = (double)selectedAccuracy;

                // Update the meal's accuracy in the Data model
                bl.Meal.AccuracyOfChoEstimate.Double = numericValue;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - cmbAccuracyMeal_SelectedIndexChanged", ex);
        }
    }
    private void cmbAccuracyFoodInMeal_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Let UiAccuracy handle the text box update, we only update the Data model
        try
        {
            if (!cmbAccuracyFoodInMeal.IsLoaded && bl.FoodInMeal != null && cmbAccuracyFoodInMeal.SelectedItem != null)
            {
                // Update the food's accuracy in the Data model
                bl.FoodInMeal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text);
                // Recalculate meal accuracy since food accuracy changed
                bl.RecalcAll();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - cmbAccuracyFoodInMeal_SelectedIndexChanged", ex);
        }
    }
    private async void Calculator_Click(object sender, TappedEventArgs e)
    {
        var focusedEntry = GetFocusedEntry();
        string sValue = focusedEntry?.Text ?? "0";
        double dValue = double.TryParse(sValue, out var val) ? val : 0;

        var calculator = new CalculatorPage(dValue);
        await Navigation.PushModalAsync(calculator);
        var result = await calculator.ResultSource.Task;

        if (result.HasValue)
        {
            if (focusedEntry == txtMealCarbohydratesGrams)
            {
                txtMealCarbohydratesGrams.Text = result.Value.ToString();
            }
            else if (focusedEntry == txtFoodCarbohydratesPerUnit)
            {
                txtFoodCarbohydratesPerUnit.Text = result.Value.ToString();
            }
            else if (focusedEntry == txtFoodQuantityInUnits)
            {
                txtFoodQuantityInUnits.Text = result.Value.ToString();
            }
            else if (focusedEntry == txtFoodCarbohydratesGrams)
            {
                txtFoodCarbohydratesGrams.Text = result.Value.ToString();
            }
            FromClassToBoxesFoodInMeal();
        }
    }
    private Entry GetFocusedEntry()
    {
        if (txtFoodQuantityInUnits.IsFocused) return txtFoodQuantityInUnits;
        if (txtFoodCarbohydratesPerUnit.IsFocused) return txtFoodCarbohydratesPerUnit;
        if (txtFoodCarbohydratesGrams.IsFocused) return txtFoodCarbohydratesGrams;
        if (txtAccuracyOfChoFoodInMeal.IsFocused) return txtAccuracyOfChoFoodInMeal;
        if (txtMealCarbohydratesGrams.IsFocused) return txtMealCarbohydratesGrams;
        if (txtAccuracyOfChoMeal.IsFocused) return txtAccuracyOfChoMeal;
        return null;
    }
    private async void ResetUnitToGrams(object sender, EventArgs e)
    {
        try
        {
            bl.FoodInMeal.UnitSymbol = "g";
            bl.FoodInMeal.GramsInOneUnit.Double = 1;
            RefreshUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - ResetUnitToGrams", ex);
        }
    }
    #endregion
}