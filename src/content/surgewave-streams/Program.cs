using Kuestenlogik.Surgewave.Streams;
using Microsoft.Extensions.Configuration;

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var bootstrapServers = config["Surgewave:BootstrapServers"] ?? "localhost:9092";
var inputTopic = config["Surgewave:InputTopic"] ?? "input-topic";
var outputTopic = config["Surgewave:OutputTopic"] ?? "output-topic";
var applicationId = config["Surgewave:ApplicationId"] ?? "surgewave-streams-app";

Console.WriteLine($"Starting Surgewave Streams application: {applicationId}");
Console.WriteLine($"Input topic: {inputTopic}");
Console.WriteLine($"Output topic: {outputTopic}");
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

// Build stream topology
var builder = new StreamsBuilder();

// Example: Read from input, transform, write to output
var stream = builder.Stream<string, string>(inputTopic);

// Transform: Convert to uppercase
var transformed = stream
    .MapValues(value => value.ToUpperInvariant())
    .Peek((key, value) => Console.WriteLine($"Processing: {key} -> {value}"));

// Write to output topic
transformed.To(outputTopic);

// Build and start the streams application
var topology = builder.Build();

Console.WriteLine("Topology:");
Console.WriteLine(topology.Describe());
Console.WriteLine();

var streams = new KafkaStreams(topology, new StreamsConfig
{
    ApplicationId = applicationId,
    BootstrapServers = bootstrapServers
});

try
{
    await streams.StartAsync(cts.Token);

    // Wait for cancellation
    await Task.Delay(Timeout.Infinite, cts.Token);
}
catch (OperationCanceledException)
{
    // Expected on shutdown
}
finally
{
    await streams.CloseAsync();
}

Console.WriteLine("Streams application stopped.");
