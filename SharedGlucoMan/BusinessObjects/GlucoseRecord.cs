using System;

namespace GlucoMan
{
    public class GlucoseRecord
    {
        private DateTime? timestamp;
        private string idDevice;
        private int? accessoryIndex;
        private string idDeviceType;
        private int? idDocumentType;

        private Common.TypeOfInsulinSpeed insulinSpeed;
        private Common.TypeOfInsulinInjection insulinInjection;
        private string insulinDrugName;
        private double? insulinValue;
        private string insulinString; // qualitative indication of insulin taken
        private Common.QualitativeAccuracy insulinAccuracy;

        private Common.TypeOfGlucoseMeasurement glucoseMeasurementType;
        private double? glucoseValue;
        private string glucoseString;  // qualitative indication of glucose measured quantity
        private Common.QualitativeAccuracy glucoseQualitativeAccuracy;

        private Common.TypeOfMeal typeOfMeal;
        private double? carbohydratesValue;
        private Common.QualitativeAccuracy carbohydratesAccuracy;
        private string carbohydratesString;
        private string mealFoodString;

        private string notes;

        // properties directly mapped to database GlucoseRecordings table 
        // !!!! TODO visually separate when done with database tables !!!!
        public int? IdGlucoseRecord { get ; set ; }
        public double? GlucoseValue { get => glucoseValue; set => glucoseValue = value; }  // in mg/l
        public DateTime? Timestamp { get => timestamp; set => timestamp = value; }
        public string GlucoseString { get => glucoseString; set => glucoseString = value; }
        public string IdDevice { get => idDevice; set => idDevice = value; }
        public string IdDeviceType { get => idDeviceType; set => idDeviceType = value; }
        public int? IdDocumentType { get => idDocumentType; set => idDocumentType = value; }
        public Common.TypeOfGlucoseMeasurement GlucoseMeasurementType { get => glucoseMeasurementType; set => glucoseMeasurementType = value; }
        public Common.QualitativeAccuracy  GlucoseQualitativeAccuracy { get => glucoseQualitativeAccuracy; set => glucoseQualitativeAccuracy = value; }
        public string Notes { get => notes; set => notes = value; }

        // properties taken from other tables of database 
        // !!!! TODO visually separate better, when done with database tables !!!!
        public double? InsulinValue { get => insulinValue; set => insulinValue = value; }
        public string InsulinString { get => insulinString; set => insulinString = value; }
        public string InsulinDrugName { get => insulinDrugName; set => insulinDrugName = value; }
        public Common.TypeOfInsulinInjection InsulinInjectionType { get => insulinInjection; set => insulinInjection = value; }
        public Common.TypeOfInsulinSpeed InsulinSpeed { get => insulinSpeed; set => insulinSpeed = value; }
        public Common.QualitativeAccuracy InsulinAccuracy { get => insulinAccuracy; set => insulinAccuracy = value; }
        public Common.QualitativeAccuracy CarbohydratesAccuracy { get => carbohydratesAccuracy; set => carbohydratesAccuracy = value; }
        public Common.TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        public double? CarbohydratesValue_g { get => carbohydratesValue; set => carbohydratesValue = value; }
        public string CarbohydratesString { get => carbohydratesString; set => carbohydratesString = value; }
        public string MealFoodString { get => mealFoodString; set => mealFoodString = value; }
        public int? AccessoryIndex { get => accessoryIndex; set => accessoryIndex = value; }
        public double? BodyWeight { get; set; }
        public double? BloodPressure { get; set; }
        public double? PhysicalActivity { get; set; }
        public int? Photo { get; set; }
        public override string ToString()
        {
            return Timestamp?.ToString("yyyy-MM-dd HH:mm:ss"); 
        }
    }
}
