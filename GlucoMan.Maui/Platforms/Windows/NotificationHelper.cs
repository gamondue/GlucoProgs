#if WINDOWS
using System;
using System.Threading.Tasks;

namespace GlucoMan.Maui.Platforms.Windows
{
    public static class NotificationHelper
    {
        /// <summary>
        /// Check if alarms are supported on Windows (always true for timer-based approach)
        /// </summary>
        public static bool AreNotificationsEnabled()
        {
            return true; // Timer-based alarms always work
        }

        /// <summary>
        /// Show a test notification to verify alarms work
        /// </summary>
        public static async Task<bool> TestNotificationAsync()
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert(
                    "GlucoMan Test",
                    "Alarm notifications are working!\n\nNote: Alarms require the app to be running.",
                    "OK");
                
                gamon.General.LogOfProgram?.Event("Windows NotificationHelper: Test notification shown successfully");
                return true;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("NotificationHelper.TestNotificationAsync", ex);
                return false;
            }
        }

        /// <summary>
        /// Get diagnostic information about alarm system
        /// </summary>
        public static string GetDiagnosticInfo()
        {
            try
            {
                int scheduledCount = SystemAlarmScheduler.GetScheduledAlarmsCount();
                
                return $"Alarm System: Timer-based (in-app)\n" +
                       $"Currently Scheduled: {scheduledCount} alarm(s)\n" +
                       $"Status: Active\n" +
                       $"Note: App must be running for alarms to trigger";
            }
            catch (Exception ex)
            {
                return $"Error getting diagnostic info: {ex.Message}";
            }
        }
    }
}
#endif
