using AIService.Models.Knowledge;

namespace AIService.Services.Prompt;

/// <summary>
/// Prompt builder interface for constructing prompts based on a question and relevant knowledge chunks.
/// </summary>
public interface IPromptBuilder
{
    /// <summary>
    /// Builds a prompt string based on the provided question and relevant knowledge chunks.
    /// </summary>
    /// <param name="question"></param>
    /// <param name="chunks"></param>
    /// <returns></returns>
    ChatPrompt Build(
        string question,
        IReadOnlyCollection<KnowledgeSearchResult> chunks);
}
