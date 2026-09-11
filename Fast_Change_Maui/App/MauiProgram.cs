using App.Http;
using App.Services;
using CommunityToolkit.Maui;
using Core.Api.Auth;
using Core.Api.Users;
using Core.Interfaces;
using Core.Services;
using Microsoft.Extensions.Logging;
using Refit;
using System.Buffers.Text;
using UI.ViewModels;
using UI.Views;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        ConfigureServices(builder);
        ConfigureApi(builder);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void ConfigureServices(MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<ITokenStorage, SecureTokenStorage>();
        builder.Services.AddSingleton<IAuthState, AuthState>();
        builder.Services.AddSingleton<IAuthService, AuthService>();
        builder.Services.AddSingleton<IUserService, UserService>();

        builder.Services.AddSingleton<StartupNavigationService>();
        builder.Services.AddSingleton<AppShell>();

        builder.Services.AddTransient<AuthHandler>();

        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<RegisterViewModel>();
        builder.Services.AddTransient<RegisterPage>();

        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddTransient<WalletsViewModel>();
        builder.Services.AddTransient<WalletsPage>();
    }

    private static void ConfigureApi(MauiAppBuilder builder)
    {
        var baseUrl = GetApiBaseUrl();

        builder.Services.AddRefitClient<IAuthApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            });

        builder.Services.AddRefitClient<IUserApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(baseUrl);
            })
            .AddHttpMessageHandler<AuthHandler>();
    }

    private static string GetApiBaseUrl()
    {
        if (DeviceInfo.Platform == DevicePlatform.Android)
            return "https://10.0.2.2:7289";

        if (DeviceInfo.Platform == DevicePlatform.iOS)
            return "https://localhost:7289";

        if (DeviceInfo.Platform == DevicePlatform.MacCatalyst)
            return "https://localhost:7289";

        if (DeviceInfo.Platform == DevicePlatform.WinUI)
            return "https://localhost:7289";

        throw new PlatformNotSupportedException($"Platform '{DeviceInfo.Platform}' is not supported.");
    }
}