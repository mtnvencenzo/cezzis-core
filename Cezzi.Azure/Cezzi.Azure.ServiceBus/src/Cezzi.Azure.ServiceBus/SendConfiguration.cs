namespace Cezzi.Azure.ServiceBus;
/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Azure.ServiceBus.ISendConfiguration" />
public class SendConfiguration : ISendConfiguration
{
    private const int defaultMaxRetries = 3;
    private const int defaultMaxRetryDelaySeconds = 30;
    private const int defaultOperationTimeoutInSeconds = 10;
    private const int defaultRetryDelaySeconds = 5;

    /// <summary>Gets or sets the label.</summary>
    /// <value>The label.</value>
    public string Label { get; set; }

    /// <summary>Gets or sets the name of the queue or topic.</summary>
    /// <value>The name of the queue or topic.</value>
    public string QueueOrTopicName { get; set; }

    /// <summary>Gets or sets the send connection string.</summary>
    /// <value>The send connection string.</value>
    public string SendConnectionString { get; set; }

    /// <summary>Gets or sets the send retry.</summary>
    /// <value>The send retry.</value>
    public SendRetryOptions SendRetry { get; set; }

    /// <summary>Clones the configuration using any supplied parameters.  Parameters that are not specified will use the original values from the object being cloned from..</summary>
    /// <param name="label">The label.</param>
    /// <param name="queueOrTopicName">Name of the queue or topic.</param>
    /// <param name="sendConnectionString">The send connection string.</param>
    /// <param name="sendRetry">The send retry.</param>
    /// <returns></returns>
    public SendConfiguration Clone(
        string label = null,
        string queueOrTopicName = null,
        string sendConnectionString = null,
        SendRetryOptions sendRetry = null)
    {
        return new SendConfiguration
        {
            Label = label ?? this.Label,
            QueueOrTopicName = queueOrTopicName ?? this.QueueOrTopicName,
            SendConnectionString = sendConnectionString ?? this.SendConnectionString,
            SendRetry = (sendRetry ?? this.SendRetry) != null
                ? new SendRetryOptions
                {
                    MaxRetries = sendRetry?.MaxRetries ?? this.SendRetry.MaxRetries,
                    MaxRetryDelaySeconds = sendRetry?.MaxRetryDelaySeconds ?? this.SendRetry.MaxRetryDelaySeconds,
                    OperationTimeoutInSeconds = sendRetry?.OperationTimeoutInSeconds ?? this.SendRetry.OperationTimeoutInSeconds,
                    RetryDelaySeconds = sendRetry?.RetryDelaySeconds ?? this.SendRetry.RetryDelaySeconds,
                } : null
        };
    }

    /// <summary>Gets the send retry.</summary>
    /// <returns></returns>
    public SendRetryOptions GetSendRetry()
    {
        return new SendRetryOptions
        {
            MaxRetries = this.SendRetry?.MaxRetries ?? defaultMaxRetries,
            MaxRetryDelaySeconds = this.SendRetry?.MaxRetryDelaySeconds ?? defaultMaxRetryDelaySeconds,
            OperationTimeoutInSeconds = this.SendRetry?.OperationTimeoutInSeconds ?? defaultOperationTimeoutInSeconds,
            RetryDelaySeconds = this.SendRetry?.RetryDelaySeconds ?? defaultRetryDelaySeconds,
        };
    }
}
