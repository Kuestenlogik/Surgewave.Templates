using Microsoft.Extensions.Configuration;

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var bootstrapServers = config["Surgewave:BootstrapServers"] ?? "localhost:9092";
var topic = config["Surgewave:Topic"] ?? "my-topic";
var groupId = config["Surgewave:GroupId"] ?? "my-consumer-group";

Console.WriteLine($"Connecting to {bootstrapServers}...");
Console.WriteLine($"Consuming from topic: {topic}");
Console.WriteLine($"Consumer group: {groupId}");
Console.WriteLine("Press Ctrl+C to stop.");
Console.WriteLine();

// Setup cancellation
using var cts = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    Console.WriteLine("\nShutting down...");
};

// Create consumer
await using var consumer = await Kuestenlogik.Surgewave.Client.SurgewaveConsumer.CreateAsync(options =>
{
    options.BootstrapServers = bootstrapServers;
    options.GroupId = groupId;
});

// Subscribe to topic
consumer.Subscribe(topic);

Console.WriteLine("Waiting for messages...");
Console.WriteLine();

// Consume messages
try
{
    while (!cts.Token.IsCancellationRequested)
    {
        var result = await consumer.ConsumeAsync(cts.Token);

        if (result != null)
        {
            Console.WriteLine($"[{result.Topic}:{result.Partition}@{result.Offset}] {result.Key}: {result.Value}");
        }
    }
}
catch (OperationCanceledException)
{
    // Expected when Ctrl+C is pressed
}

Console.WriteLine("Consumer closed.");
