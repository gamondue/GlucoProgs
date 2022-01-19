using SharedGlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    public static partial class Common
    {
        public static DataLayer Database;
        public static BL_General Bl; 
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
            QuickActionInsulin = 10,
            SlowActionInsulin = 20
        }
        public enum TypeOfInsulinInjection
        {
            NotSet = 0,
            CarbBolus = 10,
            CorrectionBolus = 20,
            ExtendedBolus = 30,
            BasalBolus = 40
        }
        public enum QualitativeAccuracy
        {
            NotSet = -1,
            Null = 0, 
            VeryBad = 10,
            Bad = 20,
            Poor = 30,
            AlmostSufficient = 40,
            Sufficient = 50,
            Satisfactory = 60,
            Good = 70,
            VeryGood = 80,
            Outstanding = 90,
            Perfect = 100
        }
        #endregion
    }
}
