using System;
using System.Collections.Generic;
using System.Linq;

namespace GlucoMan
{
    /// <summary>
    /// Stretch of the daily chart in which the UTC offset doesn't change.
    /// </summary>
    public sealed class GlucoseChartCurve
    {
        public double? UtcOffset { get; }
        public List<(float Hour, float Value)> Points { get; } = new();

        public GlucoseChartCurve(double? utcOffset)
        {
            UtcOffset = utcOffset;
        }
    }

    /// <summary>
    /// Divides the samples of a day into curves. Every curve keeps the UTC offset of its first
    /// sample, that is the clock time of the database, so when the offset changes the two curves
    /// aren't joined: a jump when the clock goes forward, two curves in the same hour when it
    /// goes back.
    /// </summary>
    public static class GlucoseChartCurves
    {
        public static float HourOfDay(DateTime localTime)
        {
            return localTime.Hour + localTime.Minute / 60f + localTime.Second / 3600f;
        }

        public static List<GlucoseChartCurve> Build(IEnumerable<GlucoseRecord> records)
        {
            var samples = records
                .Where(r => r.EventTime?.DateTime.HasValue == true && r.GlucoseValue?.Double.HasValue == true)
                .OrderBy(r => r.EventTime.DateTime.Value)
                .ToList();

            // a sample without offset belongs to the stretch of the one before
            double? offset = samples.FirstOrDefault(r => r.UtcOffset.HasValue)?.UtcOffset;
            var timedSamples = new List<(DateTime Instant, double? Offset, DateTime Local, float Value)>();
            foreach (var sample in samples)
            {
                offset = sample.UtcOffset ?? offset;
                DateTime local = sample.EventTime.DateTime.Value;
                timedSamples.Add((local.AddHours(-(offset ?? 0)), offset, local, (float)sample.GlucoseValue.Double.Value));
            }

            // the instant, not the clock time: the hour repeated in autumn is mixed up otherwise
            var curves = new List<GlucoseChartCurve>();
            foreach (var sample in timedSamples.OrderBy(s => s.Instant))
            {
                if (curves.Count == 0 || curves[^1].UtcOffset != sample.Offset)
                    curves.Add(new GlucoseChartCurve(sample.Offset));
                curves[^1].Points.Add((HourOfDay(sample.Local), sample.Value));
            }
            return curves;
        }

        /// <summary>
        /// Curve on which to put a meal or an injection: on the day of a change of offset
        /// the same clock time can be on two curves.
        /// </summary>
        public static GlucoseChartCurve? CurveOfEvent(List<GlucoseChartCurve> curves, double? utcOffset)
        {
            return curves.FirstOrDefault(c => c.UtcOffset == utcOffset) ?? curves.FirstOrDefault();
        }
    }
}
