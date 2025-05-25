namespace Cezzi.Security.Recaptcha;

/// <summary>
/// 
/// </summary>
public class RecaptchaConfig
{
    /// <summary>The section name</summary>
    public const string SectionName = "Recaptcha";

    /// <summary>Gets or sets the site secret.</summary>
    /// <value>The site secret.</value>
    public string SiteSecret { get; set; }

    /// <summary>Gets or sets the site verify URL.</summary>
    /// <value>The site verify URL.</value>
    public string SiteVerifyUrl { get; set; }
}
