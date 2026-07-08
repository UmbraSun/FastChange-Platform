using AIService.Providers.OpenAI;
using AIService.Services.Prompt;
using AIService.Services.Retrieval;

namespace AIService.Services.Chat;

public sealed class RagChatService
    : IChatService
{
    private readonly IRetrievalService _retrievalService;
    private readonly IPromptBuilder _promptBuilder;
    private readonly IChatProvider _chatProvider;

    public RagChatService(
        IRetrievalService retrievalService,
        IPromptBuilder promptBuilder,
        IChatProvider chatProvider)
    {
        _retrievalService = retrievalService;
        _promptBuilder = promptBuilder;
        _chatProvider = chatProvider;
    }

    public async Task<string> AskAsync(
        string question,
        CancellationToken cancellationToken)
    {
        var chunks =
            await _retrievalService.SearchAsync(
                question,
                5,
                cancellationToken);

        var prompt = _promptBuilder.Build(question, chunks);

        return await _chatProvider.AskAsync(
            prompt.SystemPrompt,
            prompt.UserPrompt,
            cancellationToken);
    }
}
