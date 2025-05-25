namespace Cezzi.Security.Identity.Tokens;

using Cezzi.Security.Identity.Tokens.Jwt;
using System;

/// <summary>
/// 
/// </summary>
[Serializable]
public class TokenResourceServerValidationParameters
{
    /// <summary>
    /// Gets or sets the scope.
    /// </summary>
    /// <value>
    /// The scope.
    /// </value>
    public string Scope { get; set; }

    /// <summary>
    /// Gets or sets the sub scope.
    /// </summary>
    /// <value>
    /// The sub scope.
    /// </value>
    public string SubScope { get; set; }

    /// <summary>
    /// Gets or sets the shared key.
    /// </summary>
    /// <value>
    /// The shared key.
    /// </value>
    public string SharedKey { get; set; }

    /// <summary>
    /// Gets or sets the type of the algorithm.
    /// </summary>
    /// <value>
    /// The type of the algorithm.
    /// </value>
    public JwtAlgorithmType AlgorithmType { get; set; }

    /// <summary>
    /// Gets or sets the audience.
    /// </summary>
    /// <value>
    /// The audience.
    /// </value>
    public string Audience { get; set; }

    /// <summary>
    /// Gets or sets the issuer.
    /// </summary>
    /// <value>
    /// The issuer.
    /// </value>
    public string Issuer { get; set; }
}
