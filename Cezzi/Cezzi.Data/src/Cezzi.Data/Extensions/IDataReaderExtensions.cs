namespace Cezzi.Data.Extensions;

using System;
using System.Data;

/// <summary>
/// 
/// </summary>
public static class IDataReaderExtensions
{
    /// <summary>Gets the UTC date time value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <returns></returns>
    public static DateTime GetUTCDateTimeValueSafe(this IDataReader reader, string column) => GetUTCDateTimeValueSafe(reader, column, default);

    /// <summary>Gets the UTC date time value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <param name="defaultOverride">The default override.</param>
    /// <returns></returns>
    public static DateTime GetUTCDateTimeValueSafe(this IDataReader reader, string column, DateTime defaultOverride)
    {
        var date = GetValueSafe(reader, column, defaultOverride);
        return new DateTime(date.Ticks, DateTimeKind.Utc);
    }

    /// <summary>Gets the value safe.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <param name="defaultOverride">The default override.</param>
    /// <returns></returns>
    public static T GetValueSafe<T>(this IDataReader reader, string column, T defaultOverride) => ((reader[column] ?? DBNull.Value) != DBNull.Value) ? (T)reader[column] : defaultOverride;

    /// <summary>Gets the value safe.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <returns></returns>
    public static T GetValueSafe<T>(this IDataReader reader, string column) => reader.GetValueSafe(column, default(T));
}
