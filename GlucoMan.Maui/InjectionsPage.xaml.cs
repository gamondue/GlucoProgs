using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.ApplicationModel;
using System.ComponentModel.Design;
using static GlucoMan.Common;

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

        // Wrap initialization that may touch the database or platform APIs
        // in a try/catch so Release builds don't crash the app silently.
        try
        {
            Parameters parameters = DatabaseService.Instance.Database.GetParameters();
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
        catch (Exception ex)
        {
            // Log and show user-friendly alert on UI thread. In Release the app
            // previously crashed without any visible alert — this ensures we log
            // the root cause and notify the user while keeping the app alive.
            General.LogOfProgram?.Error("InjectionsPage | ctor", ex);
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    // Fire-and-forget DisplayAlert to avoid blocking constructor
                    _ = DisplayAlert("Errore", "Impossibile aprire la pagina Iniezioni. Controlla i log.", "OK");
                });
            }
            catch
            {
                // swallow — nothing else we can do safely here
            }
        }
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
            AppStrings.UnsavedChangesMessage,
            AppStrings.Cancel,
            null,
            AppStrings.Save, AppStrings.Discard);
            
        switch (result)
        {
            case var s when s == AppStrings.Save:
                // Save and then navigate
                if (await TrySaveCurrentInjection())
                {
                    IsNavigatingAway = true;
                    await Shell.Current.GoToAsync("..");
                }
                break;
            case var d when d == AppStrings.Discard:
                // Discard changes and navigate
                HasUnsavedChanges = false;
                IsNavigatingAway = true;
                await Shell.Current.GoToAsync("..");
                break;
            case var c when c == AppStrings.Cancel:
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
                await DisplayAlert(AppStrings.Error, AppStrings.SelectInjectionToSave, AppStrings.OK);
                return false;
            }

            FromUiToClass();
            bool abort = await abortAfterChecksBeforeSavings();
            if (abort)
                return false;

            SetCurrentInjectionParametersBasedOnZone();
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
            await DisplayAlert(AppStrings.Error, $"{AppStrings.ErrorDuringSaving}: {ex.Message}", AppStrings.OK);
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
            dtpInjectionDate.Date.Value.Year, dtpInjectionDate.Date.Value.Month, dtpInjectionDate.Date.Value.Day,
            dtpInjectionTime.Time.Value.Hours, dtpInjectionTime.Time.Value.Minutes, dtpInjectionTime.Time.Value.Seconds);
        injection.EventTime.DateTime = instant;
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
               original.EventTime.DateTime == current.EventTime.DateTime &&
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
            EventTime = new DateTimeAndText { DateTime = CurrentInjection.EventTime.DateTime },
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
            this.Title = $"{AppStrings.InjectionsPageTitle} *"; // Asterisk indicates unsaved changes
        }
        else
        {
            this.Title = AppStrings.InjectionsPageTitle;
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
        if (CurrentInjection.EventTime.DateTime == null
            || CurrentInjection.EventTime.DateTime == new DateTime(1, 1, 1, 0, 0, 0))
            CurrentInjection.EventTime.DateTime = DateTime.Now;
        dtpInjectionDate.Date = ((DateTime)CurrentInjection.EventTime.DateTime);
        dtpInjectionTime.Time = ((DateTime)CurrentInjection.EventTime.DateTime).TimeOfDay;
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
            dtpInjectionDate.Date.Value.Year, dtpInjectionDate.Date.Value.Month, dtpInjectionDate.Date.Value.Day,
            dtpInjectionTime.Time.Value.Hours, dtpInjectionTime.Time.Value.Minutes, dtpInjectionTime.Time.Value.Seconds);
        CurrentInjection.EventTime.DateTime = instant;
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
        {
            ShownDataTimeMultiplicator = 6;
        }
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
    }
    private async Task<bool> abortAfterChecksBeforeSavings()
    {
        bool abort = false;
        if (!bl.CheckIfInjectionHasValue(CurrentInjection))
        {
            if (await DisplayAlert("", AppStrings.MissingBolusValue, AppStrings.Save, AppStrings.Abort))
                abort = false;
            else
                abort = true;
        }
        else if (!bl.CheckIfInjectionHasLocation(CurrentInjection))
        {
            if (await DisplayAlert("", AppStrings.MissingInjectionLocation, AppStrings.Save, AppStrings.Abort))
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
    // Support a direct tap on the item Frame to set selection (helps on Android)
    private async void OnItemTapped(object? sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is Injection tapped)
        {
            // set selected item which will trigger SelectionChanged
            gridInjections.SelectedItem = tapped;
            try
            {
                await HandleSelectionAsync(tapped);
            }
            catch
            {
                // ignore any errors from manual invocation
            }
        }
    }

    // Centralized selection handling extracted so both tap and selectionchanged can reuse it
    private async Task HandleSelectionAsync(Injection selectedInjection)
    {
        if (selectedInjection == null)
            return;

        // Check if there are unsaved changes before changing selection
        if (HasUnsavedChanges)
        {
            var save = await DisplayAlert(
                AppStrings.UnsavedChanges,
                AppStrings.UnsavedChangesMessage,
                AppStrings.Save, AppStrings.Discard);
            if (save)
            {
                if (!await TrySaveCurrentInjection())
                {
                    // If saving fails, keep current selection
                    return;
                }
            }
            else
            {
                // Continue with new selection without saving
                HasUnsavedChanges = false;
            }
        }

        // Deselect all other items in the list
        if (allInjections != null)
        {
            foreach (var injection in allInjections)
            {
                injection.IsSelectedInList = false;
            }
        }

        // Select the current item
        selectedInjection.IsSelectedInList = true;

        // make the tapped row the current injection 
        CurrentInjection = selectedInjection;

        SetTheColorsOfPictureButtons();

        FromClassToUi();

        // Update radio button content based on the injection type
        if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Short
            || CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Rapid)
        {
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(CurrentInjection.IdInsulinDrug)?.Name ?? "Short act.";
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(IdCurrentLongActingInsulin)?.Name ?? "Long act.";
        }
        else if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.Long)
        {
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(IdCurrentShortActingInsulin)?.Name ?? "Short act.";
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(CurrentInjection.IdInsulinDrug)?.Name ?? "Long act.";
        }
        else
        {
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(IdCurrentShortActingInsulin)?.Name ?? "Short act.";
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(IdCurrentLongActingInsulin)?.Name ?? "Long act.";
        }

        // Keep the selection visible
        if (gridInjections.SelectedItem != selectedInjection)
        {
            gridInjections.SelectedItem = selectedInjection;
        }
        picturePageHasBeenVisited = false;

        // Save the new injection as reference for change tracking
        SaveOriginalInjection();
        HasUnsavedChanges = false;
        UpdateTitleWithUnsavedIndicator();
    }
    private async void OnGridSelectionAsync(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection == null || e.CurrentSelection.Count == 0)
            return;

        var selectedInjection = (Injection)e.CurrentSelection[0];
        await HandleSelectionAsync(selectedInjection);
    }
    private void SetTheColorsOfPictureButtons()
    {
        // make the injection's location button green if the zone where the injection is set,
        // if it ins't make it the original color
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
            await DisplayAlert("", AppStrings.SelectInsulinType, AppStrings.OK);
            return;
        }
        // if the user hasn't open a picture page and the Position of the injection is set,
        // warn the user that it is possible that the position of an old injeectio is beeing moved 
        // to this new injection
        if (!picturePageHasBeenVisited
            && (CurrentInjection.PositionX.HasValue && CurrentInjection.PositionY.HasValue))
        {
            if (await DisplayAlert(AppStrings.PositionAlreadySet, AppStrings.PositionAlreadySetMessage, 
                AppStrings.KeepPosition, AppStrings.ClearPosition))
                return;
        }
        FromUiToClass();
        bool abort = await abortAfterChecksBeforeSavings();
        if (abort)
            return;

        if (chkNowInAdd.IsChecked)
        {
            DateTime now = DateTime.Now;
            CurrentInjection.EventTime.DateTime = now;
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
                await DisplayAlert(AppStrings.ConfigurationError, AppStrings.NoShortInsulinConfigured, AppStrings.OK);
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
                await DisplayAlert(AppStrings.ConfigurationError, AppStrings.NoLongInsulinConfigured, AppStrings.OK);
                CurrentInjection.IdInsulinDrug = null;
            }
        }
        SetCurrentInjectionParametersBasedOnZone();
        bl.SaveOneInjection(CurrentInjection);
        RefreshGrid();
        picturePageHasBeenVisited = false;

        // Reset change tracking after addition
        SaveOriginalInjection();
        HasUnsavedChanges = false;
        UpdateTitleWithUnsavedIndicator();
    }
    private void SetCurrentInjectionParametersBasedOnZone()
    {
        if (CurrentInjection.Zone == Common.ZoneOfPosition.Hands ||
            CurrentInjection.Zone == Common.ZoneOfPosition.Sensor)
        {
            // if it isn't a bolus, delete the bolus' info
            CurrentInjection.IdInsulinDrug = null;
            CurrentInjection.IdTypeOfInsulinAction = null;
            CurrentInjection.InsulinValue.Text = "";
            // set the type of the injection
            if (CurrentInjection.Zone == Common.ZoneOfPosition.Hands)
                CurrentInjection.IdTypeOfInjection = (int)TypeOfInjection.Blood;
            else if (CurrentInjection.Zone == Common.ZoneOfPosition.Sensor)
                CurrentInjection.IdTypeOfInjection = (int)TypeOfInjection.Sensor;
        }
        else
        {
            // set the type of the injection as bolus
            CurrentInjection.IdTypeOfInjection = (int)TypeOfInjection.Bolus;
        }
    }
    private async void btnRemoveInjection_Click(object sender, EventArgs e)
    {
        Injection inj = (Injection)gridInjections.SelectedItem;
        if (inj != null)
        {
            bool remove = await DisplayAlert(String.Format(
                AppStrings.DeleteInjectionConfirm,
                inj.InsulinValue.ToString(),
                inj.EventTime.ToString(),
                inj.IdInjection.ToString()),
                "", AppStrings.Yes, AppStrings.No);
            if (remove)
            {
                bl.DeleteOneInjection(inj);
                RefreshGrid();
            }
        }
        else
        {
            await DisplayAlert(AppStrings.SavingNotPossible, AppStrings.ChooseInjectionToDelete, AppStrings.OK);
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
