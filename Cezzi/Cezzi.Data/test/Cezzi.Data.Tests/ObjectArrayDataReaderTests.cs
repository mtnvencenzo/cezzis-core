namespace Cezzi.Data.Tests;

using FluentAssertions;
using System;
using Xunit;

public class ObjectArrayDataReaderTests
{
    [Fact]
    public void objectarrayreader___throws_on_null_columnNames()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new ObjectArrayDataReader(
            columnNames: null,
            datarows: []));

        ex.ParamName.Should().Be("columnNames");
    }

    [Fact]
    public void objectarrayreader___throws_on_null_datarows()
    {
        var ex = Assert.Throws<ArgumentNullException>(() => new ObjectArrayDataReader(
            columnNames: [],
            datarows: null));

        ex.ParamName.Should().Be("datarows");
    }

    [Fact]
    public void objectarrayreader___reads_nultiple_result_sets()
    {
        var value1 = GuidString();
        var value2 = GuidString();
        var value3 = GuidString();

        var r = new ObjectArrayDataReader(
            columnNames: ["test1"],
            datarows: [[value1]]);

        r.AddResult(
            columnNames: ["test2"],
            datarows: [[value2]]);

        r.AddResult(
            columnNames: ["test3"],
            datarows: [[value3]]);

        r.AddResult(
            columnNames: ["test4"],
            datarows: [[null]]);

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

            safeReader.NextResult();
            safeReader.Read();
            safeReader["test4"].Should().BeNull();
            safeReader[0].Should().BeNull();
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_short()
    {
        short value1 = 1;
        short value2 = 2;
        short value3 = 3;
        short value4 = 4;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetInt16("test1").Should().Be(matchValue);
                safeReader.GetInt16(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetInt16("test2").Should().Be(matchValue);
                safeReader.GetInt16(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetInt16("test3").Should().Be(default);
                safeReader.GetInt16(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetInt16("test4").Should().Be(default);
                safeReader.GetInt16(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_int()
    {
        var value1 = 1;
        var value2 = 2;
        var value3 = 3;
        var value4 = 4;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetInt32("test1").Should().Be(matchValue);
                safeReader.GetInt32(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetInt32("test2").Should().Be(matchValue);
                safeReader.GetInt32(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetInt32("test3").Should().Be(default);
                safeReader.GetInt32(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetInt32("test4").Should().Be(default);
                safeReader.GetInt32(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_long()
    {
        var value1 = 1L;
        var value2 = 2L;
        var value3 = 3L;
        var value4 = 4L;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetInt64("test1").Should().Be(matchValue);
                safeReader.GetInt64(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetInt64("test2").Should().Be(matchValue);
                safeReader.GetInt64(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetInt64("test3").Should().Be(default);
                safeReader.GetInt64(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetInt64("test4").Should().Be(default);
                safeReader.GetInt64(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_float()
    {
        var value1 = 1f;
        var value2 = 2f;
        var value3 = 3f;
        var value4 = 4f;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetFloat("test1").Should().Be(matchValue);
                safeReader.GetFloat(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetFloat("test2").Should().Be(matchValue);
                safeReader.GetFloat(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetFloat("test3").Should().Be(default);
                safeReader.GetFloat(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetFloat("test4").Should().Be(default);
                safeReader.GetFloat(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_double()
    {
        var value1 = 1d;
        var value2 = 2d;
        var value3 = 3d;
        var value4 = 4d;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetDouble("test1").Should().Be(matchValue);
                safeReader.GetDouble(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetDouble("test2").Should().Be(matchValue);
                safeReader.GetDouble(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetDouble("test3").Should().Be(default);
                safeReader.GetDouble(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetDouble("test4").Should().Be(default);
                safeReader.GetDouble(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_decimal()
    {
        var value1 = 1m;
        var value2 = 2m;
        var value3 = 3m;
        var value4 = 4m;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetDecimal("test1").Should().Be(matchValue);
                safeReader.GetDecimal(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetDecimal("test2").Should().Be(matchValue);
                safeReader.GetDecimal(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetDecimal("test3").Should().Be(default);
                safeReader.GetDecimal(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetDecimal("test4").Should().Be(default);
                safeReader.GetDecimal(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_byte()
    {
        byte value1 = 1;
        byte value2 = 2;
        byte value3 = 3;
        byte value4 = 4;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetByte("test1").Should().Be(matchValue);
                safeReader.GetByte(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetByte("test2").Should().Be(matchValue);
                safeReader.GetByte(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetByte("test3").Should().Be(default);
                safeReader.GetByte(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetByte("test4").Should().Be(default);
                safeReader.GetByte(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_bool()
    {
        var value1 = true;
        var value2 = false;
        var value3 = true;
        var value4 = false;

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetBoolean("test1").Should().Be(matchValue);
                safeReader.GetBoolean(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetBoolean("test2").Should().Be(matchValue);
                safeReader.GetBoolean(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetBoolean("test3").Should().Be(default);
                safeReader.GetBoolean(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetBoolean("test4").Should().Be(default);
                safeReader.GetBoolean(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_guid()
    {
        var value1 = Guid.NewGuid();
        var value2 = Guid.NewGuid();
        var value3 = Guid.NewGuid();
        var value4 = Guid.NewGuid();

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetGuid("test1").Should().Be(matchValue);
                safeReader.GetGuid(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetGuid("test2").Should().Be(matchValue);
                safeReader.GetGuid(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetGuid("test3").Should().Be(Guid.Empty);
                safeReader.GetGuid(2).Should().Be(Guid.Empty);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetGuid("test4").Should().Be(Guid.Empty);
                safeReader.GetGuid(3).Should().Be(Guid.Empty);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_datetime()
    {
        var value1 = DateTime.Now.AddDays(1);
        var value2 = DateTime.Now.AddDays(2);
        var value3 = DateTime.Now.AddDays(3);
        var value4 = DateTime.Now.AddDays(4);

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetDateTime("test1").Should().Be(matchValue);
                safeReader.GetDateTime(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetDateTime("test2").Should().Be(matchValue);
                safeReader.GetDateTime(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetDateTime("test3").Should().Be(default);
                safeReader.GetDateTime(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetDateTime("test4").Should().Be(default);
                safeReader.GetDateTime(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_datetimeoffset()
    {
        var value1 = DateTimeOffset.Now.AddDays(1);
        var value2 = DateTimeOffset.Now.AddDays(2);
        var value3 = DateTimeOffset.Now.AddDays(3);
        var value4 = DateTimeOffset.Now.AddDays(4);

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetDateTimeOffset("test1").Should().Be(matchValue);
                safeReader.GetDateTimeOffset(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetDateTimeOffset("test2").Should().Be(matchValue);
                safeReader.GetDateTimeOffset(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetDateTimeOffset("test3").Should().Be(default);
                safeReader.GetDateTimeOffset(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetDateTimeOffset("test4").Should().Be(default);
                safeReader.GetDateTimeOffset(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_char()
    {
        var value1 = 'A';
        var value2 = 'S';
        var value3 = 'D';
        var value4 = 'F';

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetChar("test1").Should().Be(matchValue);
                safeReader.GetChar(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetChar("test2").Should().Be(matchValue);
                safeReader.GetChar(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetChar("test3").Should().Be(default);
                safeReader.GetChar(2).Should().Be(default);

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetChar("test4").Should().Be(default);
                safeReader.GetChar(3).Should().Be(default);

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_string()
    {
        var value1 = GuidString();
        var value2 = GuidString();
        var value3 = GuidString();
        var value4 = GuidString();

        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows:
            [
                [value1, value2, null, DBNull.Value],
                [value3, value4, null, DBNull.Value]
            ]);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            var isFirst = true;

            while (safeReader.Read())
            {
                var matchValue = isFirst ? value1 : value3;
                safeReader["test1"].Should().Be(matchValue);
                safeReader[0].Should().Be(matchValue);
                safeReader.GetString("test1").Should().Be(matchValue);
                safeReader.GetString(0).Should().Be(matchValue);

                matchValue = isFirst ? value2 : value4;
                safeReader["test2"].Should().Be(matchValue);
                safeReader[1].Should().Be(matchValue);
                safeReader.GetString("test2").Should().Be(matchValue);
                safeReader.GetString(1).Should().Be(matchValue);

                safeReader["test3"].Should().BeNull();
                safeReader[2].Should().BeNull();
                safeReader.GetString("test3").Should().BeEmpty();
                safeReader.GetString(2).Should().BeEmpty();

                safeReader["test4"].Should().BeNull();
                safeReader[3].Should().BeNull();
                safeReader.GetString("test4").Should().BeEmpty();
                safeReader.GetString(3).Should().BeEmpty();

                isFirst = false;
            }
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___reads_columnnames()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows: []);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            safeReader.GetName(0).Should().Be("test1");
            safeReader.GetName(1).Should().Be("test2");
            safeReader.GetName(2).Should().Be("test3");
            safeReader.GetName(3).Should().Be("test4");
        }

        r.IsClosed.Should().BeTrue();
    }

    [Fact]
    public void objectarrayreader___doesnt_read_no_rows()
    {
        var r = new ObjectArrayDataReader(
            columnNames: ["test1", "test2", "test3", "test4"],
            datarows: []);

        // Testing with the safe reader to kill two birds with one stone.
        using (var safeReader = new SafeDataReader(r))
        {
            safeReader.Read().Should().BeFalse();
        }

        r.IsClosed.Should().BeTrue();
    }

    private static string GuidString() => Guid.NewGuid().ToString();
}