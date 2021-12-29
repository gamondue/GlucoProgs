using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Windows.Forms;

namespace GlucoMan.Forms
{
    public partial class frmWeighFood : Form
    {
        string persistentStorage = CommonData.PathConfigurationData + @"WeighFood.txt";

        BL_WeighFood food = new BL_WeighFood();
        BL_GrossTareAndNetWeight M0Raw;
        BL_GrossTareAndNetWeight S1Sauce;
        BL_GrossTareAndNetWeight T0Raw;
        BL_GrossTareAndNetWeight T1Cooked;

        public frmWeighFood()
        {
            InitializeComponent();

            food.RestoreData();
            FromClassToUi();
        }
        private void frmWeighFood_Load(object sender, EventArgs e)
        {
            M0Raw = new BL_GrossTareAndNetWeight(food.M0RawGross, food.M0RawTare,
                food.M0RawNet);
            S1Sauce = new BL_GrossTareAndNetWeight(food.S1SauceGross, food.S1SauceTare, food.S1SauceNet);
            T0Raw = new BL_GrossTareAndNetWeight(food.T0RawGross, food.T0RawTare, food.T0RawNet);
            T1Cooked= new BL_GrossTareAndNetWeight(food.T1CookedGross, food.T1CookedTare, food.T1CookedNet);
        }
        internal void FromUiToClass()
        {
            food.M0RawNet.Text = TxtM0RawGross.Text;
            food.M0RawNet.Text = TxtM0RawTare.Text;
            food.M0RawNet.Text = TxtM0RawNet.Text;

            food.S1SauceGross.Text = TxtS1SauceGross.Text;
            food.S1SauceTare.Text = TxtS1SauceTare.Text;
            food.S1SauceNet.Text = TxtS1SauceNet.Text;

            food.T0RawGross.Text = TxtT0RawGross.Text;
            food.T0RawTare.Text = TxtT0RawTare.Text;
            food.T0RawNet.Text = TxtT0RawNet.Text;
            food.T0SaucePlusTare.Text = TxtT0SaucePlusTare.Text; 

            ////////food.Mp0PortionReportedToRaw.Text = TxtMp0PortionReportedToRaw.Text;
            ////////food.S1pPotSaucePlusPot.Text = TxtS1SauceNet.Text; // !!!!!!
            ////////food.S1pPotSaucePlusPot.Text = TxtS1pPotSaucePlusPot.Text;
            ////////food.DiDish.Text = TxtDiDish.Text;
            ////////food.T0RawGross.Text = TxtT0RawGross.Text;
            ////////food.TpPortionWithAll.Text = TxtTpPortionWithAll.Text;
            ////////food.M0pS1pPeRawFoodAndSauce.Text = TxtT0RawNet.Text;
            ////////food.M1MainfoodCooked.Text = TxtM1CourseCooked.Text;
            ////////food.M1pS1CourseCookedPlusSauce.Text = TxtM1pS1CourseCookedPlusSauce.Text;
            ////////food.M1pS1CourseCookedPlusSauce.Text = TxtACookRatio.Text;
            ////////food.SpPortionOfSauceInGrams.Text = TxtSpPortionOfSauceInGrams.Text;
            ////////food.M1pS1CourseCookedPlusSauce.Text = TxtM1pS1CourseAndSauceCooked.Text;
            ////////food.MppSpPortionOfCoursePlusSauce.Text = TxtMppSpPortionOfCoursePlusSauce.Text;
            ////////food.PPercPercentageOfPortion.Text = TxtPPercPercentageOfPortion.Text;
            ////////food.ChoSaucePercent.Text = TxtChoSaucePercent.Text;
            ////////food.ChoMainfoodPercent.Text = TxtChoMainfoodPercent.Text;
            ////////food.ChoTotalMainfood.Text = TxtChoTotalMainfood.Text;
            ////////food.ChoTotalSauce.Text = TxtChoTotalSauce.Text; 
        }
        internal void FromClassToUi()
        {
            TxtM0RawGross.Text = food.M0RawGross.Text;
            TxtM0RawTare.Text = food.M0RawTare.Text;
            TxtM0RawNet.Text = food.M0RawNet.Text;

            TxtS1SauceGross.Text = food.S1SauceGross.Text;
            TxtS1SauceTare.Text = food.S1SauceTare.Text;
            TxtS1SauceNet.Text = food.S1SauceNet.Text;

            TxtT0RawGross.Text = food.T0RawGross.Text;
            TxtT0RawTare.Text = food.T0RawTare.Text;
            TxtT0RawNet.Text = food.T0RawNet.Text;
            TxtT0SaucePlusTare.Text = food.T0SaucePlusTare.Text;

            ////////TxtMp0PortionReportedToRaw.Text = food.Mp0PortionReportedToRaw.Text;
            ////////TxtS1pPotSaucePlusPot.Text = food.S1pPotSaucePlusPot.Text;
            ////////TxtDiDish.Text = food.DiDish.Text;

            ////////TxtTpPortionWithAll.Text = food.TpPortionWithAll.Text;
            ////////TxtM1CourseCooked.Text = food.M1MainfoodCooked.Text;
            ////////TxtM1pS1CourseCookedPlusSauce.Text = food.M1pS1CourseCookedPlusSauce.Text;
            ////////TxtACookRatio.Text = food.M1pS1CourseCookedPlusSauce.Text;
            ////////TxtSpPortionOfSauceInGrams.Text = food.SpPortionOfSauceInGrams.Text;
            ////////TxtM1pS1CourseAndSauceCooked.Text = food.M1pS1CourseCookedPlusSauce.Text;
            ////////TxtMppSpPortionOfCoursePlusSauce.Text = food.MppSpPortionOfCoursePlusSauce.Text;
            ////////TxtPPercPercentageOfPortion.Text = food.PPercPercentageOfPortion.Text;
            ////////TxtChoSaucePercent.Text = food.ChoSaucePercent.Text;
            ////////TxtChoMainfoodPercent.Text = food.ChoMainfoodPercent.Text;
            ////////TxtChoTotalMainfood.Text = food.ChoTotalMainfood.Text;
            ////////TxtChoTotalSauce.Text = food.ChoTotalSauce.Text;
        }
        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            food.CalcUnknownData();
            FromClassToUi();

            food.SaveData(); 
        }
        private void TxtS1pPotSaucePlusPot_TextChanged(object sender, EventArgs e)
        {
             
        }
        private void TxtM0RawGross_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtM0RawGross_Leave(object sender, EventArgs e)
        {
            food.M0RawGross.Text = TxtM0RawGross.Text;
            M0Raw.GrossOrTareChanged();
            
            FromClassToUi();
        }
        private void TxtM0RawTare_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtM0RawTare_Leave(object sender, EventArgs e)
        {
            food.M0RawTare.Text = TxtM0RawTare.Text;
            M0Raw.GrossOrTareChanged();
            FromClassToUi();
        }
        private void TxtM0RawNet_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtM0RawNet_Leave(object sender, EventArgs e)
        {
            food.M0RawNet.Text = TxtM0RawNet.Text;
            M0Raw.NetWeightChanged();
            FromClassToUi();
        }
        private void BtnM0RawChooseTare_Click(object sender, EventArgs e)
        {

        }
        private void TxtS1SauceGross_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtS1SauceGross_Leave(object sender, EventArgs e)
        {
            food.S1SauceGross.Text = TxtS1SauceGross.Text;
            S1Sauce.GrossOrTareChanged();
            FromClassToUi();
        }
        private void TxtS1SauceTare_Leave(object sender, EventArgs e)
        {
            food.S1SauceTare.Text = TxtS1SauceTare.Text;
            S1Sauce.GrossOrTareChanged();
            FromClassToUi();
        }
        private void TxtS1pPotSaucePlusPot_Leave(object sender, EventArgs e)
        {
            FromUiToClass();
            if (food.T0RawTare.Text != "")
                food.S1SauceNet.Double = food.S1pPotSaucePlusPot.Double -
                food.T0RawTare.Double;
            food.T0RawGross.Double = food.T0RawGross.Double +
                food.S1SauceNet.Double + food.T0RawTare.Double;

            food.CalcUnknownData();
            FromClassToUi();
        }
        private void TxtT0RawGross_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtT0RawGross_Leave(object sender, EventArgs e)
        {
            food.T0RawGross.Text = TxtT0RawGross.Text;
            T0Raw.GrossOrTareChanged();
            food.T0SaucePlusTare.Double  = food.S1SauceNet.Double + food.T0RawTare.Double;
            TxtT0SaucePlusTare.Text = food.T0SaucePlusTare.Text; 
            FromClassToUi();
        }
        private void TxtPotRawTare_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtPotRawTare_Leave(object sender, EventArgs e)
        {
            food.T0RawTare.Text = TxtT0RawTare.Text;
            T0Raw.GrossOrTareChanged();
            TxtT0SaucePlusTare.Text = (food.S1SauceNet.Double + food.T0RawTare.Double).ToString();
            FromClassToUi();
        }
        private void TxtT0SaucePlusTare_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtT0SaucePlusTare_Leave(object sender, EventArgs e)
        {
            // TxtT0SaucePlusTare
        }
        private void TxtT0RawNet_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtT0RawNet_Leave(object sender, EventArgs e)
        {
            food.T0RawNet.Text = TxtT0RawNet.Text;
            M0Raw.NetWeightChanged();

            TxtT0RawNet.Text = "";
            TxtT0SaucePlusTare.Text = (food.S1SauceNet.Double + food.T0RawTare.Double).ToString();
            FromClassToUi();
        }
        private void TxtS1SauceNet_TextChanged(object sender, EventArgs e)
        {

        }
        private void TxtS1SauceNet_Leave(object sender, EventArgs e)
        {
            food.S1SauceNet.Text = TxtS1SauceNet.Text;
            S1Sauce.NetWeightChanged();
            food.CalcUnknownData();
            FromClassToUi();
        }
    }
}
