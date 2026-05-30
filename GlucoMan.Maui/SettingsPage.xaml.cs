using GlucoMan;
using GlucoMan.Maui.Services;
using GlucoMan.Maui.Resources.Strings;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Globalization;

namespace GlucoMan.Maui;

public partial class SettingsPage : ContentPage, INotifyPropertyChanged
{
    BL_General BlGeneral = new();

    public event PropertyChangedEventHandler PropertyChanged;
    public Parameters Parameters { get; set; }
    public List<InsulinDrug> ShortActingInsulins { get; set; }
    public List<InsulinDrug> LongActingInsulins { get; set; }
    public InsulinDrug SelectedShortActingInsulin { get; set; }
    public InsulinDrug SelectedLongActingInsulin { get; set; }
    BL_BolusesAndInjections bl = new BL_BolusesAndInjections();

    private readonly LocalizationService _localizationService;
    private readonly IGeoTimeZoneService _geoTimeZoneService;

    // DST picker options: populated once, reused at every timezone change
    private List<DstOption> _dstOptions;
    // Country picker options: repopulated at every timezone change
    private List<CountryTimeZoneEntry> _countryOptions;

    public SettingsPage(LocalizationService localizationService, IGeoTimeZoneService geoTimeZoneService = null)
    {
        _localizationService = localizationService;
        _geoTimeZoneService = geoTimeZoneService;
        InitializeComponent();

        // Fill the combos with the insulins
        ShortActingInsulins = bl.GetAllInsulinDrugs(Common.TypeOfInsulinAction.Short);
        LongActingInsulins = bl.GetAllInsulinDrugs(Common.TypeOfInsulinAction.Long);
        
        cmbShortActingInsulin.ItemsSource = ShortActingInsulins;
        cmbLongActingInsulin.ItemsSource = LongActingInsulins;

        Parameters = BlGeneral.GetSettingsPageParameters();

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
        SetupTimeZonePicker();
        SetupDstPicker();
        SetupCountryPicker(Parameters?.Time_CurrentTimeZone
            ?? CountryTimeZoneCatalogue.FindBySystemTimeZone()?.StandardOffsetHours
            ?? TimeZoneInfo.Local.BaseUtcOffset.TotalHours);

        // Subscribe to culture changes to update UI labels
        _localizationService.CultureChanged += OnCultureChanged;
    }
    
    private void SetupTimeZonePicker()
    {
        // All unique offsets present in the catalogue, sorted
        var allOffsets = CountryTimeZoneCatalogue.All
            .Select(c => c.StandardOffsetHours)
            .Distinct()
            .OrderBy(o => o)
            .ToList();

        var timeZones = new List<TimeZoneOption>();
        foreach (var offset in allOffsets)
        {
            timeZones.Add(new TimeZoneOption { Offset = offset, DisplayName = FormatUtcOffset(offset) });
        }
        pickerTimeZone.ItemsSource = timeZones;
        pickerTimeZone.ItemDisplayBinding = new Binding("DisplayName");

        // Default to the device's current standard UTC offset when no saved parameters exist yet.
        var current = Parameters?.Time_CurrentTimeZone
            ?? CountryTimeZoneCatalogue.FindBySystemTimeZone()?.StandardOffsetHours
            ?? TimeZoneInfo.Local.BaseUtcOffset.TotalHours;
        var selected = timeZones.FirstOrDefault(tz => tz.Offset == current);
        if (selected != null)
            pickerTimeZone.SelectedItem = selected;
    }

    /// <summary>Formats a (possibly fractional) UTC offset as "UTC+5:30", "UTC-3:30", etc.</summary>
    private static string FormatUtcOffset(double hours)
    {
        string sign = hours >= 0 ? "+" : "-";
        double abs = Math.Abs(hours);
        int h = (int)abs;
        int m = (int)Math.Round((abs - h) * 60);
        return m == 0 ? $"UTC{sign}{h}" : $"UTC{sign}{h}:{m:D2}";
    }

    private void OnTimeZoneChanged(object sender, EventArgs e)
    {
        if (pickerTimeZone.SelectedItem is TimeZoneOption selectedTz && Parameters != null)
        {
            Parameters.Time_CurrentTimeZone = selectedTz.Offset;
            // Do NOT assign Common.CurrentTimeZone here: it will be computed DST-aware
            // inside BlGeneral.SaveAllParameters (base offset + DST flag).

            // Repopulate Country picker with only countries in this offset;
            // OnCountryChanged will then drive the DST update automatically.
            SetupCountryPicker(selectedTz.Offset);

            BlGeneral.SaveAllParameters(Parameters, SelectedShortActingInsulin, SelectedLongActingInsulin);
        }
    }

    private void SetupCountryPicker(double utcOffsetHours)
    {
        _countryOptions = CountryTimeZoneCatalogue.ForOffset(utcOffsetHours).ToList();
        pickerCountry.ItemsSource = null;
        pickerCountry.ItemsSource = _countryOptions;
        pickerCountry.ItemDisplayBinding = new Binding("Name");

        // Keep the previously saved country if it belongs to the new offset.
        // When no saved country exists, default to Italy if available, otherwise the first entry.
        var savedName = Parameters?.Time_CountryName;
        // When no saved country exists, detect from the device's system timezone.
        var deviceCountry = string.IsNullOrEmpty(savedName)
            ? CountryTimeZoneCatalogue.FindBySystemTimeZone()
            : null;
        var toSelect = (!string.IsNullOrEmpty(savedName)
            ? _countryOptions.FirstOrDefault(c => c.Name == savedName)
            : null)
            ?? (deviceCountry != null && _countryOptions.Any(c => c.Name == deviceCountry.Name)
                ? _countryOptions.First(c => c.Name == deviceCountry.Name)
                : null)
            ?? _countryOptions.FirstOrDefault();

        // Assigning SelectedItem fires OnCountryChanged which resolves and sets DST.
        pickerCountry.SelectedItem = toSelect;
    }

    private void OnCountryChanged(object sender, EventArgs e)
    {
        if (pickerCountry.SelectedItem is not CountryTimeZoneEntry country) return;
        if (Parameters == null) return;

        Parameters.Time_CountryName = country.Name;

        // Compute whether DST is actually active right now for this country's rule.
        bool isDstNow = country.IsDstActiveAt(DateTime.UtcNow);
        Parameters.Time_IsDaylightSavingTime = isDstNow;
        SelectDstInPicker(isDstNow);

        // Update the runtime offset immediately.
        Common.CurrentTimeZone = country.StandardOffsetHours + (isDstNow ? 1 : 0);

        BlGeneral.SaveAllParameters(Parameters, SelectedShortActingInsulin, SelectedLongActingInsulin);
    }

    private void SelectDstInPicker(bool isDst)
    {
        if (_dstOptions == null) return;
        var option = _dstOptions.FirstOrDefault(o => o.Value == isDst);
        if (option != null)
            pickerDst.SelectedItem = option;
    }

    private void SetupDstPicker()
    {
        _dstOptions =
        [
            new DstOption { Value = false, DisplayName = AppStrings.No },
            new DstOption { Value = true,  DisplayName = AppStrings.DaylightSavingTimeActive },
        ];
        pickerDst.ItemsSource = _dstOptions;
        pickerDst.ItemDisplayBinding = new Binding("DisplayName");

        bool currentDst = Parameters?.Time_IsDaylightSavingTime ?? false;
        SelectDstInPicker(currentDst);
    }

    private void OnDstChanged(object sender, EventArgs e)
    {
        if (pickerDst.SelectedItem is DstOption selected && Parameters != null)
        {
            Parameters.Time_IsDaylightSavingTime = selected.Value;
            // Update the runtime offset immediately so all UI time entries reflect the change.
            Common.CurrentTimeZone = (Parameters.Time_CurrentTimeZone ?? 0) + (selected.Value ? 1 : 0);
            BlGeneral.SaveAllParameters(Parameters, SelectedShortActingInsulin, SelectedLongActingInsulin);
        }
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
        if (SelectedLongActingInsulin != null)
            txtInsulinLongDuration.Text = SelectedLongActingInsulin.DurationInHours.ToString();
    }
    
    private void cmbShortActingInsulin_SelectedIndexChanged(object sender, EventArgs e)
    {
        SelectedShortActingInsulin = (InsulinDrug)cmbShortActingInsulin.SelectedItem;
        if (SelectedShortActingInsulin != null)
            txtInsulinShortDuration.Text = SelectedShortActingInsulin.DurationInHours.ToString();
    }
    
    private void Button_Clicked(object sender, EventArgs e)
    {
        if (SelectedShortActingInsulin != null)
            SelectedShortActingInsulin.DurationInHours = double.TryParse(txtInsulinShortDuration.Text,
                out double shortDuration) ? shortDuration : 0;
        if (SelectedLongActingInsulin != null)
            SelectedLongActingInsulin.DurationInHours = double.TryParse(txtInsulinLongDuration.Text,
                out double longDuration) ? longDuration : 0;
        BlGeneral.SaveAllParameters(Parameters, SelectedShortActingInsulin,
            SelectedLongActingInsulin);

        // Keep Common meal time properties in sync with the saved values
        Common.breakfastStartHour = Parameters.Meal_Breakfast_StartTime_Hours;
        Common.breakfastEndHour   = Parameters.Meal_Breakfast_EndTime_Hours;
        Common.lunchStartHour     = Parameters.Meal_Lunch_StartTime_Hours;
        Common.lunchEndHour       = Parameters.Meal_Lunch_EndTime_Hours;
        Common.dinnerStartHour    = Parameters.Meal_Dinner_StartTime_Hours;
        Common.dinnerEndHour      = Parameters.Meal_Dinner_EndTime_Hours;
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

    // Helper class for time zone picker
    public class TimeZoneOption
    {
        public double Offset { get; set; }
        public string DisplayName { get; set; }
    }

    // Helper class for DST picker
    public class DstOption
    {
        public bool Value { get; set; }
        public string DisplayName { get; set; }
    }
}