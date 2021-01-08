using System;
using System.IO;
using SharedData;

namespace GlucoMan.BusinessLayer
{
    internal class HypoPrediction
    {
        string persistentStorage = CommonData.PathConfigurationData + @"HypoPrediction.txt";

        private DateTime timeLast;
        private DateTime timePrevious;
        private TimeSpan interval;

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
        
        internal HypoPrediction()
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
    }
        internal DateTime? PredictHypoTime()
        {
            DateTime? finalTime; 
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

                int glucoseDifference = GlucoseLast.Int - GlucosePrevious.Int;

                if (glucoseDifference > 0) 
                {
                    // if glucose increases, hypo is not possible 
                    GlucoseSlope.Double = double.NaN;
                    PredictedTime.DateTime = DateTime.MaxValue;
                    return null;
                }
                GlucoseSlope.Double = glucoseDifference / secondsBetweenMeasurements; // [Glucose Units/s]

                // calculate the instant when target glucose will be reached;
                // order of subtraction to achieve a positive number and to go forward in time 
                double predictedIntervalSeconds = (HypoGlucoseTarget.Int - GlucoseLast.Int) / GlucoseSlope.Double; 

                // if we should go back in time, we set an error condition
                if (predictedIntervalSeconds < 0)
                {
                    GlucoseSlope.Double = double.NaN;
                    PredictedTime.DateTime = DateTime.MaxValue;
                    return null;
                }
                // changhe seconds to hours in 
                GlucoseSlope.Double = GlucoseSlope.Double * 60 * 60;
                
                PredictedTime.DateTime = TimeLast.AddSeconds((int) predictedIntervalSeconds);

                SaveData();
                return PredictedTime.DateTime;
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        internal void SaveData()
        {
            try
            {
                string file = HypoGlucoseTarget.Text + "\n";
                file += GlucoseLast.Text + "\n";
                file += GlucosePrevious.Text + "\n";
                file += HourLast.Text + "\n";
                file += HourPrevious.Text + "\n";
                file += MinuteLast.Text + "\n";
                file += MinutePrevious.Text;
                TextFile.StringToFile(persistentStorage, file, false);
            }
            catch (Exception ex)
            {
                Console.Beep();
            }
        }

        internal HypoPrediction RestoreData()
        {
            HypoPrediction h = null;
            if (File.Exists(persistentStorage))
                try
                {
                    string[] f = TextFile.FileToArray(persistentStorage);
                    HypoGlucoseTarget.Text = f[0];
                    GlucoseLast.Text = f[1];
                    GlucosePrevious.Text = f[2];
                    HourLast.Text = f[3];
                    HourPrevious.Text = f[4];
                    MinuteLast.Text = f[5];
                    MinutePrevious.Text = f[6];
                }
                catch (Exception ex)
                {
                    Console.Beep();
                }
            return h;
        }
    }
}
