namespace Cezzi.Security.Pgp;

using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Security;
using System;
using System.IO;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Security.Pgp.IPgpService" />
public class PgpService : IPgpService
{
    /// <summary>The buffer size
    /// </summary>
    protected const int BufferSize = 512;

    /// <summary>Decrypt the byte array passed into inputData and return it as another byte array.</summary>
    /// <param name="inputData">The data to decrypt</param>
    /// <param name="keyIn">A stream from your private keyring.</param>
    /// <param name="passphrase">The password for the private key.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">secret key for message not found.</exception>
    /// <exception cref="Org.BouncyCastle.Bcpg.OpenPgp.PgpException">encrypted message contains a signed message - not literal data.
    /// or
    /// message is not a simple encrypted file - type unknown.</exception>
    public virtual byte[] Decrypt(byte[] inputData, Stream keyIn, string passphrase)
    {
        if (inputData == null)
        {
            throw new ArgumentNullException(nameof(inputData));
        }

        if (keyIn == null)
        {
            throw new ArgumentNullException(nameof(keyIn));
        }

        if (passphrase == null)
        {
            throw new ArgumentNullException(nameof(passphrase));
        }

        using var bufferStream = new MemoryStream(inputData);
        using var inputStream = PgpUtilities.GetDecoderStream(bufferStream);
        using var decoded = new MemoryStream();

        var pgpF = new PgpObjectFactory(inputStream);
        PgpEncryptedDataList enc;
        var o = pgpF.NextPgpObject();

        // the first object might be a PGP marker packet.
        enc = o is PgpEncryptedDataList list ? list : (PgpEncryptedDataList)pgpF.NextPgpObject();

        // find the secret key
        PgpPrivateKey sKey = null;
        PgpPublicKeyEncryptedData pbe = null;
        var pgpSec = new PgpSecretKeyRingBundle(PgpUtilities.GetDecoderStream(keyIn));

        foreach (PgpPublicKeyEncryptedData pked in enc.GetEncryptedDataObjects())
        {
            sKey = this.FindSecretKey(pgpSec, pked.KeyId, passphrase.ToCharArray());
            if (sKey != null)
            {
                pbe = pked;
                break;
            }
        }

        if (sKey == null)
        {
            throw new ArgumentException("secret key for message not found.");
        }

        using var clear = pbe.GetDataStream(sKey);
        var plainFact = new PgpObjectFactory(clear);
        var message = plainFact.NextPgpObject();

        if (message is PgpCompressedData cData)
        {
            var pgpFact = new PgpObjectFactory(cData.GetDataStream());
            message = pgpFact.NextPgpObject();
        }

        if (message is PgpLiteralData ld)
        {
            var unc = ld.GetInputStream();
            this.PipeAll(unc, decoded);
        }
        else if (message is PgpOnePassSignatureList)
        {
            throw new PgpException("encrypted message contains a signed message - not literal data.");
        }
        else
        {
            throw new PgpException("message is not a simple encrypted file - type unknown.");
        }

        if (pbe.IsIntegrityProtected())
        {
            if (!pbe.Verify())
            {
                throw new Exception("Message failed integrity check.");
            }
        }

        return decoded.ToArray();
    }

    /// <summary>Encrypt the data.</summary>
    /// <param name="inputData">The byte array to encrypt</param>
    /// <param name="passphrase">The password returned by "ReadPublicKey".</param>
    /// <param name="withIntegrityCheck">Check the data for errors.</param>
    /// <param name="armor">Protect the data streams.</param>
    /// <returns>Encrypted byte array</returns>
    public virtual byte[] Encrypt(byte[] inputData, PgpPublicKey passphrase, bool withIntegrityCheck, bool armor)
    {
        var processedData = this.Compress(inputData, PgpLiteralData.Console, CompressionAlgorithmTag.Uncompressed);

        using var bOut = new MemoryStream();
        Stream output = bOut;

        if (armor)
        {
            output = new ArmoredOutputStream(output);
        }

        var encGen = new PgpEncryptedDataGenerator(SymmetricKeyAlgorithmTag.Cast5, withIntegrityCheck, new SecureRandom());
        encGen.AddMethod(passphrase);

        using (var encOut = encGen.Open(output, processedData.Length))
        {
            encOut.Write(processedData, 0, processedData.Length);
        }

        if (armor)
        {
            output.Close();
        }

        return bOut.ToArray();
    }

    /// <summary>Encrypt the data.</summary>
    /// <param name="inputData">The byte array to encrypt.</param>
    /// <param name="publicKey">The public key.</param>
    /// <returns></returns>
    public virtual byte[] Encrypt(byte[] inputData, byte[] publicKey)
    {
        using var publicKeyStream = new MemoryStream(publicKey);
        var encKey = this.ReadPublicKey(publicKeyStream);

        return this.Encrypt(inputData, encKey, true, true);
    }

    /// <summary>Reads the public key.</summary>
    /// <param name="inputStream">The input stream.</param>
    /// <param name="publicKeyResolver">The public key resolver.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">Can't find encryption key in key ring.</exception>
    protected virtual PgpPublicKey ReadPublicKey(Stream inputStream, Func<PgpPublicKey, bool> publicKeyResolver = null)
    {
        inputStream = PgpUtilities.GetDecoderStream(inputStream);
        var pgpPub = new PgpPublicKeyRingBundle(inputStream);

        // iterate through the key rings.
        foreach (var kRing in pgpPub.GetKeyRings())
        {
            foreach (var k in kRing.GetPublicKeys())
            {
                if (publicKeyResolver != null)
                {
                    if (publicKeyResolver(k))
                    {
                        return k;
                    }
                }
                else if (k.IsEncryptionKey)
                {
                    return k;
                }
            }
        }

        throw new ArgumentException("Can't find encryption key in key ring.");
    }

    /// <summary>Finds the secret key.</summary>
    /// <param name="pgpSec">The PGP sec.</param>
    /// <param name="keyId">The key identifier.</param>
    /// <param name="passphrase">The passphrase.</param>
    /// <returns></returns>
    protected virtual PgpPrivateKey FindSecretKey(PgpSecretKeyRingBundle pgpSec, long keyId, char[] passphrase)
    {
        var pgpSecKey = pgpSec.GetSecretKey(keyId);

        return pgpSecKey?.ExtractPrivateKey(passphrase);
    }

    /// <summary>Compresses the specified clear data.</summary>
    /// <param name="clearData">The clear data.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="algorithm">The algorithm.</param>
    /// <returns></returns>
    protected virtual byte[] Compress(byte[] clearData, string fileName, CompressionAlgorithmTag algorithm)
    {
        var comData = new PgpCompressedDataGenerator(algorithm);

        using var bOut = new MemoryStream();
        using var cos = comData.Open(bOut);

        var lData = new PgpLiteralDataGenerator();

        // we want to Generate compressed data. This might be a user option later,
        // in which case we would pass in bOut.
        using var pOut = lData.Open(
            outStr: cos,
            format: PgpLiteralData.Binary,
            name: fileName,
            length: clearData.Length,
            modificationTime: DateTime.UtcNow
        );

        pOut.Write(clearData, 0, clearData.Length);
        pOut.Close();
        return bOut.ToArray();
    }

    /// <summary>Pipes all.</summary>
    /// <param name="inStr">The in string.</param>
    /// <param name="outStr">The out string.</param>
    protected virtual void PipeAll(Stream inStr, Stream outStr)
    {
        var bs = new byte[BufferSize];
        int numRead;

        while ((numRead = inStr.Read(bs, 0, bs.Length)) > 0)
        {
            outStr.Write(bs, 0, numRead);
        }
    }
}
