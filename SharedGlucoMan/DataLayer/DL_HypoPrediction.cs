using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal abstract partial class DataLayer
    {
        internal void SaveHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo)
        {
            try
            {
                string file = Hypo.HypoGlucoseTarget.Text + "\n";
                file += Hypo.GlucoseLast.Text + "\n";
                file += Hypo.GlucosePrevious.Text + "\n";
                file += Hypo.HourLast.Text + "\n";
                file += Hypo.HourPrevious.Text + "\n";
                file += Hypo.MinuteLast.Text + "\n";
                file += Hypo.MinutePrevious.Text + "\n";
                file += Hypo.AlarmAdvanceTime.TotalMinutes;
                TextFile.StringToFile(persistentHypoPrediction, file, false);
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo)
        {
            if (File.Exists(persistentHypoPrediction))
                try
                {
                    string[] f = TextFile.FileToArray(persistentHypoPrediction);
                    Hypo.HypoGlucoseTarget.Text = f[0];
                    Hypo.GlucoseLast.Text = f[1];
                    Hypo.GlucosePrevious.Text = f[2];
                    Hypo.HourLast.Text = f[3];
                    Hypo.HourPrevious.Text = f[4];
                    Hypo.MinuteLast.Text = f[5];
                    Hypo.MinutePrevious.Text = f[6];
                    Hypo.AlarmAdvanceTime = new TimeSpan(0, int.Parse(f[7]), 0);
                }
                catch (Exception ex)
                {
                    CommonFunctions.NotifyError(ex.Message);
                }
        }
    }
}
