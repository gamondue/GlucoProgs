using System;
using SharedData;
using GlucoMan;

namespace GlucoMan.BusinessLayer
{
    public class BL_HypoPrediction
    {
        DataLayer dl = Common.Database;

        private TimeSpan interval;
        private bool alarmIsSet = false;

        private Alarm alarm; 

        public bool AlarmIsSet { get => alarmIsSet; set => alarmIsSet = value; }
        public string statusMessage; 
        public DateTime TimeLast { get; set; }
        public DateTime TimePrevious { get; set; }
        public IntAndText GlucoseLast { get; set; }
        public IntAndText GlucosePrevious { get; set; }
        public IntAndText GlucoseTarget { get; set; }
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
        public DoubleAndText AlarmAdvanceTime { get; set; }
        public string StatusMessage { get => statusMessage; }
        public DoubleAndText FutureSpanMinutes { get; internal set; }
        public DateTimeAndText FutureTime { get; internal set; }
        public DoubleAndText PredictedGlucose { get; internal set; }

        public BL_HypoPrediction()
        {
            GlucoseLast = new IntAndText(); 
            GlucosePrevious= new IntAndText(); 
            GlucoseTarget= new IntAndText();
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
            AlarmAdvanceTime = new DoubleAndText(); 
            AlarmHour = new IntAndText();
            AlarmMinute = new IntAndText();
            FutureSpanMinutes = new DoubleAndText();
            PredictedGlucose = new DoubleAndText();
            FutureTime = new DateTimeAndText(); 

            alarm = new Alarm();   
            alarm.InitAlarm(); 
        }
        public void PredictHypoTime()
        {
            try
            {
                statusMessage = ""; 

                int? hourLast = HourLast.Int;
                int? minuteLast = MinuteLast.Int;
                TimeLast = DateTime.Today;
                TimeLast = TimeLast.AddHours((int)HourLast.Int);
                TimeLast = TimeLast.AddMinutes((int)MinuteLast.Int);

                int? hourPrevious = HourPrevious.Int;
                int? minutePrevious = MinutePrevious.Int;

                // if the previous minute is in an hour before, subtract it to 60
                // and subtract 1 hour to the hours
                int? differenceMinutes = MinuteLast.Int - MinutePrevious.Int;
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

                TimePrevious = TimeLast.Subtract(Interval);

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
                int? glucoseDifference = GlucoseLast.Int - GlucosePrevious.Int;

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
                double? predictedIntervalSeconds = (GlucoseTarget.Int - GlucoseLast.Int) / GlucoseSlope.Double; 

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
                DateTime dummy = (DateTime)PredictedTime.DateTime;
                
                AlarmTime.DateTime = dummy.Subtract(new TimeSpan(0, (int)AlarmAdvanceTime.Double, 0));
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
        internal void PredictGlucose()
        {
            FutureTime.DateTime = TimeLast.Add(new TimeSpan(0, (int)FutureSpanMinutes.Double, 0)); 
            PredictedGlucose.Double = (double)GlucoseLast.Int + GlucoseSlope.Double * FutureSpanMinutes.Double / 60; 
        }
        public void StopAlarm()
        {
            alarm.StopAlarm();
        }
        public void SetAlarm()
        {
            try
            {
                DateTime dummy = (DateTime)AlarmTime.DateTime; 
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
                dl.SaveParameter("Hypo_GlucoseTarget", GlucoseTarget.Text);
                dl.SaveParameter("Hypo_GlucoseLast", GlucoseLast.Text);
                dl.SaveParameter("Hypo_GlucosePrevious", GlucosePrevious.Text);
                dl.SaveParameter("Hypo_HourLast", HourLast.Text);
                dl.SaveParameter("Hypo_HourPrevious", HourPrevious.Text);
                dl.SaveParameter("Hypo_MinuteLast", MinuteLast.Text);
                dl.SaveParameter("Hypo_MinutePrevious", MinutePrevious.Text);
                dl.SaveParameter("Hypo_AlarmAdvanceTime", AlarmAdvanceTime.Text);
                dl.SaveParameter("Hypo_FutureSpanMinutes", FutureSpanMinutes.Text);
            }
            catch (Exception ex)
            {
                Common.LogOfProgram.Error("BL_HypoPrediction | SaveData", ex);
            }
        }
        public void RestoreData()
        {
            GlucoseTarget.Text = dl.RestoreParameter("Hypo_GlucoseTarget");
            GlucoseLast.Text = dl.RestoreParameter("Hypo_GlucoseLast");
            GlucosePrevious.Text = dl.RestoreParameter("Hypo_GlucosePrevious");
            HourLast.Text = dl.RestoreParameter("Hypo_HourLast");
            HourPrevious.Text = dl.RestoreParameter("Hypo_HourPrevious");
            MinuteLast.Text = dl.RestoreParameter("Hypo_MinuteLast");
            MinutePrevious.Text = dl.RestoreParameter("Hypo_MinutePrevious");
            int? minutes = Safe.Int(dl.RestoreParameter("Hypo_AlarmAdvanceTime"));
            if (minutes == null)
                minutes = 0;
            AlarmAdvanceTime.Text = dl.RestoreParameter("Hypo_AlarmAdvanceTime");
            FutureSpanMinutes.Text = dl.RestoreParameter("Hypo_FutureSpanMinutes");
        }
    }
}
