namespace Cezzi.Smtp;

using System.Net;
using System.Net.Mail;

/// <summary>
/// 
/// </summary>
public interface ISmtpMailClient : IDisposable
{
    /// <summary>Sends the mail asynchronous.</summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task SendMailAsync(MailMessage message, CancellationToken cancellationToken = default);

    /// <summary>Gets or sets the host.</summary>
    /// <value>The host.</value>
    string Host { get; set; }

    /// <summary>Gets or sets the port.</summary>
    /// <value>The port.</value>
    int Port { get; set; }

    /// <summary>Gets or sets a value indicating whether [enable SSL].</summary>
    /// <value><c>true</c> if [enable SSL]; otherwise, <c>false</c>.</value>
    bool EnableSsl { get; set; }

    /// <summary>Gets or sets the delivery method.</summary>
    /// <value>The delivery method.</value>
    SmtpDeliveryMethod DeliveryMethod { get; set; }

    /// <summary>Gets or sets a value indicating whether [use default credentials].</summary>
    /// <value><c>true</c> if [use default credentials]; otherwise, <c>false</c>.</value>
    bool UseDefaultCredentials { get; set; }

    /// <summary>Gets or sets the credentials.</summary>
    /// <value>The credentials.</value>
    ICredentialsByHost Credentials { get; set; }
}
