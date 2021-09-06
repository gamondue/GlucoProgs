using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan.BusinessLayer
{
    class BL_GlucoseMeasurements
    {
        DataLayer dl = new DataLayer();         
        internal IntAndText GlucoseMeasurement { get; set; }
        internal DateTimeAndText MeasurementTime { get; set; }
        internal BL_GlucoseMeasurements()
        {
            GlucoseMeasurement = new IntAndText();
            GlucoseMeasurement.Format = "#.#";
            MeasurementTime = new DateTimeAndText();
        }
        internal void SaveGlucoseMeasurements(List<GlucoseRecord> glucoseReadings)
        {
            dl.SaveGlucoseMeasurements(glucoseReadings); 
        }
        internal List<GlucoseRecord> ReadGlucoseMeasurements(DateTime? InitialTime, 
            DateTime? FinalTime)
        {
            return dl.ReadGlucoseMeasurements(InitialTime, FinalTime); 
        }
        internal List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
        {
            return dl.GetLastTwoGlucoseMeasurements(); 
        }
    }
}
