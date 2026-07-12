using Application.Common.Behaviors;
using Application.Common.Interfaces;
using Application.Common.Services;
using Application.Common.Settings;
using Application.Features.Wallets.GetWalletHistory;
using BuildingBlocks.Messaging;
using Contracts.Events;
using Contracts.Notifications;
using Core.Infrastructure;
using FluentValidation;
using Infrastructure.BackgroundServices.Outbox;
using Infrastructure.ExchangeRates.Caching;
using Infrastructure.ExchangeRates.Clients;
using Infrastructure.ExchangeRates.Providers;
using Infrastructure.Messaging.Handlers;
using Infrastructure.Messaging.Kafka.Consumers;
using Infrastructure.Messaging.Kafka.DI;
using Infrastructure.Messaging.RabbitMq.Configuration;
using Infrastructure.Messaging.RabbitMq.Connection;
using Infrastructure.Messaging.RabbitMq.Publishers;
using Infrastructure.Mongo;
using Infrastructure.Mongo.Repositories;
using Infrastructure.Notifications;
using Infrastructure.Observability;
using Infrastructure.Redis;
using Infrastructure.SignalR;
using Infrastructure.SignalR.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using MongoDB.Driver;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistence;
using Persistence.Authentication;
using Persistence.Outbox;
using Persistence.Repositories;
using StackExchange.Redis;
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
        services.AddHttpConfiguration();
        services.Cors();
        services.RateLimiter();
        services.AddDatabase(configuration);
        services.AddMiddlewares();
        services.AddApplicationInfrastructure();
        services.AddUserLogin(builder.Configuration);
        services.AddInfrastructure(builder.Configuration);
        services.AddHostedServices(builder.Configuration);
        services.AddObservability(builder.Configuration);
        services.AddProjectServices(builder.Configuration);

        return builder;
    }

    // OpenAPI (Swagger) configuration
    private static void OpenApi(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();

                var bearerScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                };

                var securitySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = bearerScheme
                };
                document.Components.SecuritySchemes = securitySchemes;

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = new List<string>()
                };

                var operations = document.Paths?
                    .Values
                    .SelectMany(p => p.Operations?.Values ?? Enumerable.Empty<OpenApiOperation>())
                    ?? Enumerable.Empty<OpenApiOperation>();

                foreach (var operation in operations)
                    operation.Security ??= new List<OpenApiSecurityRequirement>() { securityRequirement };

                return Task.CompletedTask;
            });
        });
        services.AddEndpointsApiExplorer();
    }

    // HTTP context configuration
    private static void AddHttpConfiguration(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
    }

    // CORS configuration
    private static void Cors(this IServiceCollection services)
    {
        services.AddCors();
    }

    // Rate Limiter configuration
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

    // User login configuration
    private static void AddUserLogin(this IServiceCollection services, ConfigurationManager configuration)
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
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

    // Infrastructure services configuration
    private static void AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddCacheProvider(configuration);
        services.AddRabbitMq(configuration);
        services.AddKafka(configuration);
        services.AddSignalRConf();
        services.AddMongoDb(configuration);
    }

    // Cache provider configuration
    private static void AddCacheProvider(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<ExchangeRateSettings>(
            configuration.GetSection(ExchangeRateSettings.SectionName));

        services.AddHttpClient<FrankfurterClient>(client =>
        {
            var exchangeSettings = configuration.GetSection(ExchangeRateSettings.SectionName).Get<ExchangeRateSettings>()
                ?? throw new InvalidOperationException("ExchangeRateSettings configuration section is missing.");

            client.BaseAddress = new Uri(exchangeSettings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(exchangeSettings.TimeoutSeconds);
        });

        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));
        services.AddScoped<IExchangeRateCache, ExchangeRateRedisCache>();

        services.AddScoped<FrankfurterExchangeRateProvider>();
        services.AddScoped<IExchangeRateProvider>(sp =>
        {
            var inner = sp.GetRequiredService<FrankfurterExchangeRateProvider>();
            var cache = sp.GetRequiredService<IExchangeRateCache>();

            return new CachedExchangeRateProvider(inner, cache);
        });
    }

    // RabbitMQ configuration
    private static void AddRabbitMq(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<RabbitMqSettings>(
            configuration.GetSection(RabbitMqSettings.SectionName));

        services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
        services.AddSingleton<RabbitMqConnectionFactory>();
    }

    // SignalR configuration
    private static void AddSignalRConf(this IServiceCollection services)
    {
        services.AddSignalR();
        services.AddSingleton<IWalletNotificationService, WalletNotificationService>();
    }

    // MongoDB configuration
    private static void AddMongoDb(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<MongoSettings>(
            configuration.GetSection(MongoSettings.SectionName));

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings =
                sp.GetRequiredService<IOptions<MongoSettings>>().Value;

            return new MongoClient(settings.ConnectionString);
        });

        services.AddSingleton(sp =>
        {
            var settings =
                sp.GetRequiredService<IOptions<MongoSettings>>().Value;

            return sp.GetRequiredService<IMongoClient>()
                .GetDatabase(settings.DatabaseName);
        });

        services.AddScoped<IWalletHistoryReader, WalletHistoryRepository>();
        services.AddScoped<GetWalletHistoryHandler>();
    }

    // Add background hosted service
    private static void AddHostedServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddHostedService<OutboxDispatcher>();
        services.AddHostedService<ExchangeCompletedConsumer>();
        services.Configure<OutboxDispatcherOptions>(
            configuration.GetSection(OutboxDispatcherOptions.SectionName));
    }

    // Observability configuration
    private static void AddObservability(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelemetryOptions>(
            configuration.GetSection(
                TelemetryOptions.SectionName));

        var settings = configuration
            .GetSection(TelemetryOptions.SectionName)
            .Get<TelemetryOptions>()!;

        services.AddOpenTelemetry()
            .ConfigureResource(resource =>
            {
                resource.AddService(
                    settings.ServiceName,
                    serviceVersion: settings.ServiceVersion);
            })
            .WithTracing(builder =>
            {
                builder.AddSource(FastChangeTelemetry.SourceName)
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddConsoleExporter();
            })
            .WithMetrics(builder =>
            {
                builder.AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddConsoleExporter();
            });
    }

    // Application services adding
    private static void AddProjectServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<ApplicationDbContext>()
            .AddClasses(classes => classes.Where(t => t.Name.EndsWith("Repository")))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IOutboxStore, OutboxStore>();

        services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        services.AddScoped<IIntegrationEventHandler<ExchangeCompletedEvent>, ExchangeCompletedHandler>();

        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IWalletOperationService, WalletOperationService>();
        services.AddScoped<IWalletAccessService, WalletAccessService>();
        services.AddScoped<IExchangeService, ExchangeService>();

        services.AddScoped<IWalletNotificationService, WalletNotificationService>();
        services.AddSingleton<INotificationDispatcher, NotificationDispatcher>();
        services.AddSingleton<IExchangeNotificationChannel, SignalRNotificationChannel>();

        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IJwtTokenValidator, JwtTokenValidator>();
    }
}
