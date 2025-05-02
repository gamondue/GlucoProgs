using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    public Food CurrentFood { get; set; }
    public bool FoodIsChosen { get; internal set; }
    public FoodPage(Food Food)
    {
        InitializeComponent();
        FoodIsChosen = false;
        CurrentFood = Food;

        this.BindingContext = Food;

        //if (Food.GramsInOneUnit.Double != 1)
        //{
        //    bl.UpdateNutrientsDataWithNewUnit();
        //}

        // bind to cmbUnit the units of this food, retrieved from the business layer
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
        if (cmbUnit.Items.Count > 0)
            cmbUnit.SelectedIndex = 0;

        // just for test !!!!!!!!!!!!!!
        cmbManufacturer.ItemsSource = bl.GetAllUnitsOfOneFood(Food);

        // just for test !!!!!!!!!!!!!!
        cmbCategory.ItemsSource = bl.GetAllUnitsOfOneFood(Food);
    }
    private void btnOk_Click(object sender, EventArgs e)
    {
        FoodIsChosen = true;
        bl.SaveOneFood((Food)this.BindingContext);
        this.Navigation.PopAsync();
    }
    private void btnEscape_Clicked(object sender, EventArgs e)
    {
        FoodIsChosen = false;
        this.Navigation.PopAsync();
    }
    private void btnAbort_Click(object sender, EventArgs e)
    {
        FoodIsChosen = false;
        this.Navigation.PopAsync();
    }
    private void btnAddFoodManufacturer_Clicked(object sender, EventArgs e)
    {
    }
    private void btnCategory_Clicked(object sender, EventArgs e)
    {

    }
    private void btnAddUnit_Clicked(object sender, EventArgs e)
    {
        UnitOfFood unit = new UnitOfFood(txtUnit.Text, Convert.ToDouble(txtGramsPerUnit.Text));
        bl.AddUnitToFood(CurrentFood, unit);
        cmbUnit.ItemsSource = bl.GetAllUnitsOfOneFood((Food)this.BindingContext);
    }
    private void btnRemoveUnit_Clicked(object sender, EventArgs e)
    {
        bl.RemoveUnitFromFoodsUnits(CurrentFood);
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
        CurrentFood.Unit.Name = unit.Name;
        CurrentFood.Unit.GramsInOneUnit = unit.GramsInOneUnit;

    }
    private void txtGramsPerUnit_TextChanged(object sender, TextChangedEventArgs e)
    {

    }
    private void btnRemoveFoodManufacturer_Clicked(object sender, EventArgs e)
    {

    }
}
