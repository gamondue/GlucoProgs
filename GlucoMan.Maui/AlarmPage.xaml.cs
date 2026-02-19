using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using System.Collections.ObjectModel;
using gamon;

namespace GlucoMan.Maui;

public partial class AlarmPage : ContentPage
{
    private BL_Alarms _blAlarms;
    private ISystemAlarmScheduler _alarmScheduler;
    private ObservableCollection<Alarm> _alarms;
    private Alarm? _currentAlarm;

    public AlarmPage()
    {
        InitializeComponent();
        _blAlarms = new BL_Alarms();
        
        // Show Windows warning only on Windows platform
#if WINDOWS
        frmWindowsWarning.IsVisible = true;
#endif
        
        // Get the alarm scheduler from DI
        _alarmScheduler = Application.Current?.Handler?.MauiContext?.Services
            .GetService<ISystemAlarmScheduler>()
            ?? throw new InvalidOperationException("ISystemAlarmScheduler not registered");
        
        _alarms = new ObservableCollection<Alarm>();
        cvAlarms.ItemsSource = _alarms;
        
        // Set default dates
        dtpFrom.Date = DateTime.Today.AddDays(-7);
        dtpTo.Date = DateTime.Today.AddDays(30);
        dtpStartDate.Date = DateTime.Today;
        tpStartTime.Time = DateTime.Now.TimeOfDay;
        
        // Wire up events
        btnAdd.Clicked += BtnAdd_Clicked;
        btnSave.Clicked += BtnSave_Clicked;
        btnDelete.Clicked += BtnDelete_Clicked;
        btnDismiss.Clicked += BtnDismiss_Clicked;
        btnActivate.Clicked += BtnActivate_Clicked;
        btnDeactivate.Clicked += BtnDeactivate_Clicked;
        btnTest.Clicked += BtnTest_Clicked;
        btnClear.Clicked += BtnClear_Clicked;
        btnRefresh.Clicked += BtnRefresh_Clicked;
        btnNow.Clicked += BtnNow_Clicked;
        btnSetNext.Clicked += BtnSetNext_Clicked;
        cvAlarms.SelectionChanged += CvAlarms_SelectionChanged;
        
        // Wire up checkbox events for mutual exclusivity
        chkShowAll.CheckedChanged += (s, e) => { if (e.Value) { chkActive.IsChecked = false; chkExpired.IsChecked = false; LoadAlarms(); } };
        chkActive.CheckedChanged += (s, e) => { if (e.Value) { chkShowAll.IsChecked = false; chkExpired.IsChecked = false; LoadAlarms(); } };
        chkExpired.CheckedChanged += (s, e) => { if (e.Value) { chkShowAll.IsChecked = false; chkActive.IsChecked = false; LoadAlarms(); } };
        
        
        // Sync alarms on page load
        Task.Run(async () =>
        {
            try
            {
#if WINDOWS
                // Check Windows notification permissions
                if (!Platforms.Windows.NotificationHelper.AreNotificationsEnabled())
                {
                    General.LogOfProgram?.Debug("Windows notifications are disabled or not supported");
                    
                    // Show diagnostic info
                    var info = Platforms.Windows.NotificationHelper.GetDiagnosticInfo();
                    General.LogOfProgram?.Debug($"Windows Notification Diagnostics:\n{info}");
                }
#endif
                await AlarmSyncHelper.SyncAllAlarmsAsync(_alarmScheduler);
                AlarmSyncHelper.CleanupExpiredAlarms();
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("AlarmPage - Constructor - Syncing alarms", ex);
            }
        });
        
        LoadAlarms();
    }
    private void LoadAlarms()
    {
        try
        {
            List<Alarm> alarms;
            
            // Debug logging for Windows
            General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - ShowAll:{chkShowAll.IsChecked}, Expired:{chkExpired.IsChecked}, Active:{chkActive.IsChecked}");
            
            if (chkShowAll.IsChecked)
            {
                alarms = _blAlarms.GetAllAlarms(dtpFrom.Date, dtpTo.Date, all: true);
                General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - GetAllAlarms (all=true) returned {alarms.Count} alarms");
            }
            else if (chkExpired.IsChecked)
            {
                alarms = _blAlarms.GetExpiredAlarms();
                General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - GetExpiredAlarms returned {alarms.Count} alarms");
            }
            else if (chkActive.IsChecked)
            {
                alarms = _blAlarms.GetActiveAlarms();
                General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - GetActiveAlarms returned {alarms.Count} alarms");
                
                // Also log why each alarm might not be considered active
                var allAlarms = _blAlarms.GetAllAlarms(all: true);
                General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - Total alarms in DB: {allAlarms.Count}");
                foreach (var a in allAlarms)
                {
                    var isActive = a.IsActive();
                    General.LogOfProgram?.Debug(
                        $"Alarm {a.IdAlarm}: '{a.ReminderText}' - " +
                        $"IsActive={isActive}, IsDisabled={a.IsDisabled}, " +
                        $"NextTriggerTime={a.NextTriggerTime:yyyy-MM-dd HH:mm}, " +
                        $"TimeStart={a.TimeStart?.DateTime:yyyy-MM-dd HH:mm}, " +
                        $"ValidTimeAfterStart={a.ValidTimeAfterStart?.TotalSeconds}s, " +
                        $"RepeatCount={a.RepeatCount}/{a.MaxRepeatCount}");
                }

                // If no active alarms found but DB contains alarms, fall back to showing all to help the user
                if ((alarms == null || alarms.Count == 0) && allAlarms.Count > 0)
                {
                    General.LogOfProgram?.Debug("AlarmPage.LoadAlarms - No active alarms found, falling back to show all alarms for user visibility");
                    alarms = allAlarms;
                    // Inform user briefly on UI thread
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await DisplayAlert("Info", "Nessun allarme è risultato 'Active' secondo i filtri; verranno mostrati tutti gli allarmi presenti nel database.", "OK");
                    });
                }
            }
            else
            {
                alarms = _blAlarms.GetAllAlarms(dtpFrom.Date, dtpTo.Date);
                General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - GetAllAlarms (date range) returned {alarms.Count} alarms");
            }
            
            _alarms.Clear();
            // Normalize data for display: trim reminders and calculate missing next trigger times
            foreach (var alarm in alarms)
            {
                // Trim reminder text to avoid invisible whitespace
                if (!string.IsNullOrEmpty(alarm.ReminderText))
                    alarm.ReminderText = alarm.ReminderText.Trim();

                // If ReminderText is empty, show a placeholder so the column isn't blank
                if (string.IsNullOrEmpty(alarm.ReminderText))
                    alarm.ReminderText = "(no reminder)";

                // If NextTriggerTime is not set (DateTime.MinValue), calculate it from alarm fields
                if (alarm.NextTriggerTime == null || alarm.NextTriggerTime == DateTime.MinValue)
                {
                    try
                    {
                        alarm.CalculateNextTriggerTime();
                    }
                    catch (Exception ex)
                    {
                        General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - CalculateNextTriggerTime failed for {alarm.IdAlarm}: {ex.Message}");
                    }
                }
            }

            foreach (var alarm in alarms.OrderBy(a => a.NextTriggerTime))
            {
                _alarms.Add(alarm);
            }
            
            // Update count label on UI thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                lblAlarmCount.Text = $"({_alarms.Count} alarm{(_alarms.Count != 1 ? "s" : "")})";
            });
            
            General.LogOfProgram?.Debug($"AlarmPage.LoadAlarms - Displayed {_alarms.Count} alarms in UI");
            
            // Show visible feedback to user
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                string filterName = chkShowAll.IsChecked ? "All" : 
                                   chkExpired.IsChecked ? "Expired" : 
                                   chkActive.IsChecked ? "Active" : "Date Range";
                                   
                if (_alarms.Count == 0)
                {
                    await DisplayAlert("No Alarms Found", 
                        $"Filter: {filterName}\n" +
                        $"Found 0 alarms in database.\n\n" +
                        $"Try selecting 'Show All' checkbox to see all alarms without filters.", 
                        "OK");
                }
                else
                {
                    // Optional: show brief toast/message
                    General.LogOfProgram?.Event($"Loaded {_alarms.Count} alarms with filter: {filterName}");
                }
            });
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - LoadAlarms", ex);
            DisplayAlert(AppStrings.Error, $"Failed to load alarms: {ex.Message}", AppStrings.OK);
        }
    }
    private void CvAlarms_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        General.LogOfProgram?.Debug($"AlarmPage.CvAlarms_SelectionChanged - CurrentSelection count: {e.CurrentSelection.Count}");
        // Clear previous selection markers (use standardized IsSelectedInList)
        foreach (var a in _alarms)
        {
            a.IsSelectedInList = false;
            a.IsSelected = false; // keep legacy flag in sync
        }

        if (e.CurrentSelection.FirstOrDefault() is Alarm selected)
        {
            General.LogOfProgram?.Debug($"AlarmPage.CvAlarms_SelectionChanged - Selected alarm: {selected.IdAlarm} / '{selected.ReminderText}'");
            selected.IsSelectedInList = true;
            selected.IsSelected = true; // legacy
            _currentAlarm = selected;
            DisplayAlarm(selected);
            // Ensure selected item is visually focused in CollectionView but do not center (avoid jumping)
            try
            {
                cvAlarms.ScrollTo(selected, position: Microsoft.Maui.Controls.ScrollToPosition.MakeVisible, animate: false);
            }
            catch { }
        }
    }
    
    // Support a direct tap on the item Frame to set selection (helps on Android)
    private void OnItemTapped(object? sender, EventArgs e)
    {
        General.LogOfProgram?.Debug("AlarmPage.OnItemTapped invoked");
        if (sender is Frame frame)
        {
            General.LogOfProgram?.Debug($"AlarmPage.OnItemTapped - Frame.BindingContext = {frame.BindingContext}");
            if (frame.BindingContext is Alarm tapped)
            {
                General.LogOfProgram?.Debug($"AlarmPage.OnItemTapped - tapped alarm: {tapped.IdAlarm}");
                // set selected item which will trigger SelectionChanged
                cvAlarms.SelectedItem = tapped;
            }
        }
    }
    private void DisplayAlarm(Alarm alarm)
    {
        txtIdAlarm.Text = alarm.IdAlarm?.ToString() ?? "";
        txtReminderText.Text = alarm.ReminderText ?? "";
        
        if (alarm.TimeStart?.DateTime.HasValue == true)
        {
            dtpStartDate.Date = alarm.TimeStart.DateTime.Value.Date;
            tpStartTime.Time = alarm.TimeStart.DateTime.Value.TimeOfDay;
        }
        
        txtTriggerInterval.Text = alarm.ValidTimeAfterStart?.TotalSeconds.ToString() ?? "";
        txtInterval.Text = alarm.Interval?.TotalSeconds.ToString() ?? "";
        txtDuration.Text = alarm.Duration?.TotalSeconds.ToString() ?? "";
        txtRepetitionTime.Text = alarm.RepetitionTime?.TotalSeconds.ToString() ?? "";
        txtRepeatCount.Text = alarm.RepeatCount?.ToString() ?? "0";
        txtMaxRepeatCount.Text = alarm.MaxRepeatCount?.ToString() ?? "";
        txtNextTriggerTime.Text = alarm.NextTriggerTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
        txtLastTriggerTime.Text = alarm.LastTriggerTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
        txtTriggeredCount.Text = alarm.TriggeredCount?.ToString() ?? "0";
        chkPlaying.IsChecked = alarm.IsPlaying ?? false;
        chkPlaySound.IsChecked = alarm.EnablePlaySoundFile ?? false;
        chkVibrate.IsChecked = alarm.DoVibrate ?? false;
        txtSoundFilePath.Text = alarm.SoundFilePath ?? "";
        txtState.Text =   alarm.RingingState.ToString();
        txtReminderPreview.Text = $"{alarm.ReminderText} @ {alarm.NextTriggerTime?.ToString("HH:mm")}";
    }
    private Alarm GetAlarmFromUI()
    {
        var alarm = _currentAlarm ?? new Alarm();
        
        if (!string.IsNullOrEmpty(txtIdAlarm.Text) && int.TryParse(txtIdAlarm.Text, out int id))
        {
            alarm.IdAlarm = id;
        }
        
        alarm.ReminderText = txtReminderText.Text;
        
        var startDateTime = dtpStartDate.Date + tpStartTime.Time;
        alarm.TimeStart = new gamon.DateTimeAndText { DateTime = startDateTime };
        
        if (double.TryParse(txtTriggerInterval.Text, out double triggerSec))
            alarm.ValidTimeAfterStart = TimeSpan.FromSeconds(triggerSec);
        
        if (double.TryParse(txtInterval.Text, out double intervalSec))
            alarm.Interval = TimeSpan.FromSeconds(intervalSec);
        
        if (double.TryParse(txtDuration.Text, out double durationSec))
            alarm.Duration = TimeSpan.FromSeconds(durationSec);
        
        if (double.TryParse(txtRepetitionTime.Text, out double repSec))
            alarm.RepetitionTime = TimeSpan.FromSeconds(repSec);
        
        if (int.TryParse(txtMaxRepeatCount.Text, out int maxRep))
            alarm.MaxRepeatCount = maxRep;
        
        alarm.EnablePlaySoundFile = chkPlaySound.IsChecked;
        alarm.DoVibrate = chkVibrate.IsChecked;
        alarm.SoundFilePath = txtSoundFilePath.Text;
        
        return alarm;
    }
    private async void BtnAdd_Clicked(object? sender, EventArgs e)
    {
        try
        {
            var alarm = GetAlarmFromUI();
            alarm.IdAlarm = null; // Force new ID
            alarm.CalculateNextTriggerTime();
            
            // Save to database first - this assigns the IdAlarm
            _blAlarms.AddAlarm(alarm);
            
            if (!alarm.IdAlarm.HasValue)
            {
                throw new InvalidOperationException("Failed to save alarm to database - no ID assigned");
            }
            
            General.LogOfProgram?.Event($"AlarmPage: Alarm {alarm.IdAlarm} saved to database");
            
            // Schedule with system
            await _alarmScheduler.ScheduleAsync(alarm);
            General.LogOfProgram?.Event($"AlarmPage: Alarm {alarm.IdAlarm} scheduled with system");
            
            await DisplayAlert(AppStrings.Success, 
                $"Alarm added successfully!\n\nID: {alarm.IdAlarm}\nTime: {alarm.NextTriggerTime:yyyy-MM-dd HH:mm}", 
                AppStrings.OK);
            
            ClearForm();
            LoadAlarms(); // Reload to show the new alarm
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnAdd_Clicked", ex);
            await DisplayAlert(AppStrings.Error, 
                $"Failed to add alarm:\n\n{ex.Message}\n\nCheck logs for details.", 
                AppStrings.OK);
        }
    }
    private async void BtnSave_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentAlarm == null)
            {
                await DisplayAlert(AppStrings.Warning, "Please select an alarm to save", AppStrings.OK);
                return;
            }
            
            var alarm = GetAlarmFromUI();
            alarm.CalculateNextTriggerTime();
            
            _blAlarms.AddAlarm(alarm); // Save updates
            
            // Reschedule with system
            if (alarm.IdAlarm.HasValue)
            {
                await _alarmScheduler.CancelAsync(alarm.IdAlarm.Value);
                if (alarm.IsActive())
                {
                    await _alarmScheduler.ScheduleAsync(alarm);
                }
            }
            
            await DisplayAlert(AppStrings.Success, "Alarm saved successfully", AppStrings.OK);
            LoadAlarms();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnSave_Clicked", ex);
            await DisplayAlert(AppStrings.Error, $"Failed to save alarm: {ex.Message}", AppStrings.OK);
        }
    }
    private async void BtnDelete_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentAlarm == null || !_currentAlarm.IdAlarm.HasValue)
            {
                await DisplayAlert(AppStrings.Warning, "Please select an alarm to delete", AppStrings.OK);
                return;
            }
            
            bool confirm = await DisplayAlert(AppStrings.DeleteConfirm, 
                $"Delete alarm: {_currentAlarm.ReminderText}?", 
                AppStrings.Yes, AppStrings.No);
            
            if (!confirm) return;
            
            await _alarmScheduler.CancelAsync(_currentAlarm.IdAlarm.Value);
            _blAlarms.DeleteAlarm(_currentAlarm);
            
            await DisplayAlert(AppStrings.Success, "Alarm deleted successfully", AppStrings.OK);
            ClearForm();
            LoadAlarms();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnDelete_Clicked", ex);
            await DisplayAlert(AppStrings.Error, $"Failed to delete alarm: {ex.Message}", AppStrings.OK);
        }
    }
    private async void BtnDismiss_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentAlarm == null || !_currentAlarm.IdAlarm.HasValue)
            {
                await DisplayAlert(AppStrings.Warning, "Please select an alarm to dismiss", AppStrings.OK);
                return;
            }
            
            _currentAlarm.Dismiss();
            _blAlarms.AddAlarm(_currentAlarm);
            await _alarmScheduler.CancelAsync(_currentAlarm.IdAlarm.Value);
            
            await DisplayAlert(AppStrings.Success, "Alarm dismissed", AppStrings.OK);
            LoadAlarms();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnDismiss_Clicked", ex);
            await DisplayAlert(AppStrings.Error, $"Failed to dismiss alarm: {ex.Message}", AppStrings.OK);
        }
    }
    private async void BtnActivate_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentAlarm == null || !_currentAlarm.IdAlarm.HasValue)
            {
                await DisplayAlert(AppStrings.Warning, "Please select an alarm to activate", AppStrings.OK);
                return;
            }
            
            _currentAlarm.IsDisabled = false;
            _currentAlarm.CalculateNextTriggerTime();
            _blAlarms.AddAlarm(_currentAlarm);
            
            if (_currentAlarm.IsActive())
            {
                await _alarmScheduler.ScheduleAsync(_currentAlarm);
            }
            
            await DisplayAlert(AppStrings.Success, "Alarm activated", AppStrings.OK);
            DisplayAlarm(_currentAlarm);
            LoadAlarms();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnActivate_Clicked", ex);
            await DisplayAlert(AppStrings.Error, $"Failed to activate alarm: {ex.Message}", AppStrings.OK);
        }
    }
    private async void BtnDeactivate_Clicked(object? sender, EventArgs e)
    {
        try
        {
            if (_currentAlarm == null || !_currentAlarm.IdAlarm.HasValue)
            {
                await DisplayAlert(AppStrings.Warning, "Please select an alarm to deactivate", AppStrings.OK);
                return;
            }
            
            _currentAlarm.IsDisabled = true;
            _currentAlarm.RingingState = Alarm.AlarmRingingState.Disabled;
            _blAlarms.AddAlarm(_currentAlarm);
            await _alarmScheduler.CancelAsync(_currentAlarm.IdAlarm.Value);
            
            await DisplayAlert(AppStrings.Success, "Alarm deactivated", AppStrings.OK);
            DisplayAlarm(_currentAlarm);
            LoadAlarms();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnDeactivate_Clicked", ex);
            await DisplayAlert(AppStrings.Error, $"Failed to deactivate alarm: {ex.Message}", AppStrings.OK);
        }
    }
    private async void BtnTest_Clicked(object? sender, EventArgs e)
    {
        try
        {
            // Create a test alarm for 5 seconds from now
            var testAlarm = new Alarm
            {
                ReminderText = "Test Alarm",
                TimeStart = new gamon.DateTimeAndText { DateTime = DateTime.Now.AddSeconds(5) },
                EnablePlaySoundFile = chkPlaySound.IsChecked,
                DoVibrate = chkVibrate.IsChecked,
                SoundFilePath = txtSoundFilePath.Text
            };
            testAlarm.CalculateNextTriggerTime();
            
            _blAlarms.AddAlarm(testAlarm);
            
            if (testAlarm.IdAlarm.HasValue)
            {
                await _alarmScheduler.ScheduleAsync(testAlarm);
                await DisplayAlert(AppStrings.Info, "Test alarm scheduled for 5 seconds from now", AppStrings.OK);
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("AlarmPage - BtnTest_Clicked", ex);
            await DisplayAlert(AppStrings.Error, $"Failed to create test alarm: {ex.Message}", AppStrings.OK);
        }
    }
    private void BtnClear_Clicked(object? sender, EventArgs e)
    {
        ClearForm();
    }
    private void BtnRefresh_Clicked(object? sender, EventArgs e)
    {
        LoadAlarms();
    }
    private void BtnNow_Clicked(object? sender, EventArgs e)
    {
        dtpStartDate.Date = DateTime.Today;
        tpStartTime.Time = DateTime.Now.TimeOfDay;
    }
    private void BtnSetNext_Clicked(object? sender, EventArgs e)
    {
        if (_currentAlarm != null)
        {
            _currentAlarm.CalculateNextTriggerTime();
            DisplayAlarm(_currentAlarm);
        }
    }
    private void ClearForm()
    {
        _currentAlarm = null;
        txtIdAlarm.Text = "";
        txtReminderText.Text = "";
        dtpStartDate.Date = DateTime.Today;
        tpStartTime.Time = DateTime.Now.TimeOfDay;
        txtTriggerInterval.Text = "";
        txtInterval.Text = "";
        txtDuration.Text = "";
        txtRepetitionTime.Text = "";
        txtRepeatCount.Text = "0";
        txtMaxRepeatCount.Text = "";
        txtNextTriggerTime.Text = "";
        txtLastTriggerTime.Text = "";
        txtTriggeredCount.Text = "0";
        chkPlaying.IsChecked = false;
        chkPlaySound.IsChecked = false;
        chkVibrate.IsChecked = false;
        txtSoundFilePath.Text = "";
        txtState.Text = "";
        txtReminderPreview.Text = "";
        
        cvAlarms.SelectedItem = null;
    }
}

