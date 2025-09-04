namespace Cezzi.OTel;

/// <summary>
/// Options for OpenTelemetry logging
/// </summary>
public sealed class OTelLogsOptions
{
    /// <summary>Whether logging is enabled</summary>
    public bool Enabled { get; set; } = true;

    /// <summary>Whether to include formatted messages with logging</summary>
    public bool IncludeFormattedMessage { get; set; } = false;

    /// <summary>Whether to include scopes with the logs</summary>
    public bool IncludeScopes { get; set; } = false;

    /// <summary>Whether to include the console exporter with OpenTelemetry tracing</summary>
    public bool AddConsoleExporter { get; set; } = false;
}