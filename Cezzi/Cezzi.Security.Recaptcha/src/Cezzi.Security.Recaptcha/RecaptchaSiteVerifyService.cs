namespace Cezzi.Security.Recaptcha;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public class RecaptchaSiteVerifyService(IHttpClientFactory httpClientFactory) : IRecaptchaSiteVerifyService
{
    /// <summary>The HTTP client name</summary>
    public const string HttpClientName = "Cezzi_Recaptcha_Http_Client";

    private readonly IHttpClientFactory httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

    /// <summary>Verifies the specified verification code.</summary>
    /// <param name="verificationCode">The verification code.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="userIp">The user ip.</param>
    /// <returns></returns>
    public async virtual Task<RecaptchaVerificationResult> Verify(string verificationCode, RecaptchaConfig config, string userIp = null)
    {
        var recaptchaVerifyUrl = string.IsNullOrWhiteSpace(config?.SiteVerifyUrl)
            ? "https://www.google.com/recaptcha/api/siteverify"
            : config.SiteVerifyUrl ?? string.Empty;

        var status = RecaptchaVerificationStatus.NotAttempted;
        var resultCodes = new List<RecaptchaVerificationCode>();

        var data = new List<KeyValuePair<string, string>>
        {
            new("secret", config?.SiteSecret ?? string.Empty),
            new("response", verificationCode ?? string.Empty)
        };

        if (!string.IsNullOrWhiteSpace(userIp))
        {
            data.Add(new KeyValuePair<string, string>("remoteip", userIp));
        }

        HttpResponseMessage response = null;
        JsonObject jsonResponse = null;

        try
        {
            response = await this.InternalPostVerify(recaptchaVerifyUrl, data).ConfigureAwait(false);
        }
        catch (Exception)
        {
            resultCodes.Add(RecaptchaVerificationCode.CommunicationError);
        }

        if (response?.Content != null)
        {
            var rawResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            if ((rawResponse ?? string.Empty).StartsWith('{'))
            {
                jsonResponse = JsonObject.Parse(rawResponse).AsObject();
            }
            else
            {
                resultCodes.Add(RecaptchaVerificationCode.CommunicationError);
            }
        }
        else
        {
            resultCodes.Add(RecaptchaVerificationCode.CommunicationError);
        }

        DateTime? challengeTS = null;
        decimal? score = null;
        string hostname = null;
        var errorCodes = new List<string>();

        if (jsonResponse != null)
        {
            if (jsonResponse["success"] != null && bool.TryParse(jsonResponse["success"].ToString(), out var _success))
            {
                status = _success ? RecaptchaVerificationStatus.Success : RecaptchaVerificationStatus.Failed;
            }

            if (jsonResponse["challenge_ts"] != null && DateTime.TryParse(jsonResponse["challenge_ts"].ToString(), out var _challengeTS))
            {
                challengeTS = _challengeTS;
            }

            if (jsonResponse["score"] != null && decimal.TryParse(jsonResponse["score"].ToString(), out var _score))
            {
                score = _score;
            }

            if (jsonResponse["hostname"] != null)
            {
                hostname = jsonResponse["hostname"].ToString();
            }

            if (jsonResponse["error-codes"] != null && jsonResponse["error-codes"].GetValueKind() == JsonValueKind.Array)
            {
                var array = jsonResponse["error-codes"].AsArray();
                if (array != null && array.Count > 0)
                {
                    errorCodes.AddRange(array.GetValues<string>());
                }
            }
        }

        if (errorCodes.Contains("missing-input-secret"))
        {
            resultCodes.Add(RecaptchaVerificationCode.SecretRequired);
        }

        if (errorCodes.Contains("invalid-input-secret"))
        {
            resultCodes.Add(RecaptchaVerificationCode.SecretInvalid);
        }

        if (errorCodes.Contains("missing-input-response"))
        {
            resultCodes.Add(RecaptchaVerificationCode.VerificationCodeRequired);
        }

        if (errorCodes.Contains("invalid-input-response"))
        {
            resultCodes.Add(RecaptchaVerificationCode.VerificationCodeInvalid);
        }

        if (errorCodes.Contains("bad-request"))
        {
            resultCodes.Add(RecaptchaVerificationCode.InvalidRequest);
        }

        if (errorCodes.Contains("timeout-or-duplicate"))
        {
            resultCodes.Add(RecaptchaVerificationCode.Timeout);
        }

        if (errorCodes.Count > 0 && resultCodes.Count == 0)
        {
            resultCodes.Add(RecaptchaVerificationCode.CommunicationError);
        }

        return new RecaptchaVerificationResult
        {
            VerificationStatus = status,
            Hostname = hostname,
            Score = score,
            UtcVerificationTimestamp = challengeTS,
            ReturnCodes = resultCodes
        };
    }

    private async Task<HttpResponseMessage> InternalPostVerify(string uri, List<KeyValuePair<string, string>> data)
    {
        using var client = this.httpClientFactory.CreateClient(HttpClientName);

        var response = await client.PostAsync(uri, new FormUrlEncodedContent(data)).ConfigureAwait(false);

        return response;
    }
}
