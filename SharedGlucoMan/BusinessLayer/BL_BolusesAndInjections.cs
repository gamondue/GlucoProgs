using gamon;
using System;
using System.Collections.Generic;
using System.IO;

namespace GlucoMan.BusinessLayer
{
    public class BL_BolusesAndInjections
    {
        DataLayer dl = Common.Database;

        private string statusMessage;
        public DoubleAndText ChoToEat { get; set; }
        public DoubleAndText TypicalBolusMorning { get; set; }
        public DoubleAndText TypicalBolusMidday { get; set; }
        public DoubleAndText TypicalBolusEvening { get; set; }
        public DoubleAndText TypicalBolusNight { get; set; }
        public DoubleAndText ChoInsulinRatioLunch { get; set; }
        public DoubleAndText ChoInsulinRatioDinner { get; set; }
        public DoubleAndText ChoInsulinRatioBreakfast { get; set; }
        public DoubleAndText TotalInsulinExceptEmbarked { get; set; }
        public DoubleAndText TotalDailyDoseOfInsulin { get; set; }
        public DoubleAndText GlucoseBeforeMeal { get; set; }
        public DoubleAndText GlucoseToBeCorrected { get; set; }
        internal void SetTypeOfInsulinSpeedBasedOnTimeNow(InsulinInjection Injection)
        {
            TimeSpan timeOfDay = DateTime.Now.TimeOfDay;
            if (timeOfDay > new TimeSpan(2, 0, 0) && timeOfDay < new TimeSpan(22, 0, 0))
            {
                Injection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.RapidActing;
            }
            else
            {
                Injection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.ShortActing;
            }
        }
        public DoubleAndText TargetGlucose { get; set; }
        public DoubleAndText FactorOfInsulinCorrectionSensitivity { get; set; }
        public DoubleAndText InsulinCorrectionSensitivity { get; }
        public DoubleAndText BolusInsulinDueToCorrectionOfGlucose { get; set; }
        public DoubleAndText BolusInsulinDueToChoOfMeal { get; }
        public DoubleAndText EmbarkedInsulin { get; }
        public DoubleAndText TotalInsulinForMeal { get; set; }
        public string StatusMessage { get => statusMessage; }
        public Meal MealOfBolus { get; set; }

        private DateTime initialBreakfastPeriod;
        private DateTime finalBreakfastPeriod;
        private DateTime initialLunchPeriod;
        private DateTime finalLunchPeriod;
        private DateTime initialDinnerPeriod;

        internal List<InsulinInjection> GetInjections(DateTime InitialInstant,
            DateTime FinalInstant, 
            Common.TypeOfInsulinSpeed TypeOfInsulinSpeed = Common.TypeOfInsulinSpeed.NotSet,
            Common.ZoneOfPosition Zone = Common.ZoneOfPosition.NotSet)
        {
            return dl.GetInjections(InitialInstant, FinalInstant, TypeOfInsulinSpeed, Zone);
        }
        internal InsulinInjection GetOneInjection(int? IdInjection)
        {
            return dl.GetOneInjection(IdInjection);
        }
        private DateTime finalDinnerPeriod;

        public BL_BolusesAndInjections()
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
            EmbarkedInsulin = new DoubleAndText();
            TotalInsulinExceptEmbarked = new DoubleAndText();
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
        internal void DeleteOneInjection(InsulinInjection Injection)
        {
            dl.DeleteOneInjection(Injection);
        }
        public void CalculateInsulinCorrectionSensitivity()
        {
            TotalDailyDoseOfInsulin.Double = TypicalBolusMorning.Double + TypicalBolusMidday.Double +
                TypicalBolusEvening.Double + TypicalBolusNight.Double;
            InsulinCorrectionSensitivity.Double = FactorOfInsulinCorrectionSensitivity.Double / TotalDailyDoseOfInsulin.Double;
        }
        public void CalculateBolus()
        {
            try
            {
                // execute calculations

                // insulin sensitivity is now calculated in a specific method: CalculateInsulinCorrectionSensitivity()
                GlucoseToBeCorrected.Double = GlucoseBeforeMeal.Double - TargetGlucose.Double;
                BolusInsulinDueToCorrectionOfGlucose.Double = GlucoseToBeCorrected.Double / InsulinCorrectionSensitivity.Double;
                // calculate embarked insulin for RapidActing insulin  
                // TODO calculate embarked insulin for each TypeOfInsulinSpeed !!!!
                CalculateEmbarkedInsulin(DateTime.Now, Common.TypeOfInsulinSpeed.RapidActing);

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
                TotalInsulinExceptEmbarked.Double = BolusInsulinDueToCorrectionOfGlucose.Double;
                if (BolusInsulinDueToChoOfMeal.Double != null)
                    TotalInsulinExceptEmbarked.Double += BolusInsulinDueToChoOfMeal.Double;
                TotalInsulinForMeal.Double = TotalInsulinExceptEmbarked.Double - EmbarkedInsulin.Double;
                if (TotalInsulinForMeal.Double < 0)
                    TotalInsulinForMeal.Double = 0;
            }
            catch (Exception Ex)
            {
                Common.LogOfProgram.Error("BL_BolusesAndInjections | CalculateBolus()", Ex);
            }
        }
        public void RoundInsulinToZeroDecimal()
        {
            // find target bolus and relative CHO with bisection algorithm 

            if (TotalInsulinForMeal.Double == null)
                return;
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
        internal string RestoreChoToEat()
        {
            return dl.RestoreParameter("Meal_ChoGrams").ToString();
        }
        public void SaveBolusLog()
        {
            // !!!! TO DO !!!!
        }
        public void RestoreBolusParameters()
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
        public void SaveBolusParameters()
        {
            dl.SaveParameter("Bolus_TargetGlucose", TargetGlucose.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioBreakfast", ChoInsulinRatioBreakfast.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioLunch", ChoInsulinRatioLunch.Text);
            dl.SaveParameter("Bolus_ChoInsulinRatioDinner", ChoInsulinRatioDinner.Text);
            dl.SaveParameter("Bolus_InsulinCorrectionSensitivity", InsulinCorrectionSensitivity.Text);
            dl.SaveParameter("Bolus_TotalDailyDoseOfInsulin", TotalDailyDoseOfInsulin.Text);
            dl.SaveParameter("Bolus_ChoToEat", ChoToEat.Text);
            dl.SaveParameter("Bolus_GlucoseBeforeMeal", GlucoseBeforeMeal.Text);
            AppendToLogOfParameters();
        }
        public void SaveInsulinCorrectionParameters()
        {
            dl.SaveParameter("Correction_TypicalBolusMorning", TypicalBolusMorning.Text);
            dl.SaveParameter("Correction_TypicalBolusMidday", TypicalBolusMidday.Text);
            dl.SaveParameter("Correction_TypicalBolusEvening", TypicalBolusEvening.Text);
            dl.SaveParameter("Correction_TypicalBolusNight", TypicalBolusNight.Text);
            dl.SaveParameter("Correction_FactorOfInsulinCorrectionSensitivity", FactorOfInsulinCorrectionSensitivity.Text);
            dl.SaveParameter("Bolus_TotalDailyDoseOfInsulin", TotalDailyDoseOfInsulin.Text);
            dl.SaveParameter("Bolus_InsulinCorrectionSensitivity", InsulinCorrectionSensitivity.Text);
            // append the new data to the log file 
            AppendToLogOfParameters();
        }
        private void AppendToLogOfParameters()
        {
            string TextOfFile = "";

            TextOfFile += "Bolus_TargetGlucose=\t" + TargetGlucose.Text + "\t";
            TextOfFile += "Bolus_ChoInsulinRatioBreakfast=\t" + ChoInsulinRatioBreakfast.Text + "\t";
            TextOfFile += "Bolus_ChoInsulinRatioLunch=\t" + ChoInsulinRatioLunch.Text + "\t";
            TextOfFile += "Bolus_ChoInsulinRatioDinner=\t" + ChoInsulinRatioDinner.Text + "\t";
            TextOfFile += "Bolus_InsulinCorrectionSensitivity=\t" + InsulinCorrectionSensitivity.Text + "\t";
            TextOfFile += "Bolus_TotalDailyDoseOfInsulin=\t" + TotalDailyDoseOfInsulin.Text + "\t";
            TextOfFile += "Bolus_ChoToEat=\t" + ChoToEat.Text + "\t";
            TextOfFile += "Bolus_GlucoseBeforeMeal=\t" + GlucoseBeforeMeal.Text + "\t";

            TextOfFile += "Date and Time=\t" + DateTime.Now.ToString() + "\t";
            TextOfFile += "Typical Bolus Morning=\t" + TypicalBolusMorning.Text + "\t";
            TextOfFile += "Typical Bolus Midday=\t" + TypicalBolusMidday.Text + "\t";
            TextOfFile += "Typical Bolus Evening=\t" + TypicalBolusEvening.Text + "\t";
            TextOfFile += "Typical Bolus Night=\t" + TypicalBolusNight.Text + "\t";
            TextOfFile += "Total Daily Dose of Insulin\t" + TotalDailyDoseOfInsulin.Text + "\t";
            TextOfFile += "Factor of Insulin Correction Sensitivity=\t" + FactorOfInsulinCorrectionSensitivity.Text + "\t";
            TextOfFile += "Insulin Correction Sensitivity\t" + InsulinCorrectionSensitivity.Text + "\t";
            TextOfFile += "\n";
            // write in append to the file 
            File.AppendAllText(Common.PathAndFileLogOfParameters, TextOfFile);
        }
        public void RestoreInsulinCorrectionParameters()
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
        internal void SaveOneInjection(InsulinInjection Injection)
        {
            dl.SaveOneInjection(Injection);
        }
        public double CalculateEmbarkedInsulin(DateTime LastInjectionTargetTime, Common.TypeOfInsulinSpeed InsulinSpeed)
        {
            DateTime FinalInstant = DateTime.Now;
            DateTime InitialInstant = FinalInstant.Subtract(new TimeSpan(0, (int)(InsulinDurationInHours(InsulinSpeed) * 60), 0)); ;
            // !!!! TODO insulin duration was encoded here; manage it including the datum in database !!!! 
            //switch (InsulinSpeed)
            //{
            //    case Common.TypeOfInsulinSpeed.RapidActing: InitialInstant = FinalInstant.Subtract(new TimeSpan(3, 0, 0)); break;
            //    case Common.TypeOfInsulinSpeed.ShortActing: InitialInstant = FinalInstant.Subtract(new TimeSpan(24, 0, 0)); break;
            //    default: InitialInstant = FinalInstant.Subtract(new TimeSpan(24, 0, 0)); break;
            //}
            List<InsulinInjection> nonExaustedInsulin =
                dl.GetInjections(InitialInstant, FinalInstant, InsulinSpeed);
            EmbarkedInsulin.Double = 0;
            foreach (InsulinInjection ii in nonExaustedInsulin)
            {
                TimeSpan timeFromInjection = FinalInstant.Subtract((DateTime)ii.Timestamp.DateTime);
                // TODO should generalize with specific type of insuline drug 
                if (ii.IdTypeOfInsulinSpeed == null)
                    break;
                double insulinDurationInHours = InsulinDurationInHours((Common.TypeOfInsulinSpeed)ii.IdTypeOfInsulinSpeed);
                double partialEmbarkedInsulin = (double)ii.InsulinValue.Double
                    * (1 - timeFromInjection.TotalHours / insulinDurationInHours);
                if (partialEmbarkedInsulin > 0)
                    EmbarkedInsulin.Double += partialEmbarkedInsulin;
            }
            if (EmbarkedInsulin.Double < 0)
                EmbarkedInsulin.Double = 0;
            return (double)EmbarkedInsulin.Double;
        }
        // !!!! TODO InsulinDuration is hard encoded in this method. Bring it to the database !!!!
        internal double InsulinDurationInHours(Common.TypeOfInsulinSpeed InsulinSpeed)
        {
            switch (InsulinSpeed)
            {
                case Common.TypeOfInsulinSpeed.RapidActing:
                    {
                        return 3;
                    }
                case Common.TypeOfInsulinSpeed.ShortActing:
                    {
                        return 4.5;
                    }
                case Common.TypeOfInsulinSpeed.IntermediateActing:
                    {
                        return 12 - 0 + (18.0 - 12) / 2.0;
                    }
                case Common.TypeOfInsulinSpeed.LongActing:
                    {
                        return 24;
                    }
                default:
                    {
                        return 0;
                    }
                    //RapidActing = 10, // about 15 minutes to start working, peaks in 1-2 hours, and lasts for 2-4 hours
                    //ShortActing = 20, // 30 minutes to start working, peaks in 2-3 hours, and lasts for 3-6 hours.
                    //IntermediateActing = 30, // about 2-4 hours to start working, peaks in 4-12 hours, and lasts for 12-18 hours.
                    //LongActing = 40 // 1-2 hours to start working, has no peak effect, and lasts for 24+ hours
            };
        }
    }
}
