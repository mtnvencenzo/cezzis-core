namespace Cezzi.Azure.ServiceBus;

using global::Azure.Messaging.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Azure.ServiceBus.IServiceBusSenderProxy" />
public class ServiceBusSenderProxy : IServiceBusSenderProxy
{
    /// <summary>Sends the message asynchronous.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async virtual Task SendMessageAsync(
        ServiceBusSender sender,
        ServiceBusMessage message,
        CancellationToken cancellationToken = default) => await sender.SendMessageAsync(message, cancellationToken).ConfigureAwait(false);

    /// <summary>Internals the schedule message asynchronous.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="message">The message.</param>
    /// <param name="scheduledEnqueueTime">The scheduled enqueue time.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async virtual Task ScheduleMessageAsync(
        ServiceBusSender sender,
        ServiceBusMessage message,
        DateTimeOffset scheduledEnqueueTime,
        CancellationToken cancellationToken = default) => await sender.ScheduleMessageAsync(message, scheduledEnqueueTime, cancellationToken).ConfigureAwait(false);

}
