using System;
using System.IO;
using SharedData;
using SharedFunctions;

namespace GlucoMan.BusinessLayer
{
    internal class Bl_BolusCalculation
    {
        DataLayer dl = new DataLayer();
        internal DoubleAndText ChoToEat { get; set; }
        internal DoubleAndText TypicalBolusMorning { get; set; }
        internal DoubleAndText TypicalBolusMidday { get; set; }
        internal DoubleAndText TypicalBolusEvening { get; set; }
        internal DoubleAndText TypicalBolusNight { get; set; }
        internal DoubleAndText ChoInsulineRatioMorning { get; set; }
        internal DoubleAndText ChoInsulineRatioMidday { get; set; }
        internal DoubleAndText ChoInsulineRatioEvening { get; set; }
        internal DoubleAndText ChoInsulineRatioNight { get; set; }
        internal DoubleAndText TotalDailyDoseOfInsulin { get; set; }
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
            TotalDailyDoseOfInsulin = new DoubleAndText(); 
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
                TotalDailyDoseOfInsulin.Double = TypicalBolusMorning.Double + TypicalBolusMidday.Double + TypicalBolusEvening.Double + TypicalBolusNight.Double;
                InsulineSensitivity1500.Double = 1500 / TotalDailyDoseOfInsulin.Double;
                InsulineSensitivity1800.Double = 1800 / TotalDailyDoseOfInsulin.Double;
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
                dl.SaveBolusCalculations(this);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void SaveLog()
        {
                dl.SaveLogOfBoluses(this); 
        }
        internal void RestoreData()
        {
            dl.RestoreBolusCalculations(this); 
        }
    }
}
