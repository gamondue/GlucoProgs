using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    public Food CurrentFood { get; set; }
    
    // Add TaskCompletionSource to handle page completion
    private TaskCompletionSource<bool> _taskCompletionSource;
    public Task<bool> PageClosedTask => _taskCompletionSource?.Task ?? Task.FromResult(false);
    
    // Property to indicate if user chose/confirmed the food
    public bool FoodIsChosen { get; private set; }
    
    public FoodPage(Food Food)
    {
        InitializeComponent();
        FoodIsChosen = false;
        CurrentFood = Food;
        _taskCompletionSource = new TaskCompletionSource<bool>();

        this.BindingContext = Food;

        //if (Food.GramsInOneUnit.Double != 1)
        //{
        //    bl.UpdateNutrientsDataWithNewUnit();
        //}

        // bind to cmbUnit the units of this food, retrieved from the business layer
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;

        cmbManufacturer.ItemsSource = bl.GetAllManufacturersOfOneFood(Food);
        cmbCategory.ItemsSource = bl.GetAllCategoriesOfOneFood(Food);
    }
    private async void btnOk_Click(object sender, EventArgs e)
    {
        FoodIsChosen = true;
        bl.SaveOneFood((Food)this.BindingContext);
        
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
        // Handle back button press - user cancelled
        FoodIsChosen = false;
        _taskCompletionSource?.SetResult(false);
        return base.OnBackButtonPressed();
    }
    private void btnAddFoodManufacturer_Clicked(object sender, EventArgs e)
    {
        Manufacturer m = new Manufacturer(txtFoodManufacturer.Text);
        bl.AddManufacturerToFood(m, CurrentFood);
    }
    private void btnCategory_Clicked(object sender, EventArgs e)
    {
        try
        {
            string newCategoryName = txtCategory.Text?.Trim();
            if (string.IsNullOrEmpty(newCategoryName))
                return;
            CategoryOfFood newCategory = new CategoryOfFood(newCategoryName);
            int? idCategory = bl.AddCategoryOfFood(newCategory);
            // update category list in Picker
            cmbCategory.ItemsSource = bl.GetAllCategoriesOfOneFood(CurrentFood);
        }
        catch (Exception ex)
        {
            General.LogOfProgram.Error("FoodPage | btnCategory_Clicked", ex);
        }
    }
    private async void btnAddUnit_Clicked(object sender, EventArgs e)
    {
        double gramsPerUnit = (double)Safe.Double(txtGramsPerUnit.Text); 
        UnitOfFood unit = new UnitOfFood(txtUnit.Text, gramsPerUnit);
        // ask the user if the unit is applicable to the current food or to all foods
        bool isApplicableToAllFoods = await DisplayAlert("Unit Applicability",
            "Will this new Unit be applicable to all foods or just to this Food?", "All", "This");
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
            DisplayAlert("", "The unit symbol already exists, give the new unit a unique symbol", "Ok");
            return;
        }
        // we save the unit, if IdFood has been put to null, we mean that the UnitSymbol has to be used for any food
        if (bl.AddUnit(unit) == null)
        {
            // prompt the user that the saving of the unit failed
            DisplayAlert("", "The saving of the new unit failed. New unit not saved", "Ok");
            return;
        }
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood((Food)this.BindingContext);
    }
    private void btnRemoveUnit_Clicked(object sender, EventArgs e)
    {
        bl.RemoveUnitFromFoodsUnits(CurrentFood);
    }
    private async void btnRemoveFoodManufacturer_Clicked(object sender, EventArgs e)
    {
        // check which control has the focus

        if (bl.GetAllManufacturersOfOneFood(CurrentFood).Count == 1)
        {
            txtFoodManufacturer.Text = "";
            return;
        }
        else if (cmbManufacturer.SelectedItem == null)
        {
            await DisplayAlert("Error", "Please select a manufacturer to remove", "Ok");
            return;
        }
        else
        {
            bool deleteFromThisFood = await DisplayAlert(Title, "Do you want to delete the manufacturer from the whole database or just from this food?", "This", "Whole");
            if (deleteFromThisFood)
            {
                bl.RemoveManufacturerFromFood((Manufacturer)cmbManufacturer.SelectedItem, CurrentFood);
            }
            else
            {
                txtFoodManufacturer.Text = "";
            }
        }
    }
    private void btnRemoveFoodCategory_Clicked(object sender, EventArgs e)
    {
        if (cmbCategory.SelectedItem is CategoryOfFood selectedCategory)
        {
            bl.RemoveCategoryFromFood(selectedCategory, CurrentFood);
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
        UnitOfFood unit = (UnitOfFood)cmbUnit.SelectedItem;
        CurrentFood.UnitSymbol = unit.Symbol;
        CurrentFood.GramsInOneUnit.Double = unit.GramsInOneUnit.Double;
    }
    private void txtGramsPerUnit_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
}
