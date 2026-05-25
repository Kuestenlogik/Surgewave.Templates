using SurgewaveWorkerApp;

var builder = Host.CreateApplicationBuilder(args);

// Register Surgewave consumer as singleton
builder.Services.AddSingleton(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var bootstrapServers = config["Surgewave:BootstrapServers"] ?? "localhost:9092";
    var groupId = config["Surgewave:GroupId"] ?? "my-worker-group";

    return Kuestenlogik.Surgewave.Client.SurgewaveConsumer.CreateAsync(options =>
    {
        options.BootstrapServers = bootstrapServers;
        options.GroupId = groupId;
    }).GetAwaiter().GetResult();
});

// Register the worker
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
