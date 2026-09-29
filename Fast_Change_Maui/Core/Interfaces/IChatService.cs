namespace Core.Interfaces;

/// <summary>
/// Represents a service for interacting with a chat system.
/// </summary>
public interface IChatService
{
    /// <summary>
    /// Asks a question to the chat service and retrieves the response.
    /// </summary>
    /// <param name="question"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string> AskAsync(string question, CancellationToken cancellationToken = default);
}