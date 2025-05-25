namespace Cezzi.Data.Tests.Extensions;

using FluentAssertions;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using Xunit;

public class IDbCommandExtensions_Tests
{
    // Add Output Parameter
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addoutputparam()
    {
        var cmd = new SqlCommand().AddOutputParameter("@test1", DbType.String);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Output);
        cmd.Parameters[0].Size.Should().Be(0);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addoutputparam_ansistring()
    {
        var cmd = new SqlCommand().AddOutputParameter("@test1", DbType.String, 4);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Output);
        cmd.Parameters[0].Size.Should().Be(4);
    }

    // Add Return Parameter
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addreturnparam()
    {
        var cmd = new SqlCommand().AddReturnParameter("@test1", DbType.String);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.ReturnValue);
        cmd.Parameters[0].Size.Should().Be(0);
    }

    // Add Structured Parameter
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addstructredparam()
    {
        var cmd = new SqlCommand().AddStructuredParameter("@test1", new DataTable());
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.Object);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.Structured);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(0);
    }

    // Set Stored Procedure
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_setstoredprocedure()
    {
        var cmd = new SqlCommand().SetStoredProcedure("dbo.dosomething");
        cmd.CommandType.Should().Be(CommandType.StoredProcedure);
        cmd.CommandText.Should().Be("dbo.dosomething");
    }

    // Add Xml Parameter
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addxmlparam_element()
    {
        var shouldLookLike = "<test>test</test>";

        var cmd = new SqlCommand().AddXmlParameter("@test1", new XElement("test", "test"));
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.Xml);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.Xml);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(shouldLookLike.Length);
    }

    // AddParameterIf string
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_true()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, value, () => true);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(value.Length);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_false()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, value, () => false);
        cmd.Parameters.Count.Should().Be(0);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_resolver_true()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, value, () => true);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(value.Length);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_resolver_false()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, () => value, () => false);
        cmd.Parameters.Count.Should().Be(0);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_with_size_true()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, size, value, () => true);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(size);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_with_size_resolver_true()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, size, value, () => true);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(size);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_with_size_false()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, size, value, () => false);
        cmd.Parameters.Count.Should().Be(0);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparamif_with_size_resolver_false()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameterIf("@test1", DbType.String, size, () => value, () => false);
        cmd.Parameters.Count.Should().Be(0);
    }

    // AddParameter string
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_string()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameter("@test1", DbType.String, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(value.Length);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_string_with_size()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameter("@test1", DbType.String, size, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.String);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NVarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(size);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    // AddParameter string fixed length
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_stringfixedlength()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameter("@test1", DbType.StringFixedLength, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.StringFixedLength);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(value.Length);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_stringfixedlength_with_size()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameter("@test1", DbType.StringFixedLength, size, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.StringFixedLength);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.NChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(size);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    // AddParameter ansistring
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_ansistring()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameter("@test1", DbType.AnsiString, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.AnsiString);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.VarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(value.Length);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_ansistring_with_size()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameter("@test1", DbType.AnsiString, size, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.AnsiString);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.VarChar);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(size);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    // AddParameter ansistring fixed length
    // ----------------------

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_ansistringfixedlength()
    {
        var value = "test-value-s";

        var cmd = new SqlCommand().AddParameter("@test1", DbType.AnsiStringFixedLength, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.AnsiStringFixedLength);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.Char);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(value.Length);
        cmd.Parameters[0].Value.Should().Be(value);
    }

    [Fact]
    [Obsolete]
    public void dbcommandextenstions___sql_addparam_ansistringfixedlength_with_size()
    {
        var value = "test-value-s";
        var size = 5;

        var cmd = new SqlCommand().AddParameter("@test1", DbType.AnsiStringFixedLength, size, value);
        cmd.Parameters.Count.Should().Be(1);
        cmd.Parameters[0].Should().BeOfType<SqlParameter>();
        cmd.Parameters[0].DbType.Should().Be(DbType.AnsiStringFixedLength);
        cmd.Parameters[0].SqlDbType.Should().Be(SqlDbType.Char);
        cmd.Parameters[0].Direction.Should().Be(ParameterDirection.Input);
        cmd.Parameters[0].Size.Should().Be(size);
        cmd.Parameters[0].Value.Should().Be(value);
    }
}
