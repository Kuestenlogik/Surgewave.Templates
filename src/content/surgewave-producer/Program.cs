using Microsoft.Extensions.Configuration;

// Load configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var bootstrapServers = config["Surgewave:BootstrapServers"] ?? "localhost:9092";
var topic = config["Surgewave:Topic"] ?? "my-topic";

Console.WriteLine($"Connecting to {bootstrapServers}...");
Console.WriteLine($"Producing to topic: {topic}");
Console.WriteLine("Type messages and press Enter to send. Type 'exit' to quit.");
Console.WriteLine();

// Create producer
await using var producer = await Kuestenlogik.Surgewave.Client.SurgewaveProducer.CreateAsync(options =>
{
    options.BootstrapServers = bootstrapServers;
});

// Send messages
while (true)
{
    Console.Write("> ");
    var input = Console.ReadLine();

    if (string.IsNullOrEmpty(input))
        continue;

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    var key = Guid.NewGuid().ToString("N")[..8];
    var result = await producer.ProduceAsync(topic, key, input);

    Console.WriteLine($"  Sent to partition {result.Partition} at offset {result.Offset}");
}

Console.WriteLine("Producer closed.");
