using gamon;
using Microsoft.Maui.Controls;
using System;

namespace GlucoMan.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // change the title of the page shown in the navigation bar, showing the version of the program
            Title += " " + Common.Version;

            RequestPermissionsIfNotGiven().ConfigureAwait(false);
            Thread.Sleep(5000);
         
            Common.SetGlobalParameters();

            // restore parameters (TODO move these to SetGlobalParameters, making RestoreParameter static)
            Common.breakfastStartHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Breakfast_StartTime_Hours"));
            Common.breakfastEndHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Breakfast_EndTime_Hours"));
            Common.lunchStartHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Lunch_StartTime_Hours"));
            Common.lunchEndHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Lunch_EndTime_Hours"));
            Common.dinnerStartHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Dinner_StartTime_Hours"));
            Common.dinnerEndHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Dinner_EndTime_Hours"));
            //Common.GeneralInitializationsAsync();
            //Common.PlatformSpecificInitializations();
        }
        private async Task RequestPermissionsIfNotGiven()
        {
            // request permissions if not already given
            var PermissionStorageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
            if (PermissionStorageRead != PermissionStatus.Granted)
            {
                PermissionStorageRead = await Permissions.RequestAsync<Permissions.StorageRead>();
            }
            var PermissionStorageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (PermissionStorageWrite != PermissionStatus.Granted)
            {
                PermissionStorageWrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
            //var PermissionAlarm = await Permissions.CheckStatusAsync<Permissions.Alarm>();
            //if (PermissionAlarm != PermissionStatus.Granted)
            //{
            //    PermissionAlarm = await Permissions.RequestAsync<Permissions.Alarm>();
            //}
            //var PermissionManageStorage = await Permissions.CheckStatusAsync<Permissions.StorageManagement>();
            //if (PermissionManageStorage != PermissionStatus.Granted)
            //{
            //    PermissionManageStorage = await Permissions.RequestAsync<Permissions.StorageManagement>();
            //}
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
            await Navigation.PushAsync(new RecipesPage(null));
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
            await Navigation.PushAsync(new FoodsPage(new Food(new Unit("g", 1))));
        }
        private void btnConfigurations_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ConfigurationPage());
            Navigation.PushAsync(new SettingsPage());
        }
    }
}
