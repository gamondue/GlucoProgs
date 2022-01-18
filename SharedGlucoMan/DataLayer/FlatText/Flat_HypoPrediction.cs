using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public partial class DL_FlatText : DataLayer
    {
        public  override void SaveHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo)
        {
            try
            {
                string file = Hypo.Hypo_GlucoseTarget.Text + "\n";
                file += Hypo.Hypo_GlucoseLast.Text + "\n";
                file += Hypo.Hypo_GlucosePrevious.Text + "\n";
                file += Hypo.Hypo_HourLast.Text + "\n";
                file += Hypo.Hypo_HourPrevious.Text + "\n";
                file += Hypo.Hypo_MinuteLast.Text + "\n";
                file += Hypo.Hypo_MinutePrevious.Text + "\n";
                file += Hypo.Hypo_AlarmAdvanceTime.TotalMinutes;
                TextFile.StringToFileAsync(persistentHypoPrediction, file);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("DL_HypoPrediction | SaveHypoPrediction", ex);
            }
        }
        public  override void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo)
        {
            if (File.Exists(persistentHypoPrediction))
                try
                {
                    string[] f = TextFile.FileToArray(persistentHypoPrediction);
                    Hypo.Hypo_GlucoseTarget.Text = f[0];
                    Hypo.Hypo_GlucoseLast.Text = f[1];
                    Hypo.Hypo_GlucosePrevious.Text = f[2];
                    Hypo.Hypo_HourLast.Text = f[3];
                    Hypo.Hypo_HourPrevious.Text = f[4];
                    Hypo.Hypo_MinuteLast.Text = f[5];
                    Hypo.Hypo_MinutePrevious.Text = f[6];
                    Hypo.Hypo_AlarmAdvanceTime = new TimeSpan(0, int.Parse(f[7]), 0);
                }
                catch (Exception ex)
                {
                    Common.LogOfProgram.Error("DL_HypoPrediction | RestoreHypoPrediction" , ex);
                }
        }
    }
}
