using AIService.Services.Knowledge;

namespace AIService.Background;

public sealed class KnowledgeIndexWorker
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<KnowledgeIndexWorker> _logger;


    public KnowledgeIndexWorker(
        IServiceScopeFactory scopeFactory,
        ILogger<KnowledgeIndexWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        try
        {
            using var scope =
                _scopeFactory.CreateScope();


            var indexer =
                scope.ServiceProvider
                    .GetRequiredService<IKnowledgeIndexer>();


            _logger.LogInformation(
                "Knowledge indexing started");


            await indexer.IndexAsync(
                stoppingToken);


            _logger.LogInformation(
                "Knowledge indexing completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Knowledge indexing failed");
        }
    }
}
