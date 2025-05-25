namespace Cezzi.Data;

using System;

/// <summary>
/// 
/// </summary>
public static class DbNull
{
    /// <summary>Gets the database null if null or empty.</summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    public static object GetDBNullIfNullOrEmpty(this object obj)
    {
        if (obj == null)
        {
            return DBNull.Value;
        }

        if (obj is Guid guid)
        {
            return guid == Guid.Empty ? DBNull.Value : obj;
        }

        // This is lame, think about removing this...
        return obj.ToString().ToUpper() == "NULL" ? DBNull.Value : obj.ToString() == string.Empty ? DBNull.Value : obj;
    }
}
