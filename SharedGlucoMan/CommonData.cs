using GlucoMan.BusinessLayer;
using SharedGlucoMan.BusinessLayer;
using System;

namespace GlucoMan
{
    public static partial class Common
    {        
        internal static DataLayer Database;
        public static BL_General BlGeneral;
        public static BL_MealAndFood MealAndFood_CommonBL = new BL_MealAndFood();

        public static SharedData.Logger LogOfProgram;

        public static string Version { get; private set; }

        public static int breakfastStartHour = 6;
        public static int breakfastEndHour = 10;
        public static int lunchStartHour = 11;
        public static int lunchEndHour = 15;
        public static int dinnerStartHour = 17;
        public static int dinnerEndHour = 21;

        public static DateTime DateNull = new DateTime(1, 1, 1, 0, 0, 0, 0);

        #region enums
        public enum TypeOfGlucoseMeasurement
        {
            NotSet = 0,
            Strip = 10,
            SensorIntermediateValue = 20,
            SensorScanValue = 30
        }
        public enum TypeOfGlucoseMeasurementDevice
        {
            NotSet = 0,
            Injection = 10,
            UnderSkinSensor = 20,
            CGM = 30,
            ArtificialPancreas = 40
        }
        public enum ModelOfMeasurementSystem
        {
            Unknown = 0,
            AbbotFreestyle = 10,
            AbbotFreestyleLibre = 20
        }
        public enum TypeOfMeal
        {
            NotSet = 0,
            Breakfast = 10,
            Lunch = 20,
            Dinner = 30,
            Snack = 40,
            Other = 90
        }
        public enum TypeOfInjection
        {
            Unknown = 0,
            MealInjection = 10,
            Night = 20,
            GlucoseRecover = 30,
            GlucoseCorrection = 40,
            DecreasingGlucoseCorrection = 50,
            Other = 60
        }
        public enum TypeOfInsulinSpeed
        {
            NotSet = 0,
            QuickAction = 10,
            SlowAction = 20
        }
        public enum TypeOfInsulinInjection
        {
            NotSet = 0,
            CarbohydratesBolus = 10,
            CorrectionBolus = 20,
            ExtendedEffectBolus = 30,
            BasalBolus = 40
        }
        public enum QualitativeAccuracy
        {           
            NotSet = -1,
            Null = 0,
            AlmostNull = 10,
            VeryBad = 20,
            Bad = 30,
            Poor = 40,
            AlmostSufficient = 50,
            Sufficient = 60,
            Satisfactory = 70,
            Good = 80,
            Outstanding = 90,
            Perfect = 100
        }
        #endregion
    }
}
