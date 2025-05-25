namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
public enum CacheLocation
{
    /// <summary>The none</summary>
    None = 0,

    /// <summary>The out of process</summary>
    OutOfProcess = 1,

    /// <summary>The in process</summary>
    InProcess = 2,

    /// <summary>The in context</summary>
    InContext = 3
}
