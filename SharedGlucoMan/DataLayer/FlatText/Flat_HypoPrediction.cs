using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal partial class DL_FlatText : DataLayer
    {
        //internal  override void SaveHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo)
        //{
        //    try
        //    {
        //        string file = Hypo.GlucoseTarget.Text + "\n";
        //        file += Hypo.GlucoseLast.Text + "\n";
        //        file += Hypo.GlucosePrevious.Text + "\n";
        //        file += Hypo.HourLast.Text + "\n";
        //        file += Hypo.HourPrevious.Text + "\n";
        //        file += Hypo.MinuteLast.Text + "\n";
        //        file += Hypo.MinutePrevious.Text + "\n";
        //        file += Hypo.AlarmAdvanceTime.Text;
        //        TextFile.StringToFileAsync(persistentHypoPrediction, file);
        //    }
        //    catch (Exception ex)
        //    {
        //        Common.LogOfProgram.Error("DL_HypoPrediction | SaveHypoPrediction", ex);
        //    }
        //}
        //internal  override void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Hypo)
        //{
        //    if (File.Exists(persistentHypoPrediction))
        //        try
        //        {
        //            string[] f = TextFile.FileToArray(persistentHypoPrediction);
        //            Hypo.GlucoseTarget.Text = f[0];
        //            Hypo.GlucoseLast.Text = f[1];
        //            Hypo.GlucosePrevious.Text = f[2];
        //            Hypo.HourLast.Text = f[3];
        //            Hypo.HourPrevious.Text = f[4];
        //            Hypo.MinuteLast.Text = f[5];
        //            Hypo.MinutePrevious.Text = f[6];
        //            Hypo.AlarmAdvanceTime.Text = f[7];
        //        }
        //        catch (Exception ex)
        //        {
        //            Common.LogOfProgram.Error("DL_HypoPrediction | RestoreHypoPrediction" , ex);
        //        }
        //}
    }
}
