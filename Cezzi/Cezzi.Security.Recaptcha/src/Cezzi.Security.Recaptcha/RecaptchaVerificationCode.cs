namespace Cezzi.Security.Recaptcha;

/// <summary>
/// 
/// </summary>
public class RecaptchaVerificationCode
{
    /// <summary>Gets or sets the code.</summary>
    /// <value>The code.</value>
    public int Code { get; set; }

    /// <summary>Gets or sets the message.</summary>
    /// <value>The message.</value>
    public string Message { get; set; }

    /// <summary>Gets the communication error.</summary>
    /// <value>The communication error.</value>
    public static RecaptchaVerificationCode CommunicationError => new()
    {
        Code = 100,
        Message = "Communication Error"
    };

    /// <summary>Gets the secret required.</summary>
    /// <value>The secret required.</value>
    public static RecaptchaVerificationCode SecretRequired => new()
    {
        Code = 101,
        Message = "Secret Key Is Required"
    };

    /// <summary>Gets the secret invalid.</summary>
    /// <value>The secret invalid.</value>
    public static RecaptchaVerificationCode SecretInvalid => new()
    {
        Code = 102,
        Message = "Secret Is Invalid"
    };

    /// <summary>Gets the verification code required.</summary>
    /// <value>The verification code required.</value>
    public static RecaptchaVerificationCode VerificationCodeRequired => new()
    {
        Code = 103,
        Message = "Verification Code Is Required"
    };

    /// <summary>Gets the verification code invalid.</summary>
    /// <value>The verification code invalid.</value>
    public static RecaptchaVerificationCode VerificationCodeInvalid => new()
    {
        Code = 104,
        Message = "Verification Code Is Invalid"
    };

    /// <summary>Gets the invalid request.</summary>
    /// <value>The invalid request.</value>
    public static RecaptchaVerificationCode InvalidRequest => new()
    {
        Code = 105,
        Message = "Invalid Request"
    };

    /// <summary>Gets the timeout.</summary>
    /// <value>The timeout.</value>
    public static RecaptchaVerificationCode Timeout => new()
    {
        Code = 106,
        Message = "Timeout"
    };
}
