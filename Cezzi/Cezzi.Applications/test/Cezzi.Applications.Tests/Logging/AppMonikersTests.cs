namespace Cezzi.Applications.Tests.Logging;

using Cezzi.Applications.Logging;
using FluentAssertions;
using System.Linq;
using Xunit;

public class AppMonikersTests
{
    [Fact]
    public void appmonikers___monikers_match_expected()
    {
        var monikers = new AppMonikers();

        // These shouldn't chamge... If something broke then change it back since these values
        // shouldn't ever change or it will mess up our log searches
        monikers.ObjectGraph.Should().Be("@app_object_graph");
        monikers.ExistingObjectGraph.Should().Be("@app_existing_object_graph");
        monikers.Url.Should().Be("@app_url");
        monikers.Path.Should().Be("@app_path");
        monikers.ApiCallDuration.Should().Be("@app_api_duration");
        monikers.SqlTable.Should().Be("@app_sql_table");
        monikers.SqlBatchNumber.Should().Be("@app_sql_batch_number");
        monikers.SqlDuration.Should().Be("@app_sql_duration");
        monikers.SqlCommand.Should().Be("@app_sql_command");
        monikers.SqlCommandType.Should().Be("@app_sql_command_type");
        monikers.SqlCommandTimeout.Should().Be("@app_sql_command_timeout");
        monikers.SqlDatabase.Should().Be("@app_sql_database");
        monikers.SqlDataSource.Should().Be("@app_sql_datasource");
        monikers.SqlConnectionTimeout.Should().Be("@app_sql_connection_timeout");
        monikers.CacheKey.Should().Be("@app_cache_key");
        monikers.CreatedBy.Should().Be("@app_createdby");
        monikers.UpdatedBy.Should().Be("@app_updatedby");
        monikers.Principal.Should().Be("@app_principal");
    }

    [Fact]
    public void appmonikers___start_with_at_sign_and_are_read_only()
    {
        var monikers = new AppMonikers();
        var properties = monikers.GetType().GetProperties();

        properties.Should().NotBeNullOrEmpty();

        var hasAll = properties.All(x =>
        {
            return x.CanWrite != true && x.GetValue(monikers).ToString().StartsWith("@");
        });

        hasAll.Should().BeTrue();
    }
}
