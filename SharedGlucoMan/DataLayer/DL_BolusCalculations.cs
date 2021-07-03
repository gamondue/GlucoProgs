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
        internal void SaveBolusCalculations(BL_BolusCalculation Bolus)
        {
            try {
                string file = Bolus.ChoInsulineRatioBreakfast.Text + "\n";
                file += Bolus.ChoInsulineRatioLunch.Text + "\n";
                file += Bolus.ChoInsulineRatioDinner.Text + "\n"; 
                file += Bolus.TypicalBolusMorning.Text + "\n";
                file += Bolus.TypicalBolusMidday.Text + "\n";
                file += Bolus.TypicalBolusEvening.Text + "\n";
                file += Bolus.TypicalBolusNight.Text + "\n";
                file += Bolus.GlucoseBeforeMeal.Text + "\n";
                file += Bolus.TargetGlucose.Text + "\n";
                file += Bolus.ChoToEat.Text + "\n";
                file += Bolus.InsulineSensitivityFactor.Text + "\n";

                TextFile.StringToFile(persistentBolusCalculation, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void RestoreBolusCalculations(BL_BolusCalculation Bolus)
        {
            if (File.Exists(persistentBolusCalculation))
                try
                {
                    string[] f = TextFile.FileToArray(persistentBolusCalculation);
                    Bolus.ChoInsulineRatioBreakfast.Text = f[0];
                    Bolus.ChoInsulineRatioLunch.Text = f[1];
                    Bolus.ChoInsulineRatioDinner.Text = f[2];
                    Bolus.TypicalBolusMorning.Text = f[3];
                    Bolus.TypicalBolusMidday.Text = f[4];
                    Bolus.TypicalBolusEvening.Text = f[5];
                    Bolus.TypicalBolusNight.Text = f[6];
                    Bolus.GlucoseBeforeMeal.Text = f[7];
                    Bolus.TargetGlucose.Text = f[8];
                    Bolus.ChoToEat.Text = f[9];
                    Bolus.InsulineSensitivityFactor.Text = f[10]; ;
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
        }
        internal void SaveLogOfBoluses(BL_BolusCalculation Bolus)
        {
            string fileContent;
            try
            { 
                // create header of log file if it doesn't exist 
                if (!System.IO.File.Exists(logBolusCalculationsFile))
                {
                    fileContent = "Timestamp" +
                        "\tCHO/Insuline Ratio at breakfast\tCHO/Insuline Ratio at lunch\tCHO/Insuline Ratio at dinner" +
                        "\tTypical bolus in the morning\tTypical bolus at midday\tTypical bolus at evening\tTypical bolus at night" +
                        "\tInsuline sensitivity factor used" +
                        "\tMeasured glucose before meal\tTarget glucose\tCHO to eat at meal" +
                        "\tCorrection insuline due to glucose difference from target\tCorrection insuline due to carbohydrates in meal" +
                        "\tTotal bolus of insuline injected";
                    fileContent += "\r\n";
                }
                else
                    fileContent = "";

                fileContent += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t";
                fileContent += Bolus.ChoInsulineRatioBreakfast.Text + "\t";
                fileContent += Bolus.ChoInsulineRatioLunch.Text + "\t";
                fileContent += Bolus.ChoInsulineRatioDinner.Text + "\t";
                fileContent += Bolus.TypicalBolusMorning.Text + "\t";
                fileContent += Bolus.TypicalBolusMidday.Text + "\t";
                fileContent += Bolus.TypicalBolusEvening.Text + "\t";
                fileContent += Bolus.TypicalBolusNight.Text + "\t";
                fileContent += Bolus.InsulineSensitivityFactor.Text + "\t";
                fileContent += Bolus.GlucoseBeforeMeal.Text + "\t";
                fileContent += Bolus.TargetGlucose.Text + "\t";
                fileContent += Bolus.ChoToEat.Text + "\t";
                fileContent += Bolus.BolusInsulineDueToCorrectionOfGlucose.Text + "\t";
                fileContent += Bolus.BolusInsulineDueToChoOfMeal.Text + "\t";
                fileContent += Bolus.TotalInsulineForMeal.Text + "\t";
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
