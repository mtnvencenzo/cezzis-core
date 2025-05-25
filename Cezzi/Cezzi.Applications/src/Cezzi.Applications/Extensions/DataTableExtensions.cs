namespace Cezzi.Applications.Extensions;

using System;
using System.Data;

/// <summary>
/// 
/// </summary>
public static class DataTableExtensions
{
    /// <summary>
    /// Adds the column.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns></returns>
    public static DataTable AddColumn(this DataTable table, string columnName)
    {
        if (table == null)
        {
            return null;
        }

        table.Columns.Add(columnName);
        return table;
    }

    /// <summary>
    /// Adds the column.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <param name="columnType">Type of the column.</param>
    /// <returns></returns>
    public static DataTable AddColumn(this DataTable table, string columnName, Type columnType)
    {
        if (table == null)
        {
            return null;
        }

        table.Columns.Add(columnName, columnType);
        return table;
    }
}
