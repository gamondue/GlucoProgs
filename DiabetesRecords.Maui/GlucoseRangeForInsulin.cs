namespace DiabetesRecords; 

internal class GlucoseRangeForInsulin
{
    internal int IdRange { get; set; }
    internal double InsulinDelta { get; set; }
    internal double GlucoseSup { get; set; }
    public GlucoseRangeForInsulin(int IdRange, double InsulinDelta, 
        double GlucoseSup)
    {
        this.IdRange = IdRange; 
        this.InsulinDelta = InsulinDelta;   
        this.GlucoseSup = GlucoseSup;
    }
}