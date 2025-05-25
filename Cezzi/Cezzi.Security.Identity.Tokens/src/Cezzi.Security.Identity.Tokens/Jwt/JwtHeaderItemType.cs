namespace Cezzi.Security.Identity.Tokens.Jwt;

/// <summary>
/// 
/// </summary>
public enum JwtHeaderItemType
{
    /// <summary>The none</summary>
    None = 0,

    /// <summary>The audience</summary>
    Audience = 1,

    /// <summary>The expires</summary>
    Expires = 2,

    /// <summary>The issuer</summary>
    Issuer = 3,
}
