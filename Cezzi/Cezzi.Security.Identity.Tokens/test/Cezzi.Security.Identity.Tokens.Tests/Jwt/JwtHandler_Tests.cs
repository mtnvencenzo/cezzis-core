namespace Cezzi.Security.Identity.Tokens.Tests.Jwt;

using Cezzi.Security.Identity.Tokens;
using Cezzi.Security.Identity.Tokens.Jwt;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

public class JwtHandler_Tests
{
    private const string Audience = "https://gmail1.co";
    private const string Issuer = "https://is.gmail.co";

    // GenerateNew - Errors
    // -------------------------

    [Fact]
    public void jwthandler___generatenew_throws_on_null_parameters()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
        {
            _ = JwtHandler.GenerateNew(null);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("parameters");
    }

    [Fact]
    public void jwthandler___generatenew_throws_on_null_audience()
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Header items does not contain the 'Audience' key or value");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void jwthandler___generatenew_throws_on_empty_audience(string audience)
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, audience);

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Header items does not contain the 'Audience' key or value");
    }

    [Fact]
    public void jwthandler___generatenew_throws_on_null_expires()
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Header items does not contain the 'Expires' key or value");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void jwthandler___generatenew_throws_on_empty_expires(string expires)
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, expires);

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Header items does not contain the 'Expires' key or value");
    }

    [Fact]
    public void jwthandler___generatenew_throws_on_invalid_expires()
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, "abc");
            parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
            parameters.Claims.Add(JwtClaimType.jti.ToString(), GuidString());
            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Parameters does not contain a valid 'Expires' value");
    }

    [Fact]
    public void jwthandler___generatenew_throws_on_null_issuer()
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, ValidExp());

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Header items does not contain the 'Issuer' key or value");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void jwthandler___generatenew_throws_on_empty_issuer(string issuer)
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, ValidExp());
            parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, issuer);

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Header items does not contain the 'Issuer' key or value");
    }

    [Fact]
    public void jwthandler___generatenew_throws_on_null_claim_jti()
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, ValidExp());
            parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);

            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Claims does not contain the 'jti' key or value");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void jwthandler___generatenew_throws_on_empty_claim_jti(string jti)
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters("test", JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, ValidExp());
            parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
            parameters.Claims.Add(JwtClaimType.jti.ToString(), jti);
            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Claims does not contain the 'jti' key or value");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void jwthandler___generatenew_throws_on_empty_signature_key(string key)
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters(key, JwtAlgorithmType.HMAC_256);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, ValidExp());
            parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
            parameters.Claims.Add(JwtClaimType.jti.ToString(), GuidString());
            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Parameters does not contain a value 'SignatureKey'");
    }

    [Fact]
    public void jwthandler___generatenew_throws_on_empty_signature_type()
    {
        var ex = Assert.Throws<ApplicationException>(() =>
        {
            var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.None);
            parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
            parameters.HeaderItems.Add(JwtHeaderItemType.Expires, ValidExp());
            parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
            parameters.Claims.Add(JwtClaimType.jti.ToString(), GuidString());
            _ = JwtHandler.GenerateNew(parameters);
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Parameters does not contain a value 'SignatureType'");
    }

    // GenerateNew - Success
    // -------------------------

    [Fact]
    public void jwthandler___generatenew_with_non_utc_date()
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();

        var decoded = JwtHandler.DecodeJwtToken(token);
        decoded.Should().NotBeNull();
        decoded.Header.Should().HaveCount(2);
        decoded.Header.Alg.Should().Be("HS256");
        decoded.Header.Typ.Should().Be("JWT");
        decoded.Claims.Should().HaveCount(4);
        ClaimValue(decoded.Claims, JwtClaimType.jti).Should().Be(id);
        ClaimValue(decoded.Claims, "aud").Should().Be(Audience);
        ClaimValue(decoded.Claims, "iss").Should().Be(Issuer);
        ClaimValue(decoded.Claims, "exp").Should().Be(nowstamp.ToString());
    }

    [Theory]
    [InlineData("https://gmail1.co,https://gmail2.co,https://gmail3.co")]
    [InlineData("https://gmail1.co,https://gmail2.co,https://gmail3.co,https://gmail3.co")]
    [InlineData("https://gmail1.co, https://gmail2.co, https://gmail3.co")]
    [InlineData(" , https://gmail1.co, https://gmail2.co, https://gmail3.co, ")]
    public void jwthandler___generatenew_with_multi_audiences(string audience)
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();

        var decoded = JwtHandler.DecodeJwtToken(token);
        decoded.Should().NotBeNull();
        decoded.Header.Should().HaveCount(2);
        decoded.Header.Alg.Should().Be("HS256");
        decoded.Header.Typ.Should().Be("JWT");
        decoded.Claims.Should().HaveCount(6);
        ClaimValue(decoded.Claims, JwtClaimType.jti).Should().Be(id);
        ClaimValue(decoded.Claims, "iss").Should().Be(Issuer);
        ClaimValue(decoded.Claims, "exp").Should().Be(nowstamp.ToString());

        var aud = ClaimValues(decoded.Claims, "aud");
        aud.Should().HaveCount(3);
        aud.Should().HaveElementAt(0, "https://gmail1.co");
        aud.Should().HaveElementAt(1, "https://gmail2.co");
        aud.Should().HaveElementAt(2, "https://gmail3.co");
    }

    [Theory]
    [InlineData(JwtAlgorithmType.HMAC_256, "HS256")]
    [InlineData(JwtAlgorithmType.HMAC_384, "HS384")]
    [InlineData(JwtAlgorithmType.HMAC_512, "HS512")]
    public void jwthandler___generatenew_with_different_algs(JwtAlgorithmType algType, string algName)
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters($"{GuidString()}-{GuidString()}", algType);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();

        var decoded = JwtHandler.DecodeJwtToken(token);
        decoded.Should().NotBeNull();
        decoded.Header.Should().HaveCount(2);
        decoded.Header.Alg.Should().Be(algName);
        decoded.Header.Typ.Should().Be("JWT");
        decoded.Claims.Should().HaveCount(4);
        ClaimValue(decoded.Claims, JwtClaimType.jti).Should().Be(id);
        ClaimValue(decoded.Claims, "aud").Should().Be(Audience);
        ClaimValue(decoded.Claims, "iss").Should().Be(Issuer);
        ClaimValue(decoded.Claims, "exp").Should().Be(nowstamp.ToString());
    }

    // GetTokenId
    // -------------------------

    [Fact]
    public void jwthandler___gettokenid_matches_id()
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();
        JwtHandler.GetTokenId(token).Should().Be(id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("sahfaosfiosdkf")]
    [InlineData("asfaksflaksjfa.asjfhaskjfhajksf.oiqweroiuqeir")]
    public void jwthandler___gettokenid_doesnt_fail_on_invalid_token(string token) => JwtHandler.GetTokenId(token).Should().Be(string.Empty);

    // GetClaimValue
    // -------------------------

    [Fact]
    public void jwthandler___getclaimvalue()
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();

        JwtHandler.GetClaimValue(token, "jti").Should().Be(id);
        JwtHandler.GetClaimValue(token, "aud").Should().Be(Audience);
        JwtHandler.GetClaimValue(token, "iss").Should().Be(Issuer);
        JwtHandler.GetClaimValue(token, "exp").Should().Be(nowstamp.ToString());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("sahfaosfiosdkf")]
    [InlineData("asfaksflaksjfa.asjfhaskjfhajksf.oiqweroiuqeir")]
    public void jwthandler___getclaimvalue_doesnt_fail_on_invalid_token(string token) => JwtHandler.GetClaimValue(token, "jti").Should().Be(string.Empty);

    // GetClaimValues
    // -------------------------

    [Fact]
    public void jwthandler___getclaimvalues_single_claims()
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();

        var claims = JwtHandler.GetClaimValues(token, "jti", "aud", "iss", "exp", "not-real");
        claims.Should().HaveCount(5);
        claims["jti"].Should().Be(id);
        claims["aud"].Should().Be(Audience);
        claims["iss"].Should().Be(Issuer);
        claims["exp"].Should().Be(nowstamp.ToString());
        claims["not-real"].Should().Be(string.Empty);
    }

    [Fact]
    public void jwthandler___getclaimvalues_multi_claims()
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, "https://gmail1.co,https://gmail2.co");
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        token.Should().NotBeNullOrWhiteSpace();

        var claims = JwtHandler.GetClaimValues(token, "jti", "aud", "iss", "exp", "not-real");
        claims.Should().HaveCount(5);
        claims["jti"].Should().Be(id);
        claims["aud"].Should().Be("https://gmail1.co"); // this outcome probably signifies a bug since it's only including the first one
        claims["iss"].Should().Be(Issuer);
        claims["exp"].Should().Be(nowstamp.ToString());
        claims["not-real"].Should().Be(string.Empty);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("sahfaosfiosdkf")]
    [InlineData("asfaksflaksjfa.asjfhaskjfhajksf.oiqweroiuqeir")]
    public void jwthandler___getclaimvalues_doesnt_fail_on_invalid_token(string token)
    {
        var claims = JwtHandler.GetClaimValues(token, "jti", "test");
        claims.Should().NotBeNull();
        claims.Should().HaveCount(2);
        claims["jti"].Should().Be(string.Empty);
        claims["test"].Should().Be(string.Empty);
    }

    // Validate
    // -------------------------

    [Fact]
    public void jwthandler___validate()
    {
        var now = DateTime.Now.AddYears(1);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        var validationParameters = new JwtParameters(parameters.SignatureKey, parameters.SignatureAlgorithm);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);

        var result = JwtHandler.Validate(token, validationParameters);
        result.Should().NotBeNull();
        result.Reason.Should().BeNull();
        result.IsAuthenticated.Should().BeTrue();
        result.Claims.Should().NotBeNull();
        result.Claims.Should().HaveCount(4);
        ClaimValue(result.Claims, "aud").Should().Be(Audience);
        ClaimValue(result.Claims, "exp").Should().Be(nowstamp.ToString());
        ClaimValue(result.Claims, "iss").Should().Be(Issuer);
        ClaimValue(result.Claims, "jti").Should().Be(id.ToString());
    }

    [Fact]
    public void jwthandler___validate_expired_token()
    {
        var now = DateTime.Now.AddDays(-1);
        var nowstr = now.ToString("o");
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        var validationParameters = new JwtParameters(parameters.SignatureKey, parameters.SignatureAlgorithm);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);

        var result = JwtHandler.Validate(token, validationParameters);
        result.Should().NotBeNull();
        result.Reason.Should().Be("Token is expired.");
        result.IsAuthenticated.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("https://gmail1.co ")]
    [InlineData(" https://gmail1.co")]
    [InlineData("https://gmaiL1.co")]
    [InlineData("https://gmail1.com")]
    public void jwthandler___validate_with_non_matching_audience(string audience)
    {
        var now = DateTime.Now.AddDays(12);
        var nowstr = now.ToString("o");
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience.ToUpper());
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        var validationParameters = new JwtParameters(parameters.SignatureKey, parameters.SignatureAlgorithm);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Audience, audience);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);

        var result = JwtHandler.Validate(token, validationParameters);
        result.Should().NotBeNull();
        result.Reason.Should().Be("Token audience is invalid.");
        result.IsAuthenticated.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    [InlineData("https://gmail1.co ")]
    [InlineData(" https://gmail1.co")]
    [InlineData("https://gmAil1.co")]
    [InlineData("https://gmail1.com")]
    public void jwthandler___validate_with_non_matching_issuer(string issuer)
    {
        var now = DateTime.Now.AddDays(12);
        var nowstr = now.ToString("o");
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        var validationParameters = new JwtParameters(parameters.SignatureKey, parameters.SignatureAlgorithm);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Issuer, issuer);

        var result = JwtHandler.Validate(token, validationParameters);
        result.Should().NotBeNull();
        result.Reason.Should().Be("Token issuer is invalid.");
        result.IsAuthenticated.Should().BeFalse();
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("203r023080293424")]
    public void jwthandler___validate_with_non_matching_signaturekey(string signatureKey)
    {
        var now = DateTime.Now.AddDays(12);
        var nowstr = now.ToString("o");
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        var validationParameters = new JwtParameters(signatureKey, parameters.SignatureAlgorithm);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);

        var result = JwtHandler.Validate(token, validationParameters);
        result.Should().NotBeNull();
        result.Reason.Should().Be("Token signature is invalid.");
        result.IsAuthenticated.Should().BeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void jwthandler___validate_throws_with_null_signature_key(string signatureKey)
    {
        var now = DateTime.Now.AddDays(12);
        var nowstr = now.ToString("o");
        var nowstamp = ((DateTimeOffset)now.ToUniversalTime()).ToUnixTimeSeconds();
        var id = GuidString();

        var parameters = new JwtParameters(GuidString(), JwtAlgorithmType.HMAC_256);
        parameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        parameters.HeaderItems.Add(JwtHeaderItemType.Expires, nowstr);
        parameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);
        parameters.Claims.Add(JwtClaimType.jti.ToString(), id);
        var token = JwtHandler.GenerateNew(parameters);

        var validationParameters = new JwtParameters(signatureKey, parameters.SignatureAlgorithm);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Audience, Audience);
        validationParameters.HeaderItems.Add(JwtHeaderItemType.Issuer, Issuer);

        var ex = Assert.ThrowsAny<Exception>(() => JwtHandler.Validate(token, validationParameters));
        ex.Should().NotBeNull();
    }

    private static string ClaimValue(IEnumerable<TokenClaim> claims, string name) => claims.FirstOrDefault(x => x.Name == name)?.Value;
    private static string[] ClaimValues(IEnumerable<Claim> claims, string type) => [.. claims.Where(x => x.Type == type).Select(x => x.Value)];
    private static string ClaimValue(IEnumerable<Claim> claims, JwtClaimType type) => claims.FirstOrDefault(x => x.Type == type.ToString())?.Value;
    private static string ClaimValue(IEnumerable<Claim> claims, string type) => claims.FirstOrDefault(x => x.Type == type)?.Value;
    private static string GuidString() => Guid.NewGuid().ToString();
    private static string ValidExp() => DateTimeOffset.Now.AddYears(30).ToString();
}