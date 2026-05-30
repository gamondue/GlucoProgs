using GlucoMan;

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

        //internal static DataLayer Database;
        //public static BL_General BlGeneral;
        //internal static BL_MealAndFood MealAndFood_CommonBL;

        public static string Version { get; private set; }

        public static double? breakfastStartHour = 6;
        public static double? breakfastEndHour = 10;
        public static double? lunchStartHour = 11;
        public static double? lunchEndHour = 15;
        public static double? dinnerStartHour = 17;
        public static double? dinnerEndHour = 21;

        /// <summary>
        /// Indicates whether alarms can be set. When true, alarm buttons are disabled.
        /// This is automatically set to true when user declares to have a continuous 
        /// glucose monitoring sensor.
        /// </summary>
        public static bool CantSetAlarms;

        public static double CurrentTimeZone { get; set; } = 0;

        /// <summary>
        /// Returns the current local date and time, compensating for the Android runtime
        /// limitation where DateTime.Now returns UTC. Uses the DST-aware UtcOffset stored
        /// in <see cref="CurrentTimeZone"/>.
        /// </summary>
        public static DateTime LocalNow => DateTime.UtcNow.AddHours(CurrentTimeZone);
    }
}
