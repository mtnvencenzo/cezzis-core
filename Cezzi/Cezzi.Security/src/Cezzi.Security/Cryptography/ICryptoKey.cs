namespace Cezzi.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public interface ICryptoKey
{
    /// <summary>Gets the key.</summary>
    /// <value>The key.</value>
    string Key { get; }

    /// <summary>Gets the vector.</summary>
    /// <value>The vector.</value>
    string Vector { get; }
}
