using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public class Alarm
    {
        // platform independent part of class Alarm
        public int? IdAlarm { get; set; }
        public DateTimeAndText TimeStart { get; set; }
        public DateTimeAndText TimeAlarm { get; set; }
        public TimeSpan? Interval { get; set; }
        public TimeSpan? Duration { get; set; }
        public bool? IsRepeated { get; set; }
        public bool? IsEnabled { get; set; }
        public Alarm()
        {
            TimeStart = new DateTimeAndText();
            TimeAlarm = new DateTimeAndText();
        }

        // Windows dependant part of the Alarm class
        static System.Timers.Timer? alarm;
        // find a general way to trigger alarm
        ////////static System.Media.SoundPlayer player;

        private bool playing;
        public void InitAlarm()
        {
            alarm = new System.Timers.Timer();
            alarm.Elapsed += Alarm_Triggered;
        }
        public void SetAlarm(TimeSpan AlarmTimeSpan)
        {
            alarm.Interval = AlarmTimeSpan.TotalMilliseconds;
            alarm.Start();
        }
        private void Alarm_Triggered(object sender, System.Timers.ElapsedEventArgs e)
        {
            alarm.Stop();
            try
            {
                // find a general way to trigger alarm
                ////////player = new System.Media.SoundPlayer();
                ////////player.SoundLocation = @"C:\Windows\Media\Alarm03.wav";
                ////////player.PlayLooping();
                playing = true;
            }
            catch (Exception ex)
            {

            }
        }
        public void StopAlarm()
        {
            if (playing)
            {
                ////////player.Stop();
                playing = false;
            }
            else
            {
                alarm.Stop();
                alarm.Dispose(); 
            }
        }
    }
}
