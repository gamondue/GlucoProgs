using System.Collections.Generic;

namespace GlucoMan.BusinessLayer
{
    public class BL_Alarms
    {
        DataLayer dl = Common.Database;
        public void AddAlarm(Alarm currentAlarm)
        {
            dl.SaveOneAlarm(currentAlarm);
        }
        public List<Alarm> ReadAllAlarms()
        {
            return dl.ReadAllAlarms();
        }
    }
}
