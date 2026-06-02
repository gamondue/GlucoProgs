#if ANDROID
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Platforms.Android
{
    [Activity(
        Label = "GlucoMan Alarm",
        Theme = "@style/Theme.AppCompat.NoActionBar",
        LaunchMode = global::Android.Content.PM.LaunchMode.SingleInstance,
        ExcludeFromRecents = true,
        ShowForAllUsers = true,
        Name = "GlucoMan.Maui.Platforms.Android.AlarmActivity")]
    public class AlarmActivity : AppCompatActivity
    {
        private MediaPlayer? _mediaPlayer;
        private Vibrator? _vibrator;
        private int _alarmId;
        private string _reminderText = "";
        private bool _shouldVibrate;
        private bool _shouldPlaySound;
        private string? _soundPath;
        private PowerManager.WakeLock? _wakeLock;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Make this activity show over lock screen and turn on screen
            if (Build.VERSION.SdkInt >= BuildVersionCodes.OMr1)
            {
                SetShowWhenLocked(true);
                SetTurnScreenOn(true);
                KeyguardManager? keyguardManager = (KeyguardManager?)GetSystemService(KeyguardService);
                keyguardManager?.RequestDismissKeyguard(this, null);
            }
            else
            {
#pragma warning disable CS0618 // Tipo o membro obsoleto
                Window?.AddFlags(WindowManagerFlags.ShowWhenLocked);
                Window?.AddFlags(WindowManagerFlags.DismissKeyguard);
                Window?.AddFlags(WindowManagerFlags.KeepScreenOn);
                Window?.AddFlags(WindowManagerFlags.TurnScreenOn);
#pragma warning restore CS0618
            }

            // Acquire wake lock to keep CPU running
            PowerManager? powerManager = (PowerManager?)GetSystemService(PowerService);
            _wakeLock = powerManager?.NewWakeLock(WakeLockFlags.Partial | WakeLockFlags.AcquireCausesWakeup, "GlucoMan::AlarmWakeLock");
            _wakeLock?.Acquire(10 * 60 * 1000L); // 10 minutes max

            // Get alarm data from intent
            _alarmId = Intent?.GetIntExtra("AlarmId", 0) ?? 0;
            _reminderText = Intent?.GetStringExtra("ReminderText") ?? "Alarm";
            _shouldVibrate = Intent?.GetBooleanExtra("Vibrate", false) ?? false;
            _shouldPlaySound = Intent?.GetBooleanExtra("PlaySound", true) ?? true;
            _soundPath = Intent?.GetStringExtra("SoundPath");

            SetContentView(GlucoMan.Maui.Resource.Layout.alarm_activity);

            // Setup UI - Use reflection to get resource IDs
            int txtTitleId = GetResourceId("txtAlarmTitle", "id");
            int txtMessageId = GetResourceId("txtAlarmMessage", "id");
            int txtInstructionsId = GetResourceId("txtInstructions", "id");
            int btnStopId = GetResourceId("btnStop", "id");
            int btnSuspendId = GetResourceId("btnSuspend", "id");
            int btnSnoozeId = GetResourceId("btnSnooze", "id");
            int btnSnooze15Id = GetResourceId("btnSnooze15", "id");

            var txtTitle = FindViewById<TextView>(txtTitleId);
            var txtMessage = FindViewById<TextView>(txtMessageId);
            var txtInstructions = FindViewById<TextView>(txtInstructionsId);
            var btnStop = FindViewById<global::Android.Widget.Button>(btnStopId);
            var btnSuspend = FindViewById<global::Android.Widget.Button>(btnSuspendId);
            var btnSnooze = FindViewById<global::Android.Widget.Button>(btnSnoozeId);
            var btnSnooze15 = FindViewById<global::Android.Widget.Button>(btnSnooze15Id);

            if (txtTitle != null)
                txtTitle.Text = AppStrings.GlucoManAlarm;

            if (txtMessage != null)
                txtMessage.Text = _reminderText;

            if (txtInstructions != null)
                txtInstructions.Text = AppStrings.AlarmSwipeInstructions;

            if (btnStop != null)
            {
                btnStop.Text = AppStrings.AlarmStopButtonShort;
                btnStop.Click += BtnStop_Click;
            }

            if (btnSuspend != null)
            {
                btnSuspend.Text = AppStrings.AlarmSuspendButtonShort;
                btnSuspend.Click += BtnSuspend_Click;
            }

            if (btnSnooze != null)
            {
                btnSnooze.Text = AppStrings.AlarmSnoozeButtonShort;
                btnSnooze.Click += BtnSnooze_Click;
            }

            if (btnSnooze15 != null)
            {
                btnSnooze15.Text = "Snooze\n15 min";
                btnSnooze15.Click += BtnSnooze15_Click;
            }

            // Start sound and vibration
            StartAlarmSound();
            StartVibration();
        }

        private void StartAlarmSound()
        {
            if (!_shouldPlaySound) return;

            try
            {
                _mediaPlayer = new MediaPlayer();

                // Try custom sound first
                if (!string.IsNullOrEmpty(_soundPath) && File.Exists(_soundPath))
                {
                    _mediaPlayer.SetDataSource(_soundPath);
                }
                else
                {
                    // Use default alarm sound
                    var alarmUri = RingtoneManager.GetDefaultUri(RingtoneType.Alarm);
                    if (alarmUri == null)
                        alarmUri = RingtoneManager.GetDefaultUri(RingtoneType.Notification);
                    
                    if (alarmUri != null)
                        _mediaPlayer.SetDataSource(this, alarmUri);
                }

                _mediaPlayer.SetAudioAttributes(
                    new AudioAttributes.Builder()
                        .SetUsage(AudioUsageKind.Alarm)
                        .SetContentType(AudioContentType.Music)
                        .Build()
                );

                _mediaPlayer.Looping = true;
                _mediaPlayer.SetVolume(1.0f, 1.0f); // Max volume
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error starting alarm sound: {ex.Message}");
            }
        }

        private void StartVibration()
        {
            if (!_shouldVibrate) return;

            try
            {
                _vibrator = (Vibrator?)GetSystemService(VibratorService);
                
                if (_vibrator != null && _vibrator.HasVibrator)
                {
                    // Vibration pattern: wait 0ms, vibrate 1000ms, wait 500ms, vibrate 1000ms, repeat
                    long[] pattern = { 0, 1000, 500, 1000, 500 };

                    if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                    {
                        _vibrator.Vibrate(VibrationEffect.CreateWaveform(pattern, 0));
                    }
                    else
                    {
#pragma warning disable CS0618
                        _vibrator.Vibrate(pattern, 0);
#pragma warning restore CS0618
                    }
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error starting vibration: {ex.Message}");
            }
        }

        private void StopAlarmSound()
        {
            try
            {
                if (_mediaPlayer != null)
                {
                    if (_mediaPlayer.IsPlaying)
                    {
                        _mediaPlayer.Stop();
                    }
                    _mediaPlayer.Release();
                    _mediaPlayer = null;
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error stopping alarm sound: {ex.Message}");
            }
        }

        private void StopVibration()
        {
            try
            {
                if (_vibrator != null)
                {
                    _vibrator.Cancel();
                    _vibrator = null;
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error stopping vibration: {ex.Message}");
            }
        }

        private void BtnStop_Click(object? sender, EventArgs e)
        {
            try
            {
                global::System.Diagnostics.Debug.WriteLine("BtnStop_Click: Stopping alarm sound...");

                // Stop sound and vibration
                StopAlarmSound();
                StopVibration();

                // Mark alarm as triggered (keeps it scheduled for next time)
                UpdateAlarmStatus(dismissed: false);

                // Finish the activity
                global::System.Diagnostics.Debug.WriteLine("BtnStop_Click: Finishing activity...");
                Finish();
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error stopping alarm: {ex.Message}");
                // Force stop even if there's an error
                try
                {
                    StopAlarmSound();
                    StopVibration();
                    Finish();
                }
                catch { }
            }
        }

        private void BtnSuspend_Click(object? sender, EventArgs e)
        {
            try
            {
                global::System.Diagnostics.Debug.WriteLine("BtnSuspend_Click: Suspending alarm...");

                // Stop sound and vibration FIRST
                StopAlarmSound();
                StopVibration();

                // Then mark alarm as dismissed in database
                UpdateAlarmStatus(dismissed: true);

                // Finish the activity
                global::System.Diagnostics.Debug.WriteLine("BtnSuspend_Click: Finishing activity...");
                Finish();
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error suspending alarm: {ex.Message}");
                // Force stop even if there's an error
                try
                {
                    StopAlarmSound();
                    StopVibration();
                    Finish();
                }
                catch { }
            }
        }

        private void BtnSnooze_Click(object? sender, EventArgs e)
        {
            try
            {
                // Schedule alarm again in 5 minutes
                ScheduleSnooze(5);

                StopAlarmSound();
                StopVibration();
                Finish();
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error snoozing alarm: {ex.Message}");
            }
        }

        private void BtnSnooze15_Click(object? sender, EventArgs e)
        {
            try
            {
                // Schedule alarm again in 15 minutes
                ScheduleSnooze(15);

                StopAlarmSound();
                StopVibration();
                Finish();
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error snoozing alarm for 15 min: {ex.Message}");
            }
        }

        private void UpdateAlarmStatus(bool dismissed)
        {
            try
            {
                if (DatabaseService.Instance.Database != null && _alarmId > 0)
                {
                    var blAlarms = new BL_Alarms();
                    var alarms = blAlarms.GetAllAlarms(all: true);
                    var alarm = alarms.FirstOrDefault(a => a.IdAlarm == _alarmId);

                    if (alarm != null)
                    {
                        if (dismissed)
                        {
                            alarm.Dismiss();
                        }
                        else
                        {
                            alarm.MarkAsTriggered();
                        }
                        blAlarms.AddAlarm(alarm);
                    }
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"UpdateAlarmStatus error: {ex.Message}");
            }
        }

        private void ScheduleSnooze(int minutes)
        {
            try
            {
                var snoozeAlarm = new Alarm
                {
                    IdAlarm = -1, // in-memory only, no DB
                    ReminderText = _reminderText,
                    TimeStart = new gamon.DateTimeAndText { DateTime = DateTime.Now.AddMinutes(minutes) },
                    EnablePlaySoundFile = _shouldPlaySound,
                    DoVibrate = _shouldVibrate,
                    SoundFilePath = _soundPath,
                    StartupGraceWindow = TimeSpan.FromMinutes(10)
                };
                snoozeAlarm.CalculateNextTriggerTime();

                var scheduler = new SystemAlarmScheduler();
                scheduler.ScheduleAsync(snoozeAlarm);
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"ScheduleSnooze error: {ex.Message}");
            }
        }

        protected override void OnDestroy()
        {
            global::System.Diagnostics.Debug.WriteLine("AlarmActivity OnDestroy: Cleaning up...");

            StopAlarmSound();
            StopVibration();

            try
            {
                _wakeLock?.Release();
                _wakeLock = null;
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error releasing wake lock: {ex.Message}");
            }

            base.OnDestroy();
        }

        protected override void OnPause()
        {
            base.OnPause();

            // If activity is finishing, stop sound
            if (IsFinishing)
            {
                global::System.Diagnostics.Debug.WriteLine("AlarmActivity OnPause: Activity finishing, stopping sound...");
                StopAlarmSound();
                StopVibration();
            }
        }

        public override void OnBackPressed()
        {
            // Disable back button - user must dismiss or snooze
            // Don't call base.OnBackPressed();
        }

        private int GetResourceId(string name, string type)
        {
            try
            {
                return Resources?.GetIdentifier(name, type, PackageName) ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}
#endif
