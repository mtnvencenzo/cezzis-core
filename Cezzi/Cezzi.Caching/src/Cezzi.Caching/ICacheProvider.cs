namespace Cezzi.Caching;

using System;

/// <summary>
/// 
/// </summary>
public interface ICacheProvider
{
    /// <summary>Gets the specified key.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    CacheGetResult<T> Get<T>(CacheKey key);

    /// <summary>Gets the or put.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="getter">The getter.</param>
    /// <returns></returns>
    CacheGetResult<T> GetOrPut<T>(CacheKey key, Func<T> getter);

    /// <summary>Sets the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    CachePutResult Put<T>(CacheKey key, T value);

    /// <summary>Deletes the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    CacheDeleteResult Delete(CacheKey key);

    /// <summary>Clears this instance.</summary>
    /// <returns></returns>
    CacheClearResult Clear();

    /// <summary>Gets the stats.</summary>
    /// <returns></returns>
    CacheStatResult GetStats();
}
