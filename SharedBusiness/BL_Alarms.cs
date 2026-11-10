namespace GlucoMan
{
    public class BL_Alarms
    {
        DataLayer dl = DatabaseService.Instance.Database;
        
        public void AddAlarm(Alarm currentAlarm)
        {
            dl.SaveOneAlarm(currentAlarm);
        }  
        /// <summary>
        /// Retrieves alarms with optional filtering
        /// </summary>
        /// <param name="from">Start date filter - alarms after this date</param>
        /// <param name="to">End date filter - alarms before this date</param>
        /// <param name="all">If true, retrieves all alarms regardless of status</param>
        /// <param name="expired">If true, retrieves only expired alarms</param>
        /// <param name="active">If true, retrieves alarms that are not disabled/expired/dismissed and still have a future NextTriggerTime or a periodic Interval</param>
        /// <returns>List of filtered alarms</returns>
        public List<Alarm> GetAllAlarms(DateTime? from = null, DateTime? to = null, bool all = false, bool expired = false, bool active = true)
        {
            return dl.GetAllAlarms(from, to, all, expired, active);
        }
        /// <summary>
        /// Retrieves all active alarms (not disabled/expired/dismissed, with a future NextTriggerTime or periodic Interval)
        /// </summary>
        /// <returns>List of active alarms</returns>
        public List<Alarm> GetActiveAlarms()
        {
            return dl.GetAllAlarms(all: false, expired: false, active: true);
        }
        /// <summary>
        /// Retrieves all expired alarms (disabled, expired/dismissed, or max restart-when-not-dismissed cycles reached)
        /// </summary>
        /// <returns>List of expired alarms</returns>
        public List<Alarm> GetExpiredAlarms()
        {
            return dl.GetAllAlarms(all: false, expired: true, active: false);
        }
        /// <summary>
        /// Retrieves alarms within a date range
        /// </summary>
        /// <param name="from">Start date</param>
        /// <param name="to">End date</param>
        /// <param name="active">If true, only active alarms; if false, include expired</param>
        /// <returns>List of alarms within the specified date range</returns>
        public List<Alarm> GetAlarmsInDateRange(DateTime from, DateTime to, bool active = true)
        {
            return dl.GetAllAlarms(from: from, to: to, all: false, expired: false, active: active);
        }
        /// <summary>
        /// Retrieves alarms scheduled for today
        /// </summary>
        /// <param name="active">If true, only active alarms; if false, include expired</param>
        /// <returns>List of alarms scheduled for today</returns>
        public List<Alarm> GetTodaysAlarms(bool active = true)
        {
            DateTime today = DateTime.Today;
            DateTime tomorrow = today.AddDays(1);
            return dl.GetAllAlarms(from: today, to: tomorrow, all: false, expired: false, active: active);
        }
        /// <summary>
        /// Retrieves alarms scheduled for the next specified number of days
        /// </summary>
        /// <param name="days">Number of days to look ahead</param>
        /// <param name="active">If true, only active alarms; if false, include expired</param>
        /// <returns>List of alarms in the next specified days</returns>
        public List<Alarm> GetUpcomingAlarms(int days = 7, bool active = true)
        {
            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now.AddDays(days);
            return dl.GetAllAlarms(from: from, to: to, all: false, expired: false, active: active);
        }
        /// <summary>
        /// Retrieves all enabled alarms regardless of date
        /// </summary>
        /// <param name="active">If true, only active alarms; if false, include expired</param>
        /// <returns>List of enabled alarms</returns>
        public List<Alarm> GetEnabledAlarms(bool active = true)
        {
            return dl.GetAllAlarms(all: false, expired: false, active: active);
        }
        /// <summary>
        /// Retrieves only alarms in Waiting state (scheduled, waiting to fire for the first time or after a restart)
        /// </summary>
        public List<Alarm> GetWaitingAlarms()
        {
            return dl.GetAllAlarms(all: true)
                     .Where(a => a.RingingState == Alarm.AlarmRingingState.Waiting)
                     .ToList();
        }
        /// <summary>
        /// Retrieves only alarms in Delayed state (snoozed by the user)
        /// </summary>
        public List<Alarm> GetDelayedAlarms()
        {
            return dl.GetAllAlarms(all: true)
                     .Where(a => a.RingingState == Alarm.AlarmRingingState.Delayed)
                     .ToList();
        }
        /// <summary>
        /// Retrieves only alarms in AutoSuspended state (automatically suspended by the system)
        /// </summary>
        public List<Alarm> GetAutoSuspendedAlarms()
        {
            return dl.GetAllAlarms(all: true)
                     .Where(a => a.RingingState == Alarm.AlarmRingingState.AutoSuspended)
                     .ToList();
        }
        /// <summary>
        /// Retrieves one alarm by its id
        /// </summary>
        /// <returns>The alarm, or null if it isn't in the database</returns>
        public Alarm? GetAlarmById(int? alarmId)
        {
            return dl.GetAllAlarms(all: true).FirstOrDefault(a => a.IdAlarm == alarmId);
        }
        /// <summary>
        /// Retrieves the transient alarms, that are the ones that exist only until dismissed
        /// </summary>
        public List<Alarm> GetTransientAlarms()
        {
            return dl.GetAllAlarms(all: true).Where(a => a.IsTransient == true).ToList();
        }
        /// <summary>
        /// Records that an alarm started ringing
        /// </summary>
        public void MarkAlarmAsTriggered(int? alarmId)
        {
            Alarm? alarm = GetAlarmById(alarmId);
            if (alarm == null)
                return;
            alarm.MarkAsTriggered();
            dl.SaveOneAlarm(alarm);
        }
        /// <summary>
        /// The user delays a ringing alarm
        /// </summary>
        /// <returns>The alarm to schedule again, or null if it isn't in the database</returns>
        public Alarm? SnoozeAlarm(int? alarmId, int minutes)
        {
            Alarm? alarm = GetAlarmById(alarmId);
            if (alarm == null)
                return null;
            alarm.Snooze(minutes);
            dl.SaveOneAlarm(alarm);
            return alarm;
        }
        /// <summary>
        /// The user stops a ringing alarm: a transient alarm ends here and is deleted,
        /// any other alarm goes on with its next occurrence, if it has one.
        /// </summary>
        /// <returns>The alarm to schedule again, or null if it has no more occurrences</returns>
        public Alarm? StopAlarm(int? alarmId)
        {
            Alarm? alarm = GetAlarmById(alarmId);
            if (alarm == null)
                return null;
            if (alarm.IsTransient == true)
            {
                dl.DeleteOneAlarm(alarm);
                return null;
            }
            alarm.IsPlaying = false;
            alarm.CalculateNextTriggerTime();
            dl.SaveOneAlarm(alarm);
            return alarm.NextTriggerTime.HasValue ? alarm : null;
        }
        /// <summary>
        /// The user cancels the whole sequence of an alarm: a transient alarm is deleted,
        /// any other alarm is kept in the database as Dismissed.
        /// </summary>
        public void DismissAlarm(int? alarmId)
        {
            Alarm? alarm = GetAlarmById(alarmId);
            if (alarm == null)
                return;
            if (alarm.IsTransient == true)
            {
                dl.DeleteOneAlarm(alarm);
                return;
            }
            alarm.Dismiss();
            dl.SaveOneAlarm(alarm);
        }
        /// <summary>
        /// An alarm has rung for its whole Duration without any answer from the user
        /// </summary>
        /// <returns>The alarm to schedule again, or null if it will not ring anymore</returns>
        public Alarm? RestartAlarmNotDismissed(int? alarmId)
        {
            Alarm? alarm = GetAlarmById(alarmId);
            if (alarm == null)
                return null;
            alarm.MarkAsRestartedWhenNotDismissed();
            dl.SaveOneAlarm(alarm);
            return alarm.RingingState == Alarm.AlarmRingingState.Expired ? null : alarm;
        }
        public void DeleteAlarm(Alarm alarm)
        {
            dl.DeleteOneAlarm(alarm);
        }  
        public void DeleteAlarmById(int? alarmId)
        {
            if (alarmId.HasValue)
            {
                var alarm = new Alarm { IdAlarm = alarmId };
                dl.DeleteOneAlarm(alarm);
            }
        }
    }
}
