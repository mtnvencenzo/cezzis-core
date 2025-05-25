namespace Cezzi.Applications.Extensions;

using System.Reflection;

/// <summary>
/// 
/// </summary>
public static class ReflectionExtensions
{
    /// <summary>Determines whether this instance is override.</summary>
    /// <param name="methodInfo">The method information.</param>
    /// <returns>
    ///   <c>true</c> if the specified method information is override; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOverride(this MethodInfo methodInfo) => methodInfo != null && methodInfo.GetBaseDefinition() != methodInfo;

    /// <summary>Determines whether this instance is override.</summary>
    /// <param name="propInfo">The property information.</param>
    /// <returns>
    ///   <c>true</c> if the specified property information is override; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsOverride(this PropertyInfo propInfo) => propInfo != null && IsOverride(propInfo.GetGetMethod(false));
}
