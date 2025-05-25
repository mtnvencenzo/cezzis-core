namespace Cezzi.Security.Recaptcha;

using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class RecaptchaVerificationResult
{
    /// <summary>Initializes a new instance of the <see cref="RecaptchaVerificationResult"/> class.
    /// </summary>
    public RecaptchaVerificationResult()
    {
        this.ReturnCodes = [];
    }

    /// <summary>Gets or sets the verification status.</summary>
    /// <value>The verification status.</value>
    public RecaptchaVerificationStatus VerificationStatus { get; set; }

    /// <summary>Gets or sets the UTC verification timestamp.</summary>
    /// <value>The UTC verification timestamp.</value>
    public DateTime? UtcVerificationTimestamp { get; set; }

    /// <summary>Gets or sets the hostname.</summary>
    /// <value>The hostname.</value>
    public string Hostname { get; set; }

    /// <summary>Gets or sets the score.</summary>
    /// <value>The score.</value>
    public decimal? Score { get; set; }

    /// <summary>Gets or sets the return codes.</summary>
    /// <value>The return codes.</value>
    public List<RecaptchaVerificationCode> ReturnCodes { get; set; } = [];
}
