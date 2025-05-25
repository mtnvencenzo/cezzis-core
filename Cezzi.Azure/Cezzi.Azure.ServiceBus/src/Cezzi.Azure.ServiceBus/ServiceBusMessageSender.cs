namespace Cezzi.Azure.ServiceBus;

using global::Azure.Identity;
using global::Azure.Messaging.ServiceBus;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Azure.ServiceBus.IServiceBusMessageSender" />
/// <remarks>Initializes a new instance of the <see cref="ServiceBusMessageSender"/> class.</remarks>
/// <param name="serviceBusSenderProxy">The service bus sender proxy.</param>
/// <exception cref="System.ArgumentNullException">serviceBusSenderProxy</exception>
public class ServiceBusMessageSender(IServiceBusSenderProxy serviceBusSenderProxy) : IServiceBusMessageSender
{
    private readonly IServiceBusSenderProxy serviceBusSenderProxy = serviceBusSenderProxy ?? throw new ArgumentNullException(nameof(serviceBusSenderProxy));
    private readonly static ConcurrentDictionary<string, ServiceBusClient> serviceBusClients;

    static ServiceBusMessageSender()
    {
        serviceBusClients = new ConcurrentDictionary<string, ServiceBusClient>();
    }

    /// <summary>Sends the specified message.</summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <param name="message">The message.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="enqueueDelay">The enqueue delay.</param>
    /// <param name="onSendFailure">The on send failure.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.ArgumentNullException">message</exception>
    public async Task Send<TMessage>(
        TMessage message,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default)
    {
        if (message == null)
        {
            throw new ArgumentNullException(nameof(message));
        }

        _ = await this.SendMessages(
            messages: [message],
            configuration: configuration,
            correlationId: correlationId,
            contentType: contentType,
            sessionId: sessionId,
            enqueueDelay: enqueueDelay,
            onSendFailure: onSendFailure,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Sends the specified messages.</summary>
    /// <typeparam name="TMessage">The type of the message.</typeparam>
    /// <param name="messages">The messages.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="enqueueDelay">The enqueue delay.</param>
    /// <param name="onSendFailure">The on send failure.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">correlationId
    /// or
    /// messages
    /// or
    /// configuration</exception>
    /// <exception cref="System.Exception"></exception>
    public async Task<int> SendMessages<TMessage>(
        IEnumerable<TMessage> messages,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            throw new ArgumentNullException(nameof(correlationId));
        }

        ArgumentNullException.ThrowIfNull(messages);

        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.SendConnectionString))
        {
            throw new Exception($"{nameof(configuration.SendConnectionString)} is not defined");
        }

        if (string.IsNullOrWhiteSpace(configuration.Label))
        {
            throw new Exception($"{nameof(configuration.Label)} is not defined");
        }

        if (string.IsNullOrWhiteSpace(configuration.QueueOrTopicName))
        {
            throw new Exception($"{nameof(configuration.QueueOrTopicName)} is not defined");
        }

        var sendCount = 0;
        var sender = GetOrCreateServiceBusClient(configuration).CreateSender(configuration.QueueOrTopicName);

        // Get the sender to send the messages with
        await using (sender.ConfigureAwait(false))
        {
            foreach (var message in messages)
            {
                var bytes = ServiceBusMessageSerializer.SerializeToUtf8Bytes(message);

                var serviceBusMessage = new ServiceBusMessage(body: bytes)
                {
                    MessageId = Guid.NewGuid().ToString(),
                    CorrelationId = correlationId,
                    ContentType = contentType ?? "application/json",
                    Subject = configuration.Label
                };

                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    serviceBusMessage.SessionId = sessionId;
                }

                try
                {
                    // Determine the the intent was to schedule the message
                    // or to send it and have it available on the queue immediately
                    if (enqueueDelay.HasValue && enqueueDelay.Value.TotalSeconds > 0)
                    {
                        await this.serviceBusSenderProxy.ScheduleMessageAsync(
                            sender: sender,
                            message: serviceBusMessage,
                            scheduledEnqueueTime: DateTimeOffset.UtcNow.Add(enqueueDelay.Value),
                            cancellationToken: cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        await this.serviceBusSenderProxy.SendMessageAsync(
                            sender: sender,
                            message: serviceBusMessage,
                            cancellationToken: cancellationToken).ConfigureAwait(false);
                    }

                    sendCount++;
                }
                catch (Exception ex)
                {
                    if (onSendFailure != null)
                    {
                        var json = ServiceBusMessageSerializer.SerializeToUtf8String(message);

                        await onSendFailure(
                            message: json,
                            configuration: configuration,
                            messageId: serviceBusMessage.MessageId,
                            correlationId: serviceBusMessage.CorrelationId,
                            contentType: serviceBusMessage.ContentType,
                            sessionId: serviceBusMessage.SessionId,
                            enqueueDelay: enqueueDelay,
                            ex: ex).ConfigureAwait(false);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        return sendCount;
    }

    /// <summary>Sends the json.</summary>
    /// <param name="json">The json.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="enqueueDelay">The enqueue delay.</param>
    /// <param name="onSendFailure">The on send failure.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.ArgumentNullException">json</exception>
    public async Task SendJson(
        string json,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentNullException(nameof(json));
        }

        _ = await this.SendJsonMessages(
            jsons: [json],
            configuration: configuration,
            correlationId: correlationId,
            contentType: contentType,
            sessionId: sessionId,
            enqueueDelay: enqueueDelay,
            onSendFailure: onSendFailure,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Sends the json.</summary>
    /// <param name="jsons">The jsons.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="enqueueDelay">The enqueue delay.</param>
    /// <param name="onSendFailure">The on send failure.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">correlationId
    /// or
    /// jsons
    /// or
    /// configuration</exception>
    /// <exception cref="System.Exception"></exception>
    public async Task<int> SendJsonMessages(
        IEnumerable<string> jsons,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(correlationId))
        {
            throw new ArgumentNullException(nameof(correlationId));
        }

        ArgumentNullException.ThrowIfNull(jsons);

        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(configuration.SendConnectionString))
        {
            throw new Exception($"{nameof(configuration.SendConnectionString)} is not defined");
        }

        if (string.IsNullOrWhiteSpace(configuration.Label))
        {
            throw new Exception($"{nameof(configuration.Label)} is not defined");
        }

        if (string.IsNullOrWhiteSpace(configuration.QueueOrTopicName))
        {
            throw new Exception($"{nameof(configuration.QueueOrTopicName)} is not defined");
        }

        var sendCount = 0;
        var sender = GetOrCreateServiceBusClient(configuration).CreateSender(configuration.QueueOrTopicName);

        // Get the sender to send the messages with
        await using (sender.ConfigureAwait(false))
        {
            foreach (var json in jsons)
            {
                var bytes = Encoding.UTF8.GetBytes(json);

                var serviceBusMessage = new ServiceBusMessage(body: bytes)
                {
                    MessageId = Guid.NewGuid().ToString(),
                    CorrelationId = correlationId,
                    ContentType = contentType ?? "application/json",
                    Subject = configuration.Label
                };

                if (!string.IsNullOrWhiteSpace(sessionId))
                {
                    serviceBusMessage.SessionId = sessionId;
                }

                try
                {
                    // Determine the the intent was to schedule the message
                    // or to send it and have it available on the queue immediately
                    if (enqueueDelay.HasValue && enqueueDelay.Value.TotalSeconds > 0)
                    {
                        await this.serviceBusSenderProxy.ScheduleMessageAsync(
                            sender: sender,
                            message: serviceBusMessage,
                            scheduledEnqueueTime: DateTimeOffset.UtcNow.Add(enqueueDelay.Value),
                            cancellationToken: cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        await this.serviceBusSenderProxy.SendMessageAsync(
                            sender: sender,
                            message: serviceBusMessage,
                            cancellationToken: cancellationToken).ConfigureAwait(false);
                    }

                    sendCount++;
                }
                catch (Exception ex)
                {
                    if (onSendFailure != null)
                    {
                        await onSendFailure(
                            message: json,
                            configuration: configuration,
                            messageId: serviceBusMessage.MessageId,
                            correlationId: serviceBusMessage.CorrelationId,
                            contentType: serviceBusMessage.ContentType,
                            sessionId: serviceBusMessage.SessionId,
                            enqueueDelay: enqueueDelay,
                            ex: ex).ConfigureAwait(false);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
        }

        return sendCount;
    }

    private static ServiceBusClient GetOrCreateServiceBusClient(ISendConfiguration configuration)
    {
        var retry = configuration.GetSendRetry();
        var connectionString = BuildConnectionString(configuration);

        if (serviceBusClients.TryGetValue(connectionString, out var serviceBusClient))
        {
            // If the client is closed then 
            if (serviceBusClient == null || serviceBusClient.IsClosed)
            {
                serviceBusClients.TryRemove(connectionString, out var _);
            }
        }

        if (serviceBusClient == null || serviceBusClient.IsClosed)
        {
            // ---------------------------------------------
            // Using a shared access token connection string
            // ---------------------------------------------
            if (connectionString.Contains("SharedAccessKeyName"))
            {
                serviceBusClient = new ServiceBusClient(
                    connectionString: connectionString,
                    options: new ServiceBusClientOptions
                    {
                        RetryOptions = new ServiceBusRetryOptions
                        {
                            Mode = ServiceBusRetryMode.Exponential,
                            TryTimeout = TimeSpan.FromSeconds(retry.OperationTimeoutInSeconds),
                            Delay = TimeSpan.FromSeconds(retry.RetryDelaySeconds),
                            MaxRetries = retry.MaxRetries,
                            MaxDelay = TimeSpan.FromSeconds(retry.MaxRetryDelaySeconds)
                        },
                        TransportType = ServiceBusTransportType.AmqpTcp
                    });
            }
            else
            {
                // ---------------------------------------------
                // Using managed identity access connection string
                // ---------------------------------------------
                serviceBusClient = new ServiceBusClient(
                    fullyQualifiedNamespace: connectionString,
                    credential: new DefaultAzureCredential(),
                    options: new ServiceBusClientOptions
                    {
                        RetryOptions = new ServiceBusRetryOptions
                        {
                            Mode = ServiceBusRetryMode.Exponential,
                            TryTimeout = TimeSpan.FromSeconds(retry.OperationTimeoutInSeconds),
                            Delay = TimeSpan.FromSeconds(retry.RetryDelaySeconds),
                            MaxRetries = retry.MaxRetries,
                            MaxDelay = TimeSpan.FromSeconds(retry.MaxRetryDelaySeconds)
                        },
                        TransportType = ServiceBusTransportType.AmqpTcp
                    });
            }

            if (!serviceBusClients.TryAdd(connectionString, serviceBusClient))
            {
                _ = serviceBusClients.TryGetValue(connectionString, out serviceBusClient);
            }
        }

        return serviceBusClient;
    }

    private static string BuildConnectionString(ISendConfiguration configuration)
    {
        if (configuration.SendConnectionString.Contains("sharedaccesskeyname", StringComparison.CurrentCultureIgnoreCase))
        {
            // ------------------------------------------------------------------------------
            // Using shared access connection which reuqires the entity path (queue or topic)
            // in the connection string. Format the connection string so it contains 
            // the send configurations queueOrTopic as the entity path in the string.
            // ------------------------------------------------------------------------------
            var connectionString = configuration.SendConnectionString.EndsWith(';')
                ? configuration.SendConnectionString
                : $"{configuration.SendConnectionString};";

            if (!connectionString.Contains("entitypath", StringComparison.CurrentCultureIgnoreCase))
            {
                connectionString = $"{connectionString}EntityPath={configuration.QueueOrTopicName}";
            }

            return connectionString;
        }

        // ------------------------------------------------------------------------------
        // Made it this var means that were using managed identity access (not a shared access token)
        // and a standard connection string without the entity path (queue or topic)
        // Just return the actual connection string.
        // ------------------------------------------------------------------------------
        return configuration.SendConnectionString;
    }
}
