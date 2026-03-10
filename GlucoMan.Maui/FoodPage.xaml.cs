using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class FoodPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    internal Food CurrentFood { get; set; }

    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);
    public bool FoodIsChosen { get; private set; }
    private bool _hasUnsavedBarcode;

    internal FoodPage(Food Food)
    {
        InitializeComponent();
        FoodIsChosen = false;
        CurrentFood = Food;
        _taskCompletionSource = new TaskCompletionSource<bool>();

        this.BindingContext = Food;

        //if (Food.GramsInOneUnit.Double != 1)
        //{
        //    blMeal.UpdateNutrientsDataWithNewUnit();
        //}

        // bind to cmbUnit the units of this food, retrieved from the business layer
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        cmbUnit.ItemDisplayBinding = new Binding("Symbol");
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;

        LoadManufacturerPicker();
        LoadCategoryPicker();
        LoadContentUnitPicker();
    }

    private void LoadManufacturerPicker()
    {
        var manufacturers = bl.GetAllManufacturersOfOneFood(CurrentFood);
        manufacturers.Insert(0, new Manufacturer());
        cmbManufacturer.ItemDisplayBinding = new Binding("Name");
        cmbManufacturer.ItemsSource = manufacturers;
        cmbManufacturer.SelectedIndex = 0;
    }

    private void LoadCategoryPicker()
    {
        var categories = bl.GetAllCategoriesOfOneFood(CurrentFood);
        categories.Insert(0, new CategoryOfFood());
        cmbCategory.ItemDisplayBinding = new Binding("Name");
        cmbCategory.ItemsSource = categories;
        cmbCategory.SelectedIndex = 0;
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
        string manufacturerName = txtFoodManufacturer.Text?.Trim();
        if (string.IsNullOrEmpty(manufacturerName))
            return;

        // Case-insensitive duplicate check against the currently loaded list
        if (cmbManufacturer.ItemsSource is IEnumerable<Manufacturer> existing &&
            existing.Any(m => string.Equals(m.Name, manufacturerName, StringComparison.OrdinalIgnoreCase)))
            return;

        bl.AddManufacturerToCurrentFood(new Manufacturer(manufacturerName), CurrentFood);
        LoadManufacturerPicker();
    }
    private void btnCategory_Clicked(object sender, EventArgs e)
    {
        try
        {
            string newCategoryName = txtCategory.Text?.Trim();
            if (string.IsNullOrEmpty(newCategoryName))
                return;

            // Case-insensitive duplicate check against the currently loaded list
            if (cmbCategory.ItemsSource is IEnumerable<CategoryOfFood> existing &&
                existing.Any(c => string.Equals(c.Name, newCategoryName, StringComparison.OrdinalIgnoreCase)))
                return;

            bl.AddCategoryToCurrentFood(new CategoryOfFood(newCategoryName), CurrentFood);
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
        bool isApplicableToAllFoods = await DisplayAlert(AppStrings.UnitApplicabilityTitle,
            AppStrings.UnitApplicabilityMessage, AppStrings.UnitApplicabilityAll, AppStrings.UnitApplicabilityThis);
        if (!isApplicableToAllFoods)
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
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood((Food)this.BindingContext);
        LoadContentUnitPicker();
    }
    private void btnRemoveUnit_Clicked(object sender, EventArgs e)
    {
        bl.RemoveUnitFromFoodsUnits(CurrentFood);
        // Refresh both pickers
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(CurrentFood);
        LoadContentUnitPicker();
    }
    private void btnRemoveFoodManufacturer_Clicked(object sender, EventArgs e)
    {
        if (cmbManufacturer.SelectedItem is not Manufacturer selected || string.IsNullOrEmpty(selected.Name))
            return;
        bl.DeleteManufacturer(selected);
        if (string.Equals(CurrentFood.Manufacturer, selected.Name, StringComparison.OrdinalIgnoreCase))
            txtFoodManufacturer.Text = "";
        LoadManufacturerPicker();
    }
    private void btnRemoveFoodCategory_Clicked(object sender, EventArgs e)
    {
        if (cmbCategory.SelectedItem is not CategoryOfFood selected || string.IsNullOrEmpty(selected.Name))
            return;
        bl.DeleteCategoryOfFood(selected);
        if (string.Equals(CurrentFood.Category, selected.Name, StringComparison.OrdinalIgnoreCase))
            txtCategory.Text = "";
        LoadCategoryPicker();
    }
    private void cmbManufacturer_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbManufacturer.SelectedItem is Manufacturer m && !string.IsNullOrEmpty(m.Name))
            txtFoodManufacturer.Text = m.Name;
    }

    private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
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
                cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(CurrentFood);
                if (cmbUnit.Items.Count > 0)
                    cmbUnit.SelectedIndex = 0;
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

        // Brand / Manufacturer
        if (!string.IsNullOrEmpty(fatSecretFood.BrandName))
        {
            CurrentFood.Manufacturer = fatSecretFood.BrandName;
            // Register the manufacturer in the DB so it appears in the picker
            bl.AddManufacturerToCurrentFood(new Manufacturer(fatSecretFood.BrandName), CurrentFood);
            LoadManufacturerPicker();
        }

        // Food category (food_sub_categories from FatSecret, e.g. "Dairy Products")
        if (!string.IsNullOrEmpty(fatSecretFood.Category))
        {
            CurrentFood.Category = fatSecretFood.Category;
            bl.AddCategoryToCurrentFood(new CategoryOfFood(fatSecretFood.Category), CurrentFood);
            LoadCategoryPicker();
        }

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

