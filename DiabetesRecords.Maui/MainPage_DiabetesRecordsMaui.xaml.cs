using gamon;

namespace DiabetesRecords; 

public partial class MainPage : CarouselView
{
    BusinessLayer bl;
    DiabetesRecord currentRecord = new DiabetesRecord();
    InsulinTherapy therapy;
    public MainPage()
    {
        InitializeComponent();

        bl = new BusinessLayer();

        string ProgramVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        string ProgramName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name.ToString();
        string ProgramIdentification = ProgramName + " v." + ProgramVersion;

        bl.MakeFoldersAndFiles();

        lblAppName.Text = ProgramIdentification;

        ResetNewPageToDefaults();

        RefreshGrid();
    }
    private void FromClassToUi()
    {
        txtIdDiabetesRecord.Text = currentRecord.IdDiabetesRecord.ToString();
        if (currentRecord.Timestamp != null && currentRecord.Timestamp != General.DateNull)
        {
            dtpDateOfRecord.Date = (DateTime)Safe.DateTime(currentRecord.Timestamp);
            dtpTimeOfRecord.Time = ((DateTime)currentRecord.Timestamp).TimeOfDay;
        }
        txtGlucose.Text = currentRecord.GlucoseValue.ToString();
        txtInsulin.Text = currentRecord.InsulinValue.ToString();
        txtNote.Text = currentRecord.Notes;

        SetRadioButtonsBasedOnTypeOfInsulinSpeed(currentRecord,
            rdbFastInsulin, rdbSlowInsulin);
        SetRadioButtonsBasedOnTypeOfMeal(currentRecord,
            rdbBreakfast, rdbLunch, rdbDinner, rdbSnack);
    }
    private void FromUiToClass()
    {
        double dummy;
        double.TryParse(txtIdDiabetesRecord.Text, out dummy);
        currentRecord.IdDiabetesRecord = (int?)dummy;
        DateTime instant = new DateTime(
            dtpDateOfRecord.Date.Year, dtpDateOfRecord.Date.Month, dtpDateOfRecord.Date.Day,
            dtpTimeOfRecord.Time.Hours, dtpTimeOfRecord.Time.Minutes, dtpTimeOfRecord.Time.Seconds);
        currentRecord.Timestamp = instant;
        double.TryParse(txtGlucose.Text, out dummy);
        currentRecord.GlucoseValue = dummy;
        double.TryParse(txtInsulin.Text, out dummy);
        currentRecord.InsulinValue = dummy;
        currentRecord.Notes = txtNote.Text;
        currentRecord.IdTypeOfInsulinSpeed = GetTypeOfInsulinSpeedFromRadioButtons(rdbFastInsulin, rdbSlowInsulin);
        currentRecord.IdTypeOfMeal = GetTypeOfMealFromRadioButtons(rdbBreakfast, rdbLunch, rdbDinner, rdbLunch);
    }
    private void RefreshGrid()
    {
        DateTime now = DateTime.Now;
        gridRecords.BindingContext = bl.GetDiabetesRecords(now.AddMonths(-2), now.AddDays(1));
    }
    private void RefreshUi()
    {
        FromClassToUi();
        RefreshGrid();
    }
    private async void btnSaveNewRecord_ClickedAsync(object sender, EventArgs e)
    {
        // save new record with now as time 
        try
        {
            currentRecord.Timestamp = DateTime.Now;
            double dummy;
            double.TryParse(txtNewGlucose.Text, out dummy);
            currentRecord.GlucoseValue = dummy;
            double.TryParse(txtNewInsulin.Text, out dummy);
            currentRecord.InsulinValue = dummy;
            currentRecord.Notes = txtNewNote.Text;
            currentRecord.IdTypeOfInsulinSpeed = GetTypeOfInsulinSpeedFromRadioButtons(rdbNewFastInsulin, rdbNewSlowInsulin);
            currentRecord.IdTypeOfMeal = GetTypeOfMealFromRadioButtons(rdbNewBreakfast,
                rdbNewLunch, rdbNewDinner, rdbNewSnack);
            bl.InsertOrUpdate(currentRecord);
            RefreshUi();
        }
        catch (Exception ex)
        {
            General.Log.Error("btnSaveNewRecord_Clicked | " + ex.Message, null);
            ////////await DisplayAlert("Errore", "Scrivere bene tutti i numeri", "Ok");
            return;
        }

        dtpNewDate.Date = (DateTime)Safe.DateTime(currentRecord.Timestamp);
        dtpNewTime.Time = ((DateTime)currentRecord.Timestamp).TimeOfDay;
        // notify that save was accomplished 
        //////////await DisplayAlert("", "Salvataggio fatto", "Ok");
    }
    private int GetTypeOfMealFromRadioButtons(RadioButton RdbBreakfast,
        RadioButton RdbLunch, RadioButton RdbDinner, RadioButton RdbSnack)
    {
        if (RdbBreakfast.IsChecked)
            return (int)BusinessLayer.TypeOfMeal.Breakfast;
        else if (RdbLunch.IsChecked)
            return (int)BusinessLayer.TypeOfMeal.Lunch;
        else if (RdbDinner.IsChecked)
            return (int)BusinessLayer.TypeOfMeal.Dinner;
        else if (RdbSnack.IsChecked)
            return (int)BusinessLayer.TypeOfMeal.Snack;
        return (int)BusinessLayer.TypeOfMeal.NotSet;
    }
    private async void btnSaveRecord_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdDiabetesRecord.Text == "")
        {
            //////////await DisplayAlert("Scegliere una riga dalla lista", "Scegliere i dati da salvare", "Ok");
            return;
        }
        FromUiToClass();
        bl.SaveOneDiabetesRecord(currentRecord);
        RefreshUi();
    }
    private async void btnAddRecord_Click_Async(object sender, EventArgs e)
    {
        FromUiToClass();
        currentRecord.IdDiabetesRecord = null;
        currentRecord.Timestamp = DateTime.Now;
        bl.SaveOneDiabetesRecord(currentRecord);
        RefreshUi();
    }
    private async void btnRemoveRecord_ClickAsync(object sender, EventArgs e)
    {
        if (txtIdDiabetesRecord.Text == "")
        {
            //////////await DisplayAlert("Scegliere una riga dalla lista", "Scegliere i dati da eliminare", "Ok");
            return;
        }
        ////////bool remove = await DisplayAlert(
        ////////    String.Format("Cancello la registrazione del {0}, Id {1}?",
        ////////    currentRecord.Timestamp,
        ////////    currentRecord.IdDiabetesRecord),
        ////////    "", "Sì", "No");
        ////////if (remove)
        ////////{
            bl.DeleteOneDiabetesRecord(currentRecord);
            RefreshUi();
        ////////}
    }
    private async void btnDeleteFields_Click_Async(object sender, EventArgs e)
    {
        txtIdDiabetesRecord.Text = "";
        txtGlucose.Text = "";
        txtInsulin.Text = "";
        txtNote.Text = "";
        dtpDateOfRecord.Date = DateTime.Now.Date;
        dtpTimeOfRecord.Time = DateTime.Now.TimeOfDay;

        bl.SetTypeOfInsulinSpeedBasedOnTimeNow(currentRecord);
        SetRadioButtonsBasedOnTypeOfInsulinSpeed(currentRecord,
            rdbFastInsulin, rdbSlowInsulin);
        bl.SetTypeOfMealBasedOnTimeNow(currentRecord);
        SetRadioButtonsBasedOnTypeOfMeal(currentRecord,
            rdbBreakfast, rdbLunch, rdbDinner, rdbSnack);
        FromUiToClass();
    }
    private async void btnCopyProgramsFiles_Click(object sender, EventArgs e)
    {
        if (!bl.ExportProgramsFiles())
        {
            //////////await DisplayAlert("", "Errore nell'esportazione dei file del programma. NON tutti i filesono stati copiati, verifichi nei logs", "OK");
        }
    }
    private async void btnResetDatabase_Click(object sender, EventArgs e)
    {
        ////////////bool remove = await DisplayAlert("Devo cancellare TUTTO il database? Tutti i dati saranno persi!",
            //////////"", "Sì", "No");
        ////////if (remove)
        ////////{
            // deleting the database file
            // after deletion the software will automatically re-create the database
            if (!bl.DeleteDatabase())
            {
                //////////DisplayAlert("", "Errore nella cancellazione del file. File NON cancellato", "OK");
            }
        ////////};
    }
    private async void txt_TextChanghed(object sender, EventArgs e)
    {
        if (therapy == null)
        {
            therapy = new InsulinTherapy();
            if (!therapy.GetTherapy())
            {
                //////////await DisplayAlert("Errore di lettura",
                //////////    "File della terapia sbagliato, uso la terapia inclusa nel programma!",
                //////////    "OK");
            }
        }
        double glucose;
        double.TryParse(txtNewGlucose.Text, out glucose);
        txtHintedInsulin.Text = therapy.CalcInsulinHint(glucose,
            GetTypeOfInsulinSpeedFromRadioButtons(rdbNewFastInsulin, rdbNewSlowInsulin),
            GetTypeOfMealFromRadioButtons(rdbNewBreakfast,
                rdbNewLunch, rdbNewDinner, rdbNewSnack)).ToString();
    }
    private void OnGridSelectionAsync(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
        {
            //await DisplayAlert("XXXX", "YYYY", "Ok");
            return;
        }
        //make the tapped row the current meal 
        currentRecord = (DiabetesRecord)e.SelectedItem;
        FromClassToUi();
    }
    private void CurrentPageChanged(object sender, EventArgs e)
    {
        // if it is the first page and it must be reset, then is reset 
        ////////object currentPageIndex = this.Pages.;
        if (bl != null /*&& currentPageIndex == 0*/
            && !bl.IsLastRecordRecent())
        {
            ResetNewPageToDefaults();
        }
        if (txtIdDiabetesRecord.Text == "")
        {
            bl.SetTypeOfInsulinSpeedBasedOnTimeNow(currentRecord);
            SetRadioButtonsBasedOnTypeOfInsulinSpeed(currentRecord, rdbNewFastInsulin, rdbNewSlowInsulin);
            bl.SetTypeOfMealBasedOnTimeNow(currentRecord);
            SetRadioButtonsBasedOnTypeOfMeal(currentRecord,
                rdbNewBreakfast, rdbNewLunch, rdbNewDinner, rdbNewSnack);
        }
        ////////base.CurrentItemChanged();
    }
    private void ResetNewPageToDefaults()
    {
        txtNewGlucose.Text = "";
        txtNewInsulin.Text = "";
        txtHintedInsulin.Text = "";
        txtNewNote.Text = "";
        bl.SetTypeOfInsulinSpeedBasedOnTimeNow(currentRecord);
        SetRadioButtonsBasedOnTypeOfInsulinSpeed(currentRecord, rdbNewFastInsulin, rdbNewSlowInsulin);
        bl.SetTypeOfMealBasedOnTimeNow(currentRecord);
        SetRadioButtonsBasedOnTypeOfMeal(currentRecord,
            rdbNewBreakfast, rdbNewLunch, rdbNewDinner, rdbNewSnack);
    }
    private void SetRadioButtonsBasedOnTypeOfMeal(DiabetesRecord currentRecord,
        RadioButton RdbBreakfast, RadioButton RdbLunch, RadioButton RdbDinner, RadioButton RdbSnack)
    {
        if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Breakfast)
            RdbBreakfast.IsChecked = true;
        else if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Lunch)
            RdbLunch.IsChecked = true;
        else if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Dinner)
            RdbDinner.IsChecked = true;
        else if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Snack)
            RdbSnack.IsChecked = true;
        else
        {
            RdbBreakfast.IsChecked = false;
            RdbLunch.IsChecked = false;
            RdbDinner.IsChecked = false;
            RdbSnack.IsChecked = false;
        }
    }
    private void SetTypeOfMealBasedOnRadioButtons(DiabetesRecord currentRecord,
        RadioButton RdbBreakfast, RadioButton RdbLunch, RadioButton RdbDinner, RadioButton RdbSnack)
    {
        if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Breakfast)
            RdbBreakfast.IsChecked = true;
        else if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Lunch)
            RdbLunch.IsChecked = true;
        else if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Dinner)
            RdbDinner.IsChecked = true;
        else if (currentRecord.IdTypeOfMeal == (int)BusinessLayer.TypeOfMeal.Snack)
            RdbSnack.IsChecked = true;
    }
    private int GetTypeOfInsulinSpeedFromRadioButtons(
        RadioButton RdbFastInsulin, RadioButton RdbSlowInsulin)
    {
        int code;
        if (RdbFastInsulin.IsChecked)
            code = (int)BusinessLayer.TypeOfInsulinSpeed.QuickAction;
        else
            code = (int)BusinessLayer.TypeOfInsulinSpeed.SlowAction;
        return code;
    }
    private void SetRadioButtonsBasedOnTypeOfInsulinSpeed(DiabetesRecord currentRecord,
        RadioButton RdbFastInsulin, RadioButton RdbSlowInsulin)
    {
        if (currentRecord.IdTypeOfInsulinSpeed == (int)BusinessLayer.TypeOfInsulinSpeed.QuickAction)
            RdbFastInsulin.IsChecked = true;
        else if (currentRecord.IdTypeOfInsulinSpeed == (int)BusinessLayer.TypeOfInsulinSpeed.SlowAction)
            RdbSlowInsulin.IsChecked = true;
        else
        {
            RdbFastInsulin.IsChecked = false;
            RdbSlowInsulin.IsChecked = false;
        }
    }
}