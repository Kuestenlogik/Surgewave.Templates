var builder = WebApplication.CreateBuilder(args);

// Configure Surgewave producer as singleton
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var bootstrapServers = config["Surgewave:BootstrapServers"] ?? "localhost:9092";

    return Kuestenlogik.Surgewave.Client.SurgewaveProducer.CreateAsync(options =>
    {
        options.BootstrapServers = bootstrapServers;
    }).GetAwaiter().GetResult();
});

// Add health checks
builder.Services.AddHealthChecks();

var app = builder.Build();

// Health check endpoint
app.MapHealthChecks("/health");

// Get configuration
var topic = builder.Configuration["Surgewave:Topic"] ?? "my-topic";

// Produce message endpoint
app.MapPost("/produce", async (Kuestenlogik.Surgewave.Client.SurgewaveProducer producer, MessageRequest request) =>
{
    var targetTopic = request.Topic ?? topic;
    var key = request.Key ?? Guid.NewGuid().ToString("N")[..8];

    var result = await producer.ProduceAsync(targetTopic, key, request.Value);

    return Results.Ok(new
    {
        Topic = result.Topic,
        Partition = result.Partition,
        Offset = result.Offset,
        Key = key
    });
});

// Simple produce with query parameters
app.MapGet("/produce/{message}", async (Kuestenlogik.Surgewave.Client.SurgewaveProducer producer, string message) =>
{
    var key = Guid.NewGuid().ToString("N")[..8];
    var result = await producer.ProduceAsync(topic, key, message);

    return Results.Ok(new
    {
        Topic = result.Topic,
        Partition = result.Partition,
        Offset = result.Offset,
        Message = message
    });
});

// Info endpoint
app.MapGet("/", () => Results.Ok(new
{
    Name = "Surgewave Web Application",
    Topic = topic,
    Endpoints = new[]
    {
        "GET /health - Health check",
        "POST /produce - Send a message (JSON body: { topic?, key?, value })",
        "GET /produce/{message} - Quick send a message"
    }
}));

app.Run();

// Request model
record MessageRequest(string Value, string? Topic = null, string? Key = null);
