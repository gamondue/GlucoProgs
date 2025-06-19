using GlucoMan.MVVM.Models;
using GlucoMan.MVVM.PageModels;

namespace GlucoMan.MVVM.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageModel model)
        {
            InitializeComponent();
            BindingContext = model;
        }
        private async void btnAbout_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///about");
        }
        private void btnGlucoseMeasurement_Clicked(object sender, EventArgs e)
        {

        }

        private void btnInjections_Clicked(object sender, EventArgs e)
        {

        }

        private void btnMeals_Clicked(object sender, EventArgs e)
        {

        }

        private void btnNewMeal_Clicked(object sender, EventArgs e)
        {

        }

        private void btnInsulinCalc_Clicked(object sender, EventArgs e)
        {

        }

        private void btnFoods_Clicked(object sender, EventArgs e)
        {

        }

        private void btnRecipes_Clicked(object sender, EventArgs e)
        {

        }

        private void btnFoodToHitTargetCarbs_Clicked(object sender, EventArgs e)
        {

        }

        private void btnHypoPrediction_Clicked(object sender, EventArgs e)
        {

        }

        private void btnMiscellaneousFunctions_Clicked(object sender, EventArgs e)
        {

        }

        private void btnConfigurations_Clicked(object sender, EventArgs e)
        {

        }

        private void btnAlarms_Clicked(object sender, EventArgs e)
        {

        }
    }
}