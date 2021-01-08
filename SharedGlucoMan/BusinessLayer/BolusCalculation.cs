using System;
using System.IO;
using SharedData;

namespace GlucoMan.BusinessLayer
{
    internal class BolusCalculation
    {
        string persistentStorage = CommonData.PathConfigurationData + @"BolusCalculation.txt";
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
        internal DoubleAndText TotalChoBreakfast { get; set; }
        internal DoubleAndText TotalChoLunch { get; set; }
        internal DoubleAndText TotalChoDinner { get; set; }
        internal BolusCalculation()
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
            TotalChoBreakfast = new DoubleAndText(); 
            TotalChoLunch = new DoubleAndText(); 
            TotalChoDinner = new DoubleAndText(); 
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
                ChoInsulineDinner.Double = ChoToEat.Double / ChoInsulineRatioEvening.Double;
                ChoInsulineLunch.Double = ChoToEat.Double / ChoInsulineRatioMidday.Double;

                TotalChoBreakfast.Double = ChoInsulineBreakfast.Double + CorrectionInsuline.Double;
                TotalChoLunch.Double = ChoInsulineLunch.Double + CorrectionInsuline.Double;
                TotalChoDinner.Double = ChoInsulineDinner.Double + CorrectionInsuline.Double;
            }
            catch (Exception ex)
            {
                Console.Beep();
            }
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
                Console.Beep();
            }
        }
        internal HypoPrediction RestoreData()
        {
            HypoPrediction h = null;
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
                    Console.Beep();
                }
            return h;
        }
    }
}
