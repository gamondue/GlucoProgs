namespace DiabetesRecords
{
    internal class InsulinBase
    {
        internal int IdInsulinBase;
        internal string Name;
        internal double BaseValue;

        public InsulinBase(int IdInsulinBase, double BaseValue, string Name)
        {
            this.IdInsulinBase = IdInsulinBase;
            this.BaseValue = BaseValue;
            this.Name = Name;
        }
    }
}