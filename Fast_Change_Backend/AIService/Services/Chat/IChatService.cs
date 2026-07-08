namespace AIService.Services.Chat;

/// <summary>
/// Chat service interface for interacting with a chat provider.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Ask a question to the chat provider and get a response.
    /// </summary>
    /// <param name="question"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> AskAsync(
        string question,
        CancellationToken cancellationToken);
}
