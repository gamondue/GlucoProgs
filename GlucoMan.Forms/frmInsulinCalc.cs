using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmInsulinCalc : Form
    {
        BL_BolusCalculation bolusCalculation;
        BL_GlucoseMeasurements glucoseMeasuremet;
        public frmInsulinCalc()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation();
            glucoseMeasuremet = new BL_GlucoseMeasurements();
        }
        private void frmInsulinCalc_Load(object sender, EventArgs e)
        {
            bolusCalculation.RestoreData();
            Common.SelectMealBasedOnTimeNow();

            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();
        }
        private void txt_Leave(object sender, EventArgs e)
        {

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
            cmbSensitivityFactor.Text = bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text;
            txtTdd.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = bolusCalculation.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolusCalculation.GlucoseToBeCorrected  .Text;
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
                case (Common.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.Checked = true;
                    break;
                case (Common.TypeOfMeal.Dinner):
                    rdbIsDinner.Checked = true;
                    break;
                case (Common.TypeOfMeal.Lunch):
                    rdbIsLunch.Checked = true;
                    break;
                case (Common.TypeOfMeal.Snack):
                    rdbIsSnack.Checked = true;
                    break;
            }
            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;
        }
        private void FromUiToClass()
        {
            bolusCalculation.ChoToEat.Text = txtChoToEat.Text;
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text;
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text;
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text;
            
            bolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text;
            bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text = cmbSensitivityFactor.Text;

            bolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text;
            bolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text;
            bolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text;
            bolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text;
            bolusCalculation.TargetGlucose.Text = txtTargetGlucose.Text;
        }

        private void frmInsulinCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            bolusCalculation.SaveData();
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
            List<GlucoseRecord> list = glucoseMeasuremet.GetLastTwoGlucoseMeasurements();
            txtGlucoseBeforeMeal.Text =  list[0].GlucoseValue.ToString();
        }

        private void txtRatioMidday_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnInsulinSensitivityCalculation_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            bolusCalculation.CalculateInsulinCorrectionSensitivity();  
            // we don't save here! 
            FromClassToUi(); 
        }

        private void btnSaveBolus_Click(object sender, EventArgs e)
        {

        }
    }
}
