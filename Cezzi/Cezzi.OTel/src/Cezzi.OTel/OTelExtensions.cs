namespace Cezzi.OTel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Reflection;

/// <summary>Open Telemetry extensions for configuring traces, metrics and logging</summary>
public static class OTelExtensions
{
    private const string ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT = "OTEL_EXPORTER_OTLP_TRACES_ENDPOINT";
    private const string ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL = "OTEL_EXPORTER_OTLP_PROTOCOL";
    private const string ENVKEV_OTEL_EXPORTER_OTLP_HEADERS = "OTEL_EXPORTER_OTLP_HEADERS";
    private const string ENVKEV_OTEL_EXPORTER_OTLP_TIMEOUT = "OTEL_EXPORTER_OTLP_TIMEOUT";

    /// <summary>
    /// Adds OpenTelemetry to the application pipeline.
    /// </summary>
    /// <param name="builder">The application builder to configure OpenTelemetry for.</param>
    /// <param name="traceConfigurator">Optional delegate to further configure the TracerProviderBuilder.</param>
    /// <param name="metricsConfigurator">Optional delegate to further configure the MeterProviderBuilder.</param>
    /// <param name="logsConfigurator">Optional delegate to further configure the OpenTelemetryLoggerOptions.</param>
    /// <param name="resourceConfigurator">Optional delegate to further configure the ResourceBuilder for telemetry resources.</param>
    /// <returns>The application builder with OpenTelemetry configured.</returns>
    public static IHostApplicationBuilder AddApplicationOpenTelemetry(
        this IHostApplicationBuilder builder,
        Func<TracerProviderBuilder, TracerProviderBuilder> traceConfigurator = null,
        Func<MeterProviderBuilder, MeterProviderBuilder> metricsConfigurator = null,
        Func<OpenTelemetryLoggerOptions, OpenTelemetryLoggerOptions> logsConfigurator = null,
        Func<ResourceBuilder, ResourceBuilder> resourceConfigurator = null)
    {
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

        OTelOptions options = new();
        builder.Configuration.GetSection(OTelOptions.SectionName).Bind(options);

        builder.AddLogProvider(options, logsConfigurator, resourceConfigurator);
        builder.AddTraceProvider(options, traceConfigurator, resourceConfigurator);
        builder.AddMetricProvider(options, metricsConfigurator, resourceConfigurator);

        return builder;
    }

    /// <summary>
    /// Adds Elastic APM-compatible resource attributes to the <see cref="ResourceBuilder"/>.
    /// </summary>
    /// <param name="builder">The <see cref="ResourceBuilder"/> to add attributes to.</param>
    /// <param name="environment">The deployment environment name (e.g., "production", "staging").</param>
    /// <returns>The <see cref="ResourceBuilder"/> with Elastic APM attributes added.</returns>
    public static ResourceBuilder WithElasticApm(this ResourceBuilder builder, string environment)
    {
        return builder.AddAttributes([
            new KeyValuePair<string, object>("deployment.environment", environment),
            new KeyValuePair<string, object>("host.hostname", Environment.MachineName.ToLowerInvariant()),
            new KeyValuePair<string, object>("host.name", Environment.MachineName.ToLowerInvariant())
        ]);
    }

    private static IHostApplicationBuilder AddTraceProvider(
        this IHostApplicationBuilder builder,
        OTelOptions otelOptions,
        Func<TracerProviderBuilder, TracerProviderBuilder> configure = null,
        Func<ResourceBuilder, ResourceBuilder> resourceConfigurator = null)
    {
        var traceOptions = otelOptions.Traces;

        var endpoint = GetOtlpValue(
            config: builder.Configuration["OTel:OtlpExporter:Endpoint"],
            specificEnv: "OTEL_EXPORTER_OTLP_TRACES_ENDPOINT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT);

        if (!(traceOptions?.Enabled ?? true) || string.IsNullOrWhiteSpace(endpoint))
        {
            return builder;
        }

        var protocol = GetOtlpProtocolValue(
            config: otelOptions.OtlpExporter?.Protocol.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_TRACES_PROTOCOL",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL);

        var headers = GetOtlpValue(
            config: otelOptions.OtlpExporter?.Headers?.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_TRACES_HEADERS",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_HEADERS);

        var timeoutMilliseconds = GetOtlpValue(
            config: otelOptions.OtlpExporter?.TimeoutMilliseconds.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_TRACES_TIMEOUT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_TIMEOUT);

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(GetResourceBuilder(otelOptions, resourceConfigurator))
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

                tracing.AddOtlpExporter("traces", options => ApplyOtlpExporterOptions(options, endpoint, protocol, headers, timeoutMilliseconds, otelOptions.OtlpExporter));

                if (traceOptions?.AddConsoleExporter ?? false)
                {
                    tracing.AddConsoleExporter();
                }

                configure?.Invoke(tracing);
            });

        return builder;
    }

    private static IHostApplicationBuilder AddMetricProvider(
        this IHostApplicationBuilder builder,
        OTelOptions otelOptions,
        Func<MeterProviderBuilder, MeterProviderBuilder> configure = null,
        Func<ResourceBuilder, ResourceBuilder> resourceConfigurator = null)
    {
        var metricOptions = otelOptions.Metrics;

        var endpoint = GetOtlpValue(
            config: builder.Configuration["OTel:OtlpExporter:Endpoint"],
            specificEnv: "OTEL_EXPORTER_OTLP_METRICS_ENDPOINT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT);

        if (!(metricOptions?.Enabled ?? true) || string.IsNullOrWhiteSpace(endpoint))
        {
            return builder;
        }

        var protocol = GetOtlpProtocolValue(
            config: otelOptions.OtlpExporter?.Protocol.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_METRICS_PROTOCOL",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL);

        var headers = GetOtlpValue(
            config: otelOptions.OtlpExporter?.Headers?.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_METRICS_HEADERS",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_HEADERS);

        var timeoutMilliseconds = GetOtlpValue(
            config: otelOptions.OtlpExporter?.TimeoutMilliseconds.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_METRICS_TIMEOUT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_TIMEOUT);

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(GetResourceBuilder(otelOptions, resourceConfigurator))
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation();

                metricOptions?.Meters?.ForEach(x => metrics.AddMeter(x));

                metrics.AddOtlpExporter("metrics", options => ApplyOtlpExporterOptions(options, endpoint, protocol, headers, timeoutMilliseconds, otelOptions.OtlpExporter));

                if (metricOptions?.AddConsoleExporter ?? false)
                {
                    metrics.AddConsoleExporter();
                }

                configure?.Invoke(metrics);
            });

        return builder;
    }

    private static IHostApplicationBuilder AddLogProvider(
        this IHostApplicationBuilder builder,
        OTelOptions otelOptions,
        Func<OpenTelemetryLoggerOptions, OpenTelemetryLoggerOptions> configure = null,
        Func<ResourceBuilder, ResourceBuilder> resourceConfigurator = null)
    {
        var logOptions = otelOptions.Logs;

        var endpoint = GetOtlpValue(
            config: builder.Configuration["OTel:OtlpExporter:Endpoint"],
            specificEnv: "OTEL_EXPORTER_OTLP_LOGS_ENDPOINT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT);

        if (!(logOptions?.Enabled ?? true) || string.IsNullOrWhiteSpace(endpoint))
        {
            return builder;
        }

        var protocol = GetOtlpProtocolValue(
            config: otelOptions.OtlpExporter?.Protocol.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_LOGS_PROTOCOL",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL);

        var headers = GetOtlpValue(
            config: otelOptions.OtlpExporter?.Headers?.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_LOGS_HEADERS",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_HEADERS);

        var timeoutMilliseconds = GetOtlpValue(
            config: otelOptions.OtlpExporter?.TimeoutMilliseconds.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_LOGS_TIMEOUT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_TIMEOUT);

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.SetResourceBuilder(GetResourceBuilder(otelOptions, resourceConfigurator));
            logging.IncludeFormattedMessage = logOptions?.IncludeFormattedMessage ?? false;
            logging.IncludeScopes = logOptions?.IncludeScopes ?? false;
            logging.AddOtlpExporter("logs", options => ApplyOtlpExporterOptions(options, endpoint, protocol, headers, timeoutMilliseconds, otelOptions.OtlpExporter));

            if (logOptions?.AddConsoleExporter ?? false)
            {
                logging.AddConsoleExporter();
            }

            configure?.Invoke(logging);
        });

        return builder;
    }

    private static string GetOtlpProtocolValue(string config, string specificEnv, string defaultEnv)
    {
        // Removing slashes from http/protobuf in ENV specific value
        return GetOtlpValue(config, specificEnv, defaultEnv)?.Replace("/", string.Empty);
    }

    private static string GetOtlpValue(string config, string specificEnv, string defaultEnv) => Environment.GetEnvironmentVariable(specificEnv)
        ?? Environment.GetEnvironmentVariable(defaultEnv)
        ?? config?.ToString();

    private static ResourceBuilder GetResourceBuilder(OTelOptions otelOptions, Func<ResourceBuilder, ResourceBuilder> configure = null)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceName: otelOptions.ServiceName ?? (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name,
            serviceNamespace: otelOptions.ServiceNamespace,
            serviceVersion: (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName.ToLowerInvariant());

        configure?.Invoke(resourceBuilder);

        return resourceBuilder;
    }

    private static void ApplyOtlpExporterOptions(OtlpExporterOptions options, string endpoint, string protocol, string headers, string timeoutMilliseconds, OtlpExporterOptions configured)
    {
        options.Headers = headers ?? options.Headers;
        options.ExportProcessorType = configured?.ExportProcessorType ?? options.ExportProcessorType;
        options.BatchExportProcessorOptions = configured?.BatchExportProcessorOptions ?? options.BatchExportProcessorOptions;

        if (Uri.IsWellFormedUriString(endpoint, UriKind.Absolute))
        {
            options.Endpoint = new Uri(endpoint);
        }

        if (Enum.TryParse<OtlpExportProtocol>(protocol, true, out var parsedProtocol))
        {
            options.Protocol = parsedProtocol;
        }

        if (int.TryParse(timeoutMilliseconds, out var parsedTimeoutMilliseconds))
        {
            options.TimeoutMilliseconds = parsedTimeoutMilliseconds;
        }
    }
}