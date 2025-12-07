using gamon;
using GlucoMan.BusinessLayer;
using System.Globalization;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class FoodsPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
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
        // if a specific food is passed, load its persistent data from database 
        // if what is passed has not and IdFood,
        // we use the data actually passed 
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
    private void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        loading = true;
        //make the tapped row the current food
        Food = (Food)gridFoods.SelectedItem;
        this.BindingContext = Food;
        FromClassToUi();
        // fill the combo box of UnitSymbol
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        // set the selected item to first
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;
        loading = false;
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
            FromClassToUi();
        }
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
        //bl.SaveOneFood(Food);

        // Set the result and close the page
        _taskCompletionSource?.SetResult(true);

        await this.Navigation.PopModalAsync();
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
            // Return to calling page without saving changes or passing any data
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
