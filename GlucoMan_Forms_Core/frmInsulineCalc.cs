using GlucoMan;
using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    public partial class frmInsulineCalc : Form
    {
        Bl_BolusCalculation bolus;
        BL_GlucoseMeasurements glucoseMeasuremet; 
        public frmInsulineCalc()
        {
            InitializeComponent();

            bolus = new Bl_BolusCalculation();
            glucoseMeasuremet = new BL_GlucoseMeasurements(); 
        }
        private void frmInsulineCalc_Load(object sender, EventArgs e)
        {
            bolus.RestoreData();
            FromClassToUi();
            txtGlucoseBeforeMeal.Focus();
        }
        private void txt_Leave(object sender, EventArgs e)
        {

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
            txtTdd.Text = bolus.TotalDailyDoseOfInsulin.Text;
            txtGlucoseBeforeMeal.Text = bolus.GlucoseBeforeMeal.Text;
            txtGlucoseToBeCorrected.Text = bolus.GlucoseToBeCorrected  .Text;
            txtCorrectionInsuline.Text = bolus.CorrectionInsuline.Text;
            txtChoInsulineBreakfast.Text = bolus.ChoInsulineBreakfast.Text;
            txtChoInsulineDinner.Text = bolus.ChoInsulineDinner.Text;
            txtChoInsulineLunch.Text = bolus.ChoInsulineLunch   .Text;
            txtTotalInsulineDinner.Text = bolus.TotalInsulineDinner.Text;
            txtTotalInsulineLunch.Text = bolus.TotalInsulineLunch.Text;
            txtTotalInsulineBreakfast.Text = bolus.TotalInsulineBreakfast.Text;
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
            bolus.ChoToEat.Text = txtChoToEat.Text;
            bolus.ChoInsulineRatioEvening.Text = txtRatioEvening.Text;
            bolus.ChoInsulineRatioMidday.Text = txtRatioMidday.Text;
            bolus.ChoInsulineRatioMorning.Text = txtRatioMorning.Text;
            bolus.GlucoseBeforeMeal.Text = txtGlucoseBeforeMeal.Text;
            bolus.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text;
            bolus.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text;
            bolus.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text;
            bolus.TypicalBolusNight.Text = txtTypicalBolusNight.Text;
            bolus.TargetGlucose.Text = txtTargetGlucose.Text;
        }

        private void frmInsulineCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            bolus.SaveData();
        }
        private void btnRoundInsuline_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolus.RoundInsulineToZeroDecimal();
            bolus.SaveData();
            FromClassToUi();
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            List<GlucoseRecord> list = glucoseMeasuremet.GetLastTwoGlucoseMeasurements();
            txtGlucoseBeforeMeal.Text =  list[0].GlucoseValue.ToString();
        }
    }
}
