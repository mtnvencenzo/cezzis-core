namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System.Data;
using Xunit;

public class DataTableExtensions_Tests
{
    [Fact]
    public void datatableextensions___addcolumn_without_type_for_null_table() => (null as DataTable).AddColumn("col1").Should().BeNull();

    [Fact]
    public void datatableextensions___addcolumn_without_type()
    {
        var table = new DataTable();
        table.AddColumn("col1").Should().BeSameAs(table);
        table.Columns.Should().ContainSingle();

        var column = table.Columns["col1"];
        column.Should().NotBeNull();
        (column.DataType == typeof(string)).Should().BeTrue();
    }

    [Fact]
    public void datatableextensions___addcolumn_with_type()
    {
        var table = new DataTable();
        table.AddColumn("col1", typeof(int)).Should().BeSameAs(table);
        table.Columns.Should().ContainSingle();

        var column = table.Columns["col1"];
        column.Should().NotBeNull();
        (column.DataType == typeof(int)).Should().BeTrue();
    }
}