using SharedData;
using GlucoMan;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlucoMan
{
    public  partial class DL_Sqlite : DataLayer
    {
        public  override void SaveHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Parameters)
        {
            SaveParameter("Hypo_GlucoseTarget", Parameters.Hypo_GlucoseTarget.Text);
            SaveParameter("Hypo_GlucoseLast", Parameters.Hypo_GlucoseLast.Text);
            SaveParameter("Hypo_GlucosePrevious", Parameters.Hypo_GlucosePrevious.Text); 
            SaveParameter("Hypo_HourLast", Parameters.Hypo_HourLast.Text);
            SaveParameter("Hypo_HourPrevious", Parameters.Hypo_HourPrevious.Text);
            SaveParameter("Hypo_MinuteLast", Parameters.Hypo_MinuteLast.Text);
            SaveParameter("Hypo_MinutePrevious", Parameters.Hypo_MinutePrevious.Text);
            SaveParameter("Hypo_AlarmAdvanceTime", Parameters.Hypo_AlarmAdvanceTime.ToString());
        }
        public  override void RestoreHypoPrediction(GlucoMan.BusinessLayer.BL_HypoPrediction Parameters)
        {
            Parameters.Hypo_GlucoseTarget.Text = RestoreParameter("Hypo_GlucoseTarget");
            Parameters.Hypo_GlucoseLast.Text = RestoreParameter("Hypo_GlucoseLast");
            Parameters.Hypo_GlucosePrevious.Text = RestoreParameter("Hypo_GlucosePrevious");
            Parameters.Hypo_HourLast.Text = RestoreParameter("Hypo_HourLast");
            Parameters.Hypo_HourPrevious.Text = RestoreParameter("Hypo_HourPrevious");
            Parameters.Hypo_MinuteLast.Text = RestoreParameter("Hypo_MinuteLast");
            Parameters.Hypo_MinutePrevious.Text = RestoreParameter("Hypo_MinutePrevious");
            int? i = Safe.Int(RestoreParameter("Hypo_AlarmAdvanceTime"));
            if (i == null)
                i = 0;
            Parameters.Hypo_AlarmAdvanceTime = new TimeSpan(0, (int)i, 0); 
        }
    }
}
