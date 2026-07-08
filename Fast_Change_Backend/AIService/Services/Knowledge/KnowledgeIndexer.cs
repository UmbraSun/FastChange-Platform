using AIService.Models.Knowledge;
using AIService.Providers.OpenAI;
using AIService.Services.VectorStore;

namespace AIService.Services.Knowledge;

public sealed class KnowledgeIndexer
    : IKnowledgeIndexer
{
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly IKnowledgeLoader _loader;
    private readonly IChunkingService _chunkingService;
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly IVectorStore _vectorStore;
    private readonly ILogger<KnowledgeIndexer> _logger;

    public KnowledgeIndexer(
        IKnowledgeLoader loader,
        IChunkingService chunkingService,
        IEmbeddingProvider embeddingProvider,
        IVectorStore vectorStore,
        ILogger<KnowledgeIndexer> logger)
    {
        _loader = loader;
        _chunkingService = chunkingService;
        _embeddingProvider = embeddingProvider;
        _vectorStore = vectorStore;
        _logger = logger;
    }

    public async Task IndexAsync(
        CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Starting knowledge indexing");

        await _lock.WaitAsync(cancellationToken);

        try
        {
            await IndexInternalAsync(
                cancellationToken);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task IndexInternalAsync(
        CancellationToken cancellationToken)
    {
        var documents =
            await _loader.LoadAsync(cancellationToken);

        _logger.LogInformation(
            "Loaded {Count} documents",
            documents.Count);

        var vectors = new List<KnowledgeVector>();

        foreach (var document in documents)
        {
            _logger.LogInformation(
                "Indexed document {Document}",
                document.Name);
            
            if (await _vectorStore.ExistsAsync(
                    document.Name,
                    document.Hash,
                    cancellationToken))
                continue;

            var chunks =
                _chunkingService.Chunk(document);

            foreach (var chunk in chunks)
            {
                var embedding =
                    await _embeddingProvider.CreateEmbeddingAsync(
                        chunk.Content,
                        cancellationToken);

                vectors.Add(
                    new KnowledgeVector(
                        chunk.Id,
                        chunk.DocumentName,
                        document.Hash,
                        chunk.Heading,
                        chunk.Content,
                        embedding));
            }
        }

        await _vectorStore.UpsertAsync(
            vectors,
            cancellationToken);
    }
}
