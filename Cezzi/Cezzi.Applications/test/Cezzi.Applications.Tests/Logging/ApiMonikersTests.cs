namespace Cezzi.Applications.Tests.Logging;

using Cezzi.Applications.Logging;
using FluentAssertions;
using System.Linq;
using Xunit;

public class ApiMonikersTests
{
    [Fact]
    public void apimonikers___monikers_match_expected()
    {
        var monikers = new ApiMonikers();

        // These shouldn't change... If something broke then change it back since these values
        // shouldn't ever change or it will mess up our log searches
        monikers.Endpoint.Should().Be("@api_endpoint");
        monikers.RequestId.Should().Be("@api_request_id");
        monikers.ValidationResult.Should().Be("@api_val_result");
        monikers.CorrelationId.Should().Be("@api_correlationid");
        monikers.RouteTemplate.Should().Be("@api_route_template");
    }

    [Fact]
    public void apimonikers___start_with_at_sign_and_are_read_only()
    {
        var monikers = new ApiMonikers();
        var properties = monikers.GetType().GetProperties();

        properties.Should().NotBeNullOrEmpty();

        var hasAll = properties.All(x =>
        {
            return x.CanWrite != true && x.GetValue(monikers).ToString().StartsWith("@");
        });

        hasAll.Should().BeTrue();
    }
}
