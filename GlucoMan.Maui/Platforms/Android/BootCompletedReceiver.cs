#if ANDROID
using Android.App;
using Android.Content;
using GlucoMan;

namespace GlucoMan.Maui.Platforms.Android
{
    /// <summary>
    /// BroadcastReceiver that restores all active alarms when the device boots up.
    /// This is necessary because AlarmManager alarms are cleared on device reboot.
    /// </summary>
    [BroadcastReceiver(Enabled = true, Exported = true, DirectBootAware = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted, Intent.ActionLockedBootCompleted })]
    public class BootCompletedReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            if (context == null || intent == null) return;

            var action = intent.Action;
            if (action != Intent.ActionBootCompleted && action != Intent.ActionLockedBootCompleted)
                return;

            global::System.Diagnostics.Debug.WriteLine("BootCompletedReceiver: Device booted, restoring alarms...");

            try
            {
                // Wait a bit for system to stabilize
                System.Threading.Thread.Sleep(2000);

                // Restore all active alarms
                RestoreActiveAlarms(context);
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"BootCompletedReceiver error: {ex.Message}");
            }
        }

        private void RestoreActiveAlarms(Context context)
        {
            try
            {
                // Initialize database if needed
                if (DatabaseService.Instance.Database == null)
                {
                    global::System.Diagnostics.Debug.WriteLine("BootCompletedReceiver: Database not initialized, skipping alarm restore");
                    return;
                }

                var blAlarms = new BL_Alarms();
                var activeAlarms = blAlarms.GetActiveAlarms();

                if (activeAlarms == null || activeAlarms.Count == 0)
                {
                    global::System.Diagnostics.Debug.WriteLine("BootCompletedReceiver: No active alarms to restore");
                    return;
                }

                global::System.Diagnostics.Debug.WriteLine($"BootCompletedReceiver: Restoring {activeAlarms.Count} active alarms");

                var scheduler = new SystemAlarmScheduler();

                int restored = 0;
                foreach (var alarm in activeAlarms)
                {
                    try
                    {
                        if (alarm.IdAlarm.HasValue && alarm.IsActive())
                        {
                            // Recalculate next trigger time in case it changed
                            alarm.CalculateNextTriggerTime();
                            
                            // Only reschedule if next trigger is in the future
                            if (alarm.NextTriggerTime.HasValue && alarm.NextTriggerTime.Value > DateTime.Now)
                            {
                                scheduler.ScheduleAsync(alarm).Wait();
                                restored++;
                                
                                global::System.Diagnostics.Debug.WriteLine(
                                    $"BootCompletedReceiver: Restored alarm {alarm.IdAlarm} - {alarm.ReminderText} at {alarm.NextTriggerTime:yyyy-MM-dd HH:mm}");
                            }
                            else
                            {
                                global::System.Diagnostics.Debug.WriteLine(
                                    $"BootCompletedReceiver: Skipped expired alarm {alarm.IdAlarm} - {alarm.ReminderText}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        global::System.Diagnostics.Debug.WriteLine(
                            $"BootCompletedReceiver: Error restoring alarm {alarm.IdAlarm}: {ex.Message}");
                    }
                }

                global::System.Diagnostics.Debug.WriteLine($"BootCompletedReceiver: Successfully restored {restored} alarms");

                // Show notification to user that alarms were restored
                if (restored > 0)
                {
                    ShowRestoredNotification(context, restored);
                }
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"BootCompletedReceiver.RestoreActiveAlarms error: {ex.Message}");
            }
        }

        private void ShowRestoredNotification(Context context, int count)
        {
            try
            {
                var builder = new AndroidX.Core.App.NotificationCompat.Builder(context, "glucoman_alarms")
                    .SetContentTitle("GlucoMan Alarms Restored")
                    .SetContentText($"{count} alarm(s) reactivated after device restart")
                    .SetSmallIcon(global::Android.Resource.Drawable.IcDialogInfo)
                    .SetPriority((int)NotificationPriority.Low)
                    .SetAutoCancel(true);

                var manager = AndroidX.Core.App.NotificationManagerCompat.From(context);
                manager.Notify(999999, builder.Build());
            }
            catch (Exception ex)
            {
                global::System.Diagnostics.Debug.WriteLine($"ShowRestoredNotification error: {ex.Message}");
            }
        }
    }
}
#endif

