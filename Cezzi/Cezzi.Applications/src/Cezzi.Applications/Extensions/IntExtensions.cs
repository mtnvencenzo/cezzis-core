namespace Cezzi.Applications.Extensions;

using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public static class IntExtensions
{
    /// <summary>
    /// Determines whether the specified start is between.
    /// </summary>
    /// <param name="item">The item.</param>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    /// <returns>
    ///   <c>true</c> if the specified start is between; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBetween(this int item, int start, int end) => item >= start && item <= end;

    /// <summary>Determines whether the specified command is negative.</summary>
    /// <param name="d">The command.</param>
    /// <returns>The <see cref="bool"/>.</returns>
    public static bool IsNegative(this decimal d) => Math.Abs(d) > d;

    /// <summary>
    /// Maximums the or zero.
    /// </summary>
    /// <typeparam name="TSource">The type of the source.</typeparam>
    /// <param name="source">The source.</param>
    /// <param name="selector">The selector.</param>
    /// <returns></returns>
    public static int MaxOrZero<TSource>(this IEnumerable<TSource> source, Func<TSource, int> selector)
    {
        var max = 0;

        foreach (var item in source.Select(i => selector(i)))
        {
            if (item > max)
            {
                max = item;
            }
        }

        return max;
    }

    /// <summary>Truncates the decimal to hundredths.</summary>
    /// <param name="d">The d.</param>
    /// <returns>The <see cref="decimal"/>.</returns>
    public static decimal TruncateDecimalToHundreths(this decimal d) => Math.Truncate(d * 100) / 100;

    /// <summary>Determines whether the specified lower is between.</summary>
    /// <param name="inVal">The in value.</param>
    /// <param name="lower">The lower.</param>
    /// <param name="upper">The upper.</param>
    /// <returns>
    ///   <c>true</c> if the specified lower is between; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsBetween(this int? inVal, int lower, int upper) => (inVal ?? 0) >= lower && (inVal ?? 0) <= upper;
}
