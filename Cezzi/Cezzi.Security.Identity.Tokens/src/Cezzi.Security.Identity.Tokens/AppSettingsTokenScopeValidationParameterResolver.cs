namespace Cezzi.Security.Identity.Tokens;
using Cezzi.Security.Identity.Tokens.Jwt;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="ITokenScopeValidationParameterResolver" />
/// <remarks>
/// Initializes a new instance of the <see cref="AppSettingsTokenScopeValidationParameterResolver"/> class.
/// </remarks>
/// <param name="appSetting">The application setting.</param>
public class AppSettingsTokenScopeValidationParameterResolver(string appSetting) : ITokenScopeValidationParameterResolver
{
    /// <summary>
    /// Gets the scope parameters.
    /// </summary>
    /// <value>
    /// The scope parameters.
    /// </value>
    protected List<TokenResourceServerValidationParameters> ScopeParameters { get; private set; } = ParseAppSetting(appSetting);

    /// <summary>
    /// Gets the token validation parameters.
    /// </summary>
    /// <param name="scope">The scope.</param>
    /// <param name="subscope">The subscope.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public TokenResourceServerValidationParameters GetTokenValidationParameters(string scope, string subscope) => this.ScopeParameters.FirstOrDefault(i => string.Compare(i.Scope, scope, true) == 0 && string.Compare(i.SubScope, subscope, true) == 0);

    private static List<TokenResourceServerValidationParameters> ParseAppSetting(string appSetting)
    {
        var items = new List<TokenResourceServerValidationParameters>();

        if (string.IsNullOrWhiteSpace(appSetting))
        {
            return items;
        }

        // scope,subscope,algtype,sharedKey,audience,issuer||scope,subscope,algtype,sharedKey,audience,issuer
        var tokenParams = appSetting.Split(["||"], StringSplitOptions.RemoveEmptyEntries);

        foreach (var tokenParam in tokenParams)
        {
            var tokenSettings = tokenParam.Split(["@@"], StringSplitOptions.RemoveEmptyEntries);

            // scope,subscope,algtype,sharedKey,audience,issuer
            if (tokenSettings.Length == 6)
            {
                var scope = tokenSettings[0];
                var subscope = tokenSettings[1];
                var alg = Utilities.AsEnum<JwtAlgorithmType>(tokenSettings[2], true);
                var key = tokenSettings[3];
                var aud = tokenSettings[4];
                var issuer = tokenSettings[5];

                if (alg == JwtAlgorithmType.None)
                {
                    continue;
                }

                if (items.FirstOrDefault(i => string.Compare(i.Scope, scope, true) == 0 && string.Compare(i.SubScope, subscope, true) == 0) == null)
                {
                    items.Add(new TokenResourceServerValidationParameters
                    {
                        Scope = scope,
                        SubScope = subscope,
                        AlgorithmType = alg,
                        Audience = aud,
                        Issuer = issuer,
                        SharedKey = key
                    });
                }
            }
        }

        return items;
    }
}
