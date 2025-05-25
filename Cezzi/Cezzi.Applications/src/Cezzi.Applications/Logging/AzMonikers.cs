namespace Cezzi.Applications.Logging;

/// <summary>
/// 
/// </summary>
public class AzMonikers
{
    /// <summary>The az function</summary>
    public string AzFunction => "@az_func";

    /// <summary>Gets the az function invocation identifier.</summary>
    /// <value>The az function invocation identifier.</value>
    public string AzFunctionInvocationId => "@az_func_invocationid";

    /// <summary>Gets the az function timer is past due.</summary>
    /// <value>The az function timer is past due.</value>
    public string AzFunctionTimerIsPastDue => "@az_func_timer_ispastdue";

    /// <summary>Gets the az function timer last ran.</summary>
    /// <value>The az function timer last ran.</value>
    public string AzFunctionTimerLastRan => "@az_func_timer_lastran";

    /// <summary>Gets the az function timer next run.</summary>
    /// <value>The az function timer next run.</value>
    public string AzFunctionTimerNextRun => "@az_func_timer_nextrun";

    /// <summary>Gets the name of the az BLOB.</summary>
    /// <value>The name of the az BLOB.</value>
    public string AzBlobName => "@az_blob_name";

    /// <summary>Gets the type of the az BLOB.</summary>
    /// <value>The type of the az BLOB.</value>
    public string AzBlobType => "@az_blob_type";

    /// <summary>Gets the az BLOB bytes count.</summary>
    /// <value>The az BLOB bytes count.</value>
    public string AzBlobBytesCount => "@az_blob_byte_cnt";

    /// <summary>Gets the az BLOB created on.</summary>
    /// <value>The az BLOB created on.</value>
    public string AzBlobCreatedOn => "@az_blob_created";

    /// <summary>Gets the az BLOB deleted.</summary>
    /// <value>The az BLOB deleted.</value>
    public string AzBlobDeleted => "@az_blob_deleted";

    /// <summary>Gets the az BLOB is latest.</summary>
    /// <value>The az BLOB is latest.</value>
    public string AzBlobIsLatest => "@az_blob_islatest";

    /// <summary>Gets the az BLOB version identifier.</summary>
    /// <value>The az BLOB version identifier.</value>
    public string AzBlobVersionId => "@az_blob_versionid";
}