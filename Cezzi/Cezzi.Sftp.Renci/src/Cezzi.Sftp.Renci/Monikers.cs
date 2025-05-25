namespace Cezzi.Sftp.Renci;

/// <summary>
/// 
/// </summary>
public static class Monikers
{
    /// <summary>Initializes the <see cref="Monikers"/> class.</summary>
    static Monikers()
    {
        SftpMonikers = new SftpMonikers();
    }

    /// <summary>Gets the SFTP monikers.</summary>
    /// <value>The SFTP monikers.</value>
    public static SftpMonikers SftpMonikers { get; private set; }
}
