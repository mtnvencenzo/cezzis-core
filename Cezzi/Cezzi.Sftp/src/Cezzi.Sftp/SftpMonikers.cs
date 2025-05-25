namespace Cezzi.Sftp;

/// <summary>
/// 
/// </summary>
public class SftpMonikers
{
    /// <summary>Gets the SFTP progress.</summary>
    /// <value>The SFTP progress.</value>
    public string SftpProgress => "@sftp_progress";

    /// <summary>Gets the SFTP exception.</summary>
    /// <value>The SFTP exception.</value>
    public string SftpException => "@sftp_exception";

    /// <summary>Gets the SFTP path.</summary>
    /// <value>The SFTP path.</value>
    public string SftpPath => "@sftp_path";

    /// <summary>Gets the SFTP new path.</summary>
    /// <value>The SFTP new path.</value>
    public string SftpNewPath => "@sftp_newpath";

    /// <summary>Gets the SFTP bytes.</summary>
    /// <value>The SFTP bytes.</value>
    public string SftpBytes => "@sftp_bytes";

    /// <summary>Gets the SFTP search pattern.</summary>
    /// <value>The SFTP search pattern.</value>
    public string SftpSearchPattern => "@sftp_searchpattern";

    /// <summary>Gets the name of the SFTP file.</summary>
    /// <value>The name of the SFTP file.</value>
    public string SftpFileName => "@sftp_file_name";

    /// <summary>Gets the full name of the SFTP file.</summary>
    /// <value>The full name of the SFTP file.</value>
    public string SftpFileFullName => "@sftp_file_fullname";

    /// <summary>Gets the SFTP file is directory.</summary>
    /// <value>The SFTP file is directory.</value>
    public string SftpFileIsDirectory => "@sftp_file_isdirectory";

    /// <summary>Gets the SFTP file is regular file.</summary>
    /// <value>The SFTP file is regular file.</value>
    public string SftpFileIsRegularFile => "@sftp_file_isregularfile";

    /// <summary>Gets the SFTP file is symbolic link.</summary>
    /// <value>The SFTP file is symbolic link.</value>
    public string SftpFileIsSymbolicLink => "@sftp_file_issymboliclink";

    /// <summary>Gets the SFTP file is socket.</summary>
    /// <value>The SFTP file is socket.</value>
    public string SftpFileIsSocket => "@sftp_file_issocket";

    /// <summary>Gets the SFTP file is named pipe.</summary>
    /// <value>The SFTP file is named pipe.</value>
    public string SftpFileIsNamedPipe => "@sftp_file_isnamedpipe";

    /// <summary>Gets the SFTP authentication information.</summary>
    /// <value>The SFTP authentication information.</value>
    public string SftpAuthInfo => "@sftp_authinfo";

    /// <summary>Gets the SFTP banner information.</summary>
    /// <value>The SFTP banner information.</value>
    public string SftpBannerInfo => "@sftp_bannerinfo";

    /// <summary>Gets the SFTP host key.</summary>
    /// <value>The SFTP host key.</value>
    public string SftpHostKey => "@sftp_hostkey";

    /// <summary>Gets the SFTP retry wait.</summary>
    /// <value>The SFTP retry wait.</value>
    public string SftpRetryWait => "@sftp_retry_wait";

    /// <summary>Gets the SFTP operation.</summary>
    /// <value>The SFTP operation.</value>
    public string SftpOperation => "sftp_operation";

    /// <summary>Gets the SFTP connected host.</summary>
    /// <value>The SFTP connected host.</value>
    public string SftpConnectedHost => "sftp_connected_host";

    /// <summary>Gets the SFTP connecting host.</summary>
    /// <value>The SFTP connecting host.</value>
    public string SftpConnectingHost => "sftp_connecting_host";

    /// <summary>Gets the SFTP connected username.</summary>
    /// <value>The SFTP connected username.</value>
    public string SftpConnectedUsername => "sftp_connected_username";

    /// <summary>Gets the SFTP connecting username.</summary>
    /// <value>The SFTP connecting username.</value>
    public string SftpConnectingUsername => "sftp_connecting_username";
}
