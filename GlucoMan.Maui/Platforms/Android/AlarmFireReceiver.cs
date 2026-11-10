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
    [BroadcastReceiver(
        Enabled = true,
        Exported = true,
        Name = "GlucoMan.Maui.Platforms.Android.AlarmFireReceiver")]
    public class AlarmFireReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            int id = intent.GetIntExtra("AlarmId", 0);
            string text = intent.GetStringExtra("ReminderText") ?? "Alarm";
            bool vibrate = intent.GetBooleanExtra("Vibrate", false);
            bool playSound = intent.GetBooleanExtra("PlaySound", false);
            string? soundPath = intent.GetStringExtra("SoundPath");

            // Update alarm in database to mark as triggered; an alarm that has been
            // deleted in the meantime must not ring
            if (!UpdateAlarmStatus(id))
                return;

            // The service makes the device ring and shows the alarm to the user
            try
            {
                context.StartForegroundService(AlarmRingService.CreateIntent(context,
                    AlarmRingService.ActionRing, id, text, vibrate, playSound, soundPath));
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmFireReceiver - starting the ringing", ex);
                ShowNotification(context, id, text, vibrate, playSound, soundPath);
            }

            // Android starts this page only if the app is in use: otherwise the notification
            // of the service opens it, by itself on a locked device, or when touched
            try
            {
                context.StartActivity(AlarmActivity.CreateIntent(context, id, text, vibrate, playSound, soundPath));
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmFireReceiver - opening the page of the alarm", ex);
            }
        }

        public static void ShowNotification(Context context, int id, string text, bool vibrate, bool playSound, string? soundPath)
        {
            try
            {
                // Create notification intent
                var notificationIntent = AlarmActivity.CreateIntent(context, id, text, vibrate, playSound, soundPath);

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

        /// <summary>
        /// Marks the alarm as triggered in the database.
        /// </summary>
        /// <returns>False only if a saved alarm doesn't exist anymore; the test alarm
        /// (negative id) isn't in the database, and in doubt the alarm must ring</returns>
        private bool UpdateAlarmStatus(int alarmId)
        {
            try
            {
                if (alarmId > 0 && DatabaseService.Instance.Database != null)
                {
                    var blAlarms = new BL_Alarms();
                    if (blAlarms.GetAlarmById(alarmId) == null)
                        return false;
                    blAlarms.MarkAlarmAsTriggered(alarmId);
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"UpdateAlarmStatus error: {ex.Message}");
            }
            return true;
        }
    }
}
#endif
