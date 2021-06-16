using SharedData;
using SharedFunctions;
using System;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    static class GlucoMan_Forms_Main
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CommonFunctions.Initializations(); 

            Application.Run(new frmMain());
        }
    }
}
