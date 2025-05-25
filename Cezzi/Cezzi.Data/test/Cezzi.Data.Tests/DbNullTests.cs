namespace Cezzi.Data.Tests;

using FluentAssertions;
using System;
using Xunit;

public class DbNullTests
{
    [Fact]
    public void dbnull___returns_dbnull_for_null()
    {
        var result = (null as object).GetDBNullIfNullOrEmpty();
        result.Should().Be(DBNull.Value);
    }

    [Fact]
    public void dbnull___does_not_return_dbnull_for_non_null()
    {
        var obj = new object();

        var result = obj.GetDBNullIfNullOrEmpty();
        result.Should().Be(obj);
    }

    [Fact]
    public void dbnull___does_not_return_dbnull_for_non_null_guid()
    {
        var obj = Guid.NewGuid();

        var result = obj.GetDBNullIfNullOrEmpty();
        result.Should().Be(obj);
    }

    [Fact]
    public void dbnull___returns_dbnull_for_default_guid()
    {
        var obj = Guid.Empty;

        var result = obj.GetDBNullIfNullOrEmpty();
        result.Should().Be(DBNull.Value);
    }

    [Fact]
    public void dbnull___returns_dbnull_for_NULL_string()
    {
        var obj = "NULL";

        var result = obj.GetDBNullIfNullOrEmpty();
        result.Should().Be(DBNull.Value);
    }
}
