namespace Cezzi.Security.Identity.Tokens.Jwt;

/// <summary>
/// 
/// </summary>
public enum JwtAlgorithmType
{
    /// <summary>The none</summary>
    None = 0,

    /// <summary>The hmac 256</summary>
    HMAC_256 = 1,

    /// <summary>The hmac HMAC_384</summary>
    HMAC_384 = 2,

    /// <summary>The hmac 512</summary>
    HMAC_512 = 3
}
