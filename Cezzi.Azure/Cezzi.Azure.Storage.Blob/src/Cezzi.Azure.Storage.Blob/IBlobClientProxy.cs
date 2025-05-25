namespace Cezzi.Azure.Storage.Blob;

using global::Azure.Storage.Blobs;
using global::Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IBlobClientProxy
{
    /// <summary>Uploads the BLOB.</summary>
    /// <param name="blobClient">The BLOB client.</param>
    /// <param name="blobName">The BLOB name.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    Task<BlobContentInfo> UploadBlob(
        BlobContainerClient blobClient,
        string blobName,
        Stream stream,
        CancellationToken cancellationToken = default);

    /// <summary>Tries the get blobs.</summary>
    /// <param name="blobContainerClient">The BLOB client.</param>
    /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
    /// <param name="onlyLatestVersion">if set to <c>true</c> [only latest version].</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<IList<BlobItem>> GetBlobs(
        BlobContainerClient blobContainerClient,
        bool includeDeleted = false,
        bool onlyLatestVersion = true,
        string prefix = null,
        CancellationToken cancellationToken = default);

    /// <summary>Tries the content of the get BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<string> GetBlobContent(
        BlobContainerClient blobContainerClient,
        string blobName,
        CancellationToken cancellationToken = default);

    /// <summary>Deletes the BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task DeleteBlob(
        BlobContainerClient blobContainerClient,
        string blobName,
        CancellationToken cancellationToken = default);

    /// <summary>Renames the BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="newName">The new name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task RenameBlob(
        BlobContainerClient blobContainerClient,
        string blobName,
        string newName,
        CancellationToken cancellationToken = default);

    /// <summary>Checks if a blob exists.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<bool> BlobExists(
        BlobContainerClient blobContainerClient,
        string blobName,
        CancellationToken cancellationToken = default);
}
