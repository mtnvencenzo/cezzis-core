# 📦 Cezzi.OTel

## 🚀 What is Cezzi.OTel?
Cezzi.OTel is a professional .NET library and NuGet package for integrating [OpenTelemetry](https://opentelemetry.io/) into your applications. It provides robust extension methods and configuration options for distributed tracing, metrics, and logging, making observability simple and consistent across your services.

## 🎯 Key Features
- Plug-and-play OpenTelemetry setup for Traces, Metrics, and Logs
- Flexible configuration via appsettings, environment variables, or code
- Extensible options for custom telemetry scenarios
- .NET 9+ support and modern best practices

## 🛠️ Installation
Install via NuGet:
```bash
dotnet add package Cezzi.OTel
```

## ⚡ Quick Start
Add OpenTelemetry to your application with a single line:
```csharp
using Cezzi.OTel;

var builder = Host.CreateApplicationBuilder();
builder.AddApplicationOpenTelemetry();
var app = builder.Build();
```

## ⚙️ Configuration
You can configure Cezzi.OTel using `appsettings.json`, environment variables, or directly in code.


### Example: appsettings.json
```json
{
  "OTel": {
    "ServiceName": "MyService",
    "ServiceNamespace": "MyNamespace",
    "Enabled": true,
    "OtlpExporter": {
      "Endpoint": "http://localhost:4317",
      "Protocol": "grpc",
      "Headers": "auth=test",
      "TimeoutMilliseconds": 10000,
      "ExportProcessorType": "Batch",
      "BatchExportProcessorOptions": {
        "MaxQueueSize": 2048,
        "ScheduledDelayMilliseconds": 5000,
        "ExporterTimeoutMilliseconds": 30000,
        "MaxExportBatchSize": 512
      }
    },
    "Traces": {
      "Enabled": true,
      "ExcludePaths": ["/health", "/metrics"],
      "Sources": ["MyApp.Source"],
      "AddConsoleExporter": true
    },
    "Metrics": {
      "Enabled": true,
      "Meters": ["MyApp.Meter"],
      "AddConsoleExporter": false
    },
    "Logs": {
      "Enabled": true,
      "IncludeFormattedMessage": true,
      "IncludeScopes": false,
      "AddConsoleExporter": false
    }
  }
}
```


### OTel Configuration Properties Reference

| Property Path | Description | Default Value |
|--------------|-------------|--------------|
| OTel:ServiceName | The name of the service/application | Assembly name |
| OTel:ServiceNamespace | The namespace for the service | (empty) |
| OTel:Enabled | Enables/disables OpenTelemetry | true |
| OTel:OtlpExporter:Endpoint | The OTLP collector endpoint URL | "http://localhost:4317" |
| OTel:OtlpExporter:Protocol | Export protocol ("grpc" or "httpProtobuf") | "grpc" |
| OTel:OtlpExporter:Headers | Custom headers for OTLP requests | (empty) |
| OTel:OtlpExporter:TimeoutMilliseconds | Exporter timeout in milliseconds | 10000 |
| OTel:OtlpExporter:ExportProcessorType | Export processor type ("Batch" or "Simple") | "Batch" |
| OTel:OtlpExporter:BatchExportProcessorOptions:MaxQueueSize | Max queue size for batch processor | 2048 |
| OTel:OtlpExporter:BatchExportProcessorOptions:ScheduledDelayMilliseconds | Delay between batch exports (ms) | 5000 |
| OTel:OtlpExporter:BatchExportProcessorOptions:ExporterTimeoutMilliseconds | Timeout for batch export (ms) | 30000 |
| OTel:OtlpExporter:BatchExportProcessorOptions:MaxExportBatchSize | Max batch size for export | 512 |
| OTel:Traces:Enabled | Enables/disables tracing | true |
| OTel:Traces:ExcludePaths | List of route paths to exclude from tracing | [] |
| OTel:Traces:Sources | Additional sources for tracing | [] |
| OTel:Traces:AddConsoleExporter | Enables console exporter for traces | false |
| OTel:Metrics:Enabled | Enables/disables metrics | true |
| OTel:Metrics:Meters | Additional meters for metrics | [] |
| OTel:Metrics:AddConsoleExporter | Enables console exporter for metrics | false |
| OTel:Logs:Enabled | Enables/disables logging | true |
| OTel:Logs:IncludeFormattedMessage | Includes formatted messages in logs | false |
| OTel:Logs:IncludeScopes | Includes scopes in logs | false |
| OTel:Logs:AddConsoleExporter | Enables console exporter for logs | false |

### Example: Environment Variables
Cezzi.OTel supports standard OpenTelemetry environment variables for overrides:
- `OTEL_EXPORTER_OTLP_ENDPOINT`
- `OTEL_EXPORTER_OTLP_TRACES_ENDPOINT`, `OTEL_EXPORTER_OTLP_METRICS_ENDPOINT`, `OTEL_EXPORTER_OTLP_LOGS_ENDPOINT`
- `OTEL_EXPORTER_OTLP_PROTOCOL`, `OTEL_EXPORTER_OTLP_TRACES_PROTOCOL`, etc.
- `OTEL_EXPORTER_OTLP_HEADERS`, `OTEL_EXPORTER_OTLP_TRACES_HEADERS`, etc.
- `OTEL_EXPORTER_OTLP_TIMEOUT`, `OTEL_EXPORTER_OTLP_TRACES_TIMEOUT`, etc.

```bash
export OTEL_EXPORTER_OTLP_TRACES_ENDPOINT="http://localhost:1001"
export OTEL_EXPORTER_OTLP_METRICS_PROTOCOL="httpProtobuf"
```

### Example: Code-based Extension Usage
You can further customize telemetry providers using extension methods:
```csharp
builder.AddApplicationOpenTelemetry(
    traceConfigurator: tracing => tracing.AddSource("Custom.Source"),
    metricsConfigurator: metrics => metrics.AddMeter("Custom.Meter"),
    logsConfigurator: logging => logging.IncludeScopes = true,
    resourceConfigurator: resource => resource.WithElasticApm("production")
);
```

#### Elastic APM Resource Extension
```csharp
builder.AddApplicationOpenTelemetry(
    resourceConfigurator: resource => resource.WithElasticApm("production")
);
```

## 📚 Reference & Documentation
- [OpenTelemetry .NET Docs](https://opentelemetry.io/docs/instrumentation/net/)
- See source files in `src/Cezzi.OTel/` for available options and extension methods

## 🤝 Contributing
Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

## 📝 License
MIT License

---

> Maintained by [mtnvencenzo](https://github.com/mtnvencenzo) and contributors.
