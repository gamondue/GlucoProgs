using gamon;

namespace GlucoMan
{
    public class Injection
    {
        public int? IdInjection { get; set; }
        public DateTimeAndText Timestamp { get; set; }
        public DoubleAndText InsulinValue { get; set; }
        public DoubleAndText InsulinCalculated { get; set; }
        public Common.ZoneOfPosition Zone { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public int? IdTypeOfInsulinSpeed { get; set; }
        public int? IdTypeOfInsulinInjection { get; set; }
        public string InsulinString { get; set; }
        public string Notes { get; internal set; }

        public Injection()
        {
            Timestamp = new DateTimeAndText();
            InsulinValue = new DoubleAndText();
            InsulinValue.Format = "#";
            InsulinCalculated = new DoubleAndText();
            InsulinCalculated.Format = "#";
        }
    }
}
