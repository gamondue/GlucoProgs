using GlucoMan; 
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using gamon;

namespace GlucoMan.BusinessLayer
{
    class BL_GlucoseMeasurements
    {
        DataLayer dl = Common.Database;
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
            return dl.GetGlucoseRecords(InitialTime, FinalTime); 
        }
        internal GlucoseRecord GetOneGlucoseRecord(int? IdGlucoseRecord)
        {
            return dl.GetOneGlucoseRecord(IdGlucoseRecord); 
        }
        internal List<GlucoseRecord> GetLastTwoGlucoseMeasurements()
        {
            return dl.GetLastTwoGlucoseMeasurements(); 
        }
        internal void SaveOneGlucoseMeasurement(GlucoseRecord GlucoseMeasurement)
        {
            dl.SaveOneGlucoseMeasurement(GlucoseMeasurement); 
        }
        internal void DeleteOneGlucoseMeasurement(GlucoseRecord gr)
        {
            dl.DeleteOneGlucoseMeasurement(gr); 
        }
    }
}
