using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodsPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    public Food CurrentFood { get; set; }
    bool foodIsChosen = false;
    public bool FoodIsChosen { get => foodIsChosen; }

    List<Food> allFoods;
    private FoodPage foodPage;
    private bool loading = false;
    private Recipe recipe;

    public FoodsPage(Food Food)
    {
        InitializeComponent();
        CurrentFood = Food;
    }
    public FoodsPage(string FoodNameForSearch, string FoodDescriptionForSearch)
    {
        InitializeComponent();
        if (CurrentFood == null)
            CurrentFood = new Food(new UnitOfFood("g", 1));
        CurrentFood.Name = FoodNameForSearch;
        CurrentFood.Description = FoodDescriptionForSearch;
    }
    public FoodsPage(FoodInMeal FoodInMeal)
    {
        InitializeComponent();
        if (CurrentFood == null)
            CurrentFood = new Food(new UnitOfFood("g", 1));
        bl.FromFoodInMealToFood(FoodInMeal, CurrentFood);
    }
    public FoodsPage(Ingredient Ingredient)
    {
        InitializeComponent();
        if (CurrentFood == null)
            CurrentFood = new Food(new UnitOfFood("g", 1));
        bl.FromIngredientToFood(Ingredient, CurrentFood);
    }
    private void PageLoad(object sender, EventArgs e)
    {
        loading = true;

        foodIsChosen = false;
        txtName.Text = "";
        txtDescription.Text = "";
        CurrentFood.Name = "";
        CurrentFood.Description = "";
        allFoods = new List<Food>();
        // if a specific food is passed, load its persistent data from database 
        if (CurrentFood.IdFood != 0 && CurrentFood.IdFood != null)
        {
            CurrentFood = bl.GetOneFood(CurrentFood.IdFood);
        }
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(CurrentFood);
        // if what is passed has not and IdFood,
        // we use the data actually passed 

        // let's show the CurrentFood
        FromClassToUi();
        this.BindingContext = CurrentFood;
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
        CurrentFood = (Food)gridFoods.SelectedItem;
        this.BindingContext = CurrentFood;
        FromClassToUi();
        // fill the combo box of Unit
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(CurrentFood);
        // set the selected item to first
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;
        loading = false;
    }
    private void FromClassToUi()
    {
        txtIdFood.Text = CurrentFood.IdFood.ToString();
        txtName.Text = CurrentFood.Name;
        txtDescription.Text = CurrentFood.Description;
        ////txtFoodCarbohydrates.Text = CurrentFood.CarbohydratesPerUnit.Text;

        ////txtCalories.Text = CurrentFood.Energy.Text;
        ////txtTotalFats.Text = CurrentFood.TotalFatsPercent.Text;
        ////txtSaturatedFats.Text = CurrentFood.SaturatedFatsPercent.Text;
        ////txtSugar.Text = CurrentFood.SugarPercent.Text;
        ////txtFibers.Text = CurrentFood.FibersPercent.Text;
        ////txtProteins.Text = CurrentFood.ProteinsPercent.Text;
        ////txtSalt.Text = CurrentFood.SaltPercent.Text;
        ////txtPotassium.Text = CurrentFood.PotassiumPercent.Text;

        ////txtCholesterol.Text = CurrentFood.CholesterolPercent.Text;
        ////txtGlicemicIndex.Text = CurrentFood.GlycemicIndex.Text;
    }
    private void FromUiToClass()
    {
        CurrentFood.IdFood = SqlSafe.Int(txtIdFood.Text);
        CurrentFood.Name = txtName.Text;
        CurrentFood.Description = txtDescription.Text;
        if (cmbUnit.SelectedItem == null)
            CurrentFood.Unit = new UnitOfFood();
        ////////else
        ////////CurrentFood.Unit = cmbUnit.SelectedItem;

        //CurrentFood.CarbohydratesPerUnit.Double = SqlSafe.Double(txtFoodCarbohydrates.Text);

            //CurrentFood.Energy.Double = SqlSafe.Double(txtCalories.Text);
            //CurrentFood.TotalFatsPercent.Double = SqlSafe.Double(txtTotalFats.Text);
            //CurrentFood.SaturatedFatsPercent.Double = SqlSafe.Double(txtSaturatedFats.Text);
            //CurrentFood.SugarPercent.Double = SqlSafe.Double(txtSugar.Text);
            //CurrentFood.FibersPercent.Double = SqlSafe.Double(txtFibers.Text);
            //CurrentFood.ProteinsPercent.Double = SqlSafe.Double(txtProteins.Text);
            //CurrentFood.SaltPercent.Double = SqlSafe.Double(txtSalt.Text);
            //CurrentFood.PotassiumPercent.Double = SqlSafe.Double(txtPotassium.Text);

            //CurrentFood.CholesterolPercent.Double = SqlSafe.Double(txtCholesterol.Text);
            //CurrentFood.GlycemicIndex.Double = SqlSafe.Double(txtGlicemicIndex.Text);
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private void RefreshGrid()
    {
        if (CurrentFood.Name != "" && CurrentFood.Description != "")
            allFoods = bl.SearchFoods(CurrentFood.Name, CurrentFood.Description, 0);
        gridFoods.ItemsSource = allFoods;
    }
    private async void btnFoodDetails_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        foodPage = new FoodPage(CurrentFood);
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
        bl.SaveOneFood(CurrentFood);
        FromClassToUi();
        RefreshUi();
    }
    private void btnAddFood_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        // nulls the ID of food to create a new one
        CurrentFood.IdFood = null;
        bl.SaveOneFood(CurrentFood);
        btnSearchFood_Click(null, null);
        RefreshUi();
    }
    private void btnRemoveFood_Click(object sender, EventArgs e)
    {
        bl.DeleteOneFood(CurrentFood);
        RefreshUi();
    }
    //private void gridFoods_CellContentClick(object sender, DataGridViewCellEventArgs e)
    //{

    //}
    //private void gridFoods_CellClick(object sender, DataGridViewCellEventArgs e)
    //{
    //    if (e.RowIndex > 0)
    //    {
    //        CurrentFood = allFoods[e.RowIndex];
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
        allFoods = bl.SearchFoods(CurrentFood.Name, CurrentFood.Description, 0);
        gridFoods.ItemsSource = allFoods;
    }
    private void btnChoose_Click(object sender, EventArgs e)
    {
        foodIsChosen = true;
        FromUiToClass();
        bl.SaveOneFood(CurrentFood);
        this.Navigation.PopAsync();
    }
    private void btnCleanFields_Click(object sender, EventArgs e)
    {
        loading = true;
        txtIdFood.Text = "";
        txtName.Text = "";
        txtDescription.Text = "";
        txtFoodCarbohydrates.Text = "";

        CurrentFood.Name = "";
        CurrentFood.Description = "";

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
            CurrentFood.Name = txtName.Text;
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
    private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
    {
        CurrentFood.Unit = (UnitOfFood)cmbUnit.SelectedItem;
    }
}
