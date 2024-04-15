using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan.Maui;
public partial class InjectionsPage : ContentPage
{
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();
    InsulinInjection CurrentInjection = new InsulinInjection();
    List<InsulinInjection> allInjections;

    internal InjectionsPage(int? IdInjection)
    {
        InitializeComponent();
        if (IdInjection != null)
            CurrentInjection = bl.GetOneInjection(IdInjection);
        if (IdInjection == null)
            bl.SetTypeOfInsulinSpeedBasedOnTimeNow(CurrentInjection);
        RefreshUi();
    }
    public int? IdInsulinInjection
    {
        get
        {
            return CurrentInjection.IdInsulinInjection;
        }
    }
    private void FromClassToUi()
    {
        if (CurrentInjection.IdInsulinInjection != null)
            txtIdInjection.Text = CurrentInjection.IdInsulinInjection.ToString();
        else
            txtIdInjection.Text = "";
        txtInsulinActual.Text = CurrentInjection.InsulinValue.Text;
        txtInsulinCalculated.Text = CurrentInjection.InsulinCalculated.Text;
        dtpInjectionDate.Date = ((DateTime)CurrentInjection.Timestamp.DateTime);
        dtpInjectionTime.Time = ((DateTime)CurrentInjection.Timestamp.DateTime).TimeOfDay;
        txtNotes.Text = CurrentInjection.Notes;
        if (CurrentInjection.IdTypeOfInsulinSpeed == (int)Common.TypeOfInsulinSpeed.RapidActing)
            rdbFastInsulin.IsChecked = true;
        else if (CurrentInjection.IdTypeOfInsulinSpeed == (int)Common.TypeOfInsulinSpeed.ShortActing)
            rdbSlowInsulin.IsChecked = true;
        else
        {
            rdbFastInsulin.IsChecked = false;
            rdbSlowInsulin.IsChecked = false;
        }
    }
    private void FromUiToClass()
    {
        CurrentInjection.IdInsulinInjection = SqlSafe.Int(txtIdInjection.Text);
        CurrentInjection.InsulinValue.Text = txtInsulinActual.Text;
        CurrentInjection.InsulinCalculated.Text = txtInsulinCalculated.Text;
        DateTime instant = new DateTime(
            dtpInjectionDate.Date.Year, dtpInjectionDate.Date.Month, dtpInjectionDate.Date.Day,
            dtpInjectionTime.Time.Hours, dtpInjectionTime.Time.Minutes, dtpInjectionTime.Time.Seconds);
        CurrentInjection.Timestamp.DateTime = instant;
        CurrentInjection.Notes = txtNotes.Text;
        if (rdbFastInsulin.IsChecked)
            CurrentInjection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.RapidActing;
        else if (rdbSlowInsulin.IsChecked)
            CurrentInjection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.ShortActing;
        else
            CurrentInjection.IdTypeOfInsulinSpeed = (int)Common.TypeOfInsulinSpeed.NotSet;
    }
    private void RefreshGrid()
    {
        DateTime now = DateTime.Now;
        allInjections = bl.GetInjections(now.AddMonths(-2), now, Common.TypeOfInsulinSpeed.NotSet);
        gridInjections.ItemsSource = allInjections;
    }
    private void RefreshUi()
    {
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
        //make the tapped row the current injection 
        CurrentInjection = (InsulinInjection)e.SelectedItem;
        FromClassToUi();
    }
    private void btnAddInjection_Click(object sender, EventArgs e)
    {
        if (chkNowInAdd.IsChecked)
        {
            DateTime now = DateTime.Now;
            dtpInjectionDate.Date = now;
            dtpInjectionTime.Time = now.TimeOfDay;
        }
        FromUiToClass();
        // erase Id to save a new record
        CurrentInjection.IdInsulinInjection = null;
        bl.SaveOneInjection(CurrentInjection);
        RefreshGrid();
    }
    private async void btnRemoveInjection_Click(object sender, EventArgs e)
    {
        InsulinInjection inj = (InsulinInjection)gridInjections.SelectedItem;
        if (inj != null)
        {
            bool remove = await DisplayAlert(String.Format(
                "Should I delete the injection of {1}, insulin {0}, Id {2}?",
                inj.InsulinValue.ToString(),
                inj.Timestamp.ToString(),
                inj.IdInsulinInjection.ToString()),
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
        await Navigation.PushAsync(new FrontPicturePage());
    }
    private async void btnBack_Clicked_Async(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BackPicturePage());
    }
    private async void btnHands_ClickedAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HandsPicturePage());
    }
    private async void btnSensors_Clicked_Async(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SensorsPicturePage());
    }
}
