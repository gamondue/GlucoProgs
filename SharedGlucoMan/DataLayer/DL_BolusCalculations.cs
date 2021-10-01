using GlucoMan.BusinessLayer;
using SharedData;
using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal abstract partial class DataLayer
    {
        internal void SaveBolusCalculations(BL_BolusCalculation Bolus)
        {
            try {
                if (!System.IO.File.Exists(persistentBolusCalculation))
                {
                    System.IO.File.Create(persistentBolusCalculation); 
                }
                string file = Bolus.ChoInsulinRatioBreakfast.Text + "\n";
                file += Bolus.ChoInsulinRatioLunch.Text + "\n";
                file += Bolus.ChoInsulinRatioDinner.Text + "\n"; 
                file += Bolus.TypicalBolusMorning.Text + "\n";
                file += Bolus.TypicalBolusMidday.Text + "\n";
                file += Bolus.TypicalBolusEvening.Text + "\n";
                file += Bolus.TypicalBolusNight.Text + "\n";
                file += Bolus.GlucoseBeforeMeal.Text + "\n";
                file += Bolus.TargetGlucose.Text + "\n";
                file += Bolus.ChoToEat.Text + "\n";
                file += Bolus.InsulinCorrectionSensitivity.Text + "\n";
                file += Bolus.FactorOfInsulinCorrectionSensitivity.Text + "\n";
                file += Bolus.TotalDailyDoseOfInsulin.Text + "\n";

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
                    Bolus.ChoInsulinRatioBreakfast.Text = f[0];
                    Bolus.ChoInsulinRatioLunch.Text = f[1];
                    Bolus.ChoInsulinRatioDinner.Text = f[2];
                    Bolus.TypicalBolusMorning.Text = f[3];
                    Bolus.TypicalBolusMidday.Text = f[4];
                    Bolus.TypicalBolusEvening.Text = f[5];
                    Bolus.TypicalBolusNight.Text = f[6];
                    Bolus.GlucoseBeforeMeal.Text = f[7];
                    Bolus.TargetGlucose.Text = f[8];
                    Bolus.ChoToEat.Text = f[9];
                    Bolus.InsulinCorrectionSensitivity.Text = f[10];
                    Bolus.FactorOfInsulinCorrectionSensitivity.Text = f[11];
                    Bolus.TotalDailyDoseOfInsulin.Text = f[12];
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
                        "\tCHO/Insulin Ratio at breakfast\tCHO/Insulin Ratio at lunch\tCHO/Insulin Ratio at dinner" +
                        "\tTypical bolus in the morning\tTypical bolus at midday\tTypical bolus at evening\tTypical bolus at night" +
                        "\tInsulin sensitivity factor used" +
                        "\tMeasured glucose before meal\tTarget glucose\tCHO to eat at meal" +
                        "\tCorrection Insulin due to glucose difference from target\tCorrection Insulin due to carbohydrates in meal" +
                        "\tTotal bolus of Insulin injected";
                    fileContent += "\r\n";
                }
                else
                    fileContent = "";

                fileContent += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t";
                fileContent += Bolus.ChoInsulinRatioBreakfast.Text + "\t";
                fileContent += Bolus.ChoInsulinRatioLunch.Text + "\t";
                fileContent += Bolus.ChoInsulinRatioDinner.Text + "\t";
                fileContent += Bolus.TypicalBolusMorning.Text + "\t";
                fileContent += Bolus.TypicalBolusMidday.Text + "\t";
                fileContent += Bolus.TypicalBolusEvening.Text + "\t";
                fileContent += Bolus.TypicalBolusNight.Text + "\t";
                fileContent += Bolus.FactorOfInsulinCorrectionSensitivity.Text + "\t";
                fileContent += Bolus.GlucoseBeforeMeal.Text + "\t";
                fileContent += Bolus.TargetGlucose.Text + "\t";
                fileContent += Bolus.ChoToEat.Text + "\t";
                fileContent += Bolus.BolusInsulinDueToCorrectionOfGlucose.Text + "\t";
                fileContent += Bolus.BolusInsulinDueToChoOfMeal.Text + "\t";
                fileContent += Bolus.TotalInsulinForMeal.Text + "\t";
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
