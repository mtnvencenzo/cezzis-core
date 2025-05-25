namespace Cezzi.Security.Cryptography;

/// <summary>
/// 
/// </summary>
public interface ICryptoProvider
{
    /// <summary>Encrypts the specified to encrypt.</summary>
    /// <param name="toEncrypt">To encrypt.</param>
    /// <returns></returns>
    string Encrypt(string toEncrypt);

    /// <summary>Decrypts the specified cipher text.</summary>
    /// <param name="cipherText">The cipher text.</param>
    /// <returns></returns>
    string Decrypt(string cipherText);
}
