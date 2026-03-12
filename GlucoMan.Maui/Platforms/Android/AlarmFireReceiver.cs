#if ANDROID
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class AlarmFireReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            int id = intent.GetIntExtra("AlarmId", 0);
            string text = intent.GetStringExtra("ReminderText") ?? "Alarm";
            bool vibrate = intent.GetBooleanExtra("Vibrate", false);
            bool playSound = intent.GetBooleanExtra("PlaySound", false);
            string? soundPath = intent.GetStringExtra("SoundPath");

            // Launch fullscreen alarm activity
            var alarmIntent = new Intent(context, typeof(AlarmActivity));
            alarmIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            alarmIntent.PutExtra("AlarmId", id);
            alarmIntent.PutExtra("ReminderText", text);
            alarmIntent.PutExtra("Vibrate", vibrate);
            alarmIntent.PutExtra("PlaySound", playSound);
            alarmIntent.PutExtra("SoundPath", soundPath ?? "");

            context.StartActivity(alarmIntent);

            // Also show a notification as backup (in case activity doesn't start)
            ShowNotification(context, id, text, vibrate, playSound, soundPath);

            // Update alarm in database to mark as triggered
            try
            {
                UpdateAlarmStatus(id);
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error updating alarm status: {ex.Message}");
            }
        }

        private void ShowNotification(Context context, int id, string text, bool vibrate, bool playSound, string? soundPath)
        {
            try
            {
                // Create notification intent
                var notificationIntent = new Intent(context, typeof(AlarmActivity));
                notificationIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                notificationIntent.PutExtra("AlarmId", id);
                notificationIntent.PutExtra("ReminderText", text);

                var pendingIntent = PendingIntent.GetActivity(
                    context,
                    id,
                    notificationIntent,
                    PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

                // Build notification
                var builder = new NotificationCompat.Builder(context, "glucoman_alarms")
                    .SetContentTitle(AppStrings.GlucoManAlarm)
                    .SetContentText(text)
                    .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
                    .SetPriority((int)NotificationPriority.High)
                    .SetCategory(NotificationCompat.CategoryAlarm)
                    .SetAutoCancel(true)
                    .SetContentIntent(pendingIntent)
                    .SetFullScreenIntent(pendingIntent, true); // Important: makes it fullscreen

                // Add sound if requested
                if (playSound)
                {
                    if (!string.IsNullOrEmpty(soundPath) && File.Exists(soundPath))
                    {
                        try
                        {
                            var soundUri = global::Android.Net.Uri.Parse(soundPath);
                            builder.SetSound(soundUri);
                        }
                        catch
                        {
                            builder.SetDefaults((int)NotificationDefaults.Sound);
                        }
                    }
                    else
                    {
                        builder.SetDefaults((int)NotificationDefaults.Sound);
                    }
                }

                // Add vibration if requested
                if (vibrate)
                {
                    builder.SetVibrate(new long[] { 0, 500, 200, 500 });
                }

                // Show notification
                var manager = NotificationManagerCompat.From(context);
                manager.Notify(id, builder.Build());
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"Error showing notification: {ex.Message}");
            }
        }

        private void UpdateAlarmStatus(int alarmId)
        {
            try
            {
                if (DatabaseService.Instance.Database != null)
                {
                    var blAlarms = new BL_Alarms();
                    var alarms = blAlarms.GetAllAlarms(all: true);
                    var alarm = alarms.FirstOrDefault(a => a.IdAlarm == alarmId);
                    
                    if (alarm != null)
                    {
                        alarm.MarkAsTriggered();
                        blAlarms.AddAlarm(alarm);
                    }
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"UpdateAlarmStatus error: {ex.Message}");
            }
        }
    }
}
#endif
