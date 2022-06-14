using GlucoMan.BusinessLayer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CorrectionParametersPage : ContentPage
    {
        BL_BolusCalculation bolusCalculation;
        public CorrectionParametersPage()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation(); 
            bolusCalculation.RestoreInsulinCorrectionParameters();
            bolusCalculation.RestoreRatioCHOInsulinParameters();

            bolusCalculation.FactorOfInsulinCorrectionSensitivity.Format = "0";
            FromClassToUi();
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.CalculateBolus();
            bolusCalculation.SaveInsulinCorrectionParameters();
            bolusCalculation.SaveBolusLog();
            FromClassToUi();
        }
        private void FromClassToUi()
        {
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;
            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;

            cmbSensitivityFactor.SelectedItem = bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text;

            txtTypicalBolusMidday.Text = bolusCalculation.TypicalBolusMidday.Text;
            txtTypicalBolusMorning.Text = bolusCalculation.TypicalBolusMorning.Text;
            txtTypicalBolusEvening.Text = bolusCalculation.TypicalBolusEvening.Text;
            txtTypicalBolusNight.Text = bolusCalculation.TypicalBolusNight.Text;

            txtInsulinCorrectionSensitivity.Text = bolusCalculation.InsulinCorrectionSensitivity.Text;

            // Total Daily Dose Of Insulin is always calculated by bolusCalculation and shown here
            txtTotalDailyDoseOfInsulin.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
        }
        private void FromUiToClass()
        {
            // since it is easy to mistakenly insert blanks during editing, we tear blanks off 
            // from all the Entry controls that input a number 
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text.Replace(" ", "");
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text.Replace(" ", "");

            if (cmbSensitivityFactor.SelectedItem != null)
                bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text = cmbSensitivityFactor.SelectedItem.ToString();

            bolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text.Replace(" ", "");
            bolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text.Replace(" ", "");
            bolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text.Replace(" ", "");
            bolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text.Replace(" ", "");

            bolusCalculation.InsulinCorrectionSensitivity.Text = txtInsulinCorrectionSensitivity.Text.Replace(" ", "");

            // user has no action on Total Daily Dose Of Insulin: we do nothing 
        }
        private void btnCalculateInsulinSensitivity_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.CalculateInsulinCorrectionSensitivity();
            FromClassToUi();
        }
        private void btnSaveInsulinSensitivity_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.SaveInsulinCorrectionParameters();
            FromClassToUi();
        }
        private void btnSaveRatioCHOInsulin_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.SaveRatioCHOInsulinParameters();
            FromClassToUi();
        }  
        private void btnPhysicalActivityCalculation_Click(object sender, EventArgs e)
        {
            // !!!! TODO !!!! ready to host the code to calculate the sensitivenesses
            // (when I will have an algorithm!!)
        }
    }
}