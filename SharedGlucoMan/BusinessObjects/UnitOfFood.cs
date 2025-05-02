using gamon;

namespace GlucoMan
{
    public class UnitOfFood
    {
        public int? IdUnit { get; set; }
        public int? IdFood { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DoubleAndText GramsInOneUnit { get; set; }

        public UnitOfFood()
        {
            this.GramsInOneUnit = new DoubleAndText();
            this.Name = "g";
            this.GramsInOneUnit.Double = 1;
        }
        public UnitOfFood(string Name, double GramsInOneUnit)
        {
            this.GramsInOneUnit = new DoubleAndText();
            this.Name = Name;
            this.GramsInOneUnit.Double = GramsInOneUnit;
        }
        public override string ToString()
        {
            return Name + " - " + GramsInOneUnit.Text;
        }
    }
}
