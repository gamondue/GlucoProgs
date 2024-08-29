namespace GlucoMan.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // call method to set the paths of platform-specific folders
            Common.SetGlobalParameters();
            lblAppName.Text += " " + Common.Version;
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }
        private async void btnGlucoseMeasurement_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GlucoseMeasurementsPage(null));
        }
        private async void btnMeals_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MealsPage());
        }
        private async void btnNewMeal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MealPage(null));
        }
        private async void btnRecipes_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecipesPage());
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
        private async void btnAbout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage());
        }
        private async void btnInjections_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InjectionsPage(null));
        }
        private async void btnFoods_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FoodsPage(new Food()));
        }
    }
}
