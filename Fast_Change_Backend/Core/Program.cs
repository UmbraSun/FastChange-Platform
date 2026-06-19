using Core.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddInfrastructureServices(builder.Configuration)
    .Build()
    .UseInfrastructurePipeline()
    .Run();
