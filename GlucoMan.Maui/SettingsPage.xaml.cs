using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GlucoMan.Maui;

public partial class SettingsPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public Parameters Parameters { get; set; }
    public List<InsulinDrug> ShortActingInsulins { get; set; }
    public List<InsulinDrug> LongActingInsulins { get; set; }
    public InsulinDrug SelectedShortActingInsulin { get; set; }
    public InsulinDrug SelectedLongActingInsulin { get; set; }
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();

    public SettingsPage()
    {
        InitializeComponent();

        // Fill the combos with the insulins
        ShortActingInsulins = bl.GetAllInsulinDrugs(Common.TypeOfInsulinAction.Short);
        LongActingInsulins = bl.GetAllInsulinDrugs(Common.TypeOfInsulinAction.Long);
        
        cmbShortActingInsulin.ItemsSource = ShortActingInsulins;
        cmbLongActingInsulin.ItemsSource = LongActingInsulins;

        Parameters = Common.BlGeneral.GetSettingsPageParameters();

        if (Parameters != null)
        {
            BindingContext = Parameters;
            // Set the selected insulins
            SelectedShortActingInsulin = ShortActingInsulins.FirstOrDefault(insulin => insulin.IdInsulinDrug == Parameters.IdInsulinDrug_Short);
            cmbShortActingInsulin.SelectedItem = SelectedShortActingInsulin;
            txtInsulinShortDuration.Text = SelectedShortActingInsulin?.DurationInHours?.ToString() ?? string.Empty;

            SelectedLongActingInsulin = LongActingInsulins.FirstOrDefault(insulin => insulin.IdInsulinDrug == Parameters.IdInsulinDrug_Long);
            cmbLongActingInsulin.SelectedItem = SelectedLongActingInsulin;
            txtInsulinLongDuration.Text = SelectedLongActingInsulin?.DurationInHours?.ToString() ?? string.Empty;
        }
        else
        {
            // Default selection if parameters are null
            SelectedShortActingInsulin = null;
            SelectedLongActingInsulin = null;

            cmbShortActingInsulin.SelectedItem = null;
            cmbLongActingInsulin.SelectedItem = null;

            txtInsulinShortDuration.Text = string.Empty;
            txtInsulinLongDuration.Text = string.Empty;
        }
    }
    private void cmbLongActingInsulin_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedLongActingInsulin = (InsulinDrug)cmbLongActingInsulin.SelectedItem;
        txtInsulinLongDuration.Text = SelectedLongActingInsulin.DurationInHours.ToString();
    }
    private void cmbShortActingInsulin_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedShortActingInsulin = (InsulinDrug)cmbShortActingInsulin.SelectedItem;
        txtInsulinShortDuration.Text = SelectedShortActingInsulin.DurationInHours.ToString();
    }
    private void Button_Clicked(object sender, EventArgs e)
    {
        SelectedShortActingInsulin.DurationInHours = double.TryParse(txtInsulinShortDuration.Text, 
            out double shortDuration) ? shortDuration : 0;
        SelectedLongActingInsulin.DurationInHours = double.TryParse(txtInsulinLongDuration.Text, 
            out double longDuration) ? longDuration : 0;
        Common.BlGeneral.SaveAllParameters(Parameters, SelectedShortActingInsulin,
            SelectedLongActingInsulin);
    }
}