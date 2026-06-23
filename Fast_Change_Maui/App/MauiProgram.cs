using CommunityToolkit.Maui;
using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.Logging;
using UI.Services;
using UI.ViewModels;
using UI.Views;

namespace Core
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
            {
                client.BaseAddress = new Uri(DeviceInfo.Platform == DevicePlatform.Android
                    ? "http://10.0.2.2:5000"
                    : "https://localhost:7001");
            });

            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<DashboardPage>();

            builder.Services.AddSingleton<IAlertService, AlertService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
