using System;
using System.IO;
using SharedData;
using SharedFunctions;

namespace GlucoMan.BusinessLayer
{
    internal class Bl_BolusCalculation
    {
        string persistentStorage = Path.Combine(CommonData.PathConfigurationData, @"BolusCalculation.txt");
        string logFile = Path.Combine(CommonData.PathConfigurationData, @"LogOfBolusCalculations.tsv");
        internal DoubleAndText ChoToEat { get; set; }
        internal DoubleAndText TypicalBolusMorning { get; set; }
        internal DoubleAndText TypicalBolusMidday { get; set; }
        internal DoubleAndText TypicalBolusEvening { get; set; }
        internal DoubleAndText TypicalBolusNight { get; set; }
        internal DoubleAndText ChoInsulineRatioMorning { get; set; }
        internal DoubleAndText ChoInsulineRatioMidday { get; set; }
        internal DoubleAndText ChoInsulineRatioEvening { get; set; }
        internal DoubleAndText ChoInsulineRatioNight { get; set; }
        internal DoubleAndText Tdd { get; set; }
        internal DoubleAndText GlucoseBeforeMeal { get; set; }
        internal DoubleAndText GlucoseToBeCorrected { get; set; }
        internal DoubleAndText TargetGlucose { get; set; }
        internal DoubleAndText InsulineSensitivity1500 { get; }
        internal DoubleAndText InsulineSensitivity1800 { get; }
        internal DoubleAndText CorrectionInsuline { get; set; }
        internal DoubleAndText ChoInsulineBreakfast { get; }
        internal DoubleAndText ChoInsulineLunch { get; }
        internal DoubleAndText ChoInsulineDinner { get; }
        internal DoubleAndText TotalInsulineBreakfast { get; set; }
        internal DoubleAndText TotalInsulineLunch { get; set; }
        internal DoubleAndText TotalInsulineDinner { get; set; }

        private DateTime initialBreakfastPeriod;
        private DateTime finalBreakfastPeriod;
        private DateTime initialLunchPeriod;
        private DateTime finalLunchPeriod;
        private DateTime initialDinnerPeriod;
        private DateTime finalDinnerPeriod;
        internal Bl_BolusCalculation()
        { 
            ChoToEat = new DoubleAndText(); 
            TypicalBolusMorning = new DoubleAndText(); 
            TypicalBolusMidday = new DoubleAndText(); 
            TypicalBolusEvening = new DoubleAndText(); 
            TypicalBolusNight = new DoubleAndText(); 
            ChoInsulineRatioMorning = new DoubleAndText(); 
            ChoInsulineRatioMidday = new DoubleAndText(); 
            ChoInsulineRatioEvening = new DoubleAndText(); 
            ChoInsulineRatioNight = new DoubleAndText(); 
            Tdd = new DoubleAndText(); 
            GlucoseBeforeMeal = new DoubleAndText(); 
            GlucoseToBeCorrected = new DoubleAndText(); 
            TargetGlucose = new DoubleAndText(); 
            InsulineSensitivity1500 = new DoubleAndText(); 
            InsulineSensitivity1800 = new DoubleAndText(); 
            CorrectionInsuline = new DoubleAndText(); 
            ChoInsulineBreakfast = new DoubleAndText(); 
            ChoInsulineLunch = new DoubleAndText(); 
            ChoInsulineDinner = new DoubleAndText(); 
            TotalInsulineBreakfast = new DoubleAndText(); 
            TotalInsulineLunch = new DoubleAndText(); 
            TotalInsulineDinner = new DoubleAndText();

            DateTime now = DateTime.Now;
            int y = now.Year;
            initialBreakfastPeriod = new DateTime(y, now.Month, now.Day, 6, 0, 0);
            finalBreakfastPeriod = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
            initialLunchPeriod = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
            finalLunchPeriod = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
            initialDinnerPeriod = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
            finalDinnerPeriod = new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);
        }

    internal void CalculateBolus()
        {
            try
            {
                // execute calculations
                Tdd.Double = TypicalBolusMorning.Double + TypicalBolusMidday.Double + TypicalBolusEvening.Double + TypicalBolusNight.Double;
                InsulineSensitivity1500.Double = 1500 / Tdd.Double;
                InsulineSensitivity1800.Double = 1800 / Tdd.Double;
                GlucoseToBeCorrected.Double = GlucoseBeforeMeal.Double - TargetGlucose.Double;
                CorrectionInsuline.Double = GlucoseToBeCorrected.Double / InsulineSensitivity1800.Double;

                ChoInsulineBreakfast.Double = ChoToEat.Double / ChoInsulineRatioMorning.Double;
                ChoInsulineLunch.Double = ChoToEat.Double / ChoInsulineRatioMidday.Double;
                ChoInsulineDinner.Double = ChoToEat.Double / ChoInsulineRatioEvening.Double;

                TotalInsulineBreakfast.Double = ChoInsulineBreakfast.Double + CorrectionInsuline.Double;
                TotalInsulineLunch.Double = ChoInsulineLunch.Double + CorrectionInsuline.Double;
                TotalInsulineDinner.Double = ChoInsulineDinner.Double + CorrectionInsuline.Double;
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }

        internal void RoundInsulineToZeroDecimal()
        {
            // find target bolus and relative CHO with bisection algorithm 
            
            // calc bolus to determine target bolus
            CalculateBolus();
            // target bolus will be nearest integer 
            double targetBolus = Math.Round(bolusOfMealAtThisTime());
            // calc the two CHO "Plus" and "Minus" initial values 
            double initialCho = ChoToEat.Double;
            double choPlus = initialCho + 15;
            double choMinus = initialCho - 15;
            // calc bolus for the "Plus" initial value
            ChoToEat.Double = choPlus;  // ChoToEat.Double is the CHO 
            CalculateBolus();
            double bolusPlus = bolusOfMealAtThisTime();
            // calc bolus for the "Minus" initial value
            ChoToEat.Double = choMinus;
            CalculateBolus();
            double bolusMinus = bolusOfMealAtThisTime();

            double bolusCurrent; 
            do
            {
                // next attempt CHO, in the middle of current interval 
                ChoToEat.Double = (choPlus - choMinus) / 2 + choMinus;
                CalculateBolus();
                bolusCurrent = bolusOfMealAtThisTime(); 
                if (bolusCurrent < targetBolus)
                {
                    choMinus = ChoToEat.Double;
                    bolusMinus = bolusCurrent;
                }
                else
                {
                    choPlus = ChoToEat.Double;
                    bolusPlus = bolusCurrent;
                }
            } while (Math.Abs(bolusCurrent - targetBolus) > 0.01); 
        }

        private double bolusOfMealAtThisTime()
        {
            double b;
            DateTime now = DateTime.Now;
            if (now > initialBreakfastPeriod && now < finalBreakfastPeriod)
            {
                b = TotalInsulineBreakfast.Double; 
            }
            else if (now > initialLunchPeriod && now < finalLunchPeriod)
            {
                b = TotalInsulineLunch.Double;
            }
            else if (now > initialDinnerPeriod && now < finalDinnerPeriod)
            {
                b = TotalInsulineDinner.Double;
            }
            else
            {
                // outside any meal period uses lunch
                b = TotalInsulineLunch.Double; 
            }
            return b;
        }

        internal void SaveData()
        {
            try
            {
                string file = ChoInsulineRatioMorning.Text + "\n";
                file += ChoInsulineRatioMidday.Text + "\n";
                file += ChoInsulineRatioEvening.Text + "\n";
                file += TypicalBolusMorning.Text + "\n";
                file += TypicalBolusMidday.Text + "\n";
                file += TypicalBolusEvening.Text + "\n";
                file += TypicalBolusNight.Text + "\n";
                file += GlucoseBeforeMeal.Text + "\n";
                file += TargetGlucose.Text + "\n";
                file += ChoToEat.Text + "\n";
                TextFile.StringToFile(persistentStorage, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void SaveLog()
        {
            try
            {
                string fileContent;
                // create header of log file if it doesn't exist 
                if (!System.IO.File.Exists(logFile))
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
                fileContent += ChoInsulineRatioMorning.Text + "\t";
                fileContent += ChoInsulineRatioMidday.Text + "\t";
                fileContent += ChoInsulineRatioEvening.Text + "\t";
                fileContent += TypicalBolusMorning.Text + "\t";
                fileContent += TypicalBolusMidday.Text + "\t";
                fileContent += TypicalBolusEvening.Text + "\t";
                fileContent += TypicalBolusNight.Text + "\t";
                fileContent += GlucoseBeforeMeal.Text + "\t";
                fileContent += TargetGlucose.Text + "\t";
                fileContent += ChoToEat.Text + "\t";
                fileContent += CorrectionInsuline.Text + "\t";
                fileContent += TotalInsulineBreakfast.Text + "\t";
                fileContent += TotalInsulineLunch.Text + "\t";
                fileContent += TotalInsulineDinner.Text + "\t";
                fileContent += "\r\n";
                TextFile.StringToFile(logFile, fileContent, true);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void RestoreData()
        {
            if (File.Exists(persistentStorage))
                try
                {
                    string[] f = TextFile.FileToArray(persistentStorage);
                    ChoInsulineRatioMorning.Text = f[0];
                    ChoInsulineRatioMidday.Text = f[1];
                    ChoInsulineRatioEvening.Text = f[2];
                    TypicalBolusMorning.Text = f[3];
                    TypicalBolusMidday.Text = f[4];
                    TypicalBolusEvening.Text = f[5];
                    TypicalBolusNight.Text = f[6];
                    GlucoseBeforeMeal.Text = f[7];
                    TargetGlucose.Text = f[8];
                    ChoToEat.Text = f[9];
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
        }
    }
}
