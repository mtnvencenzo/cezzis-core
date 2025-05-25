namespace Cezzi.Data;

using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IDataAccessor
{
    /// <summary>Opens the asynchronous.</summary>
    /// <param name="conn">The connection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task OpenAsync(DbConnection conn, CancellationToken cancellationToken = default);

    /// <summary>Opens the specified connection.</summary>
    /// <param name="conn">The connection.</param>
    void Open(DbConnection conn);

    /// <summary>Executes the reader asynchronous.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="commandBehavior">The command behavior.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<IDataReader> ExecuteReaderAsync(
        DbCommand cmd,
        CommandBehavior commandBehavior = CommandBehavior.CloseConnection,
        CancellationToken cancellationToken = default);

    /// <summary>Executes the reader.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="commandBehavior">The command behavior.</param>
    /// <returns></returns>
    IDataReader ExecuteReader(
        DbCommand cmd,
        CommandBehavior commandBehavior = CommandBehavior.CloseConnection);

    /// <summary>Executes the non query asynchronous.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<int> ExecuteNonQueryAsync(DbCommand cmd, CancellationToken cancellationToken = default);

    /// <summary>Executes the non query.</summary>
    /// <param name="cmd">The command.</param>
    /// <returns></returns>
    int ExecuteNonQuery(DbCommand cmd);

    /// <summary>Executes the scalar asynchronous.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<object> ExecuteScalarAsync(DbCommand cmd, CancellationToken cancellationToken = default);

    /// <summary>Executes the scalar.</summary>
    /// <param name="cmd">The command.</param>
    /// <returns></returns>
    object ExecuteScalar(DbCommand cmd);

    /// <summary>Fills the specified adapter.</summary>
    /// <param name="adapter">The adapter.</param>
    /// <param name="dataset">The dataset.</param>
    /// <returns></returns>
    int Fill(IDataAdapter adapter, DataSet dataset);
}
