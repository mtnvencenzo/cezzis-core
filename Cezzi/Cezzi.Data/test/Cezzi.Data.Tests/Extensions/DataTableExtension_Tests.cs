namespace Cezzi.Data.Tests.Extensions;

using Cezzi.Data.Extensions;
using FluentAssertions;
using System.Data;
using Xunit;

public class DataTableExtension_Tests
{
    [Fact]
    public void datatableext___returns_null_on_null_table()
    {
        DataTable table = null;

        table.ClearRows().Should().BeNull();
    }

    [Fact]
    public void datatableext___returns_when_no_rows_exist()
    {
        var table = new DataTable();
        table.Columns.Add("Test");

        table.ClearRows().Should().BeSameAs(table);
        table.Rows.Count.Should().Be(0);
    }

    [Fact]
    public void datatableext___clears_rows()
    {
        var table = new DataTable();
        table.Columns.Add("Test");
        table.Rows.Add("1");
        table.Rows.Add("2");
        table.Rows.Count.Should().Be(2);

        var cleared = table.ClearRows();

        cleared.Should().BeSameAs(table);
        cleared.Rows.Count.Should().Be(0);
        table.Rows.Count.Should().Be(0);
    }
}
