namespace Cezzi.Http;

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IHttpClientSender
{
    /// <summary>Sends the specified HTTP client.</summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<HttpResponseMessage> Send(
        HttpClient httpClient,
        HttpRequestMessage request,
        CancellationToken cancellationToken = default);
}
