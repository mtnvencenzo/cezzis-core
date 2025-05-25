namespace Cezzi.Azure.Storage.Table;

using global::Azure;
using global::Azure.Data.Tables;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Azure.Storage.Table.ITableClientProxy" />
public class TableClientProxy : ITableClientProxy
{
    /// <summary>Gets the entity asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableClient">The table client.</param>
    /// <param name="partitionKey">The partition key.</param>
    /// <param name="rowKey">The row key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<Response<T>> GetEntityAsync<T>(
        TableClient tableClient,
        string partitionKey,
        string rowKey,
        CancellationToken cancellationToken = default) where T : class, ITableEntity, new() => await tableClient.GetEntityAsync<T>(
            partitionKey: partitionKey,
            rowKey: rowKey,
            cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Updates the entity asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableClient">The table client.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="ifMatch">If match.</param>
    /// <param name="mode">The mode.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<Response> UpdateEntityAsync<T>(
        TableClient tableClient,
        T entity,
        ETag ifMatch,
        TableUpdateMode mode,
        CancellationToken cancellationToken = default) where T : ITableEntity => await tableClient.UpdateEntityAsync(
            entity: entity,
            ifMatch: ifMatch,
            mode: mode,
            cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Adds the entity asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableClient">The table client.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<Response> AddEntityAsync<T>(
        TableClient tableClient,
        T entity,
        CancellationToken cancellationToken = default) where T : ITableEntity => await tableClient.AddEntityAsync(
            entity: entity,
            cancellationToken: cancellationToken).ConfigureAwait(false);
}
