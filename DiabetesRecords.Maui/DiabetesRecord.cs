using System;

namespace DiabetesRecords
{
    internal class DiabetesRecord
    {
        public int? IdDiabetesRecord { get; internal set; }
        public DateTime? Timestamp { get; internal set; }
        public double? GlucoseValue { get; internal set; }
        public double? InsulinValue { get; internal set; }
        public int? IdTypeOfInsulinSpeed { get; internal set; }
        public int? IdTypeOfMeal { get; internal set; }
        public string Notes { get; internal set; }
    }
}