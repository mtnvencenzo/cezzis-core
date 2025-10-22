namespace Cezzi.OTel;

using OpenTelemetry.Exporter;

/// <summary> Interface for configuring OTLP exporter options after service provider buildup</summary>
public interface IOtlpExporterConfigurator
{
    /// <summary>Configures the OTLP exporter options.</summary>
    /// <param name="discriminator"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    bool Configure(string discriminator, OtlpExporterOptions options);
}