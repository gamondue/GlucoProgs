
using GlucoMan;
using GlucoMan.BusinessLayer;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan_Mobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HypoPrediction_Page : ContentPage
    {
        HypoPrediction hypo;

        public HypoPrediction_Page()
        {
            InitializeComponent();

            hypo = new HypoPrediction();

            //btnNow_Click(null, null);

            hypo.RestoreData();
            FromClassToUi(hypo);

            txtGlucoseSlope.Text = "XXXX";
            txtGlucoseLast.Focus();
        }
        private void FromClassToUi(HypoPrediction hypo)
        {
            txtGlucoseTarget.Text = hypo.HypoGlucoseTarget.Text;
            txtGlucoseLast.Text = hypo.GlucoseLast.Text;
            txtGlucosePrevious.Text = hypo.GlucosePrevious.Text;
            txtHourLast.Text = hypo.HourLast.Text;
            txtHourPrevious.Text = hypo.HourPrevious.Text;
            txtMinuteLast.Text = hypo.MinuteLast.Text;
            txtMinutePrevious.Text = hypo.MinutePrevious.Text;
            txtGlucoseSlope.Text = hypo.GlucoseSlope.ToString();
        }

        private void FromUiToClass(HypoPrediction hypo)
        {
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
            DateTime? finalTime = hypo.PredictHypoTime();
            if (finalTime != null)
            {
                txtPredictedHour.Text = ((DateTime)finalTime).Hour.ToString();
                txtPredictedMinute.Text = ((DateTime)finalTime).Minute.ToString();
            }
            else
            {
                txtPredictedHour.Text = "XXXX";
                txtPredictedMinute.Text = "XXXX";
            }
            hypo.SaveData();
            txtGlucoseSlope.Text = hypo.GlucoseSlope.Text;
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

    }
}