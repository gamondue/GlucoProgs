using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmHypoTimePrediction : Form
    {
        BL_HypoPrediction hypo;
        BL_GlucoseMeasurements blMeasurements = new BL_GlucoseMeasurements();

        public frmHypoTimePrediction()
        {
            InitializeComponent();

            hypo = new BL_HypoPrediction();

            hypo.RestoreData();
            FromClassToUi();

            txtGlucoseSlope.Text = "----";
            txtGlucoseLast.Focus();

            txtStatusBar.Visible = false;
        }
        private void frmPredictHypo_Load(object sender, EventArgs e)
        {
            txtGlucoseLast.Focus();
        }
        private void frmPredictHypo_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass();
            hypo.SaveDataHypo();
        }
        private void FromClassToUi()
        {
            txtGlucoseTarget.Text = hypo.GlucoseTarget.Text;
            txtGlucoseLast.Text = hypo.GlucoseLast.Text;
            txtGlucosePrevious.Text = hypo.GlucosePrevious.Text;
            txtHourLast.Text = hypo.HourLast.Text;

            txtHourPrevious.Text = hypo.HourPrevious.Text;
            txtMinuteLast.Text = hypo.MinuteLast.Text;
            txtMinutePrevious.Text = hypo.MinutePrevious.Text;

            txtAlarmAdvanceTime.Text = hypo.AlarmAdvanceTime.Text;
            txtGlucoseSlope.Text = hypo.GlucoseSlope.ToString();
            DateTime dummy = (DateTime)hypo.AlarmTime.DateTime;
            txtAlarmHour.Text = dummy.Hour.ToString();
            txtAlarmMinute.Text = dummy.Minute.ToString();

            txtPredictedHour.Text = hypo.PredictedHour.Text;
            txtPredictedMinute.Text = hypo.PredictedMinute.Text;
            if (hypo.StatusMessage != null && hypo.StatusMessage != "")
            {
                txtStatusBar.Visible = true;
                txtStatusBar.Text = hypo.StatusMessage;
            }
            else
                txtStatusBar.Visible = false;

            txtAlarmAdvanceTime.Text = hypo.AlarmAdvanceTime.Text;
            if (hypo.FutureTime.DateTime != null && hypo.FutureTime.DateTime !=
                new DateTime(0001, 01, 01))
                dtpFutureTime.Value = (DateTime)hypo.FutureTime.DateTime;
            txtFutureTimeMinutes.Text = hypo.FutureSpanMinutes.Text;
            txtFutureGlucose.Text = hypo.PredictedGlucose.Text;
        }
        private void FromUiToClass()
        {
            hypo.AlarmAdvanceTime.Text = txtAlarmAdvanceTime.Text;
            hypo.GlucoseTarget.Text = txtGlucoseTarget.Text;
            hypo.GlucoseLast.Text = txtGlucoseLast.Text;
            hypo.GlucosePrevious.Text = txtGlucosePrevious.Text;

            hypo.HourLast.Text = txtHourLast.Text;
            hypo.MinuteLast.Text = txtMinuteLast.Text;

            hypo.HourPrevious.Text = txtHourPrevious.Text;
            hypo.MinutePrevious.Text = txtMinutePrevious.Text;

            hypo.AlarmAdvanceTime.Text = txtAlarmAdvanceTime.Text; 

            hypo.FutureSpanMinutes.Text = txtFutureTimeMinutes.Text;
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
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            txtGlucosePrevious.Text = txtGlucoseLast.Text;
            txtHourPrevious.Text = txtHourLast.Text;
            txtMinutePrevious.Text = txtMinuteLast.Text;

            txtGlucoseLast.Text = ""; 
            btnNow_Click(null,null);
            txtGlucoseLast.Focus();
        }
        private void btnSetAlarm_Click(object sender, EventArgs e)
        {
            if (btnSetAlarm.Text != "Stop")
            {
                hypo.SetAlarm();
                if (hypo.AlarmIsSet)
                    btnSetAlarm.Text = "Stop";
            }
            else
            {
                hypo.StopAlarm();
                btnSetAlarm.Text = "Alarm";
            }
        }
        private void btnReadGlucose_Click(object sender, EventArgs e)
        {
            List<GlucoseRecord> list = blMeasurements.GetLastTwoGlucoseMeasurements();
            txtGlucoseLast.Text = list[0].GlucoseValue.ToString();
            txtGlucosePrevious.Text = list[1].GlucoseValue.ToString();
            txtHourLast.Text = list[0].Timestamp.Value.Hour.ToString();
            txtHourPrevious.Text = list[1].Timestamp.Value.Hour.ToString();
            txtMinuteLast.Text = list[0].Timestamp.Value.Minute.ToString();
            txtMinutePrevious.Text = list[1].Timestamp.Value.Minute.ToString();
        }
        private void txtGlucoseLast_Leave(object sender, EventArgs e)
        {
            btnNow_Click(null, null);
        }
        private void btnCalcFutureGlucc_Click(object sender, EventArgs e)
        {
            FromUiToClass(); 
            hypo.PredictHypoTime();
            hypo.PredictGlucose(); 
            FromClassToUi(); 
        }
    }
}
