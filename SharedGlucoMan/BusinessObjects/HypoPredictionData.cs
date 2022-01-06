using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal class HypoPredictionData
    {
        internal int IdHypoPrediction;
        internal DateTimeAndText PredictedTime { get; set; }
        internal DateTimeAndText AlarmTime { get; set; }
        internal DoubleAndText GlucoseSlope { get; }
        internal IntAndText HypoGlucoseTarget { get; set; }
        internal IntAndText GlucoseLast { get; set; }
        internal IntAndText GlucosePrevious { get; set; }
        internal TimeSpan Interval { get => interval; set => interval = value; }
        internal IntAndText HourLast { get; set; }
        internal IntAndText MinuteLast { get; set; }
        internal IntAndText HourPrevious { get; set; }
        internal IntAndText MinutePrevious { get; set; }
        internal IntAndText PredictedHour { get; set; }
        internal IntAndText PredictedMinute { get; set; }
        internal IntAndText AlarmHour { get; set; }
        internal IntAndText AlarmMinute { get; set; }
        internal HypoPredictionData()
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

        private TimeSpan interval;
    }
}
