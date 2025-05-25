namespace Cezzi.Applications.Extensions;

using System;
using System.Data.SqlTypes;

/// <summary>
/// 
/// </summary>
public static class DateTimeExtensions
{
    private const string EstZoneId = "Eastern Standard Time";
    private static TimeZoneInfo ZoneInfo { get; set; }

    /// <summary>
    /// Gets the current date time in local time zone.
    /// </summary>
    /// <returns></returns>
    public static DateTime GetCurrentDateTimeInLocalTimeZone()
    {
        ZoneInfo ??= TimeZoneInfo.FindSystemTimeZoneById(EstZoneId);

        return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, ZoneInfo);
    }

    /// <summary>To the unix time.</summary>
    /// <param name="date">The date.</param>
    /// <returns></returns>
    public static long ToUnixTime(this DateTime date)
    {
        var utcDate = (date.Kind == DateTimeKind.Utc)
            ? date
            : date.ToUniversalTime();

        return utcDate.Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerSecond;
    }

    /// <summary>Froms the unix time.</summary>
    /// <param name="epoch">The epoch.</param>
    /// <returns></returns>
    public static DateTime FromUnixTime(this long epoch)
    {
        var utcDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        return utcDate.AddSeconds(epoch);
    }

    /// <summary>Determines whether [is valid SQL date].</summary>
    /// <param name="date">The date.</param>
    /// <returns><c>true</c> if [is valid SQL date] [the specified date]; otherwise, <c>false</c>.</returns>
    public static bool IsValidSqlDate(this DateTime date)
    {
        return
            date >= SqlDateTime.MinValue.Value &&
            date <= SqlDateTime.MaxValue.Value;
    }

    /// <summary>To the SQL date.</summary>
    /// <param name="date">The date.</param>
    /// <returns></returns>
    public static string ToSqlDate(this DateTime date) => date.ToString("yyyy-MM-dd");
}
