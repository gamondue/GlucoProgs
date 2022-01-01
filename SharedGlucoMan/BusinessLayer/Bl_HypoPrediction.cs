using System;
using SharedData;
using GlucoMan;
using SharedObjects;

namespace GlucoMan.BusinessLayer
{
    public class BL_HypoPrediction
    {
        DataLayer dl = new DataLayer(); 

        private DateTime timeLast;
        private DateTime timePrevious;
        private TimeSpan interval;
        private bool alarmIsSet = false;

        private Alarm alarm; 

        public bool AlarmIsSet { get => alarmIsSet; set => alarmIsSet = value; }
        public string statusMessage; 
        public DateTime TimeLast { get => timeLast; set => timeLast = value; }
        public DateTime TimePrevious { get => timePrevious; }
        public IntAndText GlucoseLast { get; set; }
        public IntAndText GlucosePrevious { get; set; }
        public IntAndText HypoGlucoseTarget { get; set; }
        public DoubleAndText GlucoseSlope { get; }
        public TimeSpan Interval { get => interval; set => interval = value; }
        public IntAndText HourLast { get; set; }
        public IntAndText HourPrevious { get; set; }
        public IntAndText MinuteLast { get; set; }
        public IntAndText MinutePrevious { get; set; }
        public DateTimeAndText PredictedTime { get; set; }
        public DateTimeAndText AlarmTime { get; set; }
        public IntAndText PredictedHour { get; set; }
        public IntAndText PredictedMinute { get; set; }
        public IntAndText AlarmHour { get; set; }
        public IntAndText AlarmMinute { get; set; }
        public TimeSpan AlarmAdvanceTime { get; set; }
        public string StatusMessage { get => statusMessage; }
        public BL_HypoPrediction()
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
        public void PredictHypoTime()
        {
            try
            {
                statusMessage = ""; 

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

                // if the previous date is in the day before, subtract it to 24
                int differenceHours = hourLast - hourPrevious;
                if (differenceHours < 0)
                    hourPrevious = 24 + differenceHours;
                else
                    hourPrevious = differenceHours;

                Interval = new TimeSpan(hourPrevious, minutePrevious, 0);

                timePrevious = TimeLast.Subtract(Interval);

                double secondsBetweenMeasurements = Interval.TotalSeconds;

                // check if the difference in time is too big or too little
                if (secondsBetweenMeasurements > 3 * 60 * 60)
                {
                    //Console.Beep(200, 40);
                    Common.LogOfProgram.Error("Time between measurements too big", null);
                    GlucoseSlope.Text = "----";
                    PredictedTime.Text = ">";
                    AlarmTime.Text = ">";
                    PredictedHour.Text = "> 3 h";
                    PredictedMinute.Text = ">";
                    AlarmHour.Text = ">";
                    AlarmMinute.Text = ">";
                    statusMessage = "Time between measurements too big";
                    return; 
                }
                if (secondsBetweenMeasurements < 20 * 60)
                {
                    //Console.Beep(200, 40);
                    Common.LogOfProgram.Error("Time between measurements too short", null);
                    GlucoseSlope.Text = "----";
                    PredictedTime.Text = "<";
                    AlarmTime.Text = "<";
                    PredictedHour.Text = "< 20 min";
                    PredictedMinute.Text = "<";
                    AlarmHour.Text = "<";
                    AlarmMinute.Text = "<";
                    statusMessage = "Time between measurements too short";
                    return;
                }
                int glucoseDifference = GlucoseLast.Int - GlucosePrevious.Int;

                if (glucoseDifference > 0) 
                {
                    // if glucose increases, hypo is not possible 
                    //Console.Beep(200, 40);
                    GlucoseSlope.Text = "----";
                    GlucoseSlope.Double = glucoseDifference / secondsBetweenMeasurements; // [Glucose Units/s]
                    GlucoseSlope.Double = GlucoseSlope.Double * 60 * 60;
                    PredictedTime.DateTime = DateTime.MinValue;
                    AlarmTime.DateTime = DateTime.MinValue;
                    PredictedTime.Text = "----";
                    AlarmTime.Text = "----";
                    PredictedHour.Text = "increas";
                    PredictedMinute.Text = "----";
                    AlarmHour.Text = "----";
                    AlarmMinute.Text = "----";
                    statusMessage = "Glucose increasing, hypoglicemia not possible";
                    return;
                }
                GlucoseSlope.Double = glucoseDifference / secondsBetweenMeasurements; // [Glucose Units/s]

                // calculate the instant when target glucose will be reached;
                // order of subtraction to achieve a positive number and to go forward in time 
                double predictedIntervalSeconds = (HypoGlucoseTarget.Int - GlucoseLast.Int) / GlucoseSlope.Double; 

                // if we should go back in time, we show an error condition
                if (predictedIntervalSeconds < 0)
                {
                    //Console.Beep(200, 40);
                    GlucoseSlope.Text = "----";
                    PredictedTime.DateTime = DateTime.MaxValue;
                    AlarmTime.DateTime = DateTime.MaxValue;
                    PredictedTime.Text = "----";
                    AlarmTime.Text = "----";
                    PredictedHour.Text = "back";
                    PredictedMinute.Text = "----";
                    AlarmHour.Text = "----";
                    AlarmMinute.Text = "----";
                    statusMessage = "Calculated final time should be less than now";
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
                    // Console.Beep(200, 40);
                    GlucoseSlope.Text = "----";
                    PredictedHour.Text = "----"; 
                    PredictedMinute.Text = "----"; ;
                    AlarmHour.Text = "----"; ;
                    AlarmMinute.Text = "----"; ;
                    statusMessage = "----"; ; // put somethong better!!!!
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
                    // Console.Beep(200, 40);
                    PredictedHour.Text = "----"; ; 
                    PredictedMinute.Text = "----";
                    AlarmHour.Text = "----";
                    AlarmMinute.Text = "----";
                    statusMessage = "Wrong calculations"; 
                }
                SaveDataHypo();
                return;
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("BL_HypoPrediction | PredictHypoTime()", ex); 
                return; 
            }
        }
        public void StopAlarm()
        {
            alarm.StopAlarm();
        }
        public void SetAlarm()
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
        public void SaveDataHypo()
        {
            try
            {
                dl.SaveHypoPrediction(this); 
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("BL_HypoPrediction | SaveData", ex);
            }
        }
        public void RestoreData()
        {
            dl.RestoreHypoPrediction(this); 
        }
    }
}
