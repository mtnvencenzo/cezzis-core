namespace Cezzi.OpenMarket;

/// <summary>
/// Specifies what action to take if the message content is larger than a single part SMS
/// </summary>
public enum MlcType
{
    /// <summary>Reject the message</summary>
    Reject = 1,

    /// <summary>Only the first part of the message that fits into a single SMS</summary>
    Truncate = 2,

    /// <summary>Send the message as a multipart message. If this is outside the mobile operator's size limit for a multipart message, then it is rejected.</summary>
    Segment = 3
}
