namespace Cezzi.Sftp;

using System;

/// <summary>
/// Sftp configuration settings
/// </summary>
public class SftpConfig
{
    /// <summary>
    /// default constructor
    /// </summary>
    public SftpConfig()
    {
        this.ConnectTimeout = TimeSpan.FromMinutes(10);

        this.OperationTimeout = TimeSpan.FromHours(8);

        this.ConnectRetryAttempts = 3;

        this.Port = 22;
    }

    /// <summary>Gets or sets the host name.</summary>
    /// <value>The host name.</value>
    public string Hostname { get; set; }

    /// <summary>Gets or sets the user name.</summary>
    /// <value>The user name.</value>
    public string Username { get; set; }

    /// <summary>Gets or sets the password.</summary>
    /// <value>The password.</value>
    public string Password { get; set; }

    /// <summary>Gets or sets the port.</summary>
    /// <value>The port.</value>
    public int Port { get; set; }

    /// <summary>Gets or sets the connection timeout.</summary>
    /// <value>The connection timeout.</value>
    public TimeSpan ConnectTimeout { get; set; }

    /// <summary>Gets or sets the operation timeout.</summary>
    /// <value>The operation timeout.</value>
    public TimeSpan OperationTimeout { get; set; }

    /// <summary>Gets or sets the connection retry attempts.</summary>
    /// <value>The connection retry attempts.</value>
    public int ConnectRetryAttempts { get; set; }

    /// <summary>Gets or sets the private key.</summary>
    /// <value>The private key.</value>
    public SftpPrivateKey PrivateKey { get; set; }
}
