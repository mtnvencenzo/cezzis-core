namespace Cezzi.OpenApi;

/// <summary>
/// 
/// </summary>
public class OpenApiFilterMapping
{
    /// <summary>Gets or sets the swagger filter.</summary>
    /// <value>The swagger filter.</value>
    public string SwaggerFilter { get; set; }

    /// <summary>Gets or sets the model namespace.</summary>
    /// <value>The model namespace.</value>
    public IList<string> ModelNamespaces { get; set; } = [];

    /// <summary>Gets or sets a value indicating whether this instance is default.</summary>
    /// <value><c>true</c> if this instance is default; otherwise, <c>false</c>.</value>
    public bool IsDefault { get; set; }

    /// <summary>Gets or sets the name of the description file.</summary>
    /// <value>The name of the description file.</value>
    public string DescriptionFileName { get; set; }

    /// <summary>Gets or sets the base spec URI.</summary>
    /// <value>The base spec URI.</value>
    public string BaseSpecUri { get; set; }

    /// <summary>Gets or sets the base API server URL.</summary>
    /// <value>The base API server URL.</value>
    public string BaseApiServerUrl { get; set; }

    /// <summary>Gets or sets the open API title.</summary>
    /// <value>The open API title.</value>
    public string OpenApiTitle { get; set; }

    /// <summary>Gets or sets the open API description.</summary>
    /// <value>The open API description.</value>
    public string OpenApiDescription { get; set; }
}
