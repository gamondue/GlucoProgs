using gamon;
using System.ComponentModel;

namespace GlucoMan
{
    public class Injection : Event, INotifyPropertyChanged
    {
        public int? IdInjection { get; set; }
        public DoubleAndText InsulinValue { get; set; }
        public DoubleAndText InsulinCalculated { get; set; }
        public Common.ZoneOfPosition Zone { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public int? IdTypeOfInjection{ get; set; }
        public int? IdTypeOfInsulinAction { get; set; }
        public int? IdInsulinDrug { get; set; }
        public string InsulinString { get; set; }
        public double? UtcOffset { get; set; }

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
#if !MATH_TESTS_ONLY
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
#endif

        private bool _isSelectedInList;

        public bool IsSelectedInList
        {
            get => _isSelectedInList;
            set
            {
                if (_isSelectedInList != value)
                {
                    _isSelectedInList = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelectedInList)));
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RowBorderColor)));
                }
            }
        }

        public string RowBorderColor => IsSelectedInList ? "Orange" : "Transparent";

        private DateTime? SafeEventDateTime =>
            EventTime?.DateTime is DateTime dt && dt.Year > 1 ? dt : null;

        public string EventDateTimeText
        {
            get
            {
                var dt = SafeEventDateTime;
                if (dt == null) return "";
                var tz = UtcOffset ?? 0;
                var sign = tz >= 0 ? "+" : "";
                return $"{dt:dd/MM/yyyy HH:mm:ss} ({sign}{tz})";
            }
        }

        public new event PropertyChangedEventHandler? PropertyChanged;

        public Injection()
        {
            EventTime = new DateTimeAndText();
            InsulinValue = new DoubleAndText();
            InsulinValue.Format = "#";
            InsulinCalculated = new DoubleAndText();
            InsulinCalculated.Format = "#";
        }
    }
}
