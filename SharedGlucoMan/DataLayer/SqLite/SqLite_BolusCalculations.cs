using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Data.Common;
using System.IO;

namespace GlucoMan
{
    internal partial class DL_Sqlite : DataLayer
    {
        internal override void SaveBolusCalculations(BL_BolusCalculation Bolus)
        {
            try {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Parameters " 
                        + "(GlucoseBeforeMeal, TargetGlucose, ChoToEat)" +
                        " Values(" +
                        SqlDouble(Bolus.GlucoseBeforeMeal.Double) + "," +
                        SqlDouble(Bolus.TargetGlucose.Double) + "," +
                        SqlDouble(Bolus.ChoToEat.Double) +
                        ");";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusCalculation | SaveBolusCalculations", ex);
            }
        }
        internal override void RestoreBolusCalculations(BL_BolusCalculation Bolus)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbDataReader dRead;
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT GlucoseBeforeMeal, TargetGlucose, ChoToEat" +
                        " FROM Parameters" +
                        ";";
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Bolus.GlucoseBeforeMeal.Double = SafeRead.Double(dRead["GlucoseBeforeMeal"]);
                        Bolus.TargetGlucose.Double = SafeRead.Double(dRead["TargetGlucose"]);
                        Bolus.ChoToEat.Double = SafeRead.Double(dRead["ChoToEat"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusCalculation | SaveBolusCalculations", ex);
            }
        }
        internal override void SaveInsulinParameters(BL_BolusCalculation Bolus)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "INSERT INTO Parameters "
                        + "(ChoInsulinRatioBreakfast, ChoInsulinRatioLunch, ChoInsulinRatioDinner," +
                        "TypicalBolusMorning, TypicalBolusMidday, TypicalBolusEvening, TypicalBolusNight," +
                        "TotalDailyDoseOfInsulin, FactorOfInsulinCorrectionSensitivity, InsulinCorrectionSensitivity)" +
                        " Values(" +
                        SqlDouble(Bolus.ChoInsulinRatioBreakfast.Double) + "," +
                        SqlDouble(Bolus.ChoInsulinRatioLunch.Double) + "," +
                        SqlDouble(Bolus.ChoInsulinRatioDinner.Double) + "," +
                        SqlDouble(Bolus.TypicalBolusMorning.Double) + "," +
                        SqlDouble(Bolus.TypicalBolusMidday.Double) + "," +
                        SqlDouble(Bolus.TypicalBolusEvening.Double) + "," +
                        SqlDouble(Bolus.TypicalBolusNight.Double) + "," +
                        SqlDouble(Bolus.TotalDailyDoseOfInsulin.Double) + "," +
                        SqlDouble(Bolus.FactorOfInsulinCorrectionSensitivity.Double) + "," +
                        SqlDouble(Bolus.InsulinCorrectionSensitivity.Double) +
                        ");";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusCalculation | SaveInsulinParameters", ex);
            }
        }
        internal override void RestoreInsulinParameters(BL_BolusCalculation Bolus)
        {
            try
            {
                using (DbConnection conn = Connect())
                {
                    DbDataReader dRead;
                    DbCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT ChoInsulinRatioBreakfast,ChoInsulinRatioLunch," +
                        "ChoInsulinRatioDinner,TypicalBolusMorning,TypicalBolusMidday,TypicalBolusEvening," +
                        "TypicalBolusNight,TotalDailyDoseOfInsulin,FactorOfInsulinCorrectionSensitivity," +
                        "InsulinCorrectionSensitivity" +
                        " FROM Parameters" +
                        ";";
                    dRead = cmd.ExecuteReader();
                    while (dRead.Read())
                    {
                        Bolus.ChoInsulinRatioBreakfast.Double = SafeRead.Double(dRead["ChoInsulinRatioBreakfast"]);
                        Bolus.ChoInsulinRatioLunch.Double = SafeRead.Double(dRead["ChoInsulinRatioLunch"]);
                        Bolus.ChoInsulinRatioDinner.Double = SafeRead.Double(dRead["ChoInsulinRatioDinner"]);
                        Bolus.TypicalBolusMorning.Double = SafeRead.Double(dRead["TypicalBolusMorning"]);
                        Bolus.TypicalBolusMidday.Double = SafeRead.Double(dRead["TypicalBolusMidday"]);
                        Bolus.TypicalBolusEvening.Double = SafeRead.Double(dRead["TypicalBolusEvening"]);
                        Bolus.TypicalBolusNight.Double = SafeRead.Double(dRead["TypicalBolusNight"]);
                        Bolus.TotalDailyDoseOfInsulin.Double = SafeRead.Double(dRead["TotalDailyDoseOfInsulin"]);
                        Bolus.FactorOfInsulinCorrectionSensitivity.Double = SafeRead.Double(dRead["FactorOfInsulinCorrectionSensitivity"]);
                        Bolus.InsulinCorrectionSensitivity.Double = SafeRead.Double(dRead["InsulinCorrectionSensitivity"]);
                    }
                }
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("Sqlite_BolusCalculation | RestoreInsulinParameters", ex);
            }
        }
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
                Common.LogOfProgram.Error("Sqlite_BolusCalculation | SaveBolusCalculations", ex);
            }
        }
    }
}
 