namespace Cezzi.Data;

using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

/// <summary>
/// 
/// </summary>
public static class IDataExtensions
{
    /// <summary>The get UTC date time value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTime GetUTCDateTimeValueSafe(this IDataReader reader, string column) => GetUTCDateTimeValueSafe(reader, column, default);

    /// <summary>The get UTC date time value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <param name="defaultOverride">The default override.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTime GetUTCDateTimeValueSafe(this IDataReader reader, string column, DateTime defaultOverride)
    {
        var date = GetValueSafe(reader, column, defaultOverride);

        var utcDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, DateTimeKind.Utc);
        return utcDate;
    }

    /// <summary>The get UTC date time ffset value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTimeOffset GetUTCDateTimeOffsetValueSafe(this IDataReader reader, string column) => GetUTCDateTimeOffsetValueSafe(reader, column, default);

    /// <summary>The get UTC date time offset value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <param name="defaultOverride">The default override.</param>
    /// <returns>The <see cref="DateTime"/>.</returns>
    public static DateTimeOffset GetUTCDateTimeOffsetValueSafe(this IDataReader reader, string column, DateTimeOffset defaultOverride)
    {
        var date = GetValueSafe(reader, column, defaultOverride);

        var utcDate = new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Millisecond, TimeSpan.Zero);
        return utcDate;
    }

    /// <summary>The get value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <param name="defaultOverride">The default override.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The the value of the field.</returns>
    public static T GetValueSafe<T>(this IDataReader reader, string column, T defaultOverride)
    {
        var index = reader.GetOrdinal(column);
        return (reader[column] != DBNull.Value) ? (T)reader[column] : defaultOverride;
    }

    /// <summary>The get value safe.</summary>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The the value of the field.</returns>
    public static T GetValueSafe<T>(this IDataReader reader, string column) => reader.GetValueSafe<T>(column, default);

    /// <summary>Gets the enum.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="reader">The reader.</param>
    /// <param name="column">The column.</param>
    /// <returns></returns>
    public static T GetEnum<T>(this IDataReader reader, string column) where T : struct, IConvertible
    {
        if (!typeof(T).IsEnum)
        {
            throw new ArgumentException("T must be an enumerated type");
        }

        var index = reader.GetOrdinal(column);
        var value = reader[column];

        return int.TryParse(value.ToString(), out var i)
            ? (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), value))
            : (T)Enum.Parse(typeof(T), value.ToString());
    }

    /// <summary>
    /// Adds the output parameter.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddOutputParameter(this SqlCommand cmd, string paramName, DbType type)
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>
    /// Adds the output parameter.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddOutputParameter(this SqlCommand cmd, string paramName, DbType type, int size)
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Direction = ParameterDirection.Output;
        param.Size = size;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>
    /// Adds the return parameter.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddReturnParameter(this SqlCommand cmd, string paramName, DbType type)
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Direction = ParameterDirection.ReturnValue;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the XML parameter.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="xmlString">The XML string.</param>
    [Obsolete("Will be removed in the future")]
    public static void AddXmlParam(this IDbCommand cmd, string parameterName, string xmlString)
    {
        var sqlParam = new SqlParameter(parameterName, SqlDbType.Xml)
        {
            SqlValue = xmlString
        };
        cmd.Parameters.Add(sqlParam);
    }

    /// <summary>
    /// Adds the XML parameter.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="xml">The XML.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddXmlParameter(this SqlCommand cmd, string paramName, XElement xml)
    {
        var param = cmd.CreateParameter();
        param.DbType = DbType.Xml;
        param.ParameterName = paramName;
        param.Value = xml.ToString();
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the parameter.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddParameter(this SqlCommand cmd, string paramName, DbType type, object value)
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Value = value;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>Adds the parameter.</summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddParameter(this SqlCommand cmd, string paramName, DbType type, int size, object value)
    {
        var param = cmd.CreateParameter();
        param.DbType = type;
        param.ParameterName = paramName;
        param.Value = value;
        param.Size = size;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>
    /// Adds the parameter if.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="value">The value.</param>
    /// <param name="condition">The condition.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddParameterIf(this SqlCommand cmd, string paramName, DbType type, object value, Func<bool> condition)
    {
        if (condition())
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.ParameterName = paramName;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        return cmd;
    }

    /// <summary>
    /// Adds the parameter if.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="type">The type.</param>
    /// <param name="size">The size.</param>
    /// <param name="value">The value.</param>
    /// <param name="condition">The condition.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddParameterIf(this SqlCommand cmd, string paramName, DbType type, int size, object value, Func<bool> condition)
    {
        if (condition())
        {
            var param = cmd.CreateParameter();
            param.DbType = type;
            param.Size = size;
            param.ParameterName = paramName;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        return cmd;
    }

    /// <summary>
    /// Adds the structured parameter.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="paramName">Name of the parameter.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand AddStructuredParameter(this SqlCommand cmd, string paramName, DataTable value)
    {
        var param = cmd.CreateParameter();
        param.SqlDbType = SqlDbType.Structured;
        param.ParameterName = paramName;
        param.Value = value;
        cmd.Parameters.Add(param);
        return cmd;
    }

    /// <summary>
    /// Sets the stored procedure.
    /// </summary>
    /// <param name="cmd">The command.</param>
    /// <param name="commandText">The command text.</param>
    /// <returns></returns>
    [Obsolete("Will be removed in the future")]
    public static SqlCommand SetStoredProcedure(this SqlCommand cmd, string commandText)
    {
        cmd.CommandText = commandText;
        cmd.CommandType = CommandType.StoredProcedure;
        return cmd;
    }
}
