namespace Cezzi.Security.Tests;

using Cezzi.Security;
using FluentAssertions;
using System.Text;
using Xunit;

public class HashingTests
{
    [Fact]
    public void hashing___hmac_sha512()
    {
        var plaintext = "text to hash";
        var privateKey = "key";

        var hashed1 = Hashing.GenerateHMACSHA512(
            privateKey: privateKey,
            toSign: plaintext);

        var hashed2 = Hashing.GenerateHMACSHA512(
            privateKey: privateKey,
            toSign: Encoding.UTF8.GetBytes(plaintext));

        hashed1.Should().NotBeNull();
        hashed1.Should().NotBeEmpty();

        hashed2.Should().NotBeNull();
        hashed2.Should().NotBeEmpty();

        hashed1.Should().BeEquivalentTo(hashed2);

        var hashed1String = Encoding.UTF8.GetString(hashed1);
        var hashed2String = Encoding.UTF8.GetString(hashed2);

        hashed1String.Should().NotBeNullOrWhiteSpace();
        hashed1String.Should().Be(hashed2String);
    }

    [Fact]
    public void hashing___hmac_sha384()
    {
        var plaintext = "text to hash";
        var privateKey = "key";

        var hashed1 = Hashing.GenerateHMACSHA384(
            privateKey: privateKey,
            toSign: plaintext);

        var hashed2 = Hashing.GenerateHMACSHA384(
            privateKey: privateKey,
            toSign: Encoding.UTF8.GetBytes(plaintext));

        hashed1.Should().NotBeNull();
        hashed1.Should().NotBeEmpty();

        hashed2.Should().NotBeNull();
        hashed2.Should().NotBeEmpty();

        hashed1.Should().BeEquivalentTo(hashed2);

        var hashed1String = Encoding.UTF8.GetString(hashed1);
        var hashed2String = Encoding.UTF8.GetString(hashed2);

        hashed1String.Should().NotBeNullOrWhiteSpace();
        hashed1String.Should().Be(hashed2String);
    }

    [Fact]
    public void hashing___hmac_sha256()
    {
        var plaintext = "text to hash";
        var privateKey = "key";

        var hashed1 = Hashing.GenerateHMACSHA256(
            privateKey: privateKey,
            toSign: plaintext);

        var hashed2 = Hashing.GenerateHMACSHA256(
            privateKey: privateKey,
            toSign: Encoding.UTF8.GetBytes(plaintext));

        hashed1.Should().NotBeNull();
        hashed1.Should().NotBeEmpty();

        hashed2.Should().NotBeNull();
        hashed2.Should().NotBeEmpty();

        hashed1.Should().BeEquivalentTo(hashed2);

        var hashed1String = Encoding.UTF8.GetString(hashed1);
        var hashed2String = Encoding.UTF8.GetString(hashed2);

        hashed1String.Should().NotBeNullOrWhiteSpace();
        hashed1String.Should().Be(hashed2String);
    }
}
