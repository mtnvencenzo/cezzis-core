namespace Cezzi.Applications.Logging;

/// <summary>
/// 
/// </summary>
public class ApiMonikers
{
    /// <summary>The endpoint</summary>
    public string Endpoint => "@api_endpoint";

    /// <summary>Gets the request identifier.</summary>
    /// <value>The request identifier.</value>
    public string RequestId => "@api_request_id";

    /// <summary>Gets the validation result.</summary>
    /// <value>The validation result.</value>
    public string ValidationResult => "@api_val_result";

    /// <summary>Gets the correlation identifier.</summary>
    /// <value>The correlation identifier.</value>
    public string CorrelationId => "@api_correlationid";

    /// <summary>Gets the route template.</summary>
    /// <value>The route template.</value>
    public string RouteTemplate => "@api_route_template";
}