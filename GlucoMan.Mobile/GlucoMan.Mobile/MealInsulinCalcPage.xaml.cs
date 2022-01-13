using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsulinCalcPage : ContentPage
    {
        BL_BolusCalculation currentBolusCalculation;
        BL_GlucoseMeasurements currentGlucoseMeasuremet;
        public InsulinCalcPage()
        {
            InitializeComponent();

            currentBolusCalculation = new BL_BolusCalculation();
            currentGlucoseMeasuremet = new BL_GlucoseMeasurements();

            currentBolusCalculation.RestoreBolusParameters();
            currentBolusCalculation.MealOfBolus.Type = Common.SelectMealBasedOnTimeNow();

            currentBolusCalculation.TargetGlucose.Format = "0";
            currentBolusCalculation.GlucoseBeforeMeal.Format = "0";

            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();
        }
        private void FromClassToUi()
        {
            txtChoToEat.Text = currentBolusCalculation.ChoToEat.Text;
            txtInsulinCorrectionSensitivity.Text = currentBolusCalculation.InsulinCorrectionSensitivity.Text;
            txtTdd.Text = currentBolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = currentBolusCalculation.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = currentBolusCalculation.GlucoseToBeCorrected.Text;
            txtCorrectionInsulin.Text = currentBolusCalculation.BolusInsulinDueToCorrectionOfGlucose.Text;
            txtChoInsulinMeal.Text = currentBolusCalculation.BolusInsulinDueToChoOfMeal.Text;
            txtTotalInsulin.Text = currentBolusCalculation.TotalInsulinForMeal.Text;
            txtTargetGlucose.Text = currentBolusCalculation.TargetGlucose.Text;

            txtChoInsulinRatioBreakfast.Text = currentBolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = currentBolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = currentBolusCalculation.ChoInsulinRatioDinner.Text;

            txtStatusBar.Text = currentBolusCalculation.StatusMessage;
            switch (currentBolusCalculation.MealOfBolus.Type)
            {
                case (Common.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.IsChecked = true;
                    break;
                case (Common.TypeOfMeal.Dinner):
                    rdbIsDinner.IsChecked = true;
                    break;
                case (Common.TypeOfMeal.Lunch):
                    rdbIsLunch.IsChecked = true;
                    break;
                case (Common.TypeOfMeal.Snack):
                    rdbIsSnack.IsChecked = true;
                    break;
            }
        }
        private void FromUiToClass()
        {
            // since it is easy to mistakenly insert blanks during editing, we tear blanks off 
            // from all the Entry controls that input a number 
            currentBolusCalculation.ChoToEat.Text = NoBlank(txtChoToEat.Text);
            currentBolusCalculation.ChoInsulinRatioDinner.Text = NoBlank(txtChoInsulinRatioDinner.Text);
            currentBolusCalculation.ChoInsulinRatioBreakfast.Text = NoBlank(txtChoInsulinRatioBreakfast.Text);
            currentBolusCalculation.ChoInsulinRatioLunch.Text = NoBlank(txtChoInsulinRatioLunch.Text);
            currentBolusCalculation.GlucoseBeforeMeal.Text = NoBlank(txtGlucoseBeforeMeal.Text);
            currentBolusCalculation.TargetGlucose.Text = NoBlank(txtTargetGlucose.Text);
            currentBolusCalculation.InsulinCorrectionSensitivity.Text = NoBlank(txtInsulinCorrectionSensitivity.Text);

            if (rdbIsBreakfast.IsChecked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Breakfast;
            if (rdbIsLunch.IsChecked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Lunch;
            if (rdbIsDinner.IsChecked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Dinner;
            if (rdbIsSnack.IsChecked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Snack;
        }
        private string NoBlank(string Text)
        {
            if (Text == null)
                return null;
            Text.Replace(" ", "");
            return Text;
        }
        private async void btnSetParameters_Click(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CorrectionParametersPage());
        }
        protected override void OnAppearing()
        {   
            // emulates Windows modal behaviour in Android 
            base.OnAppearing();
            currentBolusCalculation.RestoreBolusParameters();
            FromClassToUi();
        }
        private void btnBolusCalculations_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.CalculateBolus();
            currentBolusCalculation.SaveBolusParameters();
            currentBolusCalculation.SaveBolusLog();
            FromClassToUi();
        }
        private void btnRoundInsulin_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.RoundInsulinToZeroDecimal();
            currentBolusCalculation.SaveBolusParameters();
            FromClassToUi();
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            List<GlucoseRecord> list = currentGlucoseMeasuremet.GetLastTwoGlucoseMeasurements();
            if (list.Count > 0)
                txtGlucoseBeforeMeal.Text = list[0].GlucoseValue.ToString();
        }
        private void btnSaveBolus_Click(object sender, EventArgs e)
        {
            currentBolusCalculation.SaveBolusParameters();
        }
    }
}