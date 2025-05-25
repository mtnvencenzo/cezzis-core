namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
public interface ICacheable
{
    /// <summary>Gets the cache key.</summary>
    /// <returns></returns>
    CacheKey GetCacheKey();

    /// <summary>Gets the cache key.</summary>
    /// <param name="expirationSeconds">The expiration seconds.</param>
    /// <returns></returns>
    CacheKey GetCacheKey(int expirationSeconds);
}
