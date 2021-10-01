using System;

namespace GlucoMan
{
    #region enums
    enum TypeOfGlucoseMeasurement
    {
        NotSet,
        Strip,
        SensorIntermediateValue,
        SensorScanValue
    }
    enum TypeOfGlucoseMeasurementDevice
    {
        NotSet,
        Strip,
        SensorIntermediateValue,
        SensorScanValue
    }
    enum ModelOfMeasurementSystem
    {
        AbbotFreestyle,
        AbbotFreestyleLibre
    }
    enum TypeOfMeal
    {
        NotSet,
        Breakfast,
        Lunch,
        Dinner,
        Snack,
        GlucoseRecover,
        GlucoseCorrection,
        DecreasingGlucoseCorrection,
        Night,
        Other
    }
    enum TypeOfInsulinSpeed
    {
        NotSet,
        QuickActionInsulin,
        SlowActionInsulin
    }
    enum TypeOfInsulinInjection
    {
        NotSet,
        CarbBolus,
        CorrectionBolus,
        ExtendedBolus,
        BasalBolus
    }
    enum QualitativeAccuracy
    {
        NotSet,
        VeryBad,
        Bad,
        Poor,
        Satifactory,
        Good,
        VeryGood,
        Outstanding,
        Perfect
    }

    #endregion
    internal class GlucoseRecord
    {
        private DateTime? timestamp;
        private string deviceId;
        private int? accessoryIndex;
        private string deviceType;
        private int? idDocumentType;

        private TypeOfInsulinSpeed insulinSpeed;
        private TypeOfInsulinInjection insulinInjection;
        private string insulinDrugName;
        private double? insulinValue;
        private string insulinString; // qualitative indication of insulin taken
        private double? insulinAccuracy;

        private TypeOfGlucoseMeasurement glucoseMeasurementType;
        private double? glucoseValue;
        private string glucoseString;  // qualitative indication of glucose measured quantity
        private double? glucoseAccuracy;

        private TypeOfMeal typeOfMeal;
        private double? carbohydratesValue;
        private double? carbohydratesAccuracy;
        private string carbohydratesString;
        private string mealFoodString;

        private string notes;

        public DateTime? Timestamp { get => timestamp; set => timestamp = value; }
        internal string DeviceId { get => deviceId; set => deviceId = value; }
        internal string DeviceType { get => deviceType; set => deviceType = value; }
        internal int? IdDocumentType { get => idDocumentType; set => idDocumentType = value; }
        public double? GlucoseValue { get => glucoseValue; set => glucoseValue = value; }  // in mg/l
        internal double? InsulinValue { get => insulinValue; set => insulinValue = value; }
        internal string GlucoseString { get => glucoseString; set => glucoseString = value; }
        internal string InsulinString { get => insulinString; set => insulinString = value; }
        internal TypeOfInsulinInjection InsulinInjection { get => insulinInjection; set => insulinInjection = value; }
        internal string InsulinDrugName { get => insulinDrugName; set => insulinDrugName = value; }
        internal TypeOfInsulinSpeed InsulinSpeed { get => insulinSpeed; set => insulinSpeed = value; }
        internal TypeOfGlucoseMeasurement GlucoseMeasurementType { get => glucoseMeasurementType; set => glucoseMeasurementType = value; }
        internal double? InsulinAccuracy { get => insulinAccuracy; set => insulinAccuracy = value; }
        internal double? GlucoseAccuracy { get => glucoseAccuracy; set => glucoseAccuracy = value; }
        internal double? CarbohydratesAccuracy { get => carbohydratesAccuracy; set => carbohydratesAccuracy = value; }
        internal TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        internal double? CarbohydratesValue_grams { get => carbohydratesValue; set => carbohydratesValue = value; }
        internal string CarbohydratesString { get => carbohydratesString; set => carbohydratesString = value; }
        internal string MealFoodString { get => mealFoodString; set => mealFoodString = value; }
        internal int? AccessoryIndex { get => accessoryIndex; set => accessoryIndex = value; }
        internal double? BodyWeight { get; set; }
        internal double? BloodPressure { get; set; }
        internal double? PhysicalActivity { get; set; }
        internal int? Photo { get; set; }
        internal string Notes { get => notes; set => notes = value; }
        public override string ToString()
        {
            return Timestamp?.ToString("yyyy-MM-dd HH:mm:ss"); 
        }
    }
}
