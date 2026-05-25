# Surgewave Project Templates

`dotnet new` templates for quickly scaffolding Surgewave applications.

## Installation

```bash
# From NuGet (when published)
dotnet new install Kuestenlogik.Surgewave.Templates

# From local package
dotnet new install ./Kuestenlogik.Surgewave.Templates.0.1.0.nupkg
```

## Available Templates

| Template | Short Name | Description |
|----------|------------|-------------|
| Surgewave Producer | `surgewave-producer` | Console app that sends messages |
| Surgewave Consumer | `surgewave-consumer` | Console app that receives messages |
| Surgewave Worker | `surgewave-worker` | Background service with IHostedService |
| Surgewave ASP.NET | `surgewave-aspnet` | Web app with producer/consumer |
| Surgewave Streams | `surgewave-streams` | Stream processing application |
| Surgewave Connector | `surgewave-connect` | Custom connector plugin |

## Usage

### Create a Producer

```bash
dotnet new surgewave-producer -n MyProducer
cd MyProducer
dotnet run
```

### Create a Consumer

```bash
dotnet new surgewave-consumer -n MyConsumer --topic my-topic --groupId my-group
cd MyConsumer
dotnet run
```

### Create a Worker Service

```bash
dotnet new surgewave-worker -n MyWorker
cd MyWorker
dotnet run
```

### Create an ASP.NET Web App

```bash
dotnet new surgewave-aspnet -n MyWebApp
cd MyWebApp
dotnet run
# Visit http://localhost:5000
```

### Create a Streams Application

```bash
dotnet new surgewave-streams -n MyStreamsApp --inputTopic input --outputTopic output
cd MyStreamsApp
dotnet run
```

### Create a Custom Connector

```bash
# Source connector
dotnet new surgewave-connect -n MySourceConnector --connectorType source

# Sink connector
dotnet new surgewave-connect -n MySinkConnector --connectorType sink
```

## Template Parameters

### Common Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `--bootstrapServers` | `localhost:9092` | Surgewave/Kafka broker address |
| `--topic` | `my-topic` | Default topic name |

### Consumer/Worker Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `--groupId` | `my-consumer-group` | Consumer group ID |

### Streams Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `--inputTopic` | `input-topic` | Input topic |
| `--outputTopic` | `output-topic` | Output topic |

### Connector Parameters

| Parameter | Default | Description |
|-----------|---------|-------------|
| `--connectorType` | `source` | `source` or `sink` |

## Configuration

All templates use `appsettings.json` for configuration:

```json
{
  "Surgewave": {
    "BootstrapServers": "localhost:9092",
    "Topic": "my-topic",
    "GroupId": "my-consumer-group"
  }
}
```

## Uninstall

```bash
dotnet new uninstall Kuestenlogik.Surgewave.Templates
```
