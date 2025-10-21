using GlucoMan.BusinessLayer;

namespace GlucoMan
{
    public static partial class Common
    {
        public static string DatabaseFileName = @"GlucoManData.Sqlite";
        public static string LogOfParametersFileName = @"Log of the Insulin Correction Parameters.txt";
        public static string PathUser;
        public static string PathUsersDownload;

        public static string PathConfigurationData;
        public static string PathProgramsData;
        public static string PathLogs;
        public static string PathDatabase;
        public static string PathAndFileDatabase;
        public static string PathImportExport;
        public static string PathAndFileLogOfParameters;

        // paths for Android
        public static string CommonApplicationPath;
        public static string LocalApplicationPath;
        public static string ExternalPublicPath;
        //public static string AppDataDirectoryPath;
        //public static string CacheDirectoryPath;
        //public static string myDocPath;

        internal static DataLayer Database;
        public static BL_General BlGeneral;
        public static BL_MealAndFood MealAndFood_CommonBL;

        public static string Version { get; private set; }

        public static double? breakfastStartHour = 6;
        public static double? breakfastEndHour = 10;
        public static double? lunchStartHour = 11;
        public static double? lunchEndHour = 15;
        public static double? dinnerStartHour = 17;
        public static double? dinnerEndHour = 21;
        
        #region enums
        public enum TypeOfGlucoseMeasurement
        {
            NotSet = 0,
            SensorIntermediateValue = 20,
            SensorScanValue = 30,
            GlucoseReactiveStripValue = 40,
            NotApplicable = 500
        }
        public enum TypeOfGlucoseMeasurementDevice
        {
            NotSet = 0,
            FingerPuncture = 10,
            UnderSkinSensor = 20,
            CGM = 30,
            ArtificialPancreas = 40,
            NotApplicable = 500
        }
        public enum ModelOfMeasurementSystem
        {
            Unknown = 0,
            AbbotFreestyle = 10,
            AbbotFreestyleLibre = 20,
            AbbotFreestyleLibre2 = 30,
            NotApplicable = 500
        }
        public enum TypeOfMeal
        {
            NotSet = 0,
            Breakfast = 10,
            Lunch = 20,
            Dinner = 30,
            Snack = 40,
            Other = 90,
            NotApplicable = 500
        }
        public enum TypeOfInjection
        {
            NotSet = 0,
            Bolus = 10,  // injection with a syringe of a bolus of insulin
            Blood = 20,  // puncture of the hand to get blood sample for glucose measurement
            Sensor = 30, // implantation of a sensor that measures blood glucose
            Other = 90
        }
        public enum TypeOfInsulinAction
        {
            NotSet = 0,
            Rapid = 10, // about 15 minutes to start working, peaks in 1-2 hours, and lasts for 2-4 hours
            Short = 20, // 30 minutes to start working, peaks in 2-3 hours, and lasts for 3-6 hours.
            Intermediate = 30, // about 2-4 hours to start working, peaks in 4-12 hours, and lasts for 12-18 hours.
            Long = 40 // 1-2 hours to start working, has no peak effect, and lasts for 24+ hours
        }
        // TODO !!!! rethink the next enum
        public enum InsulinDrug
        {
            NotSet = 0,
            BolusInsulin = 10,
            CorrectionBolus = 20,
            BasalInsulin = 30,
            BasalBolus = 40,
            Other = 90
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
        public enum ZoneOfPosition
        {
            NotSet = 0,
            Front = 1,
            Back = 2,
            Hands = 3,
            Sensor = 4,
        }
        #endregion
    }
}
