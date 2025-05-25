namespace Cezzi.Security.Identity.Tokens.Jwt;

using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

/// <summary>
/// 
/// </summary>
public static class JwtHandler
{
    /// <summary>Generates this instance.</summary>
    /// <returns></returns>
    public static string GenerateNew(JwtParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        if (!parameters.HeaderItems.ContainsKey(JwtHeaderItemType.Audience) || string.IsNullOrWhiteSpace(parameters.HeaderItems[JwtHeaderItemType.Audience]))
        {
            throw new ApplicationException("Header items does not contain the 'Audience' key or value");
        }

        if (!parameters.HeaderItems.ContainsKey(JwtHeaderItemType.Expires) || string.IsNullOrWhiteSpace(parameters.HeaderItems[JwtHeaderItemType.Expires]))
        {
            throw new ApplicationException("Header items does not contain the 'Expires' key or value");
        }

        if (!parameters.HeaderItems.ContainsKey(JwtHeaderItemType.Issuer) || string.IsNullOrWhiteSpace(parameters.HeaderItems[JwtHeaderItemType.Issuer]))
        {
            throw new ApplicationException("Header items does not contain the 'Issuer' key or value");
        }

        if (!parameters.Claims.ContainsKey(JwtClaimType.jti.ToString()) || string.IsNullOrWhiteSpace(parameters.Claims[JwtClaimType.jti.ToString()]))
        {
            throw new ApplicationException("Claims does not contain the 'jti' key or value");
        }

        if (string.IsNullOrWhiteSpace(parameters.SignatureKey))
        {
            throw new ApplicationException("Parameters does not contain a value 'SignatureKey'");
        }

        if (parameters.SignatureAlgorithm == JwtAlgorithmType.None)
        {
            throw new ApplicationException("Parameters does not contain a value 'SignatureType'");
        }

        if (!DateTime.TryParse(parameters.HeaderItems[JwtHeaderItemType.Expires], out var expiresOn))
        {
            throw new ApplicationException("Parameters does not contain a valid 'Expires' value");
        }

        if (expiresOn.Kind != DateTimeKind.Utc)
        {
            expiresOn = expiresOn.ToUniversalTime();
        }

        var algorithm = parameters.SignatureAlgorithm switch
        {
            JwtAlgorithmType.HMAC_384 => SecurityAlgorithms.HmacSha384,
            JwtAlgorithmType.HMAC_512 => SecurityAlgorithms.HmacSha512,
            _ => SecurityAlgorithms.HmacSha256,
        };

        var keyBytes = System.Text.Encoding.UTF8.GetBytes(parameters.SignatureKey);
        if (keyBytes.Length < 128 && parameters.SignatureAlgorithm == JwtAlgorithmType.HMAC_256)
        {
            Array.Resize(ref keyBytes, 128);
        }

        ///////////////////////////////////////////////////////////////////
        // Create signing credentials for the signed JWT.
        // This object is used to cryptographically sign the JWT by the issuer.
        var sc = new SigningCredentials(
            new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(parameters.SignatureKey)),
            algorithm
        );

        ///////////////////////////////////////////////////////////////////
        // Create JWT handler
        // This object is used to write/sign/decode/validate JWTs
        var jwtHandler = new JwtSecurityTokenHandler();

        // Create a simple JWT claim set
        IList<Claim> payloadClaims = [];
        foreach (var key in parameters.Claims.Keys)
        {
            payloadClaims.Add(new Claim(key, parameters.Claims[key]));
        }

        if (!string.IsNullOrWhiteSpace(parameters.HeaderItems[JwtHeaderItemType.Audience]))
        {
            var audiences = parameters.HeaderItems[JwtHeaderItemType.Audience].Split([","], StringSplitOptions.RemoveEmptyEntries);

            foreach (var aud in audiences.Distinct().Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                payloadClaims.Add(new Claim("aud", aud.Trim()));
            }
        }

        // Create a JWT with signing credentials
        var jwt = new JwtSecurityToken(
            issuer: parameters.HeaderItems[JwtHeaderItemType.Issuer],
            claims: payloadClaims,
            expires: expiresOn,
            signingCredentials: sc
        );

        // Serialize the JWT
        // This is how our JWT looks on the wire: <Base64UrlEncoded header>.<Base64UrlEncoded body>.<signature>
        var jwtOnTheWire = jwtHandler.WriteToken(jwt);
        return jwtOnTheWire;
    }

    /// <summary>Validates the specified JWT on the wire.</summary>
    /// <param name="jwtOnTheWire">The JWT on the wire.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns></returns>
    public static TokenValidation Validate(string jwtOnTheWire, JwtParameters parameters)
    {
        ///////////////////////////////////////////////////////////////////
        // Create JWT handler
        // This object is used to write/sign/decode/validate JWTs
        var jwtHandler = new JwtSecurityTokenHandler();

        var validationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(parameters.SignatureKey)),
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = parameters.HeaderItems[JwtHeaderItemType.Issuer],
            ValidAudience = parameters.HeaderItems[JwtHeaderItemType.Audience],
        };

        SecurityToken secureToken;
        ClaimsPrincipal claimsPrincipal = null;

        try
        {
            claimsPrincipal = jwtHandler.ValidateToken(jwtOnTheWire, validationParameters, out secureToken);
        }
        catch (SecurityTokenInvalidLifetimeException)
        {
            return new TokenValidation
            {
                IsAuthenticated = false,
                Reason = "Token lifetime is not valid."
            };
        }
        catch (SecurityTokenExpiredException)
        {
            return new TokenValidation
            {
                IsAuthenticated = false,
                Reason = "Token is expired."
            };
        }
        catch (SecurityTokenInvalidAudienceException)
        {
            return new TokenValidation
            {
                IsAuthenticated = false,
                Reason = "Token audience is invalid."
            };
        }
        catch (SecurityTokenInvalidIssuerException)
        {
            return new TokenValidation
            {
                IsAuthenticated = false,
                Reason = "Token issuer is invalid."
            };
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            return new TokenValidation
            {
                IsAuthenticated = false,
                Reason = "Token signature is invalid."
            };
        }
        catch (SecurityTokenNoExpirationException)
        {
            return new TokenValidation
            {
                IsAuthenticated = false,
                Reason = "Token expiration is missing."
            };
        }

        var result = new TokenValidation
        {
            IsAuthenticated = claimsPrincipal?.Identity?.IsAuthenticated ?? false
        };

        // Echo back the claims to the caller that existed on the token
        // This is mainly being added because chapters needs the claim information
        // when calling Validate() against the token.  In a perfect world, clients
        // would be able to validate on their own but for now this is being handled here.
        if (result.IsAuthenticated)
        {
            var jwtToken = secureToken as JwtSecurityToken;

            if (jwtToken.Claims != null && jwtToken.Claims.Any())
            {
                result.Claims = [];
                jwtToken.Claims.ToList().ForEach(c =>
                {
                    result.Claims.Add(new TokenClaim
                    {
                        Name = c.Type,
                        Value = c.Value
                    });
                });
            }
        }

        return result;
    }

    /// <summary>Gets the token identifier.</summary>
    /// <param name="jwtOnTheWire">The JWT on the wire.</param>
    /// <returns></returns>
    public static string GetTokenId(string jwtOnTheWire)
    {
        ///////////////////////////////////////////////////////////////////
        // Create JWT handler
        // This object is used to write/sign/decode/validate JWTs
        var jwtHandler = new JwtSecurityTokenHandler();

        try
        {
            var secureToken = jwtHandler.ReadToken(jwtOnTheWire);
            return secureToken != null
                ? secureToken.Id ?? string.Empty
                : string.Empty;
        }
        catch { }

        return string.Empty;
    }

    /// <summary>Gets the claim value.</summary>
    /// <param name="jwtOnTheWire">The JWT on the wire.</param>
    /// <param name="claimName">Name of the claim.</param>
    /// <returns></returns>
    public static string GetClaimValue(string jwtOnTheWire, string claimName)
    {
        ///////////////////////////////////////////////////////////////////
        // Create JWT handler
        // This object is used to write/sign/decode/validate JWTs
        var jwtHandler = new JwtSecurityTokenHandler();

        try
        {
            var secureToken = jwtHandler.ReadToken(jwtOnTheWire);
            return secureToken != null
                ? ((JwtSecurityToken)secureToken).Claims.FirstOrDefault(t => t.Type == claimName)?.Value ?? string.Empty
                : string.Empty;
        }
        catch { }

        return string.Empty;
    }

    /// <summary>Gets the claim values.</summary>
    /// <param name="jwtOnTheWire">The JWT on the wire.</param>
    /// <param name="claimNames">The claim names.</param>
    /// <returns></returns>
    public static Dictionary<string, string> GetClaimValues(string jwtOnTheWire, params string[] claimNames)
    {
        var claims = new Dictionary<string, string>();

        // Seed the return claims
        foreach (var name in claimNames)
        {
            claims.Add(name, string.Empty);
        }

        ///////////////////////////////////////////////////////////////////
        // Create JWT handler
        // This object is used to write/sign/decode/validate JWTs
        var jwtHandler = new JwtSecurityTokenHandler();
        JwtSecurityToken secureToken = null;

        try
        {
            secureToken = jwtHandler.ReadToken(jwtOnTheWire) as JwtSecurityToken;
        }
        catch (Exception) { }

        if (secureToken != null)
        {
            foreach (var claimName in claimNames)
            {
                try
                {
                    claims[claimName] = secureToken.Claims.FirstOrDefault(t => t.Type == claimName)?.Value ?? string.Empty;
                }
                catch { }
            }
        }

        return claims;
    }

    /// <summary>Decodes the JWT token.</summary>
    /// <param name="jwt">The JWT.</param>
    public static JwtSecurityToken DecodeJwtToken(string jwt)
    {
        var jwtHandler = new JwtSecurityTokenHandler();
        return jwtHandler.ReadJwtToken(jwt);
    }
}
