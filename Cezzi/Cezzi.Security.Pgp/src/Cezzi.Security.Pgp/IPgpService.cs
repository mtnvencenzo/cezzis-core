namespace Cezzi.Security.Pgp;

using Org.BouncyCastle.Bcpg.OpenPgp;
using System.IO;

/// <summary>
/// 
/// </summary>
public interface IPgpService
{
    /// <summary>Decrypt the byte array passed into inputData and return it as another byte array.</summary>
    /// <param name="inputData">The data to decrypt</param>
    /// <param name="keyIn">A stream from your private keyring.</param>
    /// <param name="passphrase">The password for the private key.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">secret key for message not found.</exception>
    /// <exception cref="Org.BouncyCastle.Bcpg.OpenPgp.PgpException">encrypted message contains a signed message - not literal data.
    /// or
    /// message is not a simple encrypted file - type unknown.</exception>
    byte[] Decrypt(byte[] inputData, Stream keyIn, string passphrase);

    /// <summary>Encrypt the data.</summary>
    /// <param name="inputData">The byte array to encrypt</param>
    /// <param name="passphrase">The password returned by "ReadPublicKey".</param>
    /// <param name="withIntegrityCheck">Check the data for errors.</param>
    /// <param name="armor">Protect the data streams.</param>
    /// <returns>Encrypted byte array</returns>
    byte[] Encrypt(byte[] inputData, PgpPublicKey passphrase, bool withIntegrityCheck, bool armor);

    /// <summary>Encrypt the data.</summary>
    /// <param name="inputData">The byte array to encrypt.</param>
    /// <param name="publicKey">The public key.</param>
    /// <returns></returns>
    byte[] Encrypt(byte[] inputData, byte[] publicKey);
}
