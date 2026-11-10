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
                WindowsAlarmPage alarmPage = null!;
                alarmPage = new WindowsAlarmPage(alarm, (action) =>
                {
                    switch (action)
                    {
                        case WindowsAlarmPage.AlarmAction.Stop:
                            // Stop button: a transient alarm ends, any other goes on
                            StopAlarm(alarm);
                            break;

                        case WindowsAlarmPage.AlarmAction.Suspend:
                            // Suspend Sequence: dismiss alarm completely
                            SuspendAlarm(alarm);
                            break;

                        case WindowsAlarmPage.AlarmAction.Snooze:
                            // Snooze: the same alarm rings again after the minutes chosen by the user
                            SnoozeAlarm(alarm, alarmPage.SnoozeMinutes);
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
                Alarm? saved = DatabaseService.Instance.Database != null
                    ? new BL_Alarms().SnoozeAlarm(alarm.IdAlarm, minutes)
                    : null;
                // The test alarm isn't saved in the database, so it stays in memory
                var snoozeAlarm = saved ?? new Alarm
                {
                    IdAlarm = alarm.IdAlarm,
                    ReminderText = alarm.ReminderText,
                    NextTriggerTime = DateTime.Now.AddMinutes(minutes),
                    EnablePlaySoundFile = alarm.EnablePlaySoundFile,
                    DoVibrate = alarm.DoVibrate,
                    SoundFilePath = alarm.SoundFilePath
                };
                ScheduleAsync(snoozeAlarm).Wait();
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("SnoozeAlarm error", ex);
            }
        }

        /// <summary>
        /// Stop button handler: a transient alarm ends here, any other alarm stops
        /// ringing and is rescheduled for its next occurrence, if it has one.
        /// </summary>
        private void StopAlarm(Alarm alarm)
        {
            try
            {
                if (DatabaseService.Instance.Database == null)
                    return;
                Reschedule(new BL_Alarms().StopAlarm(alarm.IdAlarm), "StopAlarm");
                gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} stopped by user");
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
                if (DatabaseService.Instance.Database == null)
                    return;
                new BL_Alarms().DismissAlarm(alarm.IdAlarm);
                gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Alarm {alarm.IdAlarm} suspended by user");
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("SuspendAlarm error", ex);
            }
        }

        /// <summary>
        /// Called when Duration elapsed and the user did NOT dismiss the alarm: unless
        /// MaxRepeatCount has been reached, the same alarm rings again after RepetitionTime.
        /// </summary>
        private void RestartAlarmAfterRepetitionTime(Alarm alarm)
        {
            try
            {
                if (DatabaseService.Instance.Database == null)
                    return;
                Reschedule(new BL_Alarms().RestartAlarmNotDismissed(alarm.IdAlarm), "RestartAlarmAfterRepetitionTime");
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("RestartAlarmAfterRepetitionTime error", ex);
            }
        }

        private void Reschedule(Alarm? alarm, string caller)
        {
            if (alarm == null)
                return;
            ScheduleAsync(alarm).ContinueWith(t =>
            {
                if (t.IsFaulted)
                    gamon.General.LogOfProgram?.Error($"{caller}: reschedule failed for alarm {alarm.IdAlarm}", t.Exception);
                else
                    gamon.General.LogOfProgram?.Event($"{caller}: alarm {alarm.IdAlarm} rescheduled at {alarm.NextTriggerTime:yyyy-MM-dd HH:mm:ss}");
            });
        }

        private void UpdateAlarmStatus(Alarm alarm)
        {
            try
            {
                if (DatabaseService.Instance.Database != null)
                    new BL_Alarms().MarkAlarmAsTriggered(alarm.IdAlarm);
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

