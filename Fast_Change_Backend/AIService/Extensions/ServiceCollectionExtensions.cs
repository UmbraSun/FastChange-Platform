using AIService.AI.Options;
using AIService.Infrastructure;
using AIService.Providers.OpenAI;
using AIService.Services.Knowledge;
using Microsoft.Extensions.Options;
using OpenAI;
using Qdrant.Client;

namespace AIService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<OpenAiOptions>(
            configuration.GetSection(OpenAiOptions.SectionName));

        services.Configure<QdrantOptions>(
            configuration.GetSection(QdrantOptions.SectionName));

        services.AddSingleton(sp =>
        {
            var options =
                sp.GetRequiredService<IOptions<OpenAiOptions>>().Value;

            return new OpenAIClient(options.ApiKey);
        });

        services.AddSingleton(sp =>
        {
            var options =
                sp.GetRequiredService<IOptions<QdrantOptions>>()
                    .Value;

            return new QdrantClient(
                options.Host,
                options.Port);
        });

        services.AddSingleton<QdrantInitializer>();
        services.AddScoped<IChatProvider, OpenAiChatProvider>();
        services.AddScoped<IEmbeddingProvider, OpenAiEmbeddingProvider>();
        services.AddScoped<IKnowledgeLoader, FileKnowledgeLoader>();
        services.AddScoped<IChunkingService, MarkdownChunkingService>();
        services.AddScoped<IKnowledgeIndexer, KnowledgeIndexer>();

        return services;
    }
}
