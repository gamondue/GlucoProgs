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
        BL_BolusCalculation bolusCalculation;
        BL_GlucoseMeasurements glucoseMeasuremet;
        public InsulinCalcPage()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation();
            glucoseMeasuremet = new BL_GlucoseMeasurements();

            bolusCalculation.RestoreBolusData();
            bolusCalculation.RestoreInsulinParameters();
            bolusCalculation.MealOfBolus.Type = Common.SelectMealBasedOnTimeNow();

            bolusCalculation.TargetGlucose.Format = "0";
            bolusCalculation.GlucoseBeforeMeal.Format = "0";

            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();
        }
        private void FromClassToUi()
        {
            txtChoToEat.Text = bolusCalculation.ChoToEat.Text;
            txtInsulinSensitivity.Text = bolusCalculation.InsulinCorrectionSensitivity.Text;
            txtTdd.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = bolusCalculation.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolusCalculation.GlucoseToBeCorrected.Text;
            txtCorrectionInsulin.Text = bolusCalculation.BolusInsulinDueToCorrectionOfGlucose.Text;
            txtChoInsulinMeal.Text = bolusCalculation.BolusInsulinDueToChoOfMeal.Text;
            txtTotalInsulin.Text = bolusCalculation.TotalInsulinForMeal.Text;
            txtTargetGlucose.Text = bolusCalculation.TargetGlucose.Text;

            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;

            txtStatusBar.Text = bolusCalculation.StatusMessage;
            switch (bolusCalculation.MealOfBolus.Type)
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
            bolusCalculation.ChoToEat.Text = txtChoToEat.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text.Replace(" ", "");
            bolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text.Replace(" ", "");
            bolusCalculation.TargetGlucose.Text = txtTargetGlucose.Text.Replace(" ", "");

            if (rdbIsBreakfast.IsChecked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Breakfast;
            if (rdbIsLunch.IsChecked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Lunch;
            if (rdbIsDinner.IsChecked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Dinner;
            if (rdbIsSnack.IsChecked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Snack;
        }
        private async void btnSetParameters_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            await Navigation.PushModalAsync(new CorrectionParametersPage());
            // !!!! TODO: fix modal functioning of this page. Currently continues without 
            // waitng the completion of the modal page 
            bolusCalculation.RestoreInsulinParameters();
            FromClassToUi();
        }
        private void btnBolusCalculations_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.CalculateBolus();
            bolusCalculation.SaveInsulinParameters();
            bolusCalculation.SaveBolusData();
            bolusCalculation.SaveBolusLog();
            FromClassToUi();
        }
        private void btnRoundInsulin_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.RoundInsulinToZeroDecimal();
            bolusCalculation.SaveBolusData();
            FromClassToUi();
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            List<GlucoseRecord> list = glucoseMeasuremet.GetLastTwoGlucoseMeasurements();
            txtGlucoseBeforeMeal.Text = list[0].GlucoseValue.ToString();
        }
        private void btnSaveBolus_Click(object sender, EventArgs e)
        {
            bolusCalculation.SaveBolusData();
        }
    }
}