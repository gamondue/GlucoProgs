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
            GramsInOneUnit = new DoubleAndText();
        }
        public override string ToString()
        {
            return Name + " - " + GramsInOneUnit.Text;
        }
    }
}
