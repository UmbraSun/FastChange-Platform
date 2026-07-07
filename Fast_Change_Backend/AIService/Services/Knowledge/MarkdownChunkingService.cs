using AIService.Models.Knowledge;
using System.Text;

namespace AIService.Services.Knowledge;

public sealed class MarkdownChunkingService
    : IChunkingService
{
    public IReadOnlyCollection<KnowledgeChunk> Chunk(
        KnowledgeDocument document)
    {
        var chunks = new List<KnowledgeChunk>();

        string heading = document.Name;

        var builder = new StringBuilder();

        foreach (var line in document.Content.Split('\n'))
        {
            if (line.StartsWith("#"))
            {
                Flush();

                heading = line.TrimStart('#').Trim();
                continue;
            }

            builder.AppendLine(line);
        }

        Flush();

        return chunks;

        void Flush()
        {
            var content = builder.ToString().Trim();

            if (string.IsNullOrWhiteSpace(content))
            {
                builder.Clear();
                return;
            }

            chunks.Add(new KnowledgeChunk(
                Guid.NewGuid(),
                document.Name,
                heading,
                content));

            builder.Clear();
        }
    }
}
