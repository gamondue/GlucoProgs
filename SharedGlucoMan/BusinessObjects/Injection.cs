using gamon;
using GlucoMan.BusinessLayer;

namespace GlucoMan
{
    public class Injection
    {
        public int? IdInjection { get; set; }
        public DateTimeAndText Timestamp { get; set; }
        public DoubleAndText InsulinValue { get; set; }
        public DoubleAndText InsulinCalculated { get; set; }
        public Common.ZoneOfPosition Zone { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public int? IdTypeOfInjection{ get; set; }
        public int? IdTypeOfInsulinAction { get; set; }
        public int? IdInsulinDrug { get; set; }
        public string InsulinString { get; set; }
        public string Notes { get; internal set; }

        // Proprietà enum per binding diretto (come in MealsPage)
        public Common.TypeOfInjection TypeOfInjection
        {
            get
            {
                if (IdTypeOfInjection.HasValue)
                    return (Common.TypeOfInjection)IdTypeOfInjection.Value;
                return Common.TypeOfInjection.NotSet;
            }
        }

        public Common.TypeOfInsulinAction TypeOfInsulinAction
        {
            get
            {
                if (IdTypeOfInsulinAction.HasValue)
                    return (Common.TypeOfInsulinAction)IdTypeOfInsulinAction.Value;
                return Common.TypeOfInsulinAction.NotSet;
            }
        }

        // Property to show in the grid the name of the insulin drug instead of its ID
        public string InsulinDrugName
        {
            get
            {
                if (IdInsulinDrug.HasValue)
                {
                    var bl = new BL_BolusesAndInjections();
                    var insulinDrug = bl.GetOneInsulinDrug(IdInsulinDrug);
                    return insulinDrug?.Name ?? IdInsulinDrug.ToString();
                }
                return "";
            }
        }

        public Injection()
        {
            Timestamp = new DateTimeAndText();
            InsulinValue = new DoubleAndText();
            InsulinValue.Format = "#";
            InsulinCalculated = new DoubleAndText();
            InsulinCalculated.Format = "#";
        }
    }
}
