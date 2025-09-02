namespace Cezzi.OTel;
/// <summary>
/// Options for open telemetry metrics
/// </summary>
public class OTelMetricsOptions
{
    /// <summary>
    /// The open telemetry exporter options for logging
    /// </summary>
    /// <value></value>
    public OTelExporterOptions OtlpExporter { get; set; }

    /// <summary>
    /// Whether or not open telemetry metrics is enabled
    /// </summary>
    /// <value></value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Additional meters to add to the metrics logging
    /// </summary>
    /// <value></value>
    public List<string> Meters { get; set; } = [];

    /// <summary>
    /// Whether or not to include the console exporter with open telemetry logging
    /// </summary>
    /// <value></value>
    public bool AddConsoleExporter { get; set; } = false;
}