namespace GlucoMan.BusinessLayer
{
    public class BL_Alarms
    {
        DataLayer dl = Common.Database;
        
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
        /// <param name="active">If true, retrieves alarms where TimeStart + TriggerInterval > DateTime.Now</param>
        /// <returns>List of filtered alarms</returns>
        public List<Alarm> GetAllAlarms(DateTime? from = null, DateTime? to = null, bool all = false, bool expired = false, bool active = true)
        {
            return dl.GetAllAlarms(from, to, all, expired, active);
        }
        /// <summary>
        /// Retrieves all active alarms (TimeStart + TriggerInterval > DateTime.Now)
        /// </summary>
        /// <returns>List of active alarms</returns>
        public List<Alarm> GetActiveAlarms()
        {
            return dl.GetAllAlarms(all: false, expired: false, active: true);
        }
        /// <summary>
        /// Retrieves all expired alarms (TimeStart + TriggerInterval <= DateTime.Now)
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
