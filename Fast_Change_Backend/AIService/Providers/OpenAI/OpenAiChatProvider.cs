using Infrastructure.Observability;
using OpenAI.Chat;
using System.Diagnostics;

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
        using var activity = FastChangeTelemetry.ActivitySource.StartActivity(
                "OpenAI Chat",
                ActivityKind.Client);

        activity?.SetTag(
            "ai.provider",
            "openai");


        var messages = new List<ChatMessage>
    {
        new SystemChatMessage(systemPrompt),
        new UserChatMessage(userPrompt)
    };

        try
        {
            ChatCompletion completion =
                await _client.CompleteChatAsync(
                    messages,
                    cancellationToken: cancellationToken);

            activity?.SetTag(
                "ai.success",
                true);

            return completion.Content[0].Text;
        }
        catch (Exception ex)
        {
            activity?.SetStatus(
                ActivityStatusCode.Error);

            activity?.AddException(ex);

            throw;
        }
    }
}