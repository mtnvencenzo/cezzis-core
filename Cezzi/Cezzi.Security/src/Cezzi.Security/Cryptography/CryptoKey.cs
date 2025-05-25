namespace Cezzi.Security.Cryptography;

using System;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Security.Cryptography.ICryptoKey" />
public class CryptoKey : ICryptoKey
{
    /// <summary>Initializes a new instance of the <see cref="CryptoKey"/> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="vector">The vector.</param>
    /// <exception cref="ArgumentNullException">
    /// key
    /// or
    /// vector
    /// </exception>
    public CryptoKey(string key, string vector)
    {
        this.Key = key ?? throw new ArgumentNullException(nameof(key));
        this.Vector = vector ?? throw new ArgumentNullException(nameof(vector));

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new ArgumentNullException(nameof(key));
        }

        if (vector == null)
        {
            throw new ArgumentNullException(nameof(vector));
        }
    }

    /// <summary>Gets the key.</summary>
    /// <value>The key.</value>
    public string Key { get; private set; }

    /// <summary>Gets the vector.</summary>
    /// <value>The vector.</value>
    public string Vector { get; private set; }
}
