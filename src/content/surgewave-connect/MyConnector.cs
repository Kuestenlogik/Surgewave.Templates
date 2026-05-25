using Kuestenlogik.Surgewave.Connect;

namespace MyConnectorPlugin;

/// <summary>
/// Custom Surgewave Connect connector.
/// </summary>
[ConnectorMetadata(
    Name = "my-connector",
    Description = "A custom connector for MyConnector",
    Author = "Your Name",
    Tags = new[] { "custom" })]
public class MyConnector : ISourceConnector
{
    private MyConnectorConfig? _config;

    public string Version => "1.0.0";

    public ConfigDef Config => new ConfigDef()
        .Define("connection.url", ConfigDef.Type.String, ConfigDef.Importance.High, "Connection URL")
        .Define("poll.interval.ms", ConfigDef.Type.Int, 1000, ConfigDef.Importance.Medium, "Poll interval in milliseconds");

    public void Start(IDictionary<string, string> props)
    {
        _config = new MyConnectorConfig(props);
        Console.WriteLine($"Starting connector with URL: {_config.ConnectionUrl}");
    }

    public IList<IDictionary<string, string>> TaskConfigs(int maxTasks)
    {
        // Return configuration for each task
        var configs = new List<IDictionary<string, string>>();

        for (int i = 0; i < maxTasks; i++)
        {
            configs.Add(new Dictionary<string, string>
            {
                ["task.id"] = i.ToString(),
                ["connection.url"] = _config!.ConnectionUrl,
                ["poll.interval.ms"] = _config.PollIntervalMs.ToString()
            });
        }

        return configs;
    }

    public Type TaskClass => typeof(MySourceTask);

    public void Stop()
    {
        Console.WriteLine("Stopping connector");
    }
}
