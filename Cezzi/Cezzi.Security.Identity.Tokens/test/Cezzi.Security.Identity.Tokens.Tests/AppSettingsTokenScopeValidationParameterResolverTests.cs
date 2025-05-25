namespace Cezzi.Security.Identity.Tokens.Tests;

using Cezzi.Security.Identity.Tokens.Jwt;
using FluentAssertions;
using Xunit;

public class AppSettingsTokenScopeValidationParameterResolverTests
{
    [Fact]
    public void apptokenscoperesolver___from_config_string_resolves()
    {
        var resolver = new AppSettingsTokenScopeValidationParameterResolver(
            appSetting: "join@@signup@@HMAC_256@@221-#3$a-9108-dRyu-=09I-c%s5B#0909-=@@https://devjoin.gmail.com@@https://devidentity.gmail.com/api/join/signup/access/token||oauth2@@delegation@@HMAC_256@@2A!-!3AW-0198-4w3r-S22!-cEs5B@0909-#@@https://devjoin.gmail.com@@https://devidentity.gmail.com/api/oauth2/delegation/access/token");

        resolver.Should().NotBeNull();

        var scope1 = resolver.GetTokenValidationParameters(
            scope: "join",
            subscope: "signup");
        scope1.Should().NotBeNull();
        scope1.AlgorithmType.Should().Be(JwtAlgorithmType.HMAC_256);
        scope1.Scope.Should().Be("join");
        scope1.SubScope.Should().Be("signup");
        scope1.SharedKey.Should().Be("221-#3$a-9108-dRyu-=09I-c%s5B#0909-=");
        scope1.Audience.Should().Be("https://devjoin.gmail.com");
        scope1.Issuer.Should().Be("https://devidentity.gmail.com/api/join/signup/access/token");

        var scope2 = resolver.GetTokenValidationParameters(
            scope: "oauth2",
            subscope: "delegation");
        scope2.Should().NotBeNull();
        scope2.AlgorithmType.Should().Be(JwtAlgorithmType.HMAC_256);
        scope2.Scope.Should().Be("oauth2");
        scope2.SubScope.Should().Be("delegation");
        scope2.SharedKey.Should().Be("2A!-!3AW-0198-4w3r-S22!-cEs5B@0909-#");
        scope2.Audience.Should().Be("https://devjoin.gmail.com");
        scope2.Issuer.Should().Be("https://devidentity.gmail.com/api/oauth2/delegation/access/token");

        var scope3 = resolver.GetTokenValidationParameters("test", "something");

        scope3.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void apptokenscoperesolver___empty_initialized_retunrs_default_parameter(string config)
    {
        var resolver = new AppSettingsTokenScopeValidationParameterResolver(config);
        var scope1 = resolver.GetTokenValidationParameters("test", "something");

        scope1.Should().BeNull();
    }
}
