namespace Cezzi.Smtp;
/// <summary>
/// 
/// </summary>
public interface ISmtpClientFactory
{
    /// <summary>Creates the client.</summary>
    /// <returns></returns>
    ISmtpMailClient CreateClient();

    /// <summary>Creates the client.</summary>
    /// <param name="host">The host.</param>
    /// <returns></returns>
    ISmtpMailClient CreateClient(string host);

    /// <summary>Creates the client.</summary>
    /// <param name="host">The host.</param>
    /// <param name="port">The port.</param>
    /// <returns></returns>
    ISmtpMailClient CreateClient(string host, int port);
}
