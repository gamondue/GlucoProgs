using GeoTimeZone;
using TimeZoneConverter;

namespace GlucoMan.Maui.Services;

/// <summary>
/// Result of a geographic timezone lookup.
/// </summary>
public class GeoTimeZoneResult
{
    /// <summary>IANA timezone identifier, e.g. "Europe/Rome".</summary>
    public string IanaTimeZoneId { get; init; }

    /// <summary>UTC offset in hours at the moment of the query (includes DST if active). May be fractional.</summary>
    public double UtcOffsetHours { get; init; }

    /// <summary>True when Daylight Saving Time is in effect at the queried coordinates and time.</summary>
    public bool IsDaylightSavingTime { get; init; }

    /// <summary>Full UTC offset as a TimeSpan (useful when the offset is not a whole number of hours).</summary>
    public TimeSpan UtcOffset { get; init; }

    /// <summary>Display string, e.g. "Europe/Rome  UTC+2  (ora legale attiva)".</summary>
    public override string ToString()
    {
        string dst = IsDaylightSavingTime ? " (DST active)" : string.Empty;
        string sign = UtcOffsetHours >= 0 ? "+" : string.Empty;
        int h = (int)Math.Abs(UtcOffsetHours);
        int m = (int)Math.Round((Math.Abs(UtcOffsetHours) - h) * 60);
        string offsetStr = m == 0 ? $"{sign}{h}" : $"{sign}{h}:{m:D2}";
        return $"{IanaTimeZoneId}  UTC{offsetStr}{dst}";
    }
}

/// <summary>
/// Resolves the civil timezone (UTC offset + DST flag) from geographic coordinates.
/// Uses GeoTimeZone to map lat/lon → IANA id, then TimeZoneConverter + TimeZoneInfo for offset/DST.
/// </summary>
public interface IGeoTimeZoneService
{
    /// <summary>
    /// Returns timezone information for the given coordinates at the given instant.
    /// Returns null if the lookup fails.
    /// </summary>
    GeoTimeZoneResult Resolve(double latitude, double longitude, DateTimeOffset? at = null);
}

public class GeoTimeZoneService : IGeoTimeZoneService
{
    public GeoTimeZoneResult Resolve(double latitude, double longitude, DateTimeOffset? at = null)
    {
        try
        {
            // Step 1: lat/lon → IANA timezone id  (e.g. "Europe/Rome")
            string ianaId = TimeZoneLookup.GetTimeZone(latitude, longitude).Result;
            if (string.IsNullOrEmpty(ianaId))
                return null;

            // Step 2: IANA → TimeZoneInfo (.NET)
            TimeZoneInfo tzi = TZConvert.GetTimeZoneInfo(ianaId);

            // Step 3: compute offset and DST flag at the requested instant
            DateTimeOffset instant = at ?? DateTimeOffset.UtcNow;
            TimeSpan offset = tzi.GetUtcOffset(instant.UtcDateTime);
            bool isDst = tzi.IsDaylightSavingTime(instant.UtcDateTime);

            double offsetHours = offset.TotalHours;

            return new GeoTimeZoneResult
            {
                IanaTimeZoneId = ianaId,
                UtcOffset = offset,
                UtcOffsetHours = offsetHours,
                IsDaylightSavingTime = isDst,
            };
        }
        catch (Exception ex)
        {
            gamon.General.LogOfProgram?.Error("GeoTimeZoneService.Resolve", ex);
            return null;
        }
    }
}
