using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override async void SaveLogOfBoluses(BL_BolusCalculation NewBolusToSave)
        {
            // log is savedd in text file 
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
                fileContent += NewBolusToSave.ChoInsulinRatioBreakfast.Text + "\t";
                fileContent += NewBolusToSave.ChoInsulinRatioLunch.Text + "\t";
                fileContent += NewBolusToSave.ChoInsulinRatioDinner.Text + "\t";
                fileContent += NewBolusToSave.TypicalBolusMorning.Text + "\t";
                fileContent += NewBolusToSave.TypicalBolusMidday.Text + "\t";
                fileContent += NewBolusToSave.TypicalBolusEvening.Text + "\t";
                fileContent += NewBolusToSave.TypicalBolusNight.Text + "\t";
                fileContent += NewBolusToSave.FactorOfInsulinCorrectionSensitivity.Text + "\t";
                fileContent += NewBolusToSave.GlucoseBeforeMeal.Text + "\t";
                fileContent += NewBolusToSave.TargetGlucose.Text + "\t";
                fileContent += NewBolusToSave.ChoToEat.Text + "\t";
                fileContent += NewBolusToSave.BolusInsulinDueToCorrectionOfGlucose.Text + "\t";
                fileContent += NewBolusToSave.BolusInsulinDueToChoOfMeal.Text + "\t";
                fileContent += NewBolusToSave.TotalInsulinForMeal.Text + "\t";
                fileContent += "\r\n";
                // TextFile.StringToFile(logBolusCalculationsFile, fileContent, true);
                await TextFile.StringToFileAsync(logBolusCalculationsFile, fileContent);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusCalculation | SaveBolusCalculations", ex);
            }
        }
    }
}
 