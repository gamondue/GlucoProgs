using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using SharedFunctions;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsulinCalc_Page : ContentPage
    {
        BL_BolusCalculation bolusCalculation;
        BL_GlucoseMeasurements glucoseMeasuremet;
        public InsulinCalc_Page()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation();
            glucoseMeasuremet = new BL_GlucoseMeasurements();

            bolusCalculation.RestoreData();
            bolusCalculation.MealOfBolus.SelectMealBasedOnTimeNow();

            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();

            //tempText.Text = CommonData.PathUsersDownload; 
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.CalculateBolus();
            bolusCalculation.SaveData();
            bolusCalculation.SaveLog();
            FromClassToUi();
        }
        private void FromClassToUi()
        {
            txtChoToEat.Text = bolusCalculation.ChoToEat.Text;
            txtInsulinSensitivity.Text = bolusCalculation.InsulinCorrectionSensitivity.Text;
            ////////cmbSensitivityFactor.SetValue (bolusCalculation.FactorOfInsulinCorrectionSensitivity.Double);
            txtTdd.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = bolusCalculation.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolusCalculation.GlucoseToBeCorrected.Text;
            txtCorrectionInsulin.Text = bolusCalculation.BolusInsulinDueToCorrectionOfGlucose.Text;
            txtChoInsulinMeal.Text = bolusCalculation.BolusInsulinDueToChoOfMeal.Text;
            txtTotalInsulin.Text = bolusCalculation.TotalInsulinForMeal.Text;
            txtTypicalBolusMidday.Text = bolusCalculation.TypicalBolusMidday.Text;
            txtTypicalBolusMorning.Text = bolusCalculation.TypicalBolusMorning.Text;
            txtTypicalBolusEvening.Text = bolusCalculation.TypicalBolusEvening.Text;
            txtTypicalBolusNight.Text = bolusCalculation.TypicalBolusNight.Text;
            txtTargetGlucose.Text = bolusCalculation.TargetGlucose.Text;

            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;
            txtStatusBar.Text = bolusCalculation.StatusMessage;
            switch (bolusCalculation.MealOfBolus.Type)
            {
                case (Meal.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.IsChecked = true;
                    break;
                case (Meal.TypeOfMeal.Dinner):
                    rdbIsDinner.IsChecked = true;
                    break;
                case (Meal.TypeOfMeal.Lunch):
                    rdbIsLunch.IsChecked = true;
                    break;
                case (Meal.TypeOfMeal.Snack):
                    rdbIsSnack.IsChecked = true;
                    break;
            }
            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;
        }
        private void FromUiToClass()
        {
            // since it is easy to mistakenly insert blanks during editing, we tear blanks off 
            // from all the Entry controls that input a number 
            bolusCalculation.ChoToEat.Text = txtChoToEat.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text.Replace(" ", "");

            bolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text.Replace(" ", "");
            if (cmbSensitivityFactor.SelectedItem != null)
                bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text = cmbSensitivityFactor.SelectedItem.ToString();

            bolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text.Replace(" ", "");
            bolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text.Replace(" ", "");
            bolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text.Replace(" ", "");
            bolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text.Replace(" ", "");
            bolusCalculation.TargetGlucose.Text = txtTargetGlucose.Text.Replace(" ", "");

            if (rdbIsBreakfast.IsChecked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Breakfast;
            if (rdbIsLunch.IsChecked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Lunch;
            if (rdbIsDinner.IsChecked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Dinner;
            if (rdbIsSnack.IsChecked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Snack;
        }
        private void btnRoundInsulin_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.RoundInsulinToZeroDecimal();
            bolusCalculation.SaveData();
            FromClassToUi();
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            //////////////List<GlucoseRecord> list = glucoseMeasuremet.GetLastTwoGlucoseMeasurements();
            //////////////txtGlucoseBeforeMeal.Text = list[0].GlucoseValue.ToString();
        }
        private void btnSaveBolus_Click(object sender, EventArgs e)
        {
            bolusCalculation.SaveData(); 
        }
        private void btnInsulinSensitivityCalculation_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.CalculateInsulinCorrectionSensitivity();
            // we don't save here! 
            FromClassToUi();
        }
    }
}