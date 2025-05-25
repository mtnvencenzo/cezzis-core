namespace Cezzi.Smtp;
/// <summary>
/// 
/// </summary>
public class SmtpClientFactory : ISmtpClientFactory
{
    /// <summary>Creates the client.</summary>
    /// <returns></returns>
    public virtual ISmtpMailClient CreateClient() => new SmtpMailClient();

    /// <summary>Creates the client.</summary>
    /// <param name="host">The host.</param>
    /// <returns></returns>
    public virtual ISmtpMailClient CreateClient(string host) => new SmtpMailClient(host);

    /// <summary>Creates the client.</summary>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    /// <returns></returns>
    public virtual ISmtpMailClient CreateClient(string host, int port) => new SmtpMailClient(host, port);
}
