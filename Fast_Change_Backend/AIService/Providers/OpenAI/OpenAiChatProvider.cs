using AIService.AI.Options;
using Microsoft.Extensions.Options;
using OpenAI;

namespace AIService.Providers.OpenAI;

public sealed class OpenAiChatProvider
    : IChatProvider
{
    private readonly OpenAIClient _client;
    private readonly OpenAiOptions _options;

    public OpenAiChatProvider(
        OpenAIClient client,
        IOptions<OpenAiOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<string> AskAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
