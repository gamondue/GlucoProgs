#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using GlucoMan;
using AndroidApp = Android.App.Application;

namespace GlucoMan.Maui.Platforms.Android
{
    public class SystemAlarmScheduler : ISystemAlarmScheduler
    {
        private readonly Context _context;
        public SystemAlarmScheduler()
        {
            // Utilizzare l'alias per evitare ambiguità
            _context = AndroidApp.Context;
            CreateNotificationChannel();
        }

        private void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel("glucoman_alarms", "GlucoMan Alarms", NotificationImportance.High)
                {
                    Description = "Alarms and reminders"
                };
                channel.EnableVibration(true);
                var manager = (NotificationManager)_context.GetSystemService(Context.NotificationService);
                manager.CreateNotificationChannel(channel);
            }
        }

        public Task ScheduleAsync(Alarm alarm)
        {
            if (alarm == null) throw new ArgumentNullException(nameof(alarm));
            if (alarm.IdAlarm == null)
                throw new ArgumentException("Alarm must have IdAlarm before scheduling");

            var baseStart = alarm.TimeStart?.DateTime ?? DateTime.Now;
            DateTime when = alarm.NextTriggerTime ?? (baseStart + (alarm.ValidTimeAfterStart ?? TimeSpan.Zero));

            long triggerAtMillis = (long)(when.ToUniversalTime() - DateTime.UnixEpoch).TotalMilliseconds;

            var intent = new Intent(_context, typeof(AlarmFireReceiver));
            intent.PutExtra("AlarmId", alarm.IdAlarm.Value);
            intent.PutExtra("ReminderText", alarm.ReminderText ?? "Alarm");

            var pending = PendingIntent.GetBroadcast(
                _context,
                alarm.IdAlarm.Value,
                intent,
                PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

            var alarmManager = (AlarmManager)_context.GetSystemService(Context.AlarmService);
            bool exact = alarm.Interval == null && alarm.ValidTimeAfterStart != null;

            if (exact && Build.VERSION.SdkInt >= BuildVersionCodes.M)
                alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, triggerAtMillis, pending);
            else if (exact)
                alarmManager.SetExact(AlarmType.RtcWakeup, triggerAtMillis, pending);
            else
                alarmManager.Set(AlarmType.RtcWakeup, triggerAtMillis, pending);

            return Task.CompletedTask;
        }

        public Task CancelAsync(int idAlarm)
        {
            var intent = new Intent(_context, typeof(AlarmFireReceiver));
            var pending = PendingIntent.GetBroadcast(
                _context,
                idAlarm,
                intent,
                PendingIntentFlags.Immutable | PendingIntentFlags.NoCreate);
            if (pending != null)
            {
                var alarmManager = (AlarmManager)_context.GetSystemService(Context.AlarmService);
                alarmManager.Cancel(pending);
                pending.Cancel();
            }
            return Task.CompletedTask;
        }

        public Task CancelAllAsync()
        {
            // Track ids externally if full mass-cancel is required.
            return Task.CompletedTask;
        }
    }
}
#endif
