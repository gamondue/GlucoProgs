using GlucoMan;
#if WINDOWS
using System.Runtime.InteropServices;
#endif

namespace GlucoMan.Maui;

#if WINDOWS
/// <summary>
/// Windows native methods for playing system sounds
/// </summary>
internal static class WindowsNativeMethods
{
    /// <summary>
    /// Plays a waveform sound. More reliable than Console.Beep on modern Windows.
    /// </summary>
    /// <param name="uType">
    /// Sound type:
    /// 0x00000000 = Simple beep
    /// 0x00000010 = MB_ICONHAND (Critical Stop)
    /// 0x00000020 = MB_ICONQUESTION (Question)
    /// 0x00000030 = MB_ICONEXCLAMATION (Warning/Exclamation)
    /// 0x00000040 = MB_ICONASTERISK (Asterisk/Information)
    /// 0xFFFFFFFF = Standard beep using computer speaker
    /// </param>
    [DllImport("user32.dll")]
    internal static extern bool MessageBeep(uint uType);

    // Sound type constants
    internal const uint MB_OK = 0x00000000;
    internal const uint MB_ICONHAND = 0x00000010;
    internal const uint MB_ICONQUESTION = 0x00000020;
    internal const uint MB_ICONEXCLAMATION = 0x00000030;
    internal const uint MB_ICONASTERISK = 0x00000040;
}
#endif

public partial class WindowsAlarmPage : ContentPage
{
    private readonly Alarm _alarm;
    // callback(action)
    // AlarmAction.Stop → user pressed Stop (keep alarm scheduled)
    // AlarmAction.Suspend → user pressed Suspend Sequence (dismiss completely)
    // AlarmAction.Snooze → user pressed Snooze (5 minutes)
    // AlarmAction.AutoStop → Duration elapsed, no user action
    private readonly Action<AlarmAction> _dismissCallback;
    private System.Timers.Timer? _clockTimer;
    private System.Timers.Timer? _beepTimer;
    private System.Timers.Timer? _flashTimer;
    private System.Timers.Timer? _durationTimer;
    private bool _isFlashing;
    private bool _stopped; // guard against double-stop

    public enum AlarmAction
    {
        Stop,       // Stop sound but keep scheduled
        Suspend,    // Dismiss completely
        Snooze,     // Snooze 5 minutes
        Snooze15,   // Snooze 15 minutes
        AutoStop    // Auto-stopped by duration timer
    }

    public WindowsAlarmPage(Alarm alarm, Action<AlarmAction> dismissCallback)
    {
        InitializeComponent();

        _alarm = alarm;
        _dismissCallback = dismissCallback;

        // Set alarm message
        lblAlarmMessage.Text = alarm.ReminderText ?? "Alarm";

        // Start clock update
        UpdateClock();
        _clockTimer = new System.Timers.Timer(1000); // Update every second
        _clockTimer.Elapsed += (s, e) =>
        {
            MainThread.BeginInvokeOnMainThread(() => UpdateClock());
        };
        _clockTimer.Start();

        // Start visual flashing effect
        StartFlashing();

        // Play beep sound if enabled
        if (alarm.EnablePlaySoundFile == true)
        {
            StartBeeping();
        }

        // Auto-stop after Duration seconds (if set and > 0)
        double durationSeconds = alarm.Duration?.TotalSeconds ?? 0;
        if (durationSeconds > 0)
        {
            _durationTimer = new System.Timers.Timer(durationSeconds * 1000) { AutoReset = false };
            _durationTimer.Elapsed += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    AutoStopAfterDuration();
                });
            };
            _durationTimer.Start();
        }
    }

    private void UpdateClock()
    {
        lblAlarmTime.Text = DateTime.Now.ToString("HH:mm:ss");
    }

    private void StartFlashing()
    {
        _flashTimer = new System.Timers.Timer(500); // Flash every 500ms
        _flashTimer.Elapsed += (s, e) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _isFlashing = !_isFlashing;
                this.BackgroundColor = _isFlashing ? Color.FromArgb("#DC143C") : Color.FromArgb("#FF0000");
            });
        };
        _flashTimer.Start();
    }

    private void StartBeeping()
    {
        // Initial beep
        Task.Run(() => PlayBeep());
        
        // Continue beeping every 3 seconds
        _beepTimer = new System.Timers.Timer(3000);
        _beepTimer.Elapsed += (s, e) => 
        {
            Task.Run(() => PlayBeep());
        };
        _beepTimer.Start();
    }

    private void PlayBeep()
    {
        try
        {
#if WINDOWS
            // Use Windows API MessageBeep - more reliable than Console.Beep
            // Play multiple alarm sounds for attention
            for (int i = 0; i < 3; i++)
            {
                // MB_ICONEXCLAMATION = 0x30 (exclamation/warning sound)
                WindowsNativeMethods.MessageBeep(0x30);
                Thread.Sleep(300);
                
                // MB_ICONHAND = 0x10 (critical stop sound)  
                WindowsNativeMethods.MessageBeep(0x10);
                Thread.Sleep(300);
            }
#else
            // Fallback to Console.Beep for other platforms
            for (int i = 0; i < 3; i++)
            {
                Console.Beep(1400, 300);
                Thread.Sleep(200);
            }
#endif
        }
        catch (Exception ex)
        {
            // Log the error but don't crash
            gamon.General.LogOfProgram?.Error("PlayBeep error", ex);
        }
    }

    private void StopAll()
    {
        if (_stopped) return;
        _stopped = true;
        _clockTimer?.Stop();
        _clockTimer?.Dispose();
        _beepTimer?.Stop();
        _beepTimer?.Dispose();
        _flashTimer?.Stop();
        _flashTimer?.Dispose();
        _durationTimer?.Stop();
        _durationTimer?.Dispose();
        // Reset background color
        MainThread.BeginInvokeOnMainThread(() =>
        {
            this.BackgroundColor = Colors.White;
        });
    }

    /// <summary>
    /// Called when Duration has elapsed without user interaction.
    /// Stops sound/flashing; signals AutoStop so the scheduler
    /// can reschedule after RepetitionTime.
    /// </summary>
    private async void AutoStopAfterDuration()
    {
        if (_stopped) return;
        StopAll();
        _dismissCallback?.Invoke(AlarmAction.AutoStop);
        await Navigation.PopModalAsync();
    }

    /// <summary>
    /// Stop button: stops the alarm sound/flashing but keeps the alarm scheduled.
    /// The alarm will trigger again at the next scheduled time.
    /// </summary>
    private async void BtnStop_Clicked(object? sender, EventArgs e)
    {
        StopAll();
        _dismissCallback?.Invoke(AlarmAction.Stop);
        await Navigation.PopModalAsync();
    }

    /// <summary>
    /// Suspend Sequence button: completely dismisses the alarm.
    /// The alarm will not trigger again unless manually reactivated.
    /// </summary>
    private async void BtnSuspend_Clicked(object? sender, EventArgs e)
    {
        StopAll();
        _dismissCallback?.Invoke(AlarmAction.Suspend);
        await Navigation.PopModalAsync();
    }

    private async void BtnSnooze_Clicked(object? sender, EventArgs e)
    {
        StopAll();
        _dismissCallback?.Invoke(AlarmAction.Snooze);
        await Navigation.PopModalAsync();
    }

    private async void BtnSnooze15_Clicked(object? sender, EventArgs e)
    {
        StopAll();
        _dismissCallback?.Invoke(AlarmAction.Snooze15);
        await Navigation.PopModalAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        // Allow back button to stop the alarm (same behavior as Android)
        Task.Run(async () =>
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                StopAll();
                _dismissCallback?.Invoke(AlarmAction.Stop);
                await Navigation.PopModalAsync();
            });
        });
        return true;
    }
}

