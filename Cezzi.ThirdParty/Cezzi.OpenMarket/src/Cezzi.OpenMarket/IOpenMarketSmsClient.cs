namespace Cezzi.OpenMarket;

using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface IOpenMarketSmsClient
{
    /// <summary>Sends the asynchronous.</summary>
    /// <param name="body">The body.</param>
    /// <param name="originator">The originator.</param>
    /// <param name="account">The account.</param>
    /// <param name="password">The password.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<SmsSendResult> SendAsync(
        SmsSendRequest body,
        string originator,
        string account,
        string password,
        CancellationToken cancellationToken = default);
}
