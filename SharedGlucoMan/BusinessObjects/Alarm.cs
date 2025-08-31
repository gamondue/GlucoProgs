using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public partial class Alarm
    {
        // platform independent part of class Alarm
        public int? IdAlarm { get; set; }
        // ReminderText: text to be shown when the alarm is triggered
        public string? ReminderText { get; set; }
        // TimeStart: date and time when the first accurrence of the
        // alarm will be triggered
        public DateTimeAndText TimeStart { get; set; }
        // NextTriggerTime: next time when the alarm will be triggered
        public DateTime? NextTriggerTime { get; set; }
        // IsDisabled: if true the alarm is temporary disabled and will be triggered
        public bool? IsDisabled { get; set; }
        // state of this alarm according to the enum AlarmRingingState
        AlarmRingingState RingingState { get; set; }
        // interval after TimeStart when the alarm will still be triggered [s]
        // after TimeStart + ValidTimeAfterStart the alarm will not be triggered anymore
        public TimeSpan? ValidTimeAfterStart { get; set; }
        // Duration: time after which an alarm not dismissed will stop ringing [s]
        public TimeSpan? Duration { get; set; }
        // Repetition time: time when the alarm will be repeated after
        // vain ringing (not stopped nor delayed by the user)
        public TimeSpan? RepetitionTime { get; set; }
        // Interval: time  after which an enabled periodic alarm will be re-triggered [s]
        public TimeSpan? Interval { get; set; }
        // IsPlaying: if true the alarm is currently ringing
        public bool? IsPlaying { get; set; }
        // EnablePlaySoundFile: if true when the alarm is triggered a sound file will be played
        public bool? EnablePlaySoundFile { get; set; }
        // SoundFilePath: path of the sound file to be played when the alarm is triggered
        public string? SoundFilePath { get; set; }
        // RepeatCount: number of times the alarm has been repeated
        public int? RepeatCount { get; set; }
        // MaxRepeatCount: maximum number of times the alarm will be repeated
        public int? MaxRepeatCount { get; set; }
        // LastTriggerTime: last time when the alarm was triggered
        public DateTime? LastTriggerTime { get; set; }
        // TriggeredCount: number of times the alarm has been triggered
        public int? TriggeredCount { get; set; }
        // DoVibrate: if true the device will vibrate when the alarm is triggered
        public bool? DoVibrate { get; set; }
        enum AlarmRingingState
        {
            Waiting,    // the alarm is active and waiting to be triggered
            Disabled,   // the alarm is temporarily disabled and will not ring
            Ringing,    // the alarm is currently ringing and waiting to be:
                        // automatically stopped by the program after Duration time
                        // or delayed by the user to ring again after Delay time
                        // or dismissed 
            Dismissed,  // the alarm has been dismissed by the user and will not ring again
            Delayed,    // the alarm is delayed by the user and will ring after the delay time
            AutoSuspended, // the alarm has rung in vain for Duration time, hence has been suspended
                          // by the program and will ring again after RepetitionTime
            Expired,    // the alarm has expired (TimeStart + ValidTimeAfterStart <= DateTime.Now)
                        // and will not ring anymore
        }
        public Alarm()
        {
            TimeStart = new DateTimeAndText();
        }
        public void InitAlarm()
        {
            //alarm = new System.Timers.Timer();
            //alarm.Elapsed += Alarm_Triggered;
        }
        public void SetAlarm(TimeSpan AlarmTimeSpan)
        {
            //alarm.Interval = AlarmTimeSpan.TotalMilliseconds;
            //alarm.Start();
        }
        private void Alarm_Triggered(object sender, System.Timers.ElapsedEventArgs e)
        {
            //alarm.Stop();
            //try
            //{
            //}
            //catch (Exception ex)
            //{

            //}
        }
        public void StopAlarm()
        {
            //if (playing)
            //{
            //    player.Stop();
            //    playing = false;
            //}
            //else
            //{
            //    alarm.Stop();
            //    alarm.Dispose();
            //}
        }
    }
}
