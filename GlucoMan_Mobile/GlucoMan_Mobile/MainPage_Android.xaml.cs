using System;
using Xamarin.Forms;

namespace GlucoMan_Mobile
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
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
    }
}
