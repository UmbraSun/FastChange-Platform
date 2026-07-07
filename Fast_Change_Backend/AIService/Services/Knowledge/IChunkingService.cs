using AIService.Models.Knowledge;

namespace AIService.Services.Knowledge;

/// <summary>
/// Chunking service interface for splitting knowledge documents into smaller chunks for processing and storage.
/// </summary>
public interface IChunkingService
{
    /// <summary>
    /// Chunks the given knowledge document into smaller pieces for easier processing and storage.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    IReadOnlyCollection<KnowledgeChunk> Chunk(
        KnowledgeDocument document);
}
