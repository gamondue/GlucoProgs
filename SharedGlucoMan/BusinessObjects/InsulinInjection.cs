using gamon;

namespace GlucoMan
{
    public class InsulinInjection
    {
        public int? IdInsulinInjection { get; set; }
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

        public InsulinInjection()
        {
            Timestamp = new DateTimeAndText();
            InsulinValue = new DoubleAndText();
            InsulinValue.Format = "#";
            InsulinCalculated = new DoubleAndText();
            InsulinCalculated.Format = "#";
            //PositionX = new IntAndText();
            //PositionY = new IntAndText();
            //IdTypeOfInsulinSpeed = new IntAndText();
            //IdTypeOfInsulinInjection = new IntAndText();
        }
    }
}
