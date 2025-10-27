using GlucoMan.BusinessLayer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GlucoMan.Maui;

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
        txtGlucoseTarget.Text = hypo.GlucoseTarget.Text;
        txtGlucoseLast.Text = hypo.GlucoseLast.Text;
        txtGlucosePrevious.Text = hypo.GlucosePrevious.Text;
        txtHourLast.Text = hypo.HourLast.Text;

        txtHourPrevious.Text = hypo.HourPrevious.Text;
        txtMinuteLast.Text = hypo.MinuteLast.Text;
        txtMinutePrevious.Text = hypo.MinutePrevious.Text;

        txtAlarmAdvanceTime.Text = hypo.AlarmAdvanceTime.Text;
        txtGlucoseSlope.Text = hypo.GlucoseSlope.Text;
        DateTime dummy = (DateTime)hypo.AlarmTime.DateTime;
        txtAlarmHour.Text = dummy.Hour.ToString();
        txtAlarmMinute.Text = dummy.Minute.ToString();

        if (hypo.FutureTime.DateTime != null && hypo.FutureTime.DateTime !=
                new DateTime(0001, 01, 01))
        {
            dtpTimeFutureDate.Date = (DateTime)hypo.FutureTime.DateTime;
            dtpTimeFutureTime.Time = ((DateTime)hypo.FutureTime.DateTime).TimeOfDay;
        }
        txtPredictedHour.Text = hypo.PredictedHour.Text;
        txtPredictedMinute.Text = hypo.PredictedMinute.Text;
        if (hypo.StatusMessage != null && hypo.StatusMessage != "")
        {
            txtStatusBar.IsVisible = true;
            txtStatusBar.Text = hypo.StatusMessage;
        }
        else
            txtStatusBar.IsVisible = false;

        txtFutureTimeMinutes.Text = hypo.FutureSpanMinutes.Text;
        txtFutureGlucose.Text = hypo.PredictedGlucose.Text;
    }
    private void FromUiToClass()
    {
        hypo.AlarmAdvanceTime.Text = txtGlucoseTarget.Text;
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
        btnNow_Click(null, null);
        txtGlucoseLast.Focus();
    }
    private void btnAlarm_Click(object sender, EventArgs e)
    {

    }
    private void btnReadGlucose_Click(object sender, EventArgs e)
    {
        List<GlucoseRecord> list = blMeasurements.GetLastTwoGlucoseMeasurements();
        if (list.Count > 1)
        {
            txtGlucoseLast.Text = list[0].GlucoseValue.ToString();
            txtGlucosePrevious.Text = list[1].GlucoseValue.ToString();
            txtHourLast.Text = list[0].EventTime.DateTime?.Hour.ToString();
            txtHourPrevious.Text = list[1].EventTime.DateTime?.Hour.ToString();
            txtMinuteLast.Text = list[0].EventTime.DateTime?.Minute.ToString();
            txtMinutePrevious.Text = list[1].EventTime.DateTime?.Minute.ToString();
        }
    }
    private void btnCalcFutureGlucose_Click(object sender, EventArgs e)
    {
        FromUiToClass();
        hypo.PredictHypoTime();
        hypo.PredictGlucose();
        FromClassToUi();
    }
}
