using Kuestenlogik.Surgewave.Client;

namespace SurgewaveWorkerApp;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _config;
    private readonly SurgewaveConsumer _consumer;

    public Worker(ILogger<Worker> logger, IConfiguration config, SurgewaveConsumer consumer)
    {
        _logger = logger;
        _config = config;
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topic = _config["Surgewave:Topic"] ?? "my-topic";

        _logger.LogInformation("Subscribing to topic: {Topic}", topic);
        _consumer.Subscribe(topic);

        _logger.LogInformation("Worker started. Processing messages...");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _consumer.ConsumeAsync(stoppingToken);

                if (result != null)
                {
                    await ProcessMessageAsync(result, stoppingToken);
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                // Graceful shutdown
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing message");
                await Task.Delay(1000, stoppingToken); // Back off on error
            }
        }

        _logger.LogInformation("Worker stopped.");
    }

    private async Task ProcessMessageAsync(ConsumeResult<string, string> result, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Processing message from {Topic}:{Partition}@{Offset}: {Key} = {Value}",
            result.Topic,
            result.Partition,
            result.Offset,
            result.Key,
            result.Value);

        // TODO: Add your message processing logic here
        await Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker stopping...");
        await _consumer.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
