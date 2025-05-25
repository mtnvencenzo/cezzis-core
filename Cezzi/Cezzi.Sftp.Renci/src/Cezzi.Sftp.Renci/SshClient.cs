namespace Cezzi.Sftp.Renci;

using global::Renci.SshNet;
using global::Renci.SshNet.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Sftp.ISftpClient" />
/// <remarks>Initializes a new instance of the <see cref="SshClient"/> class.</remarks>
/// <param name="sftpConfig">The SFTP configuration.</param>
/// <param name="logger">The logger.</param>
/// <exception cref="System.ArgumentNullException">
/// sftpConfig
/// or
/// logger
/// </exception>
public class SshClient(SftpConfig sftpConfig, ILogger logger) : ISftpClient
{
    private readonly SftpConfig sftpConfig = sftpConfig ?? throw new ArgumentNullException(nameof(sftpConfig));
    private readonly ILogger logger = logger ?? throw new ArgumentNullException(nameof(logger));
    /// <inheritdoc/>

    public uint BufferSize { get; set; }
    /// <inheritdoc/>
    public TimeSpan OperationTimeout { get; set; }
    /// <inheritdoc/>
    public int ProtocolVersion { get; }
    /// <inheritdoc/>
    public string WorkingDirectory { get; }

    /// <summary> creates directory if not exists </summary>
    /// <param name="nameofDirectory"></param>
    /// <returns>returns true if directory gets created; else false</returns>
    public virtual bool CreateDirectory(string nameofDirectory)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, nameofDirectory },
            { Monikers.SftpMonikers.SftpOperation, nameof(CreateDirectory) }
        });

        var client = this.Connect();

        try
        {
            if (client.Exists(nameofDirectory))
            {
                this.logger.LogInformation("Ssh directory already exists, skipping directory create");
                return false;
            }

            this.logger.LogInformation("Ssh creating directory");

            client.CreateDirectory(nameofDirectory);
            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh create directory operation failed");
            throw new SftpException("Ssh create directory operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary> deletes directory if not exists </summary>
    /// <param name="nameofDirectory"></param>
    public virtual bool DeleteDirectory(string nameofDirectory)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, nameofDirectory },
            { Monikers.SftpMonikers.SftpOperation, nameof(DeleteDirectory) }
        });

        var client = this.Connect();

        try
        {
            if (client.Exists(nameofDirectory))
            {
                this.logger.LogInformation("Ssh deleting directory");

                client.DeleteDirectory(nameofDirectory);
                return true;
            }

            this.logger.LogInformation("Ssh directory did not exist, skipping directory delete");
            return false;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh delete directory operation failed");
            throw new SftpException("Ssh delete directory operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary>Uploads the file.</summary>
    /// <param name="contents">The contents.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <param name="renameAfterUploadToPath">The rename after upload to path.</param>
    /// <returns></returns>
    public virtual bool UploadFileFromBytes(
        byte[] contents,
        string remoteFilePath,
        bool overwriteIfExists = true,
        string renameAfterUploadToPath = null)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, remoteFilePath },
            { Monikers.SftpMonikers.SftpOperation, nameof(UploadFileFromBytes) }
        });

        using var ms = new MemoryStream(contents);
        return this.UploadFileInternal(ms, remoteFilePath, overwriteIfExists, renameAfterUploadToPath);
    }

    /// <summary>Uploads the file.</summary>
    /// <param name="stream">The stream.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <param name="renameAfterUploadToPath">The rename after upload to path.</param>
    /// <returns></returns>
    public virtual bool UploadFileFromStream(
        Stream stream,
        string remoteFilePath,
        bool overwriteIfExists = true,
        string renameAfterUploadToPath = null)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, remoteFilePath },
            { Monikers.SftpMonikers.SftpOperation, nameof(UploadFileFromStream) }
        });

        return this.UploadFileInternal(stream, remoteFilePath, overwriteIfExists, renameAfterUploadToPath);
    }

    /// <summary>Uploads the files.</summary>
    /// <param name="contents">The contents.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <returns></returns>
    public virtual bool UploadFilesFromBytes(IList<byte[]> contents, string remoteFilePath, bool overwriteIfExists = true)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, remoteFilePath },
            { Monikers.SftpMonikers.SftpOperation, nameof(UploadFilesFromStreams) }
        });

        var memoryStreams = new List<Stream>();

        foreach (var content in contents)
        {
            memoryStreams.Add(new MemoryStream(content));
        }

        try
        {
            return this.UploadFilesInternal(memoryStreams, remoteFilePath, overwriteIfExists);
        }
        finally
        {
            foreach (var ms in memoryStreams)
            {
                try
                {
                    ms.Close();
                }
                catch { }
            }
        }
    }

    /// <summary>Uploads the files.</summary>
    /// <param name="streams">The streams.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <returns></returns>
    public virtual bool UploadFilesFromStreams(IList<Stream> streams, string remoteFilePath, bool overwriteIfExists = true)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, remoteFilePath },
            { Monikers.SftpMonikers.SftpOperation, nameof(UploadFilesFromStreams) }
        });

        return this.UploadFilesInternal(streams, remoteFilePath, overwriteIfExists);
    }

    /// <summary> Deletes the file from the server.</summary>
    /// <param name="fileName">The file name.</param>
    /// <returns>returns true if file is deleted successfully; else false</returns>
    public virtual bool DeleteFile(string fileName)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, fileName },
            { Monikers.SftpMonikers.SftpOperation, nameof(DeleteFile) }
        });

        var client = this.Connect();

        try
        {
            if (client.Exists(fileName))
            {
                this.logger.LogInformation("Ssh deleting file");

                client.DeleteFile(fileName);
                return true;
            }

            this.logger.LogInformation("Ssh path did not exist, skipping file delete");
            return false;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh file delete operation failed");
            throw new SftpException("Ssh file delete operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary> renames remote file from old path to new path </summary>
    /// <param name="oldFilename">The old file name</param>
    /// <param name="newFilename"> The new file name</param>
    /// <param name="isPosix"></param>
    public virtual bool RenameFile(string oldFilename, string newFilename, bool isPosix = false)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, oldFilename },
            { Monikers.SftpMonikers.SftpOperation, nameof(RenameFile) }
        });

        var client = this.Connect();

        try
        {
            if (client.Exists(oldFilename))
            {
                this.logger.LogInformation("Ssh renaming file");

                client.RenameFile(oldFilename, newFilename, isPosix);
                return true;
            }

            this.logger.LogInformation("Ssh path did not exist, skipping file rename");
            return false;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh file rename operation failed");
            throw new SftpException("Ssh file rename operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary>Gets file.</summary>
    /// <param name="filePathAndName">The file path.</param>
    /// <returns>The byte array</returns>
    public virtual byte[] GetFile(string filePathAndName)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, filePathAndName },
            { Monikers.SftpMonikers.SftpOperation, nameof(GetFile) }
        });

        var client = this.Connect();

        try
        {
            this.logger.LogInformation("Downloading ssh file (w/read permissions)");

            using var ms = new MemoryStream();
            using var sftpStream = client.OpenRead(filePathAndName);

            this.logger.LogInformation("Copying file stream to memory stream.");

            sftpStream.CopyTo(ms);

            this.logger.LogInformation("Flushing file stream to memory stream.");

            sftpStream.Flush();

            this.logger.LogInformation("Seeking memory stream.");

            ms.Seek(0, SeekOrigin.Begin);
            ms.Position = 0;

            this.logger.LogInformation("Extracting memory stream bytes.");

            var bytes = ms.ToArray();

            using (var bytesScope = this.logger.BeginScope(new Dictionary<string, object>
                    {
                        { Monikers.SftpMonikers.SftpBytes, bytes.Length }
                    }))
            {
                this.logger.LogInformation("Ssh bytes read");
            }

            return bytes;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh file open read operation failed");
            throw new SftpException("Ssh file open read operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary>Gets the file text.</summary>
    /// <param name="filePathAndName">Name of the file path and.</param>
    /// <returns></returns>
    public virtual string GetFileText(string filePathAndName)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, filePathAndName },
            { Monikers.SftpMonikers.SftpOperation, nameof(GetFileText) }
        });

        var client = this.Connect();

        try
        {
            this.logger.LogInformation("Downloading ssh file text");

            var text = client.ReadAllText(filePathAndName);

            using (var bytesScope = this.logger.BeginScope(new Dictionary<string, object>
                {
                    { Monikers.SftpMonikers.SftpBytes, text?.Length ?? 0 }
                }))
            {
                this.logger.LogInformation("File text read");
            }

            return text;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh file read text operation failed");
            throw new SftpException("Ssh file read text operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary> Gets list of files on the server.</summary>
    /// <param name="path">The path.</param>
    /// <param name="options">The file list options</param>
    /// <returns>list of files in the directory</returns>
    public virtual IList<SftpFile> ListDirectory(string path, SftpFileListOptions options)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, path },
            { Monikers.SftpMonikers.SftpSearchPattern, options?.SearchPattern },
            { Monikers.SftpMonikers.SftpOperation, nameof(ListDirectory) }
        });

        var client = this.Connect();

        try
        {
            this.logger.LogInformation("Ssh listing directory");

            var result = client.ListDirectory(path);

            var filesList = new List<SftpFile>();

            foreach (var item in result)
            {
                using var fileScope = this.logger.BeginScope(new Dictionary<string, object>
                {
                    { Monikers.SftpMonikers.SftpFileName, item.Name },
                    { Monikers.SftpMonikers.SftpFileFullName, item.FullName },
                    { Monikers.SftpMonikers.SftpBytes, item.Length },
                    { Monikers.SftpMonikers.SftpFileIsDirectory, item.IsDirectory },
                    { Monikers.SftpMonikers.SftpFileIsRegularFile, item.IsRegularFile },
                    { Monikers.SftpMonikers.SftpFileIsSymbolicLink, item.IsSymbolicLink },
                    { Monikers.SftpMonikers.SftpFileIsSocket, item.IsSocket },
                    { Monikers.SftpMonikers.SftpFileIsNamedPipe, item.IsNamedPipe }
                });

                if (!item.IsRegularFile && !item.IsDirectory)
                {
                    this.logger.LogInformation("Ssh file is being skipped as it's not a regular file and not a directory");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(options?.SearchPattern) || options.SearchPattern == "*" || Regex.IsMatch(item.Name, options.SearchPattern))
                {
                    if (!item.IsDirectory)
                    {
                        this.logger.LogInformation("Ssh file found");
                    }
                    else
                    {
                        this.logger.LogInformation("Ssh directory found");
                    }

                    filesList.Add(new SftpFile
                    {
                        FullName = item.FullName,
                        LastWriteTime = item.LastWriteTime,
                        Name = item.Name,
                        Length = item.Length,
                        LastAccessTime = item.LastAccessTime,
                        IsDirectory = item.IsDirectory
                    });
                }
                else
                {
                    this.logger.LogInformation("Ssh file is being skipped as it's not a pattern match.");
                }
            }

            return filesList;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh list directory operation failed");
            throw new SftpException("Ssh list directory operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    /// <summary> Verifies if file exists on server or not.</summary>
    /// <param name="remotePath">The remote path.</param>
    /// <returns>return true if exists, else false</returns>
    public virtual bool FileExists(string remotePath)
    {
        var exists = false;

        using (var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpPath, remotePath },
            { Monikers.SftpMonikers.SftpOperation, nameof(FileExists) }
        }))
        {
            var client = this.Connect();

            try
            {
                this.logger.LogInformation("Downloading ssh file text");

                exists = client.Exists(remotePath);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Ssh file read text operation failed");
                throw new SftpException("Ssh file read text operation failed", ex);
            }
            finally
            {
                this.Disconnect(client);
            }
        }

        return exists;
    }

    private SftpClient Connect()
    {
        var connectionInfo = this.GetConnectionInfo();

        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpConnectedHost, connectionInfo.Host },
            { Monikers.SftpMonikers.SftpConnectedUsername, connectionInfo.Username }
        });

        var client = new SftpClient(connectionInfo)
        {
            OperationTimeout = this.sftpConfig.OperationTimeout
        };

        client.ErrorOccurred += this.Client_ErrorOccurred;

        try
        {
            this.logger.LogInformation("Ssh client connecting.");

            client.Connect();
            this.LogClientInfo(client);
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh exception occurred connecting client.");
            this.LogClientInfo(client);
            throw;
        }

        return client;
    }

    private void Disconnect(SftpClient client)
    {
        try
        {
            if (client?.IsConnected ?? false)
            {
                this.logger.LogInformation("Ssh client disconnecting.");

                client.Disconnect();
            }
            else
            {
                this.logger.LogInformation("Ssh client was not connected,  skipping disconnecting.");
            }
        }
        catch (ObjectDisposedException ex)
        {
            this.logger?.LogWarning(ex, "Ssh client already disposed");
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Unable to disconnect from sftp");
        }
        finally
        {
            try
            {
                client?.Dispose();
            }
            catch { }
        }
    }

    private bool UploadFilesInternal(
        IList<Stream> streams,
        string remoteFilePath,
        bool overwriteIfExists = true)
    {
        var client = this.Connect();

        foreach (var stream in streams)
        {
            stream.Flush();
            stream.Seek(0, SeekOrigin.Begin);

            try
            {
                if (client.Exists(remoteFilePath) && overwriteIfExists == false)
                {
                    this.logger.LogInformation("Ssh path exists, skipping upload");
                    return false;
                }

                this.logger.LogInformation("Uploading Ssh file");

                client.UploadFile(
                    input: stream,
                    path: remoteFilePath,
                    canOverride: overwriteIfExists);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Ssh file upload operation failed");
                throw new SftpException("Ssh file upload operation failed", ex);
            }
            finally
            {
                this.Disconnect(client);
            }
        }

        return true;
    }

    private bool UploadFileInternal(
        Stream stream,
        string remoteFilePath,
        bool overwriteIfExists = true,
        string renameAfterUploadToPath = null)
    {
        stream.Flush();
        stream.Seek(0, SeekOrigin.Begin);

        var client = this.Connect();

        try
        {
            if (client.Exists(remoteFilePath) && overwriteIfExists == false)
            {
                this.logger.LogInformation("Ssh path exists, skipping upload");
                return false;
            }

            this.logger.LogInformation("Uploading Ssh file");

            client.UploadFile(
                input: stream,
                path: remoteFilePath,
                canOverride: overwriteIfExists);

            if (!string.IsNullOrWhiteSpace(renameAfterUploadToPath) && remoteFilePath != renameAfterUploadToPath)
            {
                this.logger.LogInformation("Renaming file after upload");

                client.RenameFile(
                    oldPath: remoteFilePath,
                    newPath: renameAfterUploadToPath);
            }

            return true;
        }
        catch (Exception ex)
        {
            this.logger.LogError(ex, "Ssh file upload operation failed");
            throw new SftpException("Ssh file upload operation failed", ex);
        }
        finally
        {
            this.Disconnect(client);
        }
    }

    private ConnectionInfo GetConnectionInfo()
    {
        // Note: AuthenticationMethod implements IDisposable, but this causes issues with multi-threading
        // (i.e. Exception is thrown with "message type 52 is not valid in the current context")
        var autheticationMethods = new List<AuthenticationMethod>();

        if (!string.IsNullOrWhiteSpace(this.sftpConfig.PrivateKey?.Key))
        {
            var privateKeyStream = new MemoryStream(Encoding.ASCII.GetBytes(this.sftpConfig.PrivateKey.Key));

            var key = new PrivateKeyFile(privateKeyStream, this.sftpConfig.PrivateKey.Passphrase);
            autheticationMethods.Add(new PrivateKeyAuthenticationMethod(this.sftpConfig.Username, key));

            // Supporting multi factor auth for private key auth with password
            if (!string.IsNullOrWhiteSpace(this.sftpConfig.Password))
            {
                autheticationMethods.Add(new PasswordAuthenticationMethod(this.sftpConfig.Username, this.sftpConfig.Password));
            }
        }
        else if (!string.IsNullOrWhiteSpace(this.sftpConfig.Password))
        {
            autheticationMethods.Add(new PasswordAuthenticationMethod(this.sftpConfig.Username, this.sftpConfig.Password));
        }
        else
        {
            autheticationMethods.Add(new NoneAuthenticationMethod(this.sftpConfig.Username));
        }

        var connectionInfo = new ConnectionInfo(
            host: this.sftpConfig.Hostname,
            port: this.sftpConfig.Port,
            username: this.sftpConfig.Username,
            authenticationMethods: [.. autheticationMethods]);

        connectionInfo.AuthenticationBanner += this.OnAuthenticationBanner;
        connectionInfo.Timeout = this.sftpConfig.ConnectTimeout;
        connectionInfo.RetryAttempts = this.sftpConfig.ConnectRetryAttempts;

        return connectionInfo;
    }

    private void Client_ErrorOccurred(object sender, ExceptionEventArgs e)
    {
        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpException, e.Exception }
        });

        this.logger.LogWarning("Ssh client exception received");
    }

    private void LogClientInfo(SftpClient client)
    {
        var password = this.sftpConfig?.Password ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(password))
        {
            var length = password.Length / 3;
            password = password[..length];
            password += new string('*', password.Length - length);
        }

        var sb = new StringBuilder();
        sb.AppendLine("-----------------------------------------------");
        sb.AppendLine($"Host: {client.ConnectionInfo.Username}@{client.ConnectionInfo.Host}:{client.ConnectionInfo.Port}");
        sb.AppendLine($"Pass: {password}");

        sb.AppendLine($"{nameof(client.ConnectionInfo.ClientVersion)}: {client.ConnectionInfo.ClientVersion}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.ServerVersion)}: {client.ConnectionInfo.ServerVersion}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.Encoding)}: {client.ConnectionInfo.Encoding?.ToString()}");

        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentHostKeyAlgorithm)}: {client.ConnectionInfo.CurrentHostKeyAlgorithm}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentKeyExchangeAlgorithm)}: {client.ConnectionInfo.CurrentKeyExchangeAlgorithm}");

        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentClientCompressionAlgorithm)}: {client.ConnectionInfo.CurrentClientCompressionAlgorithm}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentServerCompressionAlgorithm)}: {client.ConnectionInfo.CurrentServerCompressionAlgorithm}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentClientEncryption)}: {client.ConnectionInfo.CurrentClientEncryption}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentServerEncryption)}: {client.ConnectionInfo.CurrentServerEncryption}");

        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentClientHmacAlgorithm)}: {client.ConnectionInfo.CurrentClientHmacAlgorithm}");
        sb.AppendLine($"{nameof(client.ConnectionInfo.CurrentServerHmacAlgorithm)}: {client.ConnectionInfo.CurrentServerHmacAlgorithm}");

        sb.AppendLine($"Supported Encryptions: {string.Join(", ", client.ConnectionInfo.Encryptions.Keys)}");
        sb.AppendLine($"Supported HostKey: {string.Join(", ", client.ConnectionInfo.HostKeyAlgorithms.Keys)}");
        sb.AppendLine($"Supported HMAC: {string.Join(", ", client.ConnectionInfo.HmacAlgorithms.Keys)}");

        sb.AppendLine("ConnectTimeout: " + client.ConnectionInfo.Timeout.ToString());
        sb.AppendLine($"{nameof(client.OperationTimeout)}: {client.OperationTimeout}");

        using var logScope = this.logger.BeginScope(new Dictionary<string, object>
        {
            { Monikers.SftpMonikers.SftpAuthInfo, sb.ToString() }
        });

        this.logger.LogInformation("Sftp client info");
    }

    private void OnAuthenticationBanner(object sender, AuthenticationBannerEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e?.BannerMessage))
        {
            using var logScope = this.logger.BeginScope(new Dictionary<string, object>
            {
                { Monikers.SftpMonikers.SftpBannerInfo, e.BannerMessage }
            });

            this.logger.LogInformation("Sftp banner info received");
        }
    }
    /// <inheritdoc/>

    public void AppendAllLines(string path, IEnumerable<string> contents) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void AppendAllLines(string path, IEnumerable<string> contents, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void AppendAllText(string path, string contents) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void AppendAllText(string path, string contents, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public StreamWriter AppendText(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public StreamWriter AppendText(string path, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginDownloadFile(string path, Stream output) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginDownloadFile(string path, Stream output, AsyncCallback asyncCallback) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginDownloadFile(string path, Stream output, AsyncCallback asyncCallback, object state, Action<ulong> downloadCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginListDirectory(string path, AsyncCallback asyncCallback, object state, Action<int> listCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginSynchronizeDirectories(string sourcePath, string destinationPath, string searchPattern, AsyncCallback asyncCallback, object state) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginUploadFile(Stream input, string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginUploadFile(Stream input, string path, AsyncCallback asyncCallback) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginUploadFile(Stream input, string path, AsyncCallback asyncCallback, object state, Action<ulong> uploadCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IAsyncResult BeginUploadFile(Stream input, string path, bool canOverride, AsyncCallback asyncCallback, object state, Action<ulong> uploadCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void ChangeDirectory(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void ChangePermissions(string path, short mode) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileStream Create(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileStream Create(string path, int bufferSize) => throw new NotImplementedException();
    void ISftpClient.CreateDirectory(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public StreamWriter CreateText(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public StreamWriter CreateText(string path, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void Delete(string path) => throw new NotImplementedException();
    void ISftpClient.DeleteDirectory(string path) => throw new NotImplementedException();
    void ISftpClient.DeleteFile(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void DownloadFile(string path, Stream output, Action<ulong> downloadCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void EndDownloadFile(IAsyncResult asyncResult) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IEnumerable<global::Renci.SshNet.Sftp.SftpFile> EndListDirectory(IAsyncResult asyncResult) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IEnumerable<FileInfo> EndSynchronizeDirectories(IAsyncResult asyncResult) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void EndUploadFile(IAsyncResult asyncResult) => throw new NotImplementedException();
    /// <inheritdoc/>
    public bool Exists(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFile Get(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileAttributes GetAttributes(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public DateTime GetLastAccessTime(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public DateTime GetLastAccessTimeUtc(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public DateTime GetLastWriteTime(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public DateTime GetLastWriteTimeUtc(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileSytemInformation GetStatus(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IEnumerable<global::Renci.SshNet.Sftp.SftpFile> ListDirectory(string path, Action<int> listCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileStream Open(string path, FileMode mode) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileStream Open(string path, FileMode mode, FileAccess access) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileStream OpenRead(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public StreamReader OpenText(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public global::Renci.SshNet.Sftp.SftpFileStream OpenWrite(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public byte[] ReadAllBytes(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public string[] ReadAllLines(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public string[] ReadAllLines(string path, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public string ReadAllText(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public string ReadAllText(string path, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IEnumerable<string> ReadLines(string path) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IEnumerable<string> ReadLines(string path, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void RenameFile(string oldPath, string newPath) => throw new NotImplementedException();
    void ISftpClient.RenameFile(string oldPath, string newPath, bool isPosix) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void SetAttributes(string path, global::Renci.SshNet.Sftp.SftpFileAttributes fileAttributes) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void SymbolicLink(string path, string linkPath) => throw new NotImplementedException();
    /// <inheritdoc/>
    public IEnumerable<FileInfo> SynchronizeDirectories(string sourcePath, string destinationPath, string searchPattern) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void UploadFile(Stream input, string path, Action<ulong> uploadCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void UploadFile(Stream input, string path, bool canOverride, Action<ulong> uploadCallback = null) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllBytes(string path, byte[] bytes) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllLines(string path, IEnumerable<string> contents) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllLines(string path, IEnumerable<string> contents, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllLines(string path, string[] contents) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllLines(string path, string[] contents, Encoding encoding) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllText(string path, string contents) => throw new NotImplementedException();
    /// <inheritdoc/>
    public void WriteAllText(string path, string contents, Encoding encoding) => throw new NotImplementedException();
}

