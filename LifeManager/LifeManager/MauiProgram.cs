using LifeManager.Interfaces;
using LifeManager.Services;
using Microsoft.Extensions.Logging;
using LifeManager.ViewModels;

namespace LifeManager
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

#endif
            builder.Services.AddHttpClient<IWeatherService, WeatherService>();
            // whenever a class requests IWeatherService, provide a WeatherService instance
            builder.Services.AddHttpClient<ITodoService, TodoService>();
            builder.Services.AddHttpClient<ICategoryService, CategoryService>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<EditTaskPage>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<EditTaskViewModel>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<SettingsPage>();

            //the DI container creates a new MainPage instance whenever one is requested

            return builder.Build();
        }
    }
}
