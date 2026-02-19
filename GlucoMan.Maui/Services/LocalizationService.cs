using System.ComponentModel;
using System.Globalization;
using System.Linq;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Services;

/// <summary>
/// Service for managing application localization and culture switching
/// </summary>
public class LocalizationService : INotifyPropertyChanged
{
    private const string CurrentCultureKey = "CurrentCulture";
    private CultureInfo _currentCulture;

    public event PropertyChangedEventHandler? PropertyChanged;

    public LocalizationService()
    {
        // Load saved culture
        var savedCultureName = Preferences.Get(CurrentCultureKey, string.Empty);
        CultureInfo initialCulture;

        if (!string.IsNullOrWhiteSpace(savedCultureName))
        {
            initialCulture = ResolveSupportedCulture(savedCultureName);
        }
        else
        {
            // Use device culture, fallback to English if not supported
            initialCulture = ResolveSupportedCulture(CultureInfo.CurrentUICulture.Name);
        }

        SetCulture(initialCulture);
    }

    /// <summary>
    /// Gets the current culture
    /// </summary>
    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        private set
        {
            if (_currentCulture?.Name != value?.Name)
            {
                _currentCulture = value;
                OnPropertyChanged(nameof(CurrentCulture));
                OnPropertyChanged(nameof(CurrentLanguageDisplayName));
            }
        }
    }

    /// <summary>
    /// Gets the display name of the current language
    /// </summary>
    public string CurrentLanguageDisplayName
    {
        get
        {
            return CurrentCulture?.Name switch
            {
                "it" or "it-IT" => AppStrings.LanguageItalian,
                "en" or "en-US" => AppStrings.LanguageEnglish,
                _ => AppStrings.LanguageEnglish
            };
        }
    }

    /// <summary>
    /// Gets available cultures for the application
    /// </summary>
    public List<CultureInfo> AvailableCultures { get; } = new()
    {
        new CultureInfo("en"),
        new CultureInfo("it")
    };

    /// <summary>
    /// Sets the application culture
    /// </summary>
    /// <param name="cultureName">Culture name (e.g., "en", "it")</param>
    public void SetCulture(string cultureName)
    {
        var culture = ResolveSupportedCulture(cultureName);
        SetCulture(culture);
    }

    /// <summary>
    /// Sets the application culture
    /// </summary>
    /// <param name="culture">CultureInfo object</param>
    public void SetCulture(CultureInfo culture)
    {
        culture ??= new CultureInfo("en");
        culture = ResolveSupportedCulture(culture.Name);

        CurrentCulture = culture;

        // Set thread culture for string formatting (dates, numbers, etc.)
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        // Set culture for resource manager
        AppStrings.Culture = culture;

        // Save preference
        Preferences.Set(CurrentCultureKey, culture.Name);

        // Notify listeners that culture changed
        OnCultureChanged();
    }

    private CultureInfo ResolveSupportedCulture(string cultureName)
    {
        CultureInfo requestedCulture;
        try
        {
            requestedCulture = new CultureInfo(string.IsNullOrWhiteSpace(cultureName) ? "en" : cultureName);
        }
        catch (CultureNotFoundException)
        {
            requestedCulture = new CultureInfo("en");
        }

        // Exact match (en-US, it-IT, etc.)
        var exactMatch = AvailableCultures.FirstOrDefault(c =>
            string.Equals(c.Name, requestedCulture.Name, StringComparison.OrdinalIgnoreCase));
        if (exactMatch != null)
        {
            return exactMatch;
        }

        // Match on two letter ISO code (en, it, etc.)
        var isoMatch = AvailableCultures.FirstOrDefault(c =>
            string.Equals(c.TwoLetterISOLanguageName, requestedCulture.TwoLetterISOLanguageName, StringComparison.OrdinalIgnoreCase));
        if (isoMatch != null)
        {
            return isoMatch;
        }

        // Default to English if unsupported
        return AvailableCultures.First(c => c.TwoLetterISOLanguageName == "en");
    }

    /// <summary>
    /// Event raised when culture changes
    /// </summary>
    public event EventHandler? CultureChanged;

    protected virtual void OnCultureChanged()
    {
        CultureChanged?.Invoke(this, EventArgs.Empty);
        
        // Notify all string properties changed to refresh UI bindings
        OnPropertyChanged(string.Empty);
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Gets a localized string by key (helper method for code-behind)
    /// </summary>
    public string GetString(string key)
    {
        var property = typeof(AppStrings).GetProperty(key, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
        return property?.GetValue(null)?.ToString() ?? key;
    }
}

