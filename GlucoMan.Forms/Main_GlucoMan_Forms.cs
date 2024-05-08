using gamon; 

namespace GlucoMan.Forms
{
    internal static class Main_GlucoMan_Forms
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();  // .Net 6 app bootstrap 

            Common.SetGlobalParameters();
            Common.GeneralInitializations();
            Common.PlatformSpecificInitializations();

            Application.Run(new frmMain());
        }
    }
}