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
        BL_BolusCalculation bolusCalculation;
        public frmCorrectionParameters()
        {
            InitializeComponent();

            bolusCalculation = new BL_BolusCalculation();
        }
        private void frmCorrectionParameters_Load(object sender, EventArgs e)
        {
            bolusCalculation.RestoreInsulinParameters();
            bolusCalculation.FactorOfInsulinCorrectionSensitivity.Format = "0"; 
            FromClassToUi();
        }
        private void FromClassToUi()
        {
            txtInsulinSensitivity.Text = bolusCalculation.InsulinCorrectionSensitivity.Text;
            cmbSensitivityFactor.Text = bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text;
            txtTdd.Text = bolusCalculation.TotalDailyDoseOfInsulin.Text;
            txtTypicalBolusMidday.Text = bolusCalculation.TypicalBolusMidday.Text;
            txtTypicalBolusMorning.Text = bolusCalculation.TypicalBolusMorning.Text;
            txtTypicalBolusEvening.Text = bolusCalculation.TypicalBolusEvening.Text;
            txtTypicalBolusNight.Text = bolusCalculation.TypicalBolusNight.Text;

            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;
        }
        private void FromUiToClass()
        {
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text;
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text;
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text;

            bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text = cmbSensitivityFactor.Text;

            bolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text;
            bolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text;
            bolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text;
            bolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text;
        }

        private void btnInsulinSensitivityCalculation_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            bolusCalculation.CalculateInsulinCorrectionSensitivity();
            bolusCalculation.SaveInsulinParameters(); 
            FromClassToUi();
        }
    }
}
