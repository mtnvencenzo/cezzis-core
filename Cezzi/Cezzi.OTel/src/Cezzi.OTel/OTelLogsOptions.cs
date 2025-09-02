namespace Cezzi.OTel;

/// <summary>
/// Options for OpenTelemetry logging
/// </summary>
public sealed class OTelLogsOptions
{
    /// <summary>
    /// OTLP exporter options for logging
    /// </summary>
    /// <value></value>
    public OTelExporterOptions OtlpExporter { get; set; } = new();

    /// <summary>
    /// Whether logging is enabled
    /// </summary>
    /// <value></value>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Whether to include formatted messages with logging
    /// </summary>
    /// <value></value>
    public bool IncludeFormattedMessage { get; set; } = false;

    /// <summary>
    /// Whether to include scopes with the logs
    /// </summary>
    /// <value></value>
    public bool IncludeScopes { get; set; } = false;

    /// <summary>
    /// Whether to include the console exporter with logging
    /// </summary>
    /// <value></value>
    public bool AddConsoleExporter { get; set; } = false;
}