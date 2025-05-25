namespace Cezzi.Data;

using System;
using System.Collections.Generic;
using System.Data;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Data.ObjectArrayDataReaderBase" />
public class DataTableDataReader : ObjectArrayDataReaderBase
{
    /// <summary>Initializes a new instance of the <see cref="DataTableDataReader"/> class.</summary>
    /// <param name="table">The table.</param>
    public DataTableDataReader(DataTable table)
    {
        _ = this.AddResult(table ?? throw new ArgumentNullException(nameof(table)));
    }

    /// <summary>Initializes a new instance of the <see cref="DataTableDataReader"/> class.</summary>
    /// <param name="tables">The tables.</param>
    public DataTableDataReader(IList<DataTable> tables)
    {
        foreach (var table in tables ?? throw new ArgumentNullException(nameof(tables)))
        {
            _ = this.AddResult(table ?? throw new ArgumentNullException(nameof(table)));
        }
    }

    /// <summary>Adds the result.</summary>
    /// <param name="table">The table.</param>
    /// <returns></returns>
    protected virtual DataTableDataReader AddResult(DataTable table)
    {
        var columnNames = new List<string>();
        foreach (DataColumn column in table.Columns)
        {
            columnNames.Add(column.ColumnName);
        }

        var datarows = new List<object[]>();
        foreach (DataRow row in table.Rows)
        {
            datarows.Add(row.ItemArray);
        }

        _ = this.AddResultInternal(
            columnNames: columnNames,
            datarows: datarows);

        return this;
    }
}
