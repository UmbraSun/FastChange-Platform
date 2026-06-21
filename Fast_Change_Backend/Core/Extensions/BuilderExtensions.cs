using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Common.Settings;
using Core.Infrastructure;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Authentication;
using System.Text;
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
        services.AddMiddlewares();
        services.AddApplicationInfrastructure();
        services.AddUserLogin(builder.Configuration);
        services.AddApplicationServices(builder.Configuration);

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

    // Add middlewares to the DI container
    private static void AddMiddlewares(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    // Application infrastructure configuration
    private static void AddApplicationInfrastructure(this IServiceCollection services)
    {
        var assembly = typeof(Application.AssemblyReference).Assembly;
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(assembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(assembly);
    }

    private static void AddUserLogin(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings!.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
            };
        });

    }

    // Application services adding
    private static void AddApplicationServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.AssemblyReference).Assembly));
        services.AddValidatorsFromAssembly(typeof(Application.AssemblyReference).Assembly);

        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationDbContext>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();
    }
}
