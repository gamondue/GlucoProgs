using System;

namespace GlucoMan
{
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
        private Accuracy insulineAccuracy;

        private TypeOfGlucoseMeasurement glucoseMeasurementType;
        private double? glucoseValue;
        private string glucoseString;  // qualitative indication of glucose measured quantity
        private Accuracy glucoseAccuracy;

        private TypeOfMeal typeOfMeal;
        private double? carbohydratesValue;
        private string carbohydratesString;
        private string mealFoodString;
        private Accuracy carbohydratesAccuracy;

        private double? chetonsMmolPerL; // in mmol/L

        private string notes;

        internal DateTime? Timestamp { get => timestamp; set => timestamp = value; }
        internal string DeviceId { get => deviceId; set => deviceId = value; }
        internal string DeviceType { get => deviceType; set => deviceType = value; }
        internal int IdDocumentType { get => idDocumentType; set => idDocumentType = value; }
        internal double? GlucoseValue_mg_l { get => glucoseValue; set => glucoseValue = value; }
        internal double? InsulineValue { get => insulineValue; set => insulineValue = value; }
        public string GlucoseString { get => glucoseString; set => glucoseString = value; }
        public string InsulineString { get => insulineString; set => insulineString = value; }
        internal TypeOfInsulineBolus InsulineInjection { get => insulineInjection; set => insulineInjection = value; }
        internal string InsulineDrugName { get => insulineDrugName; set => insulineDrugName = value; }
        internal TypeOfInsulineSpeed InsulineSpeed { get => insulineSpeed; set => insulineSpeed = value; }
        internal Accuracy InsulineAccuracy { get => insulineAccuracy; set => insulineAccuracy = value; }
        internal TypeOfGlucoseMeasurement GlucoseMeasurementType { get => glucoseMeasurementType; set => glucoseMeasurementType = value; }
        internal Accuracy GlucoseAccuracy { get => glucoseAccuracy; set => glucoseAccuracy = value; }
        internal Accuracy CarbohydratesAccuracy { get => carbohydratesAccuracy; set => carbohydratesAccuracy = value; }
        internal TypeOfMeal TypeOfMeal { get => typeOfMeal; set => typeOfMeal = value; }
        internal double? CarbohydratesValue_grams { get => carbohydratesValue; set => carbohydratesValue = value; }
        internal string CarbohydratesString { get => carbohydratesString; set => carbohydratesString = value; }
        internal string MealFoodString { get => mealFoodString; set => mealFoodString = value; }
        internal double? Chetons_mmol_l { get => chetonsMmolPerL; set => chetonsMmolPerL = value; }
        internal string Notes { get => notes; set => notes = value; }
        internal int AccessoryIndex { get => accessoryIndex; set => accessoryIndex = value; }
    }
    enum TypeOfGlucoseMeasurement
    {
        NotSet,
        Strip, 
        SensorIntermediateValue,
        SensorScanValue
    }
    enum TypeOfMeal
    {
        NotSet,
        Breakfast,
        Lunch,
        Dinner,
        Snack,
        GlucoseCorrection
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
    enum Accuracy
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
}