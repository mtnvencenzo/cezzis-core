namespace Cezzi.Sftp;

using System.Collections.Generic;
using System.IO;

/// <summary> Sftp client interface </summary>
public interface ISftpClient
{
    /// <summary> creates directory if not exists </summary>
    /// <param name="nameofDirectory"></param>
    /// <returns>returns true if directory gets created; else false</returns>
    bool CreateDirectory(string nameofDirectory);

    /// <summary> deletes directory if not exists </summary>
    /// <param name="nameofDirectory"></param>
    bool DeleteDirectory(string nameofDirectory);

    /// <summary>Uploads the file.</summary>
    /// <param name="contents">The contents.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <param name="renameAfterUploadToPath">The rename after upload to path.</param>
    /// <returns></returns>
    bool UploadFileFromBytes(
        byte[] contents,
        string remoteFilePath,
        bool overwriteIfExists = true,
        string renameAfterUploadToPath = null);

    /// <summary>Uploads the file from stream.</summary>
    /// <param name="stream">The stream.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <param name="renameAfterUploadToPath">The rename after upload to path.</param>
    /// <returns></returns>
    bool UploadFileFromStream(
        Stream stream,
        string remoteFilePath,
        bool overwriteIfExists = true,
        string renameAfterUploadToPath = null);

    /// <summary>Uploads the files.</summary>
    /// <param name="contents">The contents.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <returns></returns>
    bool UploadFilesFromBytes(IList<byte[]> contents, string remoteFilePath, bool overwriteIfExists = true);

    /// <summary>Uploads the files from streams.</summary>
    /// <param name="streams">The streams.</param>
    /// <param name="remoteFilePath">The remote file path.</param>
    /// <param name="overwriteIfExists">if set to <c>true</c> [overwrite if exists].</param>
    /// <returns></returns>
    bool UploadFilesFromStreams(IList<Stream> streams, string remoteFilePath, bool overwriteIfExists = true);

    /// <summary> Deletes the file from the server.</summary>
    /// <param name="fileName">The file name.</param>
    /// <returns>returns true if file is deleted successfully; else false</returns>
    bool DeleteFile(string fileName);

    /// <summary>Lists the directory.</summary>
    /// <param name="path">The path.</param>
    /// <param name="options">The options.</param>
    /// <returns></returns>
    IList<SftpFile> ListDirectory(string path, SftpFileListOptions options);

    /// <summary>Renames the file.</summary>
    /// <param name="oldFileName">The old file name.</param>
    /// <param name="newFileName">The new file name</param>
    /// <param name="isPosix">if set to <c>true</c> [is posix].</param>
    /// <returns>Whether or not the file was found and renamed on the remote server</returns>
    bool RenameFile(string oldFileName, string newFileName, bool isPosix = false);

    /// <summary> Gets file.</summary>
    /// <param name="filePathAndName">The file path.</param>
    /// <returns> The byte array</returns>
    byte[] GetFile(string filePathAndName);

    /// <summary>Gets the file text.</summary>
    /// <param name="filePathAndName">Name of the file path and.</param>
    /// <returns></returns>
    string GetFileText(string filePathAndName);
}
