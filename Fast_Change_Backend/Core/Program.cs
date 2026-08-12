using Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

var app = builder.AddInfrastructureServices(builder.Configuration)
    .Build();

app = await app.UseInfrastructurePipeline();
app.Run();

public partial class Program;