using gamon;
using GlucoMan.BusinessLayer;
using System.ComponentModel;
using static GlucoMan.Common;
using System.Collections.ObjectModel;

namespace GlucoMan.Maui;

public partial class PhysicalActivityPage : ContentPage, INotifyPropertyChanged
{
    // Business layer for physical activities (reusing injection structure for now)
    private BL_BolusesAndInjections bl;

    private bool loadingUi = true;

    private UiAccuracy accuracyActivity;

    private Color initialButtonBackground = Colors.White;
    private Color initialButtonTextColor = Colors.Black;

    // Current activity (using Injection object structure for now)
    private Injection CurrentActivity = new Injection();
    private List<Injection> allActivities;

    // Observable collection for ListView binding
    private ObservableCollection<Injection> _activities;
    public ObservableCollection<Injection> Activities
    {
        get => _activities;
        set
        {
            _activities = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public PhysicalActivityPage(Services.LocalizationService localizationService)
    {
        try
        {
            InitializeComponent();

            // Initialize business layer
            bl = new BL_BolusesAndInjections();

            loadingUi = true;

            // Initialize ObservableCollection for ListView binding
            Activities = new ObservableCollection<Injection>();

            // Set the page as its own BindingContext for property binding
            this.BindingContext = this;

            // Initialize current activity with default values
            InitializeCurrentActivity();

            // UI initialization with error handling - ACCURACY MANAGEMENT
            try
            {
                if (cmbAccuracyActivity != null)
                {
                    cmbAccuracyActivity.ItemsSource = Enum.GetValues(typeof(QualitativeAccuracy));

                    if (txtAccuracyOfActivity != null)
                    {
                        accuracyActivity = new UiAccuracy(txtAccuracyOfActivity, cmbAccuracyActivity);

                        // Initialize accuracy values to sync combo boxes with current text values
                        InitializeAccuracyControls();
                    }
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("PhysicalActivityPage constructor - Accuracy initialization", ex);
            }

            // Final UI refresh with error handling
            try
            {
                RefreshUi();
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("PhysicalActivityPage constructor - RefreshUi", ex);
            }

            loadingUi = false;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage constructor - Critical error", ex);
            loadingUi = false;

            // Emergency fallback initialization
            if (bl == null)
            {
                bl = new BL_BolusesAndInjections();
            }
            if (CurrentActivity == null)
            {
                InitializeCurrentActivity();
            }
            if (Activities == null)
            {
                Activities = new ObservableCollection<Injection>();
            }
        }
    }

    private void InitializeCurrentActivity()
    {
        CurrentActivity = new Injection();
        CurrentActivity.EventTime.DateTime = DateTime.Now;
        CurrentActivity.InsulinValue.Double = 1; // Activity level 1-10
        CurrentActivity.InsulinCalculated.Double = 30; // Duration in minutes
        CurrentActivity.IdTypeOfInjection = (int)Common.TypeOfInjection.Other; // Use Other for activities
        CurrentActivity.Notes = "Accuracy:100"; // Store accuracy in notes with format "Accuracy:XX"
    }

    // Helper methods to get/set accuracy from Notes field
    private double GetAccuracyFromNotes(string notes)
    {
        if (string.IsNullOrEmpty(notes))
            return 100;

        // Look for "Accuracy:XX" pattern in notes
        var accuracyIndex = notes.IndexOf("Accuracy:");
        if (accuracyIndex >= 0)
        {
            var accuracyString = notes.Substring(accuracyIndex + 9);
            var spaceIndex = accuracyString.IndexOf(' ');
            if (spaceIndex > 0)
                accuracyString = accuracyString.Substring(0, spaceIndex);

            if (double.TryParse(accuracyString, out double accuracy))
                return accuracy;
        }
        return 100; // Default accuracy
    }

    private string SetAccuracyInNotes(string notes, double accuracy)
    {
        if (string.IsNullOrEmpty(notes))
            return $"Accuracy:{accuracy}";

        // Remove existing accuracy pattern
        var accuracyIndex = notes.IndexOf("Accuracy:");
        if (accuracyIndex >= 0)
        {
            var endIndex = notes.IndexOf(' ', accuracyIndex);
            if (endIndex > 0)
                notes = notes.Remove(accuracyIndex, endIndex - accuracyIndex + 1);
            else
                notes = notes.Remove(accuracyIndex);
        }

        // Add new accuracy
        if (!string.IsNullOrEmpty(notes.Trim()))
            return $"{notes.Trim()} Accuracy:{accuracy}";
        else
            return $"Accuracy:{accuracy}";
    }

    private async void RefreshUi()
    {
        try
        {
            await RefreshActivity();
            RefreshGrid();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - RefreshUi", ex);

            // Emergency UI refresh
            try
            {
                if (CurrentActivity != null)
                {
                    await FromClassToUi();
                }
                if (Activities == null)
                {
                    Activities = new ObservableCollection<Injection>();
                }
            }
            catch (Exception emergencyEx)
            {
                General.LogOfProgram?.Error("PhysicalActivityPage - RefreshUi emergency", emergencyEx);
            }
        }
    }

    private async Task RefreshActivity()
    {
        try
        {
            if (CurrentActivity != null)
            {
                await FromClassToUi();
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - RefreshActivity", ex);
        }
    }

    private void UpdateActivitiesCollection()
    {
        try
        {
            if (Activities == null)
                Activities = new ObservableCollection<Injection>();

            Activities.Clear();

            if (allActivities != null)
            {
                foreach (var activity in allActivities)
                {
                    Activities.Add(activity);
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - UpdateActivitiesCollection", ex);
        }
    }

    private void RefreshGrid()
    {
        try
        {
            // Load activities from last 4 months (filtering by type Other for activities)
            DateTime now = DateTime.Now;
            allActivities = bl.GetInjections(now.AddMonths(-4), now.AddDays(1),
                Common.TypeOfInsulinAction.NotSet, Common.ZoneOfPosition.NotSet,
                false, false, false, false)
                .Where(i => i.IdTypeOfInjection == (int)Common.TypeOfInjection.Other)
                .ToList();

            UpdateActivitiesCollection();
            gridActivities.ItemsSource = Activities;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - RefreshGrid", ex);
        }
    }

    private async Task FromClassToUi()
    {
        try
        {
            loadingUi = true;

            if (CurrentActivity.IdInjection != null)
                txtIdActivity.Text = CurrentActivity.IdInjection.ToString();
            else
                txtIdActivity.Text = "";

            txtActivityLevel.Text = CurrentActivity.InsulinValue.Text; // Reusing InsulinValue for activity level
            txtDurationMinutes.Text = CurrentActivity.InsulinCalculated.Text; // Reusing InsulinCalculated for duration
            dtpActivityDate.Date = ((DateTime)CurrentActivity.EventTime.DateTime);
            dtpActivityTime.Time = ((DateTime)CurrentActivity.EventTime.DateTime).TimeOfDay;

            // Extract accuracy from Notes field
            double accuracy = GetAccuracyFromNotes(CurrentActivity.Notes);
            txtAccuracyOfActivity.Text = accuracy.ToString();

            // Extract notes without accuracy
            string notesWithoutAccuracy = CurrentActivity.Notes ?? "";
            var accuracyIndex = notesWithoutAccuracy.IndexOf("Accuracy:");
            if (accuracyIndex >= 0)
            {
                var endIndex = notesWithoutAccuracy.IndexOf(' ', accuracyIndex);
                if (endIndex > 0)
                    notesWithoutAccuracy = notesWithoutAccuracy.Remove(accuracyIndex, endIndex - accuracyIndex + 1);
                else
                    notesWithoutAccuracy = notesWithoutAccuracy.Remove(accuracyIndex);
            }
            txtNotes.Text = notesWithoutAccuracy.Trim();

            // Set intensity radio buttons based on activity level
            if (CurrentActivity.InsulinValue.Double != null)
            {
                if (CurrentActivity.InsulinValue.Double <= 5)
                {
                    rdbLowIntensity.IsChecked = true;
                    rdbHighIntensity.IsChecked = false;
                }
                else
                {
                    rdbLowIntensity.IsChecked = false;
                    rdbHighIntensity.IsChecked = true;
                }
            }
            else
            {
                rdbLowIntensity.IsChecked = false;
                rdbHighIntensity.IsChecked = false;
            }

            // Update accuracy controls after data is loaded
            await RefreshActivityAccuracyControls();

            loadingUi = false;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - FromClassToUi", ex);
            loadingUi = false;
        }
    }

    private void FromUiToClass()
    {
        try
        {
            CurrentActivity.IdInjection = Safe.Int(txtIdActivity.Text);
            CurrentActivity.InsulinValue.Text = txtActivityLevel.Text; // Activity level (1-10)
            CurrentActivity.InsulinCalculated.Text = txtDurationMinutes.Text; // Duration in minutes

            DateTime instant = new DateTime(
                dtpActivityDate.Date.Year, dtpActivityDate.Date.Month, dtpActivityDate.Date.Day,
                dtpActivityTime.Time.Hours, dtpActivityTime.Time.Minutes, dtpActivityTime.Time.Seconds);
            CurrentActivity.EventTime.DateTime = instant;

            // Store accuracy in Notes field with special format
            double accuracy = Safe.Double(txtAccuracyOfActivity.Text) ?? 100;
            CurrentActivity.Notes = SetAccuracyInNotes(txtNotes.Text, accuracy);

            // Set type as Other for activities
            CurrentActivity.IdTypeOfInjection = (int)Common.TypeOfInjection.Other;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - FromUiToClass", ex);
        }
    }

    private void btnNow_Click(object sender, EventArgs e)
    {
        try
        {
            DateTime now = DateTime.Now;
            dtpActivityDate.Date = now;
            dtpActivityTime.Time = now.TimeOfDay;
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - btnNow_Click", ex);
        }
    }

    private async void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtIdActivity.Text == "")
            {
                await DisplayAlert("Select one activity from the list", "Choose an activity to save", "Ok");
                return;
            }

            FromUiToClass();
            bl.SaveOneInjection(CurrentActivity);
            RefreshUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - btnSave_Click", ex);
            await DisplayAlert("Error", "Failed to save activity data. Check logs for details.", "OK");
        }
    }

    private async void btnAddActivity_Click(object sender, EventArgs e)
    {
        try
        {
            if (chkNowInAdd.IsChecked)
            {
                DateTime now = DateTime.Now;
                dtpActivityDate.Date = now;
                dtpActivityTime.Time = now.TimeOfDay;
            }

            FromUiToClass();

            // Reset ID for new entry
            CurrentActivity.IdInjection = null;
            CurrentActivity.IdTypeOfInjection = (int)Common.TypeOfInjection.Other;

            bl.SaveOneInjection(CurrentActivity);
            RefreshUi();

            General.LogOfProgram?.Event("Activity added successfully");
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - btnAddActivity_Click", ex);
            await DisplayAlert("Error", "Failed to add activity. Check logs for details.", "OK");
        }
    }

    private async void btnRemoveActivity_Click(object sender, EventArgs e)
    {
        try
        {
            var selectedActivity = (Injection)gridActivities.SelectedItem;
            if (selectedActivity != null)
            {
                bool remove = await DisplayAlert(String.Format(
                    "Should I delete the activity from {0}, level {1}?",
                    selectedActivity.EventTime.ToString(),
                    selectedActivity.InsulinValue.ToString()),
                    "", "Yes", "No");

                if (remove)
                {
                    bl.DeleteOneInjection(selectedActivity);
                    RefreshUi();
                }
            }
            else
            {
                await DisplayAlert("Deletion not possible", "Choose an activity to delete", "Ok");
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - btnRemoveActivity_Click", ex);
            await DisplayAlert("Error", "Failed to delete activity. Check logs for details.", "OK");
        }
    }

    private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            return;
        }

        try
        {
            CurrentActivity = (Injection)e.SelectedItem;
            await FromClassToUi();

            // Update accuracy controls after data selection
            await RefreshActivityAccuracyControls();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - OnGridSelectionAsync", ex);
        }
    }

    // ACCURACY MANAGEMENT METHODS (like MealPage)

    private void InitializeAccuracyControls()
    {
        try
        {
            // Initialize activity accuracy combo box based on current text value
            if (accuracyActivity != null && !string.IsNullOrEmpty(txtAccuracyOfActivity.Text))
            {
                if (double.TryParse(txtAccuracyOfActivity.Text, out double activityAccuracy))
                {
                    var qualitativeAccuracy = accuracyActivity.GetQualitativeAccuracyGivenQuantitavive(activityAccuracy);
                    cmbAccuracyActivity.SelectedItem = qualitativeAccuracy;
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - InitializeAccuracyControls", ex);
        }
    }

    private async Task RefreshActivityAccuracyControls()
    {
        try
        {
            // Small delay to ensure data binding has completed
            await Task.Delay(50);

            // Update activity accuracy controls after data binding changes
            if (accuracyActivity != null && !string.IsNullOrEmpty(txtAccuracyOfActivity.Text))
            {
                if (double.TryParse(txtAccuracyOfActivity.Text, out double activityAccuracy))
                {
                    // Update combo box selection
                    var qualitativeAccuracy = accuracyActivity.GetQualitativeAccuracyGivenQuantitavive(activityAccuracy);
                    cmbAccuracyActivity.SelectedItem = qualitativeAccuracy;

                    // Update text box colors using UiAccuracy logic
                    txtAccuracyOfActivity.BackgroundColor = accuracyActivity.AccuracyBackColor(activityAccuracy);
                    txtAccuracyOfActivity.TextColor = accuracyActivity.AccuracyForeColor(activityAccuracy);
                }
            }
            else
            {
                // Reset to default if no valid accuracy
                cmbAccuracyActivity.SelectedItem = null;
                txtAccuracyOfActivity.BackgroundColor = Colors.LightGreen; // Default from XAML
                txtAccuracyOfActivity.TextColor = Colors.Black; // Default from XAML
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - RefreshActivityAccuracyControls", ex);
        }
    }

    private void txtAccuracyOfActivity_Unfocused(object sender, FocusEventArgs e)
    {
        try
        {
            if (!loadingUi && CurrentActivity != null && !string.IsNullOrEmpty(txtAccuracyOfActivity.Text))
            {
                if (double.TryParse(txtAccuracyOfActivity.Text, out double accuracy))
                {
                    // Update the activity's accuracy in the data model via Notes field
                    CurrentActivity.Notes = SetAccuracyInNotes(txtNotes.Text, accuracy);

                    // The UiAccuracy class will handle combo box and color updates automatically
                    // But we can also ensure colors are correct here
                    if (accuracyActivity != null)
                    {
                        txtAccuracyOfActivity.BackgroundColor = accuracyActivity.AccuracyBackColor(accuracy);
                        txtAccuracyOfActivity.TextColor = accuracyActivity.AccuracyForeColor(accuracy);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - txtAccuracyOfActivity_Unfocused", ex);
        }
    }

    private void cmbAccuracyActivity_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Let UiAccuracy handle the text box update, we only update the data model
        try
        {
            if (!loadingUi && CurrentActivity != null && cmbAccuracyActivity.SelectedItem != null)
            {
                var selectedAccuracy = (QualitativeAccuracy)cmbAccuracyActivity.SelectedItem;
                double numericValue = (double)selectedAccuracy;

                // Update the activity's accuracy in the data model via Notes field
                CurrentActivity.Notes = SetAccuracyInNotes(txtNotes.Text, numericValue);

                // Update colors based on the new value
                if (accuracyActivity != null)
                {
                    txtAccuracyOfActivity.BackgroundColor = accuracyActivity.AccuracyBackColor(numericValue);
                    txtAccuracyOfActivity.TextColor = accuracyActivity.AccuracyForeColor(numericValue);
                }
            }
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - cmbAccuracyActivity_SelectedIndexChanged", ex);
        }
    }

    protected override void OnAppearing()
    {
        try
        {
            RefreshUi();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("PhysicalActivityPage - OnAppearing", ex);
        }
    }
}