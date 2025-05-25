namespace Cezzi.Caching.Core;

using System;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.CacheProviderBase" />
public class DefaultNoCacheProvider : CacheProviderBase
{
    /// <summary>Gets the location.</summary>
    /// <value>The location.</value>
    public override CacheLocation Location => CacheLocation.None;

    /// <summary>Clears this instance.</summary>
    /// <returns></returns>
    public override CacheClearResult Clear()
    {
        return new CacheClearResult
        {
            Location = this.Location,
            Result = CacheResult.Cleared,
            Key = null
        };
    }

    /// <summary>Deletes the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public override CacheDeleteResult Delete(CacheKey key)
    {
        return new CacheDeleteResult
        {
            Key = key,
            Location = this.Location,
            Result = CacheResult.Miss
        };
    }

    /// <summary>Gets the specified key.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override CacheGetResult<T> Get<T>(CacheKey key)
    {
        return new CacheGetResult<T>
        {
            Key = key,
            Location = this.Location,
            Object = default,
            Result = CacheResult.Miss,
            Hits = 0
        };
    }

    /// <summary>Sets the specified key.</summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override CachePutResult Put<T>(CacheKey key, T value)
    {
        return new CachePutResult
        {
            Key = key,
            Location = this.Location,
            Result = CacheResult.Miss
        };
    }

    /// <summary>Gets the stats.</summary>
    /// <returns></returns>
    public override CacheStatResult GetStats()
    {
        return new CacheStatResult
        {
            Statistics =
            [
                new CacheStat { Name = "KeyCount", Value = "0" },
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
}
