using AIService.Models.Knowledge;
using System.Text;

namespace AIService.Services.Prompt;

public sealed class RagPromptBuilder
    : IPromptBuilder
{
    public ChatPrompt Build(
        string question,
        IReadOnlyCollection<KnowledgeSearchResult> chunks)
    {
        var sb = new StringBuilder();

        sb.AppendLine(
            """
            You are an assistant for the FastChange project.

            Answer ONLY using the provided context.

            If the answer cannot be found in the context,
            say that the documentation does not contain the answer.

            Context:
            """);

        foreach (var chunk in chunks)
        {
            sb.AppendLine("--------------------------------");

            sb.AppendLine(
                $"Document: {chunk.DocumentName}");

            sb.AppendLine(
                $"Heading: {chunk.Heading}");

            sb.AppendLine(chunk.Content);

            sb.AppendLine();
        }

        sb.AppendLine("--------------------------------");

        sb.AppendLine($"Question: {question}");

        return new ChatPrompt(sb.ToString(), question);
    }
}
