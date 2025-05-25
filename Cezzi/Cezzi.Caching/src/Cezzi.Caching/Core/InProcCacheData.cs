namespace Cezzi.Caching.Core;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class InProcCacheData
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public IDictionary<string, CacheItem> cache;
    public DateTime startTime;
    public int purgeSeconds = 300;
    public DateTime lastPurgeTime = DateTime.UtcNow;
    public int getHitCount;
    public int getMissCount;
    public int putCount;
    public int hitCount;
    public int missCount;
    public int deleteHitCount;
    public int deleteMissCount;
    public int expiredHitCount;
    public int serializationFailureCount;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>Initializes a new instance of the <see cref="InProcCacheData"/> class.</summary>
    public InProcCacheData()
    {
        this.cache = new ConcurrentDictionary<string, CacheItem>();

        this.hitCount = 0;
        this.getHitCount = 0;
        this.getMissCount = 0;
        this.putCount = 0;
        this.missCount = 0;
        this.deleteHitCount = 0;
        this.deleteMissCount = 0;
        this.expiredHitCount = 0;
        this.serializationFailureCount = 0;
        this.startTime = DateTime.UtcNow;
    }
}
