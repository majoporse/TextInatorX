using Application;
using BlobStorage;
using Confluent.Kafka;
using Confluent.Kafka.Extensions.OpenTelemetry;
using Contracts.Events;
using ImTools;
using JasperFx.Core;
using JasperFx.Resources;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Persistence;
using TextInatorX.ServiceDefaults;
using Wolverine;
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
        .AddAspNetCoreInstrumentation()
        .AddConfluentKafkaInstrumentation()
        .AddHttpClientInstrumentation()
        .AddEntityFrameworkCoreInstrumentation());

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Host.UseWolverine(opts =>
{
    opts.UseKafka("localhost:9094")

        // See https://github.com/confluentinc/confluent-kafka-dotnet for the exact options here
        .ConfigureClient(client =>
        {
            // configure both producers and consumers
            client.BootstrapServers = "localhost:9094";
            client.ClientId = "image-storage-service";
            client.Acks = Acks.All;
            client.MessageMaxBytes = 10000000; // 10 MB
            client.AllowAutoCreateTopics = true;
        })
        .ConfigureConsumers(consumer =>
        {
            // configure only consumers
            consumer.GroupId = "image-storage-service-group";
            consumer.AutoOffsetReset = AutoOffsetReset.Earliest;
        })
        .ConfigureProducers(producer =>
        {
            // configure only producers
            producer.EnableIdempotence = true;
            producer.MessageTimeoutMs = 10000;
        })

        // .ConfigureProducerBuilders(builder =>
        // {
        //     // there are some options that are only exposed
        //     // on the ProducerBuilder
        // })
        //
        // .ConfigureConsumerBuilders(builder =>
        // {
        //     // there are some Kafka client options that
        //     // are only exposed from the builder
        // })
        //
        // .ConfigureAdminClientBuilders(builder =>
        // {
        //     // configure admin client builders
        // })
        ;

    // Just publish all messages to Kafka topics
    // based on the message type (or message attributes)
    // This will get fancier in the near future

    opts.PublishMessage<AddImageRequestResult>().ToKafkaTopic(nameof(AddImageRequestResult));
    opts.PublishMessage<DeleteImageRequestResult>().ToKafkaTopic(nameof(DeleteImageRequestResult));
    opts.PublishMessage<GetImageRequestResult>().ToKafkaTopic(nameof(GetImageRequestResult));
    opts.PublishMessage<GetAllImagesRequestResult>().ToKafkaTopic(nameof(GetAllImagesRequestResult));

    string[] topics =
    [
        nameof(AddImageRequest),
        nameof(DeleteImageRequest),
        nameof(GetAllImagesRequest),
        nameof(GetImageRequest)
    ];

    topics.ForEach(e => opts.ListenToKafkaTopic(e));

    // Listen to topics
    // opts.ListenToKafkaTopic("image-uploaded-event")
    //     .ProcessInline()
    //
    //     // Override the consumer configuration for only this 
    //     // topic
    //     .ConfigureConsumer(config =>
    //     {
    //         // This will also set the Envelope.GroupId for any
    //         // received messages at this topic
    //         config.GroupId = "foo";
    //
    //         // Other configuration
    //     });
    //
    // opts.ListenToKafkaTopic("green")
    //     .BufferedInMemory();

    // This will direct Wolverine to try to ensure that all
    // referenced Kafka topics exist at application start up
    // time
    opts.Services.AddResourceSetupOnStartup();
});

builder.Services.BlobStorageInstall(builder.Configuration);
builder.Services.PersistenceInstall(builder.Configuration);
builder.Services.ApplicationInstall();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

await app.RunAsync();