using Android.App;
using Android.Runtime;

// Permessi GPS per tracking attività fisiche
[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]

// Permessi per tracking in background (Android 10+)
[assembly: UsesPermission(Android.Manifest.Permission.AccessBackgroundLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.ForegroundService)]
// Android 14+ richiede questo permesso specifico per servizi in foreground con GPS
[assembly: UsesPermission(Android.Manifest.Permission.ForegroundServiceLocation)]

namespace GlucoMan.Maui
{
    [Application]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
