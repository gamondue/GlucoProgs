using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsulineCalc_Page : ContentPage
    {
        BL_BolusCalculation bolusCalculation;
        BL_GlucoseMeasurements glucoseMeasuremet;
        public InsulineCalc_Page()
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
            txtInsulineSensitivity.Text = bolusCalculation.InsulineCorrectionSensitivity.Text;
            ////////cmbSensitivityFactor.SetValue (bolusCalculation.FactorOfInsulineCorrectionSensitivity.Double);
            txtTdd.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = bolusCalculation.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolusCalculation.GlucoseToBeCorrected.Text;
            txtCorrectionInsuline.Text = bolusCalculation.BolusInsulineDueToCorrectionOfGlucose.Text;
            txtChoInsulineMeal.Text = bolusCalculation.BolusInsulineDueToChoOfMeal.Text;
            txtTotalInsuline.Text = bolusCalculation.TotalInsulineForMeal.Text;
            txtTypicalBolusMidday.Text = bolusCalculation.TypicalBolusMidday.Text;
            txtTypicalBolusMorning.Text = bolusCalculation.TypicalBolusMorning.Text;
            txtTypicalBolusEvening.Text = bolusCalculation.TypicalBolusEvening.Text;
            txtTypicalBolusNight.Text = bolusCalculation.TypicalBolusNight.Text;
            txtTargetGlucose.Text = bolusCalculation.TargetGlucose.Text;

            txtChoInsulineRatioBreakfast.Text = bolusCalculation.ChoInsulineRatioBreakfast.Text;
            txtChoInsulineRatioLunch.Text = bolusCalculation.ChoInsulineRatioLunch.Text;
            txtChoInsulineRatioDinner.Text = bolusCalculation.ChoInsulineRatioDinner.Text;
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
            txtChoInsulineRatioBreakfast.Text = bolusCalculation.ChoInsulineRatioBreakfast.Text;
            txtChoInsulineRatioLunch.Text = bolusCalculation.ChoInsulineRatioLunch.Text;
            txtChoInsulineRatioDinner.Text = bolusCalculation.ChoInsulineRatioDinner.Text;
        }
        private void FromUiToClass()
        {
            // since it is easy to mistakenly insert blanks during editing, we tear blanks off 
            // from all the Entry controls that input a number 
            bolusCalculation.ChoToEat.Text = txtChoToEat.Text.Replace(" ", "");
            bolusCalculation.ChoInsulineRatioDinner.Text = txtChoInsulineRatioDinner.Text.Replace(" ", "");
            bolusCalculation.ChoInsulineRatioBreakfast.Text = txtChoInsulineRatioBreakfast.Text.Replace(" ", "");
            bolusCalculation.ChoInsulineRatioLunch.Text = txtChoInsulineRatioLunch.Text.Replace(" ", "");

            bolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text.Replace(" ", "");
            bolusCalculation.FactorOfInsulineCorrectionSensitivity.Text = cmbSensitivityFactor.SelectedItem.ToString(); 

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
        private void btnRoundInsuline_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.RoundInsulineToZeroDecimal();
            bolusCalculation.SaveData();
            FromClassToUi();
        }
    }
}