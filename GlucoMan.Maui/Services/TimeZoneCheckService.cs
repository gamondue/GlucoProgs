using gamon;
using GlucoMan;
using GlucoMan.Maui.Resources.Strings;

namespace GlucoMan.Maui.Services;

/// <summary>
/// Singleton service that checks whether the device's current GPS-derived timezone
/// differs from the one stored in the app parameters.
/// When a difference is detected the user is asked whether to update the stored timezone.
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
    /// Resolves the current timezone from GPS and, if it differs from the stored one,
    /// asks the user whether to update.  Must be awaited before the caller saves the record.
    /// </summary>
    /// <param name="page">The <see cref="Page"/> used to display the confirmation dialog.</param>
    public async Task CheckAndPromptIfChangedAsync(Page page)
    {
        if (_promptInProgress) return;
        if (_geoTzService == null || _geolocation == null) return;

        try
        {
            _promptInProgress = true;

            // 1. Get current GPS position (best effort, short timeout)
            Location? location = null;
            try
            {
                location = await _geolocation.GetLocationAsync(new GeolocationRequest(
                    GeolocationAccuracy.Low,
                    TimeSpan.FromSeconds(5)));
            }
            catch
            {
                // No GPS available → skip the check silently
                return;
            }

            if (location == null) return;

            // 2. Resolve timezone + DST from coordinates
            GeoTimeZoneResult? result = _geoTzService.Resolve(
                location.Latitude, location.Longitude, DateTimeOffset.UtcNow);

            if (result == null) return;

            // 3. Compare with the stored values
            double storedOffset = Common.CurrentTimeZone;
            bool storedDst   = GetStoredDst();

            bool offsetChanged = result.UtcOffsetHours != storedOffset;
            bool dstChanged    = result.IsDaylightSavingTime != storedDst;

            if (!offsetChanged && !dstChanged) return;

            // 4. Build a readable description of what changed
            string newOffsetStr = FormatOffset(result.UtcOffsetHours);
            string oldOffsetStr = FormatOffset(storedOffset);
            string newDstStr    = result.IsDaylightSavingTime
                ? AppStrings.DaylightSavingTimeActive
                : AppStrings.No;
            string oldDstStr    = storedDst
                ? AppStrings.DaylightSavingTimeActive
                : AppStrings.No;

            string message = string.Format(
                AppStrings.TimeZoneChangedPrompt,
                oldOffsetStr, oldDstStr,
                newOffsetStr, newDstStr);

            // 5. Ask the user
            bool accepted = await page.DisplayAlert(
                AppStrings.TimeZoneChangedTitle,
                message,
                AppStrings.Yes,
                AppStrings.No);

            if (!accepted) return;

            // 6. Update in-memory state
            Common.CurrentTimeZone = result.UtcOffsetHours;

            // 7. Find the best matching country for this offset
            string? bestCountry = FindBestCountry(result.UtcOffsetHours, result.IsDaylightSavingTime);

            // 8. Persist to Parameters
            UpdateParameters(result.UtcOffsetHours, result.IsDaylightSavingTime, bestCountry);
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

    // ── helpers ───────────────────────────────────────────────────────────────

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

    private static string? FindBestCountry(double offsetHours, bool isDst)
    {
        // Prefer a country whose DST rule is currently active (or inactive) as detected by GPS.
        var candidates = CountryTimeZoneCatalogue.ForOffset(offsetHours);
        var utcNow = DateTime.UtcNow;
        var exact = candidates.FirstOrDefault(c => c.IsDstActiveAt(utcNow) == isDst);
        return (exact ?? candidates.FirstOrDefault())?.Name;
    }

    private static void UpdateParameters(double newOffset, bool isDst, string? countryName)
    {
        try
        {
            var bl = new BL_General();
            Parameters? p = bl.GetSettingsPageParameters();
            if (p == null) return;

            p.Time_CurrentTimeZone   = newOffset;
            p.Time_IsDaylightSavingTime = isDst;
            if (countryName != null)
                p.Time_CountryName = countryName;

            bl.SaveAllParameters(p, null, null);
        }
        catch (Exception ex)
        {
            General.LogOfProgram?.Error("TimeZoneCheckService.UpdateParameters", ex);
        }
    }
}
