namespace Cezzi.Security.Recaptcha.Tests;

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

public class BootstrapperTests
{
    [Fact]
    public void UseRecaptcha_injects_recaptcha_service()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configurationBuilder.Build());

        var sp = services.BuildServiceProvider();

        var recaptchaService = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        recaptchaService.Should().BeOfType<RecaptchaSiteVerifyService>();
    }

    [Fact]
    public void UseRecaptcha_injects_httpclient()
    {
        var configurationBuilder = new ConfigurationBuilder();

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configurationBuilder.Build());

        var sp = services.BuildServiceProvider();

        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();

        var httpClient = httpClientFactory.CreateClient(RecaptchaSiteVerifyService.HttpClientName);

        httpClient.Should().NotBeNull();
    }

    [Fact]
    public void UseRecaptcha_injects_recaptcha_config_options()
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        var sp = services.BuildServiceProvider();

        var options = sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value;

        options.SiteSecret.Should().Be("test-secret");
        options.SiteVerifyUrl.Should().Be("http://localhost:009090/test-url");
    }
}