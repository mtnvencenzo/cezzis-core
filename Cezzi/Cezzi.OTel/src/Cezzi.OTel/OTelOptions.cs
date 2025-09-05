namespace Cezzi.OTel;

using OpenTelemetry.Exporter;

/// <summary>Options for OpenTelemetry</summary>
public class OTelOptions
{
    /// <summary>The app settings section name used for OpenTelemetry configurations</summary>
    public const string SectionName = "OTel";

    /// <summary>The OpenTelemetry exporter options for metrics</summary>
    public OtlpExporterOptions OtlpExporter { get; set; } = new();

    /// <summary>The name of the service</summary>
    public string ServiceName { get; set; }

    /// <summary>The namespace that the service belongs to</summary>
    public string ServiceNamespace { get; set; }

    /// <summary>Options for OpenTelemetry tracing</summary>
    public OTelTracesOptions Traces { get; set; } = new();

    /// <summary>Options for OpenTelemetry logging</summary>
    public OTelLogsOptions Logs { get; set; } = new();

    /// <summary>Options for OpenTelemetry metrics</summary>
    public OTelMetricsOptions Metrics { get; set; } = new();
}