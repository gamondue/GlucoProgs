using System;
using System.IO;
using SharedData;
using SharedFunctions;

namespace GlucoMan.BusinessLayer
{
    internal class BL_BolusCalculation
    {
        DataLayer dl = new DataLayer();
        private string statusMessage;
        internal DoubleAndText ChoToEat { get; set; }
        internal DoubleAndText TypicalBolusMorning { get; set; }
        internal DoubleAndText TypicalBolusMidday { get; set; }
        internal DoubleAndText TypicalBolusEvening { get; set; }
        internal DoubleAndText TypicalBolusNight { get; set; }
        internal DoubleAndText ChoInsulinRatioLunch { get; set; }
        internal DoubleAndText ChoInsulinRatioDinner { get; set; }
        internal DoubleAndText ChoInsulinRatioBreakfast { get; set; }
        internal DoubleAndText TotalDailyDoseOfInsulin { get; set; }
        internal DoubleAndText GlucoseBeforeMeal { get; set; }
        internal DoubleAndText GlucoseToBeCorrected { get; set; }
        internal DoubleAndText TargetGlucose { get; set; }
        internal DoubleAndText FactorOfInsulinCorrectionSensitivity { get; set; }
        internal DoubleAndText InsulinCorrectionSensitivity { get; }
        internal DoubleAndText BolusInsulinDueToCorrectionOfGlucose { get; set; }
        internal DoubleAndText BolusInsulinDueToChoOfMeal { get; }
        internal DoubleAndText TotalInsulinForMeal { get; set; }
        internal string StatusMessage { get => statusMessage; }
        internal Meal MealOfBolus { get; set; }

        private DateTime initialBreakfastPeriod;
        private DateTime finalBreakfastPeriod;
        private DateTime initialLunchPeriod;
        private DateTime finalLunchPeriod;
        private DateTime initialDinnerPeriod;
        private DateTime finalDinnerPeriod;
        internal BL_BolusCalculation()
        { 
            ChoToEat = new DoubleAndText(); 
            TypicalBolusMorning = new DoubleAndText(); 
            TypicalBolusMidday = new DoubleAndText(); 
            TypicalBolusEvening = new DoubleAndText(); 
            TypicalBolusNight = new DoubleAndText();
            ChoInsulinRatioBreakfast = new DoubleAndText();
            ChoInsulinRatioLunch = new DoubleAndText();
            ChoInsulinRatioDinner = new DoubleAndText();
            TotalDailyDoseOfInsulin = new DoubleAndText(); 
            GlucoseBeforeMeal = new DoubleAndText(); 
            GlucoseToBeCorrected = new DoubleAndText(); 
            TargetGlucose = new DoubleAndText(); 
            FactorOfInsulinCorrectionSensitivity = new DoubleAndText(); 
            InsulinCorrectionSensitivity = new DoubleAndText(); 
            BolusInsulinDueToCorrectionOfGlucose = new DoubleAndText(); 
            BolusInsulinDueToChoOfMeal = new DoubleAndText(); 
            TotalInsulinForMeal = new DoubleAndText();

            statusMessage = ""; 
            DateTime now = DateTime.Now;
            int y = now.Year;

            initialBreakfastPeriod = new DateTime(y, now.Month, now.Day, 6, 0, 0);
            finalBreakfastPeriod = new DateTime(now.Year, now.Month, now.Day, 10, 0, 0);
            initialLunchPeriod = new DateTime(now.Year, now.Month, now.Day, 11, 0, 0);
            finalLunchPeriod = new DateTime(now.Year, now.Month, now.Day, 15, 0, 0);
            initialDinnerPeriod = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);
            finalDinnerPeriod = new DateTime(now.Year, now.Month, now.Day, 21, 0, 0);

            MealOfBolus = new Meal(); 
        }
        internal void CalculateInsulinCorrectionSensitivity()
        {
            TotalDailyDoseOfInsulin.Double = TypicalBolusMorning.Double + TypicalBolusMidday.Double +
                TypicalBolusEvening.Double + TypicalBolusNight.Double;

            InsulinCorrectionSensitivity.Double = FactorOfInsulinCorrectionSensitivity.Double / TotalDailyDoseOfInsulin.Double;
        }
        internal void CalculateBolus()
        {
            try
            {
                // execute calculations
                TotalDailyDoseOfInsulin.Double = TypicalBolusMorning.Double + TypicalBolusMidday.Double + 
                    TypicalBolusEvening.Double + TypicalBolusNight.Double; 
                // insulin sensitivity is now calculated in a specific method
                //InsulinCorrectionSensitivity.Double = FactorOfInsulinCorrectionSensitivity.Double / TotalDailyDoseOfInsulin.Double;
                GlucoseToBeCorrected.Double = GlucoseBeforeMeal.Double - TargetGlucose.Double;
                BolusInsulinDueToCorrectionOfGlucose.Double = GlucoseToBeCorrected.Double / InsulinCorrectionSensitivity.Double;
               
                switch (MealOfBolus.Type)
                {
                    case (Meal.TypeOfMeal.Breakfast):
                        BolusInsulinDueToChoOfMeal.Double = ChoToEat.Double / ChoInsulinRatioBreakfast.Double;
                        break; 
                    case (Meal.TypeOfMeal.Lunch):
                        BolusInsulinDueToChoOfMeal.Double = ChoToEat.Double / ChoInsulinRatioLunch.Double;
                        break;
                    case (Meal.TypeOfMeal.Dinner):
                        BolusInsulinDueToChoOfMeal.Double = ChoToEat.Double / ChoInsulinRatioBreakfast.Double;
                        break;
                    case (Meal.TypeOfMeal.Snack):
                        BolusInsulinDueToChoOfMeal.Double = 0; // snack has no insulin due to meal 
                        break;
                }
                TotalInsulinForMeal.Double = BolusInsulinDueToChoOfMeal.Double + BolusInsulinDueToCorrectionOfGlucose.Double;
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }

        internal void RoundInsulinToZeroDecimal()
        {
            // find target bolus and relative CHO with bisection algorithm 
            
            // calc bolus to determine target bolus
            CalculateBolus();
            // target bolus will be nearest integer 
            double targetBolus = Math.Round(TotalInsulinForMeal.Double);
            // calc the two CHO "Plus" and "Minus" initial values 
            double initialCho = ChoToEat.Double;
            double choPlus = initialCho + 15;
            double choMinus = initialCho - 15;
            // calc bolus for the "Plus" initial value
            ChoToEat.Double = choPlus;  // ChoToEat.Double is the CHO 
            CalculateBolus();
            double bolusPlus = TotalInsulinForMeal.Double;
            // calc bolus for the "Minus" initial value
            ChoToEat.Double = choMinus;
            CalculateBolus();
            double bolusMinus = TotalInsulinForMeal.Double;

            double bolusCurrent;
            int iterationsCount = 0; 
            do
            {
                // next attempt CHO, in the middle of current interval 
                ChoToEat.Double = (choPlus - choMinus) / 2 + choMinus;
                CalculateBolus();
                bolusCurrent = TotalInsulinForMeal.Double; 
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
                iterationsCount++; 
            } while (Math.Abs(bolusCurrent - targetBolus) > 0.01 && iterationsCount < 20); 
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
