namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.CacheResultBase" />
public class CachePutResult : CacheResultBase
{
    /// <summary>Gets a value indicating whether this instance is put.</summary>
    /// <value><c>true</c> if this instance is put; otherwise, <c>false</c>.</value>
    public virtual bool IsPut => (this.Result & CacheResult.Put) == CacheResult.Put;
}
