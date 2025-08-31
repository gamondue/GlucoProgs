using gamon;
using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;

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

    double? MonthsOfDataShownInTheGrids = 3;

    // TODO fix the behaviour of the code, that set the "Short act." radiobutton also when the insulin is Long action
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
        if (CurrentInjection.IdInjection != null)
            txtIdInjection.Text = CurrentInjection.IdInjection.ToString();
        else
            txtIdInjection.Text = "";
        txtInsulinActual.Text = CurrentInjection.InsulinValue.Text;
        txtInsulinCalculated.Text = CurrentInjection.InsulinCalculated.Text;
        dtpInjectionDate.Date = ((DateTime)CurrentInjection.Timestamp.DateTime);
        dtpInjectionTime.Time = ((DateTime)CurrentInjection.Timestamp.DateTime).TimeOfDay;
        txtNotes.Text = CurrentInjection.Notes;
        
        // both RapidActing and ShortActing should map to short insulin radio button
        if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.RapidActing ||
            CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.ShortActing)
        {
            rdbShortInsulin.IsChecked = true;
            rdbLongInsulin.IsChecked = false;
        }
        else if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.LongActing)
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
        
        //  short radio button maps to ShortActing, long to LongActing
        if (rdbShortInsulin.IsChecked)
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.ShortActing;
        else if (rdbLongInsulin.IsChecked)
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.LongActing;
        else
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.NotSet;
    }
    private void RefreshGrid()
    {
        if (pageIsLoading) return;
        DateTime now = DateTime.Now;
        allInjections = bl.GetInjections(
            now.AddMonths(-(int)(MonthsOfDataShownInTheGrids)), now.AddDays(1),
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
        if (txtIdInjection.Text == "")
        {
            await DisplayAlert("Select one injection from the list", "Choose a injection to save", "Ok");
            return;
        }
        FromUiToClass();
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
    }
    private async void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        // make the tapped row the current injection 
        CurrentInjection = (Injection)e.SelectedItem;

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
        
        FromClassToUi();
        
        // Update radio button content based on the injection type
        // Show the actual insulin name for the selected injection type, and default names for the other
        if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.ShortActing 
            || CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.RapidActing)
        {
            // the short insulin is the one that we took from the grid
            rdbShortInsulin.Content = bl.GetOneInsulinDrug(CurrentInjection.IdInsulinDrug)?.Name ?? "Short act.";
            // the long insulin is the default long insulin that we are using
            rdbLongInsulin.Content = bl.GetOneInsulinDrug(IdCurrentLongActingInsulin)?.Name ?? "Long act.";
        }
        else if (CurrentInjection.IdTypeOfInsulinAction == (int)Common.TypeOfInsulinAction.LongActing)
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
    }
    private async void btnAddInjection_Click(object sender, EventArgs e)
    {
        if (!rdbShortInsulin.IsChecked && !rdbLongInsulin.IsChecked
            && CurrentInjection.Zone != Common.ZoneOfPosition.Hands
            && CurrentInjection.Zone != Common.ZoneOfPosition.Sensor)
        {
            // notify the user that he has to choose the type of insulin
            await DisplayAlert("", "Tap on the type of insulin of this injection", "Ok");
            return;
        }
        if (chkNowInAdd.IsChecked)
        {
            DateTime now = DateTime.Now;
            dtpInjectionDate.Date = now;
            dtpInjectionTime.Time = now.TimeOfDay;
        }
        FromUiToClass();
        // erase Id to save a new record
        CurrentInjection.IdInjection = null;
        // the new record must have the default insulin determined when the page was opened
        if (rdbShortInsulin.IsChecked)
        {
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.ShortActing;
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
            CurrentInjection.IdTypeOfInsulinAction = (int)Common.TypeOfInsulinAction.LongActing;
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
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.InsulinBolus;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
    }
    private async void btnBack_Clicked_Async(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Back;
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.InsulinBolus;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
    }
    private async void btnHands_ClickedAsync(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Hands;
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.BloodSample;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
    }
    private async void btnSensors_Clicked_Async(object sender, EventArgs e)
    {
        CurrentInjection.Zone = Common.ZoneOfPosition.Sensor;
        // pass the type of injection
        CurrentInjection.IdTypeOfInjection = (int)Common.TypeOfInjection.SensorImplantation;
        await Navigation.PushAsync(new ClickableImagePage(ref CurrentInjection));
    }
    private void chkChanged(object sender, CheckedChangedEventArgs e)
    {
        RefreshGrid();
    }
}
