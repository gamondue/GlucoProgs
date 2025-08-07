using gamon;

namespace GlucoMan
{
    public class UnitOfFood
    {
        public int? IdUnitOfFood { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? IdFood { get; set; }
        public DoubleAndText GramsInOneUnit { get; set; }

        public UnitOfFood()
        {
            this.GramsInOneUnit = new DoubleAndText();
            this.Symbol = "g";
            this.Description = "grams";
            this.GramsInOneUnit.Double = 1;
        }
        public UnitOfFood(string Symbol, double GramsInOneUnit)
        {
            this.GramsInOneUnit = new DoubleAndText();
            this.Symbol = Symbol;
            this.GramsInOneUnit.Double = GramsInOneUnit;
        }
        public override string ToString()
        {
            return Symbol + " - " + GramsInOneUnit.Text;
        }
    }
}
