namespace Cezzi.Applications.Logging;

/// <summary>
/// 
/// </summary>
public class AppMonikers
{
    /// <summary>Gets the object graph.</summary>
    /// <value>The object graph.</value>
    public string ObjectGraph => "@app_object_graph";

    /// <summary>Gets the existing object graph.</summary>
    /// <value>The existing object graph.</value>
    public string ExistingObjectGraph => "@app_existing_object_graph";

    /// <summary>Gets the URL.</summary>
    /// <value>The URL.</value>
    public string Url => "@app_url";

    /// <summary>Gets the path.</summary>
    /// <value>The path.</value>
    public string Path => "@app_path";

    /// <summary>Gets the duration of the API call.</summary>
    /// <value>The duration of the API call.</value>
    public string ApiCallDuration => "@app_api_duration";

    /// <summary>Gets the SQL table.</summary>
    /// <value>The SQL table.</value>
    public string SqlTable => "@app_sql_table";

    /// <summary>Gets the SQL batch number.</summary>
    /// <value>The SQL batch number.</value>
    public string SqlBatchNumber => "@app_sql_batch_number";

    /// <summary>Gets the duration of the SQL.</summary>
    /// <value>The duration of the SQL.</value>
    public string SqlDuration => "@app_sql_duration";

    /// <summary>Gets the SQL command.</summary>
    /// <value>The SQL command.</value>
    public string SqlCommand => "@app_sql_command";

    /// <summary>Gets the type of the SQL command.</summary>
    /// <value>The type of the SQL command.</value>
    public string SqlCommandType => "@app_sql_command_type";

    /// <summary>Gets the SQL command timeout.</summary>
    /// <value>The SQL command timeout.</value>
    public string SqlCommandTimeout => "@app_sql_command_timeout";

    /// <summary>Gets the SQL database.</summary>
    /// <value>The SQL database.</value>
    public string SqlDatabase => "@app_sql_database";

    /// <summary>Gets the SQL data source.</summary>
    /// <value>The SQL data source.</value>
    public string SqlDataSource => "@app_sql_datasource";

    /// <summary>Gets the SQL connection timeout.</summary>
    /// <value>The SQL connection timeout.</value>
    public string SqlConnectionTimeout => "@app_sql_connection_timeout";

    /// <summary>Gets the cache key.</summary>
    /// <value>The cache key.</value>
    public string CacheKey => "@app_cache_key";

    /// <summary>Gets the created by.</summary>
    /// <value>The created by.</value>
    public string CreatedBy => "@app_createdby";

    /// <summary>Gets the updated by.</summary>
    /// <value>The updated by.</value>
    public string UpdatedBy => "@app_updatedby";

    /// <summary>Gets the principal identity.</summary>
    /// <value>The principal identity.</value>
    public string Principal => "@app_principal";

    /// <summary>Gets the reference date.</summary>
    /// <value>The reference date.</value>
    public string ReferenceDate => "@app_refdate";
}
