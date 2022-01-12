using System;

namespace GlucoMan
{
    internal class GlucoseRecord
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
        internal int? IdGlucoseRecord { get ; set ; }
        internal double? GlucoseValue { get => glucoseValue; set => glucoseValue = value; }  // in mg/l
        internal DateTime? Timestamp { get => timestamp; set => timestamp = value; }
        internal string GlucoseString { get => glucoseString; set => glucoseString = value; }
        internal string IdDevice { get => idDevice; set => idDevice = value; }
        internal string IdDeviceType { get => idDeviceType; set => idDeviceType = value; }
        internal int? IdDocumentType { get => idDocumentType; set => idDocumentType = value; }
        internal Common.TypeOfGlucoseMeasurement GlucoseMeasurementType { get => glucoseMeasurementType; set => glucoseMeasurementType = value; }
        internal Common.QualitativeAccuracy GlucoseQualitativeAccuracy { get => glucoseQualitativeAccuracy; set => glucoseQualitativeAccuracy = value; }
        internal string Notes { get => notes; set => notes = value; }

        // properties taken from other tables of database 
        // !!!! TODO visually separate better, when done with database tables !!!!
        internal double? InsulinValue { get => insulinValue; set => insulinValue = value; }
        internal string InsulinString { get => insulinString; set => insulinString = value; }
        internal string InsulinDrugName { get => insulinDrugName; set => insulinDrugName = value; }
        internal Common.TypeOfInsulinInjection InsulinInjection { get => insulinInjection; set => insulinInjection = value; }
        internal Common.TypeOfInsulinSpeed InsulinSpeed { get => insulinSpeed; set => insulinSpeed = value; }
        internal Common.QualitativeAccuracy InsulinAccuracy { get => insulinAccuracy; set => insulinAccuracy = value; }
        internal Common.QualitativeAccuracy CarbohydratesAccuracy { get => carbohydratesAccuracy; set => carbohydratesAccuracy = value; }
        internal Common.TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        internal double? CarbohydratesValue_grams { get => carbohydratesValue; set => carbohydratesValue = value; }
        internal string CarbohydratesString { get => carbohydratesString; set => carbohydratesString = value; }
        internal string MealFoodString { get => mealFoodString; set => mealFoodString = value; }
        internal int? AccessoryIndex { get => accessoryIndex; set => accessoryIndex = value; }
        internal double? BodyWeight { get; set; }
        internal double? BloodPressure { get; set; }
        internal double? PhysicalActivity { get; set; }
        internal int? Photo { get; set; }
        public override string ToString()
        {
            return Timestamp?.ToString("yyyy-MM-dd HH:mm:ss"); 
        }
    }
}
