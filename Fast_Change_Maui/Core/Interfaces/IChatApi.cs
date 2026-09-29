using Core.DTOs.Chat;
using Refit;

namespace Core.Api.Chat;

/// <summary>
/// Represents the API for interacting with the chat service.
/// </summary>
public interface IChatApi
{
    /// <summary>
    /// Sends a chat request to the API and retrieves the response.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [Post("/api/chat")]
    Task<ChatResponseDto> AskAsync(ChatRequestDto request, CancellationToken cancellationToken = default);
}