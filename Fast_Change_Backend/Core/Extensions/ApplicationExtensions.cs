using Core.Infrastructure;
using Infrastructure.SignalR.Hubs;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Scalar.AspNetCore;

namespace Core.Extensions;

public static class ApplicationExtensions
{
    public static WebApplication UseInfrastructurePipeline(this WebApplication app)
    {
        app.OpenApi();
        app.HttpConfigs();
        app.ApiConfigs();
        app.SecurityConfigs();
        app.HighLoadConfigs();
        app.UseMiddlewares();
        app.AddHubs();
        app.ApplyDatabaseMigrations();

        return app;
    }

    // OpenAPI (Swagger) configuration
    private static void OpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("FastChange Highload API")
                       .WithTheme(ScalarTheme.DeepSpace) // Dark fintech theme
                       .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }
    }

    // http pipeline configuration
    private static void HttpConfigs(this WebApplication app)
    {
        app.UseHttpsRedirection();
    }

    // API configuration
    private static  void ApiConfigs(this WebApplication app)
    {
        app.UseRouting();
        app.MapControllers();
        app.UseCors();
    }

    // Security configuration
    private static void SecurityConfigs(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    // High load configuration
    private static void HighLoadConfigs(this WebApplication app)
    {
        app.UseRateLimiter();
    }

    // Use middlewares
    private static void UseMiddlewares(this WebApplication app)
    {
        app.UseExceptionHandler();
    }

    // Add SignalR hubs to the application
    private static void AddHubs(this WebApplication app)
    {
        app.MapHub<WalletHub>("/hubs/wallet");
    }

    // Automatically applies pending Entity Framework Core migrations on application startup
    private static void ApplyDatabaseMigrations(this WebApplication app)
    {
        // Highload/Production optimization: Use a temporary isolated Dependency Injection scope 
        // to safely retrieve and dispose of the DbContext instance.
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;
        var config = services.GetRequiredService<IConfiguration>();
        var logger = services.GetRequiredService<ILogger<WebApplication>>();

        var conn = config.GetConnectionString("DefaultConnection");
        if (string.IsNullOrWhiteSpace(conn))
        {
            logger.LogError("Connection string 'DefaultConnection' is not configured. Aborting migrations.");
            throw new InvalidOperationException("Missing connection string 'DefaultConnection'.");
        }

        var context = services.GetRequiredService<ApplicationDbContext>();

        // Optional double-check against the resolved DbConnection
        var dbConnection = context.Database.GetDbConnection();
        if (dbConnection == null || string.IsNullOrWhiteSpace(dbConnection.ConnectionString))
        {
            logger.LogError("DbContext's DbConnection.ConnectionString is empty.");
            throw new InvalidOperationException("DbContext connection string is not initialized.");
        }

        context.Database.Migrate();
    }
}
