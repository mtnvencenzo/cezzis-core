namespace Cezzi.Caching;

using System;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.ICacheProvider" />
/// <seealso cref="Cezzi.Caching.IScopedCacheProvider" />
/// <seealso cref="Cezzi.Caching.ITransientCacheProvider" />
public abstract class CacheProviderBase : ICacheProvider, IScopedCacheProvider, ITransientCacheProvider
{
    /// <summary>Gets the location.</summary>
    /// <value>The location.</value>
    public abstract CacheLocation Location { get; }

    /// <summary>Clears this instance.</summary>
    /// <returns></returns>
    public abstract CacheClearResult Clear();

    /// <summary>Deletes the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public abstract CacheDeleteResult Delete(CacheKey key);

    /// <summary>Gets the specified key.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public abstract CacheGetResult<T> Get<T>(CacheKey key);

    /// <summary>Gets the or put.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="getter">The getter.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public virtual CacheGetResult<T> GetOrPut<T>(CacheKey key, Func<T> getter)
    {

        var getResult = this.Get<T>(key);

        if (getResult.IsHit)
        {
            return getResult;
        }

        var getterResult = getter();

        if (getterResult != null)
        {
            var putResult = this.Put(key, getterResult);

            return putResult.IsPut
                ? new CacheGetResult<T>
                {
                    Key = key,
                    Location = this.Location,
                    Object = getterResult,
                    Result = CacheResult.Hit | putResult.Result
                }
                : new CacheGetResult<T>
                {
                    Key = key,
                    Location = this.Location,
                    Result = getResult.Result | putResult.Result
                };
        }

        return getResult;
    }

    /// <summary>Gets the stats.</summary>
    /// <returns></returns>
    public abstract CacheStatResult GetStats();

    /// <summary>Sets the specified key.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public abstract CachePutResult Put<T>(CacheKey key, T value);
}
