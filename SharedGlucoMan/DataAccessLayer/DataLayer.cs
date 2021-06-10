using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SharedGlucoMan.DataAccessLayer
{
    internal abstract class DataLayer
    {
        internal void BolusCalc_SaveData(BolusCalcData bolusCalc)
        {
            //string file = ChoInsulineRatioMorning.Text + "\n";
            //file += ChoInsulineRatioMidday.Text + "\n";
            //file += ChoInsulineRatioEvening.Text + "\n";
            //file += TypicalBolusMorning.Text + "\n";
            //file += TypicalBolusMidday.Text + "\n";
            //file += TypicalBolusEvening.Text + "\n";
            //file += TypicalBolusNight.Text + "\n";
            //file += GlucoseBeforeMeal.Text + "\n";
            //file += TargetGlucose.Text + "\n";
            //file += ChoToEat.Text + "\n";

            //TextFile.StringToFile(persistentStorage, file, false);
        }

        internal abstract void BolusCalc_RestoreData(); 
    }
}
