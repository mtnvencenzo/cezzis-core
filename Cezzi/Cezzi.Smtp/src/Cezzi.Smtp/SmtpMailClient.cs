namespace Cezzi.Smtp;

using System.Net;
using System.Net.Mail;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Smtp.ISmtpMailClient" />
public class SmtpMailClient : ISmtpMailClient
{
    private readonly SmtpClient smtpClient;

    /// <summary>Initializes a new instance of the <see cref="SmtpMailClient"/> class.</summary>
    public SmtpMailClient()
    {
        this.smtpClient = new SmtpClient();
    }

    /// <summary>Initializes a new instance of the <see cref="SmtpMailClient"/> class.</summary>
    /// <param name="host">The host.</param>
    public SmtpMailClient(string host)
    {
        this.smtpClient = new SmtpClient(host);
    }

    /// <summary>Initializes a new instance of the <see cref="SmtpMailClient"/> class.</summary>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    public SmtpMailClient(string host, int port)
    {
        this.smtpClient = new SmtpClient(host, port);
    }

    /// <summary>Gets or sets the host.</summary>
    /// <value>The host.</value>
    public string Host { get => this.smtpClient.Host; set => this.smtpClient.Host = value; }

    /// <summary>Gets or sets the port.</summary>
    /// <value>The port.</value>
    public int Port
    {
        get => this.smtpClient.Port;
        set => this.smtpClient.Port = value;
    }

    /// <summary>Gets or sets a value indicating whether [enable SSL].</summary>
    /// <value><c>true</c> if [enable SSL]; otherwise, <c>false</c>.</value>
    public bool EnableSsl
    {
        get => this.smtpClient.EnableSsl;
        set => this.smtpClient.EnableSsl = value;
    }

    /// <summary>Gets or sets the delivery method.</summary>
    /// <value>The delivery method.</value>
    public SmtpDeliveryMethod DeliveryMethod
    {
        get => this.smtpClient.DeliveryMethod;
        set => this.smtpClient.DeliveryMethod = value;
    }

    /// <summary>Gets or sets a value indicating whether [use default credentials].</summary>
    /// <value><c>true</c> if [use default credentials]; otherwise, <c>false</c>.</value>
    public bool UseDefaultCredentials
    {
        get => this.smtpClient.UseDefaultCredentials;
        set => this.smtpClient.UseDefaultCredentials = value;
    }

    /// <summary>Gets or sets the credentials.</summary>
    /// <value>The credentials.</value>
    public ICredentialsByHost Credentials
    {
        get => this.smtpClient.Credentials;
        set => this.smtpClient.Credentials = value;
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose() => this.smtpClient.Dispose();

    /// <summary>Sends the mail asynchronous.</summary>
    /// <param name="message">The message.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task SendMailAsync(MailMessage message, CancellationToken cancellationToken = default) => await this.smtpClient.SendMailAsync(message, cancellationToken);
}
