using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GlucoMan;
using GlucoMan.BusinessLayer;

namespace GlucoMan_Forms_Core
{
    public partial class frmPredictHypo : Form
    {
        Bl_HypoPrediction hypo;
        BL_GlucoseMeasurements blMeasurements = new BL_GlucoseMeasurements();

        public frmPredictHypo()
        {
            InitializeComponent();

            hypo = new Bl_HypoPrediction();

            hypo.RestoreData();
            FromClassToUi();
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
            txtAlarmHour.Text = hypo.AlarmTime.DateTime.Hour.ToString();
            txtAlarmMinute.Text = hypo.AlarmTime.DateTime.Minute.ToString(); 
        }
        private void FromUiToClass()
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
        private void txtGlucoseLast_Leave(object sender, EventArgs e)
        {
            btnNow_Click(null, null); 
        }
        private void btnPredict_Click(object sender, EventArgs e)
        {
            FromUiToClass();
            hypo.PredictHypoTime();
            DateTime? finalTime = hypo.PredictedTime.DateTime;
            DateTime? alarmTime = hypo.AlarmTime.DateTime;
            if (finalTime != null)
            {
                txtPredictedHour.Text = ((DateTime)finalTime).Hour.ToString();
                txtPredictedMinute.Text = ((DateTime)finalTime).Minute.ToString();

                txtAlarmHour.Text = ((DateTime)alarmTime).Hour.ToString();
                txtAlarmMinute.Text = ((DateTime)alarmTime).Minute.ToString();
            }
            else
            {
                Console.Beep(200,40);
                txtPredictedHour.Text = "XXXX";
                txtPredictedMinute.Text = "XXXX";

                txtAlarmHour.Text = "XXXX";
                txtAlarmMinute.Text = "XXXX";
            }
            txtGlucoseSlope.Text = hypo.GlucoseSlope.Text; 
        }
        private void frmPredictHypo_Load(object sender, EventArgs e)
        {
            txtGlucoseLast.Focus(); 
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
        private void frmPredictHypo_FormClosing(object sender, FormClosingEventArgs e)
        {
            FromUiToClass(); 
            hypo.SaveData(); 
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
        private void btnPaste_Click(object sender, EventArgs e)
        {
            //////Control c = SharedWinForms.Methods.FindFocusedControl(this);
            //////try
            //////{
            //////    c.Text = Clipboard.GetText();
            //////}
            //////catch (Exception ex)
            //////{ }
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
    }
}
