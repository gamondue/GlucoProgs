using GlucoMan;
using SharedData;
using SharedFunctions;
using System;
using System.Drawing;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    public class BL_WeighFood
    {
        Sqlite_DataLayer dl = new GlucoMan.Sqlite_DataLayer();

        Color inputColor = Color.PaleGreen;
        Color inputSlowChangingColor = Color.PaleGoldenrod;
        Color toBeinputColor = Color.Red;
        Color calculatedImportantColor = Color.Yellow;
        Color calculatedNonImportantColor = Color.White;
        Color programsResult = Color.SkyBlue;

        WeighData weighData; 

        public void CalcUnknownData()
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
        public void SaveData()
        {
            dl.SaveWeighFood(weighData);
        }
        public void RestoreData()
        {
            dl.RestoreWeighFood(weighData); 
        }
    }
}
