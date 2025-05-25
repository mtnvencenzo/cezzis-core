namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class IListExtensions_Tests
{
    // AreAllNotNull
    // ----------------------------------

    [Fact]
    public void IlistExtensions___AreAllNotNull_null_items_throws()
    {
        Assert.Throws<ArgumentNullException>(
            paramName: "items",
            testCode: () => IListExtensions.AreAllNotNull(null));
    }

    [Fact]
    public void IlistExtensions___AreAllNotNull_empty_items_throws()
    {
        Assert.Throws<ArgumentException>(
            paramName: "items",
            testCode: () => IListExtensions.AreAllNotNull([]));
    }

    [Fact]
    public void IlistExtensions___AreAllNotNull_some_not_nulls_returns_false()
    {
        var result = IListExtensions.AreAllNotNull(null, null, new object());

        result.Should().BeFalse();
    }

    [Fact]
    public void IlistExtensions___AreAllNotNull_all_not_nulls_returns_true()
    {
        var result = IListExtensions.AreAllNotNull(true, string.Empty, new object());

        result.Should().BeTrue();
    }

    // AreAllNull
    // ----------------------------------

    [Fact]
    public void IlistExtensions___AreAllNull_null_items_throws()
    {
        Assert.Throws<ArgumentNullException>(
            paramName: "items",
            testCode: () => IListExtensions.AreAllNull(null));
    }

    [Fact]
    public void IlistExtensions___AreAllNull_empty_items_throws()
    {
        Assert.Throws<ArgumentException>(
            paramName: "items",
            testCode: () => IListExtensions.AreAllNull([]));
    }

    [Fact]
    public void IlistExtensions___AreAllNull_some_nulls_returns_false()
    {
        var result = IListExtensions.AreAllNull(null, null, new object());

        result.Should().BeFalse();
    }

    [Fact]
    public void IlistExtensions___AreAllNull_all_nulls_returns_true()
    {
        var result = IListExtensions.AreAllNull(null, null, null);

        result.Should().BeTrue();
    }

    // NotNullCount
    // ---------------

    [Fact]
    public void IlistExtensions___NotNullCount_null_items_returns_0()
    {
        var result = IListExtensions.NotNullCount(null);

        result.Should().Be(0);
    }

    [Fact]
    public void IlistExtensions___NotNullCount_empty_items_returns_0()
    {
        var result = IListExtensions.NotNullCount([]);

        result.Should().Be(0);
    }

    [Fact]
    public void IlistExtensions___NotNullCount_mixed_items_returns_correct_count()
    {
        var result = IListExtensions.NotNullCount(null, 4, null, "", false, null, null);

        result.Should().Be(3);
    }

    // NullCount
    // ---------------

    [Fact]
    public void IlistExtensions___NullCount_null_items_returns_0()
    {
        var result = IListExtensions.NullCount(null);

        result.Should().Be(0);
    }

    [Fact]
    public void IlistExtensions___NullCount_empty_items_returns_0()
    {
        var result = IListExtensions.NullCount([]);

        result.Should().Be(0);
    }

    [Fact]
    public void IlistExtensions___NullCount_mixed_items_returns_correct_count()
    {
        var result = IListExtensions.NullCount(null, 4, null, "", false, null, null);

        result.Should().Be(4);
    }
}
