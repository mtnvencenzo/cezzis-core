namespace Cezzi.Caching;

using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class CacheStatResult : CacheResultBase
{
    /// <summary>Gets or sets the statistics.</summary>
    /// <value>The statistics.</value>
    public List<CacheStat> Statistics { get; set; }

    /// <summary>Gets a value indicating whether this instance is hit.</summary>
    /// <value><c>true</c> if this instance is hit; otherwise, <c>false</c>.</value>
    public virtual bool IsHit => (this.Result & CacheResult.Hit) == CacheResult.Hit;
}
