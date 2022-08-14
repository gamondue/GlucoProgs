using gamon; 
using System;
using System.Drawing;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    public class BL_WeighFood
    {
        DataLayer dl = Common.Database;

        Color inputColor = Color.PaleGreen;
        Color inputSlowChangingColor = Color.PaleGoldenrod;
        Color toBeinputColor = Color.Red;
        Color calculatedImportantColor = Color.Yellow;
        Color calculatedNonImportantColor = Color.White;
        Color programsResult = Color.SkyBlue;

        public  DoubleAndText M0RawGross = new DoubleAndText();
        public  DoubleAndText M0RawTare = new DoubleAndText();
        public  DoubleAndText M0RawNet = new DoubleAndText();

        public  DoubleAndText S1SauceGross = new DoubleAndText();
        public  DoubleAndText S1SauceTare = new DoubleAndText();
        public  DoubleAndText S1SauceNet = new DoubleAndText();

        public  DoubleAndText T0RawGross = new DoubleAndText();
        public  DoubleAndText T0RawTare = new DoubleAndText();
        public  DoubleAndText T0RawNet = new DoubleAndText();
        public  DoubleAndText T0SaucePlusTare = new DoubleAndText();

        public  DoubleAndText S1pPotSaucePlusPot = new DoubleAndText(); // eliminare !!
        public  DoubleAndText DiDish = new DoubleAndText();
        public  DoubleAndText T1CookedGross = new DoubleAndText();
        public  DoubleAndText T1CookedTare = new DoubleAndText();
        public  DoubleAndText T1CookedNet = new DoubleAndText();
        public  DoubleAndText TpPortionWithAll = new DoubleAndText();
        public  DoubleAndText M0pS1pPeRawFoodAndSauce = new DoubleAndText();
        public  DoubleAndText M1MainfoodCooked = new DoubleAndText();
        public  DoubleAndText M1pS1CourseCookedPlusSauce = new DoubleAndText();
        public  DoubleAndText ACookRatio = new DoubleAndText();
        public  DoubleAndText MppSpPortionOfCoursePlusSauce = new DoubleAndText();
        public  DoubleAndText PPercPercentafPortion = new DoubleAndText();
        public  DoubleAndText SpPortionOfSauceInGrams = new DoubleAndText();
        public  DoubleAndText Mp1PortionCooked = new DoubleAndText();
        public  DoubleAndText Mp0PortionReportedToRaw = new DoubleAndText(); 
        public  DoubleAndText ChoSaucePercent = new DoubleAndText();
        public  DoubleAndText ChoMainfoodPercent = new DoubleAndText();
        public  DoubleAndText ChoTotalMainfood = new DoubleAndText();
        public  DoubleAndText ChoTotalSauce = new DoubleAndText();

        public  void CalcUnknownData()
        {
        ////////    T0RawGross.Double = T0RawGross.Double + 
        ////////        S1SauceNet.Double + T0RawTare.Double;
        ////////    M0pS1pPeRawFoodAndSauce.Double = T0RawGross.Double + S1SauceNet.Double;
        ////////    M1MainfoodCooked.Double = T1Cooked.Double - S1SauceNet.Double - T0RawTare.Double;
        ////////    M1pS1CourseCookedPlusSauce.Double = M1MainfoodCooked.Double + S1SauceNet.Double; 
        ////////    ACookRatio.Double = M1MainfoodCooked.Double / T0RawGross.Double;
        ////////    MppSpPortionOfCoursePlusSauce.Double = Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double;
        ////////    PPercpercentOfPortion.Double = (Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double) /
        ////////        (M1MainfoodCooked.Double + S1SauceNet.Double) * 100;
        ////////    SpPortionOfSauceInGrams.Double = S1SauceNet.Double * 
        ////////        PPercpercentOfPortion.Double / 100;
        ////////    Mp0PortionReportedToRaw.Double = Mp1PortionCooked.Double / ACookRatio.Double; 
        ////////    Mp1PortionCooked.Double = TpPortionWithAll.Double - 
        ////////        SpPortionOfSauceInGrams.Double - DiDish.Double;
        ////////    ChoTotalMainfood.Double = Mp1PortionCooked.Double * ChoMainfoodPercent.Double / 100;
        ////////    ChoTotalSauce.Double = SpPortionOfSauceInGrams.Double * ChoSaucePercent.Double / 100;
        ////////
        }
        public  void SaveData()
        {
            ////////dl.SaveWeighFood(this); 
        }
        public  void RestoreData()
        {
            ////////dl.RestoreWeighFood(this); 
        }
    }
}
