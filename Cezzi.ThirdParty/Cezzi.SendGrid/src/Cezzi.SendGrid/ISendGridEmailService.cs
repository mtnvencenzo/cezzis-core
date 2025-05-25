namespace Cezzi.SendGrid;

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 
/// </summary>
public interface ISendGridEmailService
{
    /// <summary>Sends the asynchronous.</summary>
    /// <param name="subject">The subject.</param>
    /// <param name="fromEmail">From email.</param>
    /// <param name="tos">The tos.</param>
    /// <param name="sendGridConfiguration">The send grid configuration.</param>
    /// <param name="htmlContent">Content of the HTML.</param>
    /// <param name="plainTextContent">Content of the plain text.</param>
    /// <param name="sendWithPriority">if set to <c>true</c> [send with priority].</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    Task<bool> SendAsync(
        string subject,
        EmailValue fromEmail,
        IList<EmailValue> tos,
        SendGridConfiguration sendGridConfiguration,
        string htmlContent = null,
        string plainTextContent = null,
        bool sendWithPriority = false,
        CancellationToken cancellationToken = default);
}
