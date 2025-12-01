using GlucoMan.BusinessLayer;
using GlucoMan.BusinessObjects;
using GlucoMan.Maui.Services;
using GlucoMan.Maui.Resources.Strings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

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
    
    private readonly LocalizationService _localizationService;

    public SettingsPage(LocalizationService localizationService)
    {
        _localizationService = localizationService;
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
        
        // Setup language picker
        SetupLanguagePicker();
        
        // Subscribe to culture changes to update UI labels
        _localizationService.CultureChanged += OnCultureChanged;
    }
    
    private void SetupLanguagePicker()
    {
        // Populate language picker
        var languages = new List<LanguageOption>
        {
            new LanguageOption { CultureCode = "en", DisplayName = "English" },
            new LanguageOption { CultureCode = "it", DisplayName = "Italiano" }
        };
        
        pickerLanguage.ItemsSource = languages;
        pickerLanguage.ItemDisplayBinding = new Binding("DisplayName");
        
        // Set current selection
        var currentCulture = _localizationService.CurrentCulture;
        var currentLanguage = languages.FirstOrDefault(l => 
            l.CultureCode == currentCulture.TwoLetterISOLanguageName);
        
        if (currentLanguage != null)
        {
            pickerLanguage.SelectedItem = currentLanguage;
        }
    }
    
    private void OnLanguageChanged(object sender, EventArgs e)
    {
        if (pickerLanguage.SelectedItem is LanguageOption selectedLanguage)
        {
            _localizationService.SetCulture(selectedLanguage.CultureCode);
            
            // Language changed successfully - no alert needed
        }
    }
    
    private void OnCultureChanged(object sender, EventArgs e)
    {
        // Refresh UI strings when culture changes
        // This will automatically update all bindings using {x:Static}
        OnPropertyChanged(string.Empty);
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
    
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _localizationService.CultureChanged -= OnCultureChanged;
    }
    
    // Helper class for language picker
    public class LanguageOption
    {
        public string CultureCode { get; set; }
        public string DisplayName { get; set; }
    }
}