namespace MyConnectorPlugin;

/// <summary>
/// Configuration for the connector.
/// </summary>
public class MyConnectorConfig
{
    public string ConnectionUrl { get; }
    public int PollIntervalMs { get; }
    public int TaskId { get; }

    public MyConnectorConfig(IDictionary<string, string> props)
    {
        ConnectionUrl = props.GetValueOrDefault("connection.url", "http://localhost:8080");
        PollIntervalMs = int.TryParse(props.GetValueOrDefault("poll.interval.ms", "1000"), out var interval)
            ? interval
            : 1000;
        TaskId = int.TryParse(props.GetValueOrDefault("task.id", "0"), out var taskId)
            ? taskId
            : 0;
    }
}
