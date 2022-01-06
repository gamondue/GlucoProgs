using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static GlucoMan.DataLayer;

namespace GlucoMan.Forms
{
    public partial class frmMealInsulinCalc : Form
    {
        BL_BolusCalculation currentBolusCalculation;
        BL_GlucoseMeasurements currentGlucoseMeasurement;
        public frmMealInsulinCalc()
        {
            InitializeComponent();

            string prova = "";

            currentBolusCalculation = new BL_BolusCalculation();
            currentGlucoseMeasurement = new BL_GlucoseMeasurements();
        }
        private void frmInsulinCalc_Load(object sender, EventArgs e)
        {
            BL_BolusCalculation dummyBolus = new BL_BolusCalculation();

            //txtGlucoseBeforeMeal.Text = Common.Bl.RestoreParameter("GlucoseBeforeMeal");
            txtGlucoseBeforeMeal.Text = Common.Bl.RestoreParameter("TargetGlucose");
            txtTargetGlucose.Text = Common.Bl.RestoreParameter("TargetGlucose");
            txtChoInsulinRatioBreakfast.Text = Common.Bl.RestoreParameter("ChoInsulinRatioBreakfast");
            txtChoInsulinRatioLunch.Text = Common.Bl.RestoreParameter("ChoInsulinRatioLunch");
            txtChoInsulinRatioDinner.Text = Common.Bl.RestoreParameter("ChoInsulinRatioDinner");
            txtTotalDailyDoseOfInsulin.Text = Common.Bl.RestoreParameter("TotalDailyDoseOfInsulin");
            txtInsulinCorrectionSensitivity.Text = Common.Bl.RestoreParameter("InsulinCorrectionSensitivity");

            Common.SelectMealBasedOnTimeNow();
            switch (currentBolusCalculation.MealOfBolus.Type)
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

            //FromClassToUi();

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
            switch (currentBolusCalculation.MealOfBolus.Type)
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

            if (rdbIsBreakfast.Checked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Breakfast;
            if (rdbIsLunch.Checked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Lunch;
            if (rdbIsDinner.Checked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Dinner;
            if (rdbIsSnack.Checked)
                currentBolusCalculation.MealOfBolus.Type = Common.TypeOfMeal.Snack;
        }
        private void frmInsulinCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.SaveBolusData();
        }
        private void txt_Leave(object sender, EventArgs e)
        {

        }
        private void btnSetParameters_Click(object sender, EventArgs e)
        {
            frmCorrectionParameters f = new frmCorrectionParameters();
            f.ShowDialog();
            FromClassToUi();
        }
        private void btnBolusCalculations_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.CalculateBolus();

            Common.Bl.SaveParameter("TargetGlucose", currentBolusCalculation.TargetGlucose.Text);
            Common.Bl.SaveParameter("ChoInsulinRatioBreakfast", currentBolusCalculation.ChoInsulinRatioBreakfast.Text);
            Common.Bl.SaveParameter("ChoInsulinRatioLunch", currentBolusCalculation.ChoInsulinRatioLunch.Text);
            Common.Bl.SaveParameter("ChoInsulinRatioDinner", currentBolusCalculation.ChoInsulinRatioDinner.Text);
            Common.Bl.SaveParameter("TotalDailyDoseOfInsulin", currentBolusCalculation.TotalDailyDoseOfInsulin.Text);
            Common.Bl.SaveParameter("InsulinCorrectionSensitivity", currentBolusCalculation.InsulinCorrectionSensitivity.Text);

            currentBolusCalculation.SaveBolusData();
            currentBolusCalculation.SaveBolusLog();
            FromClassToUi();
        }
        private void btnRoundInsulin_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.RoundInsulinToZeroDecimal();
            currentBolusCalculation.SaveBolusData();
            FromClassToUi();
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            List<GlucoseRecord> list = currentGlucoseMeasurement.GetLastTwoGlucoseMeasurements();
            txtGlucoseBeforeMeal.Text =  list[0].GlucoseValue.ToString();
        }
        private void btnSaveBolus_Click(object sender, EventArgs e)
        {
            currentBolusCalculation.SaveBolusData();
        }

        private void btnOpenGlucose_Click(object sender, EventArgs e)
        {
            frmGlucose frm = new frmGlucose();  
            frm.Show();
        }
    }
}
