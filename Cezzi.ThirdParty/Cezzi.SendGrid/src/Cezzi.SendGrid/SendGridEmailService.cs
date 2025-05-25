namespace Cezzi.SendGrid;

using global::SendGrid;
using global::SendGrid.Helpers.Mail;
using global::SendGrid.Helpers.Mail.Model;
using global::SendGrid.Helpers.Reliability;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.SendGrid.ISendGridEmailService" />
/// <remarks>Initializes a new instance of the <see cref="SendGridEmailService"/> class.</remarks>
/// <param name="httpClient">The HTTP client.</param>
/// <exception cref="System.ArgumentNullException">httpClient</exception>
public class SendGridEmailService(HttpClient httpClient) : ISendGridEmailService
{
    private readonly HttpClient httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

    private const string DefaultSendGridHost = "https://api.sendgrid.com";
    private const string DefaultSendGridUrlPath = "mail/send";
    private const string DefaultSendGridVersion = "v3";

    /// <summary>Creates the specified HTTP client.</summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <returns></returns>
    public static SendGridEmailService Create(HttpClient httpClient) => new(httpClient: httpClient);

    /// <summary>Sends the specified subject.</summary>
    /// <param name="subject">The subject.</param>
    /// <param name="fromEmail">From email.</param>
    /// <param name="tos">The tos.</param>
    /// <param name="htmlContent">Content of the HTML.</param>
    /// <param name="plainTextContent">Content of the plain text.</param>
    /// <param name="sendWithPriority">if set to <c>true</c> [send with priority].</param>
    /// <param name="sendGridConfiguration">The send grid configuration.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentException">Either {nameof(htmlContent)} or {nameof(plainTextContent)} must be supplied.
    /// or
    /// At least one receipent email must be specified in the {nameof(tos)} list.
    /// or
    /// You must specify an email address for each recipient in the {nameof(tos)} list.</exception>
    /// <exception cref="System.ArgumentNullException">subject
    /// or
    /// fromEmail</exception>
    public async virtual Task<bool> SendAsync(
        string subject,
        EmailValue fromEmail,
        IList<EmailValue> tos,
        SendGridConfiguration sendGridConfiguration,
        string htmlContent = null,
        string plainTextContent = null,
        bool sendWithPriority = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(sendGridConfiguration);

        if (string.IsNullOrWhiteSpace(htmlContent) && string.IsNullOrWhiteSpace(plainTextContent))
        {
            throw new ArgumentException($"Either {nameof(htmlContent)} or {nameof(plainTextContent)} must be supplied.");
        }

        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentNullException(nameof(subject));
        }

        if (fromEmail == null || string.IsNullOrWhiteSpace(fromEmail.Email))
        {
            throw new ArgumentNullException(nameof(fromEmail));

        }

        if (tos == null || tos.Count == 0)
        {
            throw new ArgumentException($"At least one receipent email must be specified in the {nameof(tos)} list.");
        }

        foreach (var to in tos)
        {
            if (string.IsNullOrWhiteSpace(to.Email))
            {
                throw new ArgumentException($"You must specify an email address for each recipient in the {nameof(tos)} list.");
            }
        }

        // Max backoff cannot exceed 30 seconds
        // Adjust it if over the threshold.
        var maximumBackOffseconds = sendGridConfiguration.Reliability?.MaximumBackoffSeconds ?? 0;
        if (maximumBackOffseconds > 30)
        {
            maximumBackOffseconds = 30;
        }

        // Make sure a user didn't supply a value less than zero
        var maximumNumberOfRetries = sendGridConfiguration.Reliability?.MaximumNumberOfRetries ?? 0;
        if (maximumNumberOfRetries < 0)
        {
            maximumNumberOfRetries = 0;
        }

        if (maximumNumberOfRetries > 10)
        {
            maximumNumberOfRetries = 10;
        }

        // Make sure a user didn't supply a value less than zero
        var minimumBackoffSeconds = sendGridConfiguration.Reliability?.MinimumBackoffSeconds ?? 0;
        if (minimumBackoffSeconds < 0)
        {
            minimumBackoffSeconds = 0;
        }

        // Make sure a user didn't supply a value less than zero
        var deltaBackoffSeconds = sendGridConfiguration.Reliability?.DeltaBackoffSeconds ?? 0;
        if (deltaBackoffSeconds < 0)
        {
            deltaBackoffSeconds = 0;
        }

        var sendGridClient = new SendGridClient(this.httpClient, new SendGridClientOptions
        {
            Host = sendGridConfiguration.SendGridHost ?? DefaultSendGridHost,
            UrlPath = sendGridConfiguration.SendGridUrlPath ?? DefaultSendGridUrlPath,
            Version = sendGridConfiguration.SendGridVersion ?? DefaultSendGridVersion,
            ApiKey = sendGridConfiguration.SendGridApiKey,
            Auth = new AuthenticationHeaderValue("Bearer", sendGridConfiguration.SendGridApiKey),
            HttpErrorAsException = sendGridConfiguration.HttpErrorsAsExceptions,
            ReliabilitySettings = new ReliabilitySettings(
                maximumNumberOfRetries: maximumNumberOfRetries,
                minimumBackoff: TimeSpan.FromSeconds(minimumBackoffSeconds),
                maximumBackOff: TimeSpan.FromSeconds(maximumBackOffseconds),
                deltaBackOff: TimeSpan.FromSeconds(deltaBackoffSeconds))
        });

        var sendGridFrom = (string.IsNullOrWhiteSpace(fromEmail.Name))
            ? new EmailAddress(email: fromEmail.Email)
            : new EmailAddress(email: fromEmail.Email, name: fromEmail.Name);

        var sendGridMessage = new SendGridMessage
        {
            From = sendGridFrom,
            Subject = subject,
            Contents = [],
            Personalizations =
            [
                new Personalization
                {
                    Tos = []
                }
            ]
        };

        if (!string.IsNullOrWhiteSpace(htmlContent))
        {
            sendGridMessage.Contents.Add(new HtmlContent(value: htmlContent));
        }

        if (!string.IsNullOrWhiteSpace(plainTextContent))
        {
            sendGridMessage.Contents.Add(new PlainTextContent(value: plainTextContent));
        }

        foreach (var to in tos)
        {
            if (!string.IsNullOrWhiteSpace(to.Name))
            {
                sendGridMessage.Personalizations[0].Tos.Add(new EmailAddress(email: to.Email, name: to.Name));
            }
            else
            {
                sendGridMessage.Personalizations[0].Tos.Add(new EmailAddress(email: to.Email));
            }
        }

        if (sendWithPriority)
        {
            sendGridMessage.Headers = new Dictionary<string, string>
            {
                { "Priority", "Urgent" },
                { "Importance", "high" }
            };
        }

        var isSuccess = await this.SendWithSendGrid(
            sendGridClient: sendGridClient,
            sendGridMessage: sendGridMessage,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return isSuccess;
    }

    async virtual internal Task<bool> SendWithSendGrid(
        SendGridClient sendGridClient,
        SendGridMessage sendGridMessage,
        CancellationToken cancellationToken = default)
    {
        var response = await sendGridClient.SendEmailAsync(sendGridMessage, cancellationToken: cancellationToken).ConfigureAwait(false);

        return response.IsSuccessStatusCode;
    }
}
