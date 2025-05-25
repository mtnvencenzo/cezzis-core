namespace Cezzi.OpenMarket.Tests;

using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class OpenMarketSmsClient_Tests
{
    private readonly Mock<IHttpAccessor> httpAccessorMock;
    private readonly HttpClient httpClient;
    private readonly string originator;
    private readonly string account;
    private readonly string password;

    public OpenMarketSmsClient_Tests()
    {
        this.httpClient = new HttpClient { BaseAddress = new System.Uri("https://test.gmail.com") };
        this.httpAccessorMock = new Mock<IHttpAccessor>();
        this.originator = "1112223333";
        this.account = GuidString();
        this.password = GuidString();
    }

    // Request Validations
    // ---------------------

    [Fact]
    public async Task openmarket___sendasync_throws_on_null_body()
    {
        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await client.SendAsync(
                originator: this.originator,
                account: this.account,
                password: this.password,
                cancellationToken: default,
                body: null).ConfigureAwait(false);
        }).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("body");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task openmarket___sendasync_throws_on_invalid_destination_phone(string phone)
    {
        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await client.SendAsync(
                originator: this.originator,
                account: this.account,
                password: this.password,
                cancellationToken: default,
                body: new SmsSendRequest
                {
                    DestinationPhoneNumber = phone
                }).ConfigureAwait(false);
        }).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("body");
        ex.Message.Should().Be("destination phone number is null (Parameter 'body')");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task openmarket___sendasync_throws_on_invalid_originator(string originator)
    {
        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await client.SendAsync(
                originator: originator,
                account: this.account,
                password: this.password,
                cancellationToken: default,
                body: new SmsSendRequest
                {
                    DestinationPhoneNumber = "1112223333"
                }).ConfigureAwait(false);
        }).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("originator");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task openmarket___sendasync_throws_on_invalid_account(string account)
    {
        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await client.SendAsync(
                originator: this.originator,
                account: account,
                password: this.password,
                cancellationToken: default,
                body: new SmsSendRequest
                {
                    DestinationPhoneNumber = "1112223333"
                }).ConfigureAwait(false);
        }).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("account");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task openmarket___sendasync_throws_on_invalid_password(string password)
    {
        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            await client.SendAsync(
                originator: this.originator,
                account: this.account,
                password: password,
                cancellationToken: default,
                body: new SmsSendRequest
                {
                    DestinationPhoneNumber = "1112223333"
                }).ConfigureAwait(false);
        }).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("password");
    }

    // Send Failures
    // -----------------

    [Fact]
    public async Task openmarket___sendasync_failure()
    {
        var requestId = GuidString();

        this.httpAccessorMock.Setup(x =>
            x.SendAsync(
                It.IsAny<HttpClient>(),
                It.Is<HttpRequestMessage>((r) =>
                    r.RequestUri.ToString() == "sms/v4/mt" &&
                    r.RequestUri.IsAbsoluteUri == false &&
                    r.Method == HttpMethod.Post &&
                    r.Headers.Authorization.Scheme == "Basic" &&
                    r.Headers.Authorization.Parameter == Base64Creds(this.account, this.password) &&
                    r.Content.Headers.ContentType.ToString() == "application/xml; encoding='utf-8'"),
                It.Is<HttpCompletionOption>(x => x == HttpCompletionOption.ResponseHeadersRead),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                var rs = new HttpResponseMessage(HttpStatusCode.BadRequest);
                rs.Headers.Add("X-Request-Id", requestId);
                return rs;
            });

        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var result = await client.SendAsync(
            originator: this.originator,
            account: this.account,
            password: this.password,
            cancellationToken: default,
            body: new SmsSendRequest
            {
                DestinationPhoneNumber = "1112223333"
            }).ConfigureAwait(false);

        result.Location.Should().BeNull();
        result.RequestId.Should().Be(requestId);
        result.SendStatus.Should().Be(SmsSendStatus.Failed);
        result.SendStatusMessage.Should().Be(nameof(HttpStatusCode.BadRequest));
    }

    // Success
    // ---------------------

    [Fact]
    public async Task openmarket___sendasync_success()
    {
        var requestId = GuidString();
        var location = "/test";

        var body = new SmsSendRequest
        {
            DestinationPhoneNumber = "1112223333"
        };

        this.httpAccessorMock.Setup(x =>
            x.SendAsync(
                It.IsAny<HttpClient>(),
                It.Is<HttpRequestMessage>((r) =>
                    r.RequestUri.ToString() == "sms/v4/mt" &&
                    r.RequestUri.IsAbsoluteUri == false &&
                    r.Method == HttpMethod.Post &&
                    r.Headers.Authorization.Scheme == "Basic" &&
                    r.Headers.Authorization.Parameter == Base64Creds(this.account, this.password) &&
                    r.Content.Headers.ContentType.ToString() == "application/xml; encoding='utf-8'" &&
                    XmlBodyMatches(r.Content)),
                It.Is<HttpCompletionOption>(x => x == HttpCompletionOption.ResponseHeadersRead),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() =>
            {
                var rs = new HttpResponseMessage(HttpStatusCode.Accepted);
                rs.Headers.Add("X-Request-Id", requestId);
                rs.Headers.Location = new Uri(location, UriKind.Relative);
                return rs;
            });

        var client = new OpenMarketSmsClient(this.httpClient, this.httpAccessorMock.Object);

        var result = await client.SendAsync(
            originator: this.originator,
            account: this.account,
            password: this.password,
            cancellationToken: default,
            body: body).ConfigureAwait(false);

        result.Location.Should().Be(location);
        result.RequestId.Should().Be(requestId);
        result.SendStatus.Should().Be(SmsSendStatus.Accepted);
        result.SendStatusMessage.Should().Be(nameof(SmsSendStatus.Accepted));
    }

    // ---------------------------------------------------------------------------------------------------
    // To send a real sms add the [Fact] attribute on this method and put your phone number in the request
    // Dont check this in to source control with the Fact attribute!
    // ---------------------------------------------------------------------------------------------------
#pragma warning disable xUnit1013 // Public method should be marked as test
    public async static Task real_send()
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://smsc.openmarket.com:443")
        };

        var client = new OpenMarketSmsClient(httpClient);

        var result = await client.SendAsync(
            originator: "18662455499",
            account: "000-000-131-31872",
            password: "AetZ4#26",
            body: new SmsSendRequest
            {
                TextContent = "ron test",
                DestinationPhoneNumber = "put phone number here", // 
                Note1 = "notes 1",
                Note2 = "notes 2"
            }).ConfigureAwait(false);

        result.Should().NotBeNull();

    }

    private static bool XmlBodyMatches(HttpContent content)
    {
        var sc = content as StringContent;
        using var ms = new MemoryStream();
        sc.CopyTo(ms, null, default);
        var bytes = ms.ToArray();
        ;
        var xml = Encoding.UTF8.GetString(bytes);

        xml.Should().Contain(@"<mobileTerminate promotional=""false"">");

        return true;
    }

    private static string GuidString() => Guid.NewGuid().ToString();

    private static string Base64Creds(string act, string pass) => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{act}:{pass}"));
}