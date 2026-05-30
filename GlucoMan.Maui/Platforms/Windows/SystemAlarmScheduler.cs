#if WINDOWS
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GlucoMan;
using SystemTimer = System.Timers.Timer;

namespace GlucoMan.Maui.Platforms.Windows
{
    /// <summary>
    /// Windows alarm scheduler using a two-phase approach for precise timing:
    /// Phase 1: Low-power timer until 1 minute before trigger
    /// Phase 2: High-precision active loop for the last minute
    /// Note: This requires the app to be running. For production, consider Windows Task Scheduler.
    /// </summary>
    public class SystemAlarmScheduler : ISystemAlarmScheduler
    {
        private static readonly Dictionary<int, SystemTimer> _activeTimers = new();
        private static readonly Dictionary<int, CancellationTokenSource> _activePrecisionTasks = new();
        private static readonly object _lock = new();

        // Time before alarm trigger to switch to high-precision mode
        private static readonly TimeSpan PrecisionModeThreshold = TimeSpan.FromMinutes(1);

        public Task ScheduleAsync(Alarm alarm)
        {
            try
            {
                if (alarm == null) throw new ArgumentNullException(nameof(alarm));
                if (alarm.IdAlarm == null) throw new ArgumentException("Alarm must have IdAlarm before scheduling");

                var baseStart = alarm.TimeStart?.DateTime ?? DateTime.Now;
                DateTime when = alarm.NextTriggerTime ?? baseStart;

                // Calculate time until alarm
                var timeUntilAlarm = when - DateTime.Now;

                if (timeUntilAlarm.TotalMilliseconds <= 0)
                {
                    gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} is in the past, scheduling for immediate trigger");
                    timeUntilAlarm = TimeSpan.FromSeconds(1); // Trigger in 1 second
                }

                gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Scheduling alarm {alarm.IdAlarm} for {when:yyyy-MM-dd HH:mm:ss} (in {timeUntilAlarm.TotalMinutes:F1} minutes)");

                lock (_lock)
                {
                    // Cancel existing timer/task if any
                    CancelScheduledAlarm(alarm.IdAlarm.Value);

                    // Decide which scheduling strategy to use
                    if (timeUntilAlarm <= PrecisionModeThreshold)
                    {
                        // Less than 1 minute: use high-precision mode immediately
                        gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} within precision threshold, using high-precision mode immediately");
                        StartPrecisionMode(alarm, when);
                    }
                    else
                    {
                        // More than 1 minute: use low-power timer until 1 minute before
                        var timeUntilPrecisionMode = timeUntilAlarm - PrecisionModeThreshold;
                        gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} using two-phase approach: low-power for {timeUntilPrecisionMode.TotalMinutes:F1} min, then precision mode");

                        var timer = new SystemTimer(timeUntilPrecisionMode.TotalMilliseconds)
                        {
                            AutoReset = false
                        };

                        timer.Elapsed += (sender, e) =>
                        {
                            gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} switching to precision mode");

                            lock (_lock)
                            {
                                // Remove the timer
                                if (_activeTimers.ContainsKey(alarm.IdAlarm.Value))
                                {
                                    _activeTimers.Remove(alarm.IdAlarm.Value);
                                }

                                // Start precision mode
                                StartPrecisionMode(alarm, when);
                            }
                        };

                        timer.Start();
                        _activeTimers[alarm.IdAlarm.Value] = timer;
                    }
                }

                gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Successfully scheduled alarm {alarm.IdAlarm}");
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error($"Windows SystemAlarmScheduler.ScheduleAsync - Alarm {alarm?.IdAlarm}", ex);
                throw new InvalidOperationException($"Failed to schedule alarm on Windows: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Starts high-precision active polling mode for the last minute before alarm trigger
        /// </summary>
        private void StartPrecisionMode(Alarm alarm, DateTime triggerTime)
        {
            var cts = new CancellationTokenSource();
            _activePrecisionTasks[alarm.IdAlarm.Value] = cts;

            Task.Run(async () =>
            {
                try
                {
                    gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Precision mode started for alarm {alarm.IdAlarm}, target: {triggerTime:HH:mm:ss.fff}");

                    while (!cts.Token.IsCancellationRequested)
                    {
                        var now = DateTime.Now;
                        var remaining = triggerTime - now;

                        // Check if it's time to trigger
                        if (remaining.TotalMilliseconds <= 0)
                        {
                            gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Precision trigger for alarm {alarm.IdAlarm} at {now:HH:mm:ss.fff} (scheduled: {triggerTime:HH:mm:ss.fff}, delay: {-remaining.TotalMilliseconds:F0}ms)");

                            // Trigger the alarm
                            OnAlarmTriggered(alarm);

                            // Clean up
                            lock (_lock)
                            {
                                if (_activePrecisionTasks.ContainsKey(alarm.IdAlarm.Value))
                                {
                                    _activePrecisionTasks.Remove(alarm.IdAlarm.Value);
                                }
                            }

                            break;
                        }

                        // Adaptive sleep: longer intervals when further away, shorter when close
                        int sleepMs;
                        if (remaining.TotalSeconds > 10)
                        {
                            sleepMs = 1000; // 1 second when >10s away
                        }
                        else if (remaining.TotalSeconds > 2)
                        {
                            sleepMs = 100; // 100ms when 2-10s away
                        }
                        else
                        {
                            sleepMs = 10; // 10ms when very close
                        }

                        await Task.Delay(sleepMs, cts.Token);
                    }

                    gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Precision mode ended for alarm {alarm.IdAlarm}");
                }
                catch (OperationCanceledException)
                {
                    gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Precision mode cancelled for alarm {alarm.IdAlarm}");
                }
                catch (Exception ex)
                {
                    gamon.General.LogOfProgram?.Error($"Windows SystemAlarmScheduler: Precision mode error for alarm {alarm.IdAlarm}", ex);
                }
            }, cts.Token);
        }

        /// <summary>
        /// Cancels any scheduled timer or precision task for the given alarm
        /// </summary>
        private void CancelScheduledAlarm(int idAlarm)
        {
            // Cancel timer if exists
            if (_activeTimers.TryGetValue(idAlarm, out var existingTimer))
            {
                existingTimer.Stop();
                existingTimer.Dispose();
                _activeTimers.Remove(idAlarm);
            }

            // Cancel precision task if exists
            if (_activePrecisionTasks.TryGetValue(idAlarm, out var existingCts))
            {
                existingCts.Cancel();
                existingCts.Dispose();
                _activePrecisionTasks.Remove(idAlarm);
            }
        }

        public Task CancelAsync(int idAlarm)
        {
            try
            {
                lock (_lock)
                {
                    CancelScheduledAlarm(idAlarm);
                    gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Cancelled alarm {idAlarm}");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error($"Windows SystemAlarmScheduler.CancelAsync - Alarm {idAlarm}", ex);
                throw new InvalidOperationException($"Failed to cancel alarm on Windows: {ex.Message}", ex);
            }
        }

        public Task CancelAllAsync()
        {
            try
            {
                lock (_lock)
                {
                    int timerCount = _activeTimers.Count;
                    int taskCount = _activePrecisionTasks.Count;

                    // Cancel all timers
                    foreach (var timer in _activeTimers.Values)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    _activeTimers.Clear();

                    // Cancel all precision tasks
                    foreach (var cts in _activePrecisionTasks.Values)
                    {
                        cts.Cancel();
                        cts.Dispose();
                    }
                    _activePrecisionTasks.Clear();

                    gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Cancelled all alarms ({timerCount} timers, {taskCount} precision tasks)");
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("Windows SystemAlarmScheduler.CancelAllAsync", ex);
                throw new InvalidOperationException($"Failed to cancel all alarms on Windows: {ex.Message}", ex);
            }
        }

        private void OnAlarmTriggered(Alarm alarm)
        {
            try
            {
                gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} triggered - {alarm.ReminderText}");

                // Show notification dialog on main thread
                Microsoft.Maui.ApplicationModel.MainThread.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await ShowAlarmNotification(alarm);
                    }
                    catch (Exception ex)
                    {
                        gamon.General.LogOfProgram?.Error($"Error showing alarm notification for {alarm.IdAlarm}", ex);
                    }
                });

                // Update alarm status in database
                UpdateAlarmStatus(alarm);
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error($"OnAlarmTriggered error for alarm {alarm?.IdAlarm}", ex);
            }
        }

        private async Task ShowAlarmNotification(Alarm alarm)
        {
            try
            {
                // Bring app window to front
                await BringWindowToFront();

                // Create fullscreen alarm page with action callback
                var alarmPage = new WindowsAlarmPage(alarm, (action) =>
                {
                    switch (action)
                    {
                        case WindowsAlarmPage.AlarmAction.Stop:
                            // Stop button: alarm stops ringing but remains scheduled
                            StopAlarm(alarm);
                            break;

                        case WindowsAlarmPage.AlarmAction.Suspend:
                            // Suspend Sequence: dismiss alarm completely
                            SuspendAlarm(alarm);
                            break;

                        case WindowsAlarmPage.AlarmAction.Snooze:
                            // Snooze: create temporary alarm for 5 minutes
                            SnoozeAlarm(alarm, 5);
                            break;

                        case WindowsAlarmPage.AlarmAction.Snooze15:
                            // Snooze 15: create temporary alarm for 15 minutes
                            SnoozeAlarm(alarm, 15);
                            break;

                        case WindowsAlarmPage.AlarmAction.AutoStop:
                            // Duration elapsed: restart after RepetitionTime
                            RestartAlarmAfterRepetitionTime(alarm);
                            break;
                    }
                });

                // Show as modal (fullscreen)
                await Application.Current.MainPage.Navigation.PushModalAsync(alarmPage);

                gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Shown fullscreen alarm page for {alarm.IdAlarm}");
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error($"ShowAlarmNotification error for alarm {alarm?.IdAlarm}", ex);

                // Fallback to simple dialog if modal page fails
                await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    var result = await Application.Current.MainPage.DisplayAlert(
                        "GLUCOMAN ALARM",
                        $"{alarm.ReminderText}\n\nTime: {DateTime.Now:HH:mm}",
                        "STOP",
                        "SUSPEND");

                    if (result)
                        StopAlarm(alarm);
                    else
                        SuspendAlarm(alarm);
                });
            }
        }

        private async Task BringWindowToFront()
        {
            try
            {
                await Microsoft.Maui.ApplicationModel.MainThread.InvokeOnMainThreadAsync(() =>
                {
                    var currentWindow = Application.Current?.Windows?.FirstOrDefault();
                    if (currentWindow?.Handler?.PlatformView is Microsoft.UI.Xaml.Window window)
                    {
                        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        NativeMethods.ShowWindow(hwnd, NativeMethods.SW_RESTORE);
                        NativeMethods.SetForegroundWindow(hwnd);
                    }
                });
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("BringWindowToFront error", ex);
            }
        }

        private void SnoozeAlarm(Alarm alarm, int minutes)
        {
            try
            {
                if (alarm.IdAlarm.HasValue)
                {
                    var blAlarms = new BL_Alarms();
                    var snoozeAlarm = new Alarm
                    {
                        ReminderText = $"Snooze: {alarm.ReminderText}",
                        TimeStart = new gamon.DateTimeAndText { DateTime = DateTime.Now.AddMinutes(minutes) },
                        EnablePlaySoundFile = alarm.EnablePlaySoundFile,
                        DoVibrate = alarm.DoVibrate,
                        SoundFilePath = alarm.SoundFilePath,
                        StartupGraceWindow = TimeSpan.FromMinutes(10)
                    };
                    
                    snoozeAlarm.CalculateNextTriggerTime();
                    blAlarms.AddAlarm(snoozeAlarm);

                    if (snoozeAlarm.IdAlarm.HasValue)
                    {
                        ScheduleAsync(snoozeAlarm).Wait();
                    }
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("SnoozeAlarm error", ex);
            }
        }

        /// <summary>
        /// Stop button handler: alarm stops ringing but remains active and scheduled.
        /// Just marks as triggered and reschedules for the next occurrence.
        /// </summary>
        private void StopAlarm(Alarm alarm)
        {
            try
            {
                if (alarm.IdAlarm.HasValue && DatabaseService.Instance.Database != null)
                {
                    var blAlarms = new BL_Alarms();
                    var alarms = blAlarms.GetAllAlarms(all: true);
                    var dbAlarm = alarms.FirstOrDefault(a => a.IdAlarm == alarm.IdAlarm);

                    if (dbAlarm != null)
                    {
                        // Mark as triggered to update counter
                        dbAlarm.MarkAsTriggered();

                        // Calculate next trigger time based on interval
                        if (dbAlarm.Interval.HasValue && dbAlarm.Interval.Value.TotalMilliseconds > 0)
                        {
                            // Periodic alarm: schedule next occurrence
                            dbAlarm.CalculateNextTriggerTime();
                            blAlarms.AddAlarm(dbAlarm);

                            // Reschedule the alarm
                            ScheduleAsync(dbAlarm).ContinueWith(t =>
                            {
                                if (t.IsFaulted)
                                    gamon.General.LogOfProgram?.Error($"StopAlarm: reschedule failed for alarm {alarm.IdAlarm}", t.Exception);
                                else
                                    gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} stopped by user, rescheduled for {dbAlarm.NextTriggerTime:yyyy-MM-dd HH:mm:ss}");
                            });
                        }
                        else
                        {
                            // Non-periodic alarm: just save state
                            blAlarms.AddAlarm(dbAlarm);
                            gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Non-periodic alarm {alarm.IdAlarm} stopped by user");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("StopAlarm error", ex);
            }
        }

        private void SuspendAlarm(Alarm alarm)
        {
            try
            {
                if (alarm.IdAlarm.HasValue && DatabaseService.Instance.Database != null)
                {
                    var blAlarms = new BL_Alarms();
                    var alarms = blAlarms.GetAllAlarms(all: true);
                    var dbAlarm = alarms.FirstOrDefault(a => a.IdAlarm == alarm.IdAlarm);

                    if (dbAlarm != null)
                    {
                        dbAlarm.Dismiss();
                        blAlarms.AddAlarm(dbAlarm);
                        gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} suspended by user");
                    }
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("SuspendAlarm error", ex);
            }
        }

        /// <summary>
        /// Called when Duration elapsed and the user did NOT dismiss the alarm.
        /// Increments the restart counter; if MaxRepeatCount not yet reached, schedules
        /// the same alarm again after RepetitionTime. If RepetitionTime is not set, uses
        /// a default of 5 minutes so the alarm always comes back.
        /// </summary>
        private void RestartAlarmAfterRepetitionTime(Alarm alarm)
        {
            try
            {
                if (!alarm.IdAlarm.HasValue || DatabaseService.Instance.Database == null)
                    return;

                var blAlarms = new BL_Alarms();
                var allAlarms = blAlarms.GetAllAlarms(all: true);
                var dbAlarm = allAlarms.FirstOrDefault(a => a.IdAlarm == alarm.IdAlarm);

                if (dbAlarm == null)
                {
                    gamon.General.LogOfProgram?.Debug($"RestartAlarmAfterRepetitionTime: alarm {alarm.IdAlarm} not found in DB");
                    return;
                }

                dbAlarm.MarkAsRestartedWhenNotDismissed();

                if (dbAlarm.RingingState == Alarm.AlarmRingingState.Expired)
                {
                    // Max restarts reached → just save final state, do not reschedule
                    blAlarms.AddAlarm(dbAlarm);
                    gamon.General.LogOfProgram?.Event($"RestartAlarmAfterRepetitionTime: alarm {alarm.IdAlarm} expired after max restarts");
                    return;
                }

                // NextTriggerTime was set by MarkAsRestartedWhenNotDismissed.
                // If RepetitionTime was null/0, fall back to 5 minutes.
                if (!dbAlarm.NextTriggerTime.HasValue || dbAlarm.NextTriggerTime.Value <= DateTime.Now)
                {
                    dbAlarm.NextTriggerTime = DateTime.Now.AddMinutes(5);
                    gamon.General.LogOfProgram?.Debug($"RestartAlarmAfterRepetitionTime: no RepetitionTime set, defaulting to 5 min");
                }

                blAlarms.AddAlarm(dbAlarm);
                ScheduleAsync(dbAlarm).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                        gamon.General.LogOfProgram?.Error($"RestartAlarmAfterRepetitionTime: reschedule failed for alarm {alarm.IdAlarm}", t.Exception);
                    else
                        gamon.General.LogOfProgram?.Event($"RestartAlarmAfterRepetitionTime: alarm {alarm.IdAlarm} rescheduled at {dbAlarm.NextTriggerTime:HH:mm:ss} (restart #{dbAlarm.RepeatCount})");
                });
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("RestartAlarmAfterRepetitionTime error", ex);
            }
        }

        private void UpdateAlarmStatus(Alarm alarm)
        {
            try
            {
                if (alarm.IdAlarm.HasValue && DatabaseService.Instance.Database != null)
                {
                    var blAlarms = new BL_Alarms();
                    var alarms = blAlarms.GetAllAlarms(all: true);
                    var dbAlarm = alarms.FirstOrDefault(a => a.IdAlarm == alarm.IdAlarm);

                    if (dbAlarm != null)
                    {
                        dbAlarm.MarkAsTriggered();
                        blAlarms.AddAlarm(dbAlarm);
                    }
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("UpdateAlarmStatus error", ex);
            }
        }

        /// <summary>
        /// Get count of currently scheduled alarms (timers + precision tasks)
        /// </summary>
        public static int GetScheduledAlarmsCount()
        {
            lock (_lock)
            {
                return _activeTimers.Count + _activePrecisionTasks.Count;
            }
        }
    }

    /// <summary>
    /// Native Windows API methods for window management
    /// </summary>
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        internal const int SW_RESTORE = 9;
        internal const int SW_SHOW = 5;
    }
}
#endif

