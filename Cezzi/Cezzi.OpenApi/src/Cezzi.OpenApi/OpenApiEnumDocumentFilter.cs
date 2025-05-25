namespace Cezzi.OpenApi;

using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Swashbuckle.AspNetCore.SwaggerGen.IDocumentFilter" />
public class OpenApiEnumDocumentFilter : IDocumentFilter
{
    /// <summary>Applies the specified swagger document.</summary>
    /// <param name="swaggerDoc">The swagger document.</param>
    /// <param name="context">The context.</param>
    /// <exception cref="NotImplementedException"></exception>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var schema in swaggerDoc.Components.Schemas.Where(s => (s.Value.Properties?.Count ?? 0) > 0))
        {
            var type = Type.GetType(schema.Key);

            if (type != null)
            {
                var typeProps = type.GetProperties()
                    .Where(p => p.CanWrite && p.CanRead)
                    .ToList();

                foreach (var openApiProp in schema.Value.Properties)
                {
                    var typeProp = typeProps.FirstOrDefault(p => p.Name.Equals(openApiProp.Key, StringComparison.OrdinalIgnoreCase));

                    if (typeProp != null)
                    {
                        var attribute = typeProp.GetCustomAttributes(attributeType: typeof(OpenApiEnumAttribute), inherit: false)
                            .Cast<OpenApiEnumAttribute>()
                            .FirstOrDefault();

                        if (attribute != null)
                        {
                            var exclude = attribute.Exclude;

                            var enumMemberNames = Enum.GetNames(attribute.EnumType)
                                .Where(e => exclude.Contains(e) == false)
                                .ToList();

                            openApiProp.Value.Description += $"<br/>{Environment.NewLine}";
                            openApiProp.Value.Description += "Enum: ";

                            foreach (var item in enumMemberNames)
                            {
                                openApiProp.Value.Description += $"`\"{item}\"`   ";
                            }

                            openApiProp.Value.Description += $"{Environment.NewLine}";

                            if (string.IsNullOrWhiteSpace(openApiProp.Value.Example?.ToString()))
                            {
                                openApiProp.Value.Example = new Microsoft.OpenApi.Any.OpenApiString(enumMemberNames.FirstOrDefault());
                            }
                        }
                    }
                }
            }
        }
    }
}
