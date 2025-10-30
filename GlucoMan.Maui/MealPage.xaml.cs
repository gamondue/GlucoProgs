using gamon;
using GlucoMan.BusinessLayer;
using System.ComponentModel;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class MealPage : ContentPage, INotifyPropertyChanged
{
    // since it is accessed by several pages, we use a common business
    // layer beetween different pages
    private BL_MealAndFood bl = Common.MealAndFood_CommonBL;

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
    private bool foodInMealChoGramsChanging = false ;
    private bool programmaticModification = true;

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

        if (bl.Meal.IdMeal == null || (bl.Meal.EventTime.DateTime + new TimeSpan(0, 15, 0) > DateTime.Now))
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
            // formatted visulization for program's modifications
            txtAccuracyOfChoFoodInMeal.Text = bl.FoodInMeal.AccuracyOfChoEstimate.Text;
            txtFoodCarbohydratesPerUnit.Text = bl.FoodInMeal.CarbohydratesPercent.Text;
            txtFoodQuantityInUnits.Text = bl.FoodInMeal.QuantityInUnits.Text;
            txtFoodCarbohydratesGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
            Common.SetCursorToStart(txtFoodInMealName);
        }
        foodInMealModifications = false;
    }
    private void FromBoxesFoodInMealToClass()
    {
        //bl.FoodInMeal.IdFoodInMeal = Safe.Int(txtIdFoodInMeal.Text);
        bl.FoodInMeal.Name = Safe.String(txtFoodInMealName.Text);
        bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
        bl.FoodInMeal.CarbohydratesPercent.Text = txtFoodCarbohydratesPerUnit.Text;
        // in this page the unit is read only, taken from the FoodInMeal object
        bl.FoodInMeal.QuantityInUnits.Text = txtFoodQuantityInUnits.Text;
        bl.FoodInMeal.CarbohydratesGrams.Text = txtFoodCarbohydratesGrams.Text;
        bl.FoodInMeal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoFoodInMeal.Text;
    }
    #endregion
    #region controls' events    
    private async void btnSaveAllMeal_Click(object sender, EventArgs e)
    {
        try
        {
            // Ensure the food is associated with the current meal (not necessary)
            bl.FoodInMeal.IdMeal = bl.Meal.IdMeal;
            // update the data from the FoodInMeal boxes
            FromBoxesFoodInMealToClass();
            // also update the list of the foods in the meal
            bl.UpdateOldFoodInMealInList();
            // Save all the foods in the meal
            bl.SaveAllFoodsInMeal();

            // check if the totals are updated and ask if the user wants to save the 
            // original data or the updated
            double? originalCho = Safe.Double(txtMealCarbohydratesGrams.Text);
            double? originalAccuracy = Safe.Double(txtAccuracyOfChoMeal.Text);
            
            bl.RecalcTotalCho();
            bl.RecalcTotalAccuracy();
            
            double? calculatedCho = bl.Meal.CarbohydratesGrams.Double;
            double? calculatedAccuracy = bl.Meal.AccuracyOfChoEstimate.Double;
            
            bool choChanged = Math.Abs((originalCho ?? 0) - (calculatedCho ?? 0)) > 0.01;
            bool accuracyChanged = Math.Abs((originalAccuracy ?? 0) - (calculatedAccuracy ?? 0)) > 0.01;
            
            if ((bl.FoodsInMeal != null && bl.FoodsInMeal.Count != 0) &&
                (choChanged || accuracyChanged))
            {
                // ask the user if he wants to save old or new data
                bool useCalculatedValues = await DisplayAlert(
                    "Value discrepancy", 
                    "The value of total Carbohydrates and/or accuracy are different from those calculated with the single foods." +
                    "\nShould we save the displayed values or those calculated?",
                    "Use Calculated", 
                    "Keep Displayed");
                    
                if (!useCalculatedValues)
                {
                    // Restore the original values from UI
                    bl.Meal.CarbohydratesGrams.Double = originalCho;
                    bl.Meal.AccuracyOfChoEstimate.Double = originalAccuracy;
                }
                // If useCalculatedValues is true, keep the calculated values already in bl.Meal
            }
            else
            {
                // Restore the original values from UI
                bl.Meal.CarbohydratesGrams.Double = originalCho;
                bl.Meal.AccuracyOfChoEstimate.Double = originalAccuracy;
            }
            SaveOrCreateMealData();
            RefreshUi();            
            General.LogOfProgram?.Event("Meal saved successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealPage - btnSaveAllMeal_Click", ex);
            await DisplayAlert("Error", "Failed to save meal data. This might be due to database connectivity issues. Check logs for details.", "OK");
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
                DisplayAlert("Error", "Failed to add food to meal", "OK");
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
        
        // check if the user chose a food in called page
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
            // Update the user interface with the new data
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
   // Update the current FoodInMeal with the Recipe data
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
   
   // Update the user interface with the new data
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
  await DisplayAlert("Error", $"Failed to import recipe: {ex.Message}", "OK");
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
            // take the data from the UI controls and put it into the business layer class
            FromBoxesFoodInMealToClass();
            bl.UpdateOldFoodInMealInList();
            // Refresh the bound UI data related to the Meal, since it has changed
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
            DisplayAlert("Error", "Failed to calculate meal totals. Check logs for details.", "OK");
        }
    }
    private async void btnInsulinCalc_ClickAsync(object sender, EventArgs e)
    {
        //insulinCalcPage = new InsulinCalcPage(bl.Meal.IdBolusCalculation);
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
         
            // Open WeighFoodPage with current food data
            var weighFoodPage = new WeighFoodPage(bl.FoodInMeal);
            await Navigation.PushModalAsync(weighFoodPage);
         
            // Wait for the page to be closed and get the result
            bool dataWasModified = await weighFoodPage.PageClosedTask;
            
  // Check if the user modified food data in the WeighFoodPage
 if (dataWasModified && weighFoodPage.ResultFood != null)
  {
        // Update the current FoodInMeal with the modified Food data
    bl.FromFoodToFoodInMeal(weighFoodPage.ResultFood, bl.FoodInMeal);
       
       // Set the QuantityInUnits with the WeightOfPortion from WeighFoodPage
      // This updates the "Quantity" field in the MealPage
         if (weighFoodPage.WeightOfPortion > 0)
     {
     bl.FoodInMeal.QuantityInUnits.Double = weighFoodPage.WeightOfPortion;
     bl.FoodInMeal.QuantityInUnits.Text = weighFoodPage.WeightOfPortion.ToString("F1");
 General.LogOfProgram?.Event($"MealPage - Quantity set to {weighFoodPage.WeightOfPortion:F1}g from WeighFoodPage");
         }
    
           // Recalculate the carbohydrates in grams of this FoodInMeal
           bl.CalculateChoOfFoodGrams();
        
  // Update the user interface with the new data
         FromClassToBoxesFoodInMeal();
                
   // Recalculate all values
     bl.RecalcAll();
       
      // Update the meal UI
           RefreshMeal();
                
      General.LogOfProgram?.Event("Food data updated from WeighFoodPage successfully");
          }
        }
        catch (Exception ex)
        {
    General.LogOfProgram?.Error("MealPage - btnWeighFood_Click", ex);
       await DisplayAlert("Error", "Failed to open weigh food page. Check logs for details.", "OK");
        }
    }
    private async void btnFoodCalc_ClickAsync(object sender, EventArgs e)
    {
        // update the data from the record modified
        FromBoxesFoodInMealToClass();
        bl.UpdateOldFoodInMealInList();
        // save the parameters that have to be read by the page we are opening
        Common.BlGeneral.SaveParameter("Hit_ChoAlreadyTaken", bl.Meal.CarbohydratesGrams.Text);
        Common.BlGeneral.SaveParameter("Hit_ChoOfFood", bl.FoodInMeal.CarbohydratesPercent.Text);
        Common.BlGeneral.SaveParameter("Hit_NameOfFood", bl.FoodInMeal.Name);
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
            DisplayAlert("Error", "Failed to open injections page. Check logs for details.", "OK");
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
        // when finished with the current food, update the data in the bl 
        // and show the changes

        // update bl.FoodInMeal from the UI controls
        FromBoxesFoodInMealToClass();
        bl.RecalcAll();
        bl.SaveAllFoodsInMeal();
        // Refresh the bound UI data related to the Meal, since it has changed
        if (mealSection != null && bl?.Meal != null)
        {
            mealSection.BindingContext = null;
            mealSection.BindingContext = bl.Meal;
        }
    }
    private async void gridFoodsInMeal_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        } 
        try
        {
            var selectedFood = (FoodInMeal)e.SelectedItem;
            
            if (selectedFood != bl.FoodInMeal)
            {
                if (bl.FoodInMeal?.Name != null)
                {
                    FromBoxesFoodInMealToClass();
                    bl.UpdateOldFoodInMealInList();
                    RefreshGrid();
                }
                bl.FoodInMeal = selectedFood;
                ////////SelectedFoodInMeal = selectedFood;
                FromClassToBoxesFoodInMeal();
            }    
            // Mantieni la selezione visibile
            if (gridFoodsInMeal.SelectedItem != selectedFood)
            {
                gridFoodsInMeal.SelectedItem = selectedFood;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("MealPage - gridFoodsInMeal_ItemSelected", ex);
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
            try
            {
                bl.FoodInMeal.CarbohydratesPercent.Double = Safe.Double(txtFoodCarbohydratesPerUnit.Text);
                // Safe navigation operator to avoid null reference exception
                bl.FoodInMeal.QuantityInUnits.Double = Safe.Double(txtFoodQuantityInUnits.Text);
                bl.CalculateChoOfFoodGrams();
                //bl.UpdateDataAfterQuantityChange(txtFoodCarbohydratesPerUnit.Text,
                //    txtFoodQuantityInUnits.Text);
                txtFoodCarbohydratesGrams.Text = bl.FoodInMeal.CarbohydratesGrams.Text;
                //FromClassToBoxesFoodInMeal();
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("MealPage - txtChoOrQuantity_TextChanged", ex);
            }
            programmaticModification = true;
        }
        foodInMealPercentOrQuantityChanging = false;
    }
    private void txtAccuracyOfChoMeal_TextChanged(object sender, TextChangedEventArgs e)
    {
        // The UiAccuracy class handles all synchronization internally
        // Do not interfere with its operation
    }
    private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Let UiAccuracy handle the text box update, we only update the data model
        try
        {
            if (!cmbAccuracyMeal.IsLoaded && bl.Meal != null && cmbAccuracyMeal.SelectedItem != null)
            {
                var selectedAccuracy = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
                double numericValue = (double)selectedAccuracy;
                
                // Update the meal's accuracy in the data model
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
        // Let UiAccuracy handle the text box update, we only update the data model
        try
        {
            if (!cmbAccuracyFoodInMeal.IsLoaded && bl.FoodInMeal != null && cmbAccuracyFoodInMeal.SelectedItem != null)
            {
                // Update the food's accuracy in the data model
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
        //// save the entries in the classes
        //FromUiToClasses();

        var focusedEntry = GetFocusedEntry();
        string sValue = focusedEntry?.Text ?? "0";
        double dValue = double.TryParse(sValue, out var val) ? val : 0;

        // start the CalculatorPage passing to it the value of the 
        // control that currently has the focus
        var calculator = new CalculatorPage(dValue);
        await Navigation.PushModalAsync(calculator);
        var result = await calculator.ResultSource.Task;

        // check if the page has given back a result
        if (result.HasValue)
        {
            // update the data model
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
            // show the UI starting from the classes
            FromClassToBoxesFoodInMeal();
        }
    }
    #endregion
    private Entry GetFocusedEntry()
    {
        // find the pertinent entry that currently has focus
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
}