#if ANDROID
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Platforms.Android
{
    /// <summary>
    /// Makes the device ring until the user answers, or until the Duration of the alarm elapses.
    /// Android doesn't let an app in the background open a page while the device is in use, so
    /// the ringing can't depend on AlarmActivity: this service owns sound and vibration, while its
    /// notification brings the alarm to the user: full screen if the device is locked, otherwise
    /// as a banner that has the buttons of the most common answers and opens AlarmActivity.
    /// </summary>
    [Service(
        ForegroundServiceType = ForegroundService.TypeMediaPlayback,
        Exported = false,
        Name = "GlucoMan.Maui.Platforms.Android.AlarmRingService")]
    public class AlarmRingService : Service
    {
        public const string ActionRing = "GlucoMan.Maui.Alarm.RING";
        public const string ActionStop = "GlucoMan.Maui.Alarm.STOP";
        public const string ActionDismiss = "GlucoMan.Maui.Alarm.DISMISS";
        public const string ActionSnooze = "GlucoMan.Maui.Alarm.SNOOZE";
        public const string ExtraSnoozeMinutes = "SnoozeMinutes";

        private const string ChannelId = "glucoman_alarm_ringing";
        private const int NotificationId = 999998;
        private const long MaxWakeLockMilliseconds = 10 * 60 * 1000L;

        /// <summary>
        /// Raised when the alarm doesn't ring anymore, so that AlarmActivity can close.
        /// </summary>
        public static event Action? RingingEnded;

        /// <summary>
        /// True while the device is ringing because of this service.
        /// </summary>
        public static bool IsRinging { get; private set; }

        private MediaPlayer? _mediaPlayer;
        private Vibrator? _vibrator;
        private PowerManager.WakeLock? _wakeLock;
        private readonly Handler _handler = new Handler(Looper.MainLooper!);
        private bool _isRinging;
        private int _alarmId;
        private string _reminderText = "";
        private bool _shouldVibrate;
        private bool _shouldPlaySound;
        private string? _soundPath;

        /// <summary>
        /// Creates the intent that asks this service to do one of its actions on an alarm.
        /// </summary>
        public static Intent CreateIntent(Context context, string action, int alarmId, string reminderText,
            bool vibrate, bool playSound, string? soundPath)
        {
            var intent = new Intent(context, typeof(AlarmRingService));
            intent.SetAction(action);
            intent.PutExtra("AlarmId", alarmId);
            intent.PutExtra("ReminderText", reminderText);
            intent.PutExtra("Vibrate", vibrate);
            intent.PutExtra("PlaySound", playSound);
            intent.PutExtra("SoundPath", soundPath ?? "");
            return intent;
        }

        public override IBinder? OnBind(Intent? intent) => null;

        public override StartCommandResult OnStartCommand(Intent? intent, StartCommandFlags flags, int startId)
        {
            if (intent == null)
            {
                StopSelf();
                return StartCommandResult.NotSticky;
            }
            switch (intent.Action)
            {
                case ActionRing:
                    Ring(intent);
                    break;
                case ActionStop:
                    Answer(intent, () => Reschedule(new BL_Alarms().StopAlarm(_alarmId)));
                    break;
                case ActionDismiss:
                    Answer(intent, () => new BL_Alarms().DismissAlarm(_alarmId));
                    break;
                case ActionSnooze:
                    int minutes = intent.GetIntExtra(ExtraSnoozeMinutes, Alarm.DefaultSnoozeMinutes);
                    Answer(intent, () => Snooze(minutes));
                    break;
                default:
                    StopSelf();
                    break;
            }
            return StartCommandResult.NotSticky;
        }

        private void ReadExtras(Intent intent)
        {
            _alarmId = intent.GetIntExtra("AlarmId", 0);
            _reminderText = intent.GetStringExtra("ReminderText") ?? "Alarm";
            _shouldVibrate = intent.GetBooleanExtra("Vibrate", false);
            _shouldPlaySound = intent.GetBooleanExtra("PlaySound", true);
            _soundPath = intent.GetStringExtra("SoundPath");
        }

        private void Ring(Intent intent)
        {
            // Only one alarm can ring: the one already ringing will come back later
            if (_isRinging)
            {
                SilenceDevice();
                UpdateAlarm(() => Reschedule(new BL_Alarms().RestartAlarmNotDismissed(_alarmId)));
            }
            ReadExtras(intent);

            // Android requires this to be the first thing that a foreground service does
            Alarm? alarm = ReadAlarm();
            try
            {
                StartForeground(NotificationId,
                    BuildNotification(alarm?.GetSnoozeMinutes() ?? Alarm.DefaultSnoozeMinutes),
                    ForegroundService.TypeMediaPlayback);
            }
            catch (Exception ex)
            {
                // Without the service the alarm can only be a normal notification
                gamon.General.LogOfProgram?.Error($"AlarmRingService - starting alarm {_alarmId}", ex);
                AlarmFireReceiver.ShowNotification(this, _alarmId, _reminderText, _shouldVibrate, _shouldPlaySound, _soundPath);
                StopSelf();
                return;
            }

            _isRinging = true;
            IsRinging = true;
            AcquireWakeLock();
            StartAlarmSound();
            StartVibration();

            // An alarm without answer stops after Duration and restarts later
            double durationMilliseconds = alarm?.Duration?.TotalMilliseconds ?? 0;
            if (durationMilliseconds > 0)
                _handler.PostDelayed(OnDurationElapsed, (long)durationMilliseconds);
        }

        private void OnDurationElapsed()
        {
            SilenceDevice();
            UpdateAlarm(() => Reschedule(new BL_Alarms().RestartAlarmNotDismissed(_alarmId)));
            EndService();
        }

        /// <summary>
        /// Silences the device, does what the answer of the user requires and ends the service.
        /// </summary>
        private void Answer(Intent intent, Action updateAlarm)
        {
            SilenceDevice();
            ReadExtras(intent);
            UpdateAlarm(updateAlarm);
            EndService();
        }

        private void UpdateAlarm(Action updateAlarm)
        {
            try
            {
                updateAlarm();
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error($"AlarmRingService - updating alarm {_alarmId}", ex);
            }
        }

        private void EndService()
        {
            RingingEnded?.Invoke();
            StopForeground(StopForegroundFlags.Remove);
            StopSelf();
        }

        private void Snooze(int minutes)
        {
            Alarm? saved = null;
            try
            {
                saved = _alarmId > 0 ? new BL_Alarms().SnoozeAlarm(_alarmId, minutes) : null;
            }
            catch (Exception ex)
            {
                // The alarm must come back even if the database has a problem
                gamon.General.LogOfProgram?.Error($"AlarmRingService - saving the snooze of alarm {_alarmId}", ex);
            }
            Reschedule(saved ?? CreateAlarmNotInDatabase(minutes));
        }

        /// <summary>
        /// The test alarm isn't saved in the database, so its snooze lives only in the scheduler.
        /// </summary>
        private Alarm CreateAlarmNotInDatabase(int minutes)
        {
            var alarm = new Alarm
            {
                IdAlarm = _alarmId,
                ReminderText = _reminderText,
                TimeStart = new gamon.DateTimeAndText { DateTime = DateTime.Now.AddMinutes(minutes) },
                EnablePlaySoundFile = _shouldPlaySound,
                DoVibrate = _shouldVibrate,
                SoundFilePath = _soundPath
            };
            alarm.CalculateNextTriggerTime();
            return alarm;
        }

        private static void Reschedule(Alarm? alarm)
        {
            if (alarm != null)
                new SystemAlarmScheduler().ScheduleAsync(alarm);
        }

        private Alarm? ReadAlarm()
        {
            try
            {
                if (_alarmId > 0 && DatabaseService.Instance.Database != null)
                    return new BL_Alarms().GetAlarmById(_alarmId);
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error($"AlarmRingService - reading alarm {_alarmId}", ex);
            }
            return null;
        }

        private Notification BuildNotification(int snoozeMinutes)
        {
            CreateNotificationChannel();

            var pageIntent = PendingIntent.GetActivity(
                this,
                _alarmId,
                AlarmActivity.CreateIntent(this, _alarmId, _reminderText, _shouldVibrate, _shouldPlaySound, _soundPath),
                PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

            string snoozeText = AppStrings.AlarmSnoozeButtonShort + " " +
                string.Format(AppStrings.AlarmSnoozeMinutesFormat, snoozeMinutes);

            return new NotificationCompat.Builder(this, ChannelId)
                .SetContentTitle(AppStrings.GlucoManAlarm)
                .SetContentText(_reminderText)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(_reminderText))
                .SetSmallIcon(global::Android.Resource.Drawable.IcLockIdleAlarm)
                .SetPriority((int)NotificationPriority.Max)
                .SetCategory(NotificationCompat.CategoryAlarm)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetForegroundServiceBehavior(NotificationCompat.ForegroundServiceImmediate)
                .SetOngoing(true)
                .SetContentIntent(pageIntent)
                .SetFullScreenIntent(pageIntent, true)
                .AddAction(0, snoozeText, CreateActionIntent(ActionSnooze, snoozeMinutes))
                .AddAction(0, AppStrings.AlarmStopButtonShort, CreateActionIntent(ActionStop, snoozeMinutes))
                .Build()!;
        }

        private PendingIntent CreateActionIntent(string action, int snoozeMinutes)
        {
            var intent = CreateIntent(this, action, _alarmId, _reminderText, _shouldVibrate, _shouldPlaySound, _soundPath);
            intent.PutExtra(ExtraSnoozeMinutes, snoozeMinutes);
            return PendingIntent.GetService(this, _alarmId, intent,
                PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent)!;
        }

        private void CreateNotificationChannel()
        {
            // Silent channel: the sound and the vibration are made by this service
            var channel = new NotificationChannel(ChannelId, AppStrings.GlucoManAlarm, NotificationImportance.High);
            channel.SetSound(null, null);
            channel.EnableVibration(false);
            channel.LockscreenVisibility = NotificationVisibility.Public;
            var manager = (NotificationManager?)GetSystemService(NotificationService);
            manager?.CreateNotificationChannel(channel);
        }

        private void AcquireWakeLock()
        {
            // Keeps the CPU running during the ringing
            PowerManager? powerManager = (PowerManager?)GetSystemService(PowerService);
            _wakeLock = powerManager?.NewWakeLock(WakeLockFlags.Partial, "GlucoMan::AlarmWakeLock");
            _wakeLock?.Acquire(MaxWakeLockMilliseconds);
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
                        .SetUsage(AudioUsageKind.Alarm)!
                        .SetContentType(AudioContentType.Music)!
                        .Build()!
                );

                _mediaPlayer.Looping = true;
                _mediaPlayer.SetVolume(1.0f, 1.0f); // Max volume
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmRingService - StartAlarmSound", ex);
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
                    _vibrator.Vibrate(VibrationEffect.CreateWaveform(pattern, 0));
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmRingService - StartVibration", ex);
            }
        }

        private void SilenceDevice()
        {
            _isRinging = false;
            IsRinging = false;
            _handler.RemoveCallbacksAndMessages(null);
            try
            {
                if (_mediaPlayer != null)
                {
                    if (_mediaPlayer.IsPlaying)
                        _mediaPlayer.Stop();
                    _mediaPlayer.Release();
                    _mediaPlayer = null;
                }
                _vibrator?.Cancel();
                _vibrator = null;
                if (_wakeLock?.IsHeld == true)
                    _wakeLock.Release();
                _wakeLock = null;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmRingService - SilenceDevice", ex);
            }
        }

        public override void OnDestroy()
        {
            SilenceDevice();
            base.OnDestroy();
        }
    }
}
#endif
