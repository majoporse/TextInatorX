using Confluent.Kafka;
using Confluent.Kafka.Extensions.OpenTelemetry;
using Contracts.Events;
using ImageProcessor.Application;
using JasperFx.Resources;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistence;
using TextInatorX.ServiceDefaults;
using Wolverine;
using Wolverine.Http;
using Wolverine.Kafka;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddOpenTelemetry()
    .WithTracing(t => t
        .SetResourceBuilder(ResourceBuilder
            .CreateDefault()
            .AddService("OtelWebApi")) // <-- sets service name

        // This is absolutely necessary to collect the Wolverine
        // open telemetry tracing information in your application
        .AddSource("Wolverine")
        .AddEntityFrameworkCoreInstrumentation()
        .AddConfluentKafkaInstrumentation()
        .AddHttpClientInstrumentation()
        .AddMongoDBInstrumentation()
        .AddAspNetCoreInstrumentation());

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddOpenTelemetry();
builder.Services.PersistenceInstall(builder.Configuration);
builder.Services.AddApplicationInstaller(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddWolverineHttp();
builder.Services.AddCors();

builder.Host.UseWolverine(opts =>
{
    opts.UseKafka("localhost:9094")

        // See https://github.com/confluentinc/confluent-kafka-dotnet for the exact options here
        .ConfigureClient(client =>
        {
            // configure both producers and consumers
            client.BootstrapServers = "localhost:9094";
            client.ClientId = "image-processor-service";
            client.Acks = Acks.All;
            client.MessageMaxBytes = 10000000; // 10 MB
            client.AllowAutoCreateTopics = true;
        })
        .ConfigureConsumers(consumer =>
        {
            // configure only consumers
            consumer.GroupId = "image-processor-service-group";
            consumer.AutoOffsetReset = AutoOffsetReset.Earliest;
        })
        .ConfigureProducers(producer =>
        {
            // configure only producers
            producer.EnableIdempotence = true;
            producer.MessageTimeoutMs = 10000;
        });

    opts.ListenToKafkaTopic(nameof(ImageUploadedEvent));
    opts.PublishMessage<ImageUploadedEventResult>().ToKafkaTopic(nameof(ImageUploadedEventResult));

    opts.ListenToKafkaTopic(nameof(GetImageTextRequest));
    opts.PublishMessage<GetImageTextRequestResult>().ToKafkaTopic(nameof(GetImageTextRequestResult));


    // This will direct Wolverine to try to ensure that all
    // referenced Kafka topics exist at application start up
    // time
    opts.Services.AddResourceSetupOnStartup();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
    .AllowCredentials()); // allow credentials

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (builder.Environment.IsDevelopment())
    app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });

app.UseHttpsRedirection();

app.MapWolverineEndpoints();

await app.RunAsync();