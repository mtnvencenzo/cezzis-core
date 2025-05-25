namespace Cezzi.Caching;

using System;

/// <summary>
/// 
/// </summary>
public static class ICacheProviderExt
{
    /// <summary>Gets from or put and return.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="cacheProvider">The cache provider.</param>
    /// <param name="createCacheKey">The create cache key.</param>
    /// <param name="dataFetchCallback">The data fetch callback.</param>
    /// <returns></returns>
    public static T GetFromOrPutAndReturn<T>(this ICacheProvider cacheProvider, Func<CacheKey> createCacheKey, Func<T> dataFetchCallback)
    {
        T result;

        var cacheResult = cacheProvider?.Get<T>(createCacheKey());
        if (cacheResult?.IsHit ?? false)
        {
            result = cacheResult.Object;
        }
        else
        {
            result = dataFetchCallback();
            if (result != null)
            {
                cacheProvider?.Put(createCacheKey(), result);
            }
        }

        return result;
    }
}