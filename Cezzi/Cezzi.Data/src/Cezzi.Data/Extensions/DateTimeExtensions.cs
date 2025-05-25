namespace Cezzi.Data.Extensions;

using System;
using System.Data.SqlTypes;

/// <summary>
/// 
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>To the SQL date.</summary>
    /// <param name="date">The date.</param>
    /// <returns></returns>
    public static string ToSqlDateFormat(this DateTime date) => date.ToString("yyyy-MM-dd");

    /// <summary>
    /// Determines whether [is valid SQL date].
    /// </summary>
    /// <param name="date">The date.</param>
    /// <returns>
    ///   <c>true</c> if [is valid SQL date] [the specified date]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsValidSqlDate(this DateTime date)
    {
        return
            date >= SqlDateTime.MinValue.Value &&
            date <= SqlDateTime.MaxValue.Value;

    }
}
