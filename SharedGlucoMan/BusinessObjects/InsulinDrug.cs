namespace GlucoMan.BusinessObjects
{
    public class InsulinDrug
    {
        public int IdInsulinDrug { get; set; }
        public string Name { get; set; }
        public string Maker { get; set; }
        public Common.TypeOfInsulinSpeed TypeOfSpeed { get; set; }
        public double? DurationInHours { get; set; }
    }
}
