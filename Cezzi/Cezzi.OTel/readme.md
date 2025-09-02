
# 📦 Cezzi.OTel

## 🚀 Overview
Cezzi.OTel is a .NET library designed to simplify the integration of [OpenTelemetry](https://opentelemetry.io/) into your applications. It provides a set of extension methods and configuration options to enable distributed tracing, metrics, and logging with minimal setup.

## 🎯 Features
- Easy setup for OpenTelemetry Traces, Metrics, and Logs
- Flexible exporter configuration
- Extensible options for custom telemetry scenarios
- Designed for .NET 9 and compatible with modern .NET projects

## 🛠️ Installation
Add the package to your project using NuGet:

```bash
# Using .NET CLI
dotnet add package Cezzi.OTel
```

## ⚡ Getting Started
1. Reference `Cezzi.OTel` in your project.
2. Use the provided extension methods to configure OpenTelemetry in your application's startup code.

```csharp
using Cezzi.OTel;

var builder = Host.CreateApplicationBuilder();
builder.AddApplicationOpenTelemetry();
var app = builder.Build();
```

## ⚙️ Configuration via `appsettings.json`
You can fully customize traces, metrics, and logs using the `OTel` section in your `appsettings.json`:

```json
{
    "OTel": {
        "ServiceName": "MyService",
        "ServiceNamespace": "MyNamespace",
        "Enabled": true,
        "Traces": {
            "Enabled": true,
            "ExcludePaths": ["/health", "/metrics"],
            "Sources": ["MyApp.Source"],
            "AddConsoleExporter": true,
            "OtlpExporter": {
                "Endpoint": "http://localhost:4317",
                "Protocol": "grpc",
                "Headers": "auth1=test1"
            }
        },
        "Metrics": {
            "Enabled": true,
            "Meters": ["MyApp.Meter"],
            "AddConsoleExporter": false,
            "OtlpExporter": {
                "Endpoint": "http://localhost:4317",
                "Protocol": "httpProtobuf",
                "Headers": "auth2=test2"
            }
        },
        "Logs": {
            "Enabled": true,
            "IncludeFormattedMessage": true,
            "IncludeScopes": false,
            "AddConsoleExporter": false,
            "OtlpExporter": {
                "Endpoint": "http://localhost:4317",
                "Protocol": "grpc",
                "Headers": "auth3=test3"
            }
        }
    }
}
```

## 🌍 Environment Variable Support
Cezzi.OTel supports overriding configuration via environment variables. Common variables include:

- `OTEL_EXPORTER_OTLP_ENDPOINT` (default endpoint for all signals)
- `OTEL_EXPORTER_OTLP_TRACES_ENDPOINT`, `OTEL_EXPORTER_OTLP_METRICS_ENDPOINT`, `OTEL_EXPORTER_OTLP_LOGS_ENDPOINT`
- `OTEL_EXPORTER_OTLP_PROTOCOL`, `OTEL_EXPORTER_OTLP_TRACES_PROTOCOL`, etc.
- `OTEL_EXPORTER_OTLP_HEADERS`, `OTEL_EXPORTER_OTLP_TRACES_HEADERS`, etc.
- `OTEL_EXPORTER_OTLP_TIMEOUT`, `OTEL_EXPORTER_OTLP_TRACES_TIMEOUT`, etc.

Example:
```bash
export OTEL_EXPORTER_OTLP_TRACES_ENDPOINT="http://localhost:1001"
export OTEL_EXPORTER_OTLP_METRICS_PROTOCOL="httpProtobuf"
```

## 🧩 Advanced Usage
You can combine appsettings and environment variables. Environment variables take precedence if set.

```csharp
var builder = Host.CreateApplicationBuilder();
builder.Configuration.AddEnvironmentVariables();
builder.AddApplicationOpenTelemetry();
var app = builder.Build();
```

## 📚 Documentation
- [OpenTelemetry .NET Docs](https://opentelemetry.io/docs/instrumentation/net/)
- See source files in `src/Cezzi.OTel/` for available options and extension methods.

## 🤝 Contributing
Contributions are welcome! Please open issues or submit pull requests for improvements or bug fixes.

## 📝 License
This project is licensed under the MIT License.

---

> Maintained by [mtnvencenzo](https://github.com/mtnvencenzo) and contributors.
