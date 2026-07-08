using AIService.Models.Knowledge;
using AIService.Providers.OpenAI;
using AIService.Services.VectorStore;

namespace AIService.Services.Retrieval;

public sealed class RetrievalService
    : IRetrievalService
{
    private readonly IEmbeddingProvider _embeddingProvider;
    private readonly IVectorStore _vectorStore;

    public RetrievalService(
        IEmbeddingProvider embeddingProvider,
        IVectorStore vectorStore)
    {
        _embeddingProvider = embeddingProvider;
        _vectorStore = vectorStore;
    }

    public async Task<IReadOnlyCollection<KnowledgeSearchResult>> SearchAsync(
        string question,
        int limit,
        CancellationToken cancellationToken)
    {
        var embedding =
            await _embeddingProvider.CreateEmbeddingAsync(
                question,
                cancellationToken);

        return await _vectorStore.SearchAsync(
            embedding,
            limit,
            cancellationToken);
    }
}
