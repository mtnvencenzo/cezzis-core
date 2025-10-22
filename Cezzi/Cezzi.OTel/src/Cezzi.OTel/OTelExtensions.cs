namespace Cezzi.OTel;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
    /// <param name="configureTracing">Optional delegate to further configure the TracerProviderBuilder.</param>
    /// <param name="configureMetrics">Optional delegate to further configure the MeterProviderBuilder.</param>
    /// <param name="configureLogging">Optional delegate to further configure the OpenTelemetryLoggerOptions.</param>
    /// <param name="configureResource">Optional delegate to further configure the ResourceBuilder for telemetry resources.</param>
    /// <returns>The application builder with OpenTelemetry configured.</returns>
    public static IHostApplicationBuilder AddApplicationOpenTelemetry(
        this IHostApplicationBuilder builder,
        Func<TracerProviderBuilder, TracerProviderBuilder> configureTracing = null,
        Func<MeterProviderBuilder, MeterProviderBuilder> configureMetrics = null,
        Func<OpenTelemetryLoggerOptions, OpenTelemetryLoggerOptions> configureLogging = null,
        Func<ResourceBuilder, ResourceBuilder> configureResource = null)
    {
        AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

        OTelOptions otelOptions = new();
        builder.Configuration.GetSection(OTelOptions.SectionName).Bind(otelOptions);
        builder.Services.AddSingleton(otelOptions);

        var resourceBuilder = GetResourceBuilder(otelOptions, configureResource);

        builder.AddTraceProvider(
            otelOptions: otelOptions,
            resourceBuilder: resourceBuilder,
            configureTracing: configureTracing);

        builder.AddMeterProvider(
            otelOptions: otelOptions,
            resourceBuilder: resourceBuilder,
            configureMetrics: configureMetrics);

        builder.AddLogProvider(
            otelOptions: otelOptions,
            resourceBuilder: resourceBuilder,
            configureLogging: configureLogging);

        return builder;
    }

    /// <summary>Registers a custom <see cref="IOtlpExporterConfigurator"/> implementation for configuring OTLP exporter options.</summary>
    public static IHostApplicationBuilder AddOtlpExporterConfigurator<TConfigurator>(
        this IHostApplicationBuilder builder)
        where TConfigurator : class, IOtlpExporterConfigurator
    {
        builder.Services.AddSingleton<IOtlpExporterConfigurator, TConfigurator>();
        return builder;
    }

    /// <summary>Adds Elastic APM-compatible resource attributes to the <see cref="ResourceBuilder"/>.</summary>
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
        ResourceBuilder resourceBuilder,
        Func<TracerProviderBuilder, TracerProviderBuilder> configureTracing = null)
    {
        var signal = "traces";
        var traceOptions = otelOptions.Traces;

        if (!(traceOptions?.Enabled ?? true))
        {
            return builder;
        }

        // protocol is needed here to properly build the endpoint 
        // since the endpoint format is different for http/protobuf vs grpc
        var protocol = GetOtlpProtocolValue(
            config: otelOptions.OtlpExporter?.Protocol.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_TRACES_PROTOCOL",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL);

        // using config directly here so default otlp endpoint isn't used in the check
        // By defautl, the otlp object uses localhost:4317|4318 which would always make this non empty
        // We want to skip adding the exporter if no endpoint is configured specifically
        var endpoint = GetOtlpEndpointValue(
            protocol: protocol,
            configValue: builder.Configuration["OTel:OtlpExporter:Endpoint"],
            specificEnv: "OTEL_EXPORTER_OTLP_TRACES_ENDPOINT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT,
            signal: signal);

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            return builder;
        }

        // -------------------------------------------------------------------------------
        // Using the ConfigureOpenTelemetryTracerProvider to ensure that the exporter is properly
        // configured before the trace provider is built and used.
        // -------------------------------------------------------------------------------
        // This gives us the opportunity to configure the OTel options after the service provider
        // has built up to ensure that any configuration done after the initial binding is captured here.
        // -------------------------------------------------------------------------------
        builder.Services.ConfigureOpenTelemetryTracerProvider((sp, tracing) =>
        {
            PostConfigureOtlpExporter(
                serviceProvider: sp,
                endpoint: endpoint,
                protocol: protocol,
                telemetryDiscriminator: signal,
                specificHeadersEnvKey: "OTEL_EXPORTER_OTLP_TRACES_HEADERS",
                specificTimeoutEnvKey: "OTEL_EXPORTER_OTLP_TRACES_TIMEOUT");
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .SetResourceBuilder(resourceBuilder)
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

                if (traceOptions?.AddConsoleExporter ?? false)
                {
                    tracing.AddConsoleExporter();
                }

                // Hook to allow the consumer to further configure otel logging
                configureTracing?.Invoke(tracing);

                tracing.AddOtlpExporter(signal, null as Action<OtlpExporterOptions>); // null is important here
            });

        return builder;
    }

    private static IHostApplicationBuilder AddMeterProvider(
        this IHostApplicationBuilder builder,
        OTelOptions otelOptions,
        ResourceBuilder resourceBuilder,
        Func<MeterProviderBuilder, MeterProviderBuilder> configureMetrics = null)
    {
        var signal = "metrics";
        var metricOptions = otelOptions.Metrics;

        if (!(metricOptions?.Enabled ?? true))
        {
            return builder;
        }

        // protocol is needed here to properly build the endpoint 
        // since the endpoint format is different for http/protobuf vs grpc
        var protocol = GetOtlpProtocolValue(
            config: otelOptions.OtlpExporter?.Protocol.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_METRICS_PROTOCOL",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL);

        // using config directly here so default otlp endpoint isn't used in the check
        // By defautl, the otlp object uses localhost:4317|4318 which would always make this non empty
        // We want to skip adding the exporter if no endpoint is configured specifically
        var endpoint = GetOtlpEndpointValue(
            protocol: protocol,
            configValue: builder.Configuration["OTel:OtlpExporter:Endpoint"],
            specificEnv: "OTEL_EXPORTER_OTLP_METRICS_ENDPOINT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT,
            signal: signal);

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            return builder;
        }

        // -------------------------------------------------------------------------------
        // Using the ConfigureOpenTelemetryMeterProvider to ensure that the exporter is properly
        // configured before the meter provider is built and used.
        // -------------------------------------------------------------------------------
        // This gives us the opportunity to configure the OTel options after the service provider
        // has built up to ensure that any configuration done after the initial binding is captured here.
        // -------------------------------------------------------------------------------
        builder.Services.ConfigureOpenTelemetryMeterProvider((sp, meter) =>
        {
            PostConfigureOtlpExporter(
                serviceProvider: sp,
                endpoint: endpoint,
                protocol: protocol,
                telemetryDiscriminator: signal,
                specificHeadersEnvKey: "OTEL_EXPORTER_OTLP_METRICS_HEADERS",
                specificTimeoutEnvKey: "OTEL_EXPORTER_OTLP_METRICS_TIMEOUT");
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(resourceBuilder)
                    .AddRuntimeInstrumentation()
                    .AddAspNetCoreInstrumentation();

                metricOptions?.Meters?.ForEach(x => metrics.AddMeter(x));

                if (metricOptions?.AddConsoleExporter ?? false)
                {
                    metrics.AddConsoleExporter();
                }

                // Hook to allow the consumer to further configure otel logging
                configureMetrics?.Invoke(metrics);

                metrics.AddOtlpExporter(signal, null as Action<OtlpExporterOptions>); // null is important here
            });

        return builder;
    }

    private static IHostApplicationBuilder AddLogProvider(
        this IHostApplicationBuilder builder,
        OTelOptions otelOptions,
        ResourceBuilder resourceBuilder,
        Func<OpenTelemetryLoggerOptions, OpenTelemetryLoggerOptions> configureLogging = null)
    {
        var signal = "logs";
        var logOptions = otelOptions.Logs;

        if (!(logOptions?.Enabled ?? true))
        {
            return builder;
        }

        // protocol is needed here to properly build the endpoint 
        // since the endpoint format is different for http/protobuf vs grpc
        var protocol = GetOtlpProtocolValue(
            config: otelOptions.OtlpExporter?.Protocol.ToString(),
            specificEnv: "OTEL_EXPORTER_OTLP_LOGS_PROTOCOL",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_PROTOCOL);

        // using config directly here so default otlp endpoint isn't used in the check
        // By defautl, the otlp object uses localhost:4317|4318 which would always make this non empty
        // We want to skip adding the exporter if no endpoint is configured specifically
        var endpoint = GetOtlpEndpointValue(
            protocol: protocol,
            configValue: builder.Configuration["OTel:OtlpExporter:Endpoint"],
            specificEnv: "OTEL_EXPORTER_OTLP_LOGS_ENDPOINT",
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_ENDPOINT,
            signal: signal);

        if (string.IsNullOrWhiteSpace(endpoint))
        {
            return builder;
        }

        // -------------------------------------------------------------------------------
        // Using the ConfigureOpenTelemetryLoggerProvider to ensure that the exporter is properly
        // configured before the logger provider is built and used.
        // -------------------------------------------------------------------------------
        // This gives us the opportunity to configure the OTel options after the service provider
        // has built up to ensure that any configuration done after the initial binding is captured here.
        // -------------------------------------------------------------------------------
        builder.Services.ConfigureOpenTelemetryLoggerProvider((sp, logging) =>
        {
            PostConfigureOtlpExporter(
                serviceProvider: sp,
                endpoint: endpoint,
                protocol: protocol,
                telemetryDiscriminator: signal,
                specificHeadersEnvKey: "OTEL_EXPORTER_OTLP_LOGS_HEADERS",
                specificTimeoutEnvKey: "OTEL_EXPORTER_OTLP_LOGS_TIMEOUT");
        });

        builder.Logging.AddOpenTelemetry(logging =>
        {
            logging.SetResourceBuilder(resourceBuilder);
            logging.IncludeFormattedMessage = logOptions?.IncludeFormattedMessage ?? false;
            logging.IncludeScopes = logOptions?.IncludeScopes ?? false;

            if (logOptions?.AddConsoleExporter ?? false)
            {
                logging.AddConsoleExporter();
            }

            // Hook to allow the consumer to further configure otel logging
            configureLogging?.Invoke(logging);

            logging.AddOtlpExporter(signal, null as Action<OtlpExporterOptions>); // null is important here
        });

        return builder;
    }

    private static ResourceBuilder GetResourceBuilder(OTelOptions otelOptions, Func<ResourceBuilder, ResourceBuilder> configureResource = null)
    {
        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceName: Environment.GetEnvironmentVariable("OTEL_SERVICE_NAME") ?? otelOptions.ServiceName ?? (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name ?? "unknown",
            serviceNamespace: Environment.GetEnvironmentVariable("OTEL_SERVICE_NAMESPACE") ?? otelOptions.ServiceNamespace ?? "unknown",
            serviceVersion: (Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Version?.ToString() ?? "unknown",
            serviceInstanceId: Environment.MachineName.ToLowerInvariant());

        var attributes = otelOptions.Resource?.Attributes ?? [];

        if (attributes.Any())
        {
            resourceBuilder.AddAttributes(attributes.Where(kv => !string.IsNullOrWhiteSpace(kv.Key)));
        }

        configureResource?.Invoke(resourceBuilder);

        return resourceBuilder;
    }

    private static string GetOtlpProtocolValue(string config, string specificEnv, string defaultEnv)
    {
        // Removing slashes from http/protobuf in ENV specific value
        return GetOtlpValue(config, specificEnv, defaultEnv)?.Replace("/", string.Empty);
    }

    private static string GetOtlpEndpointValue(string protocol, string configValue, string specificEnv, string defaultEnv, string signal)
    {
        var proto = Enum.TryParse<OtlpExportProtocol>(protocol?.ToString(), true, out var parsedProtocol)
            ? parsedProtocol
            : OtlpExportProtocol.Grpc;

        var endpoint = GetOtlpValue(configValue, specificEnv, defaultEnv);

        if (proto == OtlpExportProtocol.HttpProtobuf && !string.IsNullOrWhiteSpace(endpoint) && !endpoint.EndsWith($"/v1/{signal}", StringComparison.OrdinalIgnoreCase))
        {
            // Appending /v1/logs to the endpoint if using HTTP protocol and not already present
            endpoint = endpoint.EndsWith("/")
                ? $"{endpoint}v1/{signal}"
                : $"{endpoint}/v1/{signal}";
        }

        return endpoint;
    }

    private static string GetOtlpValue(string config, string specificEnv, string defaultEnv) => Environment.GetEnvironmentVariable(specificEnv)
        ?? Environment.GetEnvironmentVariable(defaultEnv)
        ?? config?.ToString();

    private static void PostConfigureOtlpExporter(
        IServiceProvider serviceProvider,
        string endpoint,
        string protocol,
        string telemetryDiscriminator,
        string specificHeadersEnvKey,
        string specificTimeoutEnvKey)
    {
        // purposley reloading OTel options here because during telemetry initialization
        // the options may not be fully bound and configured (post serviceprovider build)
        var otelOptions = serviceProvider.GetRequiredService<OTelOptions>();

        var headers = GetOtlpValue(
            config: otelOptions.OtlpExporter?.Headers,
            specificEnv: specificHeadersEnvKey,
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_HEADERS);

        var timeoutMilliseconds = GetOtlpValue(
            config: otelOptions.OtlpExporter?.TimeoutMilliseconds.ToString(),
            specificEnv: specificTimeoutEnvKey,
            defaultEnv: ENVKEV_OTEL_EXPORTER_OTLP_TIMEOUT);

        var exporterOptions = serviceProvider.GetRequiredService<IOptionsMonitor<OtlpExporterOptions>>().Get(telemetryDiscriminator);
        ApplyConfiguredOtlpExporterOptions(exporterOptions, endpoint, protocol, headers, timeoutMilliseconds, otelOptions.OtlpExporter);

        // Allowing any reigstered IOtlpExporterConfigurator to further customize the exporter options
        // This allows for custom implementations to be injected that cna pull secrets or other configuration
        // Refactor: Might be better in the future to have a way that this only gets called once instead of for each telemetry type.
        foreach (var configurator in serviceProvider.GetServices<IOtlpExporterConfigurator>())
        {
            _ = configurator.Configure(telemetryDiscriminator, exporterOptions);
        }
    }

    private static void ApplyConfiguredOtlpExporterOptions(
        OtlpExporterOptions options,
        string endpoint,
        string protocol,
        string headers,
        string timeoutMilliseconds,
        OtlpExporterOptions configured)
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