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
        ShowForAllUsers = true)]
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
            int btnDismissId = GetResourceId("btnDismiss", "id");
            int btnSnoozeId = GetResourceId("btnSnooze", "id");

            var txtTitle = FindViewById<TextView>(txtTitleId);
            var txtMessage = FindViewById<TextView>(txtMessageId);
            var btnDismiss = FindViewById<global::Android.Widget.Button>(btnDismissId);
            var btnSnooze = FindViewById<global::Android.Widget.Button>(btnSnoozeId);

            if (txtTitle != null)
                txtTitle.Text = AppStrings.GlucoManAlarm;
            
            if (txtMessage != null)
                txtMessage.Text = _reminderText;

            if (btnDismiss != null)
                btnDismiss.Click += BtnDismiss_Click;

            if (btnSnooze != null)
                btnSnooze.Click += BtnSnooze_Click;

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
                _mediaPlayer?.Stop();
                _mediaPlayer?.Release();
                _mediaPlayer = null;
            }
            catch { }
        }

        private void StopVibration()
        {
            try
            {
                _vibrator?.Cancel();
                _vibrator = null;
            }
            catch { }
        }

        private void BtnDismiss_Click(object? sender, EventArgs e)
        {
            try
            {
                // Mark alarm as dismissed in database
                UpdateAlarmStatus(dismissed: true);
                
                StopAlarmSound();
                StopVibration();
                Finish();
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error dismissing alarm: {ex.Message}");
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

        private void UpdateAlarmStatus(bool dismissed)
        {
            try
            {
                if (Common.Database != null && _alarmId > 0)
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
                if (Common.Database != null && _alarmId > 0)
                {
                    // Create a new temporary alarm for snooze
                    var snoozeAlarm = new Alarm
                    {
                        ReminderText = $"Snooze: {_reminderText}",
                        TimeStart = new gamon.DateTimeAndText { DateTime = DateTime.Now.AddMinutes(minutes) },
                        EnablePlaySoundFile = _shouldPlaySound,
                        DoVibrate = _shouldVibrate,
                        SoundFilePath = _soundPath,
                        ValidTimeAfterStart = TimeSpan.FromMinutes(10)
                    };
                    snoozeAlarm.CalculateNextTriggerTime();

                    var blAlarms = new BL_Alarms();
                    blAlarms.AddAlarm(snoozeAlarm);

                    if (snoozeAlarm.IdAlarm.HasValue)
                    {
                        var scheduler = new SystemAlarmScheduler();
                        scheduler.ScheduleAsync(snoozeAlarm);
                    }
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"ScheduleSnooze error: {ex.Message}");
            }
        }

        protected override void OnDestroy()
        {
            StopAlarmSound();
            StopVibration();
            
            _wakeLock?.Release();
            _wakeLock = null;

            base.OnDestroy();
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
