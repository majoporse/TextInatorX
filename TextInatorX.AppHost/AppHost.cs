using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<ImageProcessor>("ImageProcessor").WithReplicas(3);
builder.AddProject<Presentation_Mvc>("Frontend");
builder.AddProject<ImageStorage>("ImageStorage").WithReplicas(3);
await builder.Build().RunAsync();