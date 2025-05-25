namespace Cezzi.Security.Identity.Tokens.Jwt;

using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// 
/// </summary>
public static class JwtClaimTypeExtensionMethods
{
    private static Dictionary<JwtClaimType, bool> encryptedValues;

    /// <summary>Determines whether this instance is encrypted.</summary>
    /// <param name="type">The type.</param>
    /// <returns><c>true</c> if the specified type is encrypted; otherwise, <c>false</c>.</returns>
    public static bool IsEncrypted(this JwtClaimType type)
    {
        if (encryptedValues == null)
        {
            var isEncryptedValues = new Dictionary<JwtClaimType, bool>();

            var fields = typeof(JwtClaimType).GetFields(BindingFlags.Public | BindingFlags.Static);

            foreach (var field in fields)
            {
                isEncryptedValues.Add(
                    key: (JwtClaimType)field.GetValue(null),
                    value: field.IsDefined(typeof(RequireEncryptionAttribute)));
            }

            encryptedValues = isEncryptedValues;
        }

        return encryptedValues[type];
    }
}
