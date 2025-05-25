namespace Cezzi.Applications.Extensions;

using System;

/// <summary>
/// 
/// </summary>
public static class DecimalExtensions
{
    /// <summary>Determines whether the specified command is negative.</summary>
    /// <param name="d">The command.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public static bool IsNegative(this decimal d) => Math.Abs(d) > d;

    /// <summary>Truncates the decimal to hundredths.</summary>
    /// <param name="d">The d.</param>
    /// <returns>The <see cref="decimal"/>.</returns>
    public static decimal TruncateDecimalToHundreths(this decimal d) => Math.Truncate(d * 100) / 100;

}
