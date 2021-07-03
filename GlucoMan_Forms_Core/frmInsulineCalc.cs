using GlucoMan;
using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    public partial class frmInsulineCalc : Form
    {
        BL_BolusCalculation bolusCalculation;
        BL_GlucoseMeasurements glucoseMeasuremet;
        public frmInsulineCalc()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation();
            glucoseMeasuremet = new BL_GlucoseMeasurements();
        }
        private void frmInsulineCalc_Load(object sender, EventArgs e)
        {
            bolusCalculation.RestoreData();
            bolusCalculation.MealOfBolus.SelectMealBasedOnTimeNow();

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
            txtInsulineSensitivity.Text = bolusCalculation.InsulineSensitivity.Text;
            cmbSensitivityFactor.Text = bolusCalculation.InsulineSensitivityFactor.Text;
            txtTdd.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = bolusCalculation.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolusCalculation.GlucoseToBeCorrected  .Text;
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
            txtStaturBar.Text = bolusCalculation.StatusMessage; 
            switch (bolusCalculation.MealOfBolus.Type)
            {
                case (Meal.TypeOfMeal.Breakfast):
                    rdbIsBreakfast.Checked = true;
                    break;
                case (Meal.TypeOfMeal.Dinner):
                    rdbIsDinner.Checked = true;
                    break;
                case (Meal.TypeOfMeal.Lunch):
                    rdbIsLunch.Checked = true;
                    break;
                case (Meal.TypeOfMeal.Snack):
                    rdbIsSnack.Checked = true;
                    break;
            }
            txtChoInsulineRatioBreakfast.Text = bolusCalculation.ChoInsulineRatioBreakfast.Text;
            txtChoInsulineRatioLunch.Text = bolusCalculation.ChoInsulineRatioLunch.Text;
            txtChoInsulineRatioDinner.Text = bolusCalculation.ChoInsulineRatioDinner.Text;
        }
        private void FromUiToClass()
        {
            bolusCalculation.ChoToEat.Text = txtChoToEat.Text;
            bolusCalculation.ChoInsulineRatioDinner.Text = txtChoInsulineRatioDinner.Text;
            bolusCalculation.ChoInsulineRatioBreakfast.Text = txtChoInsulineRatioBreakfast.Text;
            bolusCalculation.ChoInsulineRatioLunch.Text = txtChoInsulineRatioLunch.Text;
            bolusCalculation.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text;
            bolusCalculation.InsulineSensitivityFactor.Text = cmbSensitivityFactor.Text; 
            bolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text;
            bolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text;
            bolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text;
            bolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text;
            bolusCalculation.TargetGlucose.Text = txtTargetGlucose.Text;

            if (rdbIsBreakfast.Checked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Breakfast;
            if (rdbIsLunch.Checked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Lunch;
            if (rdbIsDinner.Checked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Dinner;
            if (rdbIsSnack.Checked)
                bolusCalculation.MealOfBolus.Type = Meal.TypeOfMeal.Snack;
        }

        private void frmInsulineCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            bolusCalculation.SaveData();
        }
        private void btnRoundInsuline_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.RoundInsulineToZeroDecimal();
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
    }
}
