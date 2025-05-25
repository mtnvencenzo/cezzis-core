namespace Cezzi.OpenMarket;

/// <summary>
/// 
/// </summary>
public class SmsSendRequest
{
    /// <summary>The phone number to which you are sending an SMS message.</summary>
    /// <value>The phone number.</value>
    public string DestinationPhoneNumber { get; set; }

    /// <summary>Gets or sets the destination country code.</summary>
    /// <value>The destination country code.</value>
    public string DestinationCountryCode { get; set; } = "1";

    /// <summary>Gets or sets the content of the text.</summary>
    /// <value>The content of the text.</value>
    public string TextContent { get; set; }

    /// <summary>Use this to add data to the request that you may want available in reports, such as individual identifiers 
    /// (e.g. your own transaction, ticket, or system IDs). It has no effect on the message or its delivery. The value is free-form text 
    /// that is 0 to 200 characters in length.
    /// </summary>
    /// <value>The note1.</value>
    public string Note1 { get; set; }

    /// <summary>Use this to add data to the request that you may want available in reports, such as individual identifiers 
    /// (e.g.your own transaction, ticket, or system IDs). It has no effect on the message or its delivery. 
    /// The value is free-form text that is 0 to 200 characters in length.</summary>
    /// <value>The note2.</value>
    public string Note2 { get; set; }

    /// <summary>Required for US messaging when using a short code. The value is ignored for all other messaging.
    /// </summary>
    /// <remarks>
    /// This identifies a pre-provisioned program linked to the short code messaging service you are providing.OpenMarket will provide you with the value, which will be between 1 to 50 characters, and is not case-sensitive.
    /// </remarks>
    /// <value>The program identifier.</value>
    public string ProgramId { get; set; } = "1234";

    /// <summary>Determines whether the message is sent as a flash message. Flash messages are shown immediately on an end user's mobile 
    /// phone without the user taking any action, and are often not saved to the phone. We recommend avoiding sending an SMS that is both 
    /// multipart and flash.
    /// </summary>
    /// <value><c>true</c> if flash; otherwise, <c>false</c>.</value>
    public bool Flash { get; set; } = false;

    /// <summary>An OpenMarket-specific number that identifies the mobile operator to which OpenMarket should route the message.</summary>
    /// <remarks>
    /// You can include mobileOperatorId when you are sending a message using a US or Canada short code, toll-free number, or landline number. 
    /// It is optional as OpenMarket will perform a dynamic operator lookup if you do not supply a value. Depending on your contract with OpenMarket, you may be charged for each lookup.        
    /// <para>
    /// Don't include mobileOperatorId when you are sending global messages, or sending using a US or Canadian virtual mobile number (VMN). OpenMarket ignores the value and performs a dynamic 
    /// operator lookup for these messages. You are not charged for these lookups.
    /// </para>
    /// For a list of the valid IDs, see <a href='https://www.openmarket.com/docs/Content/globalcoverage/mobile-operator-ids.htm'/>
    /// </remarks>
    /// <value>The mobile operator identifier.</value>
    public string MobileOperatorId { get; set; } = "383";

    /// <summary>Specifies what action to take if the message content is larger than a single part SMS</summary>
    /// <value>The MLC.</value>
    public MlcType Mlc { get; set; } = MlcType.Segment;

    /// <summary>Specifies the period (in seconds) that OpenMarket and mobile operators will attempt to deliver the message. 
    /// You can specify a number between 1 and 259200</summary>
    /// <value>The validity period.</value>
    public int ValidityPeriod { get; set; } = 259200;

    /// <summary>Identifies what type of message you are sending (text, WAP Push, or binary). For text messages, it also 
    /// identifies whether you are sending the message contents to us as plain text or hex-encoded text.</summary>
    /// <value>The type.</value>
    public SmsMessageType Type { get; set; } = SmsMessageType.Text;
}
