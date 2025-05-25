namespace Cezzi.SendGrid.Tests;

using FluentAssertions;
using global::SendGrid;
using global::SendGrid.Helpers.Mail;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

/// <summary>
/// 
/// </summary>
public class SendGridEmailServiceTests
{
    private const string fromEmail = "noreply@gmail.com";
    private readonly EmailValue toEmail = new(email: "r@cezzis.com", name: "R Vechi");

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task sendgrid___send_throws_on_invalid_subject(string subject)
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await emailService.SendAsync(
            subject: subject,
            fromEmail: new EmailValue(email: fromEmail),
            tos:
            [
                this.toEmail,
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("subject");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData(null, null)]
    public async Task sendgrid___send_throws_on_invalid_email_content(string htmlContent, string plainTextContent)
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: new EmailValue(email: fromEmail),
            tos:
            [
                this.toEmail,
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: htmlContent,
            plainTextContent: plainTextContent,
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be($"Either htmlContent or plainTextContent must be supplied.");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task sendgrid___send_throws_on_invalid_from_email(string fromEmailTest)
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: new EmailValue(email: fromEmailTest),
            tos:
            [
                this.toEmail,
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("fromEmail");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task sendgrid___send_throws_on_null_from_email()
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () => await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: null,
            tos:
            [
                this.toEmail,
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("fromEmail");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task sendgrid___send_throws_on_null_tos()
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: new EmailValue(email: fromEmail),
            tos: null,
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be("At least one receipent email must be specified in the tos list.");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task sendgrid___send_throws_on_empty_tos()
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: new EmailValue(email: fromEmail),
            tos: [],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be("At least one receipent email must be specified in the tos list.");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task sendgrid___send_throws_on_invalid_tos_email(string toEmail)
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var ex = await Assert.ThrowsAsync<ArgumentException>(async () => await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: new EmailValue(email: fromEmail),
            tos:
            [
                this.toEmail,
                new EmailValue(email: toEmail)
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false)).ConfigureAwait(false);

        ex.Should().NotBeNull();
        ex.Message.Should().Be("You must specify an email address for each recipient in the tos list.");

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(true, null, "test")]
    [InlineData(true, "", "test")]
    [InlineData(true, " ", "test")]
    [InlineData(true, "<b>test</b>", null)]
    [InlineData(true, "<b>test</b>", "")]
    [InlineData(true, "<b>test</b>", " ")]
    [InlineData(false, "<b>test</b>", "test")]
    public async Task sendgrid___send_works(bool sendWithPriority, string htmlContent, string plainTextContent)
    {
        var sendGridConfiguration = GetConfiguration();

        var emailServiceMock = GetNoSendMock();
        var emailService = emailServiceMock.Object;

        var isSuccess = await emailService.SendAsync(
            subject: "Test Subject",
            fromEmail: new EmailValue(email: fromEmail),
            tos:
            [
                this.toEmail
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: htmlContent,
            plainTextContent: plainTextContent,
            sendWithPriority: sendWithPriority).ConfigureAwait(false);

        isSuccess.Should().BeTrue();

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // Uncomment this our and provide a real api key below to test if it really can send an email.
    //[Fact]
#pragma warning disable xUnit1013 // Public method should be marked as test
    public async Task sendgrid___sends_with_both_text_and_html_specified()
#pragma warning restore xUnit1013 // Public method should be marked as test
    {
        var sendGridConfiguration = GetConfiguration();
        sendGridConfiguration.SendGridApiKey = "<real-api-key>";

        var emailServiceMock = GetSendableMock();
        var emailService = emailServiceMock.Object;

        var isSuccess = await emailService.SendAsync(
            subject: $"Test from {nameof(SendGridEmailServiceTests)}",
            fromEmail: new EmailValue(email: fromEmail),
            tos:
            [
                this.toEmail,
            ],
            sendGridConfiguration: sendGridConfiguration,
            htmlContent: "<b>test</b>",
            plainTextContent: "test",
            sendWithPriority: true).ConfigureAwait(false);

        isSuccess.Should().BeTrue();

        emailServiceMock.Verify(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    private static SendGridConfiguration GetConfiguration()
    {
        return new SendGridConfiguration
        {
            HttpErrorsAsExceptions = true,
            SendGridApiKey = "test",
            SendGridHost = "https://api.sendgrid.com",
            SendGridUrlPath = "mail/send",
            SendGridVersion = "v3",
            Reliability = new SendGridReliability
            {
                MaximumNumberOfRetries = 0,
                MaximumBackoffSeconds = 0,
                MinimumBackoffSeconds = 0,
                DeltaBackoffSeconds = 0
            }
        };
    }

    private static Mock<SendGridEmailService> GetNoSendMock()
    {
        var mock = new Mock<SendGridEmailService>(new System.Net.Http.HttpClient())
        {
            CallBase = true
        };

        mock.Setup(x => x.SendWithSendGrid(It.IsAny<SendGridClient>(), It.IsAny<SendGridMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        return mock;
    }

    private static Mock<SendGridEmailService> GetSendableMock()
    {
        var mock = new Mock<SendGridEmailService>(new System.Net.Http.HttpClient())
        {
            CallBase = true
        };

        return mock;
    }
}
