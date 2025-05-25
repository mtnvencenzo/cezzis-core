namespace Cezzi.Security.Identity.Tokens.Tests.Jwt;

using Cezzi.Security.Identity.Tokens.Jwt;
using FluentAssertions;
using Xunit;

public class JwtClaimTypeExtensionMethods_Tests
{
    [Theory]
    [InlineData(JwtClaimType.clientdocno)]
    [InlineData(JwtClaimType.pubkey)]
    [InlineData(JwtClaimType.email)]
    public void jwtclaimtypeextensions___isencrpted_true(JwtClaimType type) => type.IsEncrypted().Should().BeTrue();

    [Theory]
    [InlineData(JwtClaimType.None)]
    [InlineData(JwtClaimType.feature)]
    [InlineData(JwtClaimType.jti)]
    [InlineData(JwtClaimType.iat)]
    [InlineData(JwtClaimType.chapid)]
    [InlineData(JwtClaimType.natid)]
    [InlineData(JwtClaimType.userid)]
    [InlineData(JwtClaimType.adminid)]
    [InlineData(JwtClaimType.usertype)]
    [InlineData(JwtClaimType.preauthenticateduser)]
    [InlineData(JwtClaimType.oauthresult)]
    [InlineData(JwtClaimType.groupid)]
    [InlineData(JwtClaimType.scope)]
    [InlineData(JwtClaimType.subscope)]
    public void jwtclaimtypeextensions___isencrpted_false(JwtClaimType type) => type.IsEncrypted().Should().BeFalse();
}
