namespace Cezzi.Azure.Storage.Blob;

using global::Azure.Storage.Blobs;
using global::Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Azure.Storage.Blob.IBlobClientProxy" />
public class BlobClientProxy : IBlobClientProxy
{
    /// <summary>Uploads the BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB client.</param>
    /// <param name="blobName">The BLOB name.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public async Task<BlobContentInfo> UploadBlob(
        BlobContainerClient blobContainerClient,
        string blobName,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        return await blobContainerClient.UploadBlobAsync(
            content: stream,
            blobName: blobName,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Tries the get blobs.</summary>
    /// <param name="blobContainerClient">The BLOB client.</param>
    /// <param name="includeDeleted">if set to <c>true</c> [include deleted].</param>
    /// <param name="onlyLatestVersion">if set to <c>true</c> [only latest version].</param>
    /// <param name="prefix">The prefix.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<IList<BlobItem>> GetBlobs(
        BlobContainerClient blobContainerClient,
        bool includeDeleted = false,
        bool onlyLatestVersion = true,
        string prefix = null,
        CancellationToken cancellationToken = default)
    {
        var response = blobContainerClient.GetBlobsAsync(
            traits: BlobTraits.All,
            states: BlobStates.None,
            prefix: prefix,
            cancellationToken: cancellationToken);

        var enumerator = response.GetAsyncEnumerator(cancellationToken);

        var blobItems = new List<BlobItem>();

        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
        {
            blobItems.Add(enumerator.Current);
        }

        return [.. blobItems
            .Where(x => includeDeleted || !x.Deleted)
            .Where(x => !onlyLatestVersion || (x.IsLatestVersion ?? true))
            .OrderBy(x => x.Properties?.CreatedOn ?? DateTimeOffset.MinValue)];
    }

    /// <summary>Tries the content of the get BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<string> GetBlobContent(
        BlobContainerClient blobContainerClient,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        var content = await blobClient.DownloadContentAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        return content?.Value?.Content?.ToString();
    }

    /// <summary>Deletes the BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task DeleteBlob(
        BlobContainerClient blobContainerClient,
        string blobName,
        CancellationToken cancellationToken = default) => await blobContainerClient.DeleteBlobIfExistsAsync(blobName: blobName, cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Renames the BLOB.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="newName">The new name.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task RenameBlob(
        BlobContainerClient blobContainerClient,
        string blobName,
        string newName,
        CancellationToken cancellationToken = default)
    {
        var sourceBlob = blobContainerClient.GetBlobClient(blobName);
        var destBlob = blobContainerClient.GetBlobClient(newName);

        var copy = await destBlob.StartCopyFromUriAsync(sourceBlob.Uri, cancellationToken: cancellationToken).ConfigureAwait(false);
        await copy.WaitForCompletionAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        await sourceBlob.DeleteAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Checks if a blob exists.</summary>
    /// <param name="blobContainerClient">The BLOB container client.</param>
    /// <param name="blobName">Name of the BLOB.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<bool> BlobExists(
        BlobContainerClient blobContainerClient,
        string blobName,
        CancellationToken cancellationToken = default)
    {
        var blobClient = blobContainerClient.GetBlobClient(blobName);

        return await blobClient.ExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
