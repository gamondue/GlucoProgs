using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;
using static GlucoMan.Common;

namespace GlucoMan.Maui;

public partial class MealsPage : ContentPage
{
    // since it is accessed by several pages, to avoid "concurrent" problems 
    // we use a common business layer object, between different pages 
    private BL_MealAndFood bl = Common.MealAndFood_CommonBL;
    UiAccuracy accuracyClass;

    private bool loadingUi = true;
    private List<Meal> allTheMeals;
    private MealPage mealPage;

    double? MonthsOfDataShownInTheGrids = 3;

    public MealsPage()
    {
        InitializeComponent();

        loadingUi = true;

        Parameters parameters = Common.Database.GetParameters();
        if (parameters != null && parameters.MonthsOfDataShownInTheGrids > 0)
            MonthsOfDataShownInTheGrids = parameters.MonthsOfDataShownInTheGrids;

        cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        ////cmbTypeOfMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
        bl.Meal = new Meal();
        bl.FoodInMeal = new FoodInMeal();
        
        // Create UiAccuracy which will handle UI synchronization automatically
        accuracyClass = new UiAccuracy(txtAccuracyOfChoMeal, cmbAccuracyMeal);
        
        bl.SetTypeOfMealBasedOnTimeNow();

        // Initialize accuracy controls to sync combo boxes with current text values
        InitializeAccuracyControls();

        RefreshGrid();
        loadingUi = false;
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
    private async Task FromClassToUi()
    {
        loadingUi = true;

        txtIdMeal.Text = bl.Meal.IdMeal.ToString();
        txtChoOfMeal.Text = Safe.String(bl.Meal.CarbohydratesGrams.Text);
        ////cmbTypeOfMeal.SelectedItem = bl.Meal.IdTypeOfMeal;
        if (bl.Meal.TimeBegin.DateTime != General.DateNull)
        {
            dtpMealDateBegin.Date = (DateTime)Safe.DateTime(bl.Meal.TimeBegin.DateTime);
            dtpMealTimeBegin.Time = ((DateTime)bl.Meal.TimeBegin.DateTime).TimeOfDay;  // - dtpMealDateBegin.Date;
        }
        if (bl.Meal.AccuracyOfChoEstimate.Double != null)
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.Text;
        txtNotes.Text = bl.Meal.Notes;
        SetCorrectTypeOfMealRadioButton();

        loadingUi = false;
        
        // Update accuracy controls after data is loaded and loadingUi is false
        // This ensures UiAccuracy can work properly and combo box gets synchronized
        await RefreshAccuracyControls();
    }
    private void FromUiToClass()
    {
        bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
        bl.Meal.CarbohydratesGrams.Text = Safe.Double(txtChoOfMeal.Text).ToString();

        bl.Meal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text);

        DateTime instant = new DateTime(dtpMealDateBegin.Date.Year, dtpMealDateBegin.Date.Month, dtpMealDateBegin.Date.Day,
            dtpMealTimeBegin.Time.Hours, dtpMealTimeBegin.Time.Minutes, dtpMealTimeBegin.Time.Seconds);
        bl.Meal.TimeBegin.DateTime = instant;
        bl.Meal.Notes = txtNotes.Text;
        // TypeOfMeal treated by controls' events
    }
    private async Task RefreshUi()
    {
        await FromClassToUi();
        RefreshGrid();
    }
    private void RefreshGrid()
    {
        DateTime now = DateTime.Now;
        allTheMeals = bl.GetMeals(
            now.AddMonths(-(int)(MonthsOfDataShownInTheGrids)),
            now.AddDays(1));
        gridMeals.BindingContext = allTheMeals;
    }
    protected override async void OnAppearing()
    {
        await RefreshUi();
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
        await FromClassToUi();
    }
    private async void btnAddMeal_ClickAsync(object sender, EventArgs e)
    {
        // reset the meal because we want a new one
        bl.Meal = new Meal();
        // add to the new meal the data coming from this page
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
            bl.Meal.TimeEnd.DateTime = instant;
        }
        bl.Meal.CarbohydratesGrams.Text = txtChoOfMeal.Text;
        bl.Meal.AccuracyOfChoEstimate.Text = txtAccuracyOfChoMeal.Text;
        bl.Meal.IdTypeOfMeal = SetTypeOfMealBasedOnRadioButtons();
        bl.Meal.Notes = txtNotes.Text;
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
            await RefreshUi();
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
        await RefreshUi();
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
        await RefreshUi();
    }
    private void btnNowBegin_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        dtpMealDateBegin.Date = now;
        dtpMealTimeBegin.Time = now.TimeOfDay;
    }
    private async void btnDefault_Click(object sender, EventArgs e)
    {
        bl.NewDefaults();
        await FromClassToUi();
    }

    // ACCURACY MANAGEMENT METHODS (following MealPage pattern exactly)

    private void InitializeAccuracyControls()
    {
        try
        {
            // Let UiAccuracy handle the initialization
            // We just ensure the data is properly formatted
            if (!string.IsNullOrEmpty(txtAccuracyOfChoMeal.Text))
            {
                if (!double.TryParse(txtAccuracyOfChoMeal.Text, out _))
                {
                    txtAccuracyOfChoMeal.Text = "100"; // Default value
                }
            }
            else
            {
                txtAccuracyOfChoMeal.Text = "100"; // Default value
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealsPage - InitializeAccuracyControls", ex);
        }
    }

    private async Task RefreshAccuracyControls()
    {
        try
        {
            // Small delay to ensure data binding has completed (like in MealPage)
            await Task.Delay(50);
            
            // Update accuracy controls after data binding changes
            if (accuracyClass != null && !string.IsNullOrEmpty(txtAccuracyOfChoMeal.Text))
            {
                if (double.TryParse(txtAccuracyOfChoMeal.Text, out double accuracy))
                {
                    // Update combo box selection
                    var qualitativeAccuracy = accuracyClass.GetQualitativeAccuracyGivenQuantitavive(accuracy);
                    cmbAccuracyMeal.SelectedItem = qualitativeAccuracy;
                    
                    // Update text box colors using UiAccuracy logic
                    txtAccuracyOfChoMeal.BackgroundColor = accuracyClass.AccuracyBackColor(accuracy);
                    txtAccuracyOfChoMeal.TextColor = accuracyClass.AccuracyForeColor(accuracy);
                }
            }
            else
            {
                // Reset to default if no valid accuracy
                cmbAccuracyMeal.SelectedItem = null;
                txtAccuracyOfChoMeal.BackgroundColor = Colors.LightGreen; // Default from XAML
                txtAccuracyOfChoMeal.TextColor = Colors.Black; // Default from XAML
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealsPage - RefreshAccuracyControls", ex);
        }
    }

    // EVENT HANDLERS - These should be minimal and only update the data model
    // Let UiAccuracy handle all UI synchronization automatically

    private void txtAccuracyOfChoMeal_TextChanged(object sender, TextChangedEventArgs e)
    {
        // The UiAccuracy class handles all synchronization internally
        // Do not interfere with its operation
    }

    private void txtAccuracyOfChoMeal_Unfocused(object sender, FocusEventArgs e)
    {
        // Let UiAccuracy handle the combo box update, we only update the data model
        try
        {
            if (!loadingUi && bl?.Meal != null && !string.IsNullOrEmpty(txtAccuracyOfChoMeal.Text))
            {
                if (double.TryParse(txtAccuracyOfChoMeal.Text, out double accuracy))
                {
                    // Update the meal's accuracy in the data model
                    bl.Meal.AccuracyOfChoEstimate.Double = accuracy;
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealsPage - txtAccuracyOfChoMeal_Unfocused", ex);
        }
    }

    private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Let UiAccuracy handle the text box update, we only update the data model
        try
        {
            if (!loadingUi && bl?.Meal != null && cmbAccuracyMeal.SelectedItem != null)
            {
                var selectedAccuracy = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
                double numericValue = (double)selectedAccuracy;

                // Update the meal's accuracy in the data model
                bl.Meal.AccuracyOfChoEstimate.Double = numericValue;
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("MealsPage - cmbAccuracyMeal_SelectedIndexChanged", ex);
        }
    }

    private void rdb_CheckedChanged(object sender, CheckedChangedEventArgs e)
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

    private void txtChoOfMeal_TextChanged(object sender, TextChangedEventArgs e)
    {
        bl.SaveMealParameters();
    }
}