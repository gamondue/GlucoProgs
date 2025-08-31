using Android.App;
using Android.Runtime;
using GlucoMan;

namespace TestGlucoMan.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        
        // Configurazione base per i test
        builder.Services.AddSingleton<ISystemAlarmScheduler, GlucoMan.Maui.Platforms.Android.SystemAlarmScheduler>();
        
        return builder.Build();
    }
}