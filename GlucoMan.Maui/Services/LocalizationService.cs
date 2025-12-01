using System.ComponentModel;
using System.Globalization;
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
        // Load saved culture or use system default
        var savedCulture = Preferences.Get(CurrentCultureKey, CultureInfo.CurrentUICulture.Name);
        SetCulture(savedCulture);
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
        try
        {
            var culture = new CultureInfo(cultureName);
            SetCulture(culture);
        }
        catch (CultureNotFoundException)
        {
            // Fallback to English if culture not found
            SetCulture(new CultureInfo("en"));
        }
    }

    /// <summary>
    /// Sets the application culture
    /// </summary>
    /// <param name="culture">CultureInfo object</param>
    public void SetCulture(CultureInfo culture)
    {
        if (culture == null)
        {
            culture = new CultureInfo("en");
        }

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
