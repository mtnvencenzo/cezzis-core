namespace Cezzi.OpenApi;

using System;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <seealso cref="System.Attribute" />
/// <remarks>Initializes a new instance of the <see cref="OpenApiEnumAttribute" /> class.</remarks>
/// <param name="enumType">Type of the enum.</param>
/// <param name="exclude">The exclude.</param>
[AttributeUsage(AttributeTargets.Property)]
public class OpenApiEnumAttribute(Type enumType, params object[] exclude) : Attribute
{

    /// <summary>Gets the type of the enum.</summary>
    /// <value>The type of the enum.</value>
    public Type EnumType { get; } = enumType;

    /// <summary>Gets the exclude.</summary>
    /// <value>The exclude.</value>
    public string[] Exclude { get; } = [.. (exclude ?? [])
            .Where(o => o != null)
            .Select(o => o.ToString())];
}
