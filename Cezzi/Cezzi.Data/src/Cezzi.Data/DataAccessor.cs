namespace Cezzi.Data;

using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Data.IDataAccessor" />
public class DataAccessor : IDataAccessor
{
    /// <summary>Opens the asynchronous.</summary>
    /// <param name="conn">The connection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task OpenAsync(DbConnection conn,
        CancellationToken cancellationToken = default) => await conn.OpenAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Opens the specified connection.</summary>
    /// <param name="conn">The connection.</param>
    public void Open(DbConnection conn) => conn.Open();

    /// <summary>Executes the reader asynchronous.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="commandBehavior">The command behavior.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<IDataReader> ExecuteReaderAsync(
        DbCommand cmd,
        CommandBehavior commandBehavior = CommandBehavior.CloseConnection,
        CancellationToken cancellationToken = default) => await cmd.ExecuteReaderAsync(behavior: commandBehavior, cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Executes the reader.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="commandBehavior">The command behavior.</param>
    /// <returns></returns>
    public IDataReader ExecuteReader(
        DbCommand cmd,
        CommandBehavior commandBehavior = CommandBehavior.CloseConnection) => cmd.ExecuteReader(behavior: commandBehavior);

    /// <summary>Executes the non query asynchronous.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<int> ExecuteNonQueryAsync(
        DbCommand cmd,
        CancellationToken cancellationToken = default) => await cmd.ExecuteNonQueryAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Executes the non query.</summary>
    /// <param name="cmd">The command.</param>
    /// <returns></returns>
    public int ExecuteNonQuery(DbCommand cmd) => cmd.ExecuteNonQuery();

    /// <summary>Executes the scalar asynchronous.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<object> ExecuteScalarAsync(
        DbCommand cmd,
        CancellationToken cancellationToken = default) => await cmd.ExecuteScalarAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

    /// <summary>Executes the scalar.</summary>
    /// <param name="cmd">The command.</param>
    /// <returns></returns>
    public object ExecuteScalar(DbCommand cmd) => cmd.ExecuteScalar();

    /// <summary>Fills the specified adapter.</summary>
    /// <param name="adapter">The adapter.</param>
    /// <param name="dataset">The dataset.</param>
    /// <returns></returns>
    public int Fill(IDataAdapter adapter, DataSet dataset) => adapter.Fill(dataset);
}
