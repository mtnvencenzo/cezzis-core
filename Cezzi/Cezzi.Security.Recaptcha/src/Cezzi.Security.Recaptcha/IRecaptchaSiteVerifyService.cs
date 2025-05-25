namespace Cezzi.Security.Recaptcha;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IRecaptchaSiteVerifyService
{
    /// <summary>Verifies the specified verification code.</summary>
    /// <param name="verificationCode">The verification code.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="userIp">The user ip.</param>
    /// <returns></returns>
    Task<RecaptchaVerificationResult> Verify(string verificationCode, RecaptchaConfig config, string userIp = null);
}
