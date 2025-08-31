#if WINDOWS
using System;
using System.Threading.Tasks;
using GlucoMan; // interface + Alarm
using Windows.UI.Notifications;
#if COMMUNITY_TOOLKIT
using CommunityToolkit.WinUI.Notifications;
#endif

namespace GlucoMan.Maui.Platforms.Windows
{
    public class SystemAlarmScheduler : ISystemAlarmScheduler
    {
        public Task ScheduleAsync(Alarm alarm)
        {
            if (alarm == null) throw new ArgumentNullException(nameof(alarm));
            if (alarm.IdAlarm == null) throw new ArgumentException("Alarm must have IdAlarm before scheduling");

            var baseStart = alarm.TimeStart?.DateTime ?? DateTime.Now;
            DateTime when = alarm.NextTriggerTime ?? (baseStart + (alarm.ValidTimeAfterStart ?? TimeSpan.Zero));

#if COMMUNITY_TOOLKIT
            var content = new ToastContentBuilder()
                .AddText("GlucoMan")
                .AddText(alarm.ReminderText ?? "Alarm")
                .GetToastContent();
            var xml = content.GetXml();
#else
            string xmlString = $"<toast><visual><binding template=\"ToastGeneric\"><text>GlucoMan</text><text>{System.Security.SecurityElement.Escape(alarm.ReminderText ?? "Alarm")}</text></binding></visual></toast>";
            var xml = new global::Windows.Data.Xml.Dom.XmlDocument();
            xml.LoadXml(xmlString);
#endif
            var scheduled = new ScheduledToastNotification(xml, new DateTimeOffset(when))
            {
                Id = alarm.IdAlarm.Value.ToString()
            };
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduled);
            return Task.CompletedTask;
        }

        public Task CancelAsync(int idAlarm)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var s in notifier.GetScheduledToastNotifications())
            {
                if (s.Id == idAlarm.ToString())
                    notifier.RemoveFromSchedule(s);
            }
            return Task.CompletedTask;
        }

        public Task CancelAllAsync()
        {
            var notifier = ToastNotificationManager.CreateToastNotifier();
            foreach (var s in notifier.GetScheduledToastNotifications())
                notifier.RemoveFromSchedule(s);
            return Task.CompletedTask;
        }
    }
}
#endif
