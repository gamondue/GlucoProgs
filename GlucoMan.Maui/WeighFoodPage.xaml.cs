using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class WeighFoodPage : ContentPage
{
    BL_WeighFood blFood = new BL_WeighFood();
    BL_GrossTareAndNetWeight blMealRaw;
    BL_GrossTareAndNetWeight blMealCooked;
    
    // Add business layer for food management and properties for Food selection like in MealPage
    private BL_MealAndFood bl = new BL_MealAndFood();
    FoodsPage foodsPage;
    private Food selectedFood;
    
    // Properties to handle data exchange with calling page
    public Food ResultFood { get; private set; }
    public bool FoodDataWasModified { get; private set; } = false;
    public bool UserCancelled { get; private set; } = false;  
    // Properties for RadioButton options
    public bool DivideIntoEqualPortions { get; set; } = false;
    public bool WeighCookedPortion { get; set; } = false;   
    // TaskCompletionSource for modal behavior similar to other pages
    private TaskCompletionSource<bool> pageClosedTaskSource = new TaskCompletionSource<bool>();
    public Task<bool> PageClosedTask => pageClosedTaskSource.Task;
    // Default constructor
    public WeighFoodPage()
    {
        try
        {
            InitializeComponent();
            
            // Initialize selectedFood with default unit BEFORE calling FromClassToUi()
            selectedFood = new Food(new UnitOfFood());
            
            blFood.RestoreData();
            FromClassToUi();

            blMealRaw = new BL_GrossTareAndNetWeight(blFood.T0RawGross, blFood.T0RawTare, blFood.T0RawNet);
            blMealCooked = new BL_GrossTareAndNetWeight(blFood.S1CookedGross, blFood.S1CookedTare, blFood.S1CookedNet);

            // Set default selection to WeighCookedPortion
            WeighCookedPortion = true;
            if (rbWeighCookedPortion != null)
                rbWeighCookedPortion.IsChecked = true;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor", ex);
            // Initialize with safe defaults to prevent further errors
            InitializeSafeDefaults();
        }
    }   
    // Constructor that accepts Food data from MealPage
    public WeighFoodPage(Food initialFood) : this()
    {
        try
        {
            if (initialFood != null)
            {
                selectedFood = initialFood;
                FromClassToUi();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor with Food", ex);
            InitializeSafeDefaults();
        }
    }
    // Constructor that accepts FoodInMeal data from MealPage
    public WeighFoodPage(FoodInMeal initialFoodInMeal) : this()
    {
        try
        {
            if (initialFoodInMeal != null)
            {
                // Convert FoodInMeal to Food for internal use
                selectedFood = new Food(new UnitOfFood());
                bl.FromFoodInMealToFood(initialFoodInMeal, selectedFood);
                FromClassToUi();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor with FoodInMeal", ex);
            InitializeSafeDefaults();
        }
    }
    private void InitializeSafeDefaults()
    {
        try
        {
            selectedFood ??= new Food(new UnitOfFood());
            blMealRaw ??= new BL_GrossTareAndNetWeight(blFood?.T0RawGross, blFood?.T0RawTare, blFood?.T0RawNet);
            blMealCooked ??= new BL_GrossTareAndNetWeight(blFood?.S1CookedGross, blFood?.S1CookedTare, blFood?.S1CookedNet);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - InitializeSafeDefaults", ex);
        }
    }
    protected override void OnDisappearing()
    {
        try
        {
            base.OnDisappearing();
            
            // Only update result food if user didn't cancel
            if (!UserCancelled)
            {
                FromUiToClass();
                ResultFood = selectedFood;
            }
            
            // Complete the task when the page is closed
            if (!pageClosedTaskSource.Task.IsCompleted)
            {
                pageClosedTaskSource.SetResult(FoodDataWasModified && !UserCancelled);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - OnDisappearing", ex);
            // Ensure task is completed even if there's an error
            if (!pageClosedTaskSource.Task.IsCompleted)
            {
                pageClosedTaskSource.SetResult(false);
            }
        }
    }
    // Back button click handler - cancels changes and returns to calling page
    private async void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            UserCancelled = true;
            FoodDataWasModified = false;
            
            // Log the action
            General.LogOfProgram?.Event("WeighFoodPage - User clicked Back button, changes cancelled");
            
            // Close the page and return to the calling page
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnBack_Click", ex);
            try
            {
                await DisplayAlert("Error", "An error occurred while going back. Please try again.", "OK");
            }
            catch
            {
                // If even DisplayAlert fails, just log it
                General.LogOfProgram?.Error("WeighFoodPage - btnBack_Click - DisplayAlert failed", ex);
            }
        }
    }
    // Choose button click handler - saves changes and returns to calling page
    private async void btnChoose_Click(object sender, EventArgs e)
    {
        try
        {
            // Update all data from UI to class before returning
            FromUiToClass();
            
            // Perform final calculation
            if (blFood != null)
            {
                blFood.CalcUnknownData();
                blFood.SaveData();
            }
            
            // Set result data
            ResultFood = selectedFood;
            FoodDataWasModified = true;
            UserCancelled = false;
            
            // Log the action
            General.LogOfProgram?.Event("WeighFoodPage - User clicked Choose button, changes saved");
            
            // Close the page and return to the calling page
            await Navigation.PopModalAsync();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnChoose_Click", ex);
            try
            {
                await DisplayAlert("Error", "An error occurred while saving changes. Please try again.", "OK");
            }
            catch
            {
                // If even DisplayAlert fails, just log it
                General.LogOfProgram?.Error("WeighFoodPage - btnChoose_Click - DisplayAlert failed", ex);
            }
        }
    }
    // Radio Button event handlers for weighing options
    private void OnDividePortionsCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (e.Value) // RadioButton is checked
            {
                DivideIntoEqualPortions = true;
                WeighCookedPortion = false;
                General.LogOfProgram?.Event("WeighFoodPage - Divide into equal portions option selected");

                if (SectionWeightOfCookedPortion != null) SectionWeightOfCookedPortion.IsVisible = false;
                if (SectionNumberCookedPortion != null) SectionNumberCookedPortion.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - OnDividePortionsCheckedChanged", ex);
        }
    }
    private void OnWeighCookedPortionCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            if (e.Value) // RadioButton is checked
            {
                DivideIntoEqualPortions = false;
                WeighCookedPortion = true;
                General.LogOfProgram?.Event("WeighFoodPage - Weigh cooked portion option selected");

                if (SectionWeightOfCookedPortion != null) SectionWeightOfCookedPortion.IsVisible = true;
                if (SectionNumberCookedPortion != null) SectionNumberCookedPortion.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - OnWeighCookedPortionCheckedChanged", ex);
        }
    }
    // Properties to access RadioButton states
    public bool IsDivideIntoEqualPortionsSelected => rbDivideIntoEqualPortions?.IsChecked ?? false;
    public bool IsWeighCookedPortionSelected => rbWeighCookedPortion?.IsChecked ?? false;
    internal void FromUiToClass()
    {
        try
        {
            if (blFood == null) return;

            blFood.M0RawGross.Text = TxtM0RawGross?.Text ?? "";
            blFood.M0RawTare.Text = TxtM0RawTare?.Text ?? "";
            blFood.M0RawNet.Text = TxtM0RawNet?.Text ?? "";

            blFood.S1CookedGross.Text = TxtS1CookedGross?.Text ?? "";
            blFood.S1CookedTare.Text = TxtS1CookedTare?.Text ?? "";
            blFood.S1CookedNet.Text = TxtS1CookedNet?.Text ?? "";

            // Update food data from UI
            if (selectedFood != null)
            {
                selectedFood.Name = txtFoodName?.Text ?? "";
                if (selectedFood.CarbohydratesPercent != null)
                    selectedFood.CarbohydratesPercent.Text = txtFoodCarbohydratesPerUnit?.Text ?? "";
                selectedFood.UnitSymbol = btnUnit?.Text ?? "g";
            }

            // Update RadioButton states from UI controls
            DivideIntoEqualPortions = rbDivideIntoEqualPortions?.IsChecked ?? false;
            WeighCookedPortion = rbWeighCookedPortion?.IsChecked ?? false;

            // Mark that data was modified only if not cancelled
            if (!UserCancelled)
            {
                FoodDataWasModified = true;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - FromUiToClass", ex);
        }
    }
    internal void FromClassToUi()
    {
        try
        {
            if (blFood == null) return;

            if (TxtM0RawGross != null) TxtM0RawGross.Text = blFood.T0RawGross?.Text ?? "";
            if (TxtM0RawTare != null) TxtM0RawTare.Text = blFood.T0RawTare?.Text ?? "";
            if (TxtM0RawNet != null) TxtM0RawNet.Text = blFood.T0RawNet?.Text ?? "";

            if (TxtS1CookedGross != null) TxtS1CookedGross.Text = blFood.S1CookedGross?.Text ?? "";
            if (TxtS1CookedTare != null) TxtS1CookedTare.Text = blFood.S1CookedTare?.Text ?? "";
            if (TxtS1CookedNet != null) TxtS1CookedNet.Text = blFood.S1CookedNet?.Text ?? "";
            
            // Update UI from food data
            if (selectedFood != null)
            {
                if (txtFoodName != null) txtFoodName.Text = selectedFood.Name ?? "";
                if (txtFoodCarbohydratesPerUnit != null) 
                    txtFoodCarbohydratesPerUnit.Text = selectedFood.CarbohydratesPercent?.Text ?? "";
                if (btnUnit != null) btnUnit.Text = selectedFood.UnitSymbol ?? "g";
            }
            
            // Update UI RadioButton states from properties
            if (rbDivideIntoEqualPortions != null) 
                rbDivideIntoEqualPortions.IsChecked = DivideIntoEqualPortions;
            if (rbWeighCookedPortion != null) 
                rbWeighCookedPortion.IsChecked = WeighCookedPortion;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - FromClassToUi", ex);
        }
    }
    private void btnCalc_Click(object sender, EventArgs e)
    {
        try
        {
            if (blFood == null) return;

            FromUiToClass();
            blFood.CalcUnknownData();
            FromClassToUi();

            blFood.SaveData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnCalc_Click", ex);
        }
    }  
    private void TxtM0RawGross_Leave(object sender, FocusEventArgs e)
    {
        try
        {
            if (blFood?.T0RawGross == null || blMealRaw == null) return;

            blFood.T0RawGross.Text = TxtM0RawGross?.Text ?? "";
            blMealRaw.GrossOrTareChanged();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtM0RawGross_Leave", ex);
        }
    }  
    private void TxtS1pPotCookedPlusPot_Leave(object sender, EventArgs e)
    {
        try
        {
            if (blFood == null) return;

            FromUiToClass();
            if (!string.IsNullOrEmpty(blFood.T0RawTare?.Text))
                blFood.S1CookedNet.Double = blFood.S1pPotCookedPlusPot.Double - blFood.T0RawTare.Double;
            
            blFood.T0RawGross.Double = blFood.T0RawGross.Double + blFood.S1CookedNet.Double + blFood.T0RawTare.Double;

            blFood.CalcUnknownData();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtS1pPotCookedPlusPot_Leave", ex);
        }
    } 
    private void TxtM0RawTare_Leave(object sender, FocusEventArgs e)
    {
        try
        {
            if (blFood?.T0RawTare == null || blMealRaw == null) return;

            blFood.T0RawTare.Text = TxtM0RawTare?.Text ?? "";
            blMealRaw.GrossOrTareChanged();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtM0RawTare_Leave", ex);
        }
    }
    private void TxtM0RawNet_Leave(object sender, FocusEventArgs e)
    {
        try
        {
            if (blFood?.T0RawNet == null || blMealRaw == null) return;

            blFood.T0RawNet.Text = TxtM0RawNet?.Text ?? "";
            blMealRaw.NetWeightChanged();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtM0RawNet_Leave", ex);
        }
    }
    private void TxtS1CookedGross_Leave(object sender, FocusEventArgs e)
    {
        try
        {
            if (blFood?.S1CookedGross == null || blMealCooked == null) return;

            blFood.S1CookedGross.Text = TxtS1CookedGross?.Text ?? "";
            blMealCooked.GrossOrTareChanged();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtS1CookedGross_Leave", ex);
        }
    }
    private void TxtS1CookedTare_Leave(object sender, FocusEventArgs e)
    {
        try
        {
            if (blFood?.S1CookedTare == null || blMealCooked == null) return;

            blFood.S1CookedTare.Text = TxtS1CookedTare?.Text ?? "";
            blMealCooked.GrossOrTareChanged();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtS1CookedTare_Leave", ex);
        }
    }
    private void TxtS1CookedNet_Leave(object sender, FocusEventArgs e)
    {
        try
        {
            if (blFood?.S1CookedNet == null || blMealCooked == null) return;

            blFood.S1CookedNet.Text = TxtS1CookedNet?.Text ?? "";
            blMealCooked.NetWeightChanged();
            FromClassToUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtS1CookedNet_Leave", ex);
        }
    }   
    private async void Calculator_Click(object sender, TappedEventArgs e)
    {
        try
        {
            // Save the entries in the classes
            FromUiToClass();

            var focusedEntry = GetFocusedEntry();
            
            // If no entry is focused, try to use the last focused entry or default to first entry
            if (focusedEntry == null)
            {
                // Default to the first numeric entry if none is focused
                focusedEntry = TxtM0RawGross;
                if (focusedEntry != null)
                {
                    await Task.Delay(100); // Small delay to ensure UI is ready
                    focusedEntry.Focus();
                }
            }

            string sValue = focusedEntry?.Text ?? "0";
            double dValue = 0;
            
            // Try to parse the current value, default to 0 if parsing fails
            if (!double.TryParse(sValue, out dValue))
            {
                dValue = 0;
            }

            // Start the CalculatorPage passing to it the value of the 
            // control that currently has the focus
            var calculator = new CalculatorPage(dValue);
            await Navigation.PushModalAsync(calculator);
            var result = await calculator.ResultSource.Task;

            // Check if the page has given back a result
            if (result.HasValue && focusedEntry != null)
            {
                string resultText = result.Value.ToString();
                
                // Update the focused entry's text
                focusedEntry.Text = resultText;
                
                // Update the corresponding data model based on which entry was focused
                UpdateDataModelFromEntry(focusedEntry, resultText);
                
                // Recalculate and refresh UI after changes
                if (blFood != null)
                {
                    blFood.CalcUnknownData();
                    FromClassToUi();
                }
                
                // Mark that data was modified
                FoodDataWasModified = true;
            }
        }
        catch (Exception ex)
        {
            // Log the error and show a user-friendly message
            General.LogOfProgram?.Error("WeighFoodPage - Calculator_Click", ex);
            
            try
            {
                await DisplayAlert("Error", "An error occurred while using the calculator. Please try again.", "OK");
            }
            catch
            {
                // If even DisplayAlert fails, just log it
                General.LogOfProgram?.Error("WeighFoodPage - Calculator_Click - DisplayAlert failed", ex);
            }
        }
    }
    private void UpdateDataModelFromEntry(Entry focusedEntry, string resultText)
    {
        try
        {
            if (focusedEntry == null || string.IsNullOrEmpty(resultText)) return;

            if (focusedEntry == txtFoodCarbohydratesPerUnit)
            {
                if (selectedFood?.CarbohydratesPercent != null)
                    selectedFood.CarbohydratesPercent.Text = resultText;
            }
            else if (focusedEntry == TxtM0RawGross && blFood?.T0RawGross != null && blMealRaw != null)
            {
                blFood.T0RawGross.Text = resultText;
                blMealRaw.GrossOrTareChanged();
            }
            else if (focusedEntry == TxtM0RawTare && blFood?.T0RawTare != null && blMealRaw != null)
            {
                blFood.T0RawTare.Text = resultText;
                blMealRaw.GrossOrTareChanged();
            }
            else if (focusedEntry == TxtM0RawNet && blFood?.T0RawNet != null && blMealRaw != null)
            {
                blFood.T0RawNet.Text = resultText;
                blMealRaw.NetWeightChanged();
            }
            else if (focusedEntry == TxtS1CookedGross && blFood?.S1CookedGross != null && blMealCooked != null)
            {
                blFood.S1CookedGross.Text = resultText;
                blMealCooked.GrossOrTareChanged();
            }
            else if (focusedEntry == TxtS1CookedTare && blFood?.S1CookedTare != null && blMealCooked != null)
            {
                blFood.S1CookedTare.Text = resultText;
                blMealCooked.GrossOrTareChanged();
            }
            else if (focusedEntry == TxtS1CookedNet && blFood?.S1CookedNet != null && blMealCooked != null)
            {
                blFood.S1CookedNet.Text = resultText;
                blMealCooked.NetWeightChanged();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - UpdateDataModelFromEntry", ex);
        }
    }  
    private Entry GetFocusedEntry()
    {
        try
        {
            // Check all the Entry controls that should trigger the calculator
            if (txtFoodCarbohydratesPerUnit?.IsFocused == true) return txtFoodCarbohydratesPerUnit;
            if (TxtM0RawGross?.IsFocused == true) return TxtM0RawGross;
            if (TxtM0RawTare?.IsFocused == true) return TxtM0RawTare;
            if (TxtM0RawNet?.IsFocused == true) return TxtM0RawNet;
            if (TxtS1CookedGross?.IsFocused == true) return TxtS1CookedGross;
            if (TxtS1CookedTare?.IsFocused == true) return TxtS1CookedTare;
            if (TxtS1CookedNet?.IsFocused == true) return TxtS1CookedNet;
            if (txtFoodName?.IsFocused == true) return txtFoodName;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - GetFocusedEntry", ex);
        }
        
        return null;
    }
    // Implement Food import functionality similar to MealPage
    private async void btnFoods_ClickAsync(object sender, EventArgs e)
    {
        try
        {
            // Update selectedFood from UI before opening FoodsPage
            FromUiToClass();
            
            // Create a temporary FoodInMeal for the FoodsPage
            var tempFoodInMeal = new FoodInMeal();
            if (selectedFood != null)
            {
                bl.FromFoodToFoodInMeal(selectedFood, tempFoodInMeal);
            }

            // Open FoodsPage
            foodsPage = new FoodsPage(tempFoodInMeal);
            await Navigation.PushModalAsync(foodsPage);
            
            // Wait for the page to be closed and get the result
            bool foodWasChosen = await foodsPage.PageClosedTask;
            
            // Check if the user chose a food in called page
            if (foodWasChosen && foodsPage.FoodIsChosen)
            {
                // Update selectedFood with the chosen Food from the called page
                selectedFood = foodsPage.Food;
                
                // Update the user interface with the new data
                FromClassToUi();
                
                General.LogOfProgram?.Event("Food imported successfully in WeighFoodPage");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnFoods_ClickAsync", ex);
            try
            {
                await DisplayAlert("Error", "Failed to open foods page. Check logs for details.", "OK");
            }
            catch
            {
                // If even DisplayAlert fails, just log it
                General.LogOfProgram?.Error("WeighFoodPage - btnFoods_ClickAsync - DisplayAlert failed", ex);
            }
        }
    }
    private void ResetUnitToGrams(object sender, EventArgs e)
    {
        try
        {
            if (btnUnit != null) btnUnit.Text = "g";
            if (selectedFood?.GramsInOneUnit != null)
            {
                selectedFood.UnitSymbol = "g";
                selectedFood.GramsInOneUnit.Double = 1;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - ResetUnitToGrams", ex);
        }
    }
    // Empty event handlers for future implementation
    private void TxtT0RawGross_Leave(object sender, FocusEventArgs e)
    {
        // Placeholder for future implementation
    }
    private void TxtT0RawTare_Leave(object sender, FocusEventArgs e)
    {
        // Placeholder for future implementation
    }
    private void TxtT0CookedPlusTare_Leave(object sender, FocusEventArgs e)
    {
        // Placeholder for future implementation
    }
    private void gridFoodsToWeigh_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        // Placeholder for future implementation
    }
    private void TxtS1pPotCookedPlusPot_TextChanged(object sender, EventArgs e)
    {
        // Placeholder for future implementation
    }
    private void TxtCookedPortion_Leave(object sender, FocusEventArgs e)
    {
        // Placeholder for future implementation
    }
    private void TxtNPortions_Leave(object sender, FocusEventArgs e)
    {
        // Placeholder for future implementation
    }
    private void TxtCookedPortionGross_Leave(object sender, FocusEventArgs e)
    {

    }

    private void TxtCookedPortionTare_Leave(object sender, FocusEventArgs e)
    {

    }

    private void TxtCookedPortionNet_Leave(object sender, FocusEventArgs e)
    {

    }
}
