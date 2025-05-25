namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="Cezzi.Caching.CacheResultBase" />
public class CacheGetResult<T> : CacheResultBase
{
    /// <summary>Gets or sets the object.</summary>
    /// <value>The object.</value>
    public T Object { get; set; }

    /// <summary>Gets the hits.</summary>
    /// <value>The hits.</value>
    public int Hits { get; internal set; }

    /// <summary>Gets a value indicating whether this instance is hit.</summary>
    /// <value><c>true</c> if this instance is hit; otherwise, <c>false</c>.</value>
    public virtual bool IsHit => (this.Result & CacheResult.Hit) == CacheResult.Hit;

    /// <summary>Gets a value indicating whether this instance is miss.</summary>
    /// <value><c>true</c> if this instance is miss; otherwise, <c>false</c>.</value>
    public virtual bool IsMiss => (this.Result & CacheResult.Miss) == CacheResult.Miss;
}
