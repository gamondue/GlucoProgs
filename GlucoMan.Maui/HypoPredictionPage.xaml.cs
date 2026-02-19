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

            // Get hour and minute from labels
            if (string.IsNullOrWhiteSpace(txtAlarmHour.Text) || txtAlarmHour.Text == "----" ||
                string.IsNullOrWhiteSpace(txtAlarmMinute.Text) || txtAlarmMinute.Text == "----")
            {
                await DisplayAlert(AppStrings.Warning, 
                    "Please calculate prediction first to get alarm time", 
                    AppStrings.OK);
                return;
            }

            if (!int.TryParse(txtAlarmHour.Text, out int hour) || 
                !int.TryParse(txtAlarmMinute.Text, out int minute))
            {
                await DisplayAlert(AppStrings.Error, "Invalid alarm time format", AppStrings.OK);
                return;
            }

            // Validate hour and minute
            if (hour < 0 || hour > 23 || minute < 0 || minute > 59)
            {
                await DisplayAlert(AppStrings.Error, 
                    $"Invalid time: {hour:D2}:{minute:D2}", 
                    AppStrings.OK);
                return;
            }

            // Create alarm datetime for today at the specified time
            var today = DateTime.Today;
            var alarmDateTime = new DateTime(today.Year, today.Month, today.Day, hour, minute, 0);

            // If the time is in the past, schedule for tomorrow
            if (alarmDateTime <= DateTime.Now)
            {
                bool setForTomorrow = await DisplayAlert(
                    AppStrings.Warning,
                    $"The alarm time ({hour:D2}:{minute:D2}) is in the past.\n\nSchedule for tomorrow?",
                    AppStrings.Yes,
                    AppStrings.No);

                if (setForTomorrow)
                {
                    alarmDateTime = alarmDateTime.AddDays(1);
                }
                else
                {
                    return;
                }
            }

            // Get glucose target for the reminder text
            string glucoseTarget = txtGlucoseTarget.Text;
            string reminderText = $"?? Hypo Alert!\nPredicted glucose below {glucoseTarget} mg/dL";

            // Create the alarm
            var alarm = new Alarm
            {
                ReminderText = reminderText,
                TimeStart = new gamon.DateTimeAndText { DateTime = alarmDateTime },
                EnablePlaySoundFile = true,  // Always play sound for hypo alerts
                DoVibrate = true,            // Always vibrate for hypo alerts
                ValidTimeAfterStart = TimeSpan.FromHours(2), // Valid for 2 hours
                Duration = TimeSpan.FromMinutes(10) // Ring up to 10 minutes
            };

            alarm.CalculateNextTriggerTime();

            // Save to database
            var blAlarms = new BL_Alarms();
            blAlarms.AddAlarm(alarm);

            // Schedule with system
            if (alarm.IdAlarm.HasValue)
            {
                await _alarmScheduler.ScheduleAsync(alarm);

                string message = $"?? Hypoglycemia alert set!\n\n" +
                                $"? Time: {alarmDateTime:dd/MM/yyyy HH:mm}\n" +
                                $"?? Target: {glucoseTarget} mg/dL\n\n" +
                                $"{AppStrings.AlarmWillAlertBeforePredictedHypoglycemia}";

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
