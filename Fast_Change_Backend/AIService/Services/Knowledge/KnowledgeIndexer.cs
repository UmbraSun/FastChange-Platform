using AIService.Models.Knowledge;
using AIService.Providers.OpenAI;

namespace AIService.Services.Knowledge;

public sealed class KnowledgeIndexer
    : IKnowledgeIndexer
{
    private readonly IKnowledgeLoader _loader;
    private readonly IChunkingService _chunkingService;
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly IVectorStore _vectorStore;

    public KnowledgeIndexer(
        IKnowledgeLoader loader,
        IChunkingService chunkingService,
        IEmbeddingProvider embeddingProvider,
        IVectorStore vectorStore)
    {
        _loader = loader;
        _chunkingService = chunkingService;
        _embeddingProvider = embeddingProvider;
        _vectorStore = vectorStore;
    }

    public async Task IndexAsync(
        CancellationToken cancellationToken)
    {
        var documents =
            await _loader.LoadAsync(cancellationToken);

        var vectors = new List<KnowledgeVector>();

        foreach (var document in documents)
        {
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
