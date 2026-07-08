using AIService.Services.Knowledge;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Controllers;

/// <summary>
/// Knowledge controller that handles knowledge indexing requests.
/// </summary>
[ApiController]
[Route("api/knowledge")]
public sealed class KnowledgeController
    : ControllerBase
{
    private readonly IKnowledgeIndexer _indexer;

    public KnowledgeController(
        IKnowledgeIndexer indexer)
    {
        _indexer = indexer;
    }

    /// <summary>
    /// Index the knowledge base and store the embeddings in the vector store.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("index")]
    public async Task<IActionResult> Index(
        CancellationToken cancellationToken)
    {
        await _indexer.IndexAsync(
            cancellationToken);

        return Ok(
            new
            {
                Message = "Knowledge indexing completed"
            });
    }
}
