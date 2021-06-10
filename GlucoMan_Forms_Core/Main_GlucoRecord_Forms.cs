using SharedData;
using SharedFunctions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GlucoMan_Forms_Core
{
    static class GlucoRecord_Forms_Main
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
