namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using Xunit;

public class IntExtensions_Tests
{
    [Theory]
    [InlineData(-1, 0, 100)]
    [InlineData(0, 1, 100)]
    [InlineData(1, 2, 100)]
    [InlineData(101, 2, 100)]
    [InlineData(-100, -99, -1)]
    [InlineData(-1, -100, -2)]
    [InlineData(100, 1, 99)]
    public void intextensions___isbetween_returns_false(int i, int start, int end) => i.IsBetween(start, end).Should().BeFalse();

    [Theory]
    [InlineData(0, 0, 100)]
    [InlineData(-1, -99, -1)]
    [InlineData(-2, -99, -1)]
    [InlineData(1, 1, 100)]
    [InlineData(2, 1, 100)]
    [InlineData(100, 1, 100)]
    public void intextensions___isbetween_returns_true(int i, int start, int end) => i.IsBetween(start, end).Should().BeTrue();
}
