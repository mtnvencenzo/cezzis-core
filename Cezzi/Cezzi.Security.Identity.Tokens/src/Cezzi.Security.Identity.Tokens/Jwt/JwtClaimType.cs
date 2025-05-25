namespace Cezzi.Security.Identity.Tokens.Jwt;

/// <summary>
/// 
/// </summary>
public enum JwtClaimType
{
    /// <summary>The none</summary>
    None = 0,

    /// <summary>The client document no</summary>
    [RequireEncryption]
    clientdocno = 1,

    /// <summary>The pubkey</summary>
    [RequireEncryption]
    pubkey = 2,

    /// <summary>The email</summary>
    [RequireEncryption]
    email = 3,

    /// <summary>The featureid</summary>
    feature = 4,

    /// <summary>The jwt token id [required]</summary>
    jti = 5,

    /// <summary>issued at</summary>
    iat = 6,

    /// <summary>The chapid</summary>
    chapid = 7,

    /// <summary>The natid</summary>
    natid = 8,

    /// <summary>The userid</summary>
    userid = 9,

    /// <summary>The adminid</summary>
    adminid = 10,

    /// <summary>The usertype</summary>
    usertype = 11,

    /// <summary>
    /// The preauthenticateduser
    /// </summary>
    preauthenticateduser = 12,

    /// <summary>The oauthlinks</summary>
    oauthresult = 13,

    /// <summary>The groupid</summary>
    groupid = 14,

    /// <summary>
    /// The scope
    /// </summary>
    scope = 15,

    /// <summary>
    /// The subscope
    /// </summary>
    subscope = 16
}
