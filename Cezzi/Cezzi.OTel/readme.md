# 📦 Cezzi.OTel

## 🚀 What is Cezzi.OTel?
Cezzi.OTel is a professional .NET library and NuGet package for integrating [OpenTelemetry](https://opentelemetry.io/) into your applications. It provides robust extension methods and configuration options for distributed tracing, metrics, and logging, making observability simple and consistent across your services.

## 🎯 Key Features
- Plug-and-play OpenTelemetry setup for Traces, Metrics, and Logs
- Easy configuration via appsettings
- Flexible configuration via appsettings, environment variables, or code
- Advanced OTLP exporter configuration via DI
- Extensible options for custom telemetry scenarios
- .NET 9+ support and modern best practices

## Usage

Add OpenTelemetry to your application:

```csharp
builder.AddApplicationOpenTelemetry();
```

## Configuration

Configure via `appsettings.json` under the `OTel` section:

```json
{
  "OTel": {
    "ServiceName": "MyService",
    "ServiceNamespace": "MyNamespace",
    "Traces": {
      "Enabled": true,
      "ExcludePaths": ["/health"],
      "Sources": ["MyActivitySource"],
      "AddConsoleExporter": false
    },
    "Logs": {
      "Enabled": true,
      "IncludeFormattedMessage": false,
      "IncludeScopes": false,
      "AddConsoleExporter": false
    },
    "Metrics": {
      "Enabled": true,
      "Meters": ["MyMeter"],
      "AddConsoleExporter": false
    },
    "Resource": {
      "Attributes": {
        "deployment.environment": "production"
      }
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
      configureTracing: tracing => tracing.AddSource("Custom.Source"),
      configureMetrics: metrics => metrics.AddMeter("Custom.Meter"),
      coinfigureLogging: logging => logging.IncludeScopes = true,
      configureResource: resource => resource.WithElasticApm("production")
  );
```

### Advanced: Custom OTLP Exporter Configuration

You can register a custom configurator to modify OTLP exporter options after DI is built:

```csharp
builder.AddOtlpExporterConfigurator<MyOtlpConfigurator>();
```

Implement `IOtlpExporterConfigurator`:

```csharp
using OpenTelemetry.Exporter;

public class MyOtlpConfigurator(IMyService myService) : IOtlpExporterConfigurator
{
    public bool Configure(string discriminator, OtlpExporterOptions options)
    {
        var token = myService.GetToken();

        // Custom logic to modify options
        options.Headers = $"Authorization=API-Token {token}";
        return true;
    }
}
```


#### Elastic APM Resource Extension
Add common resource attributes that are used within Elastic Stack / Kibana
```csharp
  builder.AddApplicationOpenTelemetry(
    configureResource: resource => resource.WithElasticApm("production")
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
