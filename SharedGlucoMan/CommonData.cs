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

        /// <summary>
        /// UTC offset (in hours, DST included) accepted by the user, in force now. It is the
        /// offset of every local time of the program (<see cref="LocalNow"/>) and the one saved
        /// with the records (see <see cref="GetActualUtcOffsetForLocalTime"/>).
        /// It changes only when the user accepts a detected change (see TimeZoneCheckService)
        /// or sets it in the Settings page.
        /// </summary>
        public static double CurrentTimeZone { get; set; } = 0;

        /// <summary>
        /// Function that returns the UTC offset (in hours, DST included) of the configured
        /// country at a given UTC instant, or null when no country is configured (offset set
        /// by hand). Only its DST changes are used, see <see cref="GetConfiguredUtcOffsetAt"/>.
        /// </summary>
        public static Func<DateTime, double>? ConfiguredZoneOffsetProvider { get; set; }

        /// <summary>
        /// UTC offset (in hours) of the accepted time zone at the given UTC instant.
        /// While the accepted offset <see cref="CurrentTimeZone"/> is the one of the configured
        /// country now, the country's DST rules give the offset of that instant (e.g. Italy
        /// accepted at +1 in winter gives +2 for a summer date). Otherwise (no country, or a
        /// DST change declined by the user) the accepted offset is used for every instant.
        /// </summary>
        public static double GetConfiguredUtcOffsetAt(DateTime utcInstant)
        {
            var provider = ConfiguredZoneOffsetProvider;
            if (provider == null)
                return CurrentTimeZone;
            try
            {
                if (provider(DateTime.UtcNow) != CurrentTimeZone)
                    return CurrentTimeZone;
                return provider(utcInstant);
            }
            catch
            {
                return CurrentTimeZone;
            }
        }

        /// <summary>
        /// Function that returns the UTC offset (in hours, DST included) that the device
        /// detects at a given UTC instant. Set by the platform layer from the OS time zone.
        /// Used only to detect a change of time zone, never for the times of the program.
        /// </summary>
        public static Func<DateTime, double>? DetectedUtcOffsetProvider { get; set; }

        /// <summary>
        /// UTC offset detected now by the device, or <see cref="CurrentTimeZone"/> if the
        /// platform provided no detection.
        /// </summary>
        public static double GetDetectedUtcOffset()
        {
            var provider = DetectedUtcOffsetProvider;
            if (provider == null)
                return CurrentTimeZone;
            try
            {
                return provider(DateTime.UtcNow);
            }
            catch
            {
                return CurrentTimeZone;
            }
        }

        /// <summary>
        /// UTC offset (in hours, DST included) of the accepted time zone in force at the given
        /// local time, so that a record dated before or after a DST change of the configured
        /// country gets the offset of its own date (e.g. Italy: +2 in summer, +1 in winter).
        /// This is the offset saved with every record, together with the local time:
        /// UTC = local time - offset, where the local time is that of <see cref="LocalNow"/>.
        /// When localTime is null the offset in force now is returned.
        /// </summary>
        public static double GetActualUtcOffsetForLocalTime(DateTime? localTime)
        {
            double offsetNow = CurrentTimeZone;
            if (localTime == null || localTime.Value == default)
                return offsetNow;
            DateTime local = DateTime.SpecifyKind(localTime.Value, DateTimeKind.Unspecified);
            // first guess with the offset in force now, then refine with the offset of that instant
            double offset = GetConfiguredUtcOffsetAt(local.AddHours(-offsetNow));
            return GetConfiguredUtcOffsetAt(local.AddHours(-offset));
        }

        /// <summary>
        /// Decides whether the user must be asked about a UTC offset change: only when the
        /// detected offset differs from the configured one and is not the same one the user
        /// already declined, so the question is repeated only on a new change.
        /// </summary>
        public static bool ShouldAskAboutUtcOffsetChange(double detectedOffset, double configuredOffset,
            double? lastDeclinedOffset)
        {
            if (detectedOffset == configuredOffset)
                return false;
            return lastDeclinedOffset == null || lastDeclinedOffset.Value != detectedOffset;
        }

        /// <summary>
        /// Returns the current local date and time in the accepted time zone
        /// (<see cref="CurrentTimeZone"/>), not that of the device clock, which can be set to
        /// another zone. It is also independent of the Android runtime limitation where
        /// DateTime.Now returns UTC, and consistent with the offset saved by
        /// <see cref="GetActualUtcOffsetForLocalTime"/>.
        /// </summary>
        public static DateTime LocalNow => DateTime.UtcNow.AddHours(CurrentTimeZone);
    }
}
