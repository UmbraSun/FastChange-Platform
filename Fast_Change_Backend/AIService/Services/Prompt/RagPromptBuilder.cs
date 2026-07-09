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
        var context = new StringBuilder();


        foreach (var chunk in chunks)
        {
            context.AppendLine("--------------------------------");

            context.AppendLine(
                $"Document: {chunk.DocumentName}");

            context.AppendLine(
                $"Heading: {chunk.Heading}");

            context.AppendLine(
                $"Chunk: {chunk.ChunkIndex}");

            context.AppendLine();

            context.AppendLine(
                chunk.Content);

            context.AppendLine();
        }


        context.AppendLine("--------------------------------");


        var systemPrompt =
            """
            You are an assistant for the FastChange project.

            Answer ONLY using the provided context.

            If the answer cannot be found in the context,
            say that the documentation does not contain the answer.
            """;


        var userPrompt =
            $"""
            Context:

            {context}

            Question:

            {question}
            """;


        return new ChatPrompt(
            systemPrompt,
            userPrompt);
    }
}
