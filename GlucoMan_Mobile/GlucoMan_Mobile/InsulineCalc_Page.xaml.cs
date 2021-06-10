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
        Bl_BolusCalculation bolus;
        public InsulineCalc_Page()
        {
            InitializeComponent();

            bolus = new Bl_BolusCalculation();
            bolus.RestoreData();
            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();

            //tempText.Text = CommonData.PathUsersDownload; 
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolus.CalculateBolus();
            bolus.SaveData();
            bolus.SaveLog();
            FromClassToUi();
        }

        private void FromClassToUi()
        {
            txtChoToEat.Text = bolus.ChoToEat.Text;
            txtRatioEvening.Text = bolus.ChoInsulineRatioEvening.Text;
            txtRatioMidday.Text = bolus.ChoInsulineRatioMidday.Text;
            txtRatioMorning.Text = bolus.ChoInsulineRatioMorning.Text;
            txtSensitivity1800.Text = bolus.InsulineSensitivity1800.Text;
            txtSensitivity1500.Text = bolus.InsulineSensitivity1500.Text;
            txtTdd.Text = bolus.Tdd.Text;
            txtGlucoseBeforeMeal.Text = bolus.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolus.GlucoseToBeCorrected.Text;
            txtCorrectionInsuline.Text = bolus.CorrectionInsuline.Text;
  
            txtTotalChoDinner.Text = bolus.TotalInsulineDinner.Text;
            txtTotalChoLunch.Text = bolus.TotalInsulineLunch.Text;
            txtTotalChoBreakfast.Text = bolus.TotalInsulineBreakfast.Text;
            txtTypicalBolusMidday.Text = bolus.TypicalBolusMidday.Text;
            txtTypicalBolusMorning.Text = bolus.TypicalBolusMorning.Text;
            txtTypicalBolusEvening.Text = bolus.TypicalBolusEvening.Text;
            txtTypicalBolusNight.Text = bolus.TypicalBolusNight.Text;
            txtTargetGlucose.Text = bolus.TargetGlucose.Text;

            txtRatioEvening.Text = bolus.ChoInsulineRatioEvening.Text;
            txtRatioMidday.Text = bolus.ChoInsulineRatioMidday.Text;
            txtRatioMorning.Text = bolus.ChoInsulineRatioMorning.Text;
        }
        private void FromUiToClass()
        {
            // since it is easy to mistakenly insert blanks during editing, we tear blanks off 
            // from all the Entry controls that input a number 
            bolus.ChoToEat.Text = txtChoToEat.Text.Replace(" ", "");
            bolus.ChoInsulineRatioEvening.Text = txtRatioEvening.Text.Replace(" ", "");
            bolus.ChoInsulineRatioMidday.Text = txtRatioMidday.Text.Replace(" ", "");
            bolus.ChoInsulineRatioMorning.Text = txtRatioMorning.Text.Replace(" ", "");

            bolus.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text.Replace(" ", "");

            bolus.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text.Replace(" ", "");
            bolus.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text.Replace(" ", "");
            bolus.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text.Replace(" ", "");
            bolus.TypicalBolusNight.Text = txtTypicalBolusNight.Text.Replace(" ", "");
            bolus.TargetGlucose.Text = txtTargetGlucose.Text.Replace(" ", "");
        }
        private void btnRoundInsuline_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolus.RoundInsulineToZeroDecimal();
            bolus.SaveData();
            FromClassToUi();
        }
    }
}