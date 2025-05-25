namespace Cezzi.Azure.ServiceBus;

/// <summary>
/// 
/// </summary>
public class SendRetryOptions
{
    /// <summary>Gets or sets the operation timeout in seconds.</summary>
    /// <value>The operation timeout in seconds.</value>
    public int OperationTimeoutInSeconds { get; set; }

    /// <summary>Gets or sets the retry delay seconds.</summary>
    /// <value>The retry delay seconds.</value>
    public int RetryDelaySeconds { get; set; }

    /// <summary>Gets or sets the maximum retries.</summary>
    /// <value>The maximum retries.</value>
    public int MaxRetries { get; set; }

    /// <summary>Gets or sets the maximum retry delay seconds.</summary>
    /// <value>The maximum retry delay seconds.</value>
    public int MaxRetryDelaySeconds { get; set; }
}
