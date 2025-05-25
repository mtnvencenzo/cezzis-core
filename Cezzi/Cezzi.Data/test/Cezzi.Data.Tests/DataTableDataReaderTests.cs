namespace Cezzi.Data.Tests;

using FluentAssertions;
using System;
using System.Data;
using Xunit;

public class DataTableDataReaderTests
{
    [Fact]
    public void datatablereader___throws_on_null_datatable()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new DataTableDataReader(table: null));

        ex.ParamName.Should().Be("table");
    }

    [Fact]
    public void datatablereader___throws_on_null_datatables()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new DataTableDataReader(tables: null));

        ex.ParamName.Should().Be("tables");
    }

    [Fact]
    public void datatablereader___reads_nultiple_tables()
    {
        var value1 = GuidString();
        var value2 = GuidString();
        var value3 = GuidString();

        var table1 = new DataTable();
        table1.Columns.Add("test1");
        table1.Rows.Add([value1]);

        var table2 = new DataTable();
        table2.Columns.Add("test2");
        table2.Rows.Add([value2]);

        var table3 = new DataTable();
        table3.Columns.Add("test3");
        table3.Rows.Add([value3]);

        var r = new DataTableDataReader([table1, table2, table3]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            safeReader.Read();
            safeReader["test1"].Should().Be(value1);
            safeReader[0].Should().Be(value1);

            safeReader.NextResult();
            safeReader.Read();
            safeReader["test2"].Should().Be(value2);
            safeReader[0].Should().Be(value2);

            safeReader.NextResult();
            safeReader.Read();
            safeReader["test3"].Should().Be(value3);
            safeReader[0].Should().Be(value3);

            safeReader.NextResult().Should().BeFalse();
            safeReader.Read().Should().BeFalse();
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void datatablereader___reads_single_tables()
    {
        var value1 = GuidString();

        var table1 = new DataTable();
        table1.Columns.Add("test1");
        table1.Rows.Add([value1]);

        var r = new DataTableDataReader(table1);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            safeReader.Read();
            safeReader["test1"].Should().Be(value1);
            safeReader[0].Should().Be(value1);

            safeReader.NextResult().Should().BeFalse();
        }

        r.IsClosed.Should().BeTrue();
    }

    private static string GuidString() => Guid.NewGuid().ToString();
}