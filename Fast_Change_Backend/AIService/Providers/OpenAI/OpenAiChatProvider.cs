using OpenAI.Chat;

namespace AIService.Providers.OpenAI;

public sealed class OpenAiChatProvider
    : IChatProvider
{
    private readonly ChatClient _client;

    public OpenAiChatProvider(
        ChatClient client)
    {
        _client = client;
    }

    public async Task<string> AskAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {
        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(systemPrompt),
            new UserChatMessage(userPrompt)
        };

        ChatCompletion completion =
            await _client.CompleteChatAsync(
                messages,
                cancellationToken: cancellationToken);

        return completion.Content[0].Text;
    }
}