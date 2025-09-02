namespace Cezzi.OTel;
/// <summary>
/// Options for OpenTelemetry metrics.
/// </summary>
public sealed class OTelMetricsOptions
{
    /// <summary>
    /// The OpenTelemetry exporter options for metrics
    /// </summary>
    /// <value></value>
    public OTelExporterOptions OtlpExporter { get; set; } = new();

    /// <summary>
    /// Whether metrics are enabled
    /// </summary>
    /// <value></value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Additional meters to include
    /// </summary>
    /// <value></value>
    public List<string> Meters { get; set; } = [];

    /// <summary>
    /// Whether to include the console exporter for metrics
    /// </summary>
    /// <value></value>
    public bool AddConsoleExporter { get; set; } = false;
}