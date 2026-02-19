using GlucoMan;

namespace GlucoMan.Maui
{
    /// <summary>
    /// Helper class to synchronize alarms between database and system scheduler.
    /// Ensures that only active alarms are scheduled and expired ones are removed.
    /// </summary>
    public static class AlarmSyncHelper
    {
        /// <summary>
        /// Synchronize all alarms: schedule active ones, cancel expired/dismissed ones
        /// </summary>
        public static async Task SyncAllAlarmsAsync(ISystemAlarmScheduler scheduler)
        {
            try
            {
                if (DatabaseService.Instance.Database == null)
                {
                    gamon.General.LogOfProgram?.Debug("AlarmSyncHelper: Database not initialized");
                    return;
                }

                var blAlarms = new BL_Alarms();
                
                // Get all alarms from database
                var allAlarms = blAlarms.GetAllAlarms(all: true);
                
                if (allAlarms == null || allAlarms.Count == 0)
                {
                    gamon.General.LogOfProgram?.Debug("AlarmSyncHelper: No alarms in database");
                    await scheduler.CancelAllAsync();
                    return;
                }

                int scheduled = 0;
                int cancelled = 0;

                foreach (var alarm in allAlarms)
                {
                    if (!alarm.IdAlarm.HasValue)
                        continue;

                    // Recalculate next trigger time
                    alarm.CalculateNextTriggerTime();

                    if (alarm.IsActive() && alarm.NextTriggerTime.HasValue && alarm.NextTriggerTime.Value > DateTime.Now)
                    {
                        // Schedule/reschedule active alarm
                        try
                        {
                            await scheduler.ScheduleAsync(alarm);
                            scheduled++;
                            
                            gamon.General.LogOfProgram?.Debug(
                                $"AlarmSyncHelper: Scheduled alarm {alarm.IdAlarm} - {alarm.ReminderText} at {alarm.NextTriggerTime:yyyy-MM-dd HH:mm}");
                        }
                        catch (Exception ex)
                        {
                            gamon.General.LogOfProgram?.Error($"AlarmSyncHelper: Error scheduling alarm {alarm.IdAlarm}", ex);
                        }
                    }
                    else
                    {
                        // Cancel expired/dismissed alarm
                        try
                        {
                            await scheduler.CancelAsync(alarm.IdAlarm.Value);
                            cancelled++;
                            
                            gamon.General.LogOfProgram?.Debug(
                                $"AlarmSyncHelper: Cancelled alarm {alarm.IdAlarm} - {alarm.ReminderText} (inactive/expired)");
                        }
                        catch (Exception ex)
                        {
                            gamon.General.LogOfProgram?.Error($"AlarmSyncHelper: Error cancelling alarm {alarm.IdAlarm}", ex);
                        }
                    }
                }

                gamon.General.LogOfProgram?.Event($"AlarmSyncHelper: Sync complete - {scheduled} scheduled, {cancelled} cancelled");
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmSyncHelper.SyncAllAlarmsAsync", ex);
            }
        }

        /// <summary>
        /// Clean up expired alarms from database
        /// </summary>
        public static void CleanupExpiredAlarms()
        {
            try
            {
                if (DatabaseService.Instance.Database == null)
                    return;

                var blAlarms = new BL_Alarms();
                var expiredAlarms = blAlarms.GetExpiredAlarms();

                if (expiredAlarms == null || expiredAlarms.Count == 0)
                    return;

                int deleted = 0;
                foreach (var alarm in expiredAlarms)
                {
                    // Only delete if expired more than 7 days ago
                    if (alarm.TimeStart?.DateTime != null && 
                        alarm.ValidTimeAfterStart.HasValue)
                    {
                        var expiryDate = alarm.TimeStart.DateTime.Value + alarm.ValidTimeAfterStart.Value;
                        if (expiryDate.AddDays(7) < DateTime.Now)
                        {
                            blAlarms.DeleteAlarm(alarm);
                            deleted++;
                        }
                    }
                }

                if (deleted > 0)
                {
                    gamon.General.LogOfProgram?.Event($"AlarmSyncHelper: Cleaned up {deleted} old expired alarms");
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("AlarmSyncHelper.CleanupExpiredAlarms", ex);
            }
        }

        /// <summary>
        /// Get count of currently active alarms
        /// </summary>
        public static int GetActiveAlarmsCount()
        {
            try
            {
                if (DatabaseService.Instance.Database == null)
                    return 0;

                var blAlarms = new BL_Alarms();
                var activeAlarms = blAlarms.GetActiveAlarms();
                
                return activeAlarms?.Count(a => 
                    a.NextTriggerTime.HasValue && 
                    a.NextTriggerTime.Value > DateTime.Now) ?? 0;
            }
            catch
            {
                return 0;
            }
        }
    }
}

