namespace Cezzi.Security.Identity.Tokens;

using System;

/// <summary>
/// 
/// </summary>
[Serializable]
public class TokenClaim
{
    /// <summary>Gets or sets the type.</summary>
    /// <value>The type.</value>
    public string Name { get; set; }

    /// <summary>Gets or sets the value.</summary>
    /// <value>The value.</value>
    public string Value { get; set; }
}
