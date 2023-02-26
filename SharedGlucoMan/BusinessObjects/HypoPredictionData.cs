using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class HypoPredictionData
    {
        public int IdHypoPrediction;
        public DateTimeAndText PredictedTime { get; set; }
        public DateTimeAndText AlarmTime { get; set; }
        public DoubleAndText GlucoseSlope { get; }
        public IntAndText HypoGlucoseTarget { get; set; }
        public IntAndText GlucoseLast { get; set; }
        public IntAndText GlucosePrevious { get; set; }
        public TimeSpan Interval { get; set; }
        public IntAndText HourLast { get; set; }
        public IntAndText MinuteLast { get; set; }
        public IntAndText HourPrevious { get; set; }
        public IntAndText MinutePrevious { get; set; }
        public IntAndText PredictedHour { get; set; }
        public IntAndText PredictedMinute { get; set; }
        public IntAndText AlarmHour { get; set; }
        public IntAndText AlarmMinute { get; set; }
        private TimeSpan interval;
        public HypoPredictionData()
        {
            GlucoseLast = new IntAndText();
            GlucosePrevious = new IntAndText();
            HypoGlucoseTarget = new IntAndText();
            GlucoseSlope = new DoubleAndText();
            GlucoseSlope.Format = "0.00";
            HourLast = new IntAndText();
            HourPrevious = new IntAndText();
            MinuteLast = new IntAndText();
            MinutePrevious = new IntAndText();
            PredictedTime = new DateTimeAndText();
            AlarmTime = new DateTimeAndText();
            PredictedHour = new IntAndText();
            PredictedMinute = new IntAndText();
            AlarmHour = new IntAndText();
            AlarmMinute = new IntAndText();
        }
    }
}
