using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    public class BL_AlarmManagement
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
