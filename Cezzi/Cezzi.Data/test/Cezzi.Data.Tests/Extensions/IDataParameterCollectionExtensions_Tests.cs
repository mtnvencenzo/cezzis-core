namespace Cezzi.Data.Tests.Extensions;

using Cezzi.Data.Extensions;
using FluentAssertions;
using System;
using System.Data;
using System.Data.SqlClient;
using Xunit;

public class IDataParameterCollectionExtensions_Tests
{
    // Add Output Parameter
    // ----------------------

    [Fact]
    [Obsolete]
    public void hasparameter___all_dbtypes()
    {
        var guid = Guid.NewGuid();
        var now = DateTime.Now;

        var cmd = new SqlCommand()
            .AddParameter("@test1", DbType.String, "test")
            .AddParameter("@test2", DbType.Guid, guid)
            .AddParameter("@test3", DbType.AnsiString, "AnsiString")
            .AddParameter("@test4", DbType.AnsiStringFixedLength, "arf")
            .AddParameter("@test5", DbType.Boolean, true)
            .AddParameter("@test6", DbType.Byte, 1)
            .AddParameter("@test7", DbType.Currency, 12.21M)
            .AddParameter("@test8", DbType.Date, now)
            .AddParameter("@test9", DbType.DateTime, now)
            .AddParameter("@test10", DbType.DateTime2, now)
            .AddParameter("@test11", DbType.DateTimeOffset, now)
            .AddParameter("@test12", DbType.Decimal, 32.12M)
            .AddParameter("@test13", DbType.Double, 32.21D)
            .AddParameter("@test14", DbType.Int16, 1)
            .AddParameter("@test15", DbType.Int32, 45)
            .AddParameter("@test16", DbType.Int64, 7L)
            .AddParameter("@test17", DbType.StringFixedLength, "r")
            .AddParameter("@test18", DbType.Time, TimeSpan.FromSeconds(12))
            .AddParameter("@test19", DbType.Xml, "<test></test>");

        cmd.Parameters.HasParameter("@test1").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test1").Should().BeFalse();
        cmd.Parameters.HasParameter("@test1", "re").Should().BeFalse();
        cmd.Parameters.HasParameter("@test1", "test").Should().BeTrue();
        cmd.Parameters.HasParameter("@test1", "test", DbType.String).Should().BeTrue();

        cmd.Parameters.HasParameter("@test2").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test2").Should().BeFalse();
        cmd.Parameters.HasParameter("@test2", Guid.NewGuid()).Should().BeFalse();
        cmd.Parameters.HasParameter("@test2", guid).Should().BeTrue();
        cmd.Parameters.HasParameter("@test2", guid, DbType.Guid).Should().BeTrue();

        cmd.Parameters.HasParameter("@test3").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test3").Should().BeFalse();
        cmd.Parameters.HasParameter("@test3", "2").Should().BeFalse();
        cmd.Parameters.HasParameter("@test3", "AnsiString").Should().BeTrue();
        cmd.Parameters.HasParameter("@test3", "AnsiString", DbType.AnsiString).Should().BeTrue();

        cmd.Parameters.HasParameter("@test4").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test4").Should().BeFalse();
        cmd.Parameters.HasParameter("@test4", "333").Should().BeFalse();
        cmd.Parameters.HasParameter("@test4", "arf").Should().BeTrue();
        cmd.Parameters.HasParameter("@test4", "arf", DbType.AnsiStringFixedLength).Should().BeTrue();

        cmd.Parameters.HasParameter("@test5").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test5").Should().BeFalse();
        cmd.Parameters.HasParameter("@test5", false).Should().BeFalse();
        cmd.Parameters.HasParameter("@test5", true).Should().BeTrue();
        cmd.Parameters.HasParameter("@test5", true, DbType.Boolean).Should().BeTrue();

        cmd.Parameters.HasParameter("@test6").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test6").Should().BeFalse();
        cmd.Parameters.HasParameter("@test6", 2).Should().BeFalse();
        cmd.Parameters.HasParameter("@test6", 1).Should().BeTrue();
        cmd.Parameters.HasParameter("@test6", 1, DbType.Byte).Should().BeTrue();

        cmd.Parameters.HasParameter("@test7").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test7").Should().BeFalse();
        cmd.Parameters.HasParameter("@test7", 12.22M).Should().BeFalse();
        cmd.Parameters.HasParameter("@test7", 12.21M).Should().BeTrue();
        cmd.Parameters.HasParameter("@test7", 12.21M, DbType.Currency).Should().BeTrue();

        cmd.Parameters.HasParameter("@test8").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test8").Should().BeFalse();
        cmd.Parameters.HasParameter("@test8", now).Should().BeTrue();
        cmd.Parameters.HasParameter("@test8", now.AddDays(1)).Should().BeFalse();
        cmd.Parameters.HasParameter("@test8", now, DbType.DateTime).Should().BeTrue();

        cmd.Parameters.HasParameter("@test9").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test9").Should().BeFalse();
        cmd.Parameters.HasParameter("@test9", now.AddDays(1)).Should().BeFalse();
        cmd.Parameters.HasParameter("@test9", now).Should().BeTrue();
        cmd.Parameters.HasParameter("@test9", now, DbType.DateTime).Should().BeTrue();

        cmd.Parameters.HasParameter("@test10").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test10").Should().BeFalse();
        cmd.Parameters.HasParameter("@test10", now.AddDays(1)).Should().BeFalse();
        cmd.Parameters.HasParameter("@test10", now).Should().BeTrue();
        cmd.Parameters.HasParameter("@test10", now, DbType.DateTime2).Should().BeTrue();

        cmd.Parameters.HasParameter("@test11").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test11").Should().BeFalse();
        cmd.Parameters.HasParameter("@test11", now.AddDays(1)).Should().BeFalse();
        cmd.Parameters.HasParameter("@test11", now).Should().BeTrue();
        cmd.Parameters.HasParameter("@test11", now, DbType.DateTimeOffset).Should().BeTrue();

        cmd.Parameters.HasParameter("@test12").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test12").Should().BeFalse();
        cmd.Parameters.HasParameter("@test12", 32.13M).Should().BeFalse();
        cmd.Parameters.HasParameter("@test12", 32.12M).Should().BeTrue();
        cmd.Parameters.HasParameter("@test12", 32.12M, DbType.Decimal).Should().BeTrue();

        cmd.Parameters.HasParameter("@test13").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test13").Should().BeFalse();
        cmd.Parameters.HasParameter("@test13", 32.23D).Should().BeFalse();
        cmd.Parameters.HasParameter("@test13", 32.21D).Should().BeTrue();
        cmd.Parameters.HasParameter("@test13", 32.21D, DbType.Double).Should().BeTrue();

        cmd.Parameters.HasParameter("@test14").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test14").Should().BeFalse();
        cmd.Parameters.HasParameter("@test14", 2).Should().BeFalse();
        cmd.Parameters.HasParameter("@test14", 1).Should().BeTrue();
        cmd.Parameters.HasParameter("@test14", 1, DbType.Int16).Should().BeTrue();

        cmd.Parameters.HasParameter("@test15").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test15").Should().BeFalse();
        cmd.Parameters.HasParameter("@test15", 47).Should().BeFalse();
        cmd.Parameters.HasParameter("@test15", 45).Should().BeTrue();
        cmd.Parameters.HasParameter("@test15", 45, DbType.Int32).Should().BeTrue();

        cmd.Parameters.HasParameter("@test16").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test16").Should().BeFalse();
        cmd.Parameters.HasParameter("@test16", 8L).Should().BeFalse();
        cmd.Parameters.HasParameter("@test16", 7L).Should().BeTrue();
        cmd.Parameters.HasParameter("@test16", 7L, DbType.Int64).Should().BeTrue();

        cmd.Parameters.HasParameter("@test17").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test17").Should().BeFalse();
        cmd.Parameters.HasParameter("@test17", "rf").Should().BeFalse();
        cmd.Parameters.HasParameter("@test17", "r").Should().BeTrue();
        cmd.Parameters.HasParameter("@test17", "r", DbType.StringFixedLength).Should().BeTrue();

        cmd.Parameters.HasParameter("@test18").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test18").Should().BeFalse();
        cmd.Parameters.HasParameter("@test18", TimeSpan.FromSeconds(14)).Should().BeFalse();
        cmd.Parameters.HasParameter("@test18", TimeSpan.FromSeconds(12)).Should().BeTrue();
        cmd.Parameters.HasParameter("@test18", TimeSpan.FromSeconds(12), DbType.DateTime).Should().BeTrue();

        cmd.Parameters.HasParameter("@test19").Should().BeTrue();
        cmd.Parameters.HasParameter("@-test19").Should().BeFalse();
        cmd.Parameters.HasParameter("@test19", "<test1></test>1").Should().BeFalse();
        cmd.Parameters.HasParameter("@test19", "<test></test>").Should().BeTrue();
        cmd.Parameters.HasParameter("@test19", "<test></test>", DbType.Xml).Should().BeTrue();
    }

    [Fact]
    [Obsolete]
    public void hasparameteroftype___all_dbtypes()
    {
        var guid = Guid.NewGuid();
        var now = DateTime.Now;

        var cmd = new SqlCommand()
            .AddParameter("@test1", DbType.String, "test")
            .AddParameter("@test2", DbType.Guid, guid)
            .AddParameter("@test3", DbType.AnsiString, "AnsiString")
            .AddParameter("@test4", DbType.AnsiStringFixedLength, "arf")
            .AddParameter("@test5", DbType.Boolean, true)
            .AddParameter("@test6", DbType.Byte, 1)
            .AddParameter("@test7", DbType.Currency, 12.21M)
            .AddParameter("@test8", DbType.Date, now)
            .AddParameter("@test9", DbType.DateTime, now)
            .AddParameter("@test10", DbType.DateTime2, now)
            .AddParameter("@test11", DbType.DateTimeOffset, now)
            .AddParameter("@test12", DbType.Decimal, 32.12M)
            .AddParameter("@test13", DbType.Double, 32.21D)
            .AddParameter("@test14", DbType.Int16, 1)
            .AddParameter("@test15", DbType.Int32, 45)
            .AddParameter("@test16", DbType.Int64, 7L)
            .AddParameter("@test17", DbType.StringFixedLength, "r")
            .AddParameter("@test18", DbType.Time, TimeSpan.FromSeconds(12))
            .AddParameter("@test19", DbType.Xml, "<test></test>");

        cmd.Parameters.HasParameterOfType("@test1", DbType.String).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test1", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test2", DbType.Guid).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test2", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test3", DbType.AnsiString).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test3", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test4", DbType.AnsiStringFixedLength).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test4", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test5", DbType.Boolean).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test5", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test6", DbType.Byte).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test6", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test7", DbType.Currency).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test7", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test8", DbType.DateTime).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test8", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test9", DbType.DateTime).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test9", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test10", DbType.DateTime2).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test10", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test11", DbType.DateTimeOffset).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test11", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test12", DbType.Decimal).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test12", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test13", DbType.Double).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test13", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test14", DbType.Int16).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test14", DbType.Int32).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test15", DbType.Int32).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test15", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test16", DbType.Int64).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test16", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test17", DbType.StringFixedLength).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test17", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test18", DbType.DateTime).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test18", DbType.Int16).Should().BeFalse();

        cmd.Parameters.HasParameterOfType("@test19", DbType.Xml).Should().BeTrue();
        cmd.Parameters.HasParameterOfType("@test19", DbType.Int16).Should().BeFalse();
    }

    [Fact]
    [Obsolete]
    public void hasparametercontainingvalue___works()
    {
        var cmd = new SqlCommand()
            .AddParameter("@test1", DbType.String, "test")
            .AddParameter("@test2", DbType.AnsiString, "AnsiString")
            .AddParameter("@test3", DbType.AnsiStringFixedLength, "arf")
            .AddParameter("@test4", DbType.Xml, "<test></test>");

        cmd.Parameters.HasParameterContainingValue("@test1", "test", DbType.String).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test1", "tes", DbType.String).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test1", "rat", DbType.String).Should().BeFalse();

        cmd.Parameters.HasParameterContainingValue("@test2", "AnsiString", DbType.AnsiString).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test2", "nsiStr", DbType.AnsiString).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test2", "uyi", DbType.AnsiString).Should().BeFalse();

        cmd.Parameters.HasParameterContainingValue("@test3", "arf", DbType.AnsiStringFixedLength).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test3", "r", DbType.AnsiStringFixedLength).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test3", "z", DbType.AnsiStringFixedLength).Should().BeFalse();

        cmd.Parameters.HasParameterContainingValue("@test4", "<test></test>", DbType.Xml).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test4", "test>", DbType.Xml).Should().BeTrue();
        cmd.Parameters.HasParameterContainingValue("@test4", "<testy>", DbType.Xml).Should().BeFalse();
    }

    [Fact]
    [Obsolete]
    public void hasparameterwithvaluematch___works()
    {
        var cmd = new SqlCommand()
            .AddParameter("@test1", DbType.String, "test")
            .AddParameter("@test2", DbType.AnsiString, "AnsiString")
            .AddParameter("@test3", DbType.AnsiStringFixedLength, "arf")
            .AddParameter("@test4", DbType.Xml, "<test></test>");

        cmd.Parameters.HasParameterWithValueMatch("@test1", (v) => v.ToString() == "test").Should().BeTrue();
        cmd.Parameters.HasParameterWithValueMatch("@test1", (v) => v.ToString() == "tes").Should().BeFalse();

        cmd.Parameters.HasParameterWithValueMatch("@test2", (v) => v.ToString() == "AnsiString").Should().BeTrue();
        cmd.Parameters.HasParameterWithValueMatch("@test2", (v) => v.ToString() == "A1nsiString").Should().BeFalse();
    }

    [Fact]
    [Obsolete]
    public void hasoutputparam___all_dbtypes()
    {
        var cmd = new SqlCommand()
            .AddParameter("@test1", DbType.String, "test")
            .AddParameter("@test2", DbType.AnsiString, "AnsiString")
            .AddParameter("@test3", DbType.AnsiStringFixedLength, "arf")
            .AddParameter("@test4", DbType.Xml, "<test></test>")
            .AddOutputParameter("@test5", DbType.Boolean);

        cmd.Parameters.HasOutputParameter("@test5").Should().BeTrue();
        cmd.Parameters.HasOutputParameter("@test6").Should().BeFalse();
    }

    [Fact]
    [Obsolete]
    public void hasreturnparam___all_dbtypes()
    {
        var cmd = new SqlCommand()
            .AddParameter("@test1", DbType.String, "test")
            .AddParameter("@test2", DbType.AnsiString, "AnsiString")
            .AddParameter("@test3", DbType.AnsiStringFixedLength, "arf")
            .AddParameter("@test4", DbType.Xml, "<test></test>")
            .AddReturnParameter("@test5", DbType.Boolean);

        cmd.Parameters.HasReturnValueParameter("@test5").Should().BeTrue();
        cmd.Parameters.HasReturnValueParameter("@test6").Should().BeFalse();
    }
}
