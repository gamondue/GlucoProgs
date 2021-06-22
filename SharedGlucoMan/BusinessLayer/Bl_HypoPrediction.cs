using System;
using System.IO;
using SharedData;
using SharedFunctions;
using SharedObjects;

namespace GlucoMan.BusinessLayer
{
    internal class Bl_HypoPrediction
    {
        DataLayer dl = new DataLayer(); 

        private DateTime timeLast;
        private DateTime timePrevious;
        private TimeSpan interval;
        private bool alarmIsSet = false;

        private Alarm alarm; 

        public bool AlarmIsSet { get => alarmIsSet; set => alarmIsSet = value; }

        internal DateTime TimeLast { get => timeLast; set => timeLast = value; }
        internal DateTime TimePrevious { get => timePrevious; }
        internal IntAndText GlucoseLast { get; set; }
        internal IntAndText GlucosePrevious { get; set; }
        internal IntAndText HypoGlucoseTarget { get; set; }
        internal DoubleAndText GlucoseSlope { get; }
        internal TimeSpan Interval { get => interval; set => interval = value; }
        internal IntAndText HourLast { get; set; }
        internal IntAndText HourPrevious { get; set; }
        internal IntAndText MinuteLast { get; set; }
        internal IntAndText MinutePrevious { get; set; }
        internal DateTimeAndText PredictedTime { get; set; }
        internal DateTimeAndText AlarmTime { get; set; }
        internal IntAndText PredictedHour { get; set; }
        internal IntAndText PredictedMinute { get; set; }
        internal IntAndText AlarmHour { get; set; }
        internal IntAndText AlarmMinute { get; set; }
        internal TimeSpan AlarmAdvanceTime { get; set; }
        internal Bl_HypoPrediction()
        {
            GlucoseLast = new IntAndText(); 
            GlucosePrevious= new IntAndText(); 
            HypoGlucoseTarget= new IntAndText();
            GlucoseSlope = new DoubleAndText();
            GlucoseSlope.Format = "0.00"; 
            HourLast= new IntAndText(); 
            HourPrevious= new IntAndText(); 
            MinuteLast= new IntAndText(); 
            MinutePrevious= new IntAndText();
            PredictedTime = new DateTimeAndText();
            AlarmTime = new DateTimeAndText();
            PredictedHour = new IntAndText();
            PredictedMinute = new IntAndText();
            AlarmHour = new IntAndText();
            AlarmMinute = new IntAndText();

            alarm = new Alarm();   
            alarm.InitAlarm(); 
        }
        internal void PredictHypoTime()
        {
            try
            {
                int hourLast = HourLast.Int;
                int minuteLast = MinuteLast.Int;
                TimeLast = DateTime.Today;
                TimeLast = TimeLast.AddHours(HourLast.Int);
                TimeLast = TimeLast.AddMinutes(MinuteLast.Int);

                int hourPrevious = HourPrevious.Int;
                int minutePrevious = MinutePrevious.Int;

                // if the previous minute is in an hour before, subtract it to 60
                // and subtract 1 hour to the hours
                int differenceMinutes = MinuteLast.Int - MinutePrevious.Int;
                if (differenceMinutes < 0)
                {
                    minutePrevious = 60 + differenceMinutes;
                    hourLast--; // borrow on the next part of time 
                }
                else
                    minutePrevious = differenceMinutes;

                // if the previous date is in day before, subtract it to 24
                int differenceHours = hourLast - hourPrevious;
                if (differenceHours < 0)
                    hourPrevious = 24 + differenceHours;
                else
                    hourPrevious = differenceHours;

                Interval = new TimeSpan(hourPrevious, minutePrevious, 0);

                timePrevious = TimeLast.Subtract(Interval);

                double secondsBetweenMeasurements = Interval.TotalSeconds;

                // check if the difference in time is too big or too little
                if (secondsBetweenMeasurements > 3 * 60 * 60 || secondsBetweenMeasurements < 20 * 60)
                {
                    CommonFunctions.NotifyError("Time between measurements too big or too little");
                    GlucoseSlope.Text = "-";
                    PredictedTime.Text = "-";
                    AlarmTime.Text = "-";
                    PredictedHour.Text = "-";
                    PredictedMinute.Text = "-";
                    AlarmHour.Text = "-";
                    AlarmMinute.Text = "-";
                    return; 
                }
                int glucoseDifference = GlucoseLast.Int - GlucosePrevious.Int;

                if (glucoseDifference > 0) 
                {
                    // if glucose increases, hypo is not possible 
                    GlucoseSlope.Double = double.NaN;
                    PredictedTime.DateTime = DateTime.MinValue;
                    AlarmTime.DateTime = DateTime.MinValue;
                    return;
                }
                GlucoseSlope.Double = glucoseDifference / secondsBetweenMeasurements; // [Glucose Units/s]

                // calculate the instant when target glucose will be reached;
                // order of subtraction to achieve a positive number and to go forward in time 
                double predictedIntervalSeconds = (HypoGlucoseTarget.Int - GlucoseLast.Int) / GlucoseSlope.Double; 

                // if we should go back in time, we show an error condition
                if (predictedIntervalSeconds < 0)
                {
                    GlucoseSlope.Double = double.NaN;
                    PredictedTime.DateTime = DateTime.MaxValue;
                    AlarmTime.DateTime = DateTime.MaxValue;

                    return;
                }
                // change seconds to hours in slope
                GlucoseSlope.Double = GlucoseSlope.Double * 60 * 60;

                PredictedTime.Format = "yyyy.MM.dd HH:mm:ss";
                PredictedTime.DateTime = TimeLast.AddSeconds((int) predictedIntervalSeconds);
                AlarmTime.DateTime = PredictedTime.DateTime.Subtract(AlarmAdvanceTime);
                if (PredictedTime.DateTime == DateTime.MinValue ||
                        AlarmTime.DateTime == DateTime.MinValue)
                {
                    GlucoseSlope.Text = "-";
                    PredictedHour.Text = "-";
                    PredictedMinute.Text = "-";
                    AlarmHour.Text = "-";
                    AlarmMinute.Text = "-";
                    return;
                }
                DateTime? finalTime = PredictedTime.DateTime;
                DateTime? alarmTime = AlarmTime.DateTime;

                if (finalTime != null)
                {
                    PredictedHour.Text = ((DateTime)finalTime).Hour.ToString();
                    PredictedMinute.Text = ((DateTime)finalTime).Minute.ToString();
                    AlarmHour.Text = ((DateTime)alarmTime).Hour.ToString();
                    AlarmMinute.Text = ((DateTime)alarmTime).Minute.ToString();
                }
                else
                {
                    Console.Beep(200, 40);
                    PredictedHour.Text = "-";
                    PredictedMinute.Text = "-";
                    AlarmHour.Text = "-";
                    AlarmMinute.Text = "-";
                }

                SaveData();

                return;
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message); 
                return; 
            }
        }

        internal void StopAlarm()
        {
            alarm.StopAlarm();
        }

        internal void SetAlarm()
        {
            try
            {
                TimeSpan ts = AlarmTime.DateTime.Subtract(DateTime.Now);
                alarm.SetAlarm(ts);
                AlarmIsSet = true;
            }
            catch
            {
                AlarmIsSet = false;
            }
        }
        internal void SaveData()
        {
            try
            {
                dl.SaveHypoPrediction(this); 
            }
            catch (Exception ex)
            {
                CommonFunctions.NotifyError(ex.Message);
            }
        }
        internal void RestoreData()
        {
            dl.RestoreHypoPrediction(this); 
        }
    }
}
