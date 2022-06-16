
using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HypoPredictionPage : ContentPage
    {
        BL_HypoPrediction hypo;
        BL_GlucoseMeasurements blMeasurements = new BL_GlucoseMeasurements();
        public HypoPredictionPage()
        {
            InitializeComponent();

            hypo = new BL_HypoPrediction();

            hypo.RestoreData();
            FromClassToUi();

            txtGlucoseSlope.Text = "----";
            txtGlucoseLast.Focus();

            txtStatusBar.IsVisible = false;
        }
        private void FromClassToUi()
        {
            txtGlucoseTarget.Text = hypo.Hypo_GlucoseTarget.Text;
            txtGlucoseLast.Text = hypo.Hypo_GlucoseLast.Text;
            txtGlucosePrevious.Text = hypo.Hypo_GlucosePrevious.Text;
            txtHourLast.Text = hypo.Hypo_HourLast.Text;

            txtHourPrevious.Text = hypo.Hypo_HourPrevious.Text;
            txtMinuteLast.Text = hypo.Hypo_MinuteLast.Text;
            txtMinutePrevious.Text = hypo.Hypo_MinutePrevious.Text;

            txtAlarmAdvanceTime.Text = hypo.Hypo_AlarmAdvanceTime.TotalMinutes.ToString();
            txtGlucoseSlope.Text = hypo.HypoGlucoseSlope.Text;
            DateTime dummy = (DateTime)hypo.HypoAlarmTime.DateTime; 
            txtAlarmHour.Text = dummy.Hour.ToString();
            txtAlarmMinute.Text = dummy.Minute.ToString();

            txtPredictedHour.Text = hypo.PredictedHour.Text;
            txtPredictedMinute.Text = hypo.PredictedMinute.Text;
            if (hypo.StatusMessage != null && hypo.StatusMessage != "")
            {
                txtStatusBar.IsVisible = true; 
                txtStatusBar.Text = hypo.StatusMessage;
            }
            else
                txtStatusBar.IsVisible = false;
        }
        private void FromUiToClass()
        {
            hypo.Hypo_AlarmAdvanceTime = new TimeSpan(0,(int)Safe.Int(txtAlarmAdvanceTime.Text), 0);
            hypo.Hypo_GlucoseTarget.Text = txtGlucoseTarget.Text;
            hypo.Hypo_GlucoseLast.Text = txtGlucoseLast.Text;
            hypo.Hypo_GlucosePrevious.Text = txtGlucosePrevious.Text;

            hypo.Hypo_HourLast.Text = txtHourLast.Text;
            hypo.Hypo_MinuteLast.Text = txtMinuteLast.Text;

            hypo.Hypo_HourPrevious.Text = txtHourPrevious.Text;
            hypo.Hypo_MinutePrevious.Text = txtMinutePrevious.Text; 
        }
        private void btnNow_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            txtHourLast.Text = now.Hour.ToString();
            txtMinuteLast.Text = now.Minute.ToString();
            txtGlucosePrevious.Focus();
        }
        private void btnPredict_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            hypo.PredictHypoTime();
            FromClassToUi();
            //txtGlucoseSlope.Text = hypo.GlucoseSlope.Text;
            //txtPredictedHour.Text = hypo.PredictedHour.Text;
            //txtPredictedMinute.Text = hypo.PredictedMinute.Text;
            //txtAlarmHour.Text = hypo.AlarmHour.Text;
            //txtAlarmMinute.Text = hypo.AlarmMinute.Text;
            //txtStatusBar.Text = hypo.StatusMessage;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            txtGlucosePrevious.Text = txtGlucoseLast.Text;
            txtHourPrevious.Text = txtHourLast.Text;
            txtMinutePrevious.Text = txtMinuteLast.Text;

            txtGlucoseLast.Text = "";
            btnNow_Click(null, null);
            txtGlucoseLast.Focus();
        }
        private void btnAlarm_Click(object sender, EventArgs e)
        {
              
        }
        private void btnPaste_Click(object sender, EventArgs e)
        {
            //Control c = SharedWinForms.Methods.FindFocusedControl(this);
            //try
            //{
            //    c.Text = Clipboard.GetText();
            //}
            //catch (Exception ex)
            //{ }
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            List<GlucoseRecord> list = blMeasurements.GetLastTwoGlucoseMeasurements();
            if (list.Count != 0)
            {
                txtGlucoseLast.Text = list[0].GlucoseValue.ToString();
                txtGlucosePrevious.Text = list[1].GlucoseValue.ToString();
                txtHourLast.Text = list[0].Timestamp.Value.Hour.ToString();
                txtHourPrevious.Text = list[1].Timestamp.Value.Hour.ToString();
                txtMinuteLast.Text = list[0].Timestamp.Value.Minute.ToString();
                txtMinutePrevious.Text = list[1].Timestamp.Value.Minute.ToString();
            }
        }
    }
}