namespace Cezzi.Data.Tests.Extensions;

using Cezzi.Data.Extensions;
using FluentAssertions;
using System;
using Xunit;

public class IDataReaderExtensions_Tests
{
    [Fact]
    public void datareaderextension___GetUTCDateTimeValueSafe_with_non_nulls()
    {
        var value1 = DateTime.Now.AddDays(1);
        var value2 = DateTime.Now.AddDays(2);

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2"],
            datarows: [[value1, value2]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetUTCDateTimeValueSafe("test1");
        actual1.Should().Be(value1);
        actual1.Kind.Should().Be(DateTimeKind.Utc);
        actual1.ToString("o").Should().Be(value1.ToString("o")[0..27] + "Z");

        var actual2 = r.GetUTCDateTimeValueSafe("test2");
        actual2.Should().Be(value2);
        actual2.Kind.Should().Be(DateTimeKind.Utc);
        actual2.ToString("o").Should().Be(value2.ToString("o")[0..27] + "Z");
    }

    [Fact]
    public void datareaderextension___GetUTCDateTimeValueSafe_with_nulls()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2"],
            datarows: [[null, null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetUTCDateTimeValueSafe("test1");
        actual1.Should().Be(DateTime.MinValue);
        actual1.Kind.Should().Be(DateTimeKind.Utc);
        actual1.ToString("o").Should().Be(DateTime.MinValue.ToString("o")[0..27] + "Z");

        var actual2 = r.GetUTCDateTimeValueSafe("test2");
        actual2.Should().Be(DateTime.MinValue);
        actual2.Kind.Should().Be(DateTimeKind.Utc);
        actual2.ToString("o").Should().Be(DateTime.MinValue.ToString("o")[0..27] + "Z");
    }

    [Fact]
    public void datareaderextension___GetUTCDateTimeValueSafe_with_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2"],
            datarows: [[DBNull.Value, DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetUTCDateTimeValueSafe("test1");
        actual1.Should().Be(DateTime.MinValue);
        actual1.Kind.Should().Be(DateTimeKind.Utc);
        actual1.ToString("o").Should().Be(DateTime.MinValue.ToString("o")[0..27] + "Z");

        var actual2 = r.GetUTCDateTimeValueSafe("test2");
        actual2.Should().Be(DateTime.MinValue);
        actual2.Kind.Should().Be(DateTimeKind.Utc);
        actual2.ToString("o").Should().Be(DateTime.MinValue.ToString("o")[0..27] + "Z");
    }

    // GetValueSave<> float
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_float()
    {
        var value1 = 1.1f;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_float_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_float_null_withdefault()
    {
        var defaultVal = 1.1f;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_float_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_float_dbnull_withdefault()
    {
        var defaultVal = 1.1f;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_float_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_float_dbnull_withdefault()
    {
        var defaultVal = 1.1f;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<float?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> double
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_double()
    {
        var value1 = 1.1d;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_double_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_double_null_withdefault()
    {
        var defaultVal = 1.1d;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_double_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_double_dbnull_withdefault()
    {
        var defaultVal = 1.1d;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_double_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_double_dbnull_withdefault()
    {
        var defaultVal = 1.1d;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<double?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> decimal
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_decimal()
    {
        var value1 = 1.1m;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_decimal_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_decimal_null_withdefault()
    {
        var defaultVal = 1.1m;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_decimal_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_decimal_dbnull_withdefault()
    {
        var defaultVal = 1.1m;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_decimal_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_decimal_dbnull_withdefault()
    {
        var defaultVal = 1.1m;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<decimal?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> short
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_short()
    {
        short value1 = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_short_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_short_null_withdefault()
    {
        short defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_short_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_short_dbnull_withdefault()
    {
        short defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_short_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_short_dbnull_withdefault()
    {
        short defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<short?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> int
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_int()
    {
        var value1 = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_int_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_int_null_withdefault()
    {
        var defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_int_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_int_dbnull_withdefault()
    {
        var defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_int_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_int_dbnull_withdefault()
    {
        var defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<int?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> long
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_long()
    {
        long value1 = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_long_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_long_null_withdefault()
    {
        long defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_long_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_long_dbnull_withdefault()
    {
        long defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_long_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_long_dbnull_withdefault()
    {
        long defaultVal = 1;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<long?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> bool
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_bool()
    {
        var value1 = true;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_bool_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_bool_null_withdefault()
    {
        var defaultVal = true;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_bool_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_bool_dbnull_withdefault()
    {
        var defaultVal = true;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_bool_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_bool_dbnull_withdefault()
    {
        var defaultVal = true;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<bool?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> string
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_string()
    {
        var value1 = "test";

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<string>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_string_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<string>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_string_null_withdefault()
    {
        var defaultVal = "test";

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<string>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_string_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<string>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_string_dbnull_withdefault()
    {
        var defaultVal = "test";

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<string>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> char
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_char()
    {
        var value1 = 'Y';

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<char>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_char_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<char>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_char_null_withdefault()
    {
        var defaultVal = 'Y';

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<char>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_char_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<char>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_char_dbnull_withdefault()
    {
        var defaultVal = 'Y';

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<char>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> DateTime
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_datetime()
    {
        var value1 = DateTime.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetime_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetime_null_withdefault()
    {
        var defaultVal = DateTime.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetime_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetime_dbnull_withdefault()
    {
        var defaultVal = DateTime.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_datetime_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_datetime_dbnull_withdefault()
    {
        var defaultVal = DateTime.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTime?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    // GetValueSave<> DateTimeOffset
    // -------------------------

    [Fact]
    public void datareaderextension___getvaluesafe_datetimeoffset()
    {
        var value1 = DateTimeOffset.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset>("test1");
        actual1.Should().Be(value1);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetimeoffset_null()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetimeoffset_null_withdefault()
    {
        var defaultVal = DateTimeOffset.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[null]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetimeoffset_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset>("test1");
        actual1.Should().Be(default);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_datetimeoffset_dbnull_withdefault()
    {
        var defaultVal = DateTimeOffset.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_datetimeoffset_dbnull()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset?>("test1");
        actual1.Should().Be(null);
    }

    [Fact]
    public void datareaderextension___getvaluesafe_nullable_datetimeoffset_dbnull_withdefault()
    {
        var defaultVal = DateTimeOffset.Now;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[DBNull.Value]]);

        r.Read().Should().BeTrue();

        var actual1 = r.GetValueSafe<DateTimeOffset?>("test1", defaultVal);
        actual1.Should().Be(defaultVal);
    }
}
