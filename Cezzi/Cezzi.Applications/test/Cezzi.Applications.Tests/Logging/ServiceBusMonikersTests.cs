namespace Cezzi.Applications.Tests.Logging;

using Cezzi.Applications.Logging;
using FluentAssertions;
using System.Linq;
using Xunit;

public class ServiceBusMonikersTests
{
    [Fact]
    public void servicebusmonikers___monikers_match_expected()
    {
        var monikers = new ServiceBusMonikers();

        // These shouldn't chamge... If something broke then change it back since these values
        // shouldn't ever change or it will mess up our log searches
        monikers.MsgId.Should().Be("@sb_msg_id");
        monikers.MsgSubject.Should().Be("@sb_msg_sub");
        monikers.MsgCorrelationId.Should().Be("@sb_msg_cid");
        monikers.MsgDeliveryCount.Should().Be("@sb_msg_dcnt");
        monikers.MsgEntityCount.Should().Be("@sb_msg_entitycount");
        monikers.Topic.Should().Be("@sb_topic");
        monikers.Queue.Should().Be("@sb_queue");
        monikers.Namespace.Should().Be("@sb_namespace");
        monikers.DeadLetterMessageId.Should().Be("@sb_dlmid");
        monikers.FailedSendMessageId.Should().Be("@sb_fsmid");
    }

    [Fact]
    public void servicebusmonikers___start_with_at_sign_and_are_read_only()
    {
        var monikers = new ServiceBusMonikers();
        var properties = monikers.GetType().GetProperties();

        properties.Should().NotBeNullOrEmpty();

        var hasAll = properties.All(x =>
        {
            return x.CanWrite != true && x.GetValue(monikers).ToString().StartsWith("@");
        });

        hasAll.Should().BeTrue();
    }
}
