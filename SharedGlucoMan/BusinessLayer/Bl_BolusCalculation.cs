using System;
using System.IO;
using SharedData;
using GlucoMan;

namespace GlucoMan.BusinessLayer
{
    public  class BL_BolusCalculation
    {
        DataLayer dl = Common.Database;

        private string statusMessage;
        public  DoubleAndText ChoToEat { get; set; }
        public  DoubleAndText TypicalBolusMorning { get; set; }
        public  DoubleAndText TypicalBolusMidday { get; set; }
        public  DoubleAndText TypicalBolusEvening { get; set; }
        public  DoubleAndText TypicalBolusNight { get; set; }
        public  DoubleAndText ChoInsulinRatioLunch { get; set; }
        public  DoubleAndText ChoInsulinRatioDinner { get; set; }
        public  DoubleAndText ChoInsulinRatioBreakfast { get; set; }
        public  DoubleAndText TotalDailyDoseOfInsulin { get; set; }
        public  DoubleAndText GlucoseBeforeMeal { get; set; }
        public  DoubleAndText GlucoseToBeCorrected { get; set; }
        public  DoubleAndText TargetGlucose { get; set; }
        public  DoubleAndText FactorOfInsulinCorrectionSensitivity { get; set; }
        public  DoubleAndText InsulinCorrectionSensitivity { get; }
        public  DoubleAndText BolusInsulinDueToCorrectionOfGlucose { get; set; }
        public  DoubleAndText BolusInsulinDueToChoOfMeal { get; }
        public  DoubleAndText TotalInsulinForMeal { get; set; }
        public  string StatusMessage { get => statusMessage; }
        public  Meal MealOfBolus { get; set; }

        private DateTime initialBreakfastPeriod;
        private DateTime finalBreakfastPeriod;
        private DateTime initialLunchPeriod;
        private DateTime finalLunchPeriod;
        private DateTime initialDinnerPeriod;
        private DateTime finalDinnerPeriod;
        public  BL_BolusCalculation()
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
        public  void CalculateInsulinCorrectionSensitivity()
        {
            TotalDailyDoseOfInsulin.Double = TypicalBolusMorning.Double + TypicalBolusMidday.Double +
                TypicalBolusEvening.Double + TypicalBolusNight.Double;
            InsulinCorrectionSensitivity.Double = FactorOfInsulinCorrectionSensitivity.Double / TotalDailyDoseOfInsulin.Double;
        }
        public  void CalculateBolus()
        {
            try
            {
                // execute calculations

                // insulin sensitivity is now calculated in a specific method: CalculateInsulinCorrectionSensitivity()
                GlucoseToBeCorrected.Double = GlucoseBeforeMeal.Double - TargetGlucose.Double;
                BolusInsulinDueToCorrectionOfGlucose.Double = GlucoseToBeCorrected.Double / InsulinCorrectionSensitivity.Double;
               
                switch (MealOfBolus.IdTypeOfMeal)
                {
                    case (Common.TypeOfMeal.Breakfast):
                        BolusInsulinDueToChoOfMeal.Double = ChoToEat.Double / ChoInsulinRatioBreakfast.Double;
                        break; 
                    case (Common.TypeOfMeal.Lunch):
                        BolusInsulinDueToChoOfMeal.Double = ChoToEat.Double / ChoInsulinRatioLunch.Double;
                        break;
                    case (Common.TypeOfMeal.Dinner):
                        BolusInsulinDueToChoOfMeal.Double = ChoToEat.Double / ChoInsulinRatioDinner.Double;
                        break;
                    case (Common.TypeOfMeal.Snack):
                        BolusInsulinDueToChoOfMeal.Double = 0; // snack has no insulin due to meal 
                        break;
                }
                TotalInsulinForMeal.Double = BolusInsulinDueToChoOfMeal.Double + BolusInsulinDueToCorrectionOfGlucose.Double;
            }
            catch (Exception Ex)
            {
                Common.LogOfProgram.Error("BL_BolusCalculations | CalculateBolus()", Ex);
            }
        }
        public  void RoundInsulinToZeroDecimal()
        {
            // find target bolus and relative CHO with bisection algorithm 
            
            // calc bolus to determine target bolus
            CalculateBolus();
            // target bolus will be nearest integer 
            double targetBolus = Math.Round((double)TotalInsulinForMeal.Double);
            // calc the two CHO "Plus" and "Minus" initial values 
            double? initialCho = ChoToEat.Double;
            double? choPlus = initialCho + 15;
            double? choMinus = initialCho - 15;
            // calc bolus for the "Plus" initial value
            ChoToEat.Double = choPlus;  // ChoToEat.Double is the CHO 
            CalculateBolus();
            double? bolusPlus = TotalInsulinForMeal.Double;
            // calc bolus for the "Minus" initial value
            ChoToEat.Double = choMinus;
            CalculateBolus();
            double? bolusMinus = TotalInsulinForMeal.Double;

            double? bolusCurrent;
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
            } while (Math.Abs((double)bolusCurrent - (double)targetBolus) > 0.01 && iterationsCount < 20); 
        }
        public  void SaveBolusLog()
        {
                dl.SaveLogOfBoluses(this); 
        }
        public  void RestoreBolusParameters()
        {
            TargetGlucose.Text = dl.RestoreParameter("Bolus_TargetGlucose");
            ChoInsulinRatioBreakfast.Text = dl.RestoreParameter("Bolus_ChoInsulinRatioBreakfast");
            ChoInsulinRatioLunch.Text = dl.RestoreParameter("Bolus_ChoInsulinRatioLunch");
            ChoInsulinRatioDinner.Text = dl.RestoreParameter("Bolus_ChoInsulinRatioDinner");
            InsulinCorrectionSensitivity.Text = dl.RestoreParameter("Bolus_InsulinCorrectionSensitivity");
            TotalDailyDoseOfInsulin.Text = dl.RestoreParameter("Bolus_TotalDailyDoseOfInsulin");
            ChoToEat.Text = dl.RestoreParameter("Bolus_ChoToEat");
            GlucoseBeforeMeal.Text = dl.RestoreParameter("Bolus_GlucoseBeforeMeal");
        }
        public  void SaveBolusParameters()
        {
            dl.SaveParameter("Bolus_TargetGlucose", TargetGlucose.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioBreakfast", ChoInsulinRatioBreakfast.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioLunch", ChoInsulinRatioLunch.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioDinner", ChoInsulinRatioDinner.Text);
            dl.SaveParameter("Bolus_InsulinCorrectionSensitivity", InsulinCorrectionSensitivity.Text);
            dl.SaveParameter("Bolus_TotalDailyDoseOfInsulin", TotalDailyDoseOfInsulin.Text);
            dl.SaveParameter("Bolus_ChoToEat", ChoToEat.Text);
            dl.SaveParameter("Bolus_GlucoseBeforeMeal", GlucoseBeforeMeal.Text);
    }
        public  void SaveInsulinCorrectionParameters()
        {
            dl.SaveParameter("Correction_TypicalBolusMorning", TypicalBolusMorning.Text);
            dl.SaveParameter("Correction_TypicalBolusMidday", TypicalBolusMidday.Text);
            dl.SaveParameter("Correction_TypicalBolusEvening", TypicalBolusEvening.Text);
            dl.SaveParameter("Correction_TypicalBolusNight", TypicalBolusNight.Text);
            dl.SaveParameter("Correction_FactorOfInsulinCorrectionSensitivity", FactorOfInsulinCorrectionSensitivity.Text);
            dl.SaveParameter("Bolus_TotalDailyDoseOfInsulin", TotalDailyDoseOfInsulin.Text);
            dl.SaveParameter("Bolus_InsulinCorrectionSensitivity", InsulinCorrectionSensitivity.Text);
        }
        public  void RestoreInsulinCorrectionParameters()
        {
            TypicalBolusMorning.Text = dl.RestoreParameter("Correction_TypicalBolusMorning");
            TypicalBolusMidday.Text = dl.RestoreParameter("Correction_TypicalBolusMidday");
            TypicalBolusEvening.Text = dl.RestoreParameter("Correction_TypicalBolusEvening");
            TypicalBolusNight.Text = dl.RestoreParameter("Correction_TypicalBolusNight");
            FactorOfInsulinCorrectionSensitivity.Text = dl.RestoreParameter("Correction_FactorOfInsulinCorrectionSensitivity");
            TotalDailyDoseOfInsulin.Text = dl.RestoreParameter("Bolus_TotalDailyDoseOfInsulin");
            InsulinCorrectionSensitivity.Text = dl.RestoreParameter("Bolus_InsulinCorrectionSensitivity");
        }
        public void SaveRatioCHOInsulinParameters()
        {
            dl.SaveParameter("Bolus_ChoInsulinRatioBreakfast", ChoInsulinRatioBreakfast.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioLunch", ChoInsulinRatioLunch.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioDinner", ChoInsulinRatioDinner.Text);
        }
        public void RestoreRatioCHOInsulinParameters()
        {
            ChoInsulinRatioBreakfast.Text = dl.RestoreParameter("Bolus_ChoInsulinRatioBreakfast");
            ChoInsulinRatioLunch.Text = dl.RestoreParameter("Bolus_ChoInsulinRatioLunch");
            ChoInsulinRatioDinner.Text = dl.RestoreParameter("Bolus_ChoInsulinRatioDinner");
        }
    }
}
  