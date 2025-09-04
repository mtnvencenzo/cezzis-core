namespace Cezzi.OTel;

/// <summary>Options for open telemetry traces</summary>
public sealed class OTelTracesOptions
{
    /// <summary>Whether or not open telemetry tracing is enabled</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Excluded route paths for the aspnet open telemetry traces</summary>
    public List<string> ExcludePaths { get; set; } = [];

    /// <summary>Additional sources to add to the opentelemetry tracing</summary>
    public List<string> Sources { get; set; } = [];

    /// <summary>Whether to include the console exporter with OpenTelemetry tracing</summary>
    public bool AddConsoleExporter { get; set; } = false;
}