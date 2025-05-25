namespace Cezzi.SendGrid;

/// <summary>
/// 
/// </summary>
public class SendGridConfiguration
{
    /// <summary>Gets or sets the send grid host.</summary>
    /// <value>The send grid host.</value>
    public string SendGridHost { get; set; }

    /// <summary>Gets or sets the send grid URL path.</summary>
    /// <value>The send grid URL path.</value>
    public string SendGridUrlPath { get; set; }

    /// <summary>Gets or sets the send grid version.</summary>
    /// <value>The send grid version.</value>
    public string SendGridVersion { get; set; }

    /// <summary>Gets or sets the send grid API key.</summary>
    /// <value>The send grid API key.</value>
    public string SendGridApiKey { get; set; }

    /// <summary>Gets or sets a value indicating whether [HTTP errors as exceptions].</summary>
    /// <value><c>true</c> if [HTTP errors as exceptions]; otherwise, <c>false</c>.</value>
    public bool HttpErrorsAsExceptions { get; set; }

    /// <summary>Gets or sets the reliability.</summary>
    /// <value>The reliability.</value>
    public SendGridReliability Reliability { get; set; }
}
