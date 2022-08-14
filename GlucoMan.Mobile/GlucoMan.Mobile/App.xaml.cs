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
            Common.PlatformSpecificInitializations();
            Common.GeneralInitializations();
        }
        protected override void OnSleep()
        {
        }
        protected override void OnResume()
        {
        }
    }
}
