namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.CacheResultBase" />
public class CacheDeleteResult : CacheResultBase
{
    /// <summary>Gets a value indicating whether this instance is deleted.</summary>
    /// <value>
    /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
    /// </value>
    public virtual bool IsDeleted => (this.Result & CacheResult.Deleted) == CacheResult.Deleted;
}
