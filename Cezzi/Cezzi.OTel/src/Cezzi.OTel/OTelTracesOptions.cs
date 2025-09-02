namespace Cezzi.OTel;

/// <summary>
/// Options for open telemetry traces
/// </summary>
public sealed class OTelTracesOptions
{
    /// <summary>
    /// The OpenTelemetry exporter options for tracing
    /// </summary>
    /// <value></value>
    public OTelExporterOptions OtlpExporter { get; set; } = new();

    /// <summary>
    /// Whether or not open telemetry tracing is enabled
    /// </summary>
    /// <value></value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Excluded route paths for the aspnet open telemetry traces
    /// </summary>
    /// <value></value>
    public List<string> ExcludePaths { get; set; } = [];

    /// <summary>
    /// Additional sources to add to the opentelemetry tracing
    /// </summary>
    /// <value></value>
    public List<string> Sources { get; set; } = [];

    /// <summary>
    /// Whether to include the console exporter with OpenTelemetry tracing
    /// </summary>
    /// <value></value>
    public bool AddConsoleExporter { get; set; } = false;
}