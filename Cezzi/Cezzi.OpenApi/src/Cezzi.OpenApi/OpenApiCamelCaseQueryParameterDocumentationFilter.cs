namespace Cezzi.OpenApi;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter" />
public class OpenApiCamelCaseQueryParameterDocumentationFilter : IDocumentFilter
{
    /// <summary>Applies the specified swagger document.</summary>
    /// <param name="swaggerDoc">The swagger document.</param>
    /// <param name="context">The context.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths)
        {
            foreach (var op in path.Value.Operations)
            {
                foreach (var opParam in op.Value.Parameters)
                {
                    if (opParam.In == ParameterLocation.Query)
                    {
                        if (opParam.Name.Length > 1)
                        {
                            opParam.Name = opParam.Name[0].ToString().ToLower() + opParam.Name[1..];
                        }
                    }
                }
            }
        }
    }
}
