namespace Cezzi.Applications.Text;

using System;

/// <summary>
/// 
/// </summary>
public static class Base64
{
    /// <summary>Encodes the specified bytes.</summary>
    /// <param name="bytes">The bytes.</param>
    /// <returns></returns>
    public static string Encode(byte[] bytes) => Convert.ToBase64String(bytes);

    /// <summary>Decodes the specified base64.</summary>
    /// <param name="base64">The base64.</param>
    /// <returns></returns>
    public static byte[] Decode(string base64) => Convert.FromBase64String(base64);
}
