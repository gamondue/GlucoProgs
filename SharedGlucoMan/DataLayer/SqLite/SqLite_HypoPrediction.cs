using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    internal  partial class DL_Sqlite : DataLayer
    {
        internal  override void SaveHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Parameters)
        {
            SaveParameter("Hypo_GlucoseTarget", Parameters.GlucoseTarget.Text);
            SaveParameter("Hypo_GlucoseLast", Parameters.GlucoseLast.Text);
            SaveParameter("Hypo_GlucosePrevious", Parameters.GlucosePrevious.Text); 
            SaveParameter("Hypo_HourLast", Parameters.HourLast.Text);
            SaveParameter("Hypo_HourPrevious", Parameters.HourPrevious.Text);
            SaveParameter("Hypo_MinuteLast", Parameters.MinuteLast.Text);
            SaveParameter("Hypo_MinutePrevious", Parameters.MinutePrevious.Text);
            SaveParameter("Hypo_AlarmAdvanceTime", Parameters.AlarmAdvanceTime.Text);
            SaveParameter("Hypo_FutureSpanMinutes", Parameters.FutureSpanMinutes.Text);
        }
        internal  override void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Parameters)
        {
            Parameters.GlucoseTarget.Text = RestoreParameter("Hypo_GlucoseTarget");
            Parameters.GlucoseLast.Text = RestoreParameter("Hypo_GlucoseLast");
            Parameters.GlucosePrevious.Text = RestoreParameter("Hypo_GlucosePrevious");
            Parameters.HourLast.Text = RestoreParameter("Hypo_HourLast");
            Parameters.HourPrevious.Text = RestoreParameter("Hypo_HourPrevious");
            Parameters.MinuteLast.Text = RestoreParameter("Hypo_MinuteLast");
            Parameters.MinutePrevious.Text = RestoreParameter("Hypo_MinutePrevious");
            int? minutes = Safe.Int(RestoreParameter("Hypo_AlarmAdvanceTime"));
            if (minutes == null)
                minutes = 0;
            Parameters.AlarmAdvanceTime.Text = RestoreParameter("Hypo_AlarmAdvanceTime");
            Parameters.FutureSpanMinutes.Text = RestoreParameter("Hypo_FutureSpanMinutes");  
        }
    }
}
