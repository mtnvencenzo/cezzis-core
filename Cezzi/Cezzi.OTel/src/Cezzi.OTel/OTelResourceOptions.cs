namespace Cezzi.OTel;

/// <summary>
/// Options for configuring OpenTelemetry resource attributes.
/// </summary>
public class OTelResourceOptions
{
    /// <summary>Dictionary of resource attributes to be added to the OpenTelemetry resource.</summary>
    public Dictionary<string, object> Attributes { get; set; } = [];
}