using GlucoMan;
using SharedData;
using SharedFunctions;
using System;
using System.Drawing;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    class Bl_WeighFood
    {
        DataLayer dl = new DataLayer();

        Color inputColor = Color.PaleGreen;
        Color inputSlowChangingColor = Color.PaleGoldenrod;
        Color toBeinputColor = Color.Red;
        Color calculatedImportantColor = Color.Yellow;
        Color calculatedNonImportantColor = Color.White;
        Color programsResult = Color.SkyBlue;

        internal DoubleAndText M0RawGross = new DoubleAndText();
        internal DoubleAndText M0RawTare = new DoubleAndText();
        internal DoubleAndText M0RawNet = new DoubleAndText();

        internal DoubleAndText S1SauceGross = new DoubleAndText();
        internal DoubleAndText S1SauceTare = new DoubleAndText();
        internal DoubleAndText S1SauceNet = new DoubleAndText();

        internal DoubleAndText T0RawGross = new DoubleAndText();
        internal DoubleAndText T0RawTare = new DoubleAndText();
        internal DoubleAndText T0RawNet = new DoubleAndText();
        internal DoubleAndText T0SaucePlusTare = new DoubleAndText();

        internal DoubleAndText S1pPotSaucePlusPot = new DoubleAndText(); // eliminare !!
        internal DoubleAndText DiDish = new DoubleAndText();
        internal DoubleAndText T1CookedGross = new DoubleAndText();
        internal DoubleAndText T1CookedTare = new DoubleAndText();
        internal DoubleAndText T1CookedNet = new DoubleAndText();
        internal DoubleAndText TpPortionWithAll = new DoubleAndText();
        internal DoubleAndText M0pS1pPeRawFoodAndSauce = new DoubleAndText();
        internal DoubleAndText M1MainfoodCooked = new DoubleAndText();
        internal DoubleAndText M1pS1CourseCookedPlusSauce = new DoubleAndText();
        internal DoubleAndText ACookRatio = new DoubleAndText();
        internal DoubleAndText MppSpPortionOfCoursePlusSauce = new DoubleAndText();
        internal DoubleAndText PPercPercentageOfPortion = new DoubleAndText();
        internal DoubleAndText SpPortionOfSauceInGrams = new DoubleAndText();
        internal DoubleAndText Mp1PortionCooked = new DoubleAndText();
        internal DoubleAndText Mp0PortionReportedToRaw = new DoubleAndText(); 
        internal DoubleAndText ChoSaucePercent = new DoubleAndText();
        internal DoubleAndText ChoMainfoodPercent = new DoubleAndText();
        internal DoubleAndText ChoTotalMainfood = new DoubleAndText();
        internal DoubleAndText ChoTotalSauce = new DoubleAndText();

        internal void CalcUnknownData()
        {
        ////////    T0RawGross.Double = T0RawGross.Double + 
        ////////        S1SauceNet.Double + T0RawTare.Double;
        ////////    M0pS1pPeRawFoodAndSauce.Double = T0RawGross.Double + S1SauceNet.Double;
        ////////    M1MainfoodCooked.Double = T1Cooked.Double - S1SauceNet.Double - T0RawTare.Double;
        ////////    M1pS1CourseCookedPlusSauce.Double = M1MainfoodCooked.Double + S1SauceNet.Double; 
        ////////    ACookRatio.Double = M1MainfoodCooked.Double / T0RawGross.Double;
        ////////    MppSpPortionOfCoursePlusSauce.Double = Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double;
        ////////    PPercPercentageOfPortion.Double = (Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double) /
        ////////        (M1MainfoodCooked.Double + S1SauceNet.Double) * 100;
        ////////    SpPortionOfSauceInGrams.Double = S1SauceNet.Double * 
        ////////        PPercPercentageOfPortion.Double / 100;
        ////////    Mp0PortionReportedToRaw.Double = Mp1PortionCooked.Double / ACookRatio.Double; 
        ////////    Mp1PortionCooked.Double = TpPortionWithAll.Double - 
        ////////        SpPortionOfSauceInGrams.Double - DiDish.Double;
        ////////    ChoTotalMainfood.Double = Mp1PortionCooked.Double * ChoMainfoodPercent.Double / 100;
        ////////    ChoTotalSauce.Double = SpPortionOfSauceInGrams.Double * ChoSaucePercent.Double / 100;
        ////////
        }
        internal void SaveData()
        {
            dl.SaveWeighFood(this); 
        }
        internal void RestoreData()
        {
            dl.RestoreWeighFood(this); 
        }
    }
}
