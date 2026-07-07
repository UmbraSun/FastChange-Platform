using AIService.Models.Knowledge;

namespace AIService.Services.Knowledge;

/// <summary>
/// Knowledge loader interface for loading knowledge documents from various sources.
/// </summary>
public interface IKnowledgeLoader
{
    /// <summary>
    /// Loads knowledge documents from the source.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<KnowledgeDocument>> LoadAsync(
        CancellationToken cancellationToken);
}