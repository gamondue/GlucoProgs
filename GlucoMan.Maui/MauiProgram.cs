using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using WinUIWindow = Microsoft.UI.Xaml.Window;
using Microsoft.UI.Windowing;
using Microsoft.Maui.Platform;
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
                            appWindow.Resize(new Windows.Graphics.SizeInt32(650, 1141)); 
                        });});
#endif
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

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
            return builder.Build();
        }
    }
}
