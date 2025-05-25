namespace Cezzi.OpenApi;

using Microsoft.AspNetCore.Http;
using System;

/// <summary>
/// 
/// </summary>
/// <remarks>Initializes a new instance of the <see cref="OpenApiFilterBase"/> class.</remarks>
/// <param name="httpContextAccessor">The HTTP context accessor.</param>
/// <param name="openApiFilterMap">The open API filter map.</param>
/// <exception cref="ArgumentNullException">
/// httpContextAccessor
/// or
/// openApiFilterMap
/// </exception>
public abstract class OpenApiFilterBase(
    IHttpContextAccessor httpContextAccessor,
    OpenApiFilterMap openApiFilterMap)
{
    /// <summary>The swagger filter vendor extension key</summary>
    public const string SwaggerFilterVendorExtensionKey = "x-filter-key";

    /// <summary>The HTTP context accessor</summary>
    protected readonly IHttpContextAccessor httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    /// <summary>The open API filter map</summary>
    protected readonly OpenApiFilterMap openApiFilterMap = openApiFilterMap ?? throw new ArgumentNullException(nameof(openApiFilterMap));
}
