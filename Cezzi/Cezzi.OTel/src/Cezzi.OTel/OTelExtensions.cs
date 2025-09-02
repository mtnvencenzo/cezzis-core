namespace Cezzi.OTel;

using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

/// <summary>
/// Open Telemetry extensions for configuring traces, metrics and logging
/// </summary>
public static class OTelExtensions
{
    /// <summary>
    /// Adds open telelemtry to the application pipeline
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="serviceName"></param>
    /// <param name="serviceNamespace"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder AddApplicationOpenTelemetry(this IHostApplicationBuilder builder, string serviceName = null, string serviceNamespace = null)
    {
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

        OTelOptions options = new();
        builder.Configuration.AddEnvironmentVariables();
        builder.Configuration.GetSection(OTelOptions.SectionName).Bind(options);

        if (options.Enabled)
        {
            builder.AddTraceProvider(options, serviceName, serviceNamespace);
            builder.AddMetricProvider(options, serviceName, serviceNamespace);
            builder.AddLogProvider(options, serviceName, serviceNamespace);
        }

        return builder;
    }

    private static IHostApplicationBuilder AddTraceProvider(this IHostApplicationBuilder builder, OTelOptions otelOptions, string serviceName = null, string serviceNamespace = null)
    {
        var traceOptions = otelOptions.Traces;

        if (!(traceOptions?.Enabled ?? true))
        {
            return builder;
        }

        var endpoint = GetOtlpValue(traceOptions?.OtlpExporter?.Endpoint?.ToString(), "OTEL_EXPORTER_OTLP_TRACES_ENDPOINT", "OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol = GetOtlpProtocolValue(traceOptions?.OtlpExporter?.Protocol.ToString(), "OTEL_EXPORTER_OTLP_TRACES_PROTOCOL", "OTEL_EXPORTER_OTLP_PROTOCOL");
        var headers = GetOtlpValue(traceOptions?.OtlpExporter?.Headers?.ToString(), "OTEL_EXPORTER_OTLP_TRACES_HEADERS", "OTEL_EXPORTER_OTLP_HEADERS");
        var timeoutMilliseconds = GetOtlpValue(traceOptions?.OtlpExporter?.TimeoutMilliseconds.ToString(), "OTEL_EXPORTER_OTLP_TRACES_TIMEOUT", "OTEL_EXPORTER_OTLP_TIMEOUT");

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName: serviceName ?? otelOptions.ServiceName ?? Assembly.GetExecutingAssembly().GetName().Name,
                        serviceNamespace: serviceNamespace ?? otelOptions.ServiceNamespace,
                        serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
                        serviceInstanceId: Environment.MachineName))
                    .AddHttpClientInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddAspNetCoreInstrumentation((o) =>
                    {
                        o.Filter = httpContext =>
                        {
                            var path = httpContext.Request.Path.Value ?? string.Empty;
                            var excluded = traceOptions?.ExcludePaths?.Any(p =>
                                path.StartsWith(p, StringComparison.OrdinalIgnoreCase)) == true;
                            return !excluded; // collect when not excluded  
                        };
                    });

                traceOptions?.Sources?.ForEach(s => tracing.AddSource(s));

                if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
                {
                    tracing.AddOtlpExporter("traces", options =>
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

                if (traceOptions?.AddConsoleExporter ?? false)
                {
                    tracing.AddConsoleExporter();
                }
            });

        var services = builder.Services;

        return builder;
    }

    private static IHostApplicationBuilder AddMetricProvider(this IHostApplicationBuilder builder, OTelOptions otelOptions, string serviceName = null, string serviceNamespace = null)
    {
        var metricOptions = otelOptions.Metrics;

        if (!(metricOptions?.Enabled ?? true))
        {
            return builder;
        }

        var endpoint = GetOtlpValue(metricOptions?.OtlpExporter?.Endpoint?.ToString(), "OTEL_EXPORTER_OTLP_METRICS_ENDPOINT", "OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol = GetOtlpProtocolValue(metricOptions?.OtlpExporter?.Protocol.ToString(), "OTEL_EXPORTER_OTLP_METRICS_PROTOCOL", "OTEL_EXPORTER_OTLP_PROTOCOL");
        var headers = GetOtlpValue(metricOptions?.OtlpExporter?.Headers?.ToString(), "OTEL_EXPORTER_OTLP_METRICS_HEADERS", "OTEL_EXPORTER_OTLP_HEADERS");
        var timeoutMilliseconds = GetOtlpValue(metricOptions?.OtlpExporter?.TimeoutMilliseconds.ToString(), "OTEL_EXPORTER_OTLP_METRICS_TIMEOUT", "OTEL_EXPORTER_OTLP_TIMEOUT");

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                        serviceName: serviceName ?? otelOptions.ServiceName ?? Assembly.GetExecutingAssembly().GetName().Name,
                        serviceNamespace: serviceNamespace ?? otelOptions.ServiceNamespace,
                        serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
                        serviceInstanceId: Environment.MachineName))
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation();

                metricOptions?.Meters?.ForEach(x => metrics.AddMeter(x));

                if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
                {
                    metrics.AddOtlpExporter("metrics", options =>
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

                if (metricOptions?.AddConsoleExporter ?? false)
                {
                    metrics.AddConsoleExporter();
                }
            });

        return builder;
    }

    private static IHostApplicationBuilder AddLogProvider(this IHostApplicationBuilder builder, OTelOptions otelOptions, string serviceName = null, string serviceNamespace = null)
    {
        var logOptions = otelOptions.Logs;

        if (!(logOptions?.Enabled ?? true))
        {
            return builder;
        }

        var endpoint = GetOtlpValue(logOptions?.OtlpExporter?.Endpoint?.ToString(), "OTEL_EXPORTER_OTLP_LOGS_ENDPOINT", "OTEL_EXPORTER_OTLP_ENDPOINT");
        var protocol = GetOtlpProtocolValue(logOptions?.OtlpExporter?.Protocol.ToString(), "OTEL_EXPORTER_OTLP_LOGS_PROTOCOL", "OTEL_EXPORTER_OTLP_PROTOCOL");
        var headers = GetOtlpValue(logOptions?.OtlpExporter?.Headers?.ToString(), "OTEL_EXPORTER_OTLP_LOGS_HEADERS", "OTEL_EXPORTER_OTLP_HEADERS");
        var timeoutMilliseconds = GetOtlpValue(logOptions?.OtlpExporter?.TimeoutMilliseconds.ToString(), "OTEL_EXPORTER_OTLP_LOGS_TIMEOUT", "OTEL_EXPORTER_OTLP_TIMEOUT");

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(
                serviceName: serviceName ?? otelOptions.ServiceName ?? Assembly.GetExecutingAssembly().GetName().Name,
                serviceNamespace: serviceNamespace ?? otelOptions.ServiceNamespace,
                serviceVersion: Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown",
                serviceInstanceId: Environment.MachineName));
            logging.IncludeFormattedMessage = logOptions?.IncludeFormattedMessage ?? false;
            logging.IncludeScopes = logOptions?.IncludeScopes ?? false;

            if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
            {
                logging.AddOtlpExporter("logs", options =>
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

            if (logOptions?.AddConsoleExporter ?? false)
            {
                logging.AddConsoleExporter();
            }
        });

        return builder;
    }

    /// <summary>Removing slashes from http/protobuf in ENV specific value</summary>
    private static string GetOtlpProtocolValue(string config, string specificEnv, string defaultEnv) =>
        GetOtlpValue(config, specificEnv?.Replace("/", string.Empty), defaultEnv);

    private static string GetOtlpValue(string config, string specificEnv, string defaultEnv) => config?.ToString()
        ?? Environment.GetEnvironmentVariable(specificEnv)
        ?? Environment.GetEnvironmentVariable(defaultEnv);
}