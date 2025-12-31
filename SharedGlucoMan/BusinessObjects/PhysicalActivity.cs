using System;

namespace GlucoMan
{
    public class PhysicalActivity
    {
        public int? IdActivity { get; set; }
        public DateTime? EventTime { get; set; }
        public int? ActivityLevel { get; set; } // 1-10
        public int? DurationMinutes { get; set; }
        public string Intensity { get; set; } // Low/High
        public double? Accuracy { get; set; }
        public string Notes { get; set; }
        public int? IdTrack { get; set; } // Foreign key to GpsTracks

        public PhysicalActivity()
        {
            EventTime = DateTime.Now;
            ActivityLevel = 1;
            DurationMinutes = 30;
            Intensity = "Low";
            Accuracy = 100;
            Notes = string.Empty;
            IdTrack = null;
        }
    }
}