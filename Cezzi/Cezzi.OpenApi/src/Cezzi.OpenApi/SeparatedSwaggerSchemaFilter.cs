namespace Cezzi.OpenApi;

using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.ISchemaFilter" />
/// <remarks>Initializes a new instance of the <see cref="SeparatedSwaggerSchemaFilter"/> class.</remarks>
/// <param name="httpContextAccessor">The HTTP context accessor.</param>
/// <param name="openApiFilterMap">The open API filter map.</param>
public class SeparatedSwaggerSchemaFilter(
    IHttpContextAccessor httpContextAccessor,
    OpenApiFilterMap openApiFilterMap) : OpenApiFilterBase(httpContextAccessor, openApiFilterMap), ISchemaFilter
{

    /// <summary>Applies the specified schema.</summary>
    /// <param name="schema">The schema.</param>
    /// <param name="context">The context.</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        foreach (var filterMapping in this.openApiFilterMap.Filters)
        {
            foreach (var ns in filterMapping.ModelNamespaces ?? [])
            {
                if (context.Type.Namespace.StartsWith(ns))
                {
                    schema.Extensions.Add(OpenApiFilterBase.SwaggerFilterVendorExtensionKey, new OpenApiString(filterMapping.SwaggerFilter));
                    break;
                }
            }
        }
    }
}
