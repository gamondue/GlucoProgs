using gamon;
using GlucoMan.Maui.Services;

namespace GlucoMan.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Initialize LocalizationService BEFORE any pages are loaded
            // This ensures that static string bindings in XAML use the correct culture
            var localizationService = Handler?.MauiContext?.Services?.GetService<LocalizationService>();
            if (localizationService == null)
            {
                // If service is not available yet, create a temporary instance to set initial culture
                var tempService = new LocalizationService();
                // Culture is already set in LocalizationService constructor from Preferences
            }

            // logging of non managed exceptions
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                var ex = e.ExceptionObject as Exception;
                General.LogOfProgram.Error("App.xaml.cs | Constructor, regular exception", ex);
            };
            // logging of non observed exceptions in async tasks
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                var ex = e.Exception;
                // command to avoid propagation of error
                e.SetObserved();
                General.LogOfProgram.Error("App.xaml.cs | Constructor, non observed exception", ex);
            };
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}