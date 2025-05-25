namespace Cezzi.Azure.ServiceBus;

/// <summary>
/// 
/// </summary>
public interface ISendConfiguration
{
    /// <summary>Gets or sets the send connection string.</summary>
    /// <value>The send connection string.</value>
    string SendConnectionString { get; set; }

    /// <summary>Gets or sets the name of the queue or topic.</summary>
    /// <value>The name of the queue or topic.</value>
    string QueueOrTopicName { get; set; }

    /// <summary>Gets or sets the label.</summary>
    /// <value>The label.</value>
    string Label { get; set; }

    /// <summary>Gets or sets the send retry.</summary>
    /// <value>The send retry.</value>
    SendRetryOptions SendRetry { get; set; }

    /// <summary>Gets the send retry.</summary>
    /// <returns></returns>
    SendRetryOptions GetSendRetry();
}
