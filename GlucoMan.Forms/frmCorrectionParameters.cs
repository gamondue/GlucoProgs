using GlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmCorrectionParameters : Form
    {
        BL_BolusCalculation currentBolusCalculation;
        public frmCorrectionParameters()
        {
            InitializeComponent();

            currentBolusCalculation = new BL_BolusCalculation();
        }
        private void frmCorrectionParameters_Load(object sender, EventArgs e)
        {
            currentBolusCalculation.RestoreInsulinCorrectionParameters();

            currentBolusCalculation.FactorOfInsulinCorrectionSensitivity.Format = "0";

            FromClassToUi(); 
        }
        private void FromClassToUi()
        {
            txtInsulinCorrectionSensitivity.Text = currentBolusCalculation.InsulinCorrectionSensitivity.Text;
            cmbSensitivityFactor.Text = currentBolusCalculation.FactorOfInsulinCorrectionSensitivity.Text;
            txtTotalDailyDoseOfInsulin.Text = currentBolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtTypicalBolusMidday.Text = currentBolusCalculation.TypicalBolusMidday.Text;
            txtTypicalBolusMorning.Text = currentBolusCalculation.TypicalBolusMorning.Text;
            txtTypicalBolusEvening.Text = currentBolusCalculation.TypicalBolusEvening.Text;
            txtTypicalBolusNight.Text = currentBolusCalculation.TypicalBolusNight.Text;
            txtChoInsulinRatioBreakfast.Text = currentBolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = currentBolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = currentBolusCalculation.ChoInsulinRatioDinner.Text;
        }
        private void FromUiToClass()
        {
            currentBolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text;
            currentBolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text;
            currentBolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text;
            currentBolusCalculation.FactorOfInsulinCorrectionSensitivity.Text = cmbSensitivityFactor.Text;
            currentBolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text;
            currentBolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text;
            currentBolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text;
            currentBolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text;
        }

        private void btnInsulinSensitivityCalculation_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            currentBolusCalculation.CalculateInsulinCorrectionSensitivity();
            currentBolusCalculation.SaveInsulinCorrectionParameters();
            FromClassToUi();
        }
    }
}
