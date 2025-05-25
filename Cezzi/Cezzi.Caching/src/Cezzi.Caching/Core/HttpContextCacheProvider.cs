namespace Cezzi.Caching.Core;

using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.CacheProviderBase" />
/// <remarks>Initializes a new instance of the <see cref="HttpContextCacheProvider"/> class.</remarks>
/// <param name="httpContextItemsAccessor">The HTTP context items accessor.</param>
/// <exception cref="System.ArgumentNullException">httpContextItemsAccessor</exception>
public class HttpContextCacheProvider(Func<System.Collections.IDictionary> httpContextItemsAccessor) : CacheProviderBase
{
    /// <summary>The context items cache key</summary>
    protected const string CONTEXT_ITEMS_CACHE_KEY = "__HTTPCONTEXT_CACHE_STORE";
    private readonly Func<System.Collections.IDictionary> httpContextItems = httpContextItemsAccessor ?? throw new ArgumentNullException(nameof(httpContextItemsAccessor));

    /// <summary>Gets the location.</summary>
    /// <value>The location.</value>
    public override CacheLocation Location => CacheLocation.InContext;

    /// <summary>Gets the cache.</summary>
    /// <returns></returns>
    public virtual IDictionary<string, CacheItem> GetCache()
    {
        IDictionary<string, CacheItem> cache = null;

        var items = this.httpContextItems();

        if (!items.Contains(CONTEXT_ITEMS_CACHE_KEY))
        {
            cache = new ConcurrentDictionary<string, CacheItem>();
            items.Add(CONTEXT_ITEMS_CACHE_KEY, cache);
        }
        else
        {
            cache = items[CONTEXT_ITEMS_CACHE_KEY] as IDictionary<string, CacheItem>;
        }

        return cache;
    }

    /// <summary>Gets the object copy.</summary>
    /// <param name="obj">The object.</param>
    /// <returns></returns>
    private object GetObjectCopy(object obj)
    {
        var data = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject(data, obj.GetType());
    }

    /// <summary>Clears this instance.</summary>
    /// <returns></returns>
    public override CacheClearResult Clear()
    {
        try
        {
            var cache = this.GetCache();
            cache.Clear();

            return new CacheClearResult
            {
                Location = this.Location,
                Result = CacheResult.Cleared,
                Key = null
            };
        }
        catch
        {
            return new CacheClearResult
            {
                Location = this.Location,
                Result = CacheResult.Unavailable,
                Key = null
            };
        }
    }

    /// <summary>Deletes the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public override CacheDeleteResult Delete(CacheKey key)
    {
        try
        {
            var cache = this.GetCache();

            if (cache.ContainsKey(key.Key))
            {
                cache.Remove(key.Key);

                return new CacheDeleteResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Deleted
                };
            }
            else
            {
                return new CacheDeleteResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Miss
                };
            }
        }
        catch
        {
            return new CacheDeleteResult
            {
                Key = key,
                Location = this.Location,
                Result = CacheResult.Miss | CacheResult.Unavailable
            };
        }
    }

    /// <summary>Gets the specified key.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override CacheGetResult<T> Get<T>(CacheKey key)
    {
        try
        {
            var cache = this.GetCache();

            if (cache.ContainsKey(key.Key))
            {
                var item = cache[key.Key];

                if (!item.IsExpired)
                {
                    T copy = default;

                    if (item.Item != null)
                    {
                        copy = (T)this.GetObjectCopy(item.Item);
                    }

                    // found the item and its not expired
                    // so return it
                    return new CacheGetResult<T>
                    {
                        Key = key,
                        Location = this.Location,
                        Object = copy,
                        Result = CacheResult.Hit
                    };
                }
                else
                {
                    // The item is expired
                    cache.Remove(key.Key);

                    return new CacheGetResult<T>
                    {
                        Key = key,
                        Location = this.Location,
                        Object = default,
                        Result = CacheResult.Miss | CacheResult.Expired
                    };
                }
            }
            else
            {
                return new CacheGetResult<T>
                {
                    Key = key,
                    Location = this.Location,
                    Object = default,
                    Result = CacheResult.Miss
                };
            }
        }
        catch
        {
            return new CacheGetResult<T>
            {
                Key = key,
                Location = this.Location,
                Object = default,
                Result = CacheResult.Miss | CacheResult.Unavailable
            };
        }
    }

    /// <summary>Sets the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override CachePutResult Put<T>(CacheKey key, T value)
    {
        var created = DateTime.UtcNow;
        var expires = (key.ExpirationSeconds > 0)
            ? created.AddSeconds(key.ExpirationSeconds)
            : DateTime.MaxValue;

        try
        {
            var cache = this.GetCache();

            var copy = (T)this.GetObjectCopy(value);

            if (cache.ContainsKey(key.Key))
            {
                cache[key.Key] = new CacheItem
                {
                    Created = created,
                    Expires = expires,
                    Item = copy
                };

                return new CachePutResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Put | CacheResult.Updated
                };
            }
            else
            {
                if (key.Key == "203948029384892384|oifwoijfwoiejfiojwejif")
                {

                }

                cache.Add(key.Key, new CacheItem
                {
                    Created = created,
                    Expires = expires,
                    Item = copy
                });

                return new CachePutResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Put | CacheResult.Added
                };
            }
        }
        catch
        {
            return new CachePutResult
            {
                Key = key,
                Location = this.Location,
                Result = CacheResult.Miss | CacheResult.Unavailable
            };
        }
    }

    /// <summary>Gets the stats.</summary>
    /// <returns></returns>
    public override CacheStatResult GetStats()
    {
        try
        {
            return new CacheStatResult
            {
                Statistics =
                [
                    new CacheStat { Name = "KeyCount", Value = this.GetCache().Count.ToString() },
                    new CacheStat { Name = "HitCount", Value = "0" },
                    new CacheStat { Name = "MissCount", Value = "0" },
                    new CacheStat { Name = "GetHitCount", Value = "0" },
                    new CacheStat { Name = "GetMissCount", Value = "0" },
                    new CacheStat { Name = "DeleteHitCount", Value = "0" },
                    new CacheStat { Name = "DeleteMissCount", Value = "0" },
                    new CacheStat { Name = "SerializationFailureCount", Value = "0" },
                    new CacheStat { Name = "PutCount", Value = "0" },
                    new CacheStat { Name = "PurgeSeconds", Value = "0" },
                    new CacheStat { Name = "StartTime", Value = DateTime.MinValue.ToString("r") },
                    new CacheStat { Name = "LastPurgeTime", Value = DateTime.MinValue.ToString("r") },
                    new CacheStat { Name = "ExpiredHitCount", Value = "0" }
                ],
                Location = this.Location,
                Result = CacheResult.Hit
            };
        }
        catch
        {
            return new CacheStatResult
            {
                Statistics = [],
                Location = this.Location,
                Result = CacheResult.Miss | CacheResult.Unavailable
            };
        }
    }
}
