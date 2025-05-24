using System;
using System.Collections.Generic;
using System.Text;
using static GlucoMan.Common;

namespace GlucoMan.BusinessObjects
{
    internal class PositionOfInjection
    {
        internal int? IdPosition { get; set; }
        internal DateTime? Timestamp { get; set; }
        internal ZoneOfPosition? Zone { get; set; }
        internal double? PositionX { get; set; }
        internal double? PositionY { get; set; }
        internal string Notes { get; set; }
    }
}
