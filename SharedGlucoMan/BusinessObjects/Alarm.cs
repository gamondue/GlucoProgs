using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal partial class Alarm
    {
        // platform independent part of class Alarm
        internal int? IdAlarm { get; set; }
        internal DateTimeAndText TimeStart { get; set; }
        internal DateTimeAndText TimeAlarm { get; set; }
        internal TimeSpan? Interval { get; set; }
        internal TimeSpan? Duration { get; set; }
        internal bool? IsRepeated { get; set; }
        internal bool? IsEnabled { get; set; }
        internal Alarm()
        {
            TimeStart = new DateTimeAndText();
            TimeAlarm = new DateTimeAndText(); 
        }
    }
}
