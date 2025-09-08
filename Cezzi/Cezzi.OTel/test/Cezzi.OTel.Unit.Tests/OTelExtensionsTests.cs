namespace Cezzi.OTel.Unit.Tests;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Xunit;
using Cezzi.OTel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenTelemetry.Exporter;
using FluentAssertions;

public class OTelExtensionsTests
{
    [Fact]
    public void otel___loads_providers_when_endpoint_not_configured()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // Verify provider setup
        app.Services.GetService<OpenTelemetry.Trace.TracerProvider>().Should().NotBeNull();
        app.Services.GetService<OpenTelemetry.Metrics.MeterProvider>().Should().NotBeNull();
        app.Services.GetService<OpenTelemetry.Logs.LoggerProvider>().Should().NotBeNull();
    }

    [Fact]
    public void otel___does_not_load_trace_provider_when_not_configured()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:OtlpExporter:Endpoint", "http://otel-collector:4317" },
            { "OTel:Traces:Enabled", "false" }
        });

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // Verify meter provider setup
        app.Services.GetService<OpenTelemetry.Trace.TracerProvider>().Should().BeNull();
    }

    [Fact]
    public void otel___does_not_load_metrics_provider_when_not_configured()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:OtlpExporter:Endpoint", "http://otel-collector:4317" },
            { "OTel:Metrics:Enabled", "false" }
        });

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // Verify meter provider setup
        app.Services.GetService<OpenTelemetry.Metrics.MeterProvider>().Should().BeNull();
    }

    [Fact]
    public void otel___does_not_load_logs_provider_when_not_configured()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:OtlpExporter:Endpoint", "http://otel-collector:4317" },
            { "OTel:Logs:Enabled", "false" }
        });

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // Verify meter provider setup
        app.Services.GetService<OpenTelemetry.Logs.LoggerProvider>().Should().BeNull();
    }

    [Fact]
    public void otel___loads_providers_with_no_configuration_specified()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:OtlpExporter:Endpoint", "http://otel-collector:4317" }
        });

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // assert
        void AssertOtlpExporter(OtlpExporterOptions opts)
        {
            opts.Should().NotBeNull();
            opts.Endpoint.Should().Be(new Uri("http://otel-collector:4317"));
            opts.Protocol.Should().Be(OtlpExportProtocol.Grpc);
            opts.Headers.Should().BeNull();
            opts.ExportProcessorType.Should().Be(OpenTelemetry.ExportProcessorType.Batch);
            opts.TimeoutMilliseconds.Should().Be(10000);
            opts.BatchExportProcessorOptions.ExporterTimeoutMilliseconds.Should().Be(30000);
            opts.BatchExportProcessorOptions.MaxExportBatchSize.Should().Be(512);
            opts.BatchExportProcessorOptions.MaxQueueSize.Should().Be(2048);
            opts.BatchExportProcessorOptions.ScheduledDelayMilliseconds.Should().Be(5000);
        }

        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Trace.TracerProvider>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.AspNetCore.AspNetCoreTraceInstrumentationOptions>>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.GrpcNetClient.GrpcClientInstrumentationOptions>>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.Http.HttpClientTraceInstrumentationOptions>>();
        var exporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("traces");
        AssertOtlpExporter(exporter);

        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Metrics.MeterProvider>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.Runtime.RuntimeInstrumentationOptions>>();
        exporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("metrics");
        AssertOtlpExporter(exporter);

        // Verify logger provider setup
        app.Services.GetRequiredService<OpenTelemetry.Logs.LoggerProvider>();
        exporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("logs");
        AssertOtlpExporter(exporter);
    }

    [Fact]
    public void otel___loads_provider_otel_exporters_with_config_settings()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:OtlpExporter:Endpoint", "http://localhost:1001" },
            { "OTel:OtlpExporter:Protocol", "grpc" },
            { "OTel:OtlpExporter:Headers", "auth1=test1" },
            { "OTel:OtlpExporter:ExportProcessorType", "simple" },
            { "OTel:OtlpExporter:TimeoutMilliseconds", "12" },
            { "OTel:OtlpExporter:BatchExportProcessorOptions:ExporterTimeoutMilliseconds", "11" },
            { "OTel:OtlpExporter:BatchExportProcessorOptions:MaxExportBatchSize", "12" },
            { "OTel:OtlpExporter:BatchExportProcessorOptions:MaxQueueSize", "13" },
            { "OTel:OtlpExporter:BatchExportProcessorOptions:ScheduledDelayMilliseconds", "14" }
        });

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // assert
        void AssertOtlpExporter(OtlpExporterOptions opts)
        {
            opts.Should().NotBeNull();
            opts.Endpoint.Should().Be(new Uri("http://localhost:1001"));
            opts.Protocol.Should().Be(OtlpExportProtocol.Grpc);
            opts.Headers.Should().Be("auth1=test1");
            opts.ExportProcessorType.Should().Be(OpenTelemetry.ExportProcessorType.Simple);
            opts.TimeoutMilliseconds.Should().Be(12);
            opts.BatchExportProcessorOptions.ExporterTimeoutMilliseconds.Should().Be(11);
            opts.BatchExportProcessorOptions.MaxExportBatchSize.Should().Be(12);
            opts.BatchExportProcessorOptions.MaxQueueSize.Should().Be(13);
            opts.BatchExportProcessorOptions.ScheduledDelayMilliseconds.Should().Be(14);
        }

        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Trace.TracerProvider>();
        var exporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("traces");
        AssertOtlpExporter(exporter);

        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Metrics.MeterProvider>();
        exporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("metrics");
        AssertOtlpExporter(exporter);

        // Verify logger provider setup
        app.Services.GetRequiredService<OpenTelemetry.Logs.LoggerProvider>();
        exporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("logs");
        AssertOtlpExporter(exporter);
    }

    [Fact]
    public void otel___calls_provider_hooks_when_configured()
    {
        // arrange
        var traceCounter = 0;
        var metricsCounter = 0;
        var logsCounter = 0;
        var resourceCounter = 0;

        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:OtlpExporter:Endpoint", "http://localhost:1001" }
        });

        // act
        builder.AddApplicationOpenTelemetry(
            traceConfigurator: (t) =>
            {
                traceCounter++;
                return t;
            },
            metricsConfigurator: (m) =>
            {
                metricsCounter++;
                return m;
            },
            logsConfigurator: (l) =>
            {
                logsCounter++;
                return l;
            },
            resourceConfigurator: (r) =>
            {
                resourceCounter++;
                return r;
            }
        );

        var app = builder.Build();

        // Assert
        traceCounter.Should().Be(1);
        metricsCounter.Should().Be(1);
        logsCounter.Should().Be(1);
        resourceCounter.Should().Be(3);
    }
}
