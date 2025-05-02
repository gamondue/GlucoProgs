using gamon;
using System;

namespace GlucoMan
{
    public class GlucoseRecord
    {
        // properties directly mapped to database GlucoseRecordings table 
        public int? IdGlucoseRecord { get ; set ; }
        public DoubleAndText GlucoseValue { get; set; }  // in mg/l
        public DateTime? Timestamp { get; set; }
        public string GlucoseString { get; set; }   // qualitative indication of glucose measured quantity
        public Common.TypeOfGlucoseMeasurement TypeOfGlucoseMeasurement { get; internal set; }
        public Common.TypeOfGlucoseMeasurementDevice TypeOfGlucoseMeasurementDevice { get; internal set; }
        public string IdModelOfMeasurementSystem { get; internal set; }
        public string IdDevice { get; set; }
        public string IdDeviceType { get; set; }
        public int? IdDocumentType { get; set; }
        public string Notes { get; set; }

        // properties taken from other tables of database. Most are unuseful
        // !!!! TODO decide if they are useful and delete those not useful!!!!
        public double? InsulinValue { get; set; }
        // qualitative indication of insulin taken
        public string InsulinString { get; set; }
        public string InsulinDrugName { get; set; }
        public Common.TypeOfInsulinInjection InsulinInjectionType { get; set; }
        public Common.TypeOfInsulinSpeed InsulinSpeed { get; set; }
        public Common.QualitativeAccuracy InsulinAccuracy { get; set; }
        public Common.QualitativeAccuracy CarbohydratesAccuracy { get; set; }
        public Common.QualitativeAccuracy GlucoseQualitativeAccuracy { get; set; }
        public Common.TypeOfMeal TypeOfMeal { get; set; }
        public double? CarbohydratesValue_grams { get; set; }
        public string CarbohydratesString { get; set; }
        public string MealFoodString { get; set; }
        public int? AccessoryIndex { get; set; }
        public double? BodyWeight { get; set; }
        public double? BloodPressure { get; set; }
        public double? PhysicalActivity { get; set; }
        public int? Photo { get; set; }
        public GlucoseRecord()
        {
            GlucoseValue = new DoubleAndText();
            GlucoseValue.Format = "#"; 
        }
        public override string ToString()
        {
            return Timestamp?.ToString("yyyy-MM-dd HH:mm:ss"); 
        }
    }
}
