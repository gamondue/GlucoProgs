using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public partial class Alarm
    {
        // platform independent part of class Alarm
        public int? IdAlarm { get; set; }
        public DateTimeAndText TimeStart { get; set; }
        public DateTimeAndText TimeAlarm { get; set; }
        public TimeSpan? Interval { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? IsRepeated { get; set; }
        public bool? IsEnabled { get; set; }
        public Alarm()
        {
            TimeStart = new DateTimeAndText();
            TimeAlarm = new DateTimeAndText(); 
        }
    }
}
