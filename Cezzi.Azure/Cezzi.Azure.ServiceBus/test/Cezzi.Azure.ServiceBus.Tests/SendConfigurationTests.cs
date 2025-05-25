namespace Cezzi.Azure.ServiceBus.Tests;

using FluentAssertions;
using Xunit;

public class SendConfigurationTests
{
    [Fact]
    public void sendconfig___reuses_all_existing_props()
    {
        var sendConfig = new SendConfiguration
        {
            Label = "mylabel",
            QueueOrTopicName = "mytopic",
            SendConnectionString = "myconn",
            SendRetry = new SendRetryOptions
            {
                MaxRetries = 1,
                MaxRetryDelaySeconds = 2,
                OperationTimeoutInSeconds = 3,
                RetryDelaySeconds = 4
            }
        };

        var cloned = sendConfig.Clone();
        cloned.Should().NotBeSameAs(sendConfig);
        cloned.Label.Should().Be(sendConfig.Label);
        cloned.SendConnectionString.Should().Be(sendConfig.SendConnectionString);
        cloned.QueueOrTopicName.Should().Be(sendConfig.QueueOrTopicName);
        cloned.SendRetry.Should().NotBeNull();
        cloned.SendRetry.Should().NotBeSameAs(sendConfig.SendRetry);
        cloned.SendRetry.MaxRetryDelaySeconds.Should().Be(sendConfig.SendRetry.MaxRetryDelaySeconds);
        cloned.SendRetry.MaxRetries.Should().Be(sendConfig.SendRetry.MaxRetries);
        cloned.SendRetry.OperationTimeoutInSeconds.Should().Be(sendConfig.SendRetry.OperationTimeoutInSeconds);
        cloned.SendRetry.RetryDelaySeconds.Should().Be(sendConfig.SendRetry.RetryDelaySeconds);
    }

    [Fact]
    public void sendconfig___updates_all_existing_props()
    {
        var sendConfig = new SendConfiguration
        {
            Label = "mylabel",
            QueueOrTopicName = "mytopic",
            SendConnectionString = "myconn",
            SendRetry = new SendRetryOptions
            {
                MaxRetries = 1,
                MaxRetryDelaySeconds = 2,
                OperationTimeoutInSeconds = 3,
                RetryDelaySeconds = 4
            }
        };

        var cloned = sendConfig.Clone(
            label: "mylabel2",
            queueOrTopicName: "mytopic2",
            sendConnectionString: "myconn2",
            sendRetry: new SendRetryOptions
            {
                MaxRetries = 11,
                MaxRetryDelaySeconds = 22,
                OperationTimeoutInSeconds = 33,
                RetryDelaySeconds = 44
            });

        cloned.Should().NotBeSameAs(sendConfig);
        cloned.Label.Should().Be("mylabel2");
        cloned.QueueOrTopicName.Should().Be("mytopic2");
        cloned.SendConnectionString.Should().Be("myconn2");
        cloned.SendRetry.Should().NotBeNull();
        cloned.SendRetry.Should().NotBeSameAs(sendConfig.SendRetry);
        cloned.SendRetry.MaxRetries.Should().Be(11);
        cloned.SendRetry.MaxRetryDelaySeconds.Should().Be(22);
        cloned.SendRetry.OperationTimeoutInSeconds.Should().Be(33);
        cloned.SendRetry.RetryDelaySeconds.Should().Be(44);
    }
}
