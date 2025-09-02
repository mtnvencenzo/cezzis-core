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
    public void otel___loads_providers_with_no_configuration_specified()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.Sources.Clear();

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // assert
        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Trace.TracerProvider>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.AspNetCore.AspNetCoreTraceInstrumentationOptions>>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.GrpcNetClient.GrpcClientInstrumentationOptions>>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.Http.HttpClientTraceInstrumentationOptions>>();
        var otlpExporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("traces");
        otlpExporter.Endpoint.Should().Be(new Uri("http://localhost:4317")); // Default value when not configured

        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Metrics.MeterProvider>();
        app.Services.GetRequiredService<IOptions<OpenTelemetry.Instrumentation.Runtime.RuntimeInstrumentationOptions>>();
        otlpExporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("metrics");
        otlpExporter.Endpoint.Should().Be(new Uri("http://localhost:4317")); // Default value when not configured

        // Verify logger provider setup
        app.Services.GetRequiredService<OpenTelemetry.Logs.LoggerProvider>();
        otlpExporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("logs");
        otlpExporter.Endpoint.Should().Be(new Uri("http://localhost:4317")); // Default value when not configured
    }

    [Fact]
    public void otel___loads_provider_otel_exporters_with_config_settings()
    {
        // arrange
        var builder = Host.CreateApplicationBuilder();
        builder.Configuration.Sources.Clear();
        builder.Configuration.AddInMemoryCollection(new Dictionary<string, string>
        {
            { "OTel:Traces:OtlpExporter:Endpoint", "http://localhost:1001" },
            { "OTel:Traces:OtlpExporter:Protocol", "grpc" },
            { "OTel:Traces:OtlpExporter:Headers", "auth1=test1" },
            { "OTel:Metrics:OtlpExporter:Endpoint", "http://localhost:1002" },
            { "OTel:Metrics:OtlpExporter:Protocol", "httpProtobuf" },
            { "OTel:Metrics:OtlpExporter:Headers", "auth2=test2" },
            { "OTel:Logs:OtlpExporter:Endpoint", "http://localhost:1003" },
            { "OTel:Logs:OtlpExporter:Protocol", "GRPC" },
            { "OTel:Logs:OtlpExporter:Headers", "auth3=test3" },
        });

        // act
        builder.AddApplicationOpenTelemetry();
        var app = builder.Build();

        // assert
        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Trace.TracerProvider>();
        var tracesExporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("traces");
        tracesExporter.Should().NotBeNull();
        tracesExporter.Endpoint.Should().Be(new Uri("http://localhost:1001"));
        tracesExporter.Protocol.Should().Be(OtlpExportProtocol.Grpc);
        tracesExporter.Headers.Should().Be("auth1=test1");

        // Verify meter provider setup
        app.Services.GetRequiredService<OpenTelemetry.Metrics.MeterProvider>();
        var metricsExporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("metrics");
        metricsExporter.Endpoint.Should().Be(new Uri("http://localhost:1002"));
        metricsExporter.Protocol.Should().Be(OtlpExportProtocol.HttpProtobuf);
        metricsExporter.Headers.Should().Be("auth2=test2");

        // Verify logger provider setup
        app.Services.GetRequiredService<OpenTelemetry.Logs.LoggerProvider>();
        var logsExporter = app.Services.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get("logs");
        logsExporter.Endpoint.Should().Be(new Uri("http://localhost:1003"));
        logsExporter.Protocol.Should().Be(OtlpExportProtocol.Grpc);
        logsExporter.Headers.Should().Be("auth3=test3");
    }
}
