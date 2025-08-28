using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodToHitTargetCarbsPage : ContentPage
{
    BL_FoodToHitTargetCarbs blFoodToEat = new BL_FoodToHitTargetCarbs();
    Color initialButtonBackground;
    Color initialButtonTextColor;
    //BL_General Common.BlGeneral = new BL_General();

    public FoodToHitTargetCarbsPage()
    {
        InitializeComponent();

        initialButtonBackground = TxtChoLeftToTake.BackgroundColor;
        initialButtonTextColor = TxtChoLeftToTake.TextColor;

        // read data from other pages
        blFoodToEat.TargetCho.Double = Safe.Double(Common.BlGeneral.RestoreParameter("Bolus_ChoToEat"));
        blFoodToEat.NameOfFood = Safe.String(Common.BlGeneral.RestoreParameter("FoodInMeal_Name"));
        blFoodToEat.RestoreParameters();
        FromClassToUi();
    }
    private void FromClassToUi()
    {
        TxtChoAlreadyTaken.Text = blFoodToEat.ChoAlreadyTaken.Text;
        TxtChoOfFood.Text = blFoodToEat.ChoOfFood.Text;
        TxtTargetCho.Text = blFoodToEat.TargetCho.Text;
        TxtChoLeftToTake.Text = blFoodToEat.ChoLeftToTake.Text;
        TxtFoodToHitTarget.Text = blFoodToEat.FoodToHitTarget.Text;
        if (TxtChoLeftToTake.Text.Contains("-"))
        {
            TxtChoLeftToTake.Background = Colors.Red;
            TxtFoodToHitTarget.Background = Colors.Red;
            TxtChoLeftToTake.TextColor = Colors.White;
            TxtFoodToHitTarget.TextColor = Colors.White;
        }
        else
        {
            TxtChoLeftToTake.Background = initialButtonBackground;
            TxtFoodToHitTarget.Background = initialButtonBackground;
            TxtChoLeftToTake.TextColor = initialButtonTextColor;
            TxtFoodToHitTarget.TextColor = initialButtonTextColor;
        }
        TxtFoodName.Text = blFoodToEat.NameOfFood;
    }
    internal void FromUiToClass()
    {
        blFoodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
        blFoodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
        blFoodToEat.TargetCho.Text = TxtTargetCho.Text;
        blFoodToEat.ChoLeftToTake.Text = TxtChoLeftToTake.Text;
        blFoodToEat.FoodToHitTarget.Text = TxtFoodToHitTarget.Text;
        blFoodToEat.NameOfFood = TxtFoodName.Text;
    }
    private void TxtChoAlreadyTaken_Leave(object sender, EventArgs e)
    {
        blFoodToEat.ChoAlreadyTaken.Text = TxtChoAlreadyTaken.Text;
        blFoodToEat.Calculations();
        FromClassToUi();
    }
    private void TxtChoOfFood_Leave(object sender, EventArgs e)
    {
        blFoodToEat.ChoOfFood.Text = TxtChoOfFood.Text;
        blFoodToEat.Calculations();
        FromClassToUi();
    }
    private void TxtTargetCho_Leave(object sender, EventArgs e)
    {
        blFoodToEat.TargetCho.Text = TxtTargetCho.Text;
        blFoodToEat.Calculations();
        FromClassToUi();
    }
    private void btnCalculateGrams_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        blFoodToEat.Calculations();
        FromClassToUi();
    }
    private void btnReadAll_Click(object sender, EventArgs e)
    {
        blFoodToEat.TargetCho.Double = Safe.Double(Common.BlGeneral.RestoreParameter("Bolus_ChoToEat"));
        blFoodToEat.ChoAlreadyTaken.Double = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_ChoGrams"));
        blFoodToEat.ChoOfFood.Double = Safe.Double(Common.BlGeneral.RestoreParameter("FoodInMeal_CarbohydratesPercent"));
        blFoodToEat.NameOfFood = Safe.String(Common.BlGeneral.RestoreParameter("FoodInMeal_Name"));
        FromClassToUi();
        TxtChoLeftToTake.Text = "";
        TxtFoodToHitTarget.Text = "";
    }
    private void btnReadTargetCho_Click(object sender, EventArgs e)
    {
        blFoodToEat.TargetCho.Double = Safe.Double(Common.BlGeneral.RestoreParameter("Bolus_ChoToEat"));
        FromClassToUi();
    }
    private void btnReadChoTaken_Click(object sender, EventArgs e)
    {
        blFoodToEat.ChoAlreadyTaken.Double = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_ChoGrams"));
        FromClassToUi();
    }
    private void btnReadFood_Click(object sender, EventArgs e)
    {
        blFoodToEat.ChoOfFood.Double = Safe.Double(Common.BlGeneral.RestoreParameter("FoodInMeal_CarbohydratesPercent"));
        blFoodToEat.NameOfFood = Safe.String(Common.BlGeneral.RestoreParameter("FoodInMeal_Name"));
        FromClassToUi();
    }
    private void btnInjection_Click(object sender, EventArgs e)
    {
        Navigation.PushAsync(new InjectionsPage(null));
    }
}
