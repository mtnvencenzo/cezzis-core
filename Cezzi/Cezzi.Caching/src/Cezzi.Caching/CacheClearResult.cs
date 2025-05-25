namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.CacheResultBase" />
public class CacheClearResult : CacheResultBase
{
    /// <summary>Gets a value indicating whether this instance is cleared.</summary>
    /// <value>
    /// <c>true</c> if this instance is cleared; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsCleared => (this.Result & CacheResult.Cleared) == CacheResult.Cleared;
}
