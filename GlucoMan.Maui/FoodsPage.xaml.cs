using gamon;
using GlucoMan;
using System.Globalization;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class FoodsPage : ContentPage
{
    BL_MealAndFood bl = new();
    internal Food Food { get; set; }
    bool foodIsChosen = false;
    public bool FoodIsChosen { get => foodIsChosen; }

    // Add TaskCompletionSource to handle page completion
    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);

    List<Food> allFoods;
    private FoodPage foodPage;
    private bool loading = false;
    private Recipe recipe;

    internal FoodsPage(Food Food)
    {
        InitializeComponent();
        this.Food = Food;
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }
    internal FoodsPage(string FoodNameForSearch, string FoodDescriptionForSearch)
    {
        InitializeComponent();
        if (Food == null)
            Food = new Food(new UnitOfFood("g", 1));
        Food.Name = FoodNameForSearch;
        Food.Description = FoodDescriptionForSearch;
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }
    public FoodsPage(FoodInMeal FoodInMeal)
    {
        InitializeComponent();
        if (Food == null)
            Food = new Food(new UnitOfFood("g", 1));
        bl.FromFoodInMealToFood(FoodInMeal, Food);
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }
    public FoodsPage(Ingredient Ingredient)
    {
        InitializeComponent();
        if (Food == null)
            Food = new Food(new UnitOfFood("g", 1));
        bl.FromIngredientToFood(Ingredient, Food);
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }
    private void PageLoad(object sender, EventArgs e)
    {
        loading = true;

        foodIsChosen = false;
        //txtName.Text = "";
        //txtDescription.Text = "";
        //Food.Name = "";
        //Food.Description = "";
        allFoods = new List<Food>();
        // if a specific food is passed, load its persistent Data from database 
        // if what is passed has not and IdFood,
        // we use the Data actually passed 
        if (Food.IdFood != 0 && Food.IdFood != null)
        {
            Food = bl.GetOneFood(Food.IdFood);
        }
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        // let's show the Food
        FromClassToUi();
        this.BindingContext = Food;

        // Set CHO% text, handling NaN case
        if (Food.CarbohydratesPercent != null && Food.CarbohydratesPercent.Double.HasValue)
        {
            txtFoodCarbohydrates.Text = Food.CarbohydratesPercent.Text;
        }
        else
        {
            txtFoodCarbohydrates.Text = "";
        }

        //gridFoods.ItemsSource = glucoseReadings;
        loading = false;
    }
    private void OnGridSelectionAsync(object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        loading = true;

        var selectedFood = (Food)e.CurrentSelection[0];

        // Deselect all other items in the list
        if (allFoods != null)
        {
            foreach (var food in allFoods)
            {
                food.IsSelectedInList = false;
            }
        }

        // Select the current item
        selectedFood.IsSelectedInList = true;

        // Make the tapped row the current food
        Food = selectedFood;
        this.BindingContext = Food;
        FromClassToUi();
        // Fill the combo box of UnitSymbol
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        // Set the selected item to first
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;
        loading = false;
    }

    // Support a direct tap on the item Frame to set selection (helps on Android)
    private void OnItemTapped(object? sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Food tapped)
        {
            gridFoods.SelectedItem = tapped;
        }
    }
    private void FromClassToUi()
    {
        txtIdFood.Text = Food.IdFood.ToString();
        txtName.Text = Food.Name;
        txtDescription.Text = Food.Description;

        // Handle CHO% display, avoiding NaN
        if (Food.CarbohydratesPercent != null && Food.CarbohydratesPercent.Double.HasValue)
        {
            txtFoodCarbohydrates.Text = Food.CarbohydratesPercent.Text;
        }
        else
        {
            txtFoodCarbohydrates.Text = "";
        }
    }
    private void FromUiToClass()
    {
        Food.IdFood = Safe.Int(txtIdFood.Text);
        Food.Name = txtName.Text;
        Food.Description = txtDescription.Text;
        Food.CarbohydratesPercent.Text = txtFoodCarbohydrates.Text;
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private void RefreshGrid()
    {
        if (Food.Name != "" && Food.Description != "")
            allFoods = bl.SearchFoods(Food.Name, Food.Description, 0);
        gridFoods.ItemsSource = allFoods;
    }
    private async void btnFoodDetails_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        foodPage = new FoodPage(Food);

        // Can be navigated as modal or regular - both work now
        await Navigation.PushModalAsync(foodPage);

        // Wait for the page to be closed and get the result
        bool foodWasChosen = await foodPage.PageClosedTask;

        // Check if the user chose/confirmed the food
        if (foodWasChosen && foodPage.FoodIsChosen)
        {
            bl.FromFoodToFoodInMeal(foodPage.CurrentFood, bl.FoodInMeal);
            Food = foodPage.CurrentFood;
            FromClassToUi();
        }

        // Always refresh the units combo: units may have been added/removed in FoodPage
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;
    }
    private async void btnSaveFood_Click(object sender, EventArgs e)
    {
        if (txtIdFood.Text == "")
        {
            await DisplayAlert(AppStrings.SelectOneFoodFromList, AppStrings.ChooseFoodToSave, AppStrings.OK);
            return;
        }

        FromUiToClass();

        // Validate that food name is not empty
        if (string.IsNullOrWhiteSpace(Food.Name))
        {
            await DisplayAlert(AppStrings.Error, AppStrings.FoodNameCannotBeEmpty, AppStrings.OK);
            txtName.Focus();
            return;
        }

        bl.SaveOneFood(Food);
        FromClassToUi();
        RefreshUi();
    }
    private void btnAddFood_Click(object sender, EventArgs e)
    {
        FromUiToClass();

        // Validate that food name is not empty
        if (string.IsNullOrWhiteSpace(Food.Name))
        {
            DisplayAlert(AppStrings.Error, AppStrings.FoodNameCannotBeEmpty, AppStrings.OK);
            txtName.Focus();
            return;
        }

        // Control if txtFoodCarbohydrates.Text is a number
        double carbs;
        Double.TryParse((string?)txtFoodCarbohydrates.Text, out carbs);
        // Validate carbohydrates numeric
        if (string.IsNullOrWhiteSpace(txtFoodCarbohydrates.Text) ||
                  carbs == 0)
        {
            DisplayAlert(AppStrings.Error, AppStrings.CarbohydratesMustBeSet, AppStrings.OK);
            txtFoodCarbohydrates.Focus();
            return;
        }

        // Set carbohydrates value
        if (Food.CarbohydratesPercent == null) Food.CarbohydratesPercent = new DoubleAndText();
        Food.CarbohydratesPercent.Double = carbs;

        // Nulls the ID of food to create a new one
        Food.IdFood = null;
        bl.SaveOneFood(Food);
        btnSearchFood_Click(null, null);
        RefreshUi();
    }
    private async void btnRemoveFood_Click(object sender, EventArgs e)
    {
        if (Food == null)
            return;

        string message = string.Format(AppStrings.DeleteFoodConfirm,
            Food.Name ?? string.Empty,
            Food.CarbohydratesPercent?.ToString() ?? string.Empty,
            Food.IdFood?.ToString() ?? string.Empty);

        bool remove = await DisplayAlert(AppStrings.ConfirmDelete, message, AppStrings.Yes, AppStrings.No);
        if (remove)
        {
            bl.DeleteOneFood(Food);
            RefreshUi();
        }
    }
    private void btnSearchFood_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        allFoods = bl.SearchFoods(Food.Name, Food.Description, 0);
        gridFoods.ItemsSource = allFoods;
    }
    private async void btnChoose_Click(object sender, EventArgs e)
    {
        FromUiToClass();

        // Validate that food name is not empty before choosing
        if (string.IsNullOrWhiteSpace(Food.Name))
        {
            await DisplayAlert(AppStrings.Error, AppStrings.FoodNameCannotBeEmptyBeforeChoosing, AppStrings.OK);
            txtName.Focus();
            return;
        }

        foodIsChosen = true;
        //blMeal.SaveOneFood(Food);

        // Set the result and close the page
        _taskCompletionSource?.SetResult(true);

        // Check if this was opened as modal or regular navigation
        var navigation = Navigation;
        if (navigation.ModalStack.Contains(this))
        {
            await navigation.PopModalAsync();
        }
        else if (navigation.NavigationStack.Count > 1)
        {
            await navigation.PopAsync();
        }
        // If neither modal nor in navigation stack, do nothing (page opened standalone)
    }
    private void btnClearFields_Click(object sender, EventArgs e)
    {
        loading = true;
        txtIdFood.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        txtFoodCarbohydrates.Text = "";

        Food.Name = "";
        Food.Description = "";

        //txtCalories.Text = "";
        //txtTotalFats.Text = "";
        //txtSaturatedFats.Text = "";
        //txtSugar.Text = "";
        //txtFibers.Text = "";
        //txtProteins.Text = "";
        //txtSalt.Text = "";
        loading = false;
        FromUiToClass();
        RefreshUi();
    }
    private async void btnBack_Click(object sender, EventArgs e)
    {
        try
        {
            // Return to calling page without saving changes or passing any Data
            foodIsChosen = false;

            // Set the result to false (no food chosen)
            _taskCompletionSource?.SetResult(false);

            // Try to close as modal first, if that fails try regular pop
            if (Navigation.ModalStack.Count > 0)
            {
                await Navigation.PopModalAsync();
            }
            else if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
            else
            {
                // If we can't navigate back, just log it
                General.LogOfProgram?.Debug("FoodsPage - btnBack_Click: Cannot navigate back, no pages in stack");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("FoodsPage - btnBack_Click", ex);
            await DisplayAlert(AppStrings.Error, string.Format(AppStrings.CannotClosePage, ex.Message), AppStrings.OK);
        }
    }
    private void txtName_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            Food.Name = txtName.Text;
            allFoods = bl.SearchFoods(txtName.Text, txtDescription.Text, 3);
            if (allFoods != null)
            {
                gridFoods.ItemsSource = allFoods;
            }
        }
    }
    private void txtName_Unfocused(object sender, FocusEventArgs e)
    {
        txtName.CursorPosition = 0;
    }
    private void txtDescription_TextChanged(object sender, EventArgs e)
    {
        if (!loading)
        {
            allFoods = bl.SearchFoods(txtName.Text, txtDescription.Text, 3);
            if (allFoods != null)
            {
                gridFoods.ItemsSource = allFoods;
            }
        }
    }
    protected override async void OnAppearing()
    {
        foodIsChosen = false;
    }
    protected override bool OnBackButtonPressed()
    {
        // Handle back button press - user cancelled
        _taskCompletionSource?.SetResult(false);
        return base.OnBackButtonPressed();
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

            // 1. Search in the local database first.
            Food foundFood = bl.SearchFoodByBarcode(scannedCode);
            if (foundFood != null)
            {
                Food = foundFood;
                this.BindingContext = Food;
                FromClassToUi();
                cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
                if (cmbUnit.Items.Count > 0)
                    cmbUnit.SelectedIndex = 0;
                allFoods = new List<Food> { foundFood };
                gridFoods.ItemsSource = allFoods;
                return;
            }

            // 2. Not in local DB: try FatSecret.
            await DisplayAlert("", AppStrings.SearchingFatSecretForBarcode, AppStrings.OK);

            FatSecretFood fatSecretFood = null;
            try
            {
                var fatSecretService = new FatSecretService();
                fatSecretFood = await fatSecretService.FindFoodByBarcodeAsync(scannedCode);
            }
            catch (Exception fatEx)
            {
                General.LogOfProgram?.Error("FoodsPage | btnBarCode_Clicked | FatSecret", fatEx);
                await DisplayAlert(AppStrings.Error,
                    AppStrings.FatSecretBarcodeError + fatEx.Message, AppStrings.OK);
                return;
            }

            if (fatSecretFood == null)
            {
                // 3. Not found anywhere.
                await DisplayAlert(AppStrings.Error, AppStrings.BarcodeNotFoundAnywhere, AppStrings.OK);
                return;
            }

            // 4. Found in FatSecret: populate a new Food and open FoodPage for editing/saving.
            var newFood = new Food(new UnitOfFood("g", 1));
            newFood.Barcode = scannedCode;
            if (!string.IsNullOrEmpty(fatSecretFood.Name))
                newFood.Name = fatSecretFood.Name;
            if (!string.IsNullOrEmpty(fatSecretFood.Description))
                newFood.Description = fatSecretFood.Description;
            if (!string.IsNullOrEmpty(fatSecretFood.BrandName))
                newFood.Manufacturer = fatSecretFood.BrandName;
            if (!string.IsNullOrEmpty(fatSecretFood.Category))
                newFood.Category = fatSecretFood.Category;
            if (fatSecretFood.Calories.HasValue)
                newFood.Energy.Double = fatSecretFood.Calories;
            if (fatSecretFood.CarbohydratesPercent.HasValue)
                newFood.CarbohydratesPercent.Double = fatSecretFood.CarbohydratesPercent;
            if (fatSecretFood.ProteinsPercent.HasValue)
                newFood.ProteinsPercent.Double = fatSecretFood.ProteinsPercent;
            if (fatSecretFood.TotalFatsPercent.HasValue)
                newFood.TotalFatsPercent.Double = fatSecretFood.TotalFatsPercent;
            if (fatSecretFood.SaturatedFatsPercent.HasValue)
                newFood.SaturatedFatsPercent.Double = fatSecretFood.SaturatedFatsPercent;
            if (fatSecretFood.FibersPercent.HasValue)
                newFood.FibersPercent.Double = fatSecretFood.FibersPercent;
            if (fatSecretFood.SugarPercent.HasValue)
                newFood.SugarPercent.Double = fatSecretFood.SugarPercent;
            if (fatSecretFood.SodiumPercent.HasValue)
                newFood.SaltPercent.Double = fatSecretFood.SodiumPercent;

            Food = newFood;
            this.BindingContext = Food;
            FromClassToUi();
            cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
            if (cmbUnit.Items.Count > 0)
                cmbUnit.SelectedIndex = 0;
            allFoods = new List<Food> { newFood };
            gridFoods.ItemsSource = allFoods;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("FoodsPage | btnBarCode_Clicked", ex);
            await DisplayAlert(AppStrings.Error, ex.Message, AppStrings.OK);
        }
    }
    private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbUnit.SelectedItem != null && cmbUnit.SelectedItem is UnitOfFood unit)
            {
                Food.UnitSymbol = unit.Symbol;
                Food.GramsInOneUnit.Double = unit.GramsInOneUnit.Double;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("FoodsPage - cmbUnit_SelectedIndexChanged", ex);
        }
    }
}
