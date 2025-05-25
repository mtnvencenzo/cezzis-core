namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using Xunit;

public class LongExtensions_Tests
{
    [Fact]
    public void longextensions___FirstGreaterThanZero_empty_params_returns_zero()
    {
        var result = LongExtensions.FirstGreaterThanZero();

        result.Should().Be(0);
    }

    [Fact]
    public void longextensions___FirstGreaterThanZero_null_nullable_returns_zero()
    {
        var result = LongExtensions.FirstGreaterThanZero(null as long?);

        result.Should().Be(0);
    }

    [Fact]
    public void longextensions___FirstGreaterThanZero_mixed_all_less_than_zero_returns_zero()
    {
        var result = LongExtensions.FirstGreaterThanZero(null, -2, -1);

        result.Should().Be(0);
    }

    [Fact]
    public void longextensions___FirstGreaterThanZero_mixed_all_less_than_or_zero_returns_zero()
    {
        var result = LongExtensions.FirstGreaterThanZero(null, 0, -1);

        result.Should().Be(0);
    }

    [Fact]
    public void longextensions___FirstGreaterThanZero_mixed_non_zeros_trailing_returns_first_non_zero()
    {
        var result = LongExtensions.FirstGreaterThanZero(null, 0, null, -4, 1, 2);

        result.Should().Be(1);
    }

    [Fact]
    public void longextensions___FirstGreaterThanZero_mixed_non_zeros_leading_returns_first_non_zero()
    {
        var result = LongExtensions.FirstGreaterThanZero(99, null, 4, 0, null, -4, 1, 2);

        result.Should().Be(99);
    }
}
