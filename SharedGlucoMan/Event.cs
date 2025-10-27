using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class Event
    {
        public int? IdEvent { get; set; }
        public DateTimeAndText EventTime { get; set; }
        public string Notes { get; set; }
        public Event ()
        {
            EventTime = new();
        }
    }
}
