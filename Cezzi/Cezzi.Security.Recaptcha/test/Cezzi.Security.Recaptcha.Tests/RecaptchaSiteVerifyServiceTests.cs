namespace Cezzi.Security.Recaptcha.Tests;

using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;

public class RecaptchaSiteVerifyServiceTests
{
    private readonly Mock<IHttpClientFactory> httpClientFactoryMock;
    private readonly Mock<HttpClient> httpClientMock;

    public RecaptchaSiteVerifyServiceTests()
    {
        this.httpClientFactoryMock = new Mock<IHttpClientFactory>();
        this.httpClientMock = new Mock<HttpClient>();

        this.httpClientFactoryMock
            .Setup(x => x.CreateClient(It.Is<string>(x => x == RecaptchaSiteVerifyService.HttpClientName)))
            .Returns(this.httpClientMock.Object);
    }

    [Fact]
    public async Task UseRecaptcha_returns_communication_error_for_null_recaptcha_response()
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);

        this.httpClientMock
            .Setup(x => x.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), this.httpClientFactoryMock.Object));

        var sp = services.BuildServiceProvider();

        var service = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        var response = await service.Verify(
            verificationCode: "oais=asd-d-sd---a-sd-asdasdas-d-a-sda-sd",
            config: sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value,
            userIp: "127.0.0.1");

        response.Should().NotBeNull();
        response.Hostname.Should().Be(null);
        response.ReturnCodes.Should().ContainSingle();
        response.ReturnCodes.First().Message.Should().Be("Communication Error");
        response.ReturnCodes.First().Code.Should().Be(100);
        response.Score.Should().Be(null);
        response.VerificationStatus.Should().Be(RecaptchaVerificationStatus.NotAttempted);
        response.UtcVerificationTimestamp.Should().Be(null);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("<>")]
    public async Task UseRecaptcha_returns_communication_error_for_non_json_recaptcha_response(string responseContent)
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(responseContent)
        };

        this.httpClientMock
            .Setup(x => x.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), this.httpClientFactoryMock.Object));

        var sp = services.BuildServiceProvider();

        var service = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        var response = await service.Verify(
            verificationCode: "oais=asd-d-sd---a-sd-asdasdas-d-a-sda-sd",
            config: sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value,
            userIp: "127.0.0.1");

        response.Should().NotBeNull();
        response.Hostname.Should().Be(null);
        response.ReturnCodes.Should().ContainSingle();
        response.ReturnCodes.First().Message.Should().Be("Communication Error");
        response.ReturnCodes.First().Code.Should().Be(100);
        response.Score.Should().Be(null);
        response.VerificationStatus.Should().Be(RecaptchaVerificationStatus.NotAttempted);
        response.UtcVerificationTimestamp.Should().Be(null);
    }

    [Fact]
    public async Task UseRecaptcha_returns_success_with_empty_array_error_codes()
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\r\n  \"success\": true,\r\n  \"challenge_ts\": \"2024-05-26T12:32:12\",\r\n  \"hostname\": \"cezzis.com\",\r\n  \"error-codes\": []\r\n}")
        };

        this.httpClientMock
            .Setup(x => x.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), this.httpClientFactoryMock.Object));

        var sp = services.BuildServiceProvider();

        var service = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        var response = await service.Verify(
            verificationCode: "oais=asd-d-sd---a-sd-asdasdas-d-a-sda-sd",
            config: sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value,
            userIp: "127.0.0.1");

        response.Should().NotBeNull();
        response.Hostname.Should().Be("cezzis.com");
        response.ReturnCodes.Should().BeEmpty();
        response.Score.Should().Be(null);
        response.VerificationStatus.Should().Be(RecaptchaVerificationStatus.Success);
        response.UtcVerificationTimestamp.Should().BeOnOrAfter(DateTime.Parse("2024-05-26T12:32:12"));
    }

    [Fact]
    public async Task UseRecaptcha_returns_failed_with_empty_array_error_codes()
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\r\n  \"success\": false,\r\n  \"challenge_ts\": \"2024-05-26T12:32:12\",\r\n  \"hostname\": \"cezzis.com\",\r\n  \"error-codes\": []\r\n}")
        };

        this.httpClientMock
            .Setup(x => x.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), this.httpClientFactoryMock.Object));

        var sp = services.BuildServiceProvider();

        var service = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        var response = await service.Verify(
            verificationCode: "oais=asd-d-sd---a-sd-asdasdas-d-a-sda-sd",
            config: sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value,
            userIp: "127.0.0.1");

        response.Should().NotBeNull();
        response.Hostname.Should().Be("cezzis.com");
        response.ReturnCodes.Should().BeEmpty();
        response.Score.Should().Be(null);
        response.VerificationStatus.Should().Be(RecaptchaVerificationStatus.Failed);
        response.UtcVerificationTimestamp.Should().BeOnOrAfter(DateTime.Parse("2024-05-26T12:32:12"));
    }

    [Fact]
    public async Task UseRecaptcha_returns_failed_with_multiple_array_error_codes()
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\r\n  \"success\": false,\r\n  \"challenge_ts\": \"2024-05-26T12:32:12\",\r\n  \"hostname\": \"cezzis.com\",\r\n  \"error-codes\": [ \"missing-input-secret\", \"invalid-input-secret\", \"missing-input-response\", \"invalid-input-response\", \"bad-request\", \"timeout-or-duplicate\" ]\r\n}")
        };

        this.httpClientMock
            .Setup(x => x.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), this.httpClientFactoryMock.Object));

        var sp = services.BuildServiceProvider();

        var service = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        var response = await service.Verify(
            verificationCode: "oais=asd-d-sd---a-sd-asdasdas-d-a-sda-sd",
            config: sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value,
            userIp: "127.0.0.1");

        response.Should().NotBeNull();
        response.Hostname.Should().Be("cezzis.com");
        response.Score.Should().Be(null);
        response.VerificationStatus.Should().Be(RecaptchaVerificationStatus.Failed);
        response.UtcVerificationTimestamp.Should().BeOnOrAfter(DateTime.Parse("2024-05-26T12:32:12"));
        response.ReturnCodes.Should().HaveCount(6);

        response.ReturnCodes[0].Message.Should().Be("Secret Key Is Required");
        response.ReturnCodes[0].Code.Should().Be(101);

        response.ReturnCodes[1].Message.Should().Be("Secret Is Invalid");
        response.ReturnCodes[1].Code.Should().Be(102);

        response.ReturnCodes[2].Message.Should().Be("Verification Code Is Required");
        response.ReturnCodes[2].Code.Should().Be(103);

        response.ReturnCodes[3].Message.Should().Be("Verification Code Is Invalid");
        response.ReturnCodes[3].Code.Should().Be(104);

        response.ReturnCodes[4].Message.Should().Be("Invalid Request");
        response.ReturnCodes[4].Code.Should().Be(105);

        response.ReturnCodes[5].Message.Should().Be("Timeout");
        response.ReturnCodes[5].Code.Should().Be(106);
    }

    [Fact]
    public async Task UseRecaptcha_returns_failed_with_unknown_array_error_code()
    {
        var configuurationManager = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

        var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\r\n  \"success\": false,\r\n  \"challenge_ts\": \"2024-05-26T12:32:12\",\r\n  \"hostname\": \"cezzis.com\",\r\n  \"error-codes\": [ \"not-a-real-code\" ]\r\n}")
        };

        this.httpClientMock
            .Setup(x => x.SendAsync(
                It.IsAny<HttpRequestMessage>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(httpResponse);

        var services = new ServiceCollection();

        Bootstrapper.UseRecaptcha(services, configuurationManager.Build());

        services.Replace(new ServiceDescriptor(typeof(IHttpClientFactory), this.httpClientFactoryMock.Object));

        var sp = services.BuildServiceProvider();

        var service = sp.GetRequiredService<IRecaptchaSiteVerifyService>();

        var response = await service.Verify(
            verificationCode: "oais=asd-d-sd---a-sd-asdasdas-d-a-sda-sd",
            config: sp.GetRequiredService<IOptions<RecaptchaConfig>>().Value,
            userIp: "127.0.0.1");

        response.Should().NotBeNull();
        response.Hostname.Should().Be("cezzis.com");
        response.Score.Should().Be(null);
        response.VerificationStatus.Should().Be(RecaptchaVerificationStatus.Failed);
        response.UtcVerificationTimestamp.Should().BeOnOrAfter(DateTime.Parse("2024-05-26T12:32:12"));
        response.ReturnCodes.Should().ContainSingle();

        response.ReturnCodes.First().Message.Should().Be("Communication Error");
        response.ReturnCodes.First().Code.Should().Be(100);
    }
}
