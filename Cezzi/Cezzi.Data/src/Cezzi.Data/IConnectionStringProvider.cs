namespace Cezzi.Data;

/// <summary>
/// 
/// </summary>
public interface IConnectionStringProvider
{
    /// <summary>Gets the <see cref="string"/> with the specified name.</summary>
    /// <value>The <see cref="string"/>.</value>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    string this[string name] { get; }

    /// <summary>Adds the connection string.</summary>
    /// <param name="name">The name.</param>
    /// <param name="connection">The connection.</param>
    /// <returns></returns>
    IConnectionStringProvider AddConnectionString(string name, string connection);
}
