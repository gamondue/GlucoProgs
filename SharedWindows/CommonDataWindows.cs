using gamon; 

namespace GlucoMan
{
    public static partial class Common
    {
        public static void SetGlobalParameters()
        {
            // program's default path and files. 
            PathUser = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //public static string PathExe = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            //public static string PathAndFileExe = System.Reflection.Assembly.GetExecutingAssembly().Location.Substring(8);
            // !!!! substitute the next with a more general way of finding the folder when MS will provide a Download folder
            //      in Environment paths 
            PathUsersDownload = System.Convert.ToString(
                            Microsoft.Win32.Registry.GetValue(
                                 @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders"
                                , "{374DE290-123F-4565-9164-39C4925E467B}"
                                , String.Empty));

            // general paths to be used by the rest of the program 
            PathConfigurationData = Path.Combine(PathUser, @"GlucoMan\Config\");
            PathProgramsData = Path.Combine(PathUser, @"GlucoMan\Data\");
            PathLogs = Path.Combine(PathUser, @"GlucoMan\Logs\");
            PathDatabase = Path.Combine(PathUser, @"GlucoMan\Data\");
            PathAndFileDatabase = Path.Combine(PathDatabase, DatabaseFileName);
            PathImportExport = Path.Combine(PathUser, @"GlucoMan\ImportExport\");
            PathAndFileLogOfParameters = Path.Combine(Common.PathLogs, Common.LogOfParametersFileName);
    }
    //public static Color InputColor = Color.PaleGreen;
    //public static Color InputSlowChangingColor = Color.PaleGoldenrod;
    //public static Color ToBeInputColor = Color.Red;
    //public static Color CalculatedImportantColor = Color.Yellow;
    //public static Color CalculatedNonImportantColor = Color.White;
    //public static Color ProgramsResult = Color.SkyBlue;
    }
}
