using AIService.Models.Knowledge;

namespace AIService.Services.VectorStore;

/// <summary>
/// Vector store interface for managing knowledge vectors and performing similarity searches.
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

    /// <summary>
    /// Searches for knowledge vectors similar to the provided embedding and returns a collection of search results.
    /// </summary>
    /// <param name="embedding"></param>
    /// <param name="limit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<KnowledgeSearchResult>> SearchAsync(
        float[] embedding,
        int limit,
        CancellationToken cancellationToken);

    /// <summary>
    /// Exists checks if a knowledge vector with the specified document name and hash exists in the vector store.
    /// </summary>
    /// <param name="documentName"></param>
    /// <param name="hash"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(
        string documentName,
        string hash,
        CancellationToken cancellationToken);
}
