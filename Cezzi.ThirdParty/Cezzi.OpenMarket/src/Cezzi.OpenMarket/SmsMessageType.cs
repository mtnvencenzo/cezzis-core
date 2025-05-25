namespace Cezzi.OpenMarket;

/// <summary>
/// Identifies what type of message you are sending(text, WAP Push, or binary). For text messages, it also identifies whether you are sending 
/// the message contents to us as plain text or hex-encoded text.
/// </summary>
public enum SmsMessageType
{
    /// <summary>Message is in UTF-8.</summary>
    Text = 1,

    /// <summary>Message is hex encoded</summary>
    HexEncodedText = 2,

    /// <summary>Message is binary content</summary>
    Binary = 3,

    /// <summary>Message is a WAP Push</summary>
    WrapPush = 4
}
