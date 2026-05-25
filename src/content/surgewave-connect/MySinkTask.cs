using Kuestenlogik.Surgewave.Connect;

namespace MyConnectorPlugin;

/// <summary>
/// Sink task that writes data to an external system.
/// </summary>
public class MySinkTask : ISinkTask
{
    private MyConnectorConfig? _config;
    private int _counter;

    public string Version => "1.0.0";

    public void Start(IDictionary<string, string> props)
    {
        _config = new MyConnectorConfig(props);
        Console.WriteLine($"Sink task {_config.TaskId} started");
    }

    public async Task PutAsync(IEnumerable<SinkRecord> records, CancellationToken cancellationToken)
    {
        foreach (var record in records)
        {
            // TODO: Replace with actual data writing logic
            Console.WriteLine($"Writing record: {record.Key} = {record.Value}");

            // Simulate write operation
            await Task.Delay(10, cancellationToken);

            _counter++;
        }
    }

    public void Flush(IDictionary<TopicPartition, long> offsets)
    {
        Console.WriteLine($"Flushing {offsets.Count} partition(s). Total records written: {_counter}");
    }

    public void Stop()
    {
        Console.WriteLine($"Sink task {_config?.TaskId} stopped. Wrote {_counter} records.");
    }
}
