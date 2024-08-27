using gamon;
using GlucoMan.BusinessLayer;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class MealsPage : ContentPage
{
    // since it is accessed by several pages, to avoid "concurrent" problems 
    // we use a common business layer object, between different pages 
    private BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    Accuracy accuracyClass;

    private List<Meal> allTheMeals;
    private MealPage mealPage;

    public MealsPage()
    {
        InitializeComponent();

        cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        ////cmbTypeOfMeal.ItemsSource = Enum.GetValues(typeof(TypeOfMeal));
        bl.Meal = new Meal();
        bl.FoodInMeal = new FoodInMeal();

        accuracyClass = new Accuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);

        bl.SetTypeOfMealBasedOnTimeNow();
        RefreshGrid();
    }
    private void SetCorrectTypeOfMealRadioButton()
    {
        // un-check all
        rdbIsBreakfast.IsChecked = false;
        rdbIsSnack.IsChecked = false;
        rdbIsLunch.IsChecked = false;
        rdbIsDinner.IsChecked = false;

        // check the one that is enabled now (if any!) 
        if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Breakfast)
            rdbIsBreakfast.IsChecked = true;
        else if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Snack)
            rdbIsSnack.IsChecked = true;
        else if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Lunch)
            rdbIsLunch.IsChecked = true;
        else if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Dinner)
            rdbIsDinner.IsChecked = true;
    }
    private void FromClassToUi()
    {
        txtIdMeal.Text = bl.Meal.IdMeal.ToString();
        txtChoOfMeal.Text = SqlSafe.String(bl.Meal.Carbohydrates.Text);
        ////cmbTypeOfMeal.SelectedItem = bl.Meal.IdTypeOfMeal;
        if (bl.Meal.TimeBegin.DateTime != General.DateNull)
        {
            dtpMealDateBegin.Date = (DateTime)SqlSafe.DateTime(bl.Meal.TimeBegin.DateTime);
            dtpMealTimeBegin.Time = ((DateTime)bl.Meal.TimeBegin.DateTime).TimeOfDay;  // - dtpMealDateBegin.Date;
        }
        if (bl.Meal.AccuracyOfChoEstimate.Double != null)
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
        txtNotes.Text = bl.Meal.Notes;
        SetCorrectTypeOfMealRadioButton();
    }
    private void FromUiToClass()
    {
        bl.Meal.IdMeal = SqlSafe.Int(txtIdMeal.Text);
        bl.Meal.Carbohydrates.Text = SqlSafe.Double(txtChoOfMeal.Text).ToString();

        bl.Meal.AccuracyOfChoEstimate.Double = SqlSafe.Double(txtAccuracyOfChoMeal.Text);

        DateTime instant = new DateTime(dtpMealDateBegin.Date.Year, dtpMealDateBegin.Date.Month, dtpMealDateBegin.Date.Day,
            dtpMealTimeBegin.Time.Hours, dtpMealTimeBegin.Time.Minutes, dtpMealTimeBegin.Time.Seconds);
        bl.Meal.TimeBegin.DateTime = instant;
        bl.Meal.Notes = txtNotes.Text;
        // TypeOfMeal treated by controls' events
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private void RefreshGrid()
    {
        DateTime now = DateTime.Now;
        allTheMeals = bl.GetMeals(
            now.Subtract(new TimeSpan(120, 00, 0, 0)),
            now.AddDays(1));
        gridMeals.BindingContext = allTheMeals;
    }
    protected override async void OnAppearing()
    {
        RefreshUi();
        base.OnAppearing();
    }
    private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            // await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        // make the tapped row the current meal 
        bl.Meal = (Meal)e.SelectedItem;
        FromClassToUi();
    }
    private async void btnAddMeal_ClickAsync(object sender, EventArgs e)
    {
        FromUiToClass();
        // reset the meal because we want a new one
        bl.Meal = new Meal();
        // add to the new meal the data cominng from this page
        if (chkNowInAdd.IsChecked)
        {
            DateTime now = DateTime.Now;
            bl.Meal.TimeBegin.DateTime = now;
            bl.Meal.TimeEnd.DateTime = now;
        }
        else
        {
            DateTime instant = new DateTime(dtpMealDateBegin.Date.Year, dtpMealDateBegin.Date.Month, dtpMealDateBegin.Date.Day,
                dtpMealTimeBegin.Time.Hours, dtpMealTimeBegin.Time.Minutes, dtpMealTimeBegin.Time.Seconds);
            bl.Meal.TimeBegin.DateTime = instant;
            bl.Meal.TimeEnd.DateTime = bl.Meal.TimeBegin.DateTime;
        }
        bl.Meal.Carbohydrates.Text = txtChoOfMeal.Text;
        bl.Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoMeal.Text;
        bl.Meal.IdTypeOfMeal = SetTypeOfMealBasedOnRadioButtons();
        mealPage = new MealPage(bl.Meal);
        await Navigation.PushAsync(mealPage);
    }
    private TypeOfMeal SetTypeOfMealBasedOnRadioButtons()
    {
        TypeOfMeal type;
        if (rdbIsBreakfast.IsChecked)
            type = TypeOfMeal.Breakfast;
        else if (rdbIsDinner.IsChecked)
            type = TypeOfMeal.Dinner;
        else if (rdbIsLunch.IsChecked)
            type = TypeOfMeal.Lunch;
        else if (rdbIsSnack.IsChecked)
            type = TypeOfMeal.Snack;
        else
            type = TypeOfMeal.Other;
        return type;
    }
    private async void btnRemoveMeal_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdMeal.Text == "")
        {
            await DisplayAlert("Deletion not possible", "Choose the meal to delete", "Ok");
            return;
        }
        bool remove = await DisplayAlert(String.Format("Should we delete the meal begun at {0}, Id {1}?",
            bl.Meal.TimeBegin.Text,
            bl.Meal.IdMeal),
            "", "Yes", "No");
        if (remove)
        {
            bl.DeleteOneMeal(bl.Meal);
            RefreshUi();
        }
    }
    private async void btnSaveMeal_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdMeal.Text == "")
        {
            await DisplayAlert("Select one meal from the list", "Choose a meal to save", "Ok");
            return;
        }
        FromUiToClass();
        bl.SaveOneMeal(bl.Meal, false);
        RefreshUi();
    }
    private async void btnShowThisMeal_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdMeal.Text == "")
        {
            await DisplayAlert("", "Choose a meal in the grid", "Ok");
            return;
        }
        FromUiToClass();
        bl.FoodInMeal = new FoodInMeal();
        await Navigation.PushAsync(new MealPage(bl.Meal));
        RefreshUi();
    }
    private void btnNowBegin_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        dtpMealDateBegin.Date = now;
        dtpMealTimeBegin.Time = now.TimeOfDay;
    }
    private void btnDefault_Click(object sender, EventArgs e)
    {
        bl.NewDefaults();
        FromClassToUi();
    }
    private void txtChoOfMeal_TextChanged(object sender, EventArgs e)
    {
        bl.SaveMealParameters();
    }
    private void rdb_CheckedChanged(object sender, EventArgs e)
    {
        if (rdbIsBreakfast.IsChecked)
            bl.Meal.IdTypeOfMeal = TypeOfMeal.Breakfast;
        else if (rdbIsSnack.IsChecked)
            bl.Meal.IdTypeOfMeal = TypeOfMeal.Snack;
        else if (rdbIsLunch.IsChecked)
            bl.Meal.IdTypeOfMeal = TypeOfMeal.Lunch;
        else if (rdbIsDinner.IsChecked)
            bl.Meal.IdTypeOfMeal = TypeOfMeal.Dinner;
    }
    private void cmbTypeOfMeal_SelectedIndexChanged(object sender, EventArgs e)
    {
        ////bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;
    }
}