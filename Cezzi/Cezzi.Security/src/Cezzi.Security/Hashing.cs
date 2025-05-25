namespace Cezzi.Security;

using System.Security.Cryptography;
using System.Text;

/// <summary>
/// 
/// </summary>
public static class Hashing
{
    /// <summary>Generates the hmacsh a512.</summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="toSign">To sign.</param>
    /// <returns></returns>
    public static byte[] GenerateHMACSHA512(string privateKey, string toSign)
    {
        var s = Encoding.UTF8.GetBytes(toSign);

        return GenerateHMACSHA512(privateKey, s);
    }

    /// <summary>Generates the hmacsh a512.</summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="toSign">To sign.</param>
    /// <returns></returns>
    public static byte[] GenerateHMACSHA512(string privateKey, byte[] toSign)
    {
        var key = Encoding.UTF8.GetBytes(privateKey);

        // Initialize the keyed hash object. 
        using var hmac = new HMACSHA512(key);
        // Compute the hash of the input string 
        return hmac.ComputeHash(toSign);
    }

    /// <summary>Generates the hmacsh a384.</summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="toSign">To sign.</param>
    /// <returns></returns>
    public static byte[] GenerateHMACSHA384(string privateKey, string toSign)
    {
        var s = Encoding.UTF8.GetBytes(toSign);

        return GenerateHMACSHA384(privateKey, s);
    }

    /// <summary>Generates the hmacsh a384.</summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="toSign">To sign.</param>
    /// <returns></returns>
    public static byte[] GenerateHMACSHA384(string privateKey, byte[] toSign)
    {
        var key = Encoding.UTF8.GetBytes(privateKey);

        // Initialize the keyed hash object. 
        using var hmac = new HMACSHA384(key);

        // Compute the hash of the input string 
        return hmac.ComputeHash(toSign);
    }

    /// <summary>Generates the HMACSH a256.</summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="toSign">To sign.</param>
    /// <returns></returns>
    public static byte[] GenerateHMACSHA256(string privateKey, string toSign)
    {
        var s = Encoding.UTF8.GetBytes(toSign);

        return GenerateHMACSHA256(privateKey, s);
    }

    /// <summary>
    /// Generates the HMACSH a256.
    /// </summary>
    /// <param name="privateKey">The private key.</param>
    /// <param name="toSign">To sign.</param>
    /// <returns></returns>
    public static byte[] GenerateHMACSHA256(string privateKey, byte[] toSign)
    {
        var key = Encoding.UTF8.GetBytes(privateKey);

        // Initialize the keyed hash object. 
        using var hmac = new HMACSHA256(key);

        // Compute the hash of the input string 
        return hmac.ComputeHash(toSign);
    }
}
