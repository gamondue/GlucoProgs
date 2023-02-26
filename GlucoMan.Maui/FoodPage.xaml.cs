using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    public FoodPage(Food currentFood)
    {
        InitializeComponent();
        FoodIsChosen = false;
    }

    public bool FoodIsChosen { get; internal set; }
    public Food CurrentFood { get; private set; }

    private void btnOk_Click(object sender, EventArgs e)
    {
        FoodIsChosen = true;
        //FromUiToClass();
        bl.SaveOneFood(CurrentFood);
        this.Navigation.PopAsync();
    }
}