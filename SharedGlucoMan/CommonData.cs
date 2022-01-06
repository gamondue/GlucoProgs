using SharedGlucoMan.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal static partial class Common
    {
        internal static DataLayer Database;
        internal static BL_General Bl; 
        internal static SharedData.Logger LogOfProgram;

        internal static string Version { get; private set; }

        internal static int breakfastStartHour = 6;
        internal static int breakfastEndHour = 10;
        internal static int lunchStartHour = 11;
        internal static int lunchEndHour = 15;
        internal static int dinnerStartHour = 17;
        internal static int dinnerEndHour = 21;

        #region enums
        internal enum TypeOfGlucoseMeasurement
        {
            NotSet = 0,
            Strip = 10,
            SensorIntermediateValue = 20,
            SensorScanValue = 30
        }
        internal enum TypeOfGlucoseMeasurementDevice
        {
            NotSet = 0,
            Injection = 10,
            UnderSkinSensor = 20,
            CGM = 30,
            ArtificialPancreas = 40
        }
        internal enum ModelOfMeasurementSystem
        {
            Unknown = 0,
            AbbotFreestyle = 10,
            AbbotFreestyleLibre = 20
        }
        internal enum TypeOfMeal
        {
            NotSet = 0,
            Breakfast = 10,
            Lunch = 20,
            Dinner = 30,
            Snack = 40,
            Other = 90
        }
        internal enum TypeOfInjection
        {
            Unknown = 0,
            MealInjection = 10,
            Night = 20,
            GlucoseRecover = 30,
            GlucoseCorrection = 40,
            DecreasingGlucoseCorrection = 50,
            Other = 60
        }
        internal enum TypeOfInsulinSpeed
        {
            NotSet = 0,
            QuickActionInsulin = 10,
            SlowActionInsulin = 20
        }
        internal enum TypeOfInsulinInjection
        {
            NotSet = 0,
            CarbBolus = 10,
            CorrectionBolus = 20,
            ExtendedBolus = 30,
            BasalBolus = 40
        }
        internal enum QualitativeAccuracy
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
