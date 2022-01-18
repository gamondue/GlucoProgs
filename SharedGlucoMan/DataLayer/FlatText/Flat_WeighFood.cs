using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public partial class DL_FlatText : DataLayer
    {
        public  override void RestoreWeighFood(GlucoMan.BusinessLayer.BL_WeighFood WeighFood)
        {
            if (File.Exists(persistentWeighFood))
                try
                {
                    string[] f = TextFile.FileToArray(persistentWeighFood);
                    WeighFood.T0RawGross.Text = f[0];
                    WeighFood.T0RawTare.Text = f[1];
                    WeighFood.T0RawGross.Text = f[2];
                    WeighFood.S1SauceNet.Text = f[3];
                    WeighFood.DiDish.Text = f[4];
                    WeighFood.TpPortionWithAll.Text = f[5];
                    WeighFood.M0pS1pPeRawFoodAndSauce.Text = f[6];
                    WeighFood.M1MainfoodCooked.Text = f[7];
                    WeighFood.M1pS1CourseCookedPlusSauce.Text = f[8];
                    WeighFood.ACookRatio.Text = f[9];
                    WeighFood.MppSpPortionOfCoursePlusSauce.Text = f[10];
                    WeighFood.PPercPercentafPortion.Text = f[11];
                    WeighFood.SpPortionOfSauceInGrams.Text = f[12];
                    WeighFood.Mp0PortionReportedToRaw.Text = f[13];
                    WeighFood.Mp1PortionCooked.Text = f[14];
                    WeighFood.ChoTotalMainfood.Text = f[15];
                    WeighFood.ChoSaucePercent.Text = f[16];
                    WeighFood.ChoMainfoodPercent.Text = f[17];
                    WeighFood.ChoTotalMainfood.Text = f[18];
                    WeighFood.ChoTotalSauce.Text = f[19];
                    WeighFood.S1pPotSaucePlusPot.Text = f[20];

                }
                catch (Exception ex)
                {
                    Common.LogOfProgram.Error("DL_WeighFood | RestoreWeighFood()", ex);
                }
        }
        public  override void SaveWeighFood(GlucoMan.BusinessLayer.BL_WeighFood WeighFood)
        {
            try
            {
                string file = WeighFood.T0RawGross.Text + "\n";
                file += WeighFood.T0RawTare.Text + "\n";
                file += WeighFood.T0RawGross.Text + "\n";
                file += WeighFood.S1SauceNet.Text + "\n";
                file += WeighFood.DiDish.Text + "\n";
                file += WeighFood.TpPortionWithAll.Text + "\n";
                file += WeighFood.M0pS1pPeRawFoodAndSauce.Text + "\n";
                file += WeighFood.M1MainfoodCooked.Text + "\n";
                file += WeighFood.M1pS1CourseCookedPlusSauce.Text + "\n";
                file += WeighFood.ACookRatio.Text + "\n";
                file += WeighFood.MppSpPortionOfCoursePlusSauce.Text + "\n";
                file += WeighFood.PPercPercentafPortion.Text + "\n";
                file += WeighFood.SpPortionOfSauceInGrams.Text + "\n";
                file += WeighFood.Mp0PortionReportedToRaw.Text + "\n";
                file += WeighFood.Mp1PortionCooked.Text + "\n";
                file += WeighFood.ChoTotalMainfood.Text + "\n";
                file += WeighFood.ChoSaucePercent.Text + "\n";
                file += WeighFood.ChoMainfoodPercent.Text + "\n";
                file += WeighFood.ChoTotalMainfood.Text + "\n";
                file += WeighFood.ChoTotalSauce.Text + "\n";
                file += WeighFood.S1pPotSaucePlusPot.Text + "\n";

                TextFile.StringToFile(persistentWeighFood, file, false);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("DL_WeighFood | SaveWeighFood()", ex);
            }
        }
    }
}
