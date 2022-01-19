using GlucoMan.BusinessLayer;
using static GlucoMan.Common;
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
    public partial class MealsPage : ContentPage
    {
        private BL_MealAndFood bl = new BL_MealAndFood();
        private List<Meal> meals;
        private Meal currentMeal;
        public MealsPage()
        {
            InitializeComponent();
            
            currentMeal = new Meal();

            RefreshGrid();
        }
        private void FromUiToClass()
        {
            //double? glucose = Safe.Double(txtGlucose.Text);
            //if (glucose == null)
            //{
            //    txtGlucose.Text = "";
            //    //Console.Beep();
            //    return;
            //}
            //currentGlucose = new GlucoseRecord();
            //currentGlucose.IdGlucoseRecord = Safe.Int(txtIdGlucoseRecord.Text);
            //currentGlucose.GlucoseValue = glucose;
            //DateTime instant = new DateTime(dtpEventDate.Date.Year, dtpEventDate.Date.Month, dtpEventDate.Date.Day,
            //    dtpEventTime.Time.Hours, dtpEventTime.Time.Minutes, dtpEventTime.Time.Seconds);
            //currentGlucose.Timestamp = instant;
        }
        private void FromClassToUi()
        {
            //txtGlucose.Text = currentGlucose.GlucoseValue.ToString();
            //dtpEventDate.Date = (DateTime)Safe.DateTime(currentGlucose.Timestamp);
            //dtpEventTime.Time = (DateTime)currentGlucose.Timestamp - dtpEventDate.Date;
            //txtIdGlucoseRecord.Text = currentGlucose.IdGlucoseRecord.ToString();
        }
        private void RefreshGrid()
        {
            meals = bl.ReadMeals(null, null);
            gridMeals.BindingContext = meals;
        }
        private void btnAddFood_Click(object sender, EventArgs e)
        {
        }
        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
        }
        void OnSelection(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
            {
                return;
            }
            // make the tapped row current
            currentMeal = (Meal)e.SelectedItem;
            FromClassToUi();
        }
    }
}