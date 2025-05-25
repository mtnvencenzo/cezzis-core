namespace Cezzi.Security.Identity.Tokens;
/// <summary>
/// 
/// </summary>
public interface ITokenScopeValidationParameterResolver
{
    /// <summary>
    /// Gets the token validation parameters.
    /// </summary>
    /// <param name="scope">The scope.</param>
    /// <param name="subscope">The subscope.</param>
    /// <returns></returns>
    TokenResourceServerValidationParameters GetTokenValidationParameters(string scope, string subscope);
}
