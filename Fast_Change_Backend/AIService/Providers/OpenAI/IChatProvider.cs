namespace AIService.Providers.OpenAI;

/// <summary>
/// Chat provider interface for interacting with AI chat models.
/// </summary>
public interface IChatProvider
{
    /// <summary>
    /// Adds a system prompt to the chat context.
    /// </summary>
    /// <param name="systemPrompt"></param>
    /// <param name="userPrompt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> AskAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken);
}
