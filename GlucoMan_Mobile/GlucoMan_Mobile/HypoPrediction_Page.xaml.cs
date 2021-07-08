
using GlucoMan;
using GlucoMan.BusinessLayer;
using SharedData;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HypoPrediction_Page : ContentPage
    {
        BL_HypoPrediction hypo;

        public HypoPrediction_Page()
        {
            InitializeComponent();

            hypo = new BL_HypoPrediction();

            hypo.RestoreData();
            FromClassToUi();

            txtGlucoseSlope.Text = "XXXX";
            txtGlucoseLast.Focus();
        }
        private void FromClassToUi()
        {
            txtGlucoseTarget.Text = hypo.HypoGlucoseTarget.Text;
            txtGlucoseLast.Text = hypo.GlucoseLast.Text;
            txtGlucosePrevious.Text = hypo.GlucosePrevious.Text;
            txtHourLast.Text = hypo.HourLast.Text;
            txtHourPrevious.Text = hypo.HourPrevious.Text;
            txtMinuteLast.Text = hypo.MinuteLast.Text;
            txtMinutePrevious.Text = hypo.MinutePrevious.Text;
            txtAlarmAdvanceTime.Text = hypo.AlarmAdvanceTime.TotalMinutes.ToString();
            txtGlucoseSlope.Text = hypo.GlucoseSlope.ToString();
        }
        private void FromUiToClass(BL_HypoPrediction hypo)
        {
            hypo.AlarmAdvanceTime = new TimeSpan(0, int.Parse(txtAlarmAdvanceTime.Text), 0);
            hypo.HypoGlucoseTarget.Text = txtGlucoseTarget.Text;
            hypo.GlucoseLast.Text = txtGlucoseLast.Text;
            hypo.GlucosePrevious.Text = txtGlucosePrevious.Text;
            hypo.HourLast.Text = txtHourLast.Text;
            hypo.HourPrevious.Text = txtHourPrevious.Text;
            hypo.MinuteLast.Text = txtMinuteLast.Text;
            hypo.MinutePrevious.Text = txtMinutePrevious.Text;
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
            FromUiToClass(hypo); 
            hypo.PredictHypoTime();
            txtGlucoseSlope.Text = hypo.GlucoseSlope.Text;
            txtPredictedHour.Text = hypo.PredictedHour.Text;
            txtPredictedMinute.Text = hypo.PredictedMinute.Text;
            txtAlarmHour.Text = hypo.AlarmHour.Text;
            txtAlarmMinute.Text = hypo.AlarmMinute.Text;
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
    }
}