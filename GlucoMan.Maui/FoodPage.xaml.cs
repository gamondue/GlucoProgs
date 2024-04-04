using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;

public partial class FoodPage : ContentPage
{
    BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    private Food currentFood;

    public FoodPage(Food currentFood)
    {
        InitializeComponent();
        this.BindingContext = currentFood;
        //gridMeasurements.ItemsSource = glucoseReadings;
        FoodIsChosen = false;
    }

    public bool FoodIsChosen { get; internal set; }
    public Food CurrentFood { get; private set; }

    private void btnOk_Click(object sender, EventArgs e)
    {
        FoodIsChosen = true;
        bl.SaveOneFood((Food)this.BindingContext);
        this.Navigation.PopAsync();
    }
    private void btnAbort_Click(object sender, EventArgs e)
    {
        FoodIsChosen = false;
        this.Navigation.PopAsync();
    }
}