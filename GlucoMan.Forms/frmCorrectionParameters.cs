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
            bolusCalculation.RestoreInsulinCorrectionParameters();
            bolusCalculation.RestoreRatioCHOInsulinParameters(); 

            bolusCalculation.FactorOfInsulinCorrectionSensitivity.Format = "0";

            FromClassToUi(); 
        }
        private void FromClassToUi()
        {
            txtChoInsulinRatioDinner.Text = bolusCalculation.ChoInsulinRatioDinner.Text;
            txtChoInsulinRatioBreakfast.Text = bolusCalculation.ChoInsulinRatioBreakfast.Text;
            txtChoInsulinRatioLunch.Text = bolusCalculation.ChoInsulinRatioLunch.Text;

            cmbSensitivityFactor.Text = bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text;

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
            bolusCalculation.ChoInsulinRatioDinner.Text = txtChoInsulinRatioDinner.Text;
            bolusCalculation.ChoInsulinRatioBreakfast.Text = txtChoInsulinRatioBreakfast.Text;
            bolusCalculation.ChoInsulinRatioLunch.Text = txtChoInsulinRatioLunch.Text;
            
            bolusCalculation.FactorOfInsulinCorrectionSensitivity.Text = cmbSensitivityFactor.Text;
            
            bolusCalculation.TypicalBolusMidday.Text = txtTypicalBolusMidday.Text;
            bolusCalculation.TypicalBolusMorning.Text = txtTypicalBolusMorning.Text;
            bolusCalculation.TypicalBolusEvening.Text = txtTypicalBolusEvening.Text;
            bolusCalculation.TypicalBolusNight.Text = txtTypicalBolusNight.Text;

            bolusCalculation.InsulinCorrectionSensitivity.Text = txtInsulinCorrectionSensitivity.Text;

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
        private void btnSavePhysicalActivity_Click(object sender, EventArgs e)
        {
            // !!!! TODO !!!! ready to host the code to calculate the sensitivenesses
            // (when I will have an algorithm!!)
        }
    }
}
