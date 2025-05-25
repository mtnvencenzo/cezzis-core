namespace Cezzi.Applications.Extensions;

/// <summary>
/// 
/// </summary>
public static class LongExtensions
{
    /// <summary>Firsts the greater than zero.</summary>
    /// <param name="longs">The longs.</param>
    /// <returns></returns>
    public static long FirstGreaterThanZero(params long?[] longs)
    {
        if (longs == null)
        {
            return 0;
        }

        foreach (var l in longs)
        {
            if (l > 0)
            {
                return l.Value;
            }
        }

        return 0;
    }

    /// <summary>Determines whether the specified start is between.</summary>
    /// <param name="item">The item.</param>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    /// <returns><c>true</c> if the specified start is between; otherwise, <c>false</c>.</returns>
    public static bool IsBetween(this long item, long start, long end) => item >= start && item <= end;
}
