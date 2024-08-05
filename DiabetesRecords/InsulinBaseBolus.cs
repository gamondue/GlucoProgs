namespace DiabetesRecords
{
    internal class InsulinBaseBolus
    {
        internal int IdInsulinBase;
        internal string Name;
        internal double Value;

        public InsulinBaseBolus(int IdInsulinBase, double BaseBolusValue, string Name)
        {
            this.IdInsulinBase = IdInsulinBase;
            this.Value = BaseBolusValue;
            this.Name = Name;
        }
    }
}