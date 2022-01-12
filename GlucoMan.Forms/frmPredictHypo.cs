using System;
using System.Collections.Generic;
using System.Windows.Forms;
using GlucoMan;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Forms
{
    public partial class frmPredictHypo : Form
    {
        BL_HypoPrediction hypo;
        BL_GlucoseMeasurements blMeasurements = new BL_GlucoseMeasurements();

        public frmPredictHypo()
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
            txtGlucoseTarget.Text = hypo.Hypo_GlucoseTarget.Text;
            txtGlucoseLast.Text = hypo.Hypo_GlucoseLast.Text;
            txtGlucosePrevious.Text = hypo.Hypo_GlucosePrevious.Text;
            txtHourLast.Text = hypo.Hypo_HourLast.Text;
            
            txtHourPrevious.Text = hypo.Hypo_HourPrevious.Text;
            txtMinuteLast.Text = hypo.Hypo_MinuteLast.Text;
            txtMinutePrevious.Text = hypo.Hypo_MinutePrevious.Text;
            
            txtAlarmAdvanceTime.Text = hypo.Hypo_AlarmAdvanceTime.TotalMinutes.ToString();
            txtGlucoseSlope.Text = hypo.HypoGlucoseSlope.ToString();
            txtAlarmHour.Text = hypo.HypoAlarmTime.DateTime.Hour.ToString();
            txtAlarmMinute.Text = hypo.HypoAlarmTime.DateTime.Minute.ToString();

            txtPredictedHour.Text = hypo.PredictedHour.Text;
            txtPredictedMinute.Text = hypo.PredictedMinute.Text;

            if (hypo.StatusMessage != null && hypo.StatusMessage != "")
            {
                txtStatusBar.Visible = true;
                txtStatusBar.Text = hypo.StatusMessage;
            }
            else
                txtStatusBar.Visible = false;
        }
        private void FromUiToClass()
        {
            hypo.Hypo_AlarmAdvanceTime = new TimeSpan(0, int.Parse(txtAlarmAdvanceTime.Text), 0);
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
            //txtGlucoseSlope.Text = hypo.HypoGlucoseSlope.Text; 
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
        private void txtGlucoseLast_Leave(object sender, EventArgs e)
        {
            btnNow_Click(null, null);
        }
    }
}
