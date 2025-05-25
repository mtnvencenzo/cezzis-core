namespace Cezzi.OpenApi;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter" />
/// <remarks>Initializes a new instance of the <see cref="SeparatedSwaggerDocumentationFilter"/> class.</remarks>
/// <param name="httpContextAccessor">The HTTP context accessor.</param>
/// <param name="openApiFilterMap">The open API filter map.</param>
/// <param name="serviceProvider">The service provider.</param>
/// <exception cref="System.ArgumentNullException">serviceProvider</exception>
public class SeparatedSwaggerDocumentationFilter(
    IHttpContextAccessor httpContextAccessor,
    OpenApiFilterMap openApiFilterMap,
    IServiceProvider serviceProvider) : OpenApiFilterBase(httpContextAccessor, openApiFilterMap), IDocumentFilter
{
    private readonly IServiceProvider serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <summary>Applies the specified swagger document.</summary>
    /// <param name="swaggerDoc">The swagger document.</param>
    /// <param name="context">The context.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        object value = null;
        this.httpContextAccessor.HttpContext?.Items.TryGetValue(nameof(OpenApiDocumentRequest), out value);

        if (value is not OpenApiDocumentRequest openApiDocumentRequest || string.IsNullOrWhiteSpace(openApiDocumentRequest.ApiFilterKey))
        {
            openApiDocumentRequest = this.serviceProvider.GetRequiredService<OpenApiDocumentRequest>();
        }

        openApiDocumentRequest.ApiFilterKey = OpenApiFilterHelpers.ResolveFilter(this.openApiFilterMap, openApiDocumentRequest.ApiFilterKey);

        // -----------------------------------------------------------------------
        // Update the swagger info section based
        // on the supplied query string filter (a default is used if not provided)
        // -----------------------------------------------------------------------

        var openApiMapping = this.openApiFilterMap.Filters
            .Where(x => x.SwaggerFilter == openApiDocumentRequest.ApiFilterKey)
            .First();

        var markDownDescription = File.ReadAllText(openApiMapping.DescriptionFileName)
            .Replace("{base-url}", openApiMapping.BaseSpecUri);

        swaggerDoc.Info.Title = openApiMapping.OpenApiTitle;

        swaggerDoc.Info.Description = openApiDocumentRequest.Context == "full"
            ? markDownDescription
            : openApiMapping.OpenApiDescription;

        swaggerDoc.Servers.Add(new OpenApiServer
        {
            Url = openApiMapping.BaseApiServerUrl
        });

        //// -----------------------------------------------------------------------
        //// Update the subscription security schmeme
        //// -----------------------------------------------------------------------
        //swaggerDoc.Components.SecuritySchemes.Add("SubscriptionKey", new OpenApiSecurityScheme
        //{
        //    Type = SecuritySchemeType.ApiKey,
        //    In = ParameterLocation.Header,
        //    Name = OpenApiGenerationConstants.SubscriptionKeyHeaderName
        //});

        // -----------------------------------------------------------------------
        // Filter out all endpoints that are not included in the filter
        // Doing this so we can split up the single api into multiple logical apis (from the client perspective)
        // -----------------------------------------------------------------------
        var pathsToRemove = new List<string>();
        var operationsToRemove = new List<string>();

        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var op in path.Value.Operations)
            {
                if (!string.IsNullOrWhiteSpace(openApiDocumentRequest.ApiFilterKey) && !HasTag(op.Value, openApiDocumentRequest.ApiFilterKey))
                {
                    operationsToRemove.Add($"{op.Key}_{path.Key}");
                }
            }
        }

        // Any paths that no longer have operations due to the above filter
        // process should also be removed.
        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var operationId in operationsToRemove)
            {
                OperationType? key = null;

                foreach (var op in path.Value.Operations)
                {
                    // ------------------------------------------------------------------------
                    // Sorting the response contents so that application/json comes first
                    // Doing this because for some reason text/plain comes first by default
                    // And this is basically driving the Redoc UI to not show the response json
                    // unless a user selects it in the drop down list because it wasn't first in the list.
                    // ------------------------------------------------------------------------
                    if (op.Value?.Responses?.Values != null)
                    {
                        foreach (var rs in op.Value.Responses.Values)
                        {
                            rs.Content = rs.Content
                                .OrderBy(x => x.Key)
                                .ToDictionary(x => x.Key, x => x.Value);
                        }
                    }

                    if ($"{op.Key}_{path.Key}" == operationId)
                    {
                        key = op.Key;
                        break;
                    }
                }

                if (key != null)
                {
                    path.Value.Operations.Remove(key.Value);
                }
            }

            if (path.Value.Operations.Count == 0)
            {
                pathsToRemove.Add(path.Key);
            }
        }

        foreach (var path in pathsToRemove)
        {
            swaggerDoc.Paths.Remove(path);
        }

        var schemasToRemove = new List<string>();
        foreach (var schema in swaggerDoc.Components.Schemas)
        {
            if (schema.Value.Extensions.TryGetValue(OpenApiFilterBase.SwaggerFilterVendorExtensionKey, out var schemaValue))
            {
                var schemaFilter = ((OpenApiString)schemaValue).Value;

                if (openApiDocumentRequest.ApiFilterKey != schemaFilter)
                {
                    schemasToRemove.Add(schema.Key);
                }
            }
            else
            {
                schemasToRemove.Add(schema.Key);
            }
        }

        foreach (var schemaKey in schemasToRemove)
        {
            swaggerDoc.Components.Schemas.Remove(schemaKey);
        }
    }

    private static bool HasTag(OpenApiOperation op, string tag) => (op?.Tags) != null && op.Tags.Count != 0 && !string.IsNullOrWhiteSpace(tag) && op.Tags.Any(x => x.Name == tag);
}
