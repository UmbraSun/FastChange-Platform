using AIService.Models.Knowledge;

namespace AIService.Services.Retrieval;

/// <summary>
/// Retrieval service interface for searching knowledge based on a question.
/// </summary>
public interface IRetrievalService
{
    /// <summary>
    /// Searches for knowledge based on the provided question and returns a collection of search results.
    /// </summary>
    /// <param name="question"></param>
    /// <param name="limit"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<KnowledgeSearchResult>> SearchAsync(
        string question,
        int limit,
        CancellationToken cancellationToken);
}
