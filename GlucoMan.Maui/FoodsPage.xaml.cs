using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodsPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    public Food Food { get; set; }
    bool foodIsChosen = false;
    public bool FoodIsChosen { get => foodIsChosen; }

    // Add TaskCompletionSource to handle page completion
    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);

    List<Food> allFoods;
    private FoodPage foodPage;
    private bool loading = false;
    private Recipe recipe;

    public FoodsPage(Food Food)
    {
        InitializeComponent();
        this.Food = Food;
        _taskCompletionSource = new TaskCompletionSource<bool>();
    }
    public FoodsPage(string FoodNameForSearch, string FoodDescriptionForSearch)
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
        txtName.Text = "";
        txtDescription.Text = "";
        Food.Name = "";
        Food.Description = "";
        allFoods = new List<Food>();
        // if a specific food is passed, load its persistent data from database 
        if (Food.IdFood != 0 && Food.IdFood != null)
        {
            Food = bl.GetOneFood(Food.IdFood);
        }
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        // if what is passed has not and IdFood,
        // we use the data actually passed 

        // let's show the Food
        FromClassToUi();
        this.BindingContext = Food;
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
        ////txtFoodCarbohydrates.Text = Food.CarbohydratesPercent.Text;

        ////txtCalories.Text = Food.Energy.Text;
        ////txtTotalFats.Text = Food.TotalFatsPercent.Text;
        ////txtSaturatedFats.Text = Food.SaturatedFatsPercent.Text;
        ////txtSugar.Text = Food.SugarPercent.Text;
        ////txtFibers.Text = Food.FibersPercent.Text;
        ////txtProteins.Text = Food.ProteinsPercent.Text;
        ////txtSalt.Text = Food.SaltPercent.Text;
        ////txtPotassium.Text = Food.PotassiumPercent.Text;

        ////txtCholesterol.Text = Food.Cholesterol.Text;
        ////txtGlicemicIndex.Text = Food.GlycemicIndex.Text;
    }
    private void FromUiToClass()
    {
        Food.IdFood = Safe.Int(txtIdFood.Text);
        Food.Name = txtName.Text;
        Food.Description = txtDescription.Text;
        //if (cmbUnit.SelectedItem == null)
            //Food.UnitSymbol = new UnitOfFood();
        ////////else
        ////////Food.UnitSymbol = cmbUnit.SelectedItem;

        //Food.CarbohydratesPercent.Double = Safe.Double(txtFoodCarbohydrates.Text);

            //Food.Energy.Double = Safe.Double(txtCalories.Text);
            //Food.TotalFatsPercent.Double = Safe.Double(txtTotalFats.Text);
            //Food.SaturatedFatsPercent.Double = Safe.Double(txtSaturatedFats.Text);
            //Food.SugarPercent.Double = Safe.Double(txtSugar.Text);
            //Food.FibersPercent.Double = Safe.Double(txtFibers.Text);
            //Food.ProteinsPercent.Double = Safe.Double(txtProteins.Text);
            //Food.Salt.Percent.Double = Safe.Double(txtSalt.Text);
            //Food.PotassiumPercent.Double = Safe.Double(txtPotassium.Text);

            //Food.Cholesterol.Double = Safe.Double(txtCholesterol.Text);
            //Food.GlycemicIndex.Double = Safe.Double(txtGlicemicIndex.Text);
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
        await Navigation.PushAsync(foodPage);
        if (foodPage.FoodIsChosen)
        {
            bl.FromFoodToFoodInMeal(foodPage.CurrentFood, bl.FoodInMeal);
            FromClassToUi();
        }
    }
    private async void btnSaveFood_Click(object sender, EventArgs e)
    {
        if (txtIdFood.Text == "")
        {
            await DisplayAlert("Select one food from the list", "Choose a food to save", "Ok");
            return;
        }
        FromUiToClass();
        bl.SaveOneFood(Food);
        FromClassToUi();
        RefreshUi();
    }
    private void btnAddFood_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        // nulls the ID of food to create a new one
        Food.IdFood = null;
        bl.SaveOneFood(Food);
        btnSearchFood_Click(null, null);
        RefreshUi();
    }
    private void btnRemoveFood_Click(object sender, EventArgs e)
    {
        bl.DeleteOneFood(Food);
        RefreshUi();
    }
    //private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
    //{

    //}
    //private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
    //{
    //    if (e.RowIndex > 0)
    //    {
    //        Food = allFoods[e.RowIndex];
    //        gridFoods.Rows[e.RowIndex].Selected = true;
    //        RefreshUi();
    //    }
    //}
    //private void gridFoods_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    //{
    //    if (gridFoods.SelectedRows.Count == 0)
    //    {
    //        MessageBox.Show("Choose a food to save");
    //        return;
    //    }
    //}
    //private void btnFatSecret_Click(object sender, EventArgs e)
    //{
    //    //MessageBox.Show("To be implemented yet!");
    //}
    private void btnSearchFood_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        allFoods = bl.SearchFoods(Food.Name, Food.Description, 0);
        gridFoods.ItemsSource = allFoods;
    }
    private async void btnChoose_Click(object sender, EventArgs e)
    {
        foodIsChosen = true;
        FromUiToClass();
        bl.SaveOneFood(Food);
        
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
        Food.UnitSymbol = ((UnitOfFood)cmbUnit.SelectedItem).Symbol;
    }
}
