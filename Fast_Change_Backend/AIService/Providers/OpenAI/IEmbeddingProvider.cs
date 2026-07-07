namespace AIService.Providers.OpenAI;

/// <summary>
/// Embedding provider interface for generating embeddings from text.
/// </summary>
public interface IEmbeddingProvider
{
    /// <summary>
    /// Creates an embedding for the given text asynchronously.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<float[]> CreateEmbeddingAsync(
        string text,
        CancellationToken cancellationToken);
}
