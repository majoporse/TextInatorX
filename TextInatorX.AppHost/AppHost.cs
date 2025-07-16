using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ImageProcessor>("ImageProcessor");
builder.AddProject<Presentation_Mvc>("ImageStorage");

await builder.Build().RunAsync();