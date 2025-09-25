using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;
using System.ComponentModel.Design;

namespace GlucoMan.Maui;
public partial class InjectionsPage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    Injection CurrentInjection = new Injection();
    List<Injection> allInjections;

    int? IdCurrentShortActingInsulin;
    int? IdCurrentLongActingInsulin;
    InsulinDrug currentShortInsulin;
    InsulinDrug currentLongInsulin;
    private bool pageIsLoading = true;
    private bool picturePageHasBeenVisited = false;

    double? MonthsOfDataShownInTheGrids = 3;
    double ShownDataTimeMultiplicator = 1;

    // Additions for tracking changes
    private Injection OriginalInjection = new Injection();
    private bool HasUnsavedChanges = false;
    private bool IsNavigatingAway = false;
    private bool isFirstSelection = true;

    internal InjectionsPage(int? IdInjection)
    {
        InitializeComponent();

        Parameters parameters = Common.Database.GetParameters();
        if (parameters != null && parameters.MonthsOfDataShownInTheGrids > 0)
            MonthsOfDataShownInTheGrids = parameters.MonthsOfDataShownInTheGrids;

        pageIsLoading = true;
        // set rdbShortInsulin and rdbInsulin text to the name of the right insulins 
        // read from Parameters the Id of current short action insulin

        IdCurrentShortActingInsulin = parameters?.IdInsulinDrug_Short;
        IdCurrentLongActingInsulin = parameters?.IdInsulinDrug_Long;
        currentShortInsulin = bl.GetOneInsulinDrug(IdCurrentShortActingInsulin);
        currentLongInsulin = bl.GetOneInsulinDrug(IdCurrentLongActingInsulin); 
        
        if (IdCurrentShortActingInsulin != null && currentShortInsulin != null)
        {
            CurrentInjection.IdInsulinDrug = IdCurrentShortActingInsulin;
            rdbShortInsulin.Content = currentShortInsulin.Name ?? "Short act.";
        }
        else
        {
            rdbShortInsulin.Content = "Short act.";
        }
        
        if (IdCurrentLongActingInsulin != null && currentLongInsulin != null)
        {
            rdbLongInsulin.Content = currentLongInsulin.Name ?? "Long act.";
        }
        else
        {
            rdbLongInsulin.Content = "Long act.";
        }
        pageIsLoading = false;
        RefreshUi();
        // Initialize change tracking
        SaveOriginalInjection();
        // Add event handlers for change tracking
        AttachChangeHandlers();
    }
    protected override bool OnBackButtonPressed()
    {
        // Intercept Android back button
        return HandleBackNavigation();
    }
    private bool HandleBackNavigation()
    {
        if (HasUnsavedChanges && !IsNavigatingAway)
        {
            ShowUnsavedChangesDialog();
            return true; // Prevent automatic navigation
        }
        return false; // Allow normal navigation
    }
    private async void ShowUnsavedChangesDialog()
    {
        var result = await DisplayActionSheet(
            "You have unsaved changes for the current injection.\nWhat do you want to do?",
            "Cancel",
            null,
            "Save", "Discard");
            
        switch (result)
        {
            case "Save":
                // Save and then navigate
                if (await TrySaveCurrentInjection())
                {
                    IsNavigatingAway = true;
                    await Shell.Current.GoToAsync("..");
                }
                break;
            case "Discard":
                // Discard changes and navigate
                HasUnsavedChanges = false;
                IsNavigatingAway = true;
                await Shell.Current.GoToAsync("..");
                break;
            case "Cancel":
            default:
                // Do nothing, stay on the page
                break;
        }
    }
    private async Task<bool> TrySaveCurrentInjection()
    {
        try
        {
            if (txtIdInjection.Text == "")
            {
                await DisplayAlert("Error", "Select an injection from the list to save it", "Ok");
                return false;
            }

            FromUiToClass();
            bool abort = await abortAfterChecksBeforeSavings();
            if (abort)
                return false;

            if (CurrentInjection.Zone == Common.ZoneOfPosition.Hands ||
                CurrentInjection.Zone == Common.ZoneOfPosition.Sensor)
            {
                // if it isn't a bolus, delete the bolus' info
                CurrentInjection.IdInsulinDrug = null;
                CurrentInjection.IdTypeOfInsulinAction = null;
                CurrentInjection.InsulinValue.Text = "";
            }

            bl.SaveOneInjection(CurrentInjection);
            RefreshGrid();
            picturePageHasBeenVisited = false;

            // Reset change tracking after saving
            HasUnsavedChanges = false;
            SaveOriginalInjection();

            return true;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Error during saving: {ex.Message}", "Ok");
            return false;
        }
    }
    private void AttachChangeHandlers()
    {
        // Add event handlers for all controls that can be modified
        txtInsulinActual.TextChanged += OnValueChanged;
        txtInsulinCalculated.TextChanged += OnValueChanged;
        dtpInjectionDate.DateSelected += OnValueChanged;
        dtpInjectionTime.TimeSelected += OnValueChanged;
        txtNotes.TextChanged += OnValueChanged;
        rdbShortInsulin.CheckedChanged += OnValueChanged;
        rdbLongInsulin.CheckedChanged += OnValueChanged;
    }
    private void OnValueChanged(object sender, EventArgs e)
    {
        if (pageIsLoading || IsNavigatingAway)
        {
            return;
        }
        if (isFirstSelection)
        {
            isFirstSelection = false;
        } else
        { 
            // Compare current state with original to determine if there are changes
            CheckForChanges();
        }
    }
    private void CheckForChanges()
    {
        if (pageIsLoading)
        {
            return;
        }
        // Create a temporary injection with current UI values
        var tempInjection = CreateInjectionFromUI();
        // Compare with original injection
        bool hasChanges = !AreInjectionsEqual(OriginalInjection, tempInjection);
        if (hasChanges != HasUnsavedChanges)
        {
            HasUnsavedChanges = hasChanges;
            UpdateTitleWithUnsavedIndicator();
        }
    }
    private Injection CreateInjectionFromUI()
    {
        var injection = new Injection();
        injection.IdInjection = Safe.Int(txtIdInjection.Text);
        injection.InsulinValue.Text = txtInsulinActual.Text;
        injection.InsulinCalculated.Text = txtInsulinCalculated.Text;

        DateTime instant = new DateTime(
            dtpInjectionDate.Date.Year, dtpInjectionDate.Date.Month, dtpInjectionDate.Date.Day,
            dtpInjectionTime.Time.Hours, dtpInjectionTime.Time.Minutes, dtpInjectionTime.Time.Seconds);
        injection.Timestamp.DateTime = instant;
        injection.Notes = txtNotes.Text;

        if (rdbShortInsulin.IsChecked)
            injection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Short;
        else if (rdbLongInsulin.IsChecked)
            injection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Long;
        else
            injection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.NotSet;

        injection.Zone = CurrentInjection.Zone;
        injection.PositionX = CurrentInjection.PositionX;
        injection.PositionY = CurrentInjection.PositionY;

        return injection;
    }
    private bool AreInjectionsEqual(Injection original, Injection current)
    {
        if (original == null && current == null) return true;
        if (original == null || current == null) return false;

        return original.IdInjection == current.IdInjection &&
               original.InsulinValue.Text == current.InsulinValue.Text &&
               original.InsulinCalculated.Text == current.InsulinCalculated.Text &&
               original.Timestamp.DateTime == current.Timestamp.DateTime &&
               original.Notes == current.Notes &&
               original.IdTypeOfInsulinAction == current.IdTypeOfInsulinAction &&
               original.Zone == current.Zone &&
               Math.Abs((original.PositionX ?? 0) - (current.PositionX ?? 0)) < 0.0001 &&
               Math.Abs((original.PositionY ?? 0) - (current.PositionY ?? 0)) < 0.0001;
    }
    private void SaveOriginalInjection()
    {
        // Save a copy of the current injection as reference for changes
        OriginalInjection = new Injection
        {
            IdInjection = CurrentInjection.IdInjection,
            InsulinValue = new DoubleAndText { Text = CurrentInjection.InsulinValue.Text },
            InsulinCalculated = new DoubleAndText { Text = CurrentInjection.InsulinCalculated.Text },
            Timestamp = new DateTimeAndText { DateTime = CurrentInjection.Timestamp.DateTime },
            Notes = CurrentInjection.Notes,
            IdTypeOfInsulinAction = CurrentInjection.IdTypeOfInsulinAction,
            Zone = CurrentInjection.Zone,
            PositionX = CurrentInjection.PositionX,
            PositionY = CurrentInjection.PositionY
        };
    }
    private void UpdateTitleWithUnsavedIndicator()
    {
        // Update page title to indicate unsaved changes
        if (HasUnsavedChanges)
        {
            this.Title = "Injections *"; // Asterisk indicates unsaved changes
        }
        else
        {
            this.Title = "Injections";
        }
    }
    public int? IdInjection
    {
        get
        {
            return CurrentInjection.IdInjection;
        }
    }
    private void FromClassToUi()
    {
        if (CurrentInjection == null)
            CurrentInjection = new Injection();

        if (CurrentInjection.IdInjection != null)
            txtIdInjection.Text = CurrentInjection.IdInjection.ToString();
        else
            txtIdInjection.Text = "";
        txtInsulinActual.Text = CurrentInjection.InsulinValue.Text;
        txtInsulinCalculated.Text = CurrentInjection.InsulinCalculated.Text;
        if (CurrentInjection.Timestamp.DateTime == null
            || CurrentInjection.Timestamp.DateTime == new DateTime(1, 1, 1, 0, 0, 0))
            CurrentInjection.Timestamp.DateTime = DateTime.Now;
        dtpInjectionDate.Date = ((DateTime)CurrentInjection.Timestamp.DateTime);
        dtpInjectionTime.Time = ((DateTime)CurrentInjection.Timestamp.DateTime).TimeOfDay;
        txtNotes.Text = CurrentInjection.Notes;

        // both Rapid and Short should map to short insulin radio button
        if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Rapid ||
            CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Short)
        {
            rdbShortInsulin.IsChecked = true;
            rdbLongInsulin.IsChecked = false;
        }
        else if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Long)
        {
            rdbShortInsulin.IsChecked = false;
            rdbLongInsulin.IsChecked = true;
        }
        else
        {
            rdbShortInsulin.IsChecked = false;
            rdbLongInsulin.IsChecked = false;
        }
    }
    private void FromUiToClass()
    {
        CurrentInjection.IdInjection = Safe.Int(txtIdInjection.Text);
        CurrentInjection.InsulinValue.Text = txtInsulinActual.Text;
        CurrentInjection.InsulinCalculated.Text = txtInsulinCalculated.Text;
        DateTime instant = new DateTime(
            dtpInjectionDate.Date.Year, dtpInjectionDate.Date.Month, dtpInjectionDate.Date.Day,
            dtpInjectionTime.Time.Hours, dtpInjectionTime.Time.Minutes, dtpInjectionTime.Time.Seconds);
        CurrentInjection.Timestamp.DateTime = instant;
        CurrentInjection.Notes = txtNotes.Text;
        
        //  short radio button maps to Short, long to Long
        if (rdbShortInsulin.IsChecked)
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Short;
        else if (rdbLongInsulin.IsChecked)
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Long;
        else
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.NotSet;
    }
    private void RefreshGrid()
    {
        if (pageIsLoading) return;
        DateTime now = DateTime.Now;
        if (chkSensors.IsChecked)
            ShownDataTimeMultiplicator = 4;
        else
            ShownDataTimeMultiplicator = 1;

        allInjections = bl.GetInjections(
            now.AddMonths(-(int)(MonthsOfDataShownInTheGrids * ShownDataTimeMultiplicator)), now.AddDays(1),
            Common.TypeOfInsulinAction.NotSet, Common.ZoneOfPosition.NotSet,
            chkFront.IsChecked, chkBack.IsChecked, chkHands.IsChecked, chkSensors.IsChecked);
        gridInjections.ItemsSource = allInjections;
    }
    private void RefreshUi()
    {
        if (pageIsLoading) return;
        FromClassToUi();
        RefreshGrid();
    }
    private void btnNow_Click(object sender, EventArgs e)
    {
        DateTime now = DateTime.Now;
        dtpInjectionDate.Date = now;
        dtpInjectionTime.Time = now.TimeOfDay;
    }
    private async void btnSave_Click(object sender, EventArgs e)
    {
        await TrySaveCurrentInjection();
        //if (txtIdInjection.Text == "")
        //{
        //    await DisplayAlert("Select one injection from the list", "Choose a injection to save", "Ok");
        //    return;
        //}
        //FromUiToClass();
        //bool abort = await abortAfterChecksBeforeSavings();
        //if (abort)
        //    return;
        //if (CurrentInjection.Zone == Common.ZoneOfPosition.Hands ||
        //    CurrentInjection.Zone == Common.ZoneOfPosition.Sensor)
        //{
        //    // if it isn't a bolus, delete the bolus' info
        //    CurrentInjection.IdInsulinDrug = null;
        //    CurrentInjection.IdTypeOfInsulinAction = null;
        //    CurrentInjection.InsulinValue.Text = "";
        //}
        //bl.SaveOneInjection(CurrentInjection);
        //RefreshGrid();
        //picturePageHasBeenVisited = false;
    }
    private async Task<bool> abortAfterChecksBeforeSavings()
    {
        bool abort = false;
        if (!bl.CheckIfInjectionHasValue(CurrentInjection))
        {
            if (await DisplayAlert("", "Missing value of bolus.\nShould we save without it?", "Save", "Abort"))
                abort = false;
            else
                abort = true;
        }
        else if (!bl.CheckIfInjectionHasLocation(CurrentInjection))
        {
            if (await DisplayAlert("", "Missing location of injection.\nShould we save without it?", "Save", "Abort"))
                abort = false;
            else
                abort = true;
        }
        if (abort)
        {
            // when aborting the saving, we restore the previous value of this injection
            CurrentInjection = bl.GetOneInjection(CurrentInjection.IdInjection);
            FromClassToUi();
        }
        return abort;
    }
    private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        // Check if there are unsaved changes before changing selection
        if (HasUnsavedChanges)
        {
            var result = await DisplayAlert(
                "Unsaved changes",
                "You have unsaved changes for the current injection.\nWhat do you want to do?",
                "Save", "Discard");
            if (result)
            {
                if (!await TrySaveCurrentInjection())
                {
                    // If saving fails, keep current selection
                    return;
                }
            }
            else
            {
                // Continue with new selection. without saving
                HasUnsavedChanges = false;
            }
        }
        // make the tapped row the current injection 
        CurrentInjection = (Injection)e.SelectedItem;

        SetTheColorsOfPictureButtons();
        
        FromClassToUi();
        
        // Update radio button content based on the injection type
        // Show the actual insulin name for the selected injection type, and default names for the other
        if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Short 
            || CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Rapid)
        {
            // the short insulin is the one that we took from the grid
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(CurrentInjection.IdInsulinDrug)?.Name ?? "Short act.";
            // the long insulin is the default long insulin that we are using
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(IdCurrentLongActingInsulin)?.Name ?? "Long act.";
        }
        else if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Long)
        {
            // the short insulin is the default short insulin that we are using
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(IdCurrentShortActingInsulin)?.Name ?? "Short act.";
            // the long insulin is the one that we took from the grid
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(CurrentInjection.IdInsulinDrug)?.Name ?? "Long act.";
        }
        else
        {
            // IdTypeOfInsulinAction is null, NotSet, or some other value
            // Show default insulin names
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(IdCurrentShortActingInsulin)?.Name ?? "Short act.";
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(IdCurrentLongActingInsulin)?.Name ?? "Long act.";
        }

        // Keep the selection visible
        if (gridInjections.SelectedItem != e.SelectedItem)
        {
            gridInjections.SelectedItem = e.SelectedItem;
        }
        picturePageHasBeenVisited = false;

        // Save the new injection as reference for change tracking
        SaveOriginalInjection();
        HasUnsavedChanges = false;
        UpdateTitleWithUnsavedIndicator();
    }
    private void SetTheColorsOfPictureButtons()
    {
        // make the injection's location button green if the zone where the injection is set,
        // if it insn't make it the original color
        if (CurrentInjection.Zone == Common.ZoneOfPosition.Front)
            btnFront.BackgroundColor = Colors.Lime;
        else
            btnFront.BackgroundColor = Colors.LightGrey;
        if (CurrentInjection.Zone == Common.ZoneOfPosition.Back)
            btnBack.BackgroundColor = Colors.Lime;
        else
            btnBack.BackgroundColor = Colors.LightGrey;
        if (CurrentInjection.Zone == Common.ZoneOfPosition.Hands)
            btnHands.BackgroundColor = Colors.Lime;
        else
            btnHands.BackgroundColor = Colors.LightGrey;
        if (CurrentInjection.Zone == Common.ZoneOfPosition.Sensor)
            btnSensors.BackgroundColor = Colors.Lime;
        else
            btnSensors.BackgroundColor = Colors.LightGrey;
    }
    private async void btnAddInjection_Click(object sender, EventArgs e)
    {
        if (!rdbShortInsulin.IsChecked && !rdbLongInsulin.IsChecked
        && CurrentInjection.Zone != Common.ZoneOfPosition.Hands
        && CurrentInjection.Zone != Common.ZoneOfPosition.Sensor)
        {
            // notify the user that he has to choose the type of insulin
            await DisplayAlert("", "Select the type of insulin of this injection", "Ok");
            return;
        }
        // if the user hasn't open a picture page and the Position of the injection is set,
        // warn the user that it is possible that the position of an old injeectio is beeing moved 
        // to this new injection
        if (!picturePageHasBeenVisited
            && (CurrentInjection.PositionX.HasValue && CurrentInjection.PositionY.HasValue))
        {
            if (await DisplayAlert("Position Already Set", "This injection already has a position from a previous selection." +
                "\nDo you want to keep this existing position for your new injection?", "Keep Position", "Clear Position"))
                return;
        }
        FromUiToClass();
        bool abort = await abortAfterChecksBeforeSavings();
        if (abort)
            return;

        if (chkNowInAdd.IsChecked)
        {
            DateTime now = DateTime.Now;
            CurrentInjection.Timestamp.DateTime = now;
            dtpInjectionDate.Date = now;
            dtpInjectionTime.Time = now.TimeOfDay;
        }
        // erase Id to abort a new record
        CurrentInjection.IdInjection = null;
        // the new record must have the default insulin determined when the page was opened
        if (rdbShortInsulin.IsChecked)
        {
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Short;
            if (currentShortInsulin != null)
            {
                CurrentInjection.IdInsulinDrug = currentShortInsulin.IdInsulinDrug;
            }
            else
            {
                // Handle case where no short-acting insulin is configured
                await DisplayAlert("Configuration Error", "No short-acting insulin is configured." +
                    "\nPlease configure insulin settings." +
                    "\nInsulin drug will be set to null", "Ok");
                CurrentInjection.IdInsulinDrug = null;
            }
        }
        else
        {
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.Long;
            if (currentLongInsulin != null)
            {
                CurrentInjection.IdInsulinDrug = currentLongInsulin.IdInsulinDrug;
            }
            else
            {
                // Handle case where no long-acting insulin is configured
                await DisplayAlert("Configuration Error", "No long-acting insulin is configured.\nPlease configure insulin settings." +
                    "\nType of insulin drug will be set to null", "Ok");
                CurrentInjection.IdInsulinDrug = null;
            }
        }
        if (CurrentInjection.Zone == Common.ZoneOfPosition.Hands ||
            CurrentInjection.Zone == Common.ZoneOfPosition.Sensor)
        {
            // if it isn't a bolus, delete the bolus' info
            CurrentInjection.IdInsulinDrug = null;
            CurrentInjection.IdTypeOfInsulinAction = null;
            CurrentInjection.InsulinValue.Text = "";
        }
        bl.SaveOneInjection(CurrentInjection);
        RefreshGrid();
        picturePageHasBeenVisited = false;

        // Reset change tracking after addition
        SaveOriginalInjection();
        HasUnsavedChanges = false;
        UpdateTitleWithUnsavedIndicator();
    }
    private async void btnRemoveInjection_Click(object sender, EventArgs e)
    {
        Injection inj = (Injection)gridInjections.SelectedItem;
        if (inj != null)
        {
            bool remove = await DisplayAlert(String.Format(
                "Should I delete the injection of {1}, insulin {0}?",
                inj.InsulinValue.ToString(),
                inj.Timestamp.ToString(),
                inj.IdInjection.ToString()),
                "", "Yes", "No");
            if (remove)
            {
                bl.DeleteOneInjection(inj);
                RefreshGrid();
            }
        }
        else
        {
            await DisplayAlert("Saving not possible", "Choose an injection to delete", "Ok");
            return;
        }
        RefreshGrid();
    }
    private async void btnFront_ClickedAsync(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Front;
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.Bolus;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
        picturePageHasBeenVisited = true;
    }
    private async void btnBack_Clicked_Async(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Back;
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.Bolus;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
        picturePageHasBeenVisited = true;
    }
    private async void btnHands_ClickedAsync(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Hands;
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.Blood;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
        picturePageHasBeenVisited = true;
    }
    private async void btnSensors_Clicked_Async(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Sensor;  
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.Sensor;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
        picturePageHasBeenVisited = true;
    }
    private void chkChanged(object sender, CheckedChangedEventArgs e)
    {
        RefreshGrid();
    }
    private void btnDefault_Click(object sender, EventArgs e)
    {
        CurrentInjection = new Injection();
        FromClassToUi();
        SetTheColorsOfPictureButtons();
    }
}
