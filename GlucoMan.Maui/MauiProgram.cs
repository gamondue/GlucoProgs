using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using gamon;
using GlucoMan; // for ISystemAlarmScheduler
using CommunityToolkit.Maui;
using SkiaSharp.Views.Maui.Controls.Hosting;
using ZXing.Net.Maui.Controls;
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
                .UseBarcodeReader()
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

            // Register GPS tracking service based on platform
            builder.Services.AddSingleton<IGeoTimeZoneService, GeoTimeZoneService>();
#if ANDROID
            builder.Services.AddSingleton<IBackgroundGpsService, BackgroundGpsServiceAndroid>();
            builder.Services.AddSingleton<ISystemAlarmScheduler, GlucoMan.Maui.Platforms.Android.SystemAlarmScheduler>();
#elif WINDOWS
#if DEBUG
            // Inject mock GPS to simulate geographic movement during Windows debug sessions.
            // Change waypoints in MockGeolocation.cs to match the route you want to simulate.
            // To use real GPS, comment out the next line (MockGeolocation) and uncomment the one after.
            builder.Services.AddSingleton<IGeolocation, MockGeolocation>();
            //builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default);
#else
            builder.Services.AddSingleton<IGeolocation>(_ => Geolocation.Default);
#endif
            builder.Services.AddSingleton<IBackgroundGpsService, DefaultBackgroundGpsService>();
            builder.Services.AddSingleton<ISystemAlarmScheduler, GlucoMan.Maui.Platforms.Windows.SystemAlarmScheduler>();
#else
            builder.Services.AddSingleton<IBackgroundGpsService, DefaultBackgroundGpsService>();
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
            
            // Initialize the DatabaseService singleton (moved from CommonFunctions.cs)
            DatabaseService.Instance.Initialize(Common.PathAndFileDatabase);

            // Initialize CurrentTimeZone from the OS local timezone (includes DST on all platforms).
            // This is always the authoritative source. The value stored in Parameters is updated
            // later by TimeZoneCheckService (GPS-based) but must never override the OS value at
            // startup, because the stored value may be stale (e.g., saved before DST change).
            // On Android, TimeZoneInfo.Local may return UTC+0 depending on the runtime, so we use
            // the native Java TimeZone API which is always reliable and includes DST.
            try
            {
#if ANDROID
                var javaDefaultTz = Java.Util.TimeZone.Default;
                // getOffset returns milliseconds including DST for the current instant
                long nowMs = Java.Lang.JavaSystem.CurrentTimeMillis();
                int offsetMs = javaDefaultTz.GetOffset(nowMs);
                Common.CurrentTimeZone = Math.Round(offsetMs / 3600000.0, 2);
#else
                Common.CurrentTimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;
#endif
            }
            catch
            {
                Common.CurrentTimeZone = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;
            }

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
