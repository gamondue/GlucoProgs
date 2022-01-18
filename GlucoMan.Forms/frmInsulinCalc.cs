using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static GlucoMan.DataLayer;

namespace GlucoMan.Forms
{
    public partial class frmInsulinCalc : Form
    {
        BL_BolusCalculation currentBolusCalculation;
        BL_GlucoseMeasurements currentGlucoseMeasurement;
        public frmInsulinCalc()
        {
            InitializeComponent();

            currentBolusCalculation = new BL_BolusCalculation();
            currentGlucoseMeasurement = new BL_GlucoseMeasurements();
        }
        private void frmInsulinCalc_Load(object sender, EventArgs e)
        {
            currentBolusCalculation.RestoreBolusParameters(); 

            Common.SelectMealBasedOnTimeNow();
            switch (currentBolusCalculation.MealOfBolus.TypeOfMeal)
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
            currentBolusCalculation.TargetGlucose.Format = "0";
            currentBolusCalculation.GlucoseBeforeMeal.Format = "0";

            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();
        }
        private void FromClassToUi()
        {
            txtChoToEat.Text = currentBolusCalculation.ChoToEat.Text;
            txtInsulinCorrectionSensitivity.Text = currentBolusCalculation.InsulinCorrectionSensitivity.Text;
            txtTotalDailyDoseOfInsulin.Text = currentBolusCalculation.TotalDailyDoseOfInsulin.Text;
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
            switch (currentBolusCalculation.MealOfBolus.TypeOfMeal)
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
            currentBolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text;
            currentBolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text;
            currentBolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text;
            currentBolusCalculation.ChoToEat.Text = txtChoToEat.Text;            
            currentBolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text;
            currentBolusCalculation.TargetGlucose.Text = txtTargetGlucose.Text;
            currentBolusCalculation.InsulinCorrectionSensitivity.Text = txtInsulinCorrectionSensitivity.Text;

            if (rdbIsBreakfast.Checked)
                currentBolusCalculation.MealOfBolus.TypeOfMeal = Common.TypeOfMeal.Breakfast;
            if (rdbIsLunch.Checked)
                currentBolusCalculation.MealOfBolus.TypeOfMeal = Common.TypeOfMeal.Lunch;
            if (rdbIsDinner.Checked)
                currentBolusCalculation.MealOfBolus.TypeOfMeal = Common.TypeOfMeal.Dinner;
            if (rdbIsSnack.Checked)
                currentBolusCalculation.MealOfBolus.TypeOfMeal = Common.TypeOfMeal.Snack;
        }
        private void frmInsulinCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.SaveBolusParameters();
        }
        private void txt_Leave(object sender, EventArgs e)
        {

        }
        private void btnSetParameters_Click(object sender, EventArgs e)
        {
            frmCorrectionParameters f = new frmCorrectionParameters();
            f.ShowDialog(); // modal form 
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
            List<GlucoseRecord> list = currentGlucoseMeasurement.GetLastTwoGlucoseMeasurements();
            txtGlucoseBeforeMeal.Text =  list[0].GlucoseValue.ToString();
        }
        private void btnSaveBolus_Click(object sender, EventArgs e)
        {
            currentBolusCalculation.SaveBolusParameters();
        }
        // !!!! TODO put the following into Xamarin project
        private void btnOpenGlucose_Click(object sender, EventArgs e)
        {
            frmGlucose frm = new frmGlucose();  
            frm.Show();
        }
    }
}
