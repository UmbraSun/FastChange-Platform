using AIService.Models.Knowledge;

namespace AIService.Services.Knowledge;

/// <summary>
/// Vector store interface for managing knowledge vectors.
/// </summary>
public interface IVectorStore
{
    /// <summary>
    /// Upserts a collection of knowledge vectors into the vector store.
    /// </summary>
    /// <param name="vectors"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task UpsertAsync(
        IReadOnlyCollection<KnowledgeVector> vectors,
        CancellationToken cancellationToken);
}
