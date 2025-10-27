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
        internal List<GlucoseRecord> GetGlucoseRecords(DateTime? startInstant = null, DateTime? endInstant = null)
        {
            return dl.GetGlucoseRecords(startInstant, endInstant);
        }
        internal List<GlucoseRecord> GetSensorsRecords(DateTime? startInstant = null, DateTime? endInstant = null)
        {
            return dl.GetSensorsRecords(startInstant, endInstant);
        }
        internal List<(float Hour, float Value)> GetGraphData(DateTime day)
        {
            // Calculate midnight of the selected day and midnight of the next day
            DateTime startOfDay = day.Date; // Midnight of the selected day (00:00:00)
            DateTime endOfDay = day.Date.AddDays(1); // Midnight of the next day (24:00:00 or 00:00:00 of next day)
            var records = dl.GetGlucoseRecords(startOfDay, endOfDay);
            var dataPoints = new List<(float Hour, float Value)>();
            foreach (var record in records)
            {
                if (record.GlucoseValue.Double.HasValue && record.EventTime.DateTime.HasValue)
                {
                    float hour = record.EventTime.DateTime.Value.Hour +
                                 record.EventTime.DateTime.Value.Minute / 60f +
                                 record.EventTime.DateTime.Value.Second / 3600f;
                    float value = (float)record.GlucoseValue.Double.Value;
                    dataPoints.Add((hour, value));
                }
            }
            return dataPoints;
        }
    }
}
