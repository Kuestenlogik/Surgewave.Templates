using Kuestenlogik.Surgewave.Connect;

namespace MyConnectorPlugin;

/// <summary>
/// Source task that reads data from an external system.
/// </summary>
public class MySourceTask : ISourceTask
{
    private MyConnectorConfig? _config;
    private int _counter;

    public string Version => "1.0.0";

    public void Start(IDictionary<string, string> props)
    {
        _config = new MyConnectorConfig(props);
        Console.WriteLine($"Task {_config.TaskId} started. Polling every {_config.PollIntervalMs}ms");
    }

    public async Task<IList<SourceRecord>> PollAsync(CancellationToken cancellationToken)
    {
        // Wait for poll interval
        await Task.Delay(_config!.PollIntervalMs, cancellationToken);

        // TODO: Replace with actual data fetching logic
        var records = new List<SourceRecord>();

        // Example: Generate a sample record
        var record = new SourceRecord(
            sourcePartition: new Dictionary<string, object> { ["source"] = "my-source" },
            sourceOffset: new Dictionary<string, object> { ["offset"] = _counter },
            topic: "my-connector-topic",
            key: $"key-{_counter}",
            value: $"Sample data from source at {DateTime.UtcNow:O}"
        );

        records.Add(record);
        _counter++;

        return records;
    }

    public void Stop()
    {
        Console.WriteLine($"Task {_config?.TaskId} stopped. Produced {_counter} records.");
    }

    public void Commit()
    {
        // Called when records are committed
    }

    public void CommitRecord(SourceRecord record)
    {
        // Called when individual record is committed
    }
}
