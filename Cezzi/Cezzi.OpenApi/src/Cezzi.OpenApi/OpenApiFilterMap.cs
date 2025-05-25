namespace Cezzi.OpenApi;

using Microsoft.Extensions.Configuration;

/// <summary>
/// 
/// </summary>
public class OpenApiFilterMap
{
    /// <summary>The section name</summary>
    public const string SectionName = "OpenApiFiltering";

    /// <summary>Gets or sets the filters.</summary>
    /// <value>The filters.</value>
    public IList<OpenApiFilterMapping> Filters { get; set; } = [];

    /// <summary>Froms the configuration.</summary>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static OpenApiFilterMap FromConfiguration(IConfiguration configuration) => configuration.GetRequiredSection(OpenApiFilterMap.SectionName).Get<OpenApiFilterMap>();
}
