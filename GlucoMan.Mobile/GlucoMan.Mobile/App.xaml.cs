using SharedData;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GlucoMan.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Common.LogOfProgram = new Logger(Common.PathProgramsData, true, @"logs\GlucoMan_log.txt",
                @"logs\GlucoMan_errors.txt", @"logs\GlucoMan_debug.txt", @"logs\GlucoMan_prompts.txt",
                @"logs\GlucoMan_data.txt");

            //MainPage = new NavigationPage(new LoginPage());
            MainPage = new NavigationPage(new AboutPage());
        }
        protected override void OnStart()
        {
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
    }
}
