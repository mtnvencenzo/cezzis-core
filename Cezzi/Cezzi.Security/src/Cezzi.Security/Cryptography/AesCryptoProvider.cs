namespace Cezzi.Security.Cryptography;

using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 
/// </summary>
public class AesCryptoProvider : IAesCryptoProvider
{
    private readonly ICryptoKey key;
    private readonly bool usePbkdf2;

    /// <summary>Initializes a new instance of the <see cref="AesCryptoProvider"/> class.</summary>
    /// <param name="key">The key.</param>
    /// <param name="usePbkdf2">if set to <c>true</c> [use PBKDF2].</param>
    /// <exception cref="ArgumentNullException">key</exception>
    public AesCryptoProvider(ICryptoKey key, bool usePbkdf2 = false)
    {
        this.key = key ?? throw new ArgumentNullException(nameof(key));
        this.usePbkdf2 = usePbkdf2;

        if (string.IsNullOrWhiteSpace(key.Key))
        {
            throw new ArgumentNullException(nameof(key), "invalid key");
        }

        if (string.IsNullOrWhiteSpace(key.Vector))
        {
            throw new ArgumentNullException(nameof(key), "invalid vector");
        }
    }

    /// <summary>Encrypts the specified to encrypt.</summary>
    /// <param name="toEncrypt">To encrypt.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// key
    /// or
    /// iv
    /// or
    /// toEncrypt
    /// </exception>
    public virtual string Encrypt(string toEncrypt)
    {
        if (string.IsNullOrWhiteSpace(toEncrypt))
        {
            throw new ArgumentNullException(nameof(toEncrypt));
        }

        using var algorithm = this.GetAes(this.key.Key, this.key.Vector);
        var rawData = Encoding.UTF8.GetBytes(toEncrypt);

        using var memoryStream = new MemoryStream();
        using var encryptor = algorithm.CreateEncryptor();
        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        cryptoStream.Write(rawData, 0, rawData.Length);
        cryptoStream.FlushFinalBlock();
        var cipherTextBytes = memoryStream.ToArray();
        return Convert.ToBase64String(cipherTextBytes);
    }

    /// <summary>Decrypts the specified cipher text.</summary>
    /// <param name="cipherText">The cipher text.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// key
    /// or
    /// iv
    /// or
    /// cipherText
    /// </exception>
    public virtual string Decrypt(string cipherText)
    {
        if (string.IsNullOrWhiteSpace(cipherText))
        {
            throw new ArgumentNullException(nameof(cipherText));
        }

        using var algorithm = this.GetAes(this.key.Key, this.key.Vector);
        var rawData = Convert.FromBase64String(cipherText);

        using var msDecrypt = new MemoryStream(rawData);
        using var decryptor = algorithm.CreateDecryptor();
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        return srDecrypt.ReadToEnd();
    }

    private System.Security.Cryptography.Aes GetAes(string keyString, string ivString)
    {
        var algorithm = Aes.Create();

        algorithm.Mode = CipherMode.CBC;
        algorithm.Padding = PaddingMode.PKCS7;
        algorithm.KeySize = 256;

        var ivSizeInBytes = algorithm.BlockSize / 8;
        var keySizeInBytes = algorithm.KeySize / 8;

        var iv = Encoding.UTF8.GetBytes(ivString).Take(ivSizeInBytes).ToArray();
        var key = Encoding.UTF8.GetBytes(keyString);
        algorithm.IV = iv;

        if (this.usePbkdf2)
        {
            algorithm.Key = KeyDerivation.Pbkdf2(
                password: keyString,
                salt: iv,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
        }
        else
        {
            algorithm.Key = [.. Encoding.UTF8.GetBytes(keyString).Take(keySizeInBytes)];
        }

        return algorithm;
    }
}
