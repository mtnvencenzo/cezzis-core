namespace Cezzi.Azure.ServiceBus.Tests;

using FluentAssertions;
using global::Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class ServiceBusMessageSenderTests : ServiceTestBase
{
    // Send
    //

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___send_throws_on_invalid_correlationid(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await sender.Send(
            message: new TestMessage(),
            configuration: null,
            correlationId: value,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("correlationId");

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_throws_on_null_message()
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await sender.Send(
            message: null as TestMessage,
            configuration: null,
            correlationId: Guid.NewGuid().ToString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("message");

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_throws_on_null_configuration()
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await sender.Send(
            message: new TestMessage(),
            configuration: null,
            correlationId: Guid.NewGuid().ToString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("configuration");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___send_throws_on_invalid_sendconnectionstring(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = value
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"SendConnectionString is not defined");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___send_throws_on_invalid_label(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = value
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"Label is not defined");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___send_throws_on_invalid_queueortopic(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = value
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"QueueOrTopicName is not defined");

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_throws_on_send_failure_and_no_exception_handler()
    {
        var message = new TestMessage();

        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()))
            .Returns((ServiceBusSender s, ServiceBusMessage m, CancellationToken c) =>
            {
                throw new Exception("Failed");
            });

        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.Send(
            message: message,
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "override",
                QueueOrTopicName = "queue"
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"Failed");

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()), Times.Exactly(1));

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_does_not_throw_on_send_failure_with_exception_handler()
    {
        var message = new TestMessage();
        var correlationId = Guid.NewGuid().ToString();
        var sendConfiguration = new SendConfiguration
        {
            SendConnectionString = "test",
            Label = "override",
            QueueOrTopicName = "queue"
        };

        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()))
            .Returns((ServiceBusSender s, ServiceBusMessage m, CancellationToken c) =>
            {
                throw new Exception("Failed");
            });

        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        string handledMessage = null;
        ISendConfiguration handledConfiguration = null;
        string handledMessageId = null;
        string handledCorrelationId = null;
        string handledContentType = null;
        string handledSessionId = null;
        TimeSpan? handledEnqueueDelay = null;
        Exception handledEx = null;

        Task exceptionHandler(
            string message,
            ISendConfiguration configuration,
            string messageId,
            string correlationId,
            string contentType,
            string sessionId,
            TimeSpan? enqueueDelay,
            Exception ex)
        {
            handledMessage = message;
            handledConfiguration = configuration;
            handledMessageId = messageId;
            handledCorrelationId = correlationId;
            handledContentType = contentType;
            handledSessionId = sessionId;
            handledEnqueueDelay = enqueueDelay;
            handledEx = ex;
            return Task.CompletedTask;
        }

        await sender.Send(
            message: message,
            configuration: sendConfiguration,
            correlationId: correlationId,
            contentType: "text/json",
            sessionId: "my-session",
            enqueueDelay: null,
            onSendFailure: exceptionHandler,
            cancellationToken: default).ConfigureAwait(false);

        handledMessage.Should().NotBeNull();
        handledMessage.Should().Be(ServiceBusMessageSerializer.SerializeToUtf8String(message));
        handledConfiguration.Should().BeEquivalentTo(sendConfiguration);
        handledMessageId.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(handledMessageId, out var _).Should().BeTrue();
        handledCorrelationId.Should().Be(correlationId);
        handledContentType.Should().Be("text/json");
        handledSessionId.Should().Be("my-session");
        handledEnqueueDelay.Should().BeNull();
        handledEx.Should().NotBeNull();
        handledEx.Message.Should().Be($"Failed");

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()), Times.Exactly(1));

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_does_not_throw_on_multiple_send_failures_with_exception_handler()
    {
        var messages = new List<TestMessage>
        {
            new(),
            new()
        };

        var correlationId = Guid.NewGuid().ToString();
        var sendConfiguration = new SendConfiguration
        {
            SendConnectionString = "test",
            Label = "override",
            QueueOrTopicName = "queue"
        };

        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()))
            .Returns((ServiceBusSender s, ServiceBusMessage m, CancellationToken c) =>
            {
                throw new Exception("Failed");
            });

        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        string handledMessage = null;
        ISendConfiguration handledConfiguration = null;
        string handledMessageId = null;
        string handledCorrelationId = null;
        string handledContentType = null;
        string handledSessionId = null;
        TimeSpan? handledEnqueueDelay = null;
        Exception handledEx = null;

        var handledCount = 0;

        Task exceptionHandler(
            string message,
            ISendConfiguration configuration,
            string messageId,
            string correlationId,
            string contentType,
            string sessionId,
            TimeSpan? enqueueDelay,
            Exception ex)
        {
            handledCount++;
            handledMessage = message;
            handledConfiguration = configuration;
            handledMessageId = messageId;
            handledCorrelationId = correlationId;
            handledContentType = contentType;
            handledSessionId = sessionId;
            handledEnqueueDelay = enqueueDelay;
            handledEx = ex;
            return Task.CompletedTask;
        }

        await sender.SendMessages(
            messages: messages,
            configuration: sendConfiguration,
            correlationId: correlationId,
            contentType: "text/json",
            sessionId: "my-session",
            enqueueDelay: null,
            onSendFailure: exceptionHandler,
            cancellationToken: default).ConfigureAwait(false);

        handledCount.Should().Be(messages.Count);

        handledMessage.Should().NotBeNull();
        handledMessage.Should().Be(ServiceBusMessageSerializer.SerializeToUtf8String(messages[1]));
        handledConfiguration.Should().BeEquivalentTo(sendConfiguration);
        handledMessageId.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(handledMessageId, out var _).Should().BeTrue();
        handledCorrelationId.Should().Be(correlationId);
        handledContentType.Should().Be("text/json");
        handledSessionId.Should().Be("my-session");
        handledEnqueueDelay.Should().BeNull();
        handledEx.Should().NotBeNull();
        handledEx.Message.Should().Be($"Failed");

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()), Times.Exactly(2));

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_success_no_retry_config_values()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue",
            },
            correlationId: correlationId,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == null &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_success_with_retry_config_values()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue",
                SendRetry = new SendRetryOptions
                {
                    MaxRetries = 1,
                    MaxRetryDelaySeconds = 1,
                    OperationTimeoutInSeconds = 1,
                    RetryDelaySeconds = 1
                }
            },
            correlationId: correlationId,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == null &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_success_with_messageid()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == null &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_success_with_sessionid_and_contenttype()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_success_with_enqueuedelay()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.Send(
            message: new TestMessage(),
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___send_success_with_multiple_messages()
    {
        this.SetupEnvironment();

        var messages = new int[1000]
            .ToList()
            .Select(x => new TestMessage())
            .ToList();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var sendCount = await sender.SendMessages(
            messages: messages,
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Exactly(1000));

        sendCount.Should().Be(1000);
        this.Verify_NoOtherCalls();
    }

    // SendJson
    //

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___sendjson_throws_on_invalid_correlationid(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await sender.SendJson(
            json: "{}",
            configuration: null,
            correlationId: value,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("correlationId");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___sendjson_throws_on_null_json(string json)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await sender.SendJson(
            json: json,
            configuration: null,
            correlationId: Guid.NewGuid().ToString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("json");

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_throws_on_null_configuration()
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await sender.SendJson(
            json: "{}",
            configuration: null,
            correlationId: Guid.NewGuid().ToString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("configuration");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___sendjson_throws_on_invalid_sendconnectionstring(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = value
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"SendConnectionString is not defined");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___sendjson_throws_on_invalid_label(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = value
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"Label is not defined");

        this.Verify_NoOtherCalls();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task servicebussender___sendjson_throws_on_invalid_queueortopic(string value)
    {
        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = value
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"QueueOrTopicName is not defined");

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_throws_on_send_failure_and_no_exception_handler()
    {
        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()))
            .Returns((ServiceBusSender s, ServiceBusMessage m, CancellationToken c) =>
            {
                throw new Exception("Failed");
            });

        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var ex = await Assert.ThrowsAsync<Exception>(async () => await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "override",
                QueueOrTopicName = "queue"
            },
            correlationId: GuidString(),
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"Failed");

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()), Times.Exactly(1));

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_does_not_throw_on_send_failure_with_exception_handler()
    {
        var correlationId = Guid.NewGuid().ToString();
        var sendConfiguration = new SendConfiguration
        {
            SendConnectionString = "test",
            Label = "override",
            QueueOrTopicName = "queue"
        };

        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()))
            .Returns((ServiceBusSender s, ServiceBusMessage m, CancellationToken c) =>
            {
                throw new Exception("Failed");
            });

        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        string handledMessage = null;
        ISendConfiguration handledConfiguration = null;
        string handledMessageId = null;
        string handledCorrelationId = null;
        string handledContentType = null;
        string handledSessionId = null;
        TimeSpan? handledEnqueueDelay = null;
        Exception handledEx = null;

        Task exceptionHandler(
            string message,
            ISendConfiguration configuration,
            string messageId,
            string correlationId,
            string contentType,
            string sessionId,
            TimeSpan? enqueueDelay,
            Exception ex)
        {
            handledMessage = message;
            handledConfiguration = configuration;
            handledMessageId = messageId;
            handledCorrelationId = correlationId;
            handledContentType = contentType;
            handledSessionId = sessionId;
            handledEnqueueDelay = enqueueDelay;
            handledEx = ex;
            return Task.CompletedTask;
        }

        await sender.SendJson(
            json: "{}",
            configuration: sendConfiguration,
            correlationId: correlationId,
            contentType: "text/json",
            sessionId: "my-session",
            enqueueDelay: null,
            onSendFailure: exceptionHandler,
            cancellationToken: default).ConfigureAwait(false);

        handledMessage.Should().NotBeNull();
        handledMessage.Should().Be("{}");
        handledConfiguration.Should().BeEquivalentTo(sendConfiguration);
        handledMessageId.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(handledMessageId, out var _).Should().BeTrue();
        handledCorrelationId.Should().Be(correlationId);
        handledContentType.Should().Be("text/json");
        handledSessionId.Should().Be("my-session");
        handledEnqueueDelay.Should().BeNull();
        handledEx.Should().NotBeNull();
        handledEx.Message.Should().Be($"Failed");

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()), Times.Exactly(1));

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_does_not_throw_on_multiple_send_failures_with_exception_handler()
    {
        var messages = new List<string>
        {
            ServiceBusMessageSerializer.SerializeToUtf8String(new TestMessage{ MyProp = "test" }),
            ServiceBusMessageSerializer.SerializeToUtf8String(new TestMessage{ MyProp = "test2" }),
        };

        var correlationId = Guid.NewGuid().ToString();
        var sendConfiguration = new SendConfiguration
        {
            SendConnectionString = "test",
            Label = "override",
            QueueOrTopicName = "queue"
        };

        this.serviceBusSenderProxyMock
            .Setup(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()))
            .Returns((ServiceBusSender s, ServiceBusMessage m, CancellationToken c) =>
            {
                throw new Exception("Failed");
            });

        this.SetupEnvironment();

        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        string handledMessage = null;
        ISendConfiguration handledConfiguration = null;
        string handledMessageId = null;
        string handledCorrelationId = null;
        string handledContentType = null;
        string handledSessionId = null;
        TimeSpan? handledEnqueueDelay = null;
        Exception handledEx = null;

        var handledCount = 0;

        Task exceptionHandler(
            string message,
            ISendConfiguration configuration,
            string messageId,
            string correlationId,
            string contentType,
            string sessionId,
            TimeSpan? enqueueDelay,
            Exception ex)
        {
            handledCount++;
            handledMessage = message;
            handledConfiguration = configuration;
            handledMessageId = messageId;
            handledCorrelationId = correlationId;
            handledContentType = contentType;
            handledSessionId = sessionId;
            handledEnqueueDelay = enqueueDelay;
            handledEx = ex;
            return Task.CompletedTask;
        }

        await sender.SendJsonMessages(
            jsons: messages,
            configuration: sendConfiguration,
            correlationId: correlationId,
            contentType: "text/json",
            sessionId: "my-session",
            enqueueDelay: null,
            onSendFailure: exceptionHandler,
            cancellationToken: default).ConfigureAwait(false);

        handledCount.Should().Be(messages.Count);

        handledMessage.Should().NotBeNull();
        handledMessage.Should().Be(messages[1]);
        handledConfiguration.Should().BeEquivalentTo(sendConfiguration);
        handledMessageId.Should().NotBeNullOrWhiteSpace();
        Guid.TryParse(handledMessageId, out var _).Should().BeTrue();
        handledCorrelationId.Should().Be(correlationId);
        handledContentType.Should().Be("text/json");
        handledSessionId.Should().Be("my-session");
        handledEnqueueDelay.Should().BeNull();
        handledEx.Should().NotBeNull();
        handledEx.Message.Should().Be($"Failed");

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>(x => x.Subject == "override"),
                It.IsAny<CancellationToken>()), Times.Exactly(2));

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_success_no_retry_config_values()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue",
            },
            correlationId: correlationId,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == null &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_success_with_retry_config_values()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue",
                SendRetry = new SendRetryOptions
                {
                    MaxRetries = 1,
                    MaxRetryDelaySeconds = 1,
                    OperationTimeoutInSeconds = 1,
                    RetryDelaySeconds = 1
                }
            },
            correlationId: correlationId,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == null &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_success_with_messageid()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: null,
            sessionId: null,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == null &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_success_with_sessionid_and_contenttype()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: "test/xml",
            sessionId: sessionId,
            enqueueDelay: null,
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.SendMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "test/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_success_with_enqueuedelay()
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_success_with_multiple_messages()
    {
        this.SetupEnvironment();

        var messages = new int[1000]
            .ToList()
            .Select(x => $"{{ \"myProp\": {x} }}")
            .ToList();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        var sendCount = await sender.SendJsonMessages(
            jsons: messages,
            configuration: new SendConfiguration
            {
                SendConnectionString = "test",
                Label = "label",
                QueueOrTopicName = "queue"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Exactly(1000));

        sendCount.Should().Be(1000);
        this.Verify_NoOtherCalls();
    }

    // Different Connection Strings

    [Theory]
    [InlineData("Endpoint=sb-cezzis.com/;SharedAccessKeyName=TestSendPolicy;SharedAccessKey=4235os1ddeslfdffsdd=;")]
    [InlineData("Endpoint=sb-cezzis.com/;SharedAccessKeyName=TestSendPolicy;SharedAccessKey=4235os1ddeslfdffsdd=")]
    [InlineData("Endpoint=sb-cezzis.com/;SharedAccessKeyName=TestSendPolicy;SharedAccessKey=4235os1ddeslfdffsdd=;EntityPath=sbt-topic")]
    [InlineData("Endpoint=sb-cezzis.com/;SharedAccessKeyName=TestSendPolicy;SharedAccessKey=4235os1ddeslfdffsdd=;EntityPath=sbt-topic;")]
    public async Task servicebussender___sendjson_sharedaccesskey(string connectionString)
    {
        this.SetupEnvironment();

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = connectionString,
                Label = "label",
                QueueOrTopicName = "sbt-test-topic"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.IsAny<ServiceBusSender>(),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    [Fact]
    public async Task servicebussender___sendjson_multiple_topics()
    {
        this.SetupEnvironment();

        var connectionString = "Endpoint=sb://sb-not-real-cezzis-001.servicebus.windows.net/;SharedAccessKeyName=TestSend;SharedAccessKey=4235os1ddeslfdffsdd=";

        var correlationId = GuidString();
        var sessionId = GuidString();
        var sender = this.ServiceProvider.GetRequiredService<IServiceBusMessageSender>();

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = connectionString,
                Label = "label1",
                QueueOrTopicName = "sbt-test-topic"
            },
            correlationId: correlationId,
            contentType: "text/xml",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        await sender.SendJson(
            json: "{}",
            configuration: new SendConfiguration
            {
                SendConnectionString = connectionString,
                Label = "label2",
                QueueOrTopicName = "sbt-test-other-topic"
            },
            correlationId: correlationId,
            contentType: "application/json",
            sessionId: sessionId,
            enqueueDelay: TimeSpan.FromSeconds(100),
            cancellationToken: default).ConfigureAwait(false);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.Is<ServiceBusSender>(x => x.EntityPath == "sbt-test-topic"),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label1" &&
                    m.ContentType == "text/xml" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Once);

        this.serviceBusSenderProxyMock
            .Verify(x => x.ScheduleMessageAsync(
                It.Is<ServiceBusSender>(x => x.EntityPath == "sbt-test-other-topic"),
                It.Is<ServiceBusMessage>((m) =>
                    m.Subject == "label2" &&
                    m.ContentType == "application/json" &&
                    m.CorrelationId == correlationId &&
                    m.SessionId == sessionId &&
                    m.ReplyToSessionId == null),
                It.IsAny<DateTimeOffset>(),
                It.IsAny<CancellationToken>()), Times.Once);

        this.Verify_NoOtherCalls();
    }

    private class TestMessage
    {
        public string MyProp { get; set; }
    }
}