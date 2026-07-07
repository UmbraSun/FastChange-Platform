namespace AIService.Services.Knowledge;

/// <summary>
/// Knowledge indexer interface for indexing knowledge data.
/// </summary>
public interface IKnowledgeIndexer
{
    /// <summary>
    /// Indexes the knowledge data asynchronously.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task IndexAsync(
        CancellationToken cancellationToken);
}
