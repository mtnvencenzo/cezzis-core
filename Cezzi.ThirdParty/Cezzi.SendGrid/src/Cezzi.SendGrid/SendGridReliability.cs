namespace Cezzi.SendGrid;

/// <summary>
/// 
/// </summary>
public class SendGridReliability
{
    /// <summary>Gets or sets the maximum number of retries.</summary>
    /// <value>The maximum number of retries.</value>
    public int MaximumNumberOfRetries { get; set; }

    /// <summary>Gets or sets the minimum backoff seconds.</summary>
    /// <value>The minimum backoff seconds.</value>
    public int MinimumBackoffSeconds { get; set; }

    /// <summary>Gets or sets the maximum backoff seconds.</summary>
    /// <value>The maximum backoff seconds.</value>
    public int MaximumBackoffSeconds { get; set; }

    /// <summary>Gets or sets the delta backoff seconds.</summary>
    /// <value>The delta backoff seconds.</value>
    public int DeltaBackoffSeconds { get; set; }
}
