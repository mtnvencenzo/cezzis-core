namespace Cezzi.OTel;

/// <summary>
/// Options for OpenTelemetry metrics.
/// /// </summary>
public sealed class OTelMetricsOptions
{
    /// <summary>Whether metrics are enabled</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Additional meters to include</summary>
    public List<string> Meters { get; set; } = [];

    /// <summary>Whether to include the console exporter with OpenTelemetry tracing</summary>
    public bool AddConsoleExporter { get; set; } = false;
}