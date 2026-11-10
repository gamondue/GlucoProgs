using GlucoMan;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui;

public partial class HypoPredictionPage : ContentPage
{
    BL_HypoPrediction hypo;
    BL_GlucoseMeasurements blMeasurements = new BL_GlucoseMeasurements();
    private ISystemAlarmScheduler? _alarmScheduler;
    
    public HypoPredictionPage()
    {
        InitializeComponent();

        hypo = new BL_HypoPrediction();

        // Get alarm scheduler from DI
        try
        {
            _alarmScheduler = Application.Current?.Handler?.MauiContext?.Services
                .GetService<ISystemAlarmScheduler>();
        }
        catch (Exception ex)
        {
            gamon.General.LogOfProgram?.Error("HypoPredictionPage - Constructor - Getting alarm scheduler", ex);
        }

        hypo.RestoreData();
        FromClassToUi();

        txtGlucoseSlope.Text = "----";
        txtGlucoseLast.Focus();

        txtStatusBar.IsVisible = false;

        // Disable alarm button if user has continuous glucose sensor
        btnSetAlarm.IsVisible = !Common.CantSetAlarms;
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
        txtAlarmHour.Text = hypo.AlarmHour.Text;
        txtAlarmMinute.Text = hypo.AlarmMinute.Text;

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
    private async void btnAlarm_Click(object sender, EventArgs e)
    {
        try
        {
            // Check if alarm scheduler is available
            if (_alarmScheduler == null)
            {
                await DisplayAlert(AppStrings.Error, "Alarm system not available", AppStrings.OK);
                return;
            }

            // The alarm time exists only if the last prediction has produced one
            if (!int.TryParse(txtAlarmHour.Text, out int hour) ||
                !int.TryParse(txtAlarmMinute.Text, out int minute) ||
                hypo.AlarmTime.DateTime is not DateTime alarmDateTime)
            {
                await DisplayAlert(AppStrings.Warning, AppStrings.HypoAlarmTimeNotAvailable, AppStrings.OK);
                return;
            }

            // A prediction speaks of the next few hours: if its time has passed there is
            // nothing to wait for, and moving it to tomorrow would make no sense
            if (alarmDateTime <= DateTime.Now)
            {
                await DisplayAlert(AppStrings.Warning,
                    string.Format(AppStrings.HypoAlarmTimeAlreadyPassed, hour, minute),
                    AppStrings.OK);
                return;
            }

            var blAlarms = new BL_Alarms();

            // A new prediction replaces the previous one, whose alarm has no more meaning
            foreach (Alarm previousAlarm in blAlarms.GetTransientAlarms())
            {
                await _alarmScheduler.CancelAsync(previousAlarm.IdAlarm!.Value);
                blAlarms.DeleteAlarm(previousAlarm);
            }

            // Get glucose target for the reminder text
            string glucoseTarget = txtGlucoseTarget.Text;
            string reminderText = string.Format(AppStrings.HypoAlarmReminderFormat, glucoseTarget);

            // Create the alarm: it lives until the user dismisses it, so it has no
            // MaxRepeatCount and comes back until someone answers
            var alarm = new Alarm
            {
                ReminderText = reminderText,
                TimeStart = new gamon.DateTimeAndText { DateTime = alarmDateTime },
                IsTransient = true,
                EnablePlaySoundFile = true,  // Always play sound for hypo alerts
                DoVibrate = true,            // Always vibrate for hypo alerts
                StartupGraceWindow = TimeSpan.FromHours(2), // Fire even at program startup up to 2h after due
                Duration = TimeSpan.FromMinutes(10), // Ring up to 10 minutes
                RepetitionTime = TimeSpan.FromMinutes(Alarm.DefaultSnoozeMinutes)
            };

            alarm.CalculateNextTriggerTime();

            // Save to database
            blAlarms.AddAlarm(alarm);

            // Schedule with system
            if (alarm.IdAlarm.HasValue)
            {
                await _alarmScheduler.ScheduleAsync(alarm);

                string message = string.Format(AppStrings.AlarmSetForTime, hour, minute) + "\n\n" +
                                 AppStrings.AlarmWillAlertBeforePredictedHypoglycemia;

                await DisplayAlert(AppStrings.AlarmSet, message, AppStrings.OK);

                // Update status bar
                txtStatusBar.IsVisible = true;
                txtStatusBar.Text = string.Format(AppStrings.AlarmSetForTime, hour, minute);
                txtStatusBar.BackgroundColor = Colors.Green;
            }
            else
            {
                await DisplayAlert(AppStrings.Error, AppStrings.FailedToCreateAlarmNoId, AppStrings.OK);
            }
        }
        catch (Exception ex)
        {
            gamon.General.LogOfProgram?.Error("HypoPredictionPage - btnAlarm_Click", ex);
            await DisplayAlert(AppStrings.Error, 
                string.Format(AppStrings.FailedToSetAlarm, ex.Message), 
                AppStrings.OK);
        }
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
