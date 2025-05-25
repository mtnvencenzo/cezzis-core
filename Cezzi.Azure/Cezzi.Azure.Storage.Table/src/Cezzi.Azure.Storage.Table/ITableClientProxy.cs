namespace Cezzi.Azure.Storage.Table;

using global::Azure;
using global::Azure.Data.Tables;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface ITableClientProxy
{
    /// <summary>Gets the entity asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableClient">The table client.</param>
    /// <param name="partitionKey">The partition key.</param>
    /// <param name="rowKey">The row key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<Response<T>> GetEntityAsync<T>(
        TableClient tableClient,
        string partitionKey,
        string rowKey,
        CancellationToken cancellationToken = default) where T : class, ITableEntity, new();

    /// <summary>Updates the entity asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableClient">The table client.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="ifMatch">If match.</param>
    /// <param name="mode">The mode.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<Response> UpdateEntityAsync<T>(
        TableClient tableClient,
        T entity,
        ETag ifMatch,
        TableUpdateMode mode,
        CancellationToken cancellationToken = default) where T : ITableEntity;

    /// <summary>Adds the entity asynchronous.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="tableClient">The table client.</param>
    /// <param name="entity">The entity.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<Response> AddEntityAsync<T>(
        TableClient tableClient,
        T entity,
        CancellationToken cancellationToken = default) where T : ITableEntity;
}
