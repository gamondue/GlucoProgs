using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MealPage : ContentPage
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
        public MealPage()
        {
            InitializeComponent();

            bl.Meal = new Meal();

            RefreshGrid();
        }
        private void FromUiToClass()
        {
            bl.Meal.IdMeal = Safe.Int(txtIdMeal.Text);
            bl.Meal.Carbohydrates.Text = txtChoOfMeal.Text;
            DateTime instant = new DateTime(dtpMealDateStart.Date.Year, dtpMealDateStart.Date.Month, dtpMealDateStart.Date.Day,
                dtpMealTimeStart.Time.Hours, dtpMealTimeStart.Time.Minutes, dtpMealTimeStart.Time.Seconds);
            bl.Meal.TimeStart.DateTime = instant;
            bl.Meal.AccuracyOfChoEstimate.Double = Safe.Double(txtAccuracyOfChoMeal.Text);
        }
        private void FromClassToUi()
        {
            txtIdMeal.Text = bl.Meal.IdMeal.ToString();
            txtChoOfMeal.Text = Safe.String(bl.Meal.Carbohydrates.Text);
            if (bl.Meal.TimeStart.DateTime != Common.DateNull)
            {
                dtpMealDateStart.Date = (DateTime)Safe.DateTime(bl.Meal.TimeStart.DateTime);
                dtpMealTimeStart.Time = (DateTime)bl.Meal.TimeStart.DateTime - dtpMealDateStart.Date;
            }
            txtAccuracyOfChoMeal.Text = bl.Meal.AccuracyOfChoEstimate.ToString();
            ////////cmbTypeOfMeal.Text = bl.Meal.TypeOfMeal.ToString();
        }
        private void RefreshGrid()
        {
            bl.ReadMeals(null, null);
            gridMeals.BindingContext = bl.Meals;
        }
        private async void btnAddMeal_Click(object sender, EventArgs e)
        {
            // !!!! make modal !!!!
            //////await Navigation.PushAsync(new MealPage());

            FromUiToClass();
            // force creation of a new record 
            bl.Meal.IdMeal = null;
            bl.Meal.TimeStart.DateTime = DateTime.Now;
            bl.SaveOneMeal();
            RefreshGrid();
            FromClassToUi();
        }
        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                //MessageBox.Show("Choose a meal in the grid");
                return;
            }
            // make the tapped row current
            bl.Meal = (Meal)e.SelectedItem;
            FromClassToUi();
        }
        private void btnRemoveMeal_Click(object sender, EventArgs e)
        {
            btnRemoveMeal_ClickAsync(sender, e);
        }
        private async Task btnRemoveMeal_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                await DisplayAlert("Deletion not possible", "Choose the meal to delete", "Ok");
                return;
            }
            FromUiToClass();
            bl.DeleteOneMeal(bl.Meal);
            RefreshGrid();
        }
        private async Task btnShowThisMeal_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                //////MessageBox.Show("Choose a meal in the grid");
                return;
            }
            await Navigation.PushAsync(new MealPage());
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave_ClickAsync(sender, e);
        }
        private async Task btnSave_ClickAsync(object sender, EventArgs e)
        {
            if (txtIdMeal.Text == "")
            {
                await DisplayAlert("Saving not possible", "Choose the meal to modify", "Ok");
                return;
            }
            FromUiToClass();
            bl.SaveOneMeal();
            RefreshGrid();
        }
        //private void txtAccuracyOfChoMeal_TextChanged(object sender, EventArgs e)
        //{
        //    bl.Meal.AccuracyOfChoEstimate = Safe.Double(txtAccuracyOfChoMeal.Text);
        //    bl.NumericalAccuracyChanged(bl.Meal.AccuracyOfChoEstimate.Value);
        //}
        //private void cmbAccuracyMeal_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    bl.Meal.AccuracyOfChoEstimate =
        //        bl.QualitativeAccuracyChanged(((QualitativeAccuracy)cmbAccuracyMeal.SelectedItem));
        //}
    }
}