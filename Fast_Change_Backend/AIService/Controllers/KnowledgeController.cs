using AIService.Contracts.Knowledge;
using AIService.Services.Knowledge;
using AIService.Services.Retrieval;
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
    private readonly IRetrievalService _retrievalService;

    public KnowledgeController(
        IKnowledgeIndexer indexer,
        IRetrievalService retrievalService)
    {
        _indexer = indexer;
        _retrievalService = retrievalService;
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

    /// <summary>
    /// Search the knowledge base for relevant documents based on the provided query and return the top results.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("search")]
    public async Task<ActionResult<SearchKnowledgeResponse>> Search(
    SearchKnowledgeRequest request,
    CancellationToken cancellationToken)
    {
        var result =
            await _retrievalService.SearchAsync(
                request.Query,
                request.Top,
                cancellationToken);

        return Ok(
            new SearchKnowledgeResponse(
                result
                    .Select(x =>
                        new SearchKnowledgeItem(
                            x.DocumentName,
                            x.Heading,
                            x.ChunkIndex,
                            x.Score,
                            x.Content))
                    .ToList()));
    }
}
