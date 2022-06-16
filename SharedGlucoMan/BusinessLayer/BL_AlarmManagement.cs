using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    internal class BL_AlarmManagement
    {
        DataLayer dl = Common.Database;
        internal void AddAlarm(Alarm currentAlarm)
        {
            dl.SaveOneAlarm(currentAlarm); 
        }
        internal List<Alarm> ReadAllAlarms()
        {
            return dl.ReadAllAlarms();
        }
    }
}
