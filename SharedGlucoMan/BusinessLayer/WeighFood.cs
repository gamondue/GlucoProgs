using GlucoMan;
using SharedData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

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

        internal DoubleAndText M0AllRawMainFood = new DoubleAndText();
        internal DoubleAndText PotCookingPot = new DoubleAndText();
        internal DoubleAndText S1AllSauce = new DoubleAndText();
        internal DoubleAndText S1pPotSaucePlusPot = new DoubleAndText();
        internal DoubleAndText DiDish = new DoubleAndText();
        internal DoubleAndText T1AllCooked = new DoubleAndText();
        internal DoubleAndText TpPortionWithAll = new DoubleAndText();
        internal DoubleAndText T0AllPreCooking = new DoubleAndText();
        internal DoubleAndText M0pS1pPeRawFoodAndSauce = new DoubleAndText();
        internal DoubleAndText M1CourseCooked = new DoubleAndText();
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
            T0AllPreCooking.Double = M0AllRawMainFood.Double + 
                S1AllSauce.Double + PotCookingPot.Double;
            M0pS1pPeRawFoodAndSauce.Double = M0AllRawMainFood.Double + S1AllSauce.Double;
            M1CourseCooked.Double = T1AllCooked.Double - S1AllSauce.Double - PotCookingPot.Double;
            M1pS1CourseCookedPlusSauce.Double = M1CourseCooked.Double + S1AllSauce.Double; 
            ACookRatio.Double = M1CourseCooked.Double / M0AllRawMainFood.Double;
            MppSpPortionOfCoursePlusSauce.Double = Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double;
            PPercPercentageOfPortion.Double = (Mp1PortionCooked.Double + SpPortionOfSauceInGrams.Double) /
                (M1CourseCooked.Double + S1AllSauce.Double) * 100;
            SpPortionOfSauceInGrams.Double = S1AllSauce.Double * 
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
                string file = T0AllPreCooking.Text + "\n";
                file += M0pS1pPeRawFoodAndSauce.Text + "\n";
                file += M1CourseCooked.Text + "\n";
                file += M1pS1CourseCookedPlusSauce.Text + "\n";
                file += ACookRatio.Text + "\n";
                file += MppSpPortionOfCoursePlusSauce.Text + "\n";
                file += PPercPercentageOfPortion.Text + "\n";
                file += SpPortionOfSauceInGrams.Text + "\n";
                file += Mp0PortionReportedToRaw.Text + "\n";
                file += Mp1PortionCooked.Text + "\n";
                file += ChoTotalMainfood.Text + "\n";
                file += ChoTotalSauce.Text;

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
                    T0AllPreCooking.Text = f[0];
                    M0pS1pPeRawFoodAndSauce.Text = f[1];
                    M1CourseCooked.Text = f[2];
                    M1pS1CourseCookedPlusSauce.Text = f[3];
                    ACookRatio.Text = f[4];
                    MppSpPortionOfCoursePlusSauce.Text = f[5];
                    PPercPercentageOfPortion.Text = f[6];
                    SpPortionOfSauceInGrams.Text = f[7];
                    Mp0PortionReportedToRaw.Text = f[8];
                    Mp1PortionCooked.Text = f[9];
                    ChoTotalMainfood.Text = f[10];
                    ChoTotalSauce.Text = f[11];
                }
                catch (Exception ex)
                {
                    Console.Beep();
                }
            return w;
        }
    }
}
