using Core.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Runtime.CompilerServices;
using System.Threading.RateLimiting;

namespace Core.Extensions;

public static class BuilderExtensions
{
    public static WebApplicationBuilder AddInfrastructureServices(
        this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        var services = builder.Services;
        services.OpenApi();
        services.Cors();
        services.RateLimiter();
        services.AddDatabase(configuration);
        services.AddApplicationServices();
        services.AddMiddlewares();

        return builder;
    }

    // OpenAPI (Swagger) configuration
    private static void OpenApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
    }

    private static void Cors(this IServiceCollection services)
    {
        services.AddCors();
    }

    private static void RateLimiter(this IServiceCollection services)
    {
        // Highload optimization: Registers and configures global Rate Limiting services
        services.AddRateLimiter(options =>
        {
            // Global policy: Fixed window of 10 seconds, max 100 requests per IP address
            options.AddFixedWindowLimiter(policyName: "GlobalFixedWindow", fixedOptions =>
            {
                fixedOptions.PermitLimit = 100;
                fixedOptions.Window = TimeSpan.FromSeconds(10);
                fixedOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                fixedOptions.QueueLimit = 2;
            });

            // Returns a strict 429 Too Many Requests status code when a user floods the API
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
    }

    // Database configuration
    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly(nameof(Persistence));
                // Highload optimization: Configures automatic query retries 
                // if PostgreSQL experiences a transient failure or connection drop
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorCodesToAdd: null);
            }));
    }

    // Application services adding
    private static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
        services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationDbContext>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());
    }

    // Add middlewares to the DI container
    private static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }
}
