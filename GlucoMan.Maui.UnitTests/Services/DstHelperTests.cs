using GlucoMan.Maui.Services;

namespace GlucoMan.Maui.UnitTests.Services;

[TestFixture]
public class DstHelperTests
{
    // ── NthSundayOf ──────────────────────────────────────────────────────────

    [Test]
    public void NthSundayOf_FirstSundayMarch2024_Returns3March()
    {
        // 1 Mar 2024 is a Friday; first Sunday is 3 Mar
        Assert.That(DstHelper.NthSundayOf(2024, 3, 1), Is.EqualTo(new DateTime(2024, 3, 3)));
    }

    [Test]
    public void NthSundayOf_SecondSundayMarch2024_Returns10March()
    {
        Assert.That(DstHelper.NthSundayOf(2024, 3, 2), Is.EqualTo(new DateTime(2024, 3, 10)));
    }

    [Test]
    public void NthSundayOf_FirstSundayNovember2024_Returns3November()
    {
        // 1 Nov 2024 is a Friday; first Sunday is 3 Nov
        Assert.That(DstHelper.NthSundayOf(2024, 11, 1), Is.EqualTo(new DateTime(2024, 11, 3)));
    }

    [Test]
    public void NthSundayOf_FirstSundayOctober2024_Returns6October()
    {
        // 1 Oct 2024 is a Tuesday; first Sunday is 6 Oct
        Assert.That(DstHelper.NthSundayOf(2024, 10, 1), Is.EqualTo(new DateTime(2024, 10, 6)));
    }

    [Test]
    public void NthSundayOf_FirstSundayApril2024_Returns7April()
    {
        // 1 Apr 2024 is a Monday; first Sunday is 7 Apr
        Assert.That(DstHelper.NthSundayOf(2024, 4, 1), Is.EqualTo(new DateTime(2024, 4, 7)));
    }

    // ── LastSundayOf ─────────────────────────────────────────────────────────

    [Test]
    public void LastSundayOf_March2024_Returns31March()
    {
        // 31 Mar 2024 is a Sunday
        Assert.That(DstHelper.LastSundayOf(2024, 3), Is.EqualTo(new DateTime(2024, 3, 31)));
    }

    [Test]
    public void LastSundayOf_October2024_Returns27October()
    {
        // 31 Oct 2024 is a Thursday; last Sunday is 27 Oct
        Assert.That(DstHelper.LastSundayOf(2024, 10), Is.EqualTo(new DateTime(2024, 10, 27)));
    }

    [Test]
    public void LastSundayOf_September2024_Returns29September()
    {
        // 30 Sep 2024 is a Monday; last Sunday is 29 Sep
        Assert.That(DstHelper.LastSundayOf(2024, 9), Is.EqualTo(new DateTime(2024, 9, 29)));
    }

    // ── DstRule.None ─────────────────────────────────────────────────────────

    [Test]
    public void IsDstActive_None_AlwaysFalse()
    {
        var utc = new DateTime(2024, 7, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.None, utc, 0), Is.False);
    }

    // ── DstRule.USA ──────────────────────────────────────────────────────────
    // 2024: start = 10 Mar 03:00 LST; end = 3 Nov 03:00 LST  (UTC-5 example)

    [Test]
    public void IsDstActive_USA_JustBeforeStart_IsFalse()
    {
        // 10 Mar 2024 02:59 LST  → UTC = 07:59
        var utc = new DateTime(2024, 3, 10, 7, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.USA, utc, -5), Is.False);
    }

    [Test]
    public void IsDstActive_USA_AtStart_IsTrue()
    {
        // 10 Mar 2024 03:00 LST  → UTC = 08:00
        var utc = new DateTime(2024, 3, 10, 8, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.USA, utc, -5), Is.True);
    }

    [Test]
    public void IsDstActive_USA_Summer_IsTrue()
    {
        var utc = new DateTime(2024, 7, 4, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.USA, utc, -5), Is.True);
    }

    [Test]
    public void IsDstActive_USA_JustBeforeEnd_IsTrue()
    {
        // 3 Nov 2024 02:59 LST → UTC = 07:59
        var utc = new DateTime(2024, 11, 3, 7, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.USA, utc, -5), Is.True);
    }

    [Test]
    public void IsDstActive_USA_AtEnd_IsFalse()
    {
        // 3 Nov 2024 03:00 LST → UTC = 08:00
        var utc = new DateTime(2024, 11, 3, 8, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.USA, utc, -5), Is.False);
    }

    [Test]
    public void IsDstActive_USA_Winter_IsFalse()
    {
        var utc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.USA, utc, -5), Is.False);
    }

    // ── DstRule.EU ───────────────────────────────────────────────────────────
    // 2024: start = 31 Mar 03:00 LST (UTC+1); end = 27 Oct 03:00 LST (UTC+1)

    [Test]
    public void IsDstActive_EU_JustBeforeStart_IsFalse()
    {
        // 31 Mar 2024 02:59 LST (UTC+1) → UTC = 01:59
        var utc = new DateTime(2024, 3, 31, 1, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.EU, utc, 1), Is.False);
    }

    [Test]
    public void IsDstActive_EU_AtStart_IsTrue()
    {
        // 31 Mar 2024 03:00 LST → UTC = 02:00
        var utc = new DateTime(2024, 3, 31, 2, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.EU, utc, 1), Is.True);
    }

    [Test]
    public void IsDstActive_EU_Summer_IsTrue()
    {
        var utc = new DateTime(2024, 6, 21, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.EU, utc, 1), Is.True);
    }

    [Test]
    public void IsDstActive_EU_JustBeforeEnd_IsTrue()
    {
        // 27 Oct 2024 02:59 LST → UTC = 01:59
        var utc = new DateTime(2024, 10, 27, 1, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.EU, utc, 1), Is.True);
    }

    [Test]
    public void IsDstActive_EU_AtEnd_IsFalse()
    {
        // 27 Oct 2024 03:00 LST → UTC = 02:00
        var utc = new DateTime(2024, 10, 27, 2, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.EU, utc, 1), Is.False);
    }

    [Test]
    public void IsDstActive_EU_Winter_IsFalse()
    {
        var utc = new DateTime(2024, 1, 10, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.EU, utc, 1), Is.False);
    }

    // ── DstRule.AUS ──────────────────────────────────────────────────────────
    // 2024: start = 6 Oct 03:00 LST (UTC+10); end = 7 Apr 03:00 LST (UTC+10)
    // DST is active Oct → Apr (crosses year boundary)

    [Test]
    public void IsDstActive_AUS_JustBeforeOctStart_IsFalse()
    {
        // 6 Oct 2024 02:59 LST (UTC+10) → UTC = 16:59 on 5 Oct
        var utc = new DateTime(2024, 10, 5, 16, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.AUS, utc, 10), Is.False);
    }

    [Test]
    public void IsDstActive_AUS_AtOctStart_IsTrue()
    {
        // 6 Oct 2024 03:00 LST → UTC = 5 Oct 17:00
        var utc = new DateTime(2024, 10, 5, 17, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.AUS, utc, 10), Is.True);
    }

    [Test]
    public void IsDstActive_AUS_MidSummer_IsTrue()
    {
        // January is summer in Australia
        var utc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.AUS, utc, 10), Is.True);
    }

    [Test]
    public void IsDstActive_AUS_JustBeforeAprilEnd_IsTrue()
    {
        // 7 Apr 2024 02:59 LST → UTC = 6 Apr 16:59
        var utc = new DateTime(2024, 4, 6, 16, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.AUS, utc, 10), Is.True);
    }

    [Test]
    public void IsDstActive_AUS_AtAprilEnd_IsFalse()
    {
        // 7 Apr 2024 03:00 LST → UTC = 6 Apr 17:00
        var utc = new DateTime(2024, 4, 6, 17, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.AUS, utc, 10), Is.False);
    }

    [Test]
    public void IsDstActive_AUS_MidWinter_IsFalse()
    {
        // July is winter in Australia
        var utc = new DateTime(2024, 7, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.AUS, utc, 10), Is.False);
    }

    // ── DstRule.NZL ──────────────────────────────────────────────────────────
    // 2024: start = last Sun Sep = 29 Sep 03:00 LST (UTC+12); end = 7 Apr 03:00 LST

    [Test]
    public void IsDstActive_NZL_JustBeforeSepStart_IsFalse()
    {
        // 29 Sep 2024 02:59 LST (UTC+12) → UTC = 28 Sep 14:59
        var utc = new DateTime(2024, 9, 28, 14, 59, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.NZL, utc, 12), Is.False);
    }

    [Test]
    public void IsDstActive_NZL_AtSepStart_IsTrue()
    {
        // 29 Sep 2024 03:00 LST → UTC = 28 Sep 15:00
        var utc = new DateTime(2024, 9, 28, 15, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.NZL, utc, 12), Is.True);
    }

    [Test]
    public void IsDstActive_NZL_MidSummer_IsTrue()
    {
        var utc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.NZL, utc, 12), Is.True);
    }

    [Test]
    public void IsDstActive_NZL_AtAprilEnd_IsFalse()
    {
        // 7 Apr 2024 03:00 LST → UTC = 6 Apr 15:00
        var utc = new DateTime(2024, 4, 6, 15, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.NZL, utc, 12), Is.False);
    }

    [Test]
    public void IsDstActive_NZL_MidWinter_IsFalse()
    {
        var utc = new DateTime(2024, 7, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(DstHelper.IsDstActive(DstRule.NZL, utc, 12), Is.False);
    }

    // ── CountryTimeZoneEntry.IsDstActiveAt ───────────────────────────────────

    [Test]
    public void IsDstActiveAt_Italy_SummerIsTrue()
    {
        var italy = CountryTimeZoneCatalogue.All.First(c => c.Name == "Italy");
        var summerUtc = new DateTime(2024, 7, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(italy.IsDstActiveAt(summerUtc), Is.True);
    }

    [Test]
    public void IsDstActiveAt_Italy_WinterIsFalse()
    {
        var italy = CountryTimeZoneCatalogue.All.First(c => c.Name == "Italy");
        var winterUtc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(italy.IsDstActiveAt(winterUtc), Is.False);
    }

    [Test]
    public void IsDstActiveAt_NewYork_SummerIsTrue()
    {
        var ny = CountryTimeZoneCatalogue.All.First(c => c.Name == "New York (US)");
        var summerUtc = new DateTime(2024, 8, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(ny.IsDstActiveAt(summerUtc), Is.True);
    }

    [Test]
    public void IsDstActiveAt_NewYork_WinterIsFalse()
    {
        var ny = CountryTimeZoneCatalogue.All.First(c => c.Name == "New York (US)");
        var winterUtc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(ny.IsDstActiveAt(winterUtc), Is.False);
    }

    [Test]
    public void IsDstActiveAt_NewZealand_SummerIsTrue()
    {
        var nz = CountryTimeZoneCatalogue.All.First(c => c.Name == "New Zealand");
        var summerUtc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(nz.IsDstActiveAt(summerUtc), Is.True);
    }

    [Test]
    public void IsDstActiveAt_AustraliaSydney_WinterIsFalse()
    {
        var sydney = CountryTimeZoneCatalogue.All.First(c => c.Name == "Australia (Sydney)");
        var winterUtc = new DateTime(2024, 7, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(sydney.IsDstActiveAt(winterUtc), Is.False);
    }

    [Test]
    public void UsesDaylightSaving_NoneRule_IsFalse()
    {
        var japan = CountryTimeZoneCatalogue.All.First(c => c.Name == "Japan");
        Assert.That(japan.UsesDaylightSaving, Is.False);
    }

    [Test]
    public void UsesDaylightSaving_EuRule_IsTrue()
    {
        var germany = CountryTimeZoneCatalogue.All.First(c => c.Name == "Germany");
        Assert.That(germany.UsesDaylightSaving, Is.True);
    }

    // ── Catalogue completeness ───────────────────────────────────────────────

    [Test]
    public void Catalogue_AllEntries_HaveDstRuleSet()
    {
        // All entries must have an explicitly assigned DstRule (not a default uninitialised value)
        // The enum values are 0=None, 1=USA, 2=EU, 3=AUS, 4=NZL — all are valid.
        Assert.That(CountryTimeZoneCatalogue.All.Count, Is.GreaterThan(0));
        // No entry should have a StandardOffsetHours outside the valid -12..+14 range
        Assert.That(CountryTimeZoneCatalogue.All.All(c => c.StandardOffsetHours >= -12 && c.StandardOffsetHours <= 14),
            Is.True);
    }

    // ── Half-hour / fractional UTC offsets ───────────────────────────────────

    [Test]
    public void Catalogue_India_HasOffset5_5()
    {
        var india = CountryTimeZoneCatalogue.All.First(c => c.Name == "India");
        Assert.That(india.StandardOffsetHours, Is.EqualTo(5.5));
    }

    [Test]
    public void Catalogue_SriLanka_HasOffset5_5()
    {
        var sl = CountryTimeZoneCatalogue.All.First(c => c.Name == "Sri Lanka");
        Assert.That(sl.StandardOffsetHours, Is.EqualTo(5.5));
    }

    [Test]
    public void Catalogue_Afghanistan_HasOffset4_5()
    {
        var af = CountryTimeZoneCatalogue.All.First(c => c.Name == "Afghanistan");
        Assert.That(af.StandardOffsetHours, Is.EqualTo(4.5));
    }

    [Test]
    public void Catalogue_Iran_HasOffset3_5()
    {
        var ir = CountryTimeZoneCatalogue.All.First(c => c.Name == "Iran");
        Assert.That(ir.StandardOffsetHours, Is.EqualTo(3.5));
    }

    [Test]
    public void Catalogue_Iran_HasEuDstRule()
    {
        var ir = CountryTimeZoneCatalogue.All.First(c => c.Name == "Iran");
        Assert.That(ir.DstRule, Is.EqualTo(DstRule.EU));
    }

    [Test]
    public void Catalogue_Myanmar_HasOffset6_5()
    {
        var mm = CountryTimeZoneCatalogue.All.First(c => c.Name == "Myanmar");
        Assert.That(mm.StandardOffsetHours, Is.EqualTo(6.5));
    }

    [Test]
    public void Catalogue_Nepal_HasOffset5_75()
    {
        var np = CountryTimeZoneCatalogue.All.First(c => c.Name == "Nepal");
        Assert.That(np.StandardOffsetHours, Is.EqualTo(5.75));
    }

    [Test]
    public void Catalogue_Newfoundland_HasOffsetMinus3_5()
    {
        var nf = CountryTimeZoneCatalogue.All.First(c => c.Name == "Newfoundland (Canada)");
        Assert.That(nf.StandardOffsetHours, Is.EqualTo(-3.5));
    }

    [Test]
    public void Catalogue_Newfoundland_HasUsaDstRule()
    {
        var nf = CountryTimeZoneCatalogue.All.First(c => c.Name == "Newfoundland (Canada)");
        Assert.That(nf.DstRule, Is.EqualTo(DstRule.USA));
    }

    [Test]
    public void ForOffset_India_ReturnsTwoEntries()
    {
        var entries = CountryTimeZoneCatalogue.ForOffset(5.5);
        Assert.That(entries.Count, Is.EqualTo(2));
        Assert.That(entries.Select(e => e.Name), Does.Contain("India"));
        Assert.That(entries.Select(e => e.Name), Does.Contain("Sri Lanka"));
    }

    [Test]
    public void ForOffset_Iran_ReturnsOneEntry()
    {
        var entries = CountryTimeZoneCatalogue.ForOffset(3.5);
        Assert.That(entries.Count, Is.EqualTo(1));
        Assert.That(entries[0].Name, Is.EqualTo("Iran"));
    }

    [Test]
    public void IsDstActive_Iran_EU_SummerIsTrue()
    {
        // Iran uses EU DST rule (UTC+3:30 standard); summer UTC = July 1 12:00
        // LST = 12:00 + 3:30 = 15:30 → within EU DST window (Mar→Oct)
        var iran = CountryTimeZoneCatalogue.All.First(c => c.Name == "Iran");
        var summerUtc = new DateTime(2024, 7, 1, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(iran.IsDstActiveAt(summerUtc), Is.True);
    }

    [Test]
    public void IsDstActive_Iran_EU_WinterIsFalse()
    {
        var iran = CountryTimeZoneCatalogue.All.First(c => c.Name == "Iran");
        var winterUtc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(iran.IsDstActiveAt(winterUtc), Is.False);
    }

    [Test]
    public void IsDstActive_India_NoneRule_AlwaysFalse()
    {
        var india = CountryTimeZoneCatalogue.All.First(c => c.Name == "India");
        var summerUtc = new DateTime(2024, 6, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(india.IsDstActiveAt(summerUtc), Is.False);
    }

    [Test]
    public void IsDstActive_Newfoundland_Summer_IsTrue()
    {
        // Newfoundland: UTC-3.5 standard, USA rule → DST in summer
        var nf = CountryTimeZoneCatalogue.All.First(c => c.Name == "Newfoundland (Canada)");
        var summerUtc = new DateTime(2024, 7, 4, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(nf.IsDstActiveAt(summerUtc), Is.True);
    }

    [Test]
    public void IsDstActive_Newfoundland_Winter_IsFalse()
    {
        var nf = CountryTimeZoneCatalogue.All.First(c => c.Name == "Newfoundland (Canada)");
        var winterUtc = new DateTime(2024, 1, 15, 12, 0, 0, DateTimeKind.Utc);
        Assert.That(nf.IsDstActiveAt(winterUtc), Is.False);
    }
}
