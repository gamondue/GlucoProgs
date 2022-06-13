using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;

namespace GlucoMan
{
    internal  partial class DL_Sqlite : DataLayer
    {
        internal  override void SaveBolusParameters(BL_BolusCalculation Parameters)
        {
            SaveParameter("ChoInsulinRatioBreakfast", Parameters.ChoInsulinRatioBreakfast.Text);
            SaveParameter("ChoInsulinRatioLunch", Parameters.ChoInsulinRatioLunch.Text);
            SaveParameter("ChoInsulinRatioDinner", Parameters.ChoInsulinRatioDinner.Text);
            SaveParameter("TargetGlucose", Parameters.TargetGlucose.Text);
            SaveParameter("InsulinCorrectionSensitivity", Parameters.InsulinCorrectionSensitivity.Text);
            SaveParameter("ChoInsulinRatioBreakfast", Parameters.ChoInsulinRatioBreakfast.Text);
            SaveParameter("ChoInsulinRatioLunch", Parameters.ChoInsulinRatioLunch.Text);
            SaveParameter("ChoInsulinRatioDinner", Parameters.ChoInsulinRatioDinner.Text);
            SaveParameter("TotalDailyDoseOfInsulin", Parameters.TotalDailyDoseOfInsulin.Text);
            SaveParameter("ChoToEat", Parameters.ChoToEat.Text);
            SaveParameter("GlucoseBeforeMeal", Parameters.GlucoseBeforeMeal.Text);
        }
        internal  override void RestoreBolusParameters(BL_BolusCalculation Parameters)
        {
            Parameters.TargetGlucose.Text = RestoreParameter("TargetGlucose");
            Parameters.InsulinCorrectionSensitivity.Text = RestoreParameter("InsulinCorrectionSensitivity");
            Parameters.ChoInsulinRatioBreakfast.Text = RestoreParameter("ChoInsulinRatioBreakfast");
            Parameters.ChoInsulinRatioLunch.Text = RestoreParameter("ChoInsulinRatioLunch");
            Parameters.ChoInsulinRatioDinner.Text = RestoreParameter("ChoInsulinRatioDinner");
            Parameters.TotalDailyDoseOfInsulin.Text = RestoreParameter("TotalDailyDoseOfInsulin");
            Parameters.ChoToEat.Text = RestoreParameter("ChoToEat");
            Parameters.GlucoseBeforeMeal.Text = RestoreParameter("GlucoseBeforeMeal");
        }
        internal  override void SaveInsulinCorrectionParameters(BL_BolusCalculation Parameters)
        {
            SaveParameter("TypicalBolusMorning", Parameters.TypicalBolusMorning.Text);
            SaveParameter("TypicalBolusMidday", Parameters.TypicalBolusMidday.Text);
            SaveParameter("TypicalBolusEvening", Parameters.TypicalBolusEvening.Text);
            SaveParameter("TypicalBolusNight", Parameters.TypicalBolusNight.Text);
            SaveParameter("TotalDailyDoseOfInsulin", Parameters.TotalDailyDoseOfInsulin.Text);
            SaveParameter("FactorOfInsulinCorrectionSensitivity", Parameters.FactorOfInsulinCorrectionSensitivity.Text);
            SaveParameter("InsulinCorrectionSensitivity", Parameters.InsulinCorrectionSensitivity.Text);
        }
        internal override void RestoreInsulinCorrectionParameters(BL_BolusCalculation Parameters)
        {
            Parameters.TypicalBolusMorning.Text = Common.Bl.RestoreParameter("TypicalBolusMorning");
            Parameters.TypicalBolusMidday.Text = Common.Bl.RestoreParameter("TypicalBolusMidday");
            Parameters.TypicalBolusEvening.Text = Common.Bl.RestoreParameter("TypicalBolusEvening");
            Parameters.TypicalBolusNight.Text = Common.Bl.RestoreParameter("TypicalBolusNight");
            Parameters.TotalDailyDoseOfInsulin.Text = Common.Bl.RestoreParameter("TotalDailyDoseOfInsulin");
            Parameters.FactorOfInsulinCorrectionSensitivity.Text = Common.Bl.RestoreParameter("FactorOfInsulinCorrectionSensitivity");
            Parameters.InsulinCorrectionSensitivity.Text = Common.Bl.RestoreParameter("InsulinCorrectionSensitivity");
        }
        internal override void SaveRatioChoInsulinParameters(BL_BolusCalculation Parameters)
        {
            SaveParameter("ChoInsulinRatioBreakfast", Parameters.ChoInsulinRatioBreakfast.Text);
            SaveParameter("ChoInsulinRatioLunch", Parameters.ChoInsulinRatioLunch.Text);
            SaveParameter("ChoInsulinRatioDinner", Parameters.ChoInsulinRatioDinner.Text);
        }
        internal override void RestoreRatioChoInsulinParameters(BL_BolusCalculation Parameters)
        {
            Parameters.ChoInsulinRatioBreakfast.Text = Common.Bl.RestoreParameter("ChoInsulinRatioBreakfast");
            Parameters.ChoInsulinRatioLunch.Text = Common.Bl.RestoreParameter("ChoInsulinRatioLunch");
            Parameters.ChoInsulinRatioDinner.Text = Common.Bl.RestoreParameter("ChoInsulinRatioDinner");
        }
        internal  override async void SaveLogOfBoluses(BL_BolusCalculation NewBolusToSave)
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
 