using Core.Api.Chat;
using Core.DTOs.Chat;
using Core.Interfaces;

namespace Infrastructure.Services;

public sealed class ChatService : IChatService
{
    private readonly IChatApi _chatApi;

    public ChatService(IChatApi chatApi)
    {
        _chatApi = chatApi;
    }

    public async Task<string> AskAsync(string question, CancellationToken cancellationToken = default)
    {
        var response = await _chatApi.AskAsync(new ChatRequestDto(question), cancellationToken);
        return response.Answer;
    }
}