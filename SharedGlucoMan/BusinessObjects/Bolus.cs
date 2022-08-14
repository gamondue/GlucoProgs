using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class Bolus
    {
        public int IdBolusCalculation { get; set; }
        public DateTimeAndText Timestamp { get; set; }
        public DoubleAndText TotalInsulinForMeal { get; set; }
        public DoubleAndText CalculatedChoToEat { get; set; }
        public DoubleAndText BolusInsulinDueToChoOfMeal { get; }
        public DoubleAndText BolusInsulinDueToCorrectionOfGlucose { get; set; }
        public DoubleAndText TargetGlucose { get; set; }
        public DoubleAndText InsulinCorrectionSensitivity { get; }
        public DoubleAndText TypicalBolusMorning { get; set; }
        public DoubleAndText TypicalBolusMidday { get; set; }
        public DoubleAndText TypicalBolusEvening { get; set; }
        public DoubleAndText TypicalBolusNight { get; set; }
        public DoubleAndText TotalDailyDoseOfInsulin { get; set; }
        public DoubleAndText ChoInsulinRatioLunch { get; set; }
        public DoubleAndText ChoInsulinRatioDinner { get; set; }
        public DoubleAndText ChoInsulinRatioBreakfast { get; set; }
        public DoubleAndText GlucoseBeforeMeal { get; set; }
        public DoubleAndText GlucoseToBeCorrected { get; set; }
        public DoubleAndText FactorOfInsulinCorrectionSensitivity { get; set; }
        public Bolus ()
        {
            Timestamp = new DateTimeAndText(); 
            TotalInsulinForMeal = new DoubleAndText();
            CalculatedChoToEat = new DoubleAndText();
            BolusInsulinDueToCorrectionOfGlucose = new DoubleAndText();
            TargetGlucose = new DoubleAndText();
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
            FactorOfInsulinCorrectionSensitivity = new DoubleAndText();
            InsulinCorrectionSensitivity = new DoubleAndText();
            BolusInsulinDueToChoOfMeal = new DoubleAndText();
        }
    }
}
