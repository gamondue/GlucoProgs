using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Services;

/// <summary>
/// Singleton service that detects changes of the UTC offset (time zone or daylight saving time
/// change) comparing the offset detected by the device (OS or GPS) with the configured one,
/// which is the one used for every time that is saved (<see cref="Common.CurrentTimeZone"/>).
/// When they differ the user is asked whether to update the configured country and offset.
/// If the user declines, the configured offset stays the previous one and the question is
/// asked again only when the detected offset changes again.
/// Call <see cref="CheckAndPromptIfChangedAsync"/> just before saving any time-stamped record.
/// </summary>
public class TimeZoneCheckService
{
    // ── singleton ────────────────────────────────────────────────────────────
    private static TimeZoneCheckService? _instance;
    public static TimeZoneCheckService Instance =>
        _instance ??= new TimeZoneCheckService(
            IPlatformApplication.Current?.Services?.GetService<IGeoTimeZoneService>(),
            IPlatformApplication.Current?.Services?.GetService<IGeolocation>());

    // ── fields ────────────────────────────────────────────────────────────────
    private const string DeclinedOffsetKey = "TimeZoneDeclinedUtcOffset";

    /// <summary>
    /// A last known position at most this old is used without waiting for a new one:
    /// changing time zone takes hours of travel, so it still tells the right zone.
    /// </summary>
    private static readonly TimeSpan MaxAgeOfLastKnownLocation = TimeSpan.FromMinutes(10);

    private readonly IGeoTimeZoneService? _geoTzService;
    private readonly IGeolocation? _geolocation;

    /// <summary>
    /// Guards against showing the dialog multiple times in rapid succession
    /// (e.g. two saves fired back-to-back).
    /// </summary>
    private bool _promptInProgress = false;

    // ── constructor ───────────────────────────────────────────────────────────
    public TimeZoneCheckService(IGeoTimeZoneService? geoTzService, IGeolocation? geolocation)
    {
        _geoTzService = geoTzService;
        _geolocation = geolocation;
    }

    // ── public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Sets the configured UTC offset (<see cref="Common.CurrentTimeZone"/>) from the one stored
    /// in the Parameters. If nothing is stored yet (first run) the offset detected by the
    /// device is used. Call once at startup, after the database has been initialized.
    /// </summary>
    public static void InitializeConfiguredOffset()
    {
        Parameters? stored = null;
        try
        {
            stored = DatabaseService.Instance.Database.GetParameters();
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TimeZoneCheckService.InitializeConfiguredOffset", ex);
        }
        ApplyAcceptedTimeZone(stored?.UtcOffset ?? Common.GetDetectedUtcOffset(), stored?.Time_CountryName);
    }

    /// <summary>
    /// Sets the time zone accepted by the user, used for all the times of the program:
    /// the offset in force now and the country whose DST rules give the offset of the
    /// other dates (null when the offset has been set by hand, so it is constant).
    /// </summary>
    public static void ApplyAcceptedTimeZone(double utcOffsetNow, string? countryName)
    {
        CountryTimeZoneEntry? country = string.IsNullOrEmpty(countryName)
            ? null
            : CountryTimeZoneCatalogue.All.FirstOrDefault(c => c.Name == countryName);
        Common.CurrentTimeZone = utcOffsetNow;
        Common.ConfiguredZoneOffsetProvider = country == null ? null : country.GetUtcOffsetAt;
    }

    /// <summary>
    /// Compares the UTC offset detected by the GPS (or by the OS, if the GPS gives no result)
    /// with the configured one and, if it changed,
    /// asks the user whether to update the configured country and offset.
    /// Call from MainPage after the DB parameters have been loaded.
    /// </summary>
    public async Task CheckStartupCountryAsync(Page page)
    {
        if (_promptInProgress) return;
        try
        {
            _promptInProgress = true;
            await CheckDetectedOffsetAsync(page);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TimeZoneCheckService.CheckStartupCountryAsync", ex);
        }
        finally
        {
            _promptInProgress = false;
        }
    }

    /// <summary>
    /// Checks the offset detected by the GPS (or by the OS, if the GPS gives no result)
    /// against the configured one. Must be awaited before the caller saves the record.
    /// </summary>
    /// <param name="page">The <see cref="Page"/> used to display the confirmation dialog.</param>
    public async Task CheckAndPromptIfChangedAsync(Page page)
    {
        if (_promptInProgress) return;

        try
        {
            _promptInProgress = true;
            await CheckDetectedOffsetAsync(page);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TimeZoneCheckService.CheckAndPromptIfChangedAsync", ex);
        }
        finally
        {
            _promptInProgress = false;
        }
    }

    // ── detection ─────────────────────────────────────────────────────────────

    /// <summary>
    /// The GPS offset takes precedence; the OS offset is used only when the GPS gives no
    /// result. Checking both would make them contradict each other when the OS time zone
    /// does not follow the position (e.g. in the emulator), asking again at every save.
    /// </summary>
    private async Task CheckDetectedOffsetAsync(Page page)
    {
        if (!await CheckGpsOffsetAsync(page))
            await CheckOsOffsetAsync(page);
    }

    private static async Task CheckOsOffsetAsync(Page page)
    {
        double detectedOffset = Common.GetDetectedUtcOffset();
        CountryTimeZoneEntry? osCountry = CountryTimeZoneCatalogue.FindBySystemTimeZone();
        bool isDst = osCountry != null && detectedOffset != osCountry.StandardOffsetHours;

        await HandleDetectedOffsetAsync(page, detectedOffset, isDst, osCountry);
    }

    /// <summary>
    /// Checks the offset of the time zone of the GPS position.
    /// Returns false when the GPS gave no result, so that the OS offset can be used instead.
    /// </summary>
    private async Task<bool> CheckGpsOffsetAsync(Page page)
    {
        if (_geoTzService == null || _geolocation == null)
        {
            General.LogOfProgram?.Debug("TimeZoneCheckService.CheckGpsOffsetAsync: geolocation service not available");
            return false;
        }

        Location? location = await GetCurrentLocationAsync();
        if (location == null)
        {
            General.LogOfProgram?.Debug("TimeZoneCheckService.CheckGpsOffsetAsync: no location available");
            return false;
        }

        // Resolve timezone + DST from coordinates
        GeoTimeZoneResult? result = _geoTzService.Resolve(
            location.Latitude, location.Longitude, DateTimeOffset.UtcNow);
        General.LogOfProgram?.Debug($"TimeZoneCheckService.CheckGpsOffsetAsync: " +
            $"{location.Latitude}, {location.Longitude} → {result?.ToString() ?? "unresolved"}");
        if (result == null) return false;

        CountryTimeZoneEntry? country = FindBestCountry(result.UtcOffsetHours, result.IsDaylightSavingTime);
        await HandleDetectedOffsetAsync(page, result.UtcOffsetHours, result.IsDaylightSavingTime, country);
        return true;
    }

    /// <summary>
    /// Asks the user about a detected offset that differs from the configured one.
    /// Accepted: country and offset are updated and saved. Declined: the configured offset is
    /// left unchanged and the declined value is remembered, so it is not asked again until
    /// the detected offset changes.
    /// </summary>
    private static async Task HandleDetectedOffsetAsync(Page page, double detectedOffset,
        bool detectedIsDst, CountryTimeZoneEntry? country)
    {
        double configuredOffset = Common.CurrentTimeZone;
        if (detectedOffset == configuredOffset)
        {
            // Back to the configured offset: a future change must be asked about again
            ClearDeclinedOffset();
            return;
        }
        if (!Common.ShouldAskAboutUtcOffsetChange(detectedOffset, configuredOffset, ReadDeclinedOffset()))
            return;

        string message = string.Format(
            AppStrings.TimeZoneChangedPrompt,
            FormatOffset(configuredOffset),
            GetStoredDst() ? AppStrings.DaylightSavingTimeActive : AppStrings.No,
            FormatOffset(detectedOffset),
            detectedIsDst ? AppStrings.DaylightSavingTimeActive : AppStrings.No);

        bool accepted = await page.DisplayAlert(
            AppStrings.TimeZoneChangedTitle,
            message,
            AppStrings.Yes,
            AppStrings.No);

        if (!accepted)
        {
            // The stored offset stays the previously configured one
            SaveDeclinedOffset(detectedOffset);
            return;
        }

        ClearDeclinedOffset();
        ApplyAcceptedTimeZone(detectedOffset, country?.Name);
        // Time_CurrentTimeZone holds the standard offset, as when a country is chosen in Settings
        UpdateParameters(detectedOffset - (detectedIsDst ? 1 : 0), detectedIsDst, country?.Name);
    }

    // ── helpers ───────────────────────────────────────────────────────────────

    /// <summary>
    /// The last known position if it is recent (see <see cref="MaxAgeOfLastKnownLocation"/>),
    /// so that saving does not wait for the GPS; otherwise a new position, or the old last
    /// known one if no fix arrives in time. High accuracy is requested because on Android a
    /// lower accuracy selects the network provider, which does not follow the GPS position
    /// (e.g. the one set in the emulator's extended controls).
    /// </summary>
    private async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            Location? lastKnown = await _geolocation!.GetLastKnownLocationAsync();
            if (lastKnown != null && DateTimeOffset.UtcNow - lastKnown.Timestamp <= MaxAgeOfLastKnownLocation)
                return lastKnown;

            Location? location = await _geolocation.GetLocationAsync(new GeolocationRequest(
                GeolocationAccuracy.High,
                TimeSpan.FromSeconds(10)));
            return location ?? lastKnown;
        }
        catch (Exception ex)
        {
            // No GPS or no permission: the check is skipped
            General.LogOfProgram?.Error("TimeZoneCheckService.GetCurrentLocationAsync", ex);
            return null;
        }
    }

    private static double? ReadDeclinedOffset()
    {
        try
        {
            double value = Preferences.Default.Get(DeclinedOffsetKey, double.NaN);
            return double.IsNaN(value) ? null : value;
        }
        catch { return null; }
    }

    private static void SaveDeclinedOffset(double offset)
    {
        try { Preferences.Default.Set(DeclinedOffsetKey, offset); }
        catch (Exception ex) { General.LogOfProgram?.Error("TimeZoneCheckService.SaveDeclinedOffset", ex); }
    }

    private static void ClearDeclinedOffset()
    {
        try { Preferences.Default.Remove(DeclinedOffsetKey); }
        catch (Exception ex) { General.LogOfProgram?.Error("TimeZoneCheckService.ClearDeclinedOffset", ex); }
    }

    private static bool GetStoredDst()
    {
        try
        {
            var p = DatabaseService.Instance.Database.GetParameters();
            return p?.Time_IsDaylightSavingTime ?? false;
        }
        catch { return false; }
    }

    private static string FormatOffset(double hours)
    {
        string sign = hours >= 0 ? "+" : "-";
        double abs = Math.Abs(hours);
        int h = (int)abs;
        int m = (int)Math.Round((abs - h) * 60);
        return m == 0 ? $"UTC{sign}{h}" : $"UTC{sign}{h}:{m:D2}";
    }

    private static CountryTimeZoneEntry? FindBestCountry(double offsetHours, bool isDst)
    {
        // The catalogue is indexed by standard offset, while offsetHours includes DST.
        // Prefer a country whose DST rule is currently active (or inactive) as detected.
        var candidates = CountryTimeZoneCatalogue.ForOffset(offsetHours - (isDst ? 1 : 0));
        var utcNow = DateTime.UtcNow;
        var exact = candidates.FirstOrDefault(c => c.IsDstActiveAt(utcNow) == isDst);
        return exact ?? candidates.FirstOrDefault();
    }

    private static void UpdateParameters(double newStandardOffset, bool isDst, string? countryName)
    {
        try
        {
            var bl = new BL_General();
            Parameters? p = bl.GetSettingsPageParameters();
            if (p == null) return;

            p.Time_CurrentTimeZone   = newStandardOffset;
            p.Time_IsDaylightSavingTime = isDst;
            // null when no country matches: the old country would give wrong DST rules
            p.Time_CountryName = countryName;

            bl.SaveAllParameters(p, null, null);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TimeZoneCheckService.UpdateParameters", ex);
        }
    }
}
