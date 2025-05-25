namespace Cezzi.Applications.Tests.Logging;

using Cezzi.Applications.Logging;
using FluentAssertions;
using System.Linq;
using Xunit;

public class AzMonikersTests
{
    [Fact]
    public void azmonikers___monikers_match_expected()
    {
        var monikers = new AzMonikers();

        // These shouldn't chamge... If something broke then change it back since these values
        // shouldn't ever change or it will mess up our log searches
        monikers.AzFunction.Should().Be("@az_func");
        monikers.AzFunctionInvocationId.Should().Be("@az_func_invocationid");
        monikers.AzFunctionTimerIsPastDue.Should().Be("@az_func_timer_ispastdue");
        monikers.AzFunctionTimerLastRan.Should().Be("@az_func_timer_lastran");
        monikers.AzFunctionTimerNextRun.Should().Be("@az_func_timer_nextrun");
        monikers.AzBlobName.Should().Be("@az_blob_name");
        monikers.AzBlobType.Should().Be("@az_blob_type");
        monikers.AzBlobBytesCount.Should().Be("@az_blob_byte_cnt");
        monikers.AzBlobCreatedOn.Should().Be("@az_blob_created");
        monikers.AzBlobDeleted.Should().Be("@az_blob_deleted");
        monikers.AzBlobIsLatest.Should().Be("@az_blob_islatest");
        monikers.AzBlobVersionId.Should().Be("@az_blob_versionid");
    }

    [Fact]
    public void azmonikers___start_with_at_sign_and_are_read_only()
    {
        var monikers = new AzMonikers();
        var properties = monikers.GetType().GetProperties();

        properties.Should().NotBeNullOrEmpty();

        var hasAll = properties.All(x =>
        {
            return x.CanWrite != true && x.GetValue(monikers).ToString().StartsWith("@");
        });

        hasAll.Should().BeTrue();
    }
}
