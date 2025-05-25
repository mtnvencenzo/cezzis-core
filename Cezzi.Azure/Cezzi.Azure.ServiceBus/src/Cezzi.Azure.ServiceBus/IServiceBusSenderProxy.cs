namespace Cezzi.Azure.ServiceBus;

using global::Azure.Messaging.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IServiceBusSenderProxy
{
    /// <summary>Sends the message asynchronous.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task SendMessageAsync(
        ServiceBusSender sender,
        ServiceBusMessage message,
        CancellationToken cancellationToken = default);

    /// <summary>Internals the schedule message asynchronous.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="message">The message.</param>
    /// <param name="scheduledEnqueueTime">The scheduled enqueue time.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task ScheduleMessageAsync(
        ServiceBusSender sender,
        ServiceBusMessage message,
        DateTimeOffset scheduledEnqueueTime,
        CancellationToken cancellationToken = default);
}
