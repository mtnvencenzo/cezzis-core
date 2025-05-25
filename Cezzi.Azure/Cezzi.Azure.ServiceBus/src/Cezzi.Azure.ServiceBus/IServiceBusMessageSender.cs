namespace Cezzi.Azure.ServiceBus;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IServiceBusMessageSender
{
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
    /// <returns></returns>
    Task Send<TMessage>(
        TMessage message,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default);

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
    Task<int> SendMessages<TMessage>(
        IEnumerable<TMessage> messages,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default);

    /// <summary>Sends the json.</summary>
    /// <param name="json">The json.</param>
    /// <param name="configuration">The configuration.</param>
    /// <param name="correlationId">The correlation identifier.</param>
    /// <param name="contentType">Type of the content.</param>
    /// <param name="sessionId">The session identifier.</param>
    /// <param name="enqueueDelay">The enqueue delay.</param>
    /// <param name="onSendFailure">The on send failure.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task SendJson(
        string json,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default);

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
    Task<int> SendJsonMessages(
        IEnumerable<string> jsons,
        ISendConfiguration configuration,
        string correlationId,
        string contentType = null,
        string sessionId = null,
        TimeSpan? enqueueDelay = null,
        FailedServiceBusSendDelegate onSendFailure = null,
        CancellationToken cancellationToken = default);
}
