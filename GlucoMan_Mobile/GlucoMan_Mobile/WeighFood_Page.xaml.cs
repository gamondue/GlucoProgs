using SharedData;
using SharedGlucoMan.BusinessLayer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeighFood_Page : ContentPage
    {
        string persistentStorage = CommonData.PathConfigurationData + @"WeighFood.txt";

        Bl_WeighFood food = new Bl_WeighFood();
        Bl_GrossTareAndNetWeight M0RawMain;
        Bl_GrossTareAndNetWeight S1Sauce;

        public WeighFood_Page()
        {
            InitializeComponent();

            food.RestoreData();
            FromClassToUi();

            M0RawMain = new Bl_GrossTareAndNetWeight(food.T0RawGross, food.T0RawTare,
                food.T0RawNet);
            S1Sauce = new Bl_GrossTareAndNetWeight(food.S1SauceGross, food.S1SauceTare, 
                food.S1SauceNet);
        }

        internal void FromUiToClass()
        {
            food.M0RawGross.Text = TxtM0RawGross.Text;
            food.M0RawTare.Text = TxtM0RawTare.Text;
            food.M0RawNet.Text = TxtM0RawNet.Text;
            
            //food.T0RawGross.Text = TxtT0RawGross.Text;
            //food.T0RawTare.Text = TxtT0RawTare.Text;
            //food.T0RawNet.Text = TxtT0RawNet.Text;

            food.S1SauceGross.Text = TxtS1SauceGross.Text;
            food.S1SauceTare.Text = TxtS1SauceTare.Text;
            food.S1SauceNet.Text = TxtS1SauceNet.Text;

            //food.Mp0PortionReportedToRaw.Text = TxtMp0PortionReportedToRaw.Text;
            //food.PotCookingPot.Text = TxtPotCookingPot.Text;
            //food.S1pPotSaucePlusPot.Text = TxtS1SauceNet.Text;
            //food.S1pPotSaucePlusPot.Text = TxtS1pPotSaucePlusPot.Text;
            //food.DiDish.Text = TxtDiDish.Text;
            //food.T0AllPreCooking.Text = TxtT0AllPreCooking.Text;
            //food.TpPortionWithAll.Text = TxtTpPortionWithAll.Text;
            //food.M0pS1pPeRawFoodAndSauce.Text = TxtM0pS1pPeRawFoodAndSauce.Text;
            //food.M1MainfoodCooked.Text = TxtM1CourseCooked.Text;
            //food.M1pS1CourseCookedPlusSauce.Text = TxtM1pS1CourseCookedPlusSauce.Text;
            //food.M1pS1CourseCookedPlusSauce.Text = TxtACookRatio.Text;
            //food.SpPortionOfSauceInGrams.Text = TxtSpPortionOfSauceInGrams.Text;
            //food.M1pS1CourseCookedPlusSauce.Text = TxtM1pS1CourseAndSauceCooked.Text;
            //food.MppSpPortionOfCoursePlusSauce.Text = TxtMppSpPortionOfCoursePlusSauce.Text;
            //food.PPercPercentageOfPortion.Text = TxtPPercPercentageOfPortion.Text;
            //food.ChoSaucePercent.Text = TxtChoSaucePercent.Text;
            //food.ChoMainfoodPercent.Text = TxtChoMainfoodPercent.Text;
            //food.ChoTotalMainfood.Text = TxtChoTotalMainfood.Text;
            //food.ChoTotalSauce.Text = TxtChoTotalSauce.Text;
        }
        internal void FromClassToUi()
        {
            TxtM0RawGross.Text = food.T0RawGross.Text;
            TxtM0RawTare.Text = food.T0RawTare.Text;
            TxtM0RawNet.Text = food.T0RawNet.Text;

            TxtS1SauceGross.Text = food.S1SauceGross.Text;
            TxtS1SauceTare.Text = food.S1SauceTare.Text;
            TxtS1SauceNet.Text = food.S1SauceNet.Text;

            //TxtMp0PortionReportedToRaw.Text = food.Mp0PortionReportedToRaw.Text;
            //TxtPotCookingPot.Text = food.PotCookingPot.Text;
            //TxtS1SauceNet.Text = food.S1pPotSaucePlusPot.Text;
            //TxtS1pPotSaucePlusPot.Text = food.S1pPotSaucePlusPot.Text;
            //TxtDiDish.Text = food.DiDish.Text;
            //TxtT0AllPreCooking.Text = food.T0AllPreCooking.Text;
            //TxtTpPortionWithAll.Text = food.TpPortionWithAll.Text;
            //TxtM0pS1pPeRawFoodAndSauce.Text = food.M0pS1pPeRawFoodAndSauce.Text;
            //TxtM1CourseCooked.Text = food.M1MainfoodCooked.Text;
            //TxtM1pS1CourseCookedPlusSauce.Text = food.M1pS1CourseCookedPlusSauce.Text;
            //TxtACookRatio.Text = food.M1pS1CourseCookedPlusSauce.Text;
            //TxtSpPortionOfSauceInGrams.Text = food.SpPortionOfSauceInGrams.Text;
            //TxtM1pS1CourseAndSauceCooked.Text = food.M1pS1CourseCookedPlusSauce.Text;
            //TxtMppSpPortionOfCoursePlusSauce.Text = food.MppSpPortionOfCoursePlusSauce.Text;
            //TxtPPercPercentageOfPortion.Text = food.PPercPercentageOfPortion.Text;
            //TxtChoSaucePercent.Text = food.ChoSaucePercent.Text;
            //TxtChoMainfoodPercent.Text = food.ChoMainfoodPercent.Text;
            //TxtChoTotalMainfood.Text = food.ChoTotalMainfood.Text;
            //TxtChoTotalSauce.Text = food.ChoTotalSauce.Text;
        }

        private void btnCalc_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            food.CalcUnknownData();
            FromClassToUi();

            food.SaveData();
        }
        private void TxtM0RawGross_Leave(object sender, FocusEventArgs e)
        {
            food.T0RawGross.Text = TxtM0RawGross.Text;
            M0RawMain.GrossOrTareChanged();
            FromClassToUi();
        }
        private void TxtS1pPotSaucePlusPot_TextChanged(object sender, EventArgs e)
        {

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
        private void TxtM0RawTare_Leave(object sender, EventArgs e)
        {
            food.T0RawTare.Text = TxtM0RawTare.Text;
            M0RawMain.GrossOrTareChanged();
            FromClassToUi();
        }

        private void TxtM0RawNet_Leave(object sender, EventArgs e)
        {
            food.T0RawNet.Text = TxtM0RawNet.Text;
            M0RawMain.NetWeightChanged();
            FromClassToUi();
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

        private void TxtS1SauceNet_Leave(object sender, EventArgs e)
        {
            food.S1SauceNet.Text = TxtS1SauceNet.Text;
            S1Sauce.NetWeightChanged();
            FromClassToUi();
        }
    }
}