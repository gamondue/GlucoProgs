using System;
using SharedData;
using GlucoMan;

namespace GlucoMan.BusinessLayer
{
    public class BL_HypoPrediction
    {
        DataLayer dl = Common.Database;

        private DateTime timeLast;
        private DateTime timePrevious;
        private TimeSpan interval;
        private bool alarmIsSet = false;

        private Alarm alarm; 

        public bool AlarmIsSet { get => alarmIsSet; set => alarmIsSet = value; }
        public string statusMessage; 
        public DateTime HypoTimeLast { get => timeLast; set => timeLast = value; }
        public DateTime HypoTimePrevious { get => timePrevious; }
        public IntAndText Hypo_GlucoseLast { get; set; }
        public IntAndText Hypo_GlucosePrevious { get; set; }
        public IntAndText Hypo_GlucoseTarget { get; set; }
        public DoubleAndText HypoGlucoseSlope { get; }
        public TimeSpan Interval { get => interval; set => interval = value; }
        public IntAndText Hypo_HourLast { get; set; }
        public IntAndText Hypo_HourPrevious { get; set; }
        public IntAndText Hypo_MinuteLast { get; set; }
        public IntAndText Hypo_MinutePrevious { get; set; }
        public DateTimeAndText HypoPredictedTime { get; set; }
        public DateTimeAndText HypoAlarmTime { get; set; }
        public IntAndText PredictedHour { get; set; }
        public IntAndText PredictedMinute { get; set; }
        public IntAndText AlarmHour { get; set; }
        public IntAndText AlarmMinute { get; set; }
        public TimeSpan Hypo_AlarmAdvanceTime { get; set; }
        public string StatusMessage { get => statusMessage; }
        public BL_HypoPrediction()
        {
            Hypo_GlucoseLast = new IntAndText(); 
            Hypo_GlucosePrevious= new IntAndText(); 
            Hypo_GlucoseTarget= new IntAndText();
            HypoGlucoseSlope = new DoubleAndText();
            HypoGlucoseSlope.Format = "0.00"; 
            Hypo_HourLast= new IntAndText(); 
            Hypo_HourPrevious= new IntAndText(); 
            Hypo_MinuteLast= new IntAndText(); 
            Hypo_MinutePrevious= new IntAndText();
            HypoPredictedTime = new DateTimeAndText();
            HypoAlarmTime = new DateTimeAndText();
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

                int? hourLast = Hypo_HourLast.Int;
                int? minuteLast = Hypo_MinuteLast.Int;
                HypoTimeLast = DateTime.Today;
                HypoTimeLast = HypoTimeLast.AddHours((int)Hypo_HourLast.Int);
                HypoTimeLast = HypoTimeLast.AddMinutes((int)Hypo_MinuteLast.Int);

                int? hourPrevious = Hypo_HourPrevious.Int;
                int? minutePrevious = Hypo_MinutePrevious.Int;

                // if the previous minute is in an hour before, subtract it to 60
                // and subtract 1 hour to the hours
                int? differenceMinutes = Hypo_MinuteLast.Int - Hypo_MinutePrevious.Int;
                if (differenceMinutes < 0)
                {
                    minutePrevious = 60 + differenceMinutes;
                    hourLast--; // borrow on the next part of time 
                }
                else
                    minutePrevious = differenceMinutes;

                // if the previous date is in the day before, subtract it to 24
                int? differenceHours = hourLast - hourPrevious;
                if (differenceHours < 0)
                    hourPrevious = 24 + differenceHours;
                else
                    hourPrevious = differenceHours;

                Interval = new TimeSpan((int)hourPrevious, (int)minutePrevious, 0);

                timePrevious = HypoTimeLast.Subtract(Interval);

                double secondsBetweenMeasurements = Interval.TotalSeconds;

                // check if the difference in time is too big or too little
                if (secondsBetweenMeasurements > 3 * 60 * 60)
                {
                    //Console.Beep(200, 40);
                    Common.LogOfProgram.Error("Time between measurements too big", null);
                    HypoGlucoseSlope.Text = "----";
                    HypoPredictedTime.Text = ">";
                    HypoAlarmTime.Text = ">";
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
                    HypoGlucoseSlope.Text = "----";
                    HypoPredictedTime.Text = "<";
                    HypoAlarmTime.Text = "<";
                    PredictedHour.Text = "< 20 min";
                    PredictedMinute.Text = "<";
                    AlarmHour.Text = "<";
                    AlarmMinute.Text = "<";
                    statusMessage = "Time between measurements too short";
                    return;
                }
                int? glucoseDifference = Hypo_GlucoseLast.Int - Hypo_GlucosePrevious.Int;

                if (glucoseDifference > 0) 
                {
                    // if glucose increases, hypo is not possible 
                    //Console.Beep(200, 40);
                    HypoGlucoseSlope.Text = "----";
                    HypoGlucoseSlope.Double = glucoseDifference / secondsBetweenMeasurements; // [Glucose Units/s]
                    HypoGlucoseSlope.Double = HypoGlucoseSlope.Double * 60 * 60;
                    HypoPredictedTime.DateTime = DateTime.MinValue;
                    HypoAlarmTime.DateTime = DateTime.MinValue;
                    HypoPredictedTime.Text = "----";
                    HypoAlarmTime.Text = "----";
                    PredictedHour.Text = "increas";
                    PredictedMinute.Text = "----";
                    AlarmHour.Text = "----";
                    AlarmMinute.Text = "----";
                    statusMessage = "Glucose increasing, hypoglicemia not possible";
                    return;
                }
                HypoGlucoseSlope.Double = glucoseDifference / secondsBetweenMeasurements; // [Glucose Units/s]

                // calculate the instant when target glucose will be reached;
                // order of subtraction to achieve a positive number and to go forward in time 
                double? predictedIntervalSeconds = (Hypo_GlucoseTarget.Int - Hypo_GlucoseLast.Int) / HypoGlucoseSlope.Double; 

                // if we should go back in time, we show an error condition
                if (predictedIntervalSeconds < 0)
                {
                    //Console.Beep(200, 40);
                    HypoGlucoseSlope.Text = "----";
                    HypoPredictedTime.DateTime = DateTime.MaxValue;
                    HypoAlarmTime.DateTime = DateTime.MaxValue;
                    HypoPredictedTime.Text = "----";
                    HypoAlarmTime.Text = "----";
                    PredictedHour.Text = "back";
                    PredictedMinute.Text = "----";
                    AlarmHour.Text = "----";
                    AlarmMinute.Text = "----";
                    statusMessage = "Calculated final time should be less than now";
                    return;
                }
                // change seconds to hours in slope
                HypoGlucoseSlope.Double = HypoGlucoseSlope.Double * 60 * 60;

                HypoPredictedTime.Format = "yyyy.MM.dd HH:mm:ss";
                HypoPredictedTime.DateTime = HypoTimeLast.AddSeconds((int) predictedIntervalSeconds);
                DateTime dummy = (DateTime)HypoPredictedTime.DateTime;
                HypoAlarmTime.DateTime = dummy.Subtract(Hypo_AlarmAdvanceTime);
                if (HypoPredictedTime.DateTime == DateTime.MinValue ||
                        HypoAlarmTime.DateTime == DateTime.MinValue)
                {
                    // Console.Beep(200, 40);
                    HypoGlucoseSlope.Text = "----";
                    PredictedHour.Text = "----"; 
                    PredictedMinute.Text = "----"; ;
                    AlarmHour.Text = "----"; ;
                    AlarmMinute.Text = "----"; ;
                    statusMessage = "----"; ; // put somethong better!!!!
                    return;
                }
                DateTime? finalTime = HypoPredictedTime.DateTime;
                DateTime? alarmTime = HypoAlarmTime.DateTime;

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
                DateTime dummy = (DateTime)HypoAlarmTime.DateTime; 
                TimeSpan ts = dummy.Subtract(DateTime.Now);
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
