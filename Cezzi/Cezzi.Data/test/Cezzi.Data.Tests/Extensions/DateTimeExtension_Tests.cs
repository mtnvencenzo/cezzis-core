namespace Cezzi.Data.Tests.Extensions;

using Cezzi.Data.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class DateTimeExtension_Tests
{
    [Fact]
    public void datetimeextensions___tosqldate_formats_correctly_now()
    {
        var date = DateTime.Now;

        var result = date.ToSqlDateFormat();

        result.Should().Be($"{date.Year}-{date.Month.ToString().PadLeft(2, '0')}-{date.Day.ToString().PadLeft(2, '0')}");
    }

    [Fact]
    public void datetimeextensions___tosqldate_formats_correctly_single_digit_month_and_day()
    {
        var date = new DateTime(
            year: 2000,
            month: 1,
            day: 1,
            hour: 1,
            minute: 1,
            second: 1);

        var result = date.ToSqlDateFormat();

        result.Should().Be($"2000-01-01");
    }

    [Fact]
    public void datetimeextensions___tosqldate_formats_correctly_double_digit_month_and_day()
    {
        var date = new DateTime(
            year: 2000,
            month: 12,
            day: 31,
            hour: 1,
            minute: 1,
            second: 1);

        var result = date.ToSqlDateFormat();

        result.Should().Be($"2000-12-31");
    }

    [Theory]
    [InlineData("1/1/1753 00:00:00")]
    [InlineData("1/2/1753")]
    [InlineData("1/2/2022")]
    [InlineData("12/31/9999 23:59:59")]
    public void datetimeextensions___isvalidsqldate_true(string date) => DateTime.Parse(date).IsValidSqlDate().Should().BeTrue();

    [Theory]
    [InlineData("1/1/1752 23:59:59")]
    public void datetimeextensions___isvalidsqldate_false(string date) => DateTime.Parse(date).IsValidSqlDate().Should().BeFalse();
}
