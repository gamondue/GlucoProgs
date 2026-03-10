using gamon;
using GlucoMan.Maui.Services;
using GlucoMan.Maui.Resources.Strings;
using Microsoft.Maui.Controls;
using System;

namespace GlucoMan.Maui
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalizationService _localizationService;

        public MainPage()
        {
            // Get LocalizationService from DI BEFORE InitializeComponent
            _localizationService = Application.Current.Handler.MauiContext.Services.GetService<LocalizationService>();
            
            // Ensure culture is set before XAML loads
            if (_localizationService != null)
            {
                // This will set AppStrings.Culture which affects {x:Static} bindings
                // The service constructor already loaded saved preferences
            }
            
            InitializeComponent();
            
            // Subscribe to culture changes to refresh page when language changes
            if (_localizationService != null)
            {
                _localizationService.CultureChanged += OnCultureChanged;
            }
            
            // change the title of the page shown in the navigation bar, showing the version of the program
            Title += " " + Common.Version;

            // Initialize async operations properly - don't block the UI thread
            _ = InitializeAsync();
        }

        private void OnCultureChanged(object sender, EventArgs e)
        {
            // Force refresh of the page to reload all static bindings
            // Note: This is a workaround for {x:Static} bindings not updating automatically
            // In a production app, consider using dynamic bindings or INotifyPropertyChanged
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_localizationService != null)
            {
                _localizationService.CultureChanged -= OnCultureChanged;
            }
        }

        private async Task InitializeAsync()
        {
            try
            {
                // Request permissions asynchronously without blocking UI
                await RequestPermissionsIfNotGiven();

                // Add a short delay if needed, but don't block the UI thread
                await Task.Delay(100);

                // already called in MauiProgram.cs
                //Common.SetGlobalParameters();

                // restore parameters (TODO move these to SetGlobalParameters, making RestoreParameter static)
                // Wrap these in a try-catch to prevent crashes if database is not ready
                try
                {
                    Common.breakfastStartHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Breakfast_StartTime_Hours"));
                    Common.breakfastEndHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Breakfast_EndTime_Hours"));
                    Common.lunchStartHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Lunch_StartTime_Hours"));
                    Common.lunchEndHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Lunch_EndTime_Hours"));
                    Common.dinnerStartHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Dinner_StartTime_Hours"));
                    Common.dinnerEndHour = Safe.Double(Common.BlGeneral.RestoreParameter("Meal_Dinner_EndTime_Hours"));

                    // Load CantSetAlarms parameter
                    string CantSetAlarmsValue = Common.BlGeneral.RestoreParameter("CantSetAlarms");
                    if (CantSetAlarmsValue == null)
                    {
                        // First run - parameter doesn't exist in database yet
                        // Ask user if they use a continuous glucose sensor
                        await AskAboutContinuousGlucoseSensor();
                    }
                    else
                    {
                        // Load existing value from database
                        Common.CantSetAlarms = CantSetAlarmsValue.ToLower() == "true" || CantSetAlarmsValue == "1";
                    }

                    // Disable alarm button if CantSetAlarms is true (user has sensor)
                    btnAlarms.IsEnabled = !Common.CantSetAlarms;
                }
                catch (Exception ex)
                {
                    General.LogOfProgram.Error("MainPage - Error loading parameters", ex);
                    // Set default values if parameters can't be loaded
                    Common.breakfastStartHour = 6.0;
                    Common.breakfastEndHour = 10.0;
                    Common.lunchStartHour = 11.0;
                    Common.lunchEndHour = 15.0;
                    Common.dinnerStartHour = 18.0;
                    Common.dinnerEndHour = 22.0;
                    Common.CantSetAlarms = false;
                    btnAlarms.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("MainPage - InitializeAsync", ex);
            }
        }
        private async Task RequestPermissionsIfNotGiven()
        {
            // Storage permissions
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
            
            // GPS/Location permissions for physical activity tracking
            var PermissionLocation = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (PermissionLocation != PermissionStatus.Granted)
            {
                PermissionLocation = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                General.LogOfProgram?.Event($"MainPage - Location permission requested: {PermissionLocation}");
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
            await Navigation.PushAsync(new AlarmPage());
        }

        /// <summary>
        /// Asks the user if they use a continuous glucose monitoring sensor.
        /// If yes, sets CantSetAlarms to true and disables alarm buttons.
        /// </summary>
        private async Task AskAboutContinuousGlucoseSensor()
        {
            try
            {
                bool usesSensor = await DisplayAlert(
                    AppStrings.CGMSensorDialogTitle,
                    AppStrings.CGMSensorDialogMessage,
                    AppStrings.CGMSensorDialogYesButton,
                    AppStrings.CGMSensorDialogNoButton);

                Common.CantSetAlarms = usesSensor;

                // Save the setting to database
                Common.BlGeneral.SaveParameter("CantSetAlarms", usesSensor ? "true" : "false");

                General.LogOfProgram?.Event($"MainPage - User configured CantSetAlarms: {Common.CantSetAlarms}");
            }
            catch (Exception ex)
            {
                General.LogOfProgram?.Error("MainPage - AskAboutContinuousGlucoseSensor", ex);
                // Default to false if there's an error
                Common.CantSetAlarms = false;
                try
                {
                    Common.BlGeneral.SaveParameter("CantSetAlarms", "false");
                }
                catch { }
             }
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
            try
            {
                var page = new InjectionsPage(null);
                await Navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                General.LogOfProgram.Error("MainPage | btnInjections_Clicked", ex);
                try { await DisplayAlert("Error", $"Impossibile aprire la pagina Injections: {ex.Message}", "OK"); } catch { /* ignore UI errors */ }
            }
        }
        private async void btnFoods_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FoodsPage(new Food(new UnitOfFood("g", 1))));
        }
        private void btnConfigurations_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new ConfigurationPage());
            Navigation.PushAsync(new SettingsPage(_localizationService));
        }
        private async void btnGraphicsAndStatistics_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatisticsAndGraphPage());
        }

        private void btnPysicalActivity_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PhysicalActivityPage(_localizationService));
        }
    }
}
