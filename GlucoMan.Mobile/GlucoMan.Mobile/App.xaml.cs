using SharedData;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new NavigationPage(new LoginPage());
            MainPage = new NavigationPage(new AboutPage());
        }
        protected override void OnStart()
        {
            Common.GeneralInitializations();
            Common.PlatformSpecificInitializations();
            Common.LogOfProgram = new Logger(Common.PathLogs, true, @"logs\GlucoMan_log.txt",
                @"logs\GlucoMan_errors.txt", @"logs\GlucoMan_debug.txt", @"logs\GlucoMan_prompts.txt",
                @"logs\GlucoMan_data.txt");
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
    }
}
