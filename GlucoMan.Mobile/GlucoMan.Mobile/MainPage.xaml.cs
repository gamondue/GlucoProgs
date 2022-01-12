using GlucoMan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GlucoMan.Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            Common.GeneralInitializations();
            Common.PlatformSpecificInitializations();

            lblAppName.Text += " " + Common.Version;
        }
        private async void btnGlucoseMeasurement_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GlucoseMeasurementPage());
        }
        private async void btnChoCount_Clicked(object sender, EventArgs e)
        {
            // !!!! TODO 
        }
        private async void btnWeighFood_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeighFoodPage());
        }
        private async void btnInsulinCalc_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InsulinCalcPage());
        }
        private async void btnFoodToHitTargetCarbs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FoodToHitTargetCarbsPage());
        }
        private async void btnHypoPrediction_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HypoPredictionPage());
        }
        private async void btnAlarms_Clicked(object sender, EventArgs e)
        {
            // !!!! TODO 
        }
        private async void btnMiscellaneousFunctions_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MiscellaneousFunctionsPage());
        }
        private async void btnAbout_Clicked (object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
    }
}
