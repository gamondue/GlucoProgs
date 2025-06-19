using gamon;

namespace GlucoMan.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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