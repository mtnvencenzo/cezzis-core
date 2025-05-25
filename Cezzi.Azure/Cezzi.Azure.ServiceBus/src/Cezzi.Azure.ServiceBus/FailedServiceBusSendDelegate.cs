namespace Cezzi.Azure.ServiceBus;

using System;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <param name="message">The message.</param>
/// <param name="configuration">The configuration.</param>
/// <param name="messageId">The message identifier.</param>
/// <param name="correlationId">The correlation identifier.</param>
/// <param name="contentType">Type of the content.</param>
/// <param name="sessionId">The session identifier.</param>
/// <param name="enqueueDelay">The enqueue delay.</param>
/// <param name="ex">The ex.</param>
public delegate Task FailedServiceBusSendDelegate(
    string message,
    ISendConfiguration configuration,
    string messageId,
    string correlationId,
    string contentType,
    string sessionId,
    TimeSpan? enqueueDelay,
    Exception ex);