# Cezzi.Security.Pgp
General pgp encryption and decryption wrapper around the [BouncyCastle.NetCore](https://www.nuget.org/packages/BouncyCastle.NetCore) nuget package.

<br/>

# Encryption and Decryption
The below example demonstrates using the `PgpService` to encrypt and decrypt a string.

> The usage of the passphrase is optional and depends on whether or not the key pair waas generated with one.

<br/>

``` csharp
namespace PgpStuff
{
    using Cezzi.Security.Pgp;
    using System;
    using System.IO;
    using System.Text;

    public class PgpServiceTests
    {
        private const string PgpPrivateKeyPassphrase = "Iifo2#Something5GzzpeKijE!";

        public void EncryptAndDecrypt()
        {
            var encryptedData = new PgpService().Encrypt(
                inputData: Encoding.ASCII.GetBytes("test-data"),
                publicKey: Encoding.ASCII.GetBytes("-----BEGIN PGP PUBLIC KEY BLOCK-----...etc...-----END PGP PUBLIC KEY BLOCK-----"));

            var passphrase = "Iifo2#Something5GzzpeKijE!";

            var privateKeyBytes = Encoding.ASCII.GetBytes("-----BEGIN PGP PRIVATE KEY BLOCK-----...etc...-----END PGP PRIVATE KEY BLOCK-----");
            var privateKey = new MemoryStream(privateKeyBytes) { Position = 0 };
            privateKey.Seek(0, SeekOrigin.Begin);

            var resultBytes = new PgpService().Decrypt(encryptedData, privateKey, passphrase);
        }
    }
}
```

<br/>

![nuspec](../../../.readme/encryption.jpg)

