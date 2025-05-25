namespace Cezzi.Applications;

/// <summary>
/// 
/// </summary>
public static class RegexConstants
{
    /// <summary>
    /// The valid email address regex expression
    /// </summary>
    public const string VALID_EMAIL_ADDRESS_REGEX_EXPRESSION = @"^([a-zA-Z0-9_]{1}[a-zA-Z0-9_.\-\+]*)(@[a-zA-Z0-9]{1}[a-zA-Z0-9_-]*)([\\\.]([a-zA-Z0-9]{1,})){1,}$";

    /// <summary>
    /// The valid phone number regex expression
    /// </summary>
    public const string VALID_PHONE_REGEX_EXPRESSION = @"^[2-9]\d{2}-\d{3}-\d{4}$";

    /// <summary>
    /// The valid usa postalcode regex expression
    /// </summary>
    public const string VALID_USA_POSTALCODE_REGEX_EXPRESSION = @"^\d{5}-\d{4}|\d{5}$";

    /// <summary>
    /// The valid can postalcode regex expression
    /// </summary>
    public const string VALID_CAN_POSTALCODE_REGEX_EXPRESSION = @"^[ABCEGHJKLMNPRSTVXYabceghjklmnprstvxy]{1}\d{1}[A-Za-z]{1} *\d{1}[A-Za-z]{1}\d{1}$";

    /// <summary>
    /// The pci card number in field violation expression
    /// </summary>
    public const string PCI_CARD_NUMBER_IN_FIELD_VIOLATION_EXPRESSION = @"[0-9]{12,}";

    /// <summary>
    /// The pci routing or account number in field violation expression
    /// </summary>
    public const string PCI_ROUTING_OR_ACCOUNT_NUMBER_IN_FIELD_VIOLATION_EXPRESSION = @"[0-9]{9,}";
}