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

            CommonData.CommonObj = new CommonObjects();
            CommonData.CommonObj.LogOfProgram = new Logger(CommonData.PathProgramsData, true, "GlucoMan_log.txt",
                CommonData.PathProgramsData, CommonData.PathProgramsData, null, null);

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
