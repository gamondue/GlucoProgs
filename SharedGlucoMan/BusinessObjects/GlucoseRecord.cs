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
        GlucoseRecover,
        DecreasingGlucoseCorrection,
        Snack,
        Night,
        GlucoseCorrection,
        Other
    }
    enum TypeOfInsulineSpeed
    {
        NotSet,
        QuickActionInsuline,
        SlowActionInsuline
    }
    enum TypeOfInsulineBolus
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
        private int accessoryIndex;
        private string deviceType;
        private int idDocumentType;

        private TypeOfInsulineSpeed insulineSpeed;
        private TypeOfInsulineBolus insulineInjection;
        private string insulineDrugName;
        private double? insulineValue;
        private string insulineString; // qualitative indication of insuline taken
        private QualitativeAccuracy insulineAccuracy;

        private TypeOfGlucoseMeasurement glucoseMeasurementType;
        private double? glucoseValue;
        private string glucoseString;  // qualitative indication of glucose measured quantity
        private QualitativeAccuracy glucoseAccuracy;

        private TypeOfMeal typeOfMeal;
        private double? carbohydratesValue;
        private string carbohydratesString;
        private string mealFoodString;
        private QualitativeAccuracy carbohydratesAccuracy;

        private double? chetonsMmolPerL; // in mmol/L

        private string notes;

        public DateTime? Timestamp { get => timestamp; set => timestamp = value; }
        internal string DeviceId { get => deviceId; set => deviceId = value; }
        internal string DeviceType { get => deviceType; set => deviceType = value; }
        internal int IdDocumentType { get => idDocumentType; set => idDocumentType = value; }
        public double? GlucoseValue { get => glucoseValue; set => glucoseValue = value; }  // in mg/l
        internal double? InsulineValue { get => insulineValue; set => insulineValue = value; }
        internal string GlucoseString { get => glucoseString; set => glucoseString = value; }
        internal string InsulineString { get => insulineString; set => insulineString = value; }
        internal TypeOfInsulineBolus InsulineInjection { get => insulineInjection; set => insulineInjection = value; }
        internal string InsulineDrugName { get => insulineDrugName; set => insulineDrugName = value; }
        internal TypeOfInsulineSpeed InsulineSpeed { get => insulineSpeed; set => insulineSpeed = value; }
        internal QualitativeAccuracy InsulineAccuracy { get => insulineAccuracy; set => insulineAccuracy = value; }
        internal TypeOfGlucoseMeasurement GlucoseMeasurementType { get => glucoseMeasurementType; set => glucoseMeasurementType = value; }
        internal QualitativeAccuracy GlucoseAccuracy { get => glucoseAccuracy; set => glucoseAccuracy = value; }
        internal QualitativeAccuracy CarbohydratesAccuracy { get => carbohydratesAccuracy; set => carbohydratesAccuracy = value; }
        internal TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        internal double? CarbohydratesValue_grams { get => carbohydratesValue; set => carbohydratesValue = value; }
        internal string CarbohydratesString { get => carbohydratesString; set => carbohydratesString = value; }
        internal string MealFoodString { get => mealFoodString; set => mealFoodString = value; }
        internal double? Chetons { get => chetonsMmolPerL; set => chetonsMmolPerL = value; } // in mmol/l
        internal int AccessoryIndex { get => accessoryIndex; set => accessoryIndex = value; }
        internal double? Weight { get; set; }
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
