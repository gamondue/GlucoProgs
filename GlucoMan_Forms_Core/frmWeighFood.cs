using SharedData;
using SharedGlucoMan.BusinessLayer;
using System;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    public partial class frmWeighFood : Form
    {
        string persistentStorage = CommonData.PathConfigurationData + @"WeighFood.txt";

        WeighFood food = new WeighFood(); 

        public frmWeighFood()
        {
            InitializeComponent();

            food.RestoreData();
            FromClassToUi();
        }

        private void frmWeighFood_Load(object sender, EventArgs e)
        {

        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {

        }
        internal void FromUiToClass()
        {
            food.Mp0PortionReportedToRaw.Text = TxtMp0PortionReportedToRaw.Text;
            food.M0AllRawMainFood.Text = TxtM0AllRawMainFood.Text;
            food.PotCookingPot.Text = TxtPotCookingPot.Text;
            food.S1pPotSaucePlusPot.Text = TxtS1pPotAllSauce.Text;
            food.S1pPotSaucePlusPot.Text = TxtS1pPotSaucePlusPot.Text;
            food.DiDish.Text = TxtDiDish.Text;
            food.T0AllPreCooking.Text = TxtT0AllPreCooking.Text;
            food.TpPortionWithAll.Text = TxtTpPortionWithAll.Text;
            food.M0pS1pPeRawFoodAndSauce.Text = TxtM0pS1pPeRawFoodAndSauce.Text;
            food.M1MainfoodCooked.Text = TxtM1CourseCooked.Text;
            food.M1pS1CourseCookedPlusSauce.Text = TxtM1pS1CourseCookedPlusSauce.Text;
            food.M1pS1CourseCookedPlusSauce.Text = TxtACookRatio.Text;
            food.SpPortionOfSauceInGrams.Text = TxtSpPortionOfSauceInGrams.Text;
            food.M1pS1CourseCookedPlusSauce.Text = TxtM1pS1CourseAndSauceCooked.Text;
            food.MppSpPortionOfCoursePlusSauce.Text = TxtMppSpPortionOfCoursePlusSauce.Text;
            food.PPercPercentageOfPortion.Text = TxtPPercPercentageOfPortion.Text;
            food.ChoSaucePercent.Text = TxtChoSaucePercent.Text;
            food.ChoMainfoodPercent.Text = TxtChoMainfoodPercent.Text;
            food.ChoTotalMainfood.Text = TxtChoTotalMainfood.Text;
            food.ChoTotalSauce.Text = TxtChoTotalSauce.Text; 
        }
        internal void FromClassToUi()
        {
            TxtMp0PortionReportedToRaw.Text = food.Mp0PortionReportedToRaw.Text;
            TxtM0AllRawMainFood.Text = food.M0AllRawMainFood.Text;
            TxtPotCookingPot.Text = food.PotCookingPot.Text;
            TxtS1pPotAllSauce.Text = food.S1pPotSaucePlusPot.Text;
            TxtS1pPotSaucePlusPot.Text = food.S1pPotSaucePlusPot.Text;
            TxtDiDish.Text = food.DiDish.Text;
            TxtT0AllPreCooking.Text = food.T0AllPreCooking.Text;
            TxtTpPortionWithAll.Text = food.TpPortionWithAll.Text;
            TxtM0pS1pPeRawFoodAndSauce.Text = food.M0pS1pPeRawFoodAndSauce.Text;
            TxtM1CourseCooked.Text = food.M1MainfoodCooked.Text;
            TxtM1pS1CourseCookedPlusSauce.Text = food.M1pS1CourseCookedPlusSauce.Text;
            TxtACookRatio.Text = food.M1pS1CourseCookedPlusSauce.Text;
            TxtSpPortionOfSauceInGrams.Text = food.SpPortionOfSauceInGrams.Text;
            TxtM1pS1CourseAndSauceCooked.Text = food.M1pS1CourseCookedPlusSauce.Text;
            TxtMppSpPortionOfCoursePlusSauce.Text = food.MppSpPortionOfCoursePlusSauce.Text;
            TxtPPercPercentageOfPortion.Text = food.PPercPercentageOfPortion.Text;
            TxtChoSaucePercent.Text = food.ChoSaucePercent.Text;
            TxtChoMainfoodPercent.Text = food.ChoMainfoodPercent.Text;
            TxtChoTotalMainfood.Text = food.ChoTotalMainfood.Text;
            TxtChoTotalSauce.Text = food.ChoTotalSauce.Text;
        }
        private void textBox_Leave(object sender, EventArgs e)
        {

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
        private void TxtS1pPotSaucePlusPot_Leave(object sender, EventArgs e)
        {
            FromUiToClass();
            if (food.PotCookingPot.Text != "")  
                food.S1AllSauce.Double = food.S1pPotSaucePlusPot.Double - 
                food.PotCookingPot.Double;
            food.T0AllPreCooking.Double = food.M0AllRawMainFood.Double +
                food.S1AllSauce.Double + food.PotCookingPot.Double; 
                
                food.CalcUnknownData(); 
            FromClassToUi();
        }
    }
}
