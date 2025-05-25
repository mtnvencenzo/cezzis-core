namespace Cezzi.Security.Pgp.Tests;

using Cezzi.Security.Pgp;
using FluentAssertions;
using System;
using System.IO;
using System.Text;
using Xunit;

public class PgpServiceTests
{
    private const string PgpPrivateKeyPassphrase = "cezzi!pezzi";

    [Fact]
    public void pgpservice___decrypt_throws_on_null_input_data()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new PgpService().Decrypt(null, null, null));

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("inputData");
    }

    [Fact]
    public void pgpservice___decrypt_throws_on_null_private_key_stream()
    {
        var encryptedData = GetEncryptedData();

        var ex = Assert.Throws<ArgumentNullException>(() => new PgpService().Decrypt(encryptedData, null, null));

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("keyIn");
    }

    [Fact]
    public void pgpservice___decrypt_throws_on_null_passphrase()
    {
        var encryptedData = GetEncryptedData();
        var privateKey = GetPrivateKeyStream();

        var ex = Assert.Throws<ArgumentNullException>(() => new PgpService().Decrypt(encryptedData, privateKey, null));

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("passphrase");
    }

    [Fact]
    public void pgpservice___decrypt_succeeds()
    {
        var encryptedData = GetEncryptedData();
        var privateKey = GetPrivateKeyStream();

        var resultBytes = new PgpService().Decrypt(encryptedData, privateKey, PgpPrivateKeyPassphrase);

        var decrypted = Encoding.ASCII.GetString(resultBytes);
        decrypted.Should().Be(TestResources.test_file);
    }

    [Fact]
    public void pgpservice___encrypt_succeeds()
    {
        var encryptedData = GetEncryptedData();

        encryptedData.Should().NotBeNull();
        encryptedData.Length.Should().BePositive();
    }

    private static byte[] GetEncryptedData()
    {
        var data = new PgpService().Encrypt(
            inputData: Encoding.ASCII.GetBytes(TestResources.test_file),
            publicKey: Encoding.ASCII.GetBytes(TestResources.public_pgp_key));

        return data;
    }

    private static Stream GetPrivateKeyStream()
    {
        var bytes = Encoding.ASCII.GetBytes(TestResources.private_pgp_key);

        var stream = new MemoryStream(bytes)
        {
            Position = 0
        };
        stream.Seek(0, SeekOrigin.Begin);
        return stream;

    }
}
