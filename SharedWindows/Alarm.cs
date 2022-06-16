using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal partial class Alarm
    {
        // Windows dependant part of the Alarm class
        static System.Timers.Timer? alarm;
        // find a general way to trigger alarm
        static System.Media.SoundPlayer player;

        private bool playing;
        internal void InitAlarm()
        {
            alarm = new System.Timers.Timer();
            alarm.Elapsed += Alarm_Triggered;
        }
        internal void SetAlarm(TimeSpan AlarmTimeSpan)
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
                player = new System.Media.SoundPlayer();
                player.SoundLocation = @"C:\Windows\Media\Alarm03.wav";
                player.PlayLooping();
                playing = true;
            }
            catch (Exception ex)
            {

            }
        }
        internal void StopAlarm()
        {
            if (playing)
            {
                player.Stop();
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
