namespace GlucoMan.Maui.Services;

/// <summary>
/// Identifies which regional DST rule a country follows.
/// None  – the country does not observe DST.
/// USA   – USA/Canada rule: +1h from 2nd Sunday of March ? 1st Sunday of November (at 02:00 local).
/// EU    – European rule:   +1h from last Sunday of March ? last Sunday of October (at 01:00 UTC / 02:00 CET).
/// AUS   – Australia rule:  +1h from 1st Sunday of October ? 1st Sunday of April (southern-hemisphere).
/// NZL   – New Zealand rule:+1h from last Sunday of September ? 1st Sunday of April.
/// </summary>
public enum DstRule
{
    None,
    USA,
    EU,
    AUS,
    NZL,
}

/// <summary>
/// Computes whether DST is currently in effect for a given <see cref="DstRule"/>
/// and a UTC instant, applying the transition at 03:00 local standard time on the
/// relevant Sunday (as stated in the requirements).
/// </summary>
public static class DstHelper
{
    /// <summary>
    /// Returns true when DST is active for <paramref name="rule"/> at the given
    /// <paramref name="utcNow"/> UTC moment (and the country's standard offset).
    /// </summary>
    public static bool IsDstActive(DstRule rule, DateTime utcNow, double standardOffsetHours)
    {
        if (rule == DstRule.None) return false;

        // Work in local standard time (no DST adjustment yet)
        DateTime lst = utcNow.AddHours(standardOffsetHours);

        return rule switch
        {
            DstRule.USA => IsBetween_USA(lst),
            DstRule.EU  => IsBetween_EU(lst),
            DstRule.AUS => IsBetween_AUS(lst),
            DstRule.NZL => IsBetween_NZL(lst),
            _           => false,
        };
    }

    // ?? USA/Canada ????????????????????????????????????????????????????????????
    // DST start: 2nd Sunday of March  03:00 LST
    // DST end  : 1st Sunday of November 03:00 LST
    private static bool IsBetween_USA(DateTime lst)
    {
        int year = lst.Year;
        DateTime start = NthSundayOf(year, 3, 2).AddHours(3);  // 2nd Sun March 03:00
        DateTime end   = NthSundayOf(year, 11, 1).AddHours(3); // 1st Sun November 03:00
        return lst >= start && lst < end;
    }

    // ?? European Union + UK ???????????????????????????????????????????????????
    // DST start: last Sunday of March  03:00 LST (= 01:00 UTC for CET, but we use LST)
    // DST end  : last Sunday of October 03:00 LST
    private static bool IsBetween_EU(DateTime lst)
    {
        int year = lst.Year;
        DateTime start = LastSundayOf(year, 3).AddHours(3);  // last Sun March 03:00
        DateTime end   = LastSundayOf(year, 10).AddHours(3); // last Sun October 03:00
        return lst >= start && lst < end;
    }

    // ?? Australia ?????????????????????????????????????????????????????????????
    // DST start: 1st Sunday of October  03:00 LST
    // DST end  : 1st Sunday of April    03:00 LST
    // Because Australia DST spans the year-boundary the logic is inverted.
    private static bool IsBetween_AUS(DateTime lst)
    {
        int year = lst.Year;
        DateTime start = NthSundayOf(year, 10, 1).AddHours(3); // 1st Sun Oct 03:00
        DateTime end   = NthSundayOf(year, 4,  1).AddHours(3); // 1st Sun Apr 03:00

        // Southern hemisphere: active from Oct ? Apr (crosses year boundary)
        // lst is in DST if lst >= start(this year) OR lst < end(this year)
        return lst >= start || lst < end;
    }

    // ?? New Zealand ???????????????????????????????????????????????????????????
    // DST start: last Sunday of September 03:00 LST
    // DST end  : 1st Sunday of April      03:00 LST
    private static bool IsBetween_NZL(DateTime lst)
    {
        int year = lst.Year;
        DateTime start = LastSundayOf(year, 9).AddHours(3);    // last Sun Sep 03:00
        DateTime end   = NthSundayOf(year, 4, 1).AddHours(3);  // 1st Sun Apr 03:00

        // Crosses year boundary, same logic as AUS
        return lst >= start || lst < end;
    }

    // ?? helpers ???????????????????????????????????????????????????????????????

    /// <summary>Returns the date of the Nth Sunday (n=1 first, 2 second, …) in the given month/year.</summary>
    public static DateTime NthSundayOf(int year, int month, int n)
    {
        DateTime first = new DateTime(year, month, 1);
        int daysToSunday = ((int)DayOfWeek.Sunday - (int)first.DayOfWeek + 7) % 7;
        return first.AddDays(daysToSunday + (n - 1) * 7);
    }

    /// <summary>Returns the date of the last Sunday in the given month/year.</summary>
    public static DateTime LastSundayOf(int year, int month)
    {
        DateTime last = new DateTime(year, month, DateTime.DaysInMonth(year, month));
        int daysBack = ((int)last.DayOfWeek - (int)DayOfWeek.Sunday + 7) % 7;
        return last.AddDays(-daysBack);
    }
}

/// <summary>
/// A country entry used to populate the Country picker in SettingsPage.
/// </summary>
public class CountryTimeZoneEntry
{
    /// <summary>English identifier used for storage and internal comparisons.</summary>
    public string Name { get; init; }

    /// <summary>Display name in the country's own official language.</summary>
    public string NativeName { get; init; }

    /// <summary>UTC standard offset in hours (without DST). May be fractional, e.g. 5.5 for India.</summary>
    public double StandardOffsetHours { get; init; }

    /// <summary>Representative latitude of the country capital / main city.</summary>
    public double Latitude { get; init; }

    /// <summary>Representative longitude of the country capital / main city.</summary>
    public double Longitude { get; init; }

    /// <summary>
    /// The regional DST rule that applies to this country/territory.
    /// <see cref="DstRule.None"/> means the country does not observe DST.
    /// </summary>
    public DstRule DstRule { get; init; }

    /// <summary>
    /// True when this country/territory observes Daylight Saving Time at some point in the year.
    /// Convenience wrapper over <see cref="DstRule"/>.
    /// </summary>
    public bool UsesDaylightSaving => DstRule != DstRule.None;

    /// <summary>
    /// Returns whether DST is currently active for this country at the given UTC instant.
    /// </summary>
    public bool IsDstActiveAt(DateTime utcNow)
        => DstHelper.IsDstActive(DstRule, utcNow, StandardOffsetHours);
}

/// <summary>
/// Static catalogue of countries grouped by standard UTC offset.
/// Coordinates are those of the capital or most populated city.
/// </summary>
public static class CountryTimeZoneCatalogue
{
    public static readonly IReadOnlyList<CountryTimeZoneEntry> All = new List<CountryTimeZoneEntry>
    {
        // UTC -12
        new() { Name = "Baker Island (US)",            NativeName = "Baker Island (US)",            StandardOffsetHours = -12, Latitude =   0.19, Longitude = -176.48, DstRule = DstRule.None },
        new() { Name = "Howland Island (US)",          NativeName = "Howland Island (US)",          StandardOffsetHours = -12, Latitude =   0.81, Longitude = -176.63, DstRule = DstRule.None },

        // UTC -11
        new() { Name = "American Samoa",               NativeName = "Amerika Sāmoa",                StandardOffsetHours = -11, Latitude = -13.83, Longitude = -171.77, DstRule = DstRule.None },
        new() { Name = "Niue",                         NativeName = "Niuē",                         StandardOffsetHours = -11, Latitude = -19.05, Longitude = -169.92, DstRule = DstRule.None },

        // UTC -10
        new() { Name = "Hawaii (US)",                  NativeName = "Hawaiʻi (US)",                 StandardOffsetHours = -10, Latitude =  21.31, Longitude = -157.86, DstRule = DstRule.None },
        new() { Name = "Cook Islands",                 NativeName = "Kūki 'Āirani",                 StandardOffsetHours = -10, Latitude = -21.24, Longitude = -159.78, DstRule = DstRule.None },
        new() { Name = "French Polynesia (Tahiti)",    NativeName = "Polynésie française (Tahiti)", StandardOffsetHours = -10, Latitude = -17.53, Longitude = -149.57, DstRule = DstRule.None },

        // UTC -9
        new() { Name = "Alaska (US)",                  NativeName = "Alaska (US)",                  StandardOffsetHours =  -9, Latitude =  61.22, Longitude = -149.90, DstRule = DstRule.USA  },
        new() { Name = "Gambier Islands",              NativeName = "Îles Gambier",                 StandardOffsetHours =  -9, Latitude = -23.12, Longitude = -134.97, DstRule = DstRule.None },

        // UTC -8
        new() { Name = "California (US)",              NativeName = "California (US)",              StandardOffsetHours =  -8, Latitude =  34.05, Longitude = -118.24, DstRule = DstRule.USA  },
        new() { Name = "British Columbia (Canada)",    NativeName = "British Columbia (Canada)",    StandardOffsetHours =  -8, Latitude =  49.28, Longitude = -123.12, DstRule = DstRule.USA  },
        new() { Name = "Baja California (Mexico)",     NativeName = "Baja California (México)",     StandardOffsetHours =  -8, Latitude =  32.53, Longitude = -117.04, DstRule = DstRule.USA  },

        // UTC -7
        new() { Name = "Arizona (US)",                 NativeName = "Arizona (US)",                 StandardOffsetHours =  -7, Latitude =  33.45, Longitude = -112.07, DstRule = DstRule.None },
        new() { Name = "Colorado (US)",                NativeName = "Colorado (US)",                StandardOffsetHours =  -7, Latitude =  39.74, Longitude = -104.98, DstRule = DstRule.USA  },
        new() { Name = "Alberta (Canada)",             NativeName = "Alberta (Canada)",             StandardOffsetHours =  -7, Latitude =  51.05, Longitude = -114.07, DstRule = DstRule.USA  },
        new() { Name = "Chihuahua (Mexico)",           NativeName = "Chihuahua (México)",           StandardOffsetHours =  -7, Latitude =  28.64, Longitude = -106.09, DstRule = DstRule.USA  },

        // UTC -6
        new() { Name = "Texas (US)",                   NativeName = "Texas (US)",                   StandardOffsetHours =  -6, Latitude =  30.27, Longitude =  -97.74, DstRule = DstRule.USA  },
        new() { Name = "Illinois (US)",                NativeName = "Illinois (US)",                StandardOffsetHours =  -6, Latitude =  41.85, Longitude =  -87.65, DstRule = DstRule.USA  },
        new() { Name = "Mexico City (Mexico)",         NativeName = "Ciudad de México (México)",    StandardOffsetHours =  -6, Latitude =  19.43, Longitude =  -99.13, DstRule = DstRule.USA  },
        new() { Name = "Guatemala",                    NativeName = "Guatemala",                    StandardOffsetHours =  -6, Latitude =  14.64, Longitude =  -90.51, DstRule = DstRule.None },
        new() { Name = "El Salvador",                  NativeName = "El Salvador",                  StandardOffsetHours =  -6, Latitude =  13.69, Longitude =  -89.19, DstRule = DstRule.None },
        new() { Name = "Honduras",                     NativeName = "Honduras",                     StandardOffsetHours =  -6, Latitude =  14.10, Longitude =  -87.21, DstRule = DstRule.None },
        new() { Name = "Nicaragua",                    NativeName = "Nicaragua",                    StandardOffsetHours =  -6, Latitude =  12.13, Longitude =  -86.28, DstRule = DstRule.None },
        new() { Name = "Costa Rica",                   NativeName = "Costa Rica",                   StandardOffsetHours =  -6, Latitude =   9.93, Longitude =  -84.08, DstRule = DstRule.None },

        // UTC -5
        new() { Name = "New York (US)",                NativeName = "New York (US)",                StandardOffsetHours =  -5, Latitude =  40.71, Longitude =  -74.01, DstRule = DstRule.USA  },
        new() { Name = "Florida (US)",                 NativeName = "Florida (US)",                 StandardOffsetHours =  -5, Latitude =  25.77, Longitude =  -80.19, DstRule = DstRule.USA  },
        new() { Name = "Ontario (Canada)",             NativeName = "Ontario (Canada)",             StandardOffsetHours =  -5, Latitude =  43.70, Longitude =  -79.42, DstRule = DstRule.USA  },
        new() { Name = "Peru",                         NativeName = "Perú",                         StandardOffsetHours =  -5, Latitude = -12.05, Longitude =  -77.03, DstRule = DstRule.None },
        new() { Name = "Colombia",                     NativeName = "Colombia",                     StandardOffsetHours =  -5, Latitude =   4.71, Longitude =  -74.07, DstRule = DstRule.None },
        new() { Name = "Ecuador",                      NativeName = "Ecuador",                      StandardOffsetHours =  -5, Latitude =  -0.23, Longitude =  -78.52, DstRule = DstRule.None },
        new() { Name = "Panama",                       NativeName = "Panamá",                       StandardOffsetHours =  -5, Latitude =   8.99, Longitude =  -79.52, DstRule = DstRule.None },
        new() { Name = "Cuba",                         NativeName = "Cuba",                         StandardOffsetHours =  -5, Latitude =  23.13, Longitude =  -82.38, DstRule = DstRule.USA  },
        new() { Name = "Jamaica",                      NativeName = "Jamaica",                      StandardOffsetHours =  -5, Latitude =  17.99, Longitude =  -76.79, DstRule = DstRule.None },

        // UTC -4
        new() { Name = "Puerto Rico (US)",             NativeName = "Puerto Rico (US)",             StandardOffsetHours =  -4, Latitude =  18.47, Longitude =  -66.11, DstRule = DstRule.None },
        new() { Name = "Venezuela",                    NativeName = "Venezuela",                    StandardOffsetHours =  -4, Latitude =  10.49, Longitude =  -66.88, DstRule = DstRule.None },
        new() { Name = "Bolivia",                      NativeName = "Bolivia",                      StandardOffsetHours =  -4, Latitude = -16.50, Longitude =  -68.15, DstRule = DstRule.None },
        new() { Name = "Chile",                        NativeName = "Chile",                        StandardOffsetHours =  -4, Latitude = -33.46, Longitude =  -70.65, DstRule = DstRule.AUS  },
        new() { Name = "Paraguay",                     NativeName = "Paraguay",                     StandardOffsetHours =  -4, Latitude = -25.29, Longitude =  -57.65, DstRule = DstRule.AUS  },
        new() { Name = "Nova Scotia (Canada)",         NativeName = "Nova Scotia (Canada)",         StandardOffsetHours =  -4, Latitude =  44.65, Longitude =  -63.58, DstRule = DstRule.USA  },

        // UTC -3:30
        new() { Name = "Newfoundland (Canada)",        NativeName = "Newfoundland (Canada)",        StandardOffsetHours = -3.5, Latitude =  47.56, Longitude =  -52.71, DstRule = DstRule.USA  },

        // UTC -3
        new() { Name = "Brazil (Brasília)",            NativeName = "Brasil (Brasília)",            StandardOffsetHours =  -3, Latitude = -15.78, Longitude =  -47.93, DstRule = DstRule.None },
        new() { Name = "Argentina",                    NativeName = "Argentina",                    StandardOffsetHours =  -3, Latitude = -34.61, Longitude =  -58.37, DstRule = DstRule.None },
        new() { Name = "Uruguay",                      NativeName = "Uruguay",                      StandardOffsetHours =  -3, Latitude = -34.90, Longitude =  -56.19, DstRule = DstRule.None },
        new() { Name = "Greenland",                    NativeName = "Kalaallit Nunaat",             StandardOffsetHours =  -3, Latitude =  64.18, Longitude =  -51.74, DstRule = DstRule.EU   },
        new() { Name = "Suriname",                     NativeName = "Suriname",                     StandardOffsetHours =  -3, Latitude =   5.87, Longitude =  -55.17, DstRule = DstRule.None },

        // UTC -2
        new() { Name = "Fernando de Noronha (Brazil)", NativeName = "Fernando de Noronha (Brasil)", StandardOffsetHours =  -2, Latitude =  -3.86, Longitude =  -32.43, DstRule = DstRule.None },
        new() { Name = "South Georgia",                NativeName = "South Georgia",                StandardOffsetHours =  -2, Latitude = -54.28, Longitude =  -36.51, DstRule = DstRule.None },

        // UTC -1
        new() { Name = "Azores (Portugal)",            NativeName = "Açores (Portugal)",            StandardOffsetHours =  -1, Latitude =  37.74, Longitude =  -25.67, DstRule = DstRule.EU   },
        new() { Name = "Cape Verde",                   NativeName = "Cabo Verde",                   StandardOffsetHours =  -1, Latitude =  14.93, Longitude =  -23.51, DstRule = DstRule.None },

        // UTC 0
        new() { Name = "United Kingdom",               NativeName = "United Kingdom",               StandardOffsetHours =   0, Latitude =  51.51, Longitude =   -0.13, DstRule = DstRule.EU   },
        new() { Name = "Ireland",                      NativeName = "Éire",                         StandardOffsetHours =   0, Latitude =  53.33, Longitude =   -6.25, DstRule = DstRule.EU   },
        new() { Name = "Portugal",                     NativeName = "Portugal",                     StandardOffsetHours =   0, Latitude =  38.72, Longitude =   -9.14, DstRule = DstRule.EU   },
        new() { Name = "Iceland",                      NativeName = "Ísland",                       StandardOffsetHours =   0, Latitude =  64.13, Longitude =  -21.82, DstRule = DstRule.None },
        new() { Name = "Ghana",                        NativeName = "Ghana",                        StandardOffsetHours =   0, Latitude =   5.55, Longitude =   -0.20, DstRule = DstRule.None },
        new() { Name = "Senegal",                      NativeName = "Sénégal",                      StandardOffsetHours =   0, Latitude =  14.69, Longitude =  -17.44, DstRule = DstRule.None },
        new() { Name = "Mali",                         NativeName = "Mali",                         StandardOffsetHours =   0, Latitude =  12.65, Longitude =   -8.00, DstRule = DstRule.None },
        new() { Name = "Morocco",                      NativeName = "المغرب",                       StandardOffsetHours =   0, Latitude =  33.99, Longitude =   -6.85, DstRule = DstRule.EU   },

        // UTC +1
        new() { Name = "Italy",                        NativeName = "Italia",                       StandardOffsetHours =  +1, Latitude =  41.90, Longitude =   12.49, DstRule = DstRule.EU   },
        new() { Name = "Germany",                      NativeName = "Deutschland",                  StandardOffsetHours =  +1, Latitude =  52.52, Longitude =   13.40, DstRule = DstRule.EU   },
        new() { Name = "France",                       NativeName = "France",                       StandardOffsetHours =  +1, Latitude =  48.85, Longitude =    2.35, DstRule = DstRule.EU   },
        new() { Name = "Spain",                        NativeName = "España",                       StandardOffsetHours =  +1, Latitude =  40.42, Longitude =   -3.70, DstRule = DstRule.EU   },
        new() { Name = "Netherlands",                  NativeName = "Nederland",                    StandardOffsetHours =  +1, Latitude =  52.37, Longitude =    4.90, DstRule = DstRule.EU   },
        new() { Name = "Belgium",                      NativeName = "België / Belgique",            StandardOffsetHours =  +1, Latitude =  50.85, Longitude =    4.35, DstRule = DstRule.EU   },
        new() { Name = "Switzerland",                  NativeName = "Schweiz / Suisse / Svizzera",  StandardOffsetHours =  +1, Latitude =  46.95, Longitude =    7.45, DstRule = DstRule.EU   },
        new() { Name = "Austria",                      NativeName = "Österreich",                   StandardOffsetHours =  +1, Latitude =  48.21, Longitude =   16.37, DstRule = DstRule.EU   },
        new() { Name = "Poland",                       NativeName = "Polska",                       StandardOffsetHours =  +1, Latitude =  52.23, Longitude =   21.01, DstRule = DstRule.EU   },
        new() { Name = "Sweden",                       NativeName = "Sverige",                      StandardOffsetHours =  +1, Latitude =  59.33, Longitude =   18.07, DstRule = DstRule.EU   },
        new() { Name = "Norway",                       NativeName = "Norge",                        StandardOffsetHours =  +1, Latitude =  59.91, Longitude =   10.75, DstRule = DstRule.EU   },
        new() { Name = "Denmark",                      NativeName = "Danmark",                      StandardOffsetHours =  +1, Latitude =  55.68, Longitude =   12.57, DstRule = DstRule.EU   },
        new() { Name = "Nigeria",                      NativeName = "Nigeria",                      StandardOffsetHours =  +1, Latitude =   6.45, Longitude =    3.40, DstRule = DstRule.None },
        new() { Name = "Algeria",                      NativeName = "الجزائر",                      StandardOffsetHours =  +1, Latitude =  36.74, Longitude =    3.06, DstRule = DstRule.None },
        new() { Name = "Tunisia",                      NativeName = "تونس",                         StandardOffsetHours =  +1, Latitude =  36.82, Longitude =   10.17, DstRule = DstRule.None },
        new() { Name = "Angola",                       NativeName = "Angola",                       StandardOffsetHours =  +1, Latitude =  -8.84, Longitude =   13.23, DstRule = DstRule.None },

        // UTC +2
        new() { Name = "Greece",                       NativeName = "Ελλάδα",                       StandardOffsetHours =  +2, Latitude =  37.98, Longitude =   23.73, DstRule = DstRule.EU   },
        new() { Name = "Romania",                      NativeName = "România",                      StandardOffsetHours =  +2, Latitude =  44.44, Longitude =   26.10, DstRule = DstRule.EU   },
        new() { Name = "Bulgaria",                     NativeName = "България",                     StandardOffsetHours =  +2, Latitude =  42.70, Longitude =   23.32, DstRule = DstRule.EU   },
        new() { Name = "Finland",                      NativeName = "Suomi",                        StandardOffsetHours =  +2, Latitude =  60.17, Longitude =   24.94, DstRule = DstRule.EU   },
        new() { Name = "Ukraine",                      NativeName = "Україна",                      StandardOffsetHours =  +2, Latitude =  50.45, Longitude =   30.52, DstRule = DstRule.EU   },
        new() { Name = "Israel",                       NativeName = "ישראל",                        StandardOffsetHours =  +2, Latitude =  31.77, Longitude =   35.22, DstRule = DstRule.EU   },
        new() { Name = "Egypt",                        NativeName = "مصر",                          StandardOffsetHours =  +2, Latitude =  30.06, Longitude =   31.25, DstRule = DstRule.None },
        new() { Name = "South Africa",                 NativeName = "South Africa",                 StandardOffsetHours =  +2, Latitude = -25.75, Longitude =   28.19, DstRule = DstRule.None },
        new() { Name = "Zimbabwe",                     NativeName = "Zimbabwe",                     StandardOffsetHours =  +2, Latitude = -17.83, Longitude =   31.05, DstRule = DstRule.None },
        new() { Name = "Libya",                        NativeName = "ليبيا",                        StandardOffsetHours =  +2, Latitude =  32.90, Longitude =   13.18, DstRule = DstRule.None },
        new() { Name = "Cyprus",                       NativeName = "Κύπρος",                       StandardOffsetHours =  +2, Latitude =  35.17, Longitude =   33.37, DstRule = DstRule.EU   },

        // UTC +3
        new() { Name = "Russia (Moscow)",              NativeName = "Россия (Москва)",              StandardOffsetHours =  +3, Latitude =  55.75, Longitude =   37.62, DstRule = DstRule.None },
        new() { Name = "Turkey",                       NativeName = "Türkiye",                      StandardOffsetHours =  +3, Latitude =  39.93, Longitude =   32.86, DstRule = DstRule.None },
        new() { Name = "Saudi Arabia",                 NativeName = "السعودية",                     StandardOffsetHours =  +3, Latitude =  24.69, Longitude =   46.72, DstRule = DstRule.None },
        new() { Name = "Iraq",                         NativeName = "العراق",                       StandardOffsetHours =  +3, Latitude =  33.34, Longitude =   44.40, DstRule = DstRule.None },
        new() { Name = "Kenya",                        NativeName = "Kenya",                        StandardOffsetHours =  +3, Latitude =  -1.29, Longitude =   36.82, DstRule = DstRule.None },
        new() { Name = "Ethiopia",                     NativeName = "ኢትዮጵያ",                        StandardOffsetHours =  +3, Latitude =   9.02, Longitude =   38.75, DstRule = DstRule.None },
        new() { Name = "Qatar",                        NativeName = "قطر",                          StandardOffsetHours =  +3, Latitude =  25.29, Longitude =   51.53, DstRule = DstRule.None },
        new() { Name = "Kuwait",                       NativeName = "الكويت",                       StandardOffsetHours =  +3, Latitude =  29.37, Longitude =   47.98, DstRule = DstRule.None },

        // UTC +3:30
        new() { Name = "Iran",                         NativeName = "ایران",                        StandardOffsetHours =  3.5, Latitude =  35.69, Longitude =   51.42, DstRule = DstRule.EU   },

        // UTC +4
        new() { Name = "UAE",                          NativeName = "الإمارات",                     StandardOffsetHours =  +4, Latitude =  25.20, Longitude =   55.27, DstRule = DstRule.None },
        new() { Name = "Oman",                         NativeName = "عُمان",                        StandardOffsetHours =  +4, Latitude =  23.61, Longitude =   58.59, DstRule = DstRule.None },
        new() { Name = "Azerbaijan",                   NativeName = "Azərbaycan",                   StandardOffsetHours =  +4, Latitude =  40.41, Longitude =   49.87, DstRule = DstRule.None },
        new() { Name = "Georgia",                      NativeName = "საქართველო",                   StandardOffsetHours =  +4, Latitude =  41.69, Longitude =   44.83, DstRule = DstRule.None },
        new() { Name = "Armenia",                      NativeName = "Հայաստան",                     StandardOffsetHours =  +4, Latitude =  40.18, Longitude =   44.51, DstRule = DstRule.None },
        new() { Name = "Mauritius",                    NativeName = "Maurice",                      StandardOffsetHours =  +4, Latitude = -20.16, Longitude =   57.49, DstRule = DstRule.None },

        // UTC +4:30
        new() { Name = "Afghanistan",                  NativeName = "افغانستان",                    StandardOffsetHours =  4.5, Latitude =  34.53, Longitude =   69.17, DstRule = DstRule.None },

        // UTC +5
        new() { Name = "Pakistan",                     NativeName = "پاکستان",                      StandardOffsetHours =  +5, Latitude =  33.72, Longitude =   73.04, DstRule = DstRule.None },
        new() { Name = "Uzbekistan",                   NativeName = "O'zbekiston",                  StandardOffsetHours =  +5, Latitude =  41.30, Longitude =   69.24, DstRule = DstRule.None },
        new() { Name = "Kazakhstan (West)",            NativeName = "Қазақстан (Батыс)",            StandardOffsetHours =  +5, Latitude =  51.18, Longitude =   71.45, DstRule = DstRule.None },
        new() { Name = "Tajikistan",                   NativeName = "Тоҷикистон",                   StandardOffsetHours =  +5, Latitude =  38.56, Longitude =   68.77, DstRule = DstRule.None },
        new() { Name = "Turkmenistan",                 NativeName = "Türkmenistan",                 StandardOffsetHours =  +5, Latitude =  37.95, Longitude =   58.38, DstRule = DstRule.None },
        new() { Name = "Maldives",                     NativeName = "ދިވެހިރާއްޖެ",                  StandardOffsetHours =  +5, Latitude =   4.17, Longitude =   73.51, DstRule = DstRule.None },

        // UTC +5:30
        new() { Name = "India",                        NativeName = "भारत",                         StandardOffsetHours =  5.5, Latitude =  28.64, Longitude =   77.22, DstRule = DstRule.None },
        new() { Name = "Sri Lanka",                    NativeName = "ශ්‍රී ලංකාව",                  StandardOffsetHours =  5.5, Latitude =   6.93, Longitude =   79.86, DstRule = DstRule.None },

        // UTC +5:45
        new() { Name = "Nepal",                        NativeName = "नेपाल",                        StandardOffsetHours =  5.75, Latitude =  27.71, Longitude =   85.32, DstRule = DstRule.None },

        // UTC +6
        new() { Name = "Bangladesh",                   NativeName = "বাংলাদেশ",                     StandardOffsetHours =  +6, Latitude =  23.73, Longitude =   90.39, DstRule = DstRule.None },
        new() { Name = "Kazakhstan (East)",            NativeName = "Қазақстан (Шығыс)",            StandardOffsetHours =  +6, Latitude =  43.25, Longitude =   76.95, DstRule = DstRule.None },
        new() { Name = "Kyrgyzstan",                   NativeName = "Кыргызстан",                   StandardOffsetHours =  +6, Latitude =  42.87, Longitude =   74.59, DstRule = DstRule.None },
        new() { Name = "Bhutan",                       NativeName = "འབྲུག",                         StandardOffsetHours =  +6, Latitude =  27.47, Longitude =   89.64, DstRule = DstRule.None },

        // UTC +6:30
        new() { Name = "Myanmar",                      NativeName = "မြန်မာ",                       StandardOffsetHours =  6.5, Latitude =  16.87, Longitude =   96.16, DstRule = DstRule.None },
        new() { Name = "Cocos Islands",                NativeName = "Cocos Islands",                StandardOffsetHours =  6.5, Latitude = -12.16, Longitude =   96.83, DstRule = DstRule.None },

        // UTC +7
        new() { Name = "Thailand",                     NativeName = "ประเทศไทย",                    StandardOffsetHours =  +7, Latitude =  13.75, Longitude =  100.52, DstRule = DstRule.None },
        new() { Name = "Vietnam",                      NativeName = "Việt Nam",                     StandardOffsetHours =  +7, Latitude =  21.03, Longitude =  105.85, DstRule = DstRule.None },
        new() { Name = "Indonesia (Jakarta)",          NativeName = "Indonesia (Jakarta)",          StandardOffsetHours =  +7, Latitude =  -6.21, Longitude =  106.85, DstRule = DstRule.None },
        new() { Name = "Cambodia",                     NativeName = "កម្ពុជា",                       StandardOffsetHours =  +7, Latitude =  11.56, Longitude =  104.92, DstRule = DstRule.None },
        new() { Name = "Laos",                         NativeName = "ລາວ",                          StandardOffsetHours =  +7, Latitude =  17.97, Longitude =  102.63, DstRule = DstRule.None },
        new() { Name = "Russia (Krasnoyarsk)",         NativeName = "Россия (Красноярск)",          StandardOffsetHours =  +7, Latitude =  56.01, Longitude =   92.85, DstRule = DstRule.None },

        // UTC +8
        new() { Name = "China",                        NativeName = "中国",                          StandardOffsetHours =  +8, Latitude =  39.91, Longitude =  116.39, DstRule = DstRule.None },
        new() { Name = "Philippines",                  NativeName = "Pilipinas",                    StandardOffsetHours =  +8, Latitude =  14.60, Longitude =  120.98, DstRule = DstRule.None },
        new() { Name = "Malaysia",                     NativeName = "Malaysia",                     StandardOffsetHours =  +8, Latitude =   3.15, Longitude =  101.69, DstRule = DstRule.None },
        new() { Name = "Singapore",                    NativeName = "Singapore",                    StandardOffsetHours =  +8, Latitude =   1.29, Longitude =  103.85, DstRule = DstRule.None },
        new() { Name = "Australia (Perth)",            NativeName = "Australia (Perth)",            StandardOffsetHours =  +8, Latitude = -31.95, Longitude =  115.86, DstRule = DstRule.None },
        new() { Name = "Taiwan",                       NativeName = "臺灣",                          StandardOffsetHours =  +8, Latitude =  25.05, Longitude =  121.53, DstRule = DstRule.None },
        new() { Name = "Hong Kong",                    NativeName = "香港",                          StandardOffsetHours =  +8, Latitude =  22.32, Longitude =  114.17, DstRule = DstRule.None },

        // UTC +9
        new() { Name = "Japan",                        NativeName = "日本",                          StandardOffsetHours =  +9, Latitude =  35.69, Longitude =  139.69, DstRule = DstRule.None },
        new() { Name = "South Korea",                  NativeName = "대한민국",                       StandardOffsetHours =  +9, Latitude =  37.57, Longitude =  126.98, DstRule = DstRule.None },
        new() { Name = "North Korea",                  NativeName = "조선민주주의인민공화국",             StandardOffsetHours =  +9, Latitude =  39.02, Longitude =  125.75, DstRule = DstRule.None },
        new() { Name = "Indonesia (Makassar)",         NativeName = "Indonesia (Makassar)",         StandardOffsetHours =  +9, Latitude =  -5.13, Longitude =  119.42, DstRule = DstRule.None },
        new() { Name = "Russia (Yakutsk)",             NativeName = "Россия (Якутск)",              StandardOffsetHours =  +9, Latitude =  62.03, Longitude =  129.73, DstRule = DstRule.None },

        // UTC +10
        new() { Name = "Australia (Sydney)",           NativeName = "Australia (Sydney)",           StandardOffsetHours = +10, Latitude = -33.87, Longitude =  151.21, DstRule = DstRule.AUS  },
        new() { Name = "Australia (Melbourne)",        NativeName = "Australia (Melbourne)",        StandardOffsetHours = +10, Latitude = -37.81, Longitude =  144.96, DstRule = DstRule.AUS  },
        new() { Name = "Papua New Guinea",             NativeName = "Papua Niugini",                StandardOffsetHours = +10, Latitude =  -9.44, Longitude =  147.18, DstRule = DstRule.None },
        new() { Name = "Russia (Vladivostok)",         NativeName = "Россия (Владивосток)",         StandardOffsetHours = +10, Latitude =  43.12, Longitude =  131.90, DstRule = DstRule.None },

        // UTC +11
        new() { Name = "Solomon Islands",              NativeName = "Solomon Islands",              StandardOffsetHours = +11, Latitude =  -9.43, Longitude =  160.04, DstRule = DstRule.None },
        new() { Name = "New Caledonia",                NativeName = "Nouvelle-Calédonie",           StandardOffsetHours = +11, Latitude = -22.27, Longitude =  166.46, DstRule = DstRule.None },
        new() { Name = "Vanuatu",                      NativeName = "Vanuatu",                      StandardOffsetHours = +11, Latitude = -17.74, Longitude =  168.32, DstRule = DstRule.None },

        // UTC +12
        new() { Name = "New Zealand",                  NativeName = "New Zealand",                  StandardOffsetHours = +12, Latitude = -36.86, Longitude =  174.77, DstRule = DstRule.NZL  },
        new() { Name = "Fiji",                         NativeName = "Viti",                         StandardOffsetHours = +12, Latitude = -18.14, Longitude =  178.44, DstRule = DstRule.AUS  },
        new() { Name = "Russia (Kamchatka)",           NativeName = "Россия (Камчатка)",            StandardOffsetHours = +12, Latitude =  53.02, Longitude =  158.65, DstRule = DstRule.None },

        // UTC +13
        new() { Name = "Tonga",                        NativeName = "Tonga",                        StandardOffsetHours = +13, Latitude = -21.14, Longitude = -175.22, DstRule = DstRule.None },
        new() { Name = "Samoa",                        NativeName = "Samoa",                        StandardOffsetHours = +13, Latitude = -13.83, Longitude = -172.00, DstRule = DstRule.NZL  },

        // UTC +14
        new() { Name = "Kiribati (Line Islands)",      NativeName = "Kiribati (Line Islands)",      StandardOffsetHours = +14, Latitude =   1.87, Longitude = -157.33, DstRule = DstRule.None },
    };

    /// <summary>Returns only the countries whose standard UTC offset matches the given value.</summary>
    public static IReadOnlyList<CountryTimeZoneEntry> ForOffset(double utcOffsetHours)
        => All.Where(c => c.StandardOffsetHours == utcOffsetHours).ToList();

    /// <summary>
    /// Tries to identify the best-matching catalogue entry for the current device timezone.
    /// Strategy:
    ///   1. Obtain the device's standard UTC offset (without DST) from the OS.
    ///   2. Filter the catalogue to entries with that offset.
    ///   3. Among those, pick the one whose name contains a keyword from the IANA/Windows
    ///      timezone id reported by the OS (case-insensitive substring match).
    ///   4. If no keyword matches, return the first entry for that offset (or null).
    /// </summary>
    public static CountryTimeZoneEntry FindBySystemTimeZone()
    {
        try
        {
            var local = TimeZoneInfo.Local;

            // Standard offset (no DST) at a known winter date to avoid DST influence.
            double standardOffset = local.BaseUtcOffset.TotalHours;

            var candidates = ForOffset(standardOffset);
            if (candidates.Count == 0) return null;
            if (candidates.Count == 1) return candidates[0];

            // Build a search string from the OS timezone id (works for both
            // Windows timezone names like "W. Europe Standard Time" and
            // IANA names like "Europe/Rome").
            string tzId = local.Id.ToLowerInvariant();

            // Try to find a candidate whose name (lowercase) appears in the tz id,
            // or whose tz id contains a word from the candidate name.
            foreach (var entry in candidates)
            {
                string entryName = entry.Name.ToLowerInvariant();
                // Strip qualifiers like " (US)", " (east)", etc. for matching
                string baseName = entryName.Split('(')[0].Trim();
                if (tzId.Contains(baseName) || baseName.Split(' ').Any(w => w.Length > 3 && tzId.Contains(w)))
                    return entry;
            }

            // Fallback: return the first candidate for that offset.
            return candidates[0];
        }
        catch
        {
            return null;
        }
    }
}
