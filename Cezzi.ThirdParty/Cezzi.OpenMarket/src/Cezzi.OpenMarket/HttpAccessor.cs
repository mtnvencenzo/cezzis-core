namespace Cezzi.OpenMarket;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.OpenMarket.IHttpAccessor" />
internal class HttpAccessor : IHttpAccessor
{
    /// <summary>Sends the asynchronous.</summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="request">The request.</param>
    /// <param name="completionOption">The completion option.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public async Task<HttpResponseMessage> SendAsync(
        HttpClient httpClient,
        HttpRequestMessage request,
        HttpCompletionOption completionOption,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.SendAsync(
            request: request,
            completionOption: completionOption,
            cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
