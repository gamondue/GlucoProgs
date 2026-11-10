#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Activity;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Platforms.Android
{
    /// <summary>
    /// Full screen page of a ringing alarm. The ringing belongs to AlarmRingService:
    /// this page shows the alarm, lets the user choose the delay and passes on the answer.
    /// </summary>
    [Activity(
        Label = "GlucoMan Alarm",
        Theme = "@style/Theme.AppCompat.NoActionBar",
        LaunchMode = global::Android.Content.PM.LaunchMode.SingleInstance,
        ExcludeFromRecents = true,
        ShowForAllUsers = true,
        Name = "GlucoMan.Maui.Platforms.Android.AlarmActivity")]
    public class AlarmActivity : AppCompatActivity
    {
        private int _alarmId;
        private string _reminderText = "";
        private bool _shouldVibrate;
        private bool _shouldPlaySound;
        private string? _soundPath;
        private int _snoozeMinutes = Alarm.DefaultSnoozeMinutes;
        private TextView? _txtSnoozeMinutes;
        private TextView? _txtMessage;
        private global::Android.Widget.Button? _btnSuspend;

        /// <summary>
        /// Creates the intent that shows this page. The same intent is used by all the
        /// ways of reaching the page, so it always has what it needs to answer correctly.
        /// </summary>
        public static Intent CreateIntent(Context context, int alarmId, string reminderText,
            bool vibrate, bool playSound, string? soundPath)
        {
            var intent = new Intent(context, typeof(AlarmActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            intent.PutExtra("AlarmId", alarmId);
            intent.PutExtra("ReminderText", reminderText);
            intent.PutExtra("Vibrate", vibrate);
            intent.PutExtra("PlaySound", playSound);
            intent.PutExtra("SoundPath", soundPath ?? "");
            return intent;
        }

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // The back gesture must not hide the alarm: the user has to choose a button
            OnBackPressedDispatcher.AddCallback(this, new IgnoreBackCallback());

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

            SetContentView(GlucoMan.Maui.Resource.Layout.alarm_activity);

            // Setup UI - Use reflection to get resource IDs
            int txtTitleId = GetResourceId("txtAlarmTitle", "id");
            int txtMessageId = GetResourceId("txtAlarmMessage", "id");
            int txtInstructionsId = GetResourceId("txtInstructions", "id");
            int btnStopId = GetResourceId("btnStop", "id");
            int btnSuspendId = GetResourceId("btnSuspend", "id");
            int btnSnoozeId = GetResourceId("btnSnooze", "id");
            int btnSnoozeUpId = GetResourceId("btnSnoozeUp", "id");
            int btnSnoozeDownId = GetResourceId("btnSnoozeDown", "id");
            int txtSnoozeMinutesId = GetResourceId("txtSnoozeMinutes", "id");

            var txtTitle = FindViewById<TextView>(txtTitleId);
            _txtMessage = FindViewById<TextView>(txtMessageId);
            var txtInstructions = FindViewById<TextView>(txtInstructionsId);
            var btnStop = FindViewById<global::Android.Widget.Button>(btnStopId);
            _btnSuspend = FindViewById<global::Android.Widget.Button>(btnSuspendId);
            var btnSnooze = FindViewById<global::Android.Widget.Button>(btnSnoozeId);
            var btnSnoozeUp = FindViewById<global::Android.Widget.Button>(btnSnoozeUpId);
            var btnSnoozeDown = FindViewById<global::Android.Widget.Button>(btnSnoozeDownId);
            _txtSnoozeMinutes = FindViewById<TextView>(txtSnoozeMinutesId);

            if (txtTitle != null)
                txtTitle.Text = AppStrings.GlucoManAlarm;

            if (txtInstructions != null)
                txtInstructions.Text = AppStrings.AlarmSwipeInstructions;

            if (btnStop != null)
            {
                btnStop.Text = AppStrings.AlarmStopButtonShort;
                btnStop.Click += (s, e) => AnswerAlarm(AlarmRingService.ActionStop);
            }

            if (_btnSuspend != null)
            {
                _btnSuspend.Text = AppStrings.AlarmSuspendButtonShort;
                _btnSuspend.Click += (s, e) => AnswerAlarm(AlarmRingService.ActionDismiss);
            }

            if (btnSnooze != null)
            {
                btnSnooze.Text = AppStrings.AlarmSnoozeButtonShort;
                btnSnooze.Click += (s, e) => AnswerAlarm(AlarmRingService.ActionSnooze);
            }

            if (btnSnoozeUp != null)
                btnSnoozeUp.Click += (s, e) => ChangeSnoozeMinutes(up: true);

            if (btnSnoozeDown != null)
                btnSnoozeDown.Click += (s, e) => ChangeSnoozeMinutes(up: false);

            ShowAlarm(Intent);

            AlarmRingService.RingingEnded += OnRingingEnded;
        }

        // This page has a single instance: another alarm that rings arrives here
        protected override void OnNewIntent(Intent? intent)
        {
            base.OnNewIntent(intent);
            Intent = intent;
            ShowAlarm(intent);
        }

        private void ShowAlarm(Intent? intent)
        {
            // Get alarm data from intent
            _alarmId = intent?.GetIntExtra("AlarmId", 0) ?? 0;
            _reminderText = intent?.GetStringExtra("ReminderText") ?? "Alarm";
            _shouldVibrate = intent?.GetBooleanExtra("Vibrate", false) ?? false;
            _shouldPlaySound = intent?.GetBooleanExtra("PlaySound", true) ?? true;
            _soundPath = intent?.GetStringExtra("SoundPath");

            // The notification that replaces this page when it can't start isn't needed anymore
            NotificationManagerCompat.From(this).Cancel(_alarmId);

            Alarm? alarm = ReadAlarm();
            _snoozeMinutes = alarm?.GetSnoozeMinutes() ?? Alarm.DefaultSnoozeMinutes;
            ShowSnoozeMinutes();

            if (_txtMessage != null)
                _txtMessage.Text = _reminderText;

            // A transient alarm has no sequence to suspend: Stop already ends it
            if (_btnSuspend != null)
                _btnSuspend.Visibility = alarm?.IsTransient == true ? ViewStates.Gone : ViewStates.Visible;
        }

        protected override void OnResume()
        {
            base.OnResume();

            // An alarm shown without sound is what remains if the process has been killed
            // while ringing, or if this page was reached from the backup notification
            if (!AlarmRingService.IsRinging && !IsFinishing)
            {
                StartForegroundService(AlarmRingService.CreateIntent(this, AlarmRingService.ActionRing,
                    _alarmId, _reminderText, _shouldVibrate, _shouldPlaySound, _soundPath));
            }
        }

        private void ChangeSnoozeMinutes(bool up)
        {
            _snoozeMinutes = Alarm.StepSnoozeMinutes(_snoozeMinutes, up);
            ShowSnoozeMinutes();
        }

        private void ShowSnoozeMinutes()
        {
            if (_txtSnoozeMinutes != null)
                _txtSnoozeMinutes.Text = string.Format(AppStrings.AlarmSnoozeMinutesFormat, _snoozeMinutes);
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
                gamon.General.LogOfProgram?.Error($"AlarmActivity - reading alarm {_alarmId}", ex);
            }
            return null;
        }

        private void AnswerAlarm(string action)
        {
            var intent = AlarmRingService.CreateIntent(this, action, _alarmId, _reminderText,
                _shouldVibrate, _shouldPlaySound, _soundPath);
            intent.PutExtra(AlarmRingService.ExtraSnoozeMinutes, _snoozeMinutes);
            StartService(intent);
            Finish();
        }

        private void OnRingingEnded()
        {
            RunOnUiThread(Finish);
        }

        protected override void OnDestroy()
        {
            AlarmRingService.RingingEnded -= OnRingingEnded;
            base.OnDestroy();
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

        private sealed class IgnoreBackCallback : OnBackPressedCallback
        {
            public IgnoreBackCallback() : base(true) { }
            public override void HandleOnBackPressed() { }
        }
    }
}
#endif
