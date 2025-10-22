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

    // Properties to access RadioButton states
    public bool IsDivideIntoEqualPortionsSelected => rbDivideIntoEqualPortions?.IsChecked ?? false;
    public bool IsWeighCookedPortionSelected => rbWeighCookedPortion?.IsChecked ?? false;
    public bool IsChoOfRawFoodSelected => chkChoOfRawFood?.IsChecked ?? false;

    // Default constructor
    public WeighFoodPage()
    {
        try
        {
            InitializeComponent();

            // Initialize selectedFood with default unit BEFORE calling FromClassToUi()
            selectedFood = new Food(new UnitOfFood());

            // Restore saved weighing data from database
            blFood.RestoreData();

            blMealRaw = new BL_GrossTareAndNetWeight(blFood.RawGross, blFood.RawTare, blFood.RawNet);
            blMealCooked = new BL_GrossTareAndNetWeight(blFood.CookedGross, blFood.CookedTare, blFood.CookedNet);

            // Populate UI with restored data
            PopulateUIFromBlFood();

            // Set default selection to WeighCookedPortion
            WeighCookedPortion = true;
            if (rbWeighCookedPortion != null)
                rbWeighCookedPortion.IsChecked = true;

            // Set default CHO of raw food to checked
            ChoOfRawFood = true;
            if (chkChoOfRawFood != null)
                chkChoOfRawFood.IsChecked = true;

            // Hook up TextChanged for CHO% and Number of portions for summary calculation
            if (txtFoodCarbohydratesPerUnit != null)
            {
                txtFoodCarbohydratesPerUnit.TextChanged += TxtFoodCarbohydratesPerUnit_TextChanged;
            }
            if (TxtNPortions != null)
            {
                TxtNPortions.TextChanged += TxtNPortions_TextChanged;
            }

            // Use Loaded event to enable automatic calculations after all controls are loaded
            this.Loaded += WeighFoodPage_Loaded;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - Constructor", ex);
            // Initialize with safe defaults to prevent further errors
            InitializeSafeDefaults();
        }
    }
    private void WeighFoodPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            General.LogOfProgram?.Event("WeighFoodPage - Page loaded, automatic calculations enabled");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - WeighFoodPage_Loaded", ex);
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

                // Populate UI fields with food data
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

                // Populate UI fields with food data
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
    }
    // Constructor that accepts Ingredient data from RecipePage
    public WeighFoodPage(Ingredient initialIngredient) : this()
    {
        try
        {
            if (initialIngredient != null)
            {
                // Convert Ingredient to Food for internal use
                selectedFood = new Food(new UnitOfFood());
                bl.FromIngredientToFood(initialIngredient, selectedFood);

                // Populate UI fields with ingredient data
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
    }
    private void InitializeSafeDefaults()
    {
        try
        {
            selectedFood ??= new Food(new UnitOfFood());
            blMealRaw ??= new BL_GrossTareAndNetWeight(blFood?.RawGross, blFood?.RawTare, blFood?.RawNet);
            blMealCooked ??= new BL_GrossTareAndNetWeight(blFood?.CookedGross, blFood?.CookedTare, blFood?.CookedNet);
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
            //FromUiToClass();

            // Perform final calculation to ensure WeightOfPortion is up to date
            CalculateSummaryData();

            // Perform final calculation
            if (blFood != null)
            {
                blFood.CalcUnknownData();
                // Comment out SaveData as it might cause database errors
                // Data is passed through ResultFood and WeightOfPortion properties
                // blFood.SaveData();
            }

            // Set result data
            ResultFood = selectedFood;
            FoodDataWasModified = true;
            UserCancelled = false;

            // Log the action
            General.LogOfProgram?.Event($"WeighFoodPage - User clicked Choose button, changes saved. WeightOfPortion={WeightOfPortion:F1}g");

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

                // Recalculate summary data
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
                General.LogOfProgram?.Event("WeighFoodPage - Weigh cooked portion option selected");

                if (SectionWeightOfCookedPortion != null) SectionWeightOfCookedPortion.IsVisible = true;
                if (SectionNumberCookedPortion != null) SectionNumberCookedPortion.IsVisible = false;

                // Recalculate summary data
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

            // Recalculate summary data when CHO of raw food changes
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
    #region Summary Data Calculation
    /// <summary>
    /// Calculates Summary Data: Raw/Cooked ratio, Weight of portion, CHO of portion
    /// Called whenever any weight or portion value changes
    /// </summary>
    private void CalculateSummaryData()
    {
        try
        {
            General.LogOfProgram?.Debug($"WeighFoodPage - CalculateSummaryData STARTED");

            // Parse values from UI
            double rawNet = Safe.Double(TxtRawNet?.Text) ?? 0;
            double cookedNet = Safe.Double(TxtCookedNet?.Text) ?? 0;
            double portionNet = Safe.Double(TxtCookedPortionNet?.Text) ?? 0;
            int nPortions = (int)(Safe.Double(TxtNPortions?.Text) ?? 0);
            double choPercent = Safe.Double(txtFoodCarbohydratesPerUnit?.Text) ?? 0;

            General.LogOfProgram?.Debug($"WeighFoodPage - Parsed values: rawNet={rawNet}, cookedNet={cookedNet}, portionNet={portionNet}, nPortions={nPortions}, choPercent={choPercent}");

            // Calculate Raw/Cooked ratio
            double rawCookedRatio = 0;
            if (cookedNet > 0)
            {
                rawCookedRatio = rawNet / cookedNet;
            }

            // Update Raw/Cooked ratio display
            if (TxtRawByCooked != null)
            {
                TxtRawByCooked.Text = rawCookedRatio > 0 ? rawCookedRatio.ToString("F3") : "";
            }

            double weightOfPortion = 0;
            double choOfPortion = 0;

            // Calculate based on selected option
            if (WeighCookedPortion)
            {
                // Weigh the portion option
                if (ChoOfRawFood)
                {
                    // CHO of raw food enabled: Weight of portion = Raw/cooked ratio * Net of portion
                    weightOfPortion = rawCookedRatio * portionNet;
                }
                else
                {
                    // CHO of raw food disabled: Weight of portion = Net of portion
                    weightOfPortion = portionNet;
                }
            }
            else if (DivideIntoEqualPortions && nPortions > 0)
            {
                // Equal portions option
                if (ChoOfRawFood)
                {
                    weightOfPortion = rawCookedRatio * cookedNet / nPortions;
                }
                else
                {
                    weightOfPortion = cookedNet / nPortions;
                }
            }
            // CHO [g] of portion = Weight of portion * CHO [%] / 100
            choOfPortion = weightOfPortion * choPercent / 100;

            // Update Summary Data displays
            if (TxtWeightOfPortion != null)
            {
                TxtWeightOfPortion.Text = weightOfPortion > 0 ? weightOfPortion.ToString("F1") : "";
            }

            if (TxtCarboydratesOfPortion != null)
            {
                TxtCarboydratesOfPortion.Text = choOfPortion > 0 ? choOfPortion.ToString("F1") : "";
            }

            // Store the calculated weight of portion for return to calling page
            WeightOfPortion = weightOfPortion;

            General.LogOfProgram?.Event($"WeighFoodPage - Summary calculated: Ratio={rawCookedRatio:F3}, Weight={weightOfPortion:F1}g, CHO={choOfPortion:F1}g");

            // Save weighing data to database after calculation
            CopyUIToBlFood();
            General.LogOfProgram?.Debug($"WeighFoodPage - About to call blFood.SaveData()");
            blFood.SaveData();
            General.LogOfProgram?.Debug($"WeighFoodPage - CalculateSummaryData COMPLETED");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - CalculateSummaryData", ex);
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

        // Cooked food section
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
    private void TxtRawGrossOrTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded || rawGrossOrTareChanging || rawNetChanging)
        {
            return;
        }

        rawGrossOrTareChanging = true;
        try
        {
            double gross = Safe.Double(TxtRawGross?.Text) ?? 0;
            double tare = Safe.Double(TxtRawTare?.Text) ?? 0;
            double net = gross - tare;

            if (TxtRawNet != null && net >= 0)
            {
                TxtRawNet.Text = net.ToString("F1");
            }

            // Recalculate summary data after weight changes
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtRawGrossOrTare_TextChanged", ex);
        }
        finally
        {
            rawGrossOrTareChanging = false;
        }
    }
    private void TxtRawNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded || rawNetChanging || rawGrossOrTareChanging)
        {
            return;
        }

        rawNetChanging = true;
        try
        {
            // User is manually changing Net, so clear Gross and Tare
            if (TxtRawGross != null) TxtRawGross.Text = "";
            if (TxtRawTare != null) TxtRawTare.Text = "";

            // Recalculate summary data
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtRawNet_TextChanged", ex);
        }
        finally
        {
            rawNetChanging = false;
        }
    }
    // TextChanged event handlers for Cooked food weighing
    private void TxtCookedGrossOrTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded || cookedGrossOrTareChanging || cookedNetChanging)
        {
            return;
        }

        cookedGrossOrTareChanging = true;
        try
        {
            double gross = Safe.Double(TxtCookedGross?.Text) ?? 0;
            double tare = Safe.Double(TxtCookedTare?.Text) ?? 0;
            double net = gross - tare;

            if (TxtCookedNet != null && net >= 0)
            {
                TxtCookedNet.Text = net.ToString("F1");
            }

            // Recalculate summary data after weight changes
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtCookedGrossOrTare_TextChanged", ex);
        }
        finally
        {
            cookedGrossOrTareChanging = false;
        }
    }
    private void TxtCookedNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;

        // Log entry point
        General.LogOfProgram?.Debug($"WeighFoodPage - TxtCookedNet_TextChanged STARTED: OldValue='{e.OldTextValue}', NewValue='{e.NewTextValue}'");

        if (entry == null || !entry.IsLoaded || cookedNetChanging || cookedGrossOrTareChanging)
        {
            General.LogOfProgram?.Debug($"WeighFoodPage - TxtCookedNet_TextChanged SKIPPED: entry={entry != null}, IsLoaded={entry?.IsLoaded}, cookedNetChanging={cookedNetChanging}, cookedGrossOrTareChanging={cookedGrossOrTareChanging}");
            return;
        }

        cookedNetChanging = true;
        try
        {
            // User is manually changing Net, so clear Gross and Tare
            if (TxtCookedGross != null) TxtCookedGross.Text = "";
            if (TxtCookedTare != null) TxtCookedTare.Text = "";

            General.LogOfProgram?.Debug($"WeighFoodPage - TxtCookedNet_TextChanged: Cleared Gross and Tare, calling CalculateSummaryData");

            // Recalculate summary data
            CalculateSummaryData();

            General.LogOfProgram?.Debug($"WeighFoodPage - TxtCookedNet_TextChanged COMPLETED successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtCookedNet_TextChanged", ex);
        }
        finally
        {
            cookedNetChanging = false;
            General.LogOfProgram?.Debug($"WeighFoodPage - TxtCookedNet_TextChanged: Flag reset");
        }
    }
    // TextChanged event handlers for Portion weighing
    private void TxtPortionGrossOrTare_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded || portionGrossOrTareChanging || portionNetChanging)
        {
            return;
        }

        portionGrossOrTareChanging = true;
        try
        {
            double gross = Safe.Double(TxtCookedPortionGross?.Text) ?? 0;
            double tare = Safe.Double(TxtCookedPortionTare?.Text) ?? 0;
            double net = gross - tare;

            if (TxtCookedPortionNet != null && net >= 0)
            {
                TxtCookedPortionNet.Text = net.ToString("F1");
            }

            // Recalculate summary data after weight changes
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtPortionGrossOrTare_TextChanged", ex);
        }
        finally
        {
            portionGrossOrTareChanging = false;
        }
    }
    private void TxtPortionNet_TextChanged(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (entry == null || !entry.IsLoaded || portionNetChanging || portionGrossOrTareChanging)
        {
            return;
        }

        portionNetChanging = true;
        try
        {
            // User is manually changing Net, so clear Gross and Tare
            if (TxtCookedPortionGross != null) TxtCookedPortionGross.Text = "";
            if (TxtCookedPortionTare != null) TxtCookedPortionTare.Text = "";

            // Recalculate summary data
            CalculateSummaryData();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - TxtPortionNet_TextChanged", ex);
        }
        finally
        {
            portionNetChanging = false;
        }
    }
    // TextChanged event handlers for CHO% and Number of portions
    private void TxtFoodCarbohydratesPerUnit_TextChanged(object sender, TextChangedEventArgs e)
    {
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
    #endregion
    /// <summary>
    /// Populates UI fields with data from blFood (restored from database)
    /// </summary>
    private void PopulateUIFromBlFood()
    {
        try
        {
            // Raw food weighing data
            if (TxtRawGross != null) TxtRawGross.Text = blFood.RawGross.Text;
            if (TxtRawTare != null) TxtRawTare.Text = blFood.RawTare.Text;
            if (TxtRawNet != null) TxtRawNet.Text = blFood.RawNet.Text;

            // Cooked food weighing data
            if (TxtCookedGross != null) TxtCookedGross.Text = blFood.CookedGross.Text;
            if (TxtCookedTare != null) TxtCookedTare.Text = blFood.CookedTare.Text;
            if (TxtCookedNet != null) TxtCookedNet.Text = blFood.CookedNet.Text;

            // Cooked portion weighing data
            if (TxtCookedPortionGross != null) TxtCookedPortionGross.Text = blFood.CookedPortionGross.Text;
            if (TxtCookedPortionTare != null) TxtCookedPortionTare.Text = blFood.CookedPortionTare.Text;
            if (TxtCookedPortionNet != null) TxtCookedPortionNet.Text = blFood.CookedPortionNet.Text;

            // Number of portions
            if (TxtNPortions != null) TxtNPortions.Text = blFood.NPortions.Text;

            General.LogOfProgram?.Event("WeighFoodPage - UI populated from saved data");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - PopulateUIFromBlFood", ex);
        }
    }
    /// <summary>
    /// Copies data from UI to blFood object before saving
    /// </summary>
    private void CopyUIToBlFood()
    {
        try
        {
            General.LogOfProgram?.Debug($"WeighFoodPage - CopyUIToBlFood STARTED");
            General.LogOfProgram?.Debug($"WeighFoodPage - UI Values: RawGross='{TxtRawGross?.Text}', RawTare='{TxtRawTare?.Text}', RawNet='{TxtRawNet?.Text}'");
            General.LogOfProgram?.Debug($"WeighFoodPage - UI Values: CookedGross='{TxtCookedGross?.Text}', CookedTare='{TxtCookedTare?.Text}', CookedNet='{TxtCookedNet?.Text}'");

            // Raw food weighing data
            blFood.RawGross.Text = TxtRawGross?.Text ?? "";
            blFood.RawTare.Text = TxtRawTare?.Text ?? "";
            blFood.RawNet.Text = TxtRawNet?.Text ?? "";

            // Cooked food weighing data
            blFood.CookedGross.Text = TxtCookedGross?.Text ?? "";
            blFood.CookedTare.Text = TxtCookedTare?.Text ?? "";
            blFood.CookedNet.Text = TxtCookedNet?.Text ?? "";

            // Cooked portion weighing data
            blFood.CookedPortionGross.Text = TxtCookedPortionGross?.Text ?? "";
            blFood.CookedPortionTare.Text = TxtCookedPortionTare?.Text ?? "";
            blFood.CookedPortionNet.Text = TxtCookedPortionNet?.Text ?? "";

            // Number of portions
            blFood.NPortions.Text = TxtNPortions?.Text ?? "";

            General.LogOfProgram?.Event("WeighFoodPage - Data copied from UI to BL");
            General.LogOfProgram?.Debug($"WeighFoodPage - BL Values: CookedGross='{blFood.CookedGross.Text}', CookedTare='{blFood.CookedTare.Text}', CookedNet='{blFood.CookedNet.Text}'");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("WeighFoodPage - CopyUIToBlFood", ex);
        }
    }
}

