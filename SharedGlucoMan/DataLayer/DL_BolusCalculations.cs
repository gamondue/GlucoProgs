using GlucoMan.BusinessLayer;
using SharedData;
using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DataLayer
    {
        internal void SaveBolusCalculations(Bl_BolusCalculation Bolus)
        {
            try { 
                string file = Bolus.ChoInsulineRatioMorning.Text + "\n";
                file += Bolus.ChoInsulineRatioMidday.Text + "\n";
                file += Bolus.ChoInsulineRatioEvening.Text + "\n";
                file += Bolus.TypicalBolusMorning.Text + "\n";
                file += Bolus.TypicalBolusMidday.Text + "\n";
                file += Bolus.TypicalBolusEvening.Text + "\n";
                file += Bolus.TypicalBolusNight.Text + "\n";
                file += Bolus.GlucoseBeforeMeal.Text + "\n";
                file += Bolus.TargetGlucose.Text + "\n";
                file += Bolus.ChoToEat.Text + "\n";

                TextFile.StringToFile(persistentBolusCalculation, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }

        internal void RestoreBolusCalculations(Bl_BolusCalculation Bolus)
        {
            if (File.Exists(persistentBolusCalculation))
                try
                {
                    string[] f = TextFile.FileToArray(persistentBolusCalculation);
                    Bolus.ChoInsulineRatioMorning.Text = f[0];
                    Bolus.ChoInsulineRatioMidday.Text = f[1];
                    Bolus.ChoInsulineRatioEvening.Text = f[2];
                    Bolus.TypicalBolusMorning.Text = f[3];
                    Bolus.TypicalBolusMidday.Text = f[4];
                    Bolus.TypicalBolusEvening.Text = f[5];
                    Bolus.TypicalBolusNight.Text = f[6];
                    Bolus.GlucoseBeforeMeal.Text = f[7];
                    Bolus.TargetGlucose.Text = f[8];
                    Bolus.ChoToEat.Text = f[9];
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
        }
        internal void SaveLogOfBoluses(Bl_BolusCalculation Bolus)
        {
            string fileContent;
            try
            { 
                // create header of log file if it doesn't exist 
                if (!System.IO.File.Exists(logBolusCalculationsFile))
                {
                    fileContent = "Timestamp" +
                        "\tCHO/Insuline Ratio in the Morning\tCHO/Insuline Ratio at midday\tCHO/Insuline Ratio at evening" +
                        "\tTypical bolus in the morning\tTypical bolus at midday\tTypical bolus at evening\tTypical bolus at night" +
                        "\tMeasured glucose before meal\tTarget glucose\tCHO to eat at meal" +
                        "\tCorrection insuline due to glucose difference from target\tBolus of insuline to inject if Breakfast" +
                        "\tBolus of insuline to inject if Lunch\tBolus of insuline to inject if Dinner";
                    fileContent += "\r\n";
                }
                else
                    fileContent = "";

                fileContent += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t";
                fileContent += Bolus.ChoInsulineRatioMorning.Text + "\t";
                fileContent += Bolus.ChoInsulineRatioMidday.Text + "\t";
                fileContent += Bolus.ChoInsulineRatioEvening.Text + "\t";
                fileContent += Bolus.TypicalBolusMorning.Text + "\t";
                fileContent += Bolus.TypicalBolusMidday.Text + "\t";
                fileContent += Bolus.TypicalBolusEvening.Text + "\t";
                fileContent += Bolus.TypicalBolusNight.Text + "\t";
                fileContent += Bolus.GlucoseBeforeMeal.Text + "\t";
                fileContent += Bolus.TargetGlucose.Text + "\t";
                fileContent += Bolus.ChoToEat.Text + "\t";
                fileContent += Bolus.CorrectionInsuline.Text + "\t";
                fileContent += Bolus.TotalInsulineBreakfast.Text + "\t";
                fileContent += Bolus.TotalInsulineLunch.Text + "\t";
                fileContent += Bolus.TotalInsulineDinner.Text + "\t";
                fileContent += "\r\n";
                TextFile.StringToFile(logBolusCalculationsFile, fileContent, true);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
    }
}
