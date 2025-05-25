namespace Cezzi.Applications.Tests.Extensions;

using Cezzi.Applications.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class DateTimeExtensions_Tests
{
    [Fact]
    public void datetimeextensions___tounixtime_should_be_accurate()
    {
        var now = DateTime.Now;

        var unix = now.ToUnixTime();
        var offsetCheck = new DateTimeOffset(now).ToUnixTimeSeconds();

        unix.Should().Be(offsetCheck);
    }

    [Fact]
    public void datetimeextensions___fromunixtime_should_be_accurate()
    {
        var now = DateTime.UtcNow;
        var nowNoMilli = new DateTime(
            year: now.Year,
            month: now.Month,
            day: now.Day,
            hour: now.Hour,
            minute: now.Minute,
            second: now.Second,
            kind: DateTimeKind.Utc);

        var unix = nowNoMilli.ToUnixTime();

        var nowFrom = unix.FromUnixTime();
        nowFrom.Should().Be(nowNoMilli);
    }

    [Theory]
    [InlineData("1/1/1753 12:00:00 AM", true)]
    [InlineData("1/1/1752 23:59:59", false)]
    [InlineData("12/31/9999 11:59:59 PM", true)]
    public void datetimeextensions___is_valid_sql_date(string dateString, bool expectedResult)
    {
        var date = DateTime.Parse(dateString);
        date.IsValidSqlDate().Should().Be(expectedResult);
    }

    [Fact]
    public void datetimeextensions___to_sql_date()
    {
        var date = DateTime.Now;
        date.ToSqlDate().Should().Be(date.ToString("yyyy-MM-dd"));
    }
}