using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using gamon;
using GlucoMan; // for ISystemAlarmScheduler
using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;
using GlucoMan.Maui.Services; // Add localization service
#if WINDOWS
using WinUIWindow = Microsoft.UI.Xaml.Window;
using Microsoft.UI.Windowing;
using Microsoft.Maui.Platform;
using GlucoMan.Maui.Platforms.Windows;
#endif
#if ANDROID
using GlucoMan.Maui.Platforms.Android;
#endif

namespace GlucoMan.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseSkiaSharp()
                .ConfigureLifecycleEvents(events =>
                {
#if WINDOWS
                    // in Windows give the window dimensions like those of smartphones
                    events.AddWindows(w =>
                    {
                        w.OnWindowCreated(window =>
                        {
                            var mauiWinUIWindow = (WinUIWindow)window;
                            var hwnd = mauiWinUIWindow.GetWindowHandle();
                            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hwnd);
                            var appWindow = AppWindow.GetFromWindowId(windowId);
                            appWindow.Resize(new Windows.Graphics.SizeInt32(400, 950));
                        });
                    });
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register Localization Service as Singleton
            builder.Services.AddSingleton<LocalizationService>();

#if ANDROID
            builder.Services.AddSingleton<ISystemAlarmScheduler, GlucoMan.Maui.Platforms.Android.SystemAlarmScheduler>();
#elif WINDOWS
            builder.Services.AddSingleton<ISystemAlarmScheduler, GlucoMan.Maui.Platforms.Windows.SystemAlarmScheduler>();
#else
            builder.Services.AddSingleton<ISystemAlarmScheduler, DummyAlarmScheduler>();
#endif

#if DEBUG
            builder.Logging.AddDebug();
            builder.Services.AddLogging(configure =>
            {
                configure.AddDebug();
                configure.SetMinimumLevel(LogLevel.Trace);
            });
#endif
            Common.SetGlobalParameters();
            Common.GeneralInitializationsAsync();

            General.LogOfProgram = new Logger(Common.PathLogs, true,
                @"GlucoMan_Log.txt",
                @"GlucoMan_Errors.txt",
                @"GlucoMan_Debug.txt",
                @"GlucoMan_Prompts.txt",
                @"GlucoMan_Data.txt");
            return builder.Build();
        }
    }
    // Fallback dummy implementation of the alarm scheduler, for unsupported platforms
    public class DummyAlarmScheduler : ISystemAlarmScheduler
    {
        public Task ScheduleAsync(Alarm alarm) => Task.CompletedTask;
        public Task CancelAsync(int idAlarm) => Task.CompletedTask;
        public Task CancelAllAsync() => Task.CompletedTask;
    }
}
