using FocusLightApp.Services;
using FocusLightApp.ViewModels;
using FocusLightApp.Views;
using Microsoft.Extensions.Logging;

namespace FocusLightApp;

/// <summary>
/// MAUI program builder and dependency injection configuration
/// </summary>
public static class MauiProgram
{
    /// <summary>
    /// Creates the MAUI application
    /// </summary>
    /// <returns>Configured MauiApp instance</returns>
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

        // Register services
        builder.Services.AddSingleton<IEEGSimulationService, EEGSimulationService>();

        // Register view models
        builder.Services.AddTransient<MainViewModel>();

        // Register views
        builder.Services.AddTransient<MainPage>();

        // Add logging
#if DEBUG
        builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        return builder.Build();
    }
}