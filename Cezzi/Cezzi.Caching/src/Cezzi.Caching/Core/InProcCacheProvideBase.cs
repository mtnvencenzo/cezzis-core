namespace Cezzi.Caching.Core;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.CacheProviderBase" />
public abstract class InProcCacheProvideBase : CacheProviderBase
{
    /// <summary>Gets the location.</summary>
    /// <value>The location.</value>
    public override CacheLocation Location => CacheLocation.InProcess;

    /// <summary>Clears this instance.</summary>
    /// <returns></returns>
    public override CacheClearResult Clear()
    {
        var cachData = this.GetCacheData();

        try
        {
            cachData.cache.Clear();

            // reseting counters
            cachData.getHitCount = 0;
            cachData.getMissCount = 0;
            cachData.putCount = 0;
            cachData.hitCount = 0;
            cachData.missCount = 0;
            cachData.deleteHitCount = 0;
            cachData.deleteMissCount = 0;
            cachData.expiredHitCount = 0;
            cachData.serializationFailureCount = 0;

            return new CacheClearResult
            {
                Location = this.Location,
                Result = CacheResult.Cleared,
                Key = null
            };
        }
        catch (System.Exception)
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
        var cachData = this.GetCacheData();

        try
        {
            if (this.ShouldPurgeExpiredItems())
            {
                this.PurgeExpiredItems();
            }

            if (cachData.cache.ContainsKey(key.Key))
            {
                cachData.cache.Remove(key.Key);
                Interlocked.Increment(ref cachData.hitCount);
                Interlocked.Increment(ref cachData.deleteHitCount);

                return new CacheDeleteResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Deleted
                };
            }
            else
            {
                Interlocked.Increment(ref cachData.missCount);
                Interlocked.Increment(ref cachData.deleteMissCount);

                return new CacheDeleteResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Miss
                };
            }
        }
        catch (Exception)
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
        var cachData = this.GetCacheData();

        try
        {
            if (this.ShouldPurgeExpiredItems())
            {
                this.PurgeExpiredItems();
            }

            if (cachData.cache.ContainsKey(key.Key))
            {
                var item = cachData.cache[key.Key];

                if (!item.IsExpired)
                {
                    T copy = default;

                    if (item.Item != null)
                    {
                        try
                        {
                            copy = (T)this.GetObjectCopy(item.Item);
                        }
                        catch (Exception)
                        {
                            Interlocked.Increment(ref cachData.serializationFailureCount);
                            throw;
                        }
                    }

                    item.IncrementHits();
                    Interlocked.Increment(ref cachData.hitCount);
                    Interlocked.Increment(ref cachData.getHitCount);

                    // found the item and its not expired
                    // so return it
                    return new CacheGetResult<T>
                    {
                        Key = key,
                        Location = this.Location,
                        Object = copy,
                        Result = CacheResult.Hit,
                        Hits = item.Hits
                    };
                }
                else
                {
                    Interlocked.Increment(ref cachData.missCount);
                    Interlocked.Increment(ref cachData.getMissCount);
                    Interlocked.Increment(ref cachData.expiredHitCount);

                    // The item is expired
                    cachData.cache.Remove(key.Key);

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
                Interlocked.Increment(ref cachData.missCount);
                Interlocked.Increment(ref cachData.getMissCount);

                return new CacheGetResult<T>
                {
                    Key = key,
                    Location = this.Location,
                    Object = default,
                    Result = CacheResult.Miss
                };
            }
        }
        catch (Exception)
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
        var cachData = this.GetCacheData();

        var created = DateTime.UtcNow;
        var expires = (key.ExpirationSeconds > 0)
            ? created.AddSeconds(key.ExpirationSeconds)
            : DateTime.MaxValue;

        try
        {
            if (this.ShouldPurgeExpiredItems())
            {
                this.PurgeExpiredItems();
            }

            T copy = default;

            try
            {
                copy = (T)this.GetObjectCopy(value);
            }
            catch
            {
                Interlocked.Increment(ref cachData.serializationFailureCount);
                throw;
            }

            if (cachData.cache.ContainsKey(key.Key))
            {
                cachData.cache[key.Key] = new CacheItem
                {
                    Created = created,
                    Expires = expires,
                    Item = copy
                };

                Interlocked.Increment(ref cachData.hitCount);
                Interlocked.Increment(ref cachData.putCount);

                return new CachePutResult
                {
                    Key = key,
                    Location = this.Location,
                    Result = CacheResult.Put | CacheResult.Updated
                };
            }
            else
            {
                cachData.cache.Add(key.Key, new CacheItem
                {
                    Created = created,
                    Expires = expires,
                    Item = copy
                });

                Interlocked.Increment(ref cachData.putCount);

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
        var cachData = this.GetCacheData();

        try
        {
            return new CacheStatResult
            {
                Statistics =
                [
                    new CacheStat { Name = "KeyCount", Value = cachData.cache.Count.ToString() },
                    new CacheStat { Name = "HitCount", Value = cachData.hitCount.ToString() },
                    new CacheStat { Name = "MissCount", Value = cachData.missCount.ToString() },
                    new CacheStat { Name = "GetHitCount", Value = cachData.getHitCount.ToString() },
                    new CacheStat { Name = "GetMissCount", Value = cachData.getMissCount.ToString() },
                    new CacheStat { Name = "DeleteHitCount", Value = cachData.deleteHitCount.ToString() },
                    new CacheStat { Name = "DeleteMissCount", Value = cachData.deleteMissCount.ToString() },
                    new CacheStat { Name = "SerializationFailureCount", Value = cachData.serializationFailureCount.ToString() },
                    new CacheStat { Name = "PutCount", Value = cachData.putCount.ToString() },
                    new CacheStat { Name = "PurgeSeconds", Value = cachData.purgeSeconds.ToString() },
                    new CacheStat { Name = "StartTime", Value = cachData.startTime.ToString("r") },
                    new CacheStat { Name = "LastPurgeTime", Value = cachData.lastPurgeTime.ToString("r") },
                    new CacheStat { Name = "ExpiredHitCount", Value = cachData.expiredHitCount.ToString() }
                ],
                Location = this.Location,
                Result = CacheResult.Hit
            };
        }
        catch (Exception)
        {
            return new CacheStatResult
            {
                Statistics = [],
                Location = this.Location,
                Result = CacheResult.Miss | CacheResult.Unavailable
            };
        }
    }

    /// <summary>Gets the cache data.</summary>
    /// <returns></returns>
    protected abstract InProcCacheData GetCacheData();

    private object GetObjectCopy(object obj)
    {
        var data = JsonConvert.SerializeObject(obj);
        return JsonConvert.DeserializeObject(data, obj.GetType());
    }

    private void PurgeExpiredItems()
    {
        var cachData = this.GetCacheData();
        var keysToRemove = new List<string>();

        foreach (var item in cachData.cache)
        {
            if (item.Value.IsExpired)
            {
                keysToRemove.Add(item.Key);
            }
        }

        foreach (var key in keysToRemove)
        {
            cachData.cache.Remove(key);
        }

        cachData.lastPurgeTime = DateTime.UtcNow;
    }

    private bool ShouldPurgeExpiredItems() => this.GetCacheData().lastPurgeTime.AddSeconds(this.GetCacheData().purgeSeconds) <= DateTime.UtcNow;

}
