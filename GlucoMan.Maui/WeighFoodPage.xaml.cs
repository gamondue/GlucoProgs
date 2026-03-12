using gamon;
using GlucoMan;

namespace GlucoMan.Maui;

public partial class WeighFoodPage : ContentPage
{
    // Add business layer for food weighing
    BL_WeighFood blFood = new BL_WeighFood();
    BL_GrossTareAndNetWeight blMealRaw;
    BL_GrossTareAndNetWeight blMealCooked;

    // Add business layer for food management and properties for Food selection like in MealPage
    private BL_MealAndFood blMeal = new BL_MealAndFood();
    FoodsPage foodsPage;

    // Properties to handle Data exchange with calling page
    internal DoubleAndText ResultCarbohydratesPercent { get; private set; } = new DoubleAndText();
    internal DoubleAndText ResultWeightOfPortion { get; private set; } = new();
    internal string FoodName => txtFoodName?.Text ?? "";
    public bool FoodDataWasModified { get; private set; } = false;
    public bool UserCancelled { get; private set; } = false;
    
    // Properties for RadioButton options
    public bool DivideIntoEqualPortions { get; set; } = false;
    public bool WeighCookedPortion { get; set; } = false;

    // Property for CheckBox option
    public bool ChoOfRawFood { get; set; } = false;

    // Property to store the calculated weight of portion (to be returned to calling page)
    public double WeightOfPortion { get; private set; } = 0;

    // TaskCompletionSource for modal behavior similar to other pages
    private TaskCompletionSource<bool> pageClosedTaskSource = new TaskCompletionSource<bool>();
    public Task<bool> PageClosedTask => pageClosedTaskSource.Task;

    // Properties for tracking manual changes (similar to MealPage)
    private bool rawGrossOrTareChanging = false;
    private bool rawNetChanging = false;
    private bool cookedGrossOrTareChanging = false;
    private bool cookedNetChanging = false;
    private bool portionGrossOrTareChanging = false;
    private bool portionNetChanging = false;

    // Flag to prevent event storm during page loading
    private bool isLoading = true;

    // Properties to access RadioButton states
    public bool IsDivideIntoEqualPortionsSelected => rbDivideIntoEqualPortions?.IsChecked ?? false;
    public bool IsWeighCookedPortionSelected => rbWeighCookedPortion?.IsChecked ?? false;
    public bool IsChoOfRawFoodSelected => chkChoOfRawFood?.IsChecked ?? false;

    // Default constructor
    public WeighFoodPage()
    {
        try
        {
            isLoading = true;  // Prevent calculations during initialization
            
            InitializeComponent();

            // Restore saved weighing data from database
            blFood.RestoreData();

            blMealRaw = new BL_GrossTareAndNetWeight(blFood.Data.Raw.Gross, blFood.Data.Raw.Tare, blFood.Data.Raw.Net);
            blMealCooked = new BL_GrossTareAndNetWeight(blFood.Data.CookedFood.Gross, blFood.Data.CookedFood.Tare, blFood.Data.CookedFood.Net);

            // Set BindingContext to blFood.Data for automatic UI binding
            this.BindingContext = blFood.Data;

            // Set default selection to WeighCookedPortion
            WeighCookedPortion = true;
            blFood.Data.DoWeighCookedPortion = true;
            if (rbWeighCookedPortion != null)
                rbWeighCookedPortion.IsChecked = true;

            // Set default CHO of raw food to checked
            ChoOfRawFood = true;
            blFood.Data.IsChoOfRawFood = true;
            if (chkChoOfRawFood != null)
                chkChoOfRawFood.IsChecked = true;

            // Use Loaded event to enable automatic calculations after all controls are loaded
            this.Loaded += WeighFoodPage_Loaded;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor", ex);
            // Initialize with safe defaults to prevent further errors
            InitializeSafeDefaults();
        }
        finally
        {
            isLoading = false;  // Re-enable calculations after initialization
        }
    }
    // Constructor that accepts Food Data from MealPage
    internal WeighFoodPage(Food initialFood) : this()
    {
        try
        {
            isLoading = true;  // Prevent calculations during initialization
            
            if (initialFood != null)
            {
                // Populate UI fields with food Data
                if (txtFoodName != null)
                {
                    txtFoodName.Text = initialFood.Name ?? "";
                }

                if (txtFoodCarbohydratesPerUnit != null && initialFood.CarbohydratesPercent?.Double != null)
                {
                    txtFoodCarbohydratesPerUnit.Text = initialFood.CarbohydratesPercent.Double.Value.ToString("F1");
                }

                General.LogOfProgram?.Event($"WeighFoodPage - Loaded food: Name={initialFood.Name}, CHO%={initialFood.CarbohydratesPercent?.Double ?? 0:F1}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor with Food", ex);
            InitializeSafeDefaults();
        }
        finally
        {
            isLoading = false;  // Re-enable calculations
        }
    }
    // Constructor that accepts FoodInMeal Data from MealPage
    public WeighFoodPage(FoodInMeal initialFoodInMeal) : this()
    {
        try
        {
            isLoading = true;  // Prevent calculations during initialization
            
            if (initialFoodInMeal != null)
            {
                // Convert FoodInMeal to Food for internal use
                Food outputFood = new Food(new UnitOfFood());
                blMeal.FromFoodInMealToFood(initialFoodInMeal, outputFood);

                // Populate UI fields with food Data
                if (txtFoodName != null)
                {
                    txtFoodName.Text = initialFoodInMeal.Name ?? "";
                }

                if (txtFoodCarbohydratesPerUnit != null && initialFoodInMeal.CarbohydratesPercent?.Double != null)
                {
                    txtFoodCarbohydratesPerUnit.Text = initialFoodInMeal.CarbohydratesPercent.Double.Value.ToString("F1");
                }

                General.LogOfProgram?.Event($"WeighFoodPage - Loaded food: Name={initialFoodInMeal.Name}, CHO%={initialFoodInMeal.CarbohydratesPercent?.Double ?? 0:F1}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor with FoodInMeal", ex);
            InitializeSafeDefaults();
        }
        finally
        {
            isLoading = false;  // Re-enable calculations
        }
    }
    // Constructor that accepts Ingredient Data from RecipePage
    public WeighFoodPage(Ingredient initialIngredient) : this()
    {
        try
        {
            isLoading = true;  // Prevent calculations during initialization
            
            if (initialIngredient != null)
            {
                // Convert Ingredient to Food for internal use
                Food outputFood = new Food(new UnitOfFood());
                blMeal.FromIngredientToFood(initialIngredient, outputFood);

                // Populate UI fields with ingredient Data
                if (txtFoodName != null)
                {
                    txtFoodName.Text = initialIngredient.Name ?? "";
                }

                if (txtFoodCarbohydratesPerUnit != null && initialIngredient.CarbohydratesPercent?.Double != null)
                {
                    txtFoodCarbohydratesPerUnit.Text = initialIngredient.CarbohydratesPercent.Double.Value.ToString("F1");
                }

                General.LogOfProgram?.Event($"WeighFoodPage - Loaded ingredient: Name={initialIngredient.Name}, CHO%={initialIngredient.CarbohydratesPercent?.Double ?? 0:F1}");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor with Ingredient", ex);
            InitializeSafeDefaults();
        }
        finally
        {
            isLoading = false;  // Re-enable calculations
        }
    }
    private void WeighFoodPage_Loaded(object sender, EventArgs e)
    {
        //try
        //{
        //    General.LogOfProgram?.Event("WeighFoodPage - Page loaded, automatic calculations enabled");
        //}
        //catch (Exception ex)
        //{
        //    General.LogOfProgram?.Error("WeighFoodPage - WeighFoodPage_Loaded", ex);
        //}
    }
    private void InitializeSafeDefaults()
    {
        try
        {
            //////////outputFood ??= new Food(new UnitOfFood());
            //////////blMealRaw ??= new BL_GrossTareAndNetWeight(blFood?.RawGross, blFood?.RawTare, blFood?.RawNet);
            //////////blMealCooked ??= new BL_GrossTareAndNetWeight(blFood?.CookedGross, blFood?.CookedTare, blFood?.CookedNet);
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
                //FromUiToClass();
                ////////ResultFood = selectedFood;
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
            // Update Result CHO% from UI before calculation
            if (txtFoodCarbohydratesPerUnit != null && !string.IsNullOrEmpty(txtFoodCarbohydratesPerUnit.Text))
            {
                ResultCarbohydratesPercent.Double = Safe.Double(TxtCarboydratesPercentOfTotal.Text) ?? 0;
                //if (.CarbohydratesPercent != null)
                //{
                //    selectedFood.CarbohydratesPercent.Double = choPercent;
                //    General.LogOfProgram?.Event($"WeighFoodPage - Updated selectedFood.CarbohydratesPercent to {choPercent:F1}%");
                //}
            }
            if (!string.IsNullOrEmpty(TxtWeightOfPortion.Text))
            {
                ResultWeightOfPortion.Double = Safe.Double(TxtWeightOfPortion.Text) ?? 0;
                //if (selectedFood?.CarbohydratesPercent != null)
                //{
                //    selectedFood. .Double = weightOfPortion;
                //    General.LogOfProgram?.Event($"WeighFoodPage - Updated selectedFood.CarbohydratesPercent to {choPercent:F1}%");
                //}
            }
            
            //// Centralize output selection here: prefer WeightOfPortion, fallback to RawNet (TxtRawNet)
            //double effectiveWeight = WeightOfPortion;
            //if (effectiveWeight <=0)
            //{
            //    double rawNet = Safe.Double(TxtRawNet?.Text) ??0;
            //    if (rawNet >0)
            //    {
            //        effectiveWeight = rawNet;
            //        General.LogOfProgram?.Event($"WeighFoodPage - Fallback to RawNet for output weight: {effectiveWeight:F1}g");
            //    }
            //    else
            //    {
            //        General.LogOfProgram?.Event("WeighFoodPage - No WeightOfPortion and RawNet empty; output weight remains0");
            //    }
            //}
            //// Expose the chosen weight via WeightOfPortion so callers have a single source
            //WeightOfPortion = effectiveWeight;

            // Signal result Data
            UserCancelled = false;
            FoodDataWasModified = true;

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

                // Recalculate summary Data
                CalculateSummaryData();
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
                General.LogOfProgram?.Event("WeighFoodPage - Weigh cooked Portion option selected");

                if (SectionWeightOfCookedPortion != null) SectionWeightOfCookedPortion.IsVisible = true;
                if (SectionNumberCookedPortion != null) SectionNumberCookedPortion.IsVisible = false;

                // Recalculate summary Data
                CalculateSummaryData();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - OnWeighCookedPortionCheckedChanged", ex);
        }
    }
    // CheckBox event handler for CHO of raw food
    private void OnChoOfRawFoodCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        try
        {
            ChoOfRawFood = e.Value;
            General.LogOfProgram?.Event($"WeighFoodPage - CHO of raw food option: {(e.Value ? "enabled" : "disabled")}");

            // Recalculate summary Data when CHO of raw food changes
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - OnChoOfRawFoodCheckedChanged", ex);
        }
    }
    // Handler for tapping the label (to toggle checkbox)
    private void OnChoOfRawFoodLabelTapped(object sender, EventArgs e)
    {
        try
        {
            if (chkChoOfRawFood != null)
            {
                chkChoOfRawFood.IsChecked = !chkChoOfRawFood.IsChecked;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - OnChoOfRawFoodLabelTapped", ex);
        }
    }
    private async void Calculator_Click(object sender, TappedEventArgs e)
    {
        try
        {
            var focusedEntry = GetFocusedEntry();
            string sValue = focusedEntry?.Text ?? "0";
            double dValue = double.TryParse(sValue, out var val) ? val : 0;

            // Start the CalculatorPage passing to it the value of the control that currently has the focus
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
    /// <summary>
    /// Gets the currently focused Entry control from all weight input fields
    /// </summary>
    /// <returns>The focused Entry, or null if none is focused</returns>
    private Entry GetFocusedEntry()
    {
        // Raw food section
        if (TxtRawGross != null && TxtRawGross.IsFocused) return TxtRawGross;
        if (TxtRawTare != null && TxtRawTare.IsFocused) return TxtRawTare;
        if (TxtRawNet != null && TxtRawNet.IsFocused) return TxtRawNet;

        // CookedFood food section
        if (TxtCookedGross != null && TxtCookedGross.IsFocused) return TxtCookedGross;
        if (TxtCookedTare != null && TxtCookedTare.IsFocused) return TxtCookedTare;
        if (TxtCookedNet != null && TxtCookedNet.IsFocused) return TxtCookedNet;

        // Portion section
        if (TxtCookedPortionGross != null && TxtCookedPortionGross.IsFocused) return TxtCookedPortionGross;
        if (TxtCookedPortionTare != null && TxtCookedPortionTare.IsFocused) return TxtCookedPortionTare;
        if (TxtCookedPortionNet != null && TxtCookedPortionNet.IsFocused) return TxtCookedPortionNet;

        // Number of portions
        if (TxtNPortions != null && TxtNPortions.IsFocused) return TxtNPortions;

        // CHO percentage
        if (txtFoodCarbohydratesPerUnit != null && txtFoodCarbohydratesPerUnit.IsFocused) return txtFoodCarbohydratesPerUnit;

        return null;
    }
    private async void btnRawTareContainer_Click(object sender, TappedEventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("WeighFoodPage - Opening ContainersPage for raw tare");

            // Get current tare value if exists
            double? currentTare = Safe.Double(TxtRawTare?.Text);

            // Open ContainersPage
            var containersPage = new ContainersPage(currentTare);
            await Navigation.PushModalAsync(containersPage);

            // Wait for the page to be closed and get the result
            bool containerWasSelected = await containersPage.PageClosedTask;

            // Check if container was selected
            if (containerWasSelected && containersPage.SelectedContainer != null)
            {
                var selectedContainer = containersPage.SelectedContainer;

                // Set the tare weight from the selected container
                if (TxtRawTare != null && selectedContainer.Weight != null)
                {
                    TxtRawTare.Text = selectedContainer.Weight.Text;
                    General.LogOfProgram?.Event($"WeighFoodPage - Raw tare set to: {selectedContainer.Weight.Text}g from container '{selectedContainer.Name}'");
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnRawTareContainer_Click", ex);
            await DisplayAlert("Error", "Failed to select container. Please try again.", "OK");
        }
    }
    private async void btnCookedTareContainer_Click(object sender, TappedEventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("WeighFoodPage - Opening ContainersPage for cooked tare");

            // Get current tare value if exists
            double? currentTare = Safe.Double(TxtCookedTare?.Text);

            // Open ContainersPage
            var containersPage = new ContainersPage(currentTare);
            await Navigation.PushModalAsync(containersPage);

            // Wait for the page to be closed and get the result
            bool containerWasSelected = await containersPage.PageClosedTask;

            // Check if container was selected
            if (containerWasSelected && containersPage.SelectedContainer != null)
            {
                var selectedContainer = containersPage.SelectedContainer;

                // Set the tare weight from the selected container
                if (TxtCookedTare != null && selectedContainer.Weight != null)
                {
                    TxtCookedTare.Text = selectedContainer.Weight.Text;
                    General.LogOfProgram?.Event($"WeighFoodPage - Cooked tare set to: {selectedContainer.Weight.Text}g from container '{selectedContainer.Name}'");
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnCookedTareContainer_Click", ex);
            await DisplayAlert("Error", "Failed to select container. Please try again.", "OK");
        }
    }
    private async void btnPortionTareContainer_Click(object sender, TappedEventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("WeighFoodPage - Opening ContainersPage for portion tare");

            // Get current tare value if exists
            double? currentTare = Safe.Double(TxtCookedPortionTare?.Text);

            // Open ContainersPage
            var containersPage = new ContainersPage(currentTare);
            await Navigation.PushModalAsync(containersPage);

            // Wait for the page to be closed and get the result
            bool containerWasSelected = await containersPage.PageClosedTask;

            // Check if container was selected
            if (containerWasSelected && containersPage.SelectedContainer != null)
            {
                var selectedContainer = containersPage.SelectedContainer;

                // Set the tare weight from the selected container
                if (TxtCookedPortionTare != null && selectedContainer.Weight != null)
                {
                    TxtCookedPortionTare.Text = selectedContainer.Weight.Text;
                    General.LogOfProgram?.Event($"WeighFoodPage - Portion tare set to: {selectedContainer.Weight.Text}g from container '{selectedContainer.Name}'");
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnPortionTareContainer_Click", ex);
            await DisplayAlert("Error", "Failed to select container. Please try again.", "OK");
        }
    }
    // TextChanged event handlers for Raw food weighing
    private void TxtRawGross_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.Raw.Gross.Double = Safe.Double(TxtRawGross.Text);
        rawGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.Raw, BL_WeighFood.TypeOfWeigh.Gross);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtRawGross_TextChanged", ex);
        }
        finally
        {
            rawGrossOrTareChanging = false;
        }
    }
    
    private void TxtRawTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.Raw.Tare.Double = Safe.Double(TxtRawTare.Text);
        rawGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.Raw, BL_WeighFood.TypeOfWeigh.Tare);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtRawTare_TextChanged", ex);
        }
        finally
        {
            rawGrossOrTareChanging = false;
        }
    }
    
    private void TxtRawNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.Raw.Net.Double = Safe.Double(TxtRawNet.Text);
        rawGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.Raw, BL_WeighFood.TypeOfWeigh.Net);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtRawNet_TextChanged", ex);
        }
        finally
        {
            rawGrossOrTareChanging = false;
        }
    }
    
    private void TxtCookedGross_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.CookedFood.Gross.Double = Safe.Double(TxtCookedGross.Text);
        cookedGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.CookedFood, BL_WeighFood.TypeOfWeigh.Gross);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtCookedGross_TextChanged", ex);
        }
        finally
        {
            cookedGrossOrTareChanging = false;
        }
    }
    
    private void TxtCookedTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.CookedFood.Tare.Double = Safe.Double(TxtCookedTare.Text);
        cookedGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.CookedFood, BL_WeighFood.TypeOfWeigh.Tare);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtCookedTare_TextChanged", ex);
        }
        finally
        {
            cookedGrossOrTareChanging = false;
        }
    }
    
    private void TxtCookedNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.CookedFood.Net.Double = Safe.Double(TxtCookedNet.Text);
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.CookedFood, BL_WeighFood.TypeOfWeigh.Net);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtCookedNet_TextChanged", ex);
        }
        finally
        {
            cookedGrossOrTareChanging = false;
        }
    }
    
    // TextChanged event handlers for Portion weighing
    private void TxtPortionGross_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.Portion.Gross.Double = Safe.Double(TxtCookedPortionGross.Text);
        portionGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.Portion, BL_WeighFood.TypeOfWeigh.Gross);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtPortionGross_TextChanged", ex);
        }
        finally
        {
            portionGrossOrTareChanging = false;
        }
    }
    
    private void TxtPortionTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.Portion.Tare.Double = Safe.Double(TxtCookedPortionTare.Text);
        portionGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.Portion, BL_WeighFood.TypeOfWeigh.Tare);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtPortionTare_TextChanged", ex);
        }
        finally
        {
            portionGrossOrTareChanging = false;
        }
    }
    
    private void TxtPortionNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.Portion.Net.Double = Safe.Double(TxtCookedPortionNet.Text);
        portionGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.Portion, BL_WeighFood.TypeOfWeigh.Net);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtPortionNet_TextChanged", ex);
        }
        finally
        {
            portionGrossOrTareChanging = false;
        }
    }
    // TextChanged event handlers for CHO% and Number of portions
    private void TxtFoodCarbohydratesPerUnit_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded)
        {
            return;
        }

        try
        {
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtFoodCarbohydratesPerUnit_TextChanged", ex);
        }
    }
    private void TxtNPortions_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded)
        {
            return;
        }
        try
        {
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtNPortions_TextChanged", ex);
        }
    }
    private void txtFoodName_TextChanged(object sender, TextChangedEventArgs e)
    {
        FoodDataWasModified = true;
    }
    private void btnClearFields_Click(object sender, EventArgs e)
    {
        TxtRawGross.Text = "";
        TxtRawTare.Text = "";
        TxtRawNet.Text = "";
        TxtCookedGross.Text = "";
        TxtCookedTare.Text = "";
        TxtCookedNet.Text = "";
        TxtSeasoningGross.Text = "";
        TxtSeasoningTare.Text = "";
        TxtSeasoningNet.Text = "";
        TxtCookedPortionGross.Text = "";
        TxtCookedPortionTare.Text = "";
        TxtCookedPortionNet.Text = "";

        TxtNPortions.Text = "";
    }
    #region Seasoning Event Handlers
    // TextChanged event handlers for CookedSeasoning weighing
    private bool seasoningGrossOrTareChanging = false;
    private bool seasoningNetChanging = false;
    
    private void TxtSeasoningGross_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.CookedSeasoning.Gross.Double = Safe.Double(TxtSeasoningGross.Text);
        seasoningGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.CookedSeasoning, BL_WeighFood.TypeOfWeigh.Gross);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtSeasoningGross_TextChanged", ex);
        }
        finally
        {
            seasoningGrossOrTareChanging = false;
        }
    }
    
    private void TxtSeasoningCarbohydratesPercent_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        blFood.Data.CarbohydratesOfPortion.Double = Safe.Double(TxtSeasoningCarbohydratesPercent.Text);
        blFood.CalculateSummaryData();
    }
    
    private void TxtSeasoningTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        //blFood.Data.CookedSeasoning.Tare.Double = Safe.Double(TxtSeasoningTare.Text);
        seasoningGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.CookedSeasoning, BL_WeighFood.TypeOfWeigh.Tare);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtSeasoningTare_TextChanged", ex);
        }
        finally
        {
            seasoningGrossOrTareChanging = false;
        }
    }
    
    private void TxtSeasoningNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (isLoading) return;  // Skip during page loading
        
        seasoningGrossOrTareChanging = true;
        try
        {
            blFood.CalculateThirdFromTwoAndSummaryData(blFood.Data.CookedSeasoning, BL_WeighFood.TypeOfWeigh.Net);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtSeasoningNet_TextChanged", ex); 
        }
        finally
        {
            seasoningGrossOrTareChanging = false;
        }
    }  
    private async void btnSeasoningTareContainer_Click(object sender, TappedEventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("WeighFoodPage - Opening ContainersPage for seasoning tare");

            // Get current tare value if exists
            double? currentTare = Safe.Double(TxtSeasoningTare?.Text);

            // Open ContainersPage
            var containersPage = new ContainersPage(currentTare);
            await Navigation.PushModalAsync(containersPage);

            // Wait for the page to be closed and get the result
            bool containerWasSelected = await containersPage.PageClosedTask;

            // Check if container was selected
            if (containerWasSelected && containersPage.SelectedContainer != null)
            {
                var selectedContainer = containersPage.SelectedContainer;

                // Set the tare weight from the selected container
                if (TxtSeasoningTare != null && selectedContainer.Weight != null)
                {
                    TxtSeasoningTare.Text = selectedContainer.Weight.Text;
                    General.LogOfProgram?.Event($"WeighFoodPage - Seasoning tare set to: {selectedContainer.Weight.Text}g from container '{selectedContainer.Name}'");
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - btnSeasoningTareContainer_Click", ex);
            await DisplayAlert("Error", "Failed to select container. Please try again.", "OK");
        }
    }
    private void CalculateSummaryData()
    {
        try
        {
            General.LogOfProgram?.Debug($"WeighFoodPage - CalculateSummaryData STARTED");
            // Just call the calculation method in business layer
            blFood.CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - CalculateSummaryData", ex);
        }
    }
    #endregion
}

