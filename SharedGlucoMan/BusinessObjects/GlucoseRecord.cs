using gamon;
using System;

namespace GlucoMan
{
    public class GlucoseRecord : Event
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
