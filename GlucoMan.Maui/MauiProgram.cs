using Microsoft.Extensions.Logging;

namespace GlucoMan.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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
