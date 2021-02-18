using System;
using Xamarin.Forms;

namespace GlucoMan_Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            lblAppName.Text += " " + version;
        }

        private async void btnWeighFood_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeighFood_Page());
        }

        private async void btnInsuline_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InsulineCalc_Page());
        }

        private async void btnHypoPrediction_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new HypoPrediction_Page());
        }

        private async void btnFoodToHitTargetCarbs_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FoodToHitTargetCarbs_Page());
        }
    }
}
