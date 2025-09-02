namespace Cezzi.OTel;

/// <summary>
/// Options for OpenTelemetry
/// </summary>
public class OTelOptions
{
    /// <summary>
    /// The app settings section name used for OpenTelemetry configurations
    /// </summary>
    public const string SectionName = "OTel";

    /// <summary>
    /// The name of the service
    /// </summary>
    /// <value></value>
    public string ServiceName { get; set; }

    /// <summary>
    /// Whether OpenTelemetry is enabled
    /// </summary>
    /// <value></value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// The namespace that the service belongs to
    /// </summary>
    /// <value></value>
    public string ServiceNamespace { get; set; }

    /// <summary>
    /// Options for OpenTelemetry tracing
    /// </summary>
    /// <value></value>
    public OTelTracesOptions Traces { get; set; } = new();

    /// <summary>
    /// Options for OpenTelemetry logging
    /// </summary>
    /// <value></value>
    public OTelLogsOptions Logs { get; set; } = new();

    /// <summary>
    /// Options for OpenTelemetry metrics
    /// </summary>
    /// <value></value>
    public OTelMetricsOptions Metrics { get; set; } = new();
}