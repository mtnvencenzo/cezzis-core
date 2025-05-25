namespace Cezzi.Security.Tests.Cryptography;

using Cezzi.Security.Cryptography;
using FluentAssertions;
using System;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// 
/// </summary>
public class AesCryptoProviderTests
{
    [Fact]
    public void aescrypto___roundtrip()
    {
        var plainText = "this is my text to encrypt";
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var encrypted = cryptoProvider.Encrypt(toEncrypt: plainText);
        encrypted.Should().NotBeNullOrWhiteSpace();
        encrypted.Should().NotBe(plainText);

        var decrypted = cryptoProvider.Decrypt(cipherText: encrypted);
        decrypted.Should().NotBeNullOrWhiteSpace();
        decrypted.Should().Be(plainText);
    }

    [Fact]
    public void aescrypto___extensions_roundtrip()
    {
        var plainText = "this is my text to encrypt";
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Encrytable = plainText
        };

        var retval1 = testClass.EncryptProperty(x => x.Encrytable, cryptoProvider);
        testClass.Encrytable.Should().NotBeNullOrWhiteSpace();
        testClass.Encrytable.Should().NotBe(plainText);

        var retval2 = testClass.DecryptProperty(x => x.Encrytable, cryptoProvider);
        testClass.Encrytable.Should().NotBeNullOrWhiteSpace();
        testClass.Encrytable.Should().Be(plainText);

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public void aescrypto___extensions_roundtrip_but_null_object()
    {
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        TestClass testClass = null;

        var retval1 = testClass.EncryptProperty(x => x.Encrytable, cryptoProvider);
        testClass.Should().BeNull();
        retval1.Should().BeNull();

        var retval2 = testClass.DecryptProperty(x => x.Encrytable, cryptoProvider);
        testClass.Should().BeNull();
        retval2.Should().BeNull();

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public void aescrypto___extensions_roundtrip_but_null_property()
    {
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Encrytable = null
        };

        var retval1 = testClass.EncryptProperty(x => x.Encrytable, cryptoProvider);
        testClass.Should().NotBeNull();
        testClass.Encrytable.Should().BeNull();
        retval1.Should().NotBeNull();
        retval1.Encrytable.Should().BeNull();

        var retval2 = testClass.DecryptProperty(x => x.Encrytable, cryptoProvider);
        testClass.Should().NotBeNull();
        testClass.Encrytable.Should().BeNull();
        retval2.Should().NotBeNull();
        retval2.Encrytable.Should().BeNull();

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public void aescrypto___extensions_roundtrip_with_nested_prop()
    {
        var plainText = "this is my text to encrypt";
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Nested = new NestedClass()
        };
        testClass.Nested.NestedProperty = plainText;

        var retval1 = testClass.EncryptProperty(x => x.Nested.NestedProperty, cryptoProvider);
        testClass.Nested.NestedProperty.Should().NotBeNullOrWhiteSpace();
        testClass.Nested.NestedProperty.Should().NotBe(plainText);

        var retval2 = testClass.DecryptProperty(x => x.Nested.NestedProperty, cryptoProvider);
        testClass.Nested.NestedProperty.Should().NotBeNullOrWhiteSpace();
        testClass.Nested.NestedProperty.Should().Be(plainText);

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public void aescrypto___extensions_roundtrip_with_nested_prop_but_null()
    {
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Nested = null
        };

        var retval1 = testClass.EncryptProperty(x => x.Nested.NestedProperty, cryptoProvider);
        testClass.Should().NotBeNull();
        testClass.Nested.Should().BeNull();
        retval1.Should().NotBeNull();
        retval1.Nested.Should().BeNull();

        var retval2 = testClass.DecryptProperty(x => x.Nested.NestedProperty, cryptoProvider);
        testClass.Should().NotBeNull();
        testClass.Nested.Should().BeNull();
        retval2.Should().NotBeNull();
        retval2.Nested.Should().BeNull();

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public void aescrypto___extensions_roundtrip_with_nested_prop_but_property()
    {
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Nested = new NestedClass
            {
                NestedProperty = null
            }
        };

        var retval1 = testClass.EncryptProperty(x => x.Nested.NestedProperty, cryptoProvider);
        testClass.Should().NotBeNull();
        testClass.Nested.Should().NotBeNull();
        testClass.Nested.NestedProperty.Should().BeNull();
        retval1.Should().NotBeNull();
        retval1.Nested.Should().NotBeNull();
        retval1.Nested.NestedProperty.Should().BeNull();

        var retval2 = testClass.DecryptProperty(x => x.Nested.NestedProperty, cryptoProvider);
        testClass.Should().NotBeNull();
        testClass.Nested.Should().NotBeNull();
        testClass.Nested.NestedProperty.Should().BeNull();
        retval2.Should().NotBeNull();
        retval2.Nested.Should().NotBeNull();
        retval2.Nested.NestedProperty.Should().BeNull();

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public async Task aescrypto___extensions_roundtrip_async()
    {
        var plainText = "this is my text to encrypt";
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Encrytable = plainText
        };

        var retval1 = await testClass.EncryptPropertyAsync(x => x.Encrytable, cryptoProvider).ConfigureAwait(false);
        testClass.Encrytable.Should().NotBeNullOrWhiteSpace();
        testClass.Encrytable.Should().NotBe(plainText);

        var retval2 = await testClass.DecryptPropertyAsync(x => x.Encrytable, cryptoProvider).ConfigureAwait(false);
        testClass.Encrytable.Should().NotBeNullOrWhiteSpace();
        testClass.Encrytable.Should().Be(plainText);

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public async Task aescrypto___extensions_roundtrip_with_nested_prop_async()
    {
        var plainText = "this is my text to encrypt";
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Nested = new NestedClass()
        };
        testClass.Nested.NestedProperty = plainText;

        var retval1 = await testClass.EncryptPropertyAsync(x => x.Nested.NestedProperty, cryptoProvider).ConfigureAwait(false);
        testClass.Nested.NestedProperty.Should().NotBeNullOrWhiteSpace();
        testClass.Nested.NestedProperty.Should().NotBe(plainText);

        var retval2 = await testClass.DecryptPropertyAsync(x => x.Nested.NestedProperty, cryptoProvider).ConfigureAwait(false);
        testClass.Nested.NestedProperty.Should().NotBeNullOrWhiteSpace();
        testClass.Nested.NestedProperty.Should().Be(plainText);

        retval1.Should().BeSameAs(testClass);
        retval2.Should().BeSameAs(testClass);
    }

    [Fact]
    public void aescrypto___extensions_throws_when_trying_to_encrypt_or_decrypt_a_readonly_property()
    {
        var plainText = "this is my text to encrypt";
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass
        {
            Encrytable = plainText
        };

        var ex1 = Assert.Throws<ArgumentException>(() => testClass.EncryptProperty(x => x.NotEncrytableReadonly, cryptoProvider));
        ex1.Should().NotBeNull();

        var ex2 = Assert.Throws<ArgumentException>(() => testClass.DecryptProperty(x => x.NotEncrytableReadonly, cryptoProvider));
        ex2.Should().NotBeNull();
    }

    [Fact]
    public void aescrypto___extensions_throws_when_trying_to_encrypt_or_decrypt_a_method()
    {
        var cryptoKey = new CryptoKey(
            key: "==29kkkffUI9&093883940ff",
            vector: "==08940049d9fj(99404400I");

        var cryptoProvider = new AesCryptoProvider(key: cryptoKey);

        var testClass = new TestClass();

        var ex1 = Assert.Throws<ArgumentException>(() => testClass.EncryptProperty(x => TestClass.StringMethod(), cryptoProvider));
        ex1.Should().NotBeNull();

        var ex2 = Assert.Throws<ArgumentException>(() => testClass.DecryptProperty(x => TestClass.StringMethod(), cryptoProvider));
        ex2.Should().NotBeNull();
    }

    private class TestClass
    {
        public string NotEncrytableReadonly { get; }

        public string Encrytable { get; set; }

        public static string StringMethod() => "test";

        public NestedClass Nested { get; set; }
    }

    private class NestedClass
    {
        public string NestedProperty { get; set; }
    }
}
