namespace Cezzi.Data.Extensions;

using System.Data;

/// <summary>
/// 
/// </summary>
public static class DataTableExtensions
{
    /// <summary>Clears the rows.</summary>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    public static DataTable ClearRows(this DataTable table)
    {
        if (table == null)
        {
            return null;
        }

        table.Rows.Clear();
        return table;
    }
}
