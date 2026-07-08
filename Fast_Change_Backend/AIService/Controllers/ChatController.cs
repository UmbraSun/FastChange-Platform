using AIService.Contracts.Chat;
using AIService.Services.Chat;
using Microsoft.AspNetCore.Mvc;

namespace AIService.Controllers;

/// <summary>
/// Chat controller that handles chat requests and responses.
/// </summary>
[ApiController]
[Route("api/chat")]
public sealed class ChatController
    : ControllerBase
{
    private readonly IChatService _chatService;


    public ChatController(
        IChatService chatService)
    {
        _chatService = chatService;
    }

    /// <summary>
    /// Ask a question and get an answer from the chat service.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Ask(
        ChatRequest request,
        CancellationToken cancellationToken)
    {
        var answer =
            await _chatService.AskAsync(
                request.Question,
                cancellationToken);


        return Ok(
            new ChatResponse(answer));
    }
}
