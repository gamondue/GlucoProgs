using System;
using System.Collections.Generic;
using System.Text;

namespace GlucoMan
{
    internal static partial class Common
    {
        internal static DataLayer Database;

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
            NotSet,
            Strip,
            SensorIntermediateValue,
            SensorScanValue
        }
        internal enum TypeOfGlucoseMeasurementDevice
        {
            NotSet,
            Injection,
            UnderSkinSensor,
            CGM,
            ArtificialPancreas
        }
        internal enum ModelOfMeasurementSystem
        {
            AbbotFreestyle,
            AbbotFreestyleLibre
        }
        internal enum TypeOfMeal
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
        internal enum TypeOfInsulinSpeed
        {
            NotSet,
            QuickActionInsulin,
            SlowActionInsulin
        }
        internal enum TypeOfInsulinInjection
        {
            NotSet,
            CarbBolus,
            CorrectionBolus,
            ExtendedBolus,
            BasalBolus
        }
        internal enum QualitativeAccuracy
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
    }
}
