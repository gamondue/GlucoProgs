using GlucoMan;
using SharedData;
using System;
using System.Drawing;
using System.IO;

namespace SharedGlucoMan.BusinessLayer
{
    class WeighFood
    {
        string persistentStorage = CommonData.PathConfigurationData + @"WeighFood.txt";

        Color inputColor = Color.PaleGreen;
        Color inputSlowChangingColor = Color.PaleGoldenrod;
        Color toBeinputColor = Color.Red;
        Color calculatedImportantColor = Color.Yellow;
        Color calculatedNonImportantColor = Color.White;
        Color programsResult = Color.SkyBlue;

        internal DoubleAndText M0MainRawGross = new DoubleAndText();
        internal DoubleAndText M0MainRawTare = new DoubleAndText();
        internal DoubleAndText M0MainRawNet = new DoubleAndText();
        internal DoubleAndText PotCookingPot = new DoubleAndText();
        internal DoubleAndText S1SauceGross = new DoubleAndText();
        internal DoubleAndText S1SauceTare = new DoubleAndText();
        internal DoubleAndText S1SauceNet = new DoubleAndText();
        internal DoubleAndText S1pPotSaucePlusPot = new DoubleAndText(); // eliminare !!
        internal DoubleAndText DiDish = new DoubleAndText();
        internal DoubleAndText T1AllCooked = new DoubleAndText();
        internal DoubleAndText TpPortionWithAll = new DoubleAndText();
        internal DoubleAndText T0AllPreCooking = new DoubleAndText();
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
            T0AllPreCooking.Double = M0MainRawGross.Double + 
                S1SauceNet.Double + PotCookingPot.Double;
            M0pS1pPeRawFoodAndSauce.Double = M0MainRawGross.Double + S1SauceNet.Double;
            M1MainfoodCooked.Double = T1AllCooked.Double - S1SauceNet.Double - PotCookingPot.Double;
            M1pS1CourseCookedPlusSauce.Double = M1MainfoodCooked.Double + S1SauceNet.Double; 
            ACookRatio.Double = M1MainfoodCooked.Double / M0MainRawGross.Double;
            MppSpPortionOfCoursePlusSauce.Double = Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double;
            PPercPercentageOfPortion.Double = (Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double) /
                (M1MainfoodCooked.Double + S1SauceNet.Double) * 100;
            SpPortionOfSauceInGrams.Double = S1SauceNet.Double * 
                PPercPercentageOfPortion.Double / 100;
            Mp0PortionReportedToRaw.Double = Mp1PortionCooked.Double / ACookRatio.Double; 
            Mp1PortionCooked.Double = TpPortionWithAll.Double - 
                SpPortionOfSauceInGrams.Double - DiDish.Double;
            ChoTotalMainfood.Double = Mp1PortionCooked.Double * ChoMainfoodPercent.Double / 100;
            ChoTotalSauce.Double = SpPortionOfSauceInGrams.Double * ChoSaucePercent.Double / 100;
        }

        internal void SaveData()
        {
            try
            {
                string file = M0MainRawGross.Text + "\n";
                file += PotCookingPot.Text + "\n";
                file += T0AllPreCooking.Text + "\n";
                file += S1SauceNet.Text + "\n";
                file += DiDish.Text + "\n";
                file += TpPortionWithAll.Text + "\n";
                file += M0pS1pPeRawFoodAndSauce.Text + "\n";
                file += M1MainfoodCooked.Text + "\n";
                file += M1pS1CourseCookedPlusSauce.Text + "\n";
                file += ACookRatio.Text + "\n";
                file += MppSpPortionOfCoursePlusSauce.Text + "\n";
                file += PPercPercentageOfPortion.Text + "\n";
                file += SpPortionOfSauceInGrams.Text + "\n";
                file += Mp0PortionReportedToRaw.Text + "\n";
                file += Mp1PortionCooked.Text + "\n";
                file += ChoTotalMainfood.Text + "\n";
                file += ChoSaucePercent.Text + "\n";
                file += ChoMainfoodPercent.Text + "\n";
                file += ChoTotalMainfood.Text + "\n";
                file += ChoTotalSauce.Text + "\n";
                file += S1pPotSaucePlusPot.Text + "\n";

                TextFile.StringToFile(persistentStorage, file, false);
            }
            catch (Exception ex)
            {
                Console.Beep();
            }
        }
        internal WeighFood RestoreData()
        {
            WeighFood w = null;
            if (File.Exists(persistentStorage))
                try
                {
                    string[] f = TextFile.FileToArray(persistentStorage);
                    M0MainRawGross.Text = f[0];
                    PotCookingPot.Text = f[1];
                    T0AllPreCooking.Text = f[2];
                    S1SauceNet.Text = f[3];
                    DiDish.Text = f[4];
                    TpPortionWithAll.Text = f[5];
                    M0pS1pPeRawFoodAndSauce.Text = f[6];
                    M1MainfoodCooked.Text = f[7];
                    M1pS1CourseCookedPlusSauce.Text = f[8];
                    ACookRatio.Text = f[9];
                    MppSpPortionOfCoursePlusSauce.Text = f[10];
                    PPercPercentageOfPortion.Text = f[11];
                    SpPortionOfSauceInGrams.Text = f[12];
                    Mp0PortionReportedToRaw.Text = f[13];
                    Mp1PortionCooked.Text = f[14];
                    ChoTotalMainfood.Text = f[15];
                    ChoSaucePercent.Text = f[16];
                    ChoMainfoodPercent.Text = f[17];
                    ChoTotalMainfood.Text = f[18];
                    ChoTotalSauce.Text = f[19];
                    S1pPotSaucePlusPot.Text = f[20];
                    
                }
                catch (Exception ex)
                {
                    Console.Beep();
                }
            return w;
        }
    }
}
