namespace Cezzi.Data;

using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Data.IConnectionStringProvider" />
public class DefaultConnectionStringProvider : IConnectionStringProvider
{
    /// <summary>Initializes a new instance of the <see cref="DefaultConnectionStringProvider"/> class.</summary>
    public DefaultConnectionStringProvider()
    {
        this._connectionStrings = [];
    }

    private readonly Dictionary<string, string> _connectionStrings;

    /// <summary>Gets the <see cref="string"/> with the specified name.</summary>
    /// <value>The <see cref="string"/>.</value>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name</exception>
    /// <exception cref="IndexOutOfRangeException">Connect string '{name}</exception>
    public string this[string name] => name == null
                ? throw new ArgumentNullException(nameof(name))
                : this._connectionStrings.ContainsKey(name.ToLower())
                ? this._connectionStrings[name.ToLower()]
                : throw new IndexOutOfRangeException($"Connect string '{name}' has not been defined.");

    /// <summary>Adds the connection string.</summary>
    /// <param name="name">The name.</param>
    /// <param name="connection">The connection.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">name</exception>
    public IConnectionStringProvider AddConnectionString(string name, string connection)
    {
        if (name == null)
        {
            throw new ArgumentNullException(nameof(name));
        }

        if (connection == null)
        {
            throw new ArgumentNullException(nameof(connection));
        }

        if (!this._connectionStrings.ContainsKey(name.ToLower()))
        {
            this._connectionStrings.Add(name.ToLower(), connection);
        }
        else
        {
            this._connectionStrings[name.ToLower()] = connection;
        }

        return this;
    }
}
