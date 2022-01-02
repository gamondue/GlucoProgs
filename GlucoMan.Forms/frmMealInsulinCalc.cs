using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmMealInsulinCalc : Form
    {
        BL_BolusCalculation bolusCalculation;
        BL_GlucoseMeasurements glucoseMeasurement;
        public frmMealInsulinCalc()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation();
            glucoseMeasurement = new BL_GlucoseMeasurements();
        }
        private void frmInsulinCalc_Load(object sender, EventArgs e)
        {
            bolusCalculation.RestoreBolusData();
            bolusCalculation.RestoreInsulinParameters();
            Common.SelectMealBasedOnTimeNow();

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
            txtGlucoseToBeCorrected.Text = bolusCalculation.GlucoseToBeCorrected  .Text;
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
        }
        private void FromUiToClass()
        {
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text;
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text;
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text;
            bolusCalculation.ChoToEat.Text = txtChoToEat.Text;            
            bolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text;
            bolusCalculation.TargetGlucose.Text = txtTargetGlucose.Text;

            if (rdbIsBreakfast.Checked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Breakfast;
            if (rdbIsLunch.Checked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Lunch;
            if (rdbIsDinner.Checked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Dinner;
            if (rdbIsSnack.Checked)
                bolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Snack;
        }
        private void frmInsulinCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            bolusCalculation.SaveBolusData();
        }
        private void txt_Leave(object sender, EventArgs e)
        {

        }
        private void btnSetParameters_Click(object sender, EventArgs e)
        {
            frmCorrectionParameters f = new frmCorrectionParameters();
            f.ShowDialog();
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
            List<GlucoseRecord> list = glucoseMeasurement.GetLastTwoGlucoseMeasurements();
            txtGlucoseBeforeMeal.Text =  list[0].GlucoseValue.ToString();
        }
        private void btnSaveBolus_Click(object sender, EventArgs e)
        {
            bolusCalculation.SaveBolusData();
        }
    }
}
