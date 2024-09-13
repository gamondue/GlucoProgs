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
        //PathProgramsData = Path.Combine(Common.LocalApplicationPath, @"Data");
        public static string PathLogs;
        // PathLogs = Path.Combine(Common.LocalApplicationPath, @"Logs");
        public static string PathDatabase;
        // PathDatabase = Path.Combine(Common.LocalApplicationPath, @"Data");
        public static string PathAndFileDatabase;
        // PathAndFileDatabase = Path.Combine(Common.PathDatabase, Common.DatabaseFileName);
        public static string PathImportExport;
        // PathImportExport = Path.Combine(Common.ExternalPublicPath, @"Glucoman");
        public static string PathAndFileLogOfParameters;
        // PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, LogOfParametersFileName);

        internal static DataLayer Database;
        public static BL_General BlGeneral;
        public static BL_MealAndFood MealAndFood_CommonBL;

        public static string Version { get; private set; }

        public static int breakfastStartHour = 6;
        public static int breakfastEndHour = 10;
        public static int lunchStartHour = 11;
        public static int lunchEndHour = 15;
        public static int dinnerStartHour = 17;
        public static int dinnerEndHour = 21;

        #region enums
        public enum TypeOfGlucoseMeasurement
        {
            NotSet = 0,
            SensorIntermediateValue = 20,
            SensorScanValue = 30
        }
        public enum TypeOfGlucoseMeasurementDevice
        {
            NotSet = 0,
            FingerPuncture = 10,
            UnderSkinSensor = 20,
            CGM = 30,
            ArtificialPancreas = 40
        }
        public enum ModelOfMeasurementSystem
        {
            Unknown = 0,
            AbbotFreestyle = 10,
            AbbotFreestyleLibre = 20,
            AbbotFreestyleLibre2 = 20
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
        public enum TypeOfInsulinSpeed
        {
            NotSet = 0,
            RapidActing = 10, // about 15 minutes to start working, peaks in 1-2 hours, and lasts for 2-4 hours
            ShortActing = 20, // 30 minutes to start working, peaks in 2-3 hours, and lasts for 3-6 hours.
            IntermediateActing = 30, // about 2-4 hours to start working, peaks in 4-12 hours, and lasts for 12-18 hours.
            LongActing = 40 // 1-2 hours to start working, has no peak effect, and lasts for 24+ hours
        }
        public enum TypeOfInsulinInjection
        {
            NotSet = 0,
            CarbohydratesBolus = 10,
            CorrectionBolus = 20,
            ExtendedEffectBolus = 30,
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
