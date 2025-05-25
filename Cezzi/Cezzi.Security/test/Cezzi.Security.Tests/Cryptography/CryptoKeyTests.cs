namespace Cezzi.Security.Tests.Cryptography;

using Cezzi.Security.Cryptography;
using FluentAssertions;
using System;
using Xunit;

public class CryptoKeyTests
{
    [Theory]
    [InlineData(null)]
    [InlineData(" ")]
    [InlineData("")]
    public void cryptokey___throws_on_null_or_white_space_key(string key)
    {
        var ex = Assert.Throws<ArgumentNullException>(paramName: nameof(key), testCode: () => new CryptoKey(key: key, vector: "test"));
        ex.Should().NotBeNull();
    }

    [Theory]
    [InlineData(null)]
    public void cryptokey___throws_on_null_vector(string vector)
    {
        var ex = Assert.Throws<ArgumentNullException>(paramName: nameof(vector), testCode: () => new CryptoKey(key: "test", vector: vector));
        ex.Should().NotBeNull();
    }

    [Fact]
    public void cryptokey___constrctor_creates_key()
    {
        var cryptoKey = new CryptoKey(key: "testKey", vector: "testVector");

        cryptoKey.Key.Should().Be("testKey");
        cryptoKey.Vector.Should().Be("testVector");
    }
}
