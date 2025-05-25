namespace Cezzi.OpenMarket;

using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.OpenMarket.IOpenMarketSmsClient" />
public class OpenMarketSmsClient : IOpenMarketSmsClient
{
    private readonly HttpClient httpClient;
    private readonly IHttpAccessor httpAccessor;

    /// <summary>Initializes a new instance of the <see cref="OpenMarketSmsClient"/> class.</summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <exception cref="System.ArgumentNullException">httpClient</exception>
    public OpenMarketSmsClient(HttpClient httpClient)
        : this(httpClient, new HttpAccessor())
    {
    }

    /// <summary>Initializes a new instance of the <see cref="OpenMarketSmsClient"/> class.</summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="httpAccessor">The HTTP accessor.</param>
    /// <exception cref="System.ArgumentNullException">httpClient</exception>
    internal OpenMarketSmsClient(HttpClient httpClient, IHttpAccessor httpAccessor)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        this.httpAccessor = httpAccessor ?? throw new ArgumentNullException(nameof(httpAccessor));
    }

    /// <summary>Sends the asynchronous.</summary>
    /// <param name="body">The body.</param>
    /// <param name="originator">The originator.</param>
    /// <param name="account">The account.</param>
    /// <param name="password">The password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">body</exception>
    public async Task<SmsSendResult> SendAsync(
        SmsSendRequest body,
        string originator,
        string account,
        string password,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(body);

        if (string.IsNullOrWhiteSpace(body.DestinationPhoneNumber))
        {
            throw new ArgumentNullException(nameof(body), "destination phone number is null");
        }

        if (string.IsNullOrWhiteSpace(originator))
        {
            throw new ArgumentNullException(nameof(originator));
        }

        if (string.IsNullOrWhiteSpace(account))
        {
            throw new ArgumentNullException(nameof(account));
        }

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        var urlBuilder_ = new StringBuilder();
        urlBuilder_.Append("sms/v4/mt");

        var client_ = this.httpClient;
        var disposeClient_ = false;
        try
        {
            using var request_ = new HttpRequestMessage();
            var xml = BuildSmsSendXml(body, originator);

            var content_ = new StringContent(xml);
            content_.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/xml; encoding='utf-8'");
            request_.Content = content_;
            request_.Method = new HttpMethod("POST");

            var basicCredentials = Encoding.UTF8.GetBytes($"{account}:{password}");
            request_.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(basicCredentials));

            var url_ = urlBuilder_.ToString();
            request_.RequestUri = new Uri(url_, UriKind.RelativeOrAbsolute);

            var response_ = await this.httpAccessor.SendAsync(client_, request_, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            var disposeResponse_ = true;

            try
            {
                var headers_ = Enumerable.ToDictionary(response_.Headers, h_ => h_.Key, h_ => h_.Value);
                if (response_.Content != null && response_.Content.Headers != null)
                {
                    foreach (var item_ in response_.Content.Headers)
                    {
                        headers_[item_.Key] = item_.Value;
                    }
                }

                response_.Headers.TryGetValues("X-Request-Id", out var requestIdValues);
                response_.Headers.TryGetValues("Location", out var locationValues);

                var status_ = (int)response_.StatusCode;
                if (status_ == 202)
                {
                    return new SmsSendResult
                    {
                        SendStatus = SmsSendStatus.Accepted,
                        SendStatusMessage = nameof(SmsSendStatus.Accepted),
                        DetailedMessage = string.Empty,
                        RequestId = requestIdValues?.FirstOrDefault(),
                        Location = locationValues?.FirstOrDefault()
                    };
                }
                else
                {
                    var detailedMessage = string.Empty;
                    try
                    {
                        detailedMessage = await response_.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                    }
                    catch { }

                    return new SmsSendResult
                    {
                        SendStatus = SmsSendStatus.Failed,
                        SendStatusMessage = response_?.StatusCode.ToString(),
                        DetailedMessage = detailedMessage,
                        RequestId = requestIdValues?.FirstOrDefault(),
                        Location = locationValues?.FirstOrDefault()
                    };
                }
            }
            finally
            {
                if (disposeResponse_)
                {
                    response_.Dispose();
                }
            }
        }
        finally
        {
            if (disposeClient_)
            {
                client_.Dispose();
            }
        }
    }

    internal static string BuildSmsSendXml(SmsSendRequest body, string originator)
    {
        var request = new StringBuilder();
        var writer = XmlWriter.Create(request, new XmlWriterSettings
        {
            OmitXmlDeclaration = true
        });

        // Create mobileTerminate (main) element and add two attributes
        writer.WriteStartElement("mobileTerminate");
        writer.WriteAttributeString("promotional", "false");         // Used for India

        // Create options element and four attributes
        writer.WriteStartElement("options");
        writer.WriteAttributeString("note1", body.Note1);             // 0 - 200 character field, returned in SMS reports
        writer.WriteAttributeString("note2", body.Note2);             // 0 - 200 character field, returned in SMS reports
        writer.WriteAttributeString("programId", body.ProgramId);      // Required only for short code messaging, ignored otherwise
        writer.WriteAttributeString("flash", body.Flash.ToString().ToLower());                      // Show message immediately, do not retain on phone - likely ignored
        writer.WriteEndElement();

        // Create destination element and add two attributes
        writer.WriteStartElement("destination");
        writer.WriteAttributeString("address", (body.DestinationCountryCode?.ToString() ?? "1") + body.DestinationPhoneNumber?.ToString());
        writer.WriteAttributeString("mobileOperatorId", (body.MobileOperatorId ?? "383"));             // Used to route outgoing traffic - 383 is AT&T, OM will look up so it's not 
        // really necessary, but it MAY be avoiding a lookup call.
        writer.WriteEndElement();

        // Create source element and add two attributes
        writer.WriteStartElement("source");
        writer.WriteAttributeString("address", originator);  // Setting origination number

        writer.WriteEndElement();

        var messageType = SmsMessageType.Text.ToString().ToLower();
        if (body.Type == SmsMessageType.HexEncodedText)
        {
            messageType = "hexEncodedText";
        }
        else if (body.Type == SmsMessageType.Binary)
        {
            messageType = "binary";
        }
        else if (body.Type == SmsMessageType.WrapPush)
        {
            messageType = "wapPush";
        }

        var validityPeriod = body.ValidityPeriod;
        if (validityPeriod > 259200)
        {
            validityPeriod = 259200;
        }

        // Create message element and add three attributes
        writer.WriteStartElement("message");
        writer.WriteAttributeString("type", messageType); // Required
        writer.WriteAttributeString("mlc", body.Mlc.ToString().ToLower()); // Specifies what action to take if the message is too long (reject, truncate, or segment)
        writer.WriteAttributeString("validityPeriod", validityPeriod.ToString()); // How many seconds to attempt to deliver the message

        // Create content element within message element and add the message value
        writer.WriteStartElement("content");
        writer.WriteValue(body.TextContent);
        writer.WriteEndElement();

        writer.WriteEndElement();

        writer.WriteEndElement(); // Close the mobileTerminate (main) element

        writer.Flush();
        return request.ToString();
    }
}
