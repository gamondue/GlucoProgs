using gamon;
using GlucoMan;
using System.Globalization;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class FoodsInMealSearchResultsPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    string nameToMatch;
    internal Food Food { get; set; }
    bool foodIsChosen = false;
    public bool FoodIsChosen { get => foodIsChosen; }

    // Add TaskCompletionSource to handle page completion
    private TaskCompletionSource<FoodInMeal?> _taskCompletionSource;
    public Task<FoodInMeal?> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult<FoodInMeal?>(null);

    // Current FoodInMeal being edited
    internal FoodInMeal CurrentFoodInMeal { get; set; }

    List<FoodInMeal> allFoundFoodsInMeal;
    private FoodPage foodPage;
    private bool loading = false;
    private Recipe recipe;

    internal FoodsInMealSearchResultsPage(FoodInMeal foodInMeal)
    {
        InitializeComponent();
        CurrentFoodInMeal = foodInMeal ?? new FoodInMeal();
        this.nameToMatch = CurrentFoodInMeal.Name ?? "";
        if (Food == null)
            Food = new Food(new UnitOfFood("g", 1));
        _taskCompletionSource = new TaskCompletionSource<FoodInMeal?>();
    }
    private void PageLoad(object sender, EventArgs e)
    {
        loading = true;

        foodIsChosen = false;

        allFoundFoodsInMeal = bl.GetAllMatchingFoodsInMeals(nameToMatch) ?? new List<FoodInMeal>();

        // bind results to grid
        gridFoods.ItemsSource = allFoundFoodsInMeal;
        // initialize Food from the incoming FoodInMeal so bindings show the passed values
        if (Food == null)
            Food = new Food(new UnitOfFood("g", 1));

        if (CurrentFoodInMeal != null)
        {
            if (!string.IsNullOrWhiteSpace(CurrentFoodInMeal.Name))
                Food.Name = CurrentFoodInMeal.Name;
            if (CurrentFoodInMeal.CarbohydratesPercent != null)
            {
                if (Food.CarbohydratesPercent == null)
                    Food.CarbohydratesPercent = new DoubleAndText();
                Food.CarbohydratesPercent.Double = CurrentFoodInMeal.CarbohydratesPercent.Double;
                Food.CarbohydratesPercent.Text = CurrentFoodInMeal.CarbohydratesPercent.Text;
            }
        }

        // set binding context AFTER populating Food so XAML bindings reflect the passed values
        this.BindingContext = Food;
        // initialize UI
        FromClassToUi();
        loading = false;
    }
    private void OnGridSelectionAsync(object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        loading = true;

        var selectedFood = (FoodInMeal)e.CurrentSelection[0];

        // Deselect all other items in the list
        if (allFoundFoodsInMeal != null)
        {
            foreach (var food in allFoundFoodsInMeal)
            {
                food.IsSelectedInList = false;
            }
        }

        // Select the current item
        selectedFood.IsSelectedInList = true;

        // Map selected FoodInMeal to Food for UI
        Food = new Food(new UnitOfFood("g", 1));
        bl.FromFoodInMealToFood(selectedFood, Food);
        this.BindingContext = Food;
        FromClassToUi();
        // Fill the combo box of UnitSymbol
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        // Set the selected item to first
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;
        // Ensure the top Entry fields show the selected item's values (name and carbs)
        try
        {
            txtName.Text = selectedFood.Name ?? string.Empty;
            string carbsText = selectedFood.CarbohydratesPercent?.Text;
            if (string.IsNullOrWhiteSpace(carbsText))
            {
                // attempt to use numeric value if available
                try
                {
                    var d = selectedFood.CarbohydratesPercent?.Double;
                    if (d != null)
                        carbsText = d.ToString();
                }
                catch { }
            }
            txtFoodCarbohydrates.Text = carbsText ?? string.Empty;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("FoodsInMealSearchResultsPage - OnGridSelectionAsync mapping to entries", ex);
        }
        loading = false;
    }

    // Support a direct tap on the item Frame to set selection (helps on Android)
    private FoodInMeal? _lastTappedItem = null;
    private DateTime _lastTapTime = DateTime.MinValue;
    private readonly TimeSpan _doubleTapThreshold = TimeSpan.FromMilliseconds(500);

    private async void OnItemTapped(object? sender, EventArgs e)
    {
        if (!(sender is Frame frame) || !(frame.BindingContext is FoodInMeal tapped))
            return;

        // Set selection
        gridFoods.SelectedItem = tapped;

        // Detect double tap: same item tapped twice within threshold
        var now = DateTime.Now;
        bool isSameItem = false;
        if (_lastTappedItem != null)
        {
            // Prefer comparing by IdFoodInMeal when available, otherwise by Name (case-insensitive)
            if (_lastTappedItem.IdFoodInMeal != null && tapped.IdFoodInMeal != null)
                isSameItem = _lastTappedItem.IdFoodInMeal == tapped.IdFoodInMeal;
            else
                isSameItem = string.Equals(_lastTappedItem.Name, tapped.Name, StringComparison.OrdinalIgnoreCase);
        }

        if (isSameItem && (now - _lastTapTime) <= _doubleTapThreshold)
        {
            // Reset last tap to avoid re-entrance
            _lastTappedItem = null;
            _lastTapTime = DateTime.MinValue;

            // Perform double-tap action: open the Meal page if IdMeal is present
            try
            {
                int? idMeal = tapped.IdMeal;
                if (idMeal == null)
                    return;

                var meal = bl.GetOneMeal(idMeal);
                if (meal == null)
                    return;

                var mealPage = new MealPage(meal);
                await Navigation.PushAsync(mealPage);
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("FoodsInMealSearchResultsPage - OnItemTapped double-tap action", ex);
            }
        }
        else
        {
            // record this tap
            _lastTappedItem = tapped;
            _lastTapTime = now;
        }
    }

    // Handle double tap: open the corresponding Meal page for the meal that contains this FoodInMeal
    private async void OnItemDoubleTapped(object sender, EventArgs e)
    {
        try
        {
            if (sender is Frame frame && frame.BindingContext is FoodInMeal tapped)
            {
                int? idMeal = tapped.IdMeal;
                if (idMeal == null)
                    return; // nothing to open

                // Get meal from business layer
                var meal = bl.GetOneMeal(idMeal);
                if (meal == null)
                    return;

                // Open MealPage (assuming it has a constructor accepting Meal)
                var mealPage = new MealPage(meal);
                await Navigation.PushAsync(mealPage);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("FoodsInMealSearchResultsPage - OnItemDoubleTapped", ex);
        }
    }
    private void FromClassToUi()
    {
        // Name: prefer Food.Name (set after selecting an item), otherwise fallback to CurrentFoodInMeal or initial nameToMatch
        txtName.Text = !string.IsNullOrWhiteSpace(Food?.Name) ? Food.Name : (CurrentFoodInMeal?.Name ?? nameToMatch);
        // Carbohydrates: prefer Food value, otherwise fallback to CurrentFoodInMeal
        txtFoodCarbohydrates.Text = Food?.CarbohydratesPercent?.Text ?? CurrentFoodInMeal?.CarbohydratesPercent?.Text ?? string.Empty;
    }
    private void FromUiToClass()
    {
        if (Food == null)
            Food = new Food(new UnitOfFood("g", 1));

        Food.Name = txtName.Text;
        double carbs;
        double.TryParse(txtFoodCarbohydrates.Text, out carbs);
        if (Food.CarbohydratesPercent == null) Food.CarbohydratesPercent = new DoubleAndText();
        Food.CarbohydratesPercent.Double = carbs;
        Food.CarbohydratesPercent.Text = txtFoodCarbohydrates.Text;
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private void RefreshGrid()
    {
        string searchName = !string.IsNullOrWhiteSpace(txtName.Text) ? txtName.Text : nameToMatch;
        allFoundFoodsInMeal = bl.GetAllMatchingFoodsInMeals(searchName) ?? new List<FoodInMeal>();
        gridFoods.ItemsSource = allFoundFoodsInMeal;
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
            FromClassToUi();
        }
    }
    private async void btnSaveFood_Click(object sender, EventArgs e)
    {
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
        string searchName = !string.IsNullOrWhiteSpace(Food?.Name) ? Food.Name : nameToMatch;
        allFoundFoodsInMeal = bl.GetAllMatchingFoodsInMeals(searchName) ?? new List<FoodInMeal>();
        gridFoods.ItemsSource = allFoundFoodsInMeal;
    }
    private async void btnChoose_Click(object sender, EventArgs e)
    {
        FromUiToClass();

        // Validate that food name is not empty before choosing
        if (string.IsNullOrWhiteSpace(txtName.Text))
        {
            await DisplayAlert(AppStrings.Error, AppStrings.FoodNameCannotBeEmptyBeforeChoosing, AppStrings.OK);
            txtName.Focus();
            return;
        }

        // Validate carbohydrates numeric
        if (!double.TryParse(txtFoodCarbohydrates.Text, out double carbs) || carbs == 0)
        {
            await DisplayAlert(AppStrings.Error, AppStrings.CarbohydratesMustBeSet, AppStrings.OK);
            txtFoodCarbohydrates.Focus();
            return;
        }

        foodIsChosen = true;

        // Create result FoodInMeal with only Name and CarbohydratesPercent
        var result = new FoodInMeal
        {
            Name = txtName.Text,
            CarbohydratesPercent = new DoubleAndText { Double = carbs }
        };

        // Set the result and close the page
        _taskCompletionSource?.SetResult(result);

        await this.Navigation.PopModalAsync();
    }
    private void btnClearFields_Click(object sender, EventArgs e)
    {
        loading = true;

        txtName.Text = "";

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

            // Set the result to null (no food chosen)
            _taskCompletionSource?.SetResult(null);

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
    protected override async void OnAppearing()
    {
        foodIsChosen = false;
    }
    protected override bool OnBackButtonPressed()
    {
        // Handle back button press - user cancelled
        _taskCompletionSource?.SetResult(null);
        return base.OnBackButtonPressed();
    }
    private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cmbUnit.SelectedItem != null && cmbUnit.SelectedItem is UnitOfFood unit)
            {
                Food.UnitSymbol = unit.Symbol;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("FoodsPage - cmbUnit_SelectedIndexChanged", ex);
        }
    }
}
