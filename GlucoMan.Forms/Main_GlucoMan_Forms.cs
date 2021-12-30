using SharedData;

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
            ApplicationConfiguration.Initialize();

            Common.GeneralInitializations();
            Common.PlatformSpecificInitializations();

            Application.Run(new frmMain());
        }
    }
}