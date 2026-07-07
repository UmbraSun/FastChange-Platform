using AIService.Models.Knowledge;

namespace AIService.Services.Knowledge;

public sealed class FileKnowledgeLoader
    : IKnowledgeLoader
{
    private readonly IWebHostEnvironment _environment;

    public FileKnowledgeLoader(
        IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<IReadOnlyCollection<KnowledgeDocument>> LoadAsync(
        CancellationToken cancellationToken)
    {
        var knowledgeFolder =
            Path.Combine(_environment.ContentRootPath, "Knowledge");

        if (!Directory.Exists(knowledgeFolder))
            return [];

        var documents = new List<KnowledgeDocument>();

        foreach (var file in Directory.EnumerateFiles(
                     knowledgeFolder,
                     "*.md",
                     SearchOption.AllDirectories))
        {
            var content =
                await File.ReadAllTextAsync(file, cancellationToken);

            documents.Add(
                new KnowledgeDocument(
                    Path.GetFileName(file),
                    content));
        }

        return documents;
    }
}
