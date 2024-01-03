using gamon;

namespace GlucoMan
{
    internal class InsulinInjection
    {
        public int? IdInsulinInjection { get; set; }
        public DateTimeAndText Timestamp { get; set; }
        public DoubleAndText InsulinValue { get; set; }
        public DoubleAndText InsulinCalculated { get; set; }
        public IntAndText InjectionPositionX { get; set; }
        public IntAndText InjectionPositionY { get; set; }
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
            InjectionPositionX = new IntAndText();
            InjectionPositionY = new IntAndText();
            //IdTypeOfInsulinSpeed = new IntAndText();
            //IdTypeOfInsulinInjection = new IntAndText();
        }
    }
}
