namespace Cezzi.OpenApi;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerGen;

/// <summary>
/// The service bootstrap.
/// </summary>
public static class ServiceBootstrap
{
    /// <summary>Uses the cezzi open API.</summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns></returns>
    public static IServiceCollection UseCezziOpenApi(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<OpenApiFilterMap>((sp) => OpenApiFilterMap.FromConfiguration(configuration));
        services.AddScoped<OpenApiDocumentRequest>();

        return services;
    }

    /// <summary>Uses the cezzi open API filters.</summary>
    /// <param name="swaggerGenOptions">The swagger gen options.</param>
    public static void UseCezziOpenApiFilters(this SwaggerGenOptions swaggerGenOptions)
    {
        swaggerGenOptions.DocumentFilter<SeparatedSwaggerDocumentationFilter>(); // For updating the api descriptions based on query param
        swaggerGenOptions.DocumentFilter<OpenApiCamelCaseQueryParameterDocumentationFilter>(); // for making query parameters camel cased
        swaggerGenOptions.DocumentFilter<OpenApiEnumDocumentFilter>(); // For adding enumeration members to string properties
        swaggerGenOptions.SchemaFilter<SeparatedSwaggerSchemaFilter>();
    }
}
