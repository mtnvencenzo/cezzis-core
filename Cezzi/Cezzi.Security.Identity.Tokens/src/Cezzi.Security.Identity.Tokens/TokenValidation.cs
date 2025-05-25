namespace Cezzi.Security.Identity.Tokens;

using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class TokenValidation
{
    /// <summary>
    /// Gets or sets a value indicating whether this instance is authenticated.
    /// </summary>
    /// <value>
    /// <c>true</c> if this instance is authenticated; otherwise, <c>false</c>.
    /// </value>
    public bool IsAuthenticated { get; set; }

    /// <summary>Gets or sets the claims.</summary>
    /// <value>The claims.</value>
    public List<TokenClaim> Claims { get; set; }

    /// <summary>Gets or sets the reason.</summary>
    /// <value>The reason.</value>
    public string Reason { get; set; }
}
