using System.IO;

namespace GlucoMan
{
    public static partial class Common
    {
        public static void PlatformSpecificInitializations()
        {
            // create the files that will be useful
            // (so they will not give access problems when non existing 

            if (!File.Exists(Path.Combine(Common.PathLogs, @"GlucoMan_log.txt")))
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_log.txt"));

            if (!File.Exists(Path.Combine(Common.PathLogs, @"GlucoMan_errors.txt")))
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_errors.txt"));

            if (!File.Exists(Path.Combine(Common.PathLogs, @"GlucoMan_prompts.txt")))
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_prompts.txt"));

            if (!File.Exists(Path.Combine(Common.PathLogs, @"GlucoMan_data.txt")))
                File.Create(Path.Combine(Common.PathLogs, @"GlucoMan_data.txt"));

            if (!File.Exists(PathAndFileLogOfParameters))
                File.Create(PathAndFileLogOfParameters);

            if (!File.Exists(Path.Combine(Common.PathExport, @"Logs")))
                File.Create(Path.Combine(Common.PathExport, @"Logs"));

            if (!File.Exists(Path.Combine(Common.PathExport, @"Logs")))
                File.Create(Path.Combine(Common.PathExport, @"Logs"));
        }
        public static void SetGeneralPaths()
        {
            // general paths to be used by the rest of the program 
            PathConfigurationData = Path.Combine(Common.LocalApplicationPath, @"Config");
            PathProgramsData = Path.Combine(Common.LocalApplicationPath, @"Data");
            PathLogs = Path.Combine(Common.LocalApplicationPath, @"Logs");
            PathDatabase = Path.Combine(Common.LocalApplicationPath, @"Data");
            PathAndFileDatabase = Path.Combine(Common.PathDatabase, Common.DatabaseFileName);
            PathExport = Path.Combine(Common.ExternalPublicPath, @"Glucoman");
            PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, LogOfParametersFileName);
        }
    }
}
