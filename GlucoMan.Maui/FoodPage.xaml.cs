using System.Globalization;
using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class FoodPage : ContentPage
{
    BL_MealAndFood bl = new();
    internal Food CurrentFood { get; set; }

    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);
    public bool FoodIsChosen { get; private set; }
    private bool _hasUnsavedBarcode;
    private Entry _lastFocusedNumericEntry;
    // true while the pickers are being (re)loaded by the code: in this phase the
    // SelectedIndexChanged handlers must not copy the picker's selection into the food
    private bool _loadingPickers;

    internal FoodPage(Food Food)
    {
        InitializeComponent();
#if FATSECRET
        btnFatSecret.IsVisible = true;
        btnFatSecret.IsEnabled = true;
#endif
        RegisterNumericEntryFocusTracking();
        FoodIsChosen = false;
        CurrentFood = Food;
        _taskCompletionSource = new TaskCompletionSource<bool>();

        this.BindingContext = Food;

        //if (Food.GramsInOneUnit.Double != 1)
        //{
        //    blMeal.UpdateNutrientsDataWithNewUnit();
        //}

        LoadUnitPicker();
        LoadManufacturerPicker();
        LoadCategoryPicker();
        LoadContentUnitPicker();
    }

    // The pickers of unit, manufacturer and category are lists of suggestions for the entries
    // beside them: the field of the food is a free string, that can only be copied from the
    // picker when the user chooses an item. For this reason (re)loading a picker never
    // selects the value of the food and never changes the food.

    // Binds to cmbUnit the units of this food, retrieved from the business layer, with no selection
    private void LoadUnitPicker()
    {
        _loadingPickers = true;
        try
        {
            cmbUnit.ItemDisplayBinding = new Binding("Symbol");
            cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(CurrentFood);
            cmbUnit.SelectedIndex = -1;
        }
        finally
        {
            _loadingPickers = false;
        }
    }

    private void LoadManufacturerPicker()
    {
        _loadingPickers = true;
        try
        {
            cmbManufacturer.ItemDisplayBinding = new Binding("Name");
            cmbManufacturer.ItemsSource = bl.GetManufacturersForPicker();
            // the first item is blank
            cmbManufacturer.SelectedIndex = 0;
        }
        finally
        {
            _loadingPickers = false;
        }
    }

    private void LoadCategoryPicker()
    {
        _loadingPickers = true;
        try
        {
            cmbCategory.ItemDisplayBinding = new Binding("Name");
            cmbCategory.ItemsSource = bl.GetCategoriesForPicker();
            // the first item is blank
            cmbCategory.SelectedIndex = 0;
        }
        finally
        {
            _loadingPickers = false;
        }
    }

    private void LoadContentUnitPicker()
    {
        try
        {
            // Use FindByName to access cmbContentUnit in case .g.cs is not regenerated yet
            var cmbContentUnit = this.FindByName<Picker>("cmbContentUnit");
            if (cmbContentUnit == null) return;

            // bind to cmbContentUnit the same units as cmbUnit (synchronized)
            var units = bl.GetAllUnitsOfOneFood(CurrentFood);
            cmbContentUnit.ItemsSource = units;
            cmbContentUnit.ItemDisplayBinding = new Binding("Symbol");
            // Set selected item based on CurrentFood.ContentUnit if it exists
            if (!string.IsNullOrEmpty(CurrentFood.ContentUnit))
            {
                var matchingUnit = units?.FirstOrDefault(u => u.Symbol == CurrentFood.ContentUnit);
                if (matchingUnit != null)
                    cmbContentUnit.SelectedItem = matchingUnit;
            }
            else if (cmbContentUnit.Items.Count > 0)
                cmbContentUnit.SelectedIndex = 0;
        }
        catch
        {
            // Ignore if cmbContentUnit is not yet initialized
        }
    }

    private async void btnOk_Click(object sender, EventArgs e)
    {
      // Validate that food name is not empty before saving
    var food = (Food)this.BindingContext;
  if (string.IsNullOrWhiteSpace(food?.Name))
  {
 await DisplayAlert(AppStrings.Error, AppStrings.FoodNameCannotBeEmpty, AppStrings.OK);
  return;
   }

        // saving the food never adds its manufacturer or category to their lists
        FoodIsChosen = true;
        bl.SaveOneFood(food);
        _hasUnsavedBarcode = false;

        // Set the result and close the page
        _taskCompletionSource?.SetResult(true);
        
        // Check if this was opened as modal or regular navigation
        var navigation = Navigation;
        if (navigation.ModalStack.Contains(this))
     {
     await navigation.PopModalAsync();
     }
   else
        {
    await navigation.PopAsync();
        }
    }
    
    private async void btnEscape_Clicked(object sender, EventArgs e)
    {
        if (_hasUnsavedBarcode)
        {
            bool discard = await DisplayAlert(
                AppStrings.Warning ?? "Warning",
                AppStrings.UnsavedBarcodeWarning ?? "A new barcode was scanned but not saved. Exit without saving?",
                AppStrings.Yes ?? "Yes",
                AppStrings.No ?? "No");
            if (!discard)
                return;
        }

        FoodIsChosen = false;

        // Set the result and close the page
        _taskCompletionSource?.SetResult(false);

        // Check if this was opened as modal or regular navigation
        var navigation = Navigation;
        if (navigation.ModalStack.Contains(this))
        {
            await navigation.PopModalAsync();
        }
        else
        {
            await navigation.PopAsync();
        }
    }

    protected override bool OnBackButtonPressed()
    {
        if (_hasUnsavedBarcode)
        {
            Dispatcher.Dispatch(async () =>
            {
                bool discard = await DisplayAlert(
                    AppStrings.Warning ?? "Warning",
                    AppStrings.UnsavedBarcodeWarning ?? "A new barcode was scanned but not saved. Exit without saving?",
                    AppStrings.Yes ?? "Yes",
                    AppStrings.No ?? "No");
                if (discard)
                {
                    FoodIsChosen = false;
                    _taskCompletionSource?.SetResult(false);
                    var navigation = Navigation;
                    if (navigation.ModalStack.Contains(this))
                        await navigation.PopModalAsync();
                    else
                        await navigation.PopAsync();
                }
            });
            return true; // prevent default back navigation
        }

        // Handle back button press - user cancelled
        FoodIsChosen = false;
        _taskCompletionSource?.SetResult(false);
        return base.OnBackButtonPressed();
    }
    private void btnAddFoodManufacturer_Clicked(object sender, EventArgs e)
    {
        // adds the text of the entry to the list of the manufacturers (if it is not already there);
        // the food is not changed: it is written in the database only with the Save button
        if (bl.AddManufacturerToList(txtFoodManufacturer.Text))
            LoadManufacturerPicker();
    }
    private void btnCategory_Clicked(object sender, EventArgs e)
    {
        try
        {
            // adds the text of the entry to the list of the categories (if it is not already there);
            // the food is not changed: it is written in the database only with the Save button
            if (bl.AddCategoryToList(txtCategory.Text))
                LoadCategoryPicker();
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("FoodPage | btnCategory_Clicked", ex);
        }
    }
    private async void btnAddUnit_Clicked(object sender, EventArgs e)
    {
        double? gramsPerUnitNullable = Safe.Double(txtGramsPerUnit.Text);
        if (gramsPerUnitNullable == null)
        {
            await DisplayAlert(AppStrings.Error, AppStrings.InvalidNumberMessage, AppStrings.OK);
            return;
        }
        double gramsPerUnit = gramsPerUnitNullable.Value;
        UnitOfFood unit = new UnitOfFood(txtUnit.Text, gramsPerUnit);
        // ask the user if the unit is applicable to the current food or to all foods
        bool? isApplicableToAllFoods = await AskUnitApplicability();
        if (isApplicableToAllFoods == null)
            return;
        if (isApplicableToAllFoods == false)
        {
            unit.IdFood = CurrentFood.IdFood;
        }
        else
        {
            // if the unit is valid for any food it will have a null IdFood
            unit.IdFood = null;
        }
        if (bl.CheckIfUnitSymbolExists(unit, unit.IdFood))
        {
            // prompt the user that the unit already exists
            await DisplayAlert("", AppStrings.UnitSymbolExistsMessage, AppStrings.OK);
            return;
        }
        // we save the unit, if IdFood has been put to null, we mean that the UnitSymbol has to be used for any food
        if (bl.AddUnit(unit) == null)
        {
            // prompt the user that the saving of the unit failed
            await DisplayAlert("", AppStrings.UnitSaveFailedMessage, AppStrings.OK);
            return;
        }
        LoadUnitPicker();
        LoadContentUnitPicker();
    }
    // Returns true if the unit is for all the foods, false if it is only for this one,
    // null if the user cancels. The choice of all the foods has to be confirmed:
    // without confirmation the question is asked again
    private async Task<bool?> AskUnitApplicability()
    {
        while (true)
        {
            string choice = await DisplayActionSheet(
                AppStrings.UnitApplicabilityTitle + "\n" + AppStrings.UnitApplicabilityMessage,
                AppStrings.Cancel, null,
                AppStrings.UnitApplicabilityAll, AppStrings.UnitApplicabilityThis);
            if (choice == AppStrings.UnitApplicabilityThis)
                return false;
            if (choice != AppStrings.UnitApplicabilityAll)
                return null;
            if (await DisplayAlert(AppStrings.UnitApplicabilityTitle, AppStrings.UnitApplicableToAllWarning,
                AppStrings.Confirm, AppStrings.DoNotConfirm))
                return true;
        }
    }
    private async void btnRemoveUnit_Clicked(object sender, EventArgs e)
    {
        if (cmbUnit.SelectedItem is not UnitOfFood selected)
            return;
        // a unit with null (or 0) IdFood is shown for all the foods: ask confirmation before deleting it
        if (selected.IdFood == null || selected.IdFood == 0)
        {
            bool remove = await DisplayAlert(AppStrings.ConfirmDelete,
                string.Format(AppStrings.RemoveUnitForAllFoodsMessage, selected.Symbol),
                AppStrings.Yes, AppStrings.No);
            if (!remove)
                return;
        }
        bl.RemoveUnitFromFoodsUnits(selected);
        // Refresh both pickers
        LoadUnitPicker();
        LoadContentUnitPicker();
    }
    // deletes the selected item from the list of the manufacturers;
    // all the foods, including the one in the page, remain untouched
    private void btnRemoveFoodManufacturer_Clicked(object sender, EventArgs e)
    {
        if (cmbManufacturer.SelectedItem is not Manufacturer selected || string.IsNullOrEmpty(selected.Name))
            return;
        bl.DeleteManufacturerFromList(selected);
        LoadManufacturerPicker();
    }
    // deletes the selected item from the list of the categories;
    // all the foods, including the one in the page, remain untouched
    private void btnRemoveFoodCategory_Clicked(object sender, EventArgs e)
    {
        if (cmbCategory.SelectedItem is not CategoryOfFood selected || string.IsNullOrEmpty(selected.Name))
            return;
        bl.DeleteCategoryFromList(selected);
        LoadCategoryPicker();
    }
    private void cmbManufacturer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_loadingPickers)
            return;
        if (cmbManufacturer.SelectedItem is Manufacturer m && !string.IsNullOrEmpty(m.Name))
            txtFoodManufacturer.Text = m.Name;
    }

    private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_loadingPickers)
            return;
        if (cmbCategory.SelectedItem is CategoryOfFood cat && !string.IsNullOrEmpty(cat.Name))
            txtCategory.Text = cat.Name;
    }

    private void cmbContentUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var cmbContentUnit = sender as Picker;
            if (cmbContentUnit?.SelectedItem is UnitOfFood unit && !string.IsNullOrEmpty(unit.Symbol))
            {
                CurrentFood.ContentUnit = unit.Symbol;
            }
        }
        catch
        {
            // Ignore if cmbContentUnit is not initialized
        }
    }

    private void txtFoodManufacturer_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void txtCategory_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_loadingPickers)
            return;
        if (cmbUnit.SelectedItem is UnitOfFood unit && !string.IsNullOrEmpty(unit.Symbol))
        {
            CurrentFood.UnitSymbol = unit.Symbol;
            CurrentFood.GramsInOneUnit.Double = unit.GramsInOneUnit.Double;
            txtUnit.Text = unit.Symbol;
        }
    }
    private void txtGramsPerUnit_TextChanged(object sender, TextChangedEventArgs e)
    {

    }

    private async void btnCalculator_Clicked(object sender, EventArgs e)
    {
        var focusedEntry = GetFocusedNumericEntry() ?? txtFoodCarbohydrates;
        if (focusedEntry == null)
            return;

        double initialValue = 0;
        if (!TryParseNumericValue(focusedEntry.Text, out initialValue))
            initialValue = 0;

        var calculator = new CalculatorPage(initialValue);
        await Navigation.PushModalAsync(calculator);
        var result = await calculator.ResultSource.Task;

        if (result.HasValue)
        {
            focusedEntry.Text = result.Value.ToString(CultureInfo.CurrentCulture);
            _lastFocusedNumericEntry = focusedEntry;
        }
    }

    private void RegisterNumericEntryFocusTracking()
    {
        foreach (var entry in GetNumericEntries())
        {
            if (entry != null)
                entry.Focused += NumericEntry_Focused;
        }
    }

    private void NumericEntry_Focused(object sender, FocusEventArgs e)
    {
        if (sender is Entry entry)
            _lastFocusedNumericEntry = entry;
    }

    private Entry[] GetNumericEntries()
    {
        return new Entry[]
        {
            txtCalories,
            txtTotalFats,
            txtSaturatedFats,
            txtMonounsaturatedFats,
            txtPolyunsaturatedFats,
            txtFoodCarbohydrates,
            txtSugar,
            txtFibers,
            txtProteins,
            txtSalt,
            txtCholesterol,
            txtPotassium,
            txtGlycemicIndex,
            txtGramsPerUnit,
            txtContentWeight
        };
    }

    private Entry GetFocusedNumericEntry()
    {
        foreach (var entry in GetNumericEntries())
        {
            if (entry != null && entry.IsFocused)
                return entry;
        }

        return _lastFocusedNumericEntry;
    }

    private static bool TryParseNumericValue(string text, out double value)
    {
        return double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value)
            || double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out value);
    }

    private async void btnFatSecret_Clicked(object sender, EventArgs e)
    {
        try
        {
            string searchQuery = CurrentFood?.Name ?? string.Empty;
            var fatSecretPage = new FatSecretSearchPage(searchQuery);
            await Navigation.PushModalAsync(new NavigationPage(fatSecretPage));

            // Wait for the page to complete and check if a food was selected
            bool foodSelected = await fatSecretPage.PageClosedTask;
            if (foodSelected && fatSecretPage.SelectedFood != null)
            {
                // Import the selected food data into the current food
                ImportFatSecretFoodData(fatSecretPage.SelectedFood);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("FoodPage | btnFatSecret_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }

    private async void btnBarCode_Clicked(object sender, EventArgs e)
    {
        try
        {
            var scannerPage = new BarcodeScannerPage();
            await Navigation.PushModalAsync(scannerPage);

            bool barcodeScanned = await scannerPage.PageClosedTask;
            if (!barcodeScanned || string.IsNullOrWhiteSpace(scannerPage.ScannedBarcode))
                return;

            string scannedCode = scannerPage.ScannedBarcode;

            // Search in the local database
            Food foundFood = bl.SearchFoodByBarcode(scannedCode);
            if (foundFood != null)
            {
                // Food found: load all its data into the form
                CurrentFood = foundFood;
                this.BindingContext = null;
                this.BindingContext = CurrentFood;
                _hasUnsavedBarcode = false;
                LoadUnitPicker();
                LoadManufacturerPicker();
                LoadCategoryPicker();
            }
            else
            {
                // Food not found: just store the barcode on the current food
                CurrentFood.Barcode = scannedCode;
                this.BindingContext = null;
                this.BindingContext = CurrentFood;
                _hasUnsavedBarcode = true;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("FoodPage | btnBarCode_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }

    private void ImportFatSecretFoodData(FatSecretFood fatSecretFood)
    {
        if (!string.IsNullOrEmpty(fatSecretFood.Name))
            CurrentFood.Name = fatSecretFood.Name;

        if (!string.IsNullOrEmpty(fatSecretFood.Description))
            CurrentFood.Description = fatSecretFood.Description;

        // Brand / Manufacturer and food category (food_sub_categories from FatSecret,
        // e.g. "Dairy Products"): they are copied only into the fields of the food, as if the
        // user had typed them; they get into the lists of the pickers only with the "+" buttons
        if (!string.IsNullOrEmpty(fatSecretFood.BrandName))
            CurrentFood.Manufacturer = fatSecretFood.BrandName;

        if (!string.IsNullOrEmpty(fatSecretFood.Category))
            CurrentFood.Category = fatSecretFood.Category;

        if (fatSecretFood.Calories.HasValue)
            CurrentFood.Energy.Double = fatSecretFood.Calories;

        if (fatSecretFood.CarbohydratesPercent.HasValue)
            CurrentFood.CarbohydratesPercent.Double = fatSecretFood.CarbohydratesPercent;

        if (fatSecretFood.ProteinsPercent.HasValue)
            CurrentFood.ProteinsPercent.Double = fatSecretFood.ProteinsPercent;

        if (fatSecretFood.TotalFatsPercent.HasValue)
            CurrentFood.TotalFatsPercent.Double = fatSecretFood.TotalFatsPercent;

        if (fatSecretFood.SaturatedFatsPercent.HasValue)
            CurrentFood.SaturatedFatsPercent.Double = fatSecretFood.SaturatedFatsPercent;

        if (fatSecretFood.FibersPercent.HasValue)
            CurrentFood.FibersPercent.Double = fatSecretFood.FibersPercent;

        if (fatSecretFood.SugarPercent.HasValue)
            CurrentFood.SugarPercent.Double = fatSecretFood.SugarPercent;

        if (fatSecretFood.SodiumPercent.HasValue)
            CurrentFood.SaltPercent.Double = fatSecretFood.SodiumPercent;

        // Refresh the bindings
        this.BindingContext = null;
        this.BindingContext = CurrentFood;
    }
}

