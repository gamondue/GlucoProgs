using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.IO;

namespace GlucoMan
{
    internal partial class DL_FlatText : DataLayer
    {
        //internal override async void SaveBolusParameters(BL_BolusCalculation Bolus)
        //{
        //    try {
        //        string file = ""; 

        //        file += Bolus.GlucoseBeforeMeal.Text + "\n";
        //        file += Bolus.TargetGlucose.Text + "\n";
        //        file += Bolus.ChoToEat.Text + "\n";

        //        await TextFile.StringToFileAsync(persistentBolusCalculation, file);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogOfProgram.Error("DL_BolusCalculation | SaveBolusCalculations", ex);
        //    }
        //}
        //internal override void RestoreBolusParameters(BL_BolusCalculation Parameters)
        //{
        //    if (File.Exists(persistentInsulinParameters))
        //    {
        //        try
        //        {
        //            string[] f = TextFile.FileToArray(persistentInsulinParameters);
        //            Parameters.ChoInsulinRatioBreakfast.Text = f[0];
        //            Parameters.ChoInsulinRatioLunch.Text = f[1];
        //            Parameters.ChoInsulinRatioDinner.Text = f[2];
        //            Parameters.TypicalBolusMorning.Text = f[3];
        //            Parameters.TypicalBolusMidday.Text = f[4];
        //            Parameters.TypicalBolusEvening.Text = f[5];
        //            Parameters.TypicalBolusNight.Text = f[6];
        //            Parameters.TotalDailyDoseOfInsulin.Text = f[7];
        //            Parameters.FactorOfInsulinCorrectionSensitivity.Text = f[8];
        //            Parameters.InsulinCorrectionSensitivity.Text = f[9];
        //        }
        //        catch (Exception ex)
        //        {
        //            Common.LogOfProgram.Error("DL_BolusCalculation | RestoreInsulinParameters", ex);
        //        }
        //    }
        //}
        //internal override async void SaveInsulinCorrectionParameters(BL_BolusCalculation Bolus)
        //{
        //    try
        //    {
        //        string file = "";

        //        file += Bolus.GlucoseBeforeMeal.Text + "\n";
        //        file += Bolus.TargetGlucose.Text + "\n";
        //        file += Bolus.ChoToEat.Text + "\n";

        //        await TextFile.StringToFileAsync(persistentBolusCalculation, file);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogOfProgram.Error("DL_BolusCalculation | SaveBolusCalculations", ex);
        //    }
        //}
        //internal override void RestoreInsulinCorrectionParameters(BL_BolusCalculation Parameters)
        //{
        //    if (File.Exists(persistentInsulinParameters))
        //    {
        //        try
        //        {
        //            string[] f = TextFile.FileToArray(persistentInsulinParameters);
        //            Parameters.ChoInsulinRatioBreakfast.Text = f[0];
        //            Parameters.ChoInsulinRatioLunch.Text = f[1];
        //            Parameters.ChoInsulinRatioDinner.Text = f[2];
        //            Parameters.TypicalBolusMorning.Text = f[3];
        //            Parameters.TypicalBolusMidday.Text = f[4];
        //            Parameters.TypicalBolusEvening.Text = f[5];
        //            Parameters.TypicalBolusNight.Text = f[6];
        //            Parameters.TotalDailyDoseOfInsulin.Text = f[7];
        //            Parameters.FactorOfInsulinCorrectionSensitivity.Text = f[8];
        //            Parameters.InsulinCorrectionSensitivity.Text = f[9];
        //        }
        //        catch (Exception ex)
        //        {
        //            Common.LogOfProgram.Error("DL_BolusCalculation | RestoreInsulinParameters", ex);
        //        }
        //    }
        //}
        internal override async void SaveLogOfBoluses(BL_BolusCalculation Bolus)
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
                // TextFile.StringToFile(logBolusCalculationsFile, fileContent, true);
                await TextFile.StringToFileAsync(logBolusCalculationsFile, fileContent);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("DL_BolusCalculation | SaveBolusCalculations", ex);
            }
        }
    }
}
 