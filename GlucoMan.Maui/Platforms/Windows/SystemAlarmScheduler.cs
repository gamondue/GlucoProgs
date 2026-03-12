#if WINDOWS
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using GlucoMan;
using SystemTimer = System.Timers.Timer;

namespace GlucoMan.Maui.Platforms.Windows
{
    /// <summary>
    /// Windows alarm scheduler using in-memory timers.
    /// Note: This requires the app to be running. For production, consider Windows Task Scheduler.
    /// </summary>
    public class SystemAlarmScheduler : ISystemAlarmScheduler
    {
        private static readonly Dictionary<int, SystemTimer> _activeTimers = new();
        private static readonly object _lock = new();

        public Task ScheduleAsync(Alarm alarm)
        {
            try
            {
                if (alarm == null) throw new ArgumentNullException(nameof(alarm));
                if (alarm.IdAlarm == null) throw new ArgumentException("Alarm must have IdAlarm before scheduling");

                var baseStart = alarm.TimeStart?.DateTime ?? DateTime.Now;
                DateTime when = alarm.NextTriggerTime ?? (baseStart + (alarm.ValidTimeAfterStart ?? TimeSpan.Zero));

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
                    // Cancel existing timer if any
                    if (_activeTimers.TryGetValue(alarm.IdAlarm.Value, out var existingTimer))
                    {
                        existingTimer.Stop();
                        existingTimer.Dispose();
                        _activeTimers.Remove(alarm.IdAlarm.Value);
                    }

                    // Create new timer
                    var timer = new SystemTimer(timeUntilAlarm.TotalMilliseconds)
                    {
                        AutoReset = false
                    };

                    timer.Elapsed += (sender, e) =>
                    {
                        OnAlarmTriggered(alarm);
                        
                        lock (_lock)
                        {
                            if (_activeTimers.ContainsKey(alarm.IdAlarm.Value))
                            {
                                _activeTimers.Remove(alarm.IdAlarm.Value);
                            }
                        }
                    };

                    timer.Start();
                    _activeTimers[alarm.IdAlarm.Value] = timer;
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

        public Task CancelAsync(int idAlarm)
        {
            try
            {
                lock (_lock)
                {
                    if (_activeTimers.TryGetValue(idAlarm, out var timer))
                    {
                        timer.Stop();
                        timer.Dispose();
                        _activeTimers.Remove(idAlarm);
                        
                        gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Cancelled alarm {idAlarm}");
                    }
                    else
                    {
                        gamon.General.LogOfProgram?.Debug($"Windows SystemAlarmScheduler: Alarm {idAlarm} not found in active timers");
                    }
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
                    int count = _activeTimers.Count;
                    
                    foreach (var timer in _activeTimers.Values)
                    {
                        timer.Stop();
                        timer.Dispose();
                    }
                    
                    _activeTimers.Clear();
                    
                    gamon.General.LogOfProgram?.Event($"Windows SystemAlarmScheduler: Cancelled all {count} alarms");
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
                bool dismissed = false;
                
                // Bring app window to front
                await BringWindowToFront();
                
                // Create fullscreen alarm page
                var alarmPage = new WindowsAlarmPage(alarm, (isDismissed) =>
                {
                    dismissed = isDismissed;
                    
                    // Handle dismiss or snooze
                    if (isDismissed)
                    {
                        DismissAlarm(alarm);
                    }
                    else
                    {
                        SnoozeAlarm(alarm, 5);
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
                        "DISMISS",
                        "SNOOZE 5 MIN");

                    if (!result)
                    {
                        SnoozeAlarm(alarm, 5);
                    }
                    else
                    {
                        DismissAlarm(alarm);
                    }
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
                        ValidTimeAfterStart = TimeSpan.FromMinutes(10)
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

        private void DismissAlarm(Alarm alarm)
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
                    }
                }
            }
            catch (Exception ex)
            {
                gamon.General.LogOfProgram?.Error("DismissAlarm error", ex);
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
        /// Get count of currently scheduled alarms
        /// </summary>
        public static int GetScheduledAlarmsCount()
        {
            lock (_lock)
            {
                return _activeTimers.Count;
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

