using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedGlucoMan.DataAccessLayer
{
    class DataLayer_Flat : DataLayer
    {
        string persistentStorage = Path.Combine(CommonData.PathConfigurationData, @"BolusCalculation.txt");

        internal void BolusCalc_RestoreData()
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
                    Console.Beep();
                }
        }
    }
}
