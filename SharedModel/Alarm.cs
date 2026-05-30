using gamon;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public partial class Alarm : System.ComponentModel.INotifyPropertyChanged
    {
        // platform independent part of class Alarm
        public int? IdAlarm { get; set; }
        // ReminderText: text to be shown when the alarm is triggered
        private string? _reminderText;
        public string? ReminderText
        {
            get => _reminderText;
            set
            {
                if (value == _reminderText) return;
                _reminderText = value;
                OnPropertyChanged(nameof(ReminderText));
            }
        }
        // TimeStart: date and time when the first accurrence of the
        // alarm will be triggered
        public DateTimeAndText TimeStart { get; set; }
        // NextTriggerTime: next time when the alarm will be triggered
        public DateTime? NextTriggerTime { get; set; }
        // IsDisabled: if true the alarm is temporary disabled and will be triggered
        public bool? IsDisabled { get; set; }
        // state of this alarm according to the enum AlarmRingingState
        public AlarmRingingState RingingState { get; set; }
        // StartupGraceWindow: when the program starts, an alarm whose scheduled time
        // already passed is still fired if the elapsed time since that scheduled time
        // is less than this value. E.g. if alarm time is 12:00 and grace = 30 min, starting
        // the program at 12:29 will still fire the alarm.
        public TimeSpan? StartupGraceWindow { get; set; }
        // Duration: time after which an alarm not dismissed will stop ringing [s]
        public TimeSpan? Duration { get; set; }
        // RepetitionTime: time after which an alarm that has rung in vain (was not dismissed
        // by the user) will restart ringing. Together with MaxRepeatCount it defines the
        // "restart when not dismissed" behavior.
        public TimeSpan? RepetitionTime { get; set; }
        // Interval: period of the regular repetition of the alarm. If null or <= 0
        // the alarm is fired only once.
        public TimeSpan? Interval { get; set; }
        // IsPlaying: if true the alarm is currently ringing
        public bool? IsPlaying { get; set; }
        // EnablePlaySoundFile: if true when the alarm is triggered a sound file will be played
        public bool? EnablePlaySoundFile { get; set; }
        // SoundFilePath: path of the sound file to be played when the alarm is triggered
        public string? SoundFilePath { get; set; }
        // RepeatCount: number of times the alarm has been restarted because it was not
        // dismissed (counter of "restart when not dismissed", not of periodic occurrences).
        public int? RepeatCount { get; set; }
        // MaxRepeatCount: maximum number of "restart when not dismissed" cycles allowed.
        // 0 or null means unlimited.
        public int? MaxRepeatCount { get; set; }
        // LastTriggerTime: last time when the alarm was triggered
        public DateTime? LastTriggerTime { get; set; }
        // TriggeredCount: number of times the alarm has been triggered
        public int? TriggeredCount { get; set; }
        // DoVibrate: if true the device will vibrate when the alarm is triggered
        public bool? DoVibrate { get; set; }
        // UI helper: marks this alarm as selected in lists
        private bool _isSelected;
        // legacy property kept for compatibility (not used by CollectionViews anymore)
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        // Standardized selection flag used across the app for list highlighting
        private bool _isSelectedInList;
        public bool IsSelectedInList
        {
            get => _isSelectedInList;
            set
            {
                if (_isSelectedInList == value) return;
                _isSelectedInList = value;
                OnPropertyChanged(nameof(IsSelectedInList));
                OnPropertyChanged(nameof(RowBorderColor));
            }
        }

        public string RowBorderColor => IsSelectedInList ? "Orange" : "Transparent";

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }
        
        public enum AlarmRingingState
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
            Expired,    // the alarm has reached MaxRepeatCount "restart when not dismissed"
                        // cycles, or has no future periodic occurrences left, and will not ring anymore
        }
        
        public Alarm()
        {
            TimeStart = new DateTimeAndText();
            RingingState = AlarmRingingState.Waiting;
        }
        
        /// <summary>
        /// Calculate and set the next trigger time for this alarm.
        /// Semantics:
        /// - Interval defines a regular repetition period; if null or <= 0 the alarm fires only once.
        /// - StartupGraceWindow allows firing an alarm whose scheduled time is in the past,
        ///   as long as the delay is within the grace window (handled at program startup).
        /// - RepeatCount / MaxRepeatCount count "restart when not dismissed" cycles, not periodic occurrences.
        /// </summary>
        public void CalculateNextTriggerTime()
        {
            if (IsDisabled == true)
            {
                RingingState = AlarmRingingState.Disabled;
                NextTriggerTime = null;
                return;
            }

            var startTime = TimeStart?.DateTime ?? DateTime.Now;
            bool hasPeriod = Interval.HasValue && Interval.Value.TotalSeconds > 0;

            // First firing: at TimeStart (the actual firing decision honors StartupGraceWindow
            // when the program runs). Until LastTriggerTime is set, the next trigger is startTime.
            if (!LastTriggerTime.HasValue)
            {
                // For periodic alarms, if TimeStart is in the past, calculate the next future occurrence
                // as a multiple of Interval from TimeStart
                if (hasPeriod && startTime < DateTime.Now)
                {
                    // Calculate how many intervals have passed since startTime
                    var elapsed = DateTime.Now - startTime;
                    var intervalsPassed = (long)Math.Floor(elapsed.TotalSeconds / Interval.Value.TotalSeconds);

                    // Next trigger is startTime + (intervalsPassed + 1) * Interval
                    NextTriggerTime = startTime.AddSeconds((intervalsPassed + 1) * Interval.Value.TotalSeconds);
                }
                else
                {
                    // One-shot alarm or startTime is in the future
                    NextTriggerTime = startTime;
                }

                RingingState = AlarmRingingState.Waiting;
                return;
            }

            // One-shot alarm: once it has triggered, no more occurrences.
            if (!hasPeriod)
            {
                NextTriggerTime = null;
                RingingState = AlarmRingingState.Expired;
                return;
            }

            // Periodic alarm: compute the next occurrence strictly in the future
            DateTime nextTime = LastTriggerTime.Value + Interval.Value;
            while (nextTime <= DateTime.Now)
            {
                nextTime += Interval.Value;
            }

            NextTriggerTime = nextTime;
            RingingState = AlarmRingingState.Waiting;
        }

        /// <summary>
        /// Check if this alarm is currently active (not expired, not disabled, not dismissed).
        /// An alarm becomes expired only when its RingingState says so (set by
        /// CalculateNextTriggerTime or by reaching MaxRepeatCount restarts).
        /// </summary>
        public bool IsActive()
        {
            if (IsDisabled == true) return false;
            if (RingingState == AlarmRingingState.Expired) return false;
            if (RingingState == AlarmRingingState.Dismissed) return false;
            return true;
        }
        
        /// <summary>
        /// Mark alarm as triggered: records that the alarm fired (either at scheduled time or
        /// at a periodic occurrence). RepeatCount is NOT incremented here, since it counts
        /// "restart when not dismissed" cycles only.
        /// </summary>
        public void MarkAsTriggered()
        {
            LastTriggerTime = DateTime.Now;
            TriggeredCount = (TriggeredCount ?? 0) + 1;
            RepeatCount = 0; // reset the "restart when not dismissed" counter for this firing
            RingingState = AlarmRingingState.Ringing;

            if (Interval.HasValue && Interval.Value.TotalSeconds > 0)
            {
                CalculateNextTriggerTime();
            }
        }

        /// <summary>
        /// Called when the alarm has rung in vain (was not dismissed and Duration elapsed):
        /// increments the restart counter and either suspends the alarm to restart after
        /// RepetitionTime, or marks it expired if MaxRepeatCount restarts have been reached.
        /// </summary>
        public void MarkAsRestartedWhenNotDismissed()
        {
            RepeatCount = (RepeatCount ?? 0) + 1;

            if (MaxRepeatCount.HasValue && MaxRepeatCount.Value > 0 &&
                RepeatCount.Value >= MaxRepeatCount.Value)
            {
                RingingState = AlarmRingingState.Expired;
                NextTriggerTime = null;
                return;
            }

            RingingState = AlarmRingingState.AutoSuspended;
            if (RepetitionTime.HasValue && RepetitionTime.Value.TotalSeconds > 0)
            {
                NextTriggerTime = DateTime.Now + RepetitionTime.Value;
            }
        }
        
        /// <summary>
        /// Dismiss this alarm (no more triggers)
        /// </summary>
        public void Dismiss()
        {
            RingingState = AlarmRingingState.Dismissed;
            IsPlaying = false;
            NextTriggerTime = null;
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
