namespace Cezzi.OpenMarket;

/// <summary>
/// 
/// </summary>
public class SmsSendResult
{
    /// <summary>Initializes a new instance of the <see cref="SmsSendResult"/> class.</summary>
    public SmsSendResult()
    {
        this.SendStatus = SmsSendStatus.None;
    }

    /// <summary>Gets or sets the send status.</summary>
    /// <value>The send status.</value>
    public SmsSendStatus SendStatus { get; set; }

    /// <summary>Gets or sets the send status message.</summary>
    /// <value>The send status message.</value>
    public string SendStatusMessage { get; set; }

    /// <summary>Gets or sets the detailed message.</summary>
    /// <value>The detailed message.</value>
    public string DetailedMessage { get; set; }

    /// <summary>Gets or sets the request identifier.</summary>
    /// <value>The request identifier.</value>
    public string RequestId { get; set; }

    /// <summary>Gets or sets the location.</summary>
    /// <value>The location.</value>
    public string Location { get; set; }
}
