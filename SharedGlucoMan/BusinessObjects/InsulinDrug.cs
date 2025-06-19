namespace GlucoMan.BusinessObjects
{
    public class InsulinDrug
    {
        public int? IdInsulinDrug { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public Common.TypeOfInsulinAction TypeOfInsulinAction { get; set; }
        public double? DurationInHours { get; set; }
        public double? OnsetTimeTimeInHours { get; set; } 
        public double? PeakTimeInHours { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
