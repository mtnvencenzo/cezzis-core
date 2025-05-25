namespace Cezzi.Security.Identity.Tokens.Jwt;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="JwtParameters" /> class.
/// </remarks>
/// <param name="key">The key.</param>
/// <param name="signatureType">Type of the signature.</param>
public class JwtParameters(string key, JwtAlgorithmType signatureType)
{
    /// <summary>Gets or sets the JWT identifier.</summary>
    /// <value>The JWT identifier.</value>
    public string JwtId { get; set; }

    /// <summary>Gets the header items.</summary>
    /// <value>The header items.</value>
    public IDictionary<JwtHeaderItemType, string> HeaderItems { get; } = new Dictionary<JwtHeaderItemType, string>();

    /// <summary>Gets the claims.</summary>
    /// <value>The claims.</value>
    public IDictionary<string, string> Claims { get; } = new Dictionary<string, string>();

    /// <summary>Gets or sets the signature key.</summary>
    /// <value>The signature key.</value>
    public string SignatureKey { get; } = key;

    /// <summary>Gets or sets the type of the signature.</summary>
    /// <value>The type of the signature.</value>
    public JwtAlgorithmType SignatureAlgorithm { get; } = signatureType;
}
