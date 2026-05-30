using gamon;
using System;
using System.ComponentModel;

namespace GlucoMan
{
    public class GlucoseRecord : Event, INotifyPropertyChanged
    {
        // properties inherited from Event class
        // public DateTimeAndText EventTime { get; set; }
        // public string Notes { get; set; }

        // properties directly mapped to database GlucoseRecords table 
        public int? IdGlucoseRecord { get ; set ; }
        public DoubleAndText GlucoseValue { get; set; }  // in mg/l
        public string GlucoseString { get; set; }   // qualitative indication of glucose measured quantity
        public Common.TypeOfGlucoseMeasurement TypeOfGlucoseMeasurement { get; internal set; }
        public Common.TypeOfDevice TypeOfGlucoseMeasurementDevice { get; internal set; }
        public int? IdOfDevice { get; set; }
        public string IdTypeOfDevice { get; set; }
        public int? IdDeviceModel { get; internal set; }
        public double? UtcOffset { get; set; }

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

        public string EventDateText => SafeEventDateTime?.ToString("dd/MM") ?? "";
        public string EventTimeText => SafeEventDateTime?.ToString("HH:mm") ?? "";
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

        public event PropertyChangedEventHandler? PropertyChanged;

        public GlucoseRecord()
        {
            GlucoseValue = new DoubleAndText();
            GlucoseValue.Format = "#"; 
        }
        public override string ToString()
        {
            string glucoseString;
            // return a string representation of the glucose record
            if (EventTime == null || EventTime.DateTime == null || EventTime.DateTime == General.DateNull)
                glucoseString = "No timestamp available";
            else
            {
                // use the DateTimeAndText class to format the timestamp
                glucoseString = EventTime.Text;
            }
            return glucoseString + " - " + GlucoseValue.Text + " mg/dL" +
                (Notes != null && Notes.Length > 0 ? " - " + Notes : "") +
                (IdGlucoseRecord != null ? " - Id: " + IdGlucoseRecord.ToString() : "");
        }
    }
}
