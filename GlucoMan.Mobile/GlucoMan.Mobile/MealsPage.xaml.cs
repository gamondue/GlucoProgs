using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static GlucoMan.Common;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealsPage : ContentPage
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
        //Accuracy accuracyClass;

        private List<Meal> allTheMeals;

        public MealsPage()
        {
            InitializeComponent();

            cmbAccuracyMeal.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));
            cmbTypeOfMeal.ItemsSource = Enum.GetValues(typeof(TypeOfMeal));

            //accuracyClass = new Accuracy(this, txtAccuracyOfChoMeal, cmbAccuracyMeal, bl);

            RefreshUi();
        }
        private void FromClassToUi()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(bl.Meal.CarbohydratesGrams.Text);
            if (bl.Meal.TimeBegin.DateTime != Common.DateNull)
            {
                dtpMealDateBegin.Date = (DateTime)Safe.DateTime(bl.Meal.TimeBegin.DateTime);
                dtpMealTimeBegin.Time = ((DateTime)bl.Meal.TimeBegin.DateTime).TimeOfDay;  // - dtpMealDateBegin.Date;
            }
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.ToString();
            cmbAccuracyMeal.SelectedItem = bl.Meal.QualitativeAccuracyOfChoEstimate;

            cmbTypeOfMeal.SelectedItem = bl.Meal.IdTypeOfMeal;
            cmbAccuracyMeal.SelectedItem = bl.Meal.QualitativeAccuracyOfChoEstimate;
            SetCorrectRadioButtons(); 
        }

        private void SetCorrectRadioButtons()
        {
            rdbIsBreakfast.IsChecked = false;
            rdbIsSnack.IsChecked = false;
            rdbIsLunch.IsChecked = false;
            rdbIsDinner.IsChecked = false;

            if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Breakfast)
                rdbIsBreakfast.IsChecked = true;
            else if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Snack)
                rdbIsSnack.IsChecked = true;
            else if (bl.Meal.IdTypeOfMeal != TypeOfMeal.Lunch)
                rdbIsLunch.IsChecked = true;
            else if (bl.Meal.IdTypeOfMeal == TypeOfMeal.Dinner)
                rdbIsDinner.IsChecked = true;
        }

        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.CarbohydratesGrams.Text = Safe.Double(txtChoOfMeal.Text).ToString();
            DateTime instant = new DateTime(dtpMealDateBegin.Date.Year, dtpMealDateBegin.Date.Month, dtpMealDateBegin.Date.Day,
                dtpMealTimeBegin.Time.Hours, dtpMealTimeBegin.Time.Minutes, dtpMealTimeBegin.Time.Seconds);
            bl.Meal.TimeBegin.DateTime = instant;
            bl.Meal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text);

            bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;
            bl.Meal.QualitativeAccuracyOfChoEstimate = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
            if (cmbTypeOfMeal.SelectedItem != null)
                bl.Meal.IdTypeOfMeal = (TypeOfMeal)cmbTypeOfMeal.SelectedItem;
            else
                bl.Meal.IdTypeOfMeal = TypeOfMeal.NotSet;
            
            if (cmbAccuracyMeal.SelectedItem != null)
                bl.Meal.QualitativeAccuracyOfChoEstimate = (QualitativeAccuracy)cmbAccuracyMeal.SelectedItem;
            else
                bl.Meal.QualitativeAccuracyOfChoEstimate = QualitativeAccuracy.NotSet;
        }
        private void RefreshUi()
        {
            FromClassToUi();
            allTheMeals = bl.GetMeals(dtpMealDateBegin.Date.Subtract(new TimeSpan(60, 00, 0, 0)),
                dtpMealDateBegin.Date.AddDays(1));
            gridMeals.BindingContext = allTheMeals;
        }
        private async void btnAddMeal_ClickAsync(object sender, EventArgs e)
        {
            FromUiToClass();
            // erase Id to create a new meal
            bl.Meal.IdMeal = null;

            if (chkNowInAdd.IsChecked)
            {
                DateTime now = DateTime.Now;
                bl.Meal.TimeBegin.DateTime = now;
                bl.Meal.TimeEnd.DateTime = now;
            }
            // ???? make modal ????
            txtIdMeal.Text = bl.SaveOneMeal(bl.Meal).ToString(); 
            await Navigation.PushAsync(new MealPage(bl.Meal)); 
            RefreshUi();
        }
        private async void btnRemoveMeal_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                await DisplayAlert("Deletion not possible", "Choose the meal to delete", "Ok");
                return;
            }
            FromUiToClass();
            bl.DeleteOneMeal(bl.Meal);
            RefreshUi();
        }
        private async Task btnSaveMeal_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                await DisplayAlert("Select one meal from the list", "Choose a meal to save", "Ok");

                return;
            }
            FromUiToClass();
            bl.SaveOneMeal(bl.Meal);
            RefreshUi();
        }
        private async void btnShowThisMeal_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                await DisplayAlert("", "Choose a meal in the grid", "Ok");
                return;
            }
            await Navigation.PushAsync(new MealPage(bl.Meal));
        }
        private void btnNowBegin_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            dtpMealDateBegin.Date = now;
            dtpMealTimeBegin.Time = now.TimeOfDay;
        }
        private void btnSaveMeal_Click(object sender, EventArgs e)
        {
            btnSaveMeal_ClickAsync(sender, e); 
        }
        private void btnDefault_Click(object sender, EventArgs e)
        {
            bl.NewDefaults(); 
            RefreshUi(); 
        }
        private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                //await DisplayAlert("XXXX", "YYYY", "Ok");
                return;
            }
            //make the tapped row the current meal 
            bl.Meal = (Meal)e.SelectedItem;
            FromClassToUi();
        }

    }
}