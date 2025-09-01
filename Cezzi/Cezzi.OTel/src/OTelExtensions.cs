namespace Cezzi.OTel;

using System;
using System.Linq;
using Cocktails.Api.Application.Behaviors.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

internal static class OTelExtensions
{
    internal static IHostApplicationBuilder AddApplicationOpenTelemetry(this IHostApplicationBuilder builder, string serviceName)
    {
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

        builder.AddTraceProvider(serviceName);
        builder.AddMetricProvider(serviceName);
        builder.AddLogProvider(serviceName);

        return builder;
    }

    private static IHostApplicationBuilder AddTraceProvider(this IHostApplicationBuilder builder, string serviceName)
    {
        OtelTracesOptions traceOptions = new();
        builder.Configuration.GetSection("OpenTelemetry:Traces").Bind(traceOptions);

        var endpoint = GetOtlpValue(traceOptions.OtlpExporter?.Endpoint?.ToString(), "OTEL_EXPORTER_OTLP_TRACES_ENDPOINT", "OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol = GetOtlpValue(traceOptions.OtlpExporter?.Protocol.ToString(), "OTEL_EXPORTER_OTLP_TRACES_PROTOCOL", "OTEL_EXPORTER_OTLP_PROTOCOL");
        var headers = GetOtlpValue(traceOptions.OtlpExporter?.Headers.ToString(), "OTEL_EXPORTER_OTLP_TRACES_HEADERS", "OTEL_EXPORTER_OTLP_HEADERS");
        var timeoutMilliseconds = GetOtlpValue(traceOptions.OtlpExporter?.TimeoutMilliseconds.ToString(), "OTEL_EXPORTER_OTLP_TRACES_TIMEOUT", "OTEL_EXPORTER_OTLP_TIMEOUT");

        var traceProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddHttpClientInstrumentation()
            .AddGrpcClientInstrumentation()
            .AddAspNetCoreInstrumentation((o) =>
            {
                o.Filter = (httpContext) => traceOptions.ExcludePaths
                    ?.Any(x => x.StartsWith(httpContext.Request.Path.Value, StringComparison.OrdinalIgnoreCase)) ?? false;
            });

        traceOptions.Sources?.ForEach(s => traceProviderBuilder.AddSource(s));

        if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
        {
            traceProviderBuilder.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(endpoint);
                options.Headers = headers;
                options.ExportProcessorType = traceOptions?.OtlpExporter?.ExportProcessorType ?? options.ExportProcessorType;
                options.TimeoutMilliseconds = int.TryParse(timeoutMilliseconds, out var parsedTimeoutMilliseconds)
                    ? parsedTimeoutMilliseconds
                    : 10000;
                options.Protocol = Enum.TryParse<OtlpExportProtocol>(protocol, true, out var parsedProtocol)
                    ? parsedProtocol
                    : OtlpExportProtocol.Grpc;
                options.BatchExportProcessorOptions = traceOptions?.OtlpExporter?.BatchExportProcessorOptions ?? options.BatchExportProcessorOptions;
            });
        }

        if (traceOptions.OtlpExporter?.AddConsoleExporter ?? false)
        {
            traceProviderBuilder.AddConsoleExporter();
        }

        traceProviderBuilder.Build();

        return builder;
    }

    private static IHostApplicationBuilder AddMetricProvider(this IHostApplicationBuilder builder, string serviceName)
    {
        OtelMetricsOptions metricOptions = new();
        builder.Configuration.GetSection("OpenTelemetry:Metrics").Bind(metricOptions);

        var endpoint = GetOtlpValue(metricOptions?.OtlpExporter?.Endpoint?.ToString(), "OTEL_EXPORTER_OTLP_METRICS_ENDPOINT", "OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol = GetOtlpValue(metricOptions?.OtlpExporter?.Protocol.ToString(), "OTEL_EXPORTER_OTLP_METRICS_PROTOCOL", "OTEL_EXPORTER_OTLP_PROTOCOL");
        var headers = GetOtlpValue(metricOptions?.OtlpExporter?.Headers.ToString(), "OTEL_EXPORTER_OTLP_METRICS_HEADERS", "OTEL_EXPORTER_OTLP_HEADERS");
        var timeoutMilliseconds = GetOtlpValue(metricOptions?.OtlpExporter?.TimeoutMilliseconds.ToString(), "OTEL_EXPORTER_OTLP_METRICS_TIMEOUT", "OTEL_EXPORTER_OTLP_TIMEOUT");

        var meterProviderBuilder = Sdk.CreateMeterProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
            .AddRuntimeInstrumentation()
            .AddAspNetCoreInstrumentation();

        metricOptions.Meters?.ForEach(x => meterProviderBuilder.AddMeter(x));

        if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
        {
            meterProviderBuilder.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(endpoint);
                options.Headers = headers;
                options.ExportProcessorType = metricOptions?.OtlpExporter?.ExportProcessorType ?? options.ExportProcessorType;
                options.TimeoutMilliseconds = int.TryParse(timeoutMilliseconds, out var parsedTimeoutMilliseconds)
                    ? parsedTimeoutMilliseconds
                    : 10000;
                options.Protocol = Enum.TryParse<OtlpExportProtocol>(protocol, true, out var parsedProtocol)
                    ? parsedProtocol
                    : OtlpExportProtocol.Grpc;
                options.BatchExportProcessorOptions = metricOptions?.OtlpExporter?.BatchExportProcessorOptions ?? options.BatchExportProcessorOptions;
            });
        }

        if (metricOptions.OtlpExporter?.AddConsoleExporter ?? false)
        {
            meterProviderBuilder.AddConsoleExporter();
        }

        meterProviderBuilder.Build();

        return builder;
    }

    private static IHostApplicationBuilder AddLogProvider(this IHostApplicationBuilder builder, string serviceName)
    {
        OtelLogsOptions logOptions = new();
        builder.Configuration.GetSection("OpenTelemetry:Logs").Bind(logOptions);

        var endpoint = GetOtlpValue(logOptions?.OtlpExporter?.Endpoint?.ToString(), "OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol = GetOtlpValue(logOptions?.OtlpExporter?.Protocol.ToString(), "OTEL_EXPORTER_OTLP_LOGS_PROTOCOL", "OTEL_EXPORTER_OTLP_PROTOCOL");
        var headers = GetOtlpValue(logOptions?.OtlpExporter?.Headers.ToString(), "OTEL_EXPORTER_OTLP_LOGS_HEADERS", "OTEL_EXPORTER_OTLP_HEADERS");
        var timeoutMilliseconds = GetOtlpValue(logOptions?.OtlpExporter?.TimeoutMilliseconds.ToString(), "OTEL_EXPORTER_OTLP_LOGS_TIMEOUT", "OTEL_EXPORTER_OTLP_TIMEOUT");

        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddOpenTelemetry(logging =>
            {
                logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName));
                logging.IncludeFormattedMessage = logOptions.IncludeFormattedMessage;
                logging.IncludeScopes = logOptions.IncludeScopes;

                if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
                {
                    logging.AddOtlpExporter(options =>
                    {
                        options.Endpoint = new Uri(endpoint);
                        options.Headers = headers;
                        options.ExportProcessorType = logOptions?.OtlpExporter?.ExportProcessorType ?? options.ExportProcessorType;
                        options.TimeoutMilliseconds = int.TryParse(timeoutMilliseconds, out var parsedTimeoutMilliseconds)
                            ? parsedTimeoutMilliseconds
                            : 10000;
                        options.Protocol = Enum.TryParse<OtlpExportProtocol>(protocol, true, out var parsedProtocol)
                            ? parsedProtocol
                            : OtlpExportProtocol.Grpc;
                        options.BatchExportProcessorOptions = logOptions?.OtlpExporter?.BatchExportProcessorOptions ?? options.BatchExportProcessorOptions;
                    });
                }

                if (logOptions.OtlpExporter?.AddConsoleExporter ?? false)
                {
                    logging.AddConsoleExporter();
                }
            });
        });

        return builder;
    }

    private static string GetOtlpValue(string config, string specificEnv, string defaultEnv) => config?.ToString()
        ?? Environment.GetEnvironmentVariable(specificEnv)
        ?? Environment.GetEnvironmentVariable(defaultEnv);
}
