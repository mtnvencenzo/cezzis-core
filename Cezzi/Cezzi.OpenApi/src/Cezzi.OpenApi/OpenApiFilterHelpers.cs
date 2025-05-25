namespace Cezzi.OpenApi;

/// <summary>
/// 
/// </summary>
public static class OpenApiFilterHelpers
{
    /// <summary>Resolves the filter.</summary>
    /// <param name="openApiFilterMap">The open API filter map.</param>
    /// <param name="filter">The filter.</param>
    /// <returns></returns>
    public static string ResolveFilter(this OpenApiFilterMap openApiFilterMap, string filter)
    {
        var resolvedFilter = filter;

        if (openApiFilterMap == null)
        {
            return resolvedFilter;
        }

        if (!string.IsNullOrWhiteSpace(filter))
        {
            if (!openApiFilterMap.Filters.Any(x => x.SwaggerFilter.Equals(filter, StringComparison.OrdinalIgnoreCase)))
            {
                resolvedFilter = null;
            }
        }

        if (string.IsNullOrWhiteSpace(resolvedFilter))
        {
            resolvedFilter = openApiFilterMap.Filters
                .Where(x => x.IsDefault)
                .FirstOrDefault()?.SwaggerFilter;
        }

        if (string.IsNullOrWhiteSpace(resolvedFilter))
        {
            resolvedFilter = openApiFilterMap.Filters.FirstOrDefault()?.SwaggerFilter;
        }

        return resolvedFilter;
    }
}
