namespace Cezzi.Caching.Tests.Core;

using Cezzi.Caching;
using Cezzi.Caching.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

public class HttpContextCacheProvider_Tests
{
    [Fact]
    public void httpcontext___has_correct_location()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        provider.Location.Should().Be(CacheLocation.InContext);
    }

    // Clear
    // -----------------

    [Fact]
    public void httpcontext___clear_new_instance()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        var result = provider.Clear();

        result.IsCleared.Should().BeTrue();
        result.Key.Should().BeNull();
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Cleared);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void httpcontext___clear_verify()
    {
        var contextItems = new Dictionary<string, object>();
        var consistentKey = new CacheKey(GuidString(), GuidString(), 12);

        var provider = new HttpContextCacheProvider(() => contextItems);

        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Put(consistentKey, "test");
        _ = provider.Get<string>(consistentKey);
        _ = provider.Put(consistentKey, "test2");
        _ = provider.Delete(consistentKey);
        _ = provider.Delete(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Delete(new CacheKey(GuidString(), GuidString(), 12));

        var result = provider.Clear();
        result.IsCleared.Should().BeTrue();
        result.Key.Should().BeNull();
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Cleared);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    // Get
    // -----------------

    [Fact]
    public void httpcontext___get_not_exists()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Get<string>(key);
        result.IsHit.Should().BeFalse();
        result.IsMiss.Should().BeTrue();
        result.Object.Should().BeNull();
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Miss);

        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void httpcontext___get_exists()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);
        _ = provider.Put(key, "test");

        _ = provider.Get<string>(key);
        _ = provider.Get<string>(key);
        _ = provider.Get<string>(key);
        _ = provider.Get<string>(key);
        _ = provider.Get<string>(key);
        _ = provider.Get<string>(key);
        var result = provider.Get<string>(key);
        result.IsHit.Should().BeTrue();
        result.IsMiss.Should().BeFalse();
        result.Object.Should().Be("test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Hit);

        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void inproc___get_wrong_type()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        provider.Put(key, "test");
        var result = provider.Get<SerializeableTestObj>(key);
        result.IsHit.Should().BeFalse();
        result.IsMiss.Should().BeTrue();
        result.Object.Should().BeNull();
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Miss | CacheResult.Unavailable);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void inproc___get_expired()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 1);
        _ = provider.Put(key, "test");

        Thread.Sleep(2000);

        var result = provider.Get<string>(key);
        result.IsHit.Should().BeFalse();
        result.IsMiss.Should().BeTrue();
        result.Object.Should().BeNull();
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(1);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Miss | CacheResult.Expired);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    // Delete
    // -----------------

    [Fact]
    public void httpcontext___delete_not_exists()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Miss);
        result.IsDeleted.Should().BeFalse();

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void httpcontext___delete_exists()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);
        _ = provider.Put(key, "test");

        var result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Deleted);
        result.IsDeleted.Should().BeTrue();

        result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Miss);
        result.IsDeleted.Should().BeFalse();

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    // Put
    // -----------------

    [Fact]
    public void httpcontext___put_first_time()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, "test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<string>(key);
        get.Object.Should().Be("test");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void httpcontext___put_many_time2()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, "test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<string>(key);
        get.Object.Should().Be("test");

        result = provider.Put(key, "test2");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Updated | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        get = provider.Get<string>(key);
        get.Object.Should().Be("test2");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);

    }

    [Fact]
    public void httpcontext___put_expired()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 1);

        _ = provider.Put(key, "test");

        Thread.Sleep(2000);

        var result = provider.Put(key, "test2");

        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(1);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Updated | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<string>(key);
        get.Object.Should().Be("test2");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    [Fact]
    public void httpcontext___put_change_type()
    {
        var contextItems = new Dictionary<string, object>();
        var provider = new HttpContextCacheProvider(() => contextItems);

        var obj = new SerializeableTestObj
        {
            Id = GuidString().GetHashCode(),
            Value = GuidString()
        };

        var keyRegion = "203948029384892384";
        var keyBase = "oifwoijfwoiejfiojwejif";
        var key = new CacheKey(keyRegion, keyBase, 5);

        var result = provider.Put(key, "test");
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);

        var newkey = new CacheKey(keyRegion, keyBase);
        result = provider.Put(newkey, obj);

        result.Key.Should().BeSameAs(newkey);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Updated | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<SerializeableTestObj>(key);
        get.Object.Should().NotBeNull();
        get.Object.Id.Should().Be(obj.Id);
        get.Object.Value.Should().Be(obj.Value);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    // Round Trip
    // -----------------

    [Fact]
    public void httpcontext___roundtrip()
    {
        var contextItems = new Dictionary<string, object>();

        var provider = new HttpContextCacheProvider(() => contextItems);
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);
        var obj = new TestObj
        {
            Id = 1,
            Value = GuidString()
        };

        var result = provider.Put(key, obj);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InContext);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<TestObj>(key);
        get.IsHit.Should().BeTrue();
        get.IsMiss.Should().BeFalse();
        get.Object.Should().NotBeNull();
        get.Object.Should().NotBeSameAs(obj);
        get.Object.Id.Should().Be(obj.Id);
        get.Object.Value.Should().Be(obj.Value);
        get.Key.Should().BeSameAs(key);
        get.Key.ExpirationSeconds.Should().Be(12);
        get.Key.Region.Should().Be(keyRegion);
        get.Key.BaseKey.Should().Be(keyBase);
        get.Location.Should().Be(CacheLocation.InContext);
        get.Result.Should().Be(CacheResult.Hit);

        var delete = provider.Delete(key);
        delete.Key.Should().BeSameAs(key);
        delete.Key.ExpirationSeconds.Should().Be(12);
        delete.Key.Region.Should().Be(keyRegion);
        delete.Key.BaseKey.Should().Be(keyBase);
        delete.Location.Should().Be(CacheLocation.InContext);
        delete.Result.Should().Be(CacheResult.Deleted);
        delete.IsDeleted.Should().BeTrue();

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(0);
    }

    // Concurrent Tests
    // ------------------

    [Fact]
    public void httpcontext___concurrent()
    {
        var contextItems1 = new Dictionary<string, object>();
        var provider1 = new HttpContextCacheProvider(() => contextItems1);
        _ = provider1.Clear();

        var contextItems2 = new Dictionary<string, object>();
        var provider2 = new HttpContextCacheProvider(() => contextItems2);
        _ = provider2.Clear();

        static void test(int i, ICacheProvider provider)
        {
            var keyRegion = i.ToString() + GuidString();
            var keyBase = i.ToString() + GuidString();
            var key = new CacheKey(keyRegion, keyBase, 12);
            var obj = new TestObj
            {
                Id = GuidString().GetHashCode(),
                Value = GuidString()
            };

            var result = provider.Put(key, obj);
            result.Key.Should().BeSameAs(key);
            result.Key.ExpirationSeconds.Should().Be(12);
            result.Key.Region.Should().Be(keyRegion);
            result.Key.BaseKey.Should().Be(keyBase);
            result.Location.Should().Be(CacheLocation.InContext);
            result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
            result.IsPut.Should().BeTrue();

            var get = provider.Get<TestObj>(key);
            get.IsHit.Should().BeTrue();
            get.IsMiss.Should().BeFalse();
            get.Object.Should().NotBeNull();
            get.Object.Should().NotBeSameAs(obj);
            get.Object.Id.Should().Be(obj.Id);
            get.Object.Value.Should().Be(obj.Value);
            get.Key.Should().BeSameAs(key);
            get.Key.ExpirationSeconds.Should().Be(12);
            get.Key.Region.Should().Be(keyRegion);
            get.Key.BaseKey.Should().Be(keyBase);
            get.Location.Should().Be(CacheLocation.InContext);
            get.Result.Should().Be(CacheResult.Hit);

            var delete = provider.Delete(key);
            delete.Key.Should().BeSameAs(key);
            delete.Key.ExpirationSeconds.Should().Be(12);
            delete.Key.Region.Should().Be(keyRegion);
            delete.Key.BaseKey.Should().Be(keyBase);
            delete.Location.Should().Be(CacheLocation.InContext);
            delete.Result.Should().Be(CacheResult.Deleted);
            delete.IsDeleted.Should().BeTrue();
        }

        var items1 = new List<int>();
        for (var i = 0; i < 100000; i++)
        {
            items1.Add(i);
        }

        var items2 = new List<int>();
        for (var i = 0; i < 100000; i++)
        {
            items1.Add(i);
        }

        items1.AsParallel().ForAll((i) =>
        {
            test(i, provider1);
            test(i, provider2);
        });

        var stats1 = provider1.GetStats();
        StatValue(stats1, "KeyCount").Should().Be(0);
        StatValue(stats1, "HitCount").Should().Be(0);
        StatValue(stats1, "MissCount").Should().Be(0);
        StatValue(stats1, "GetHitCount").Should().Be(0);
        StatValue(stats1, "DeleteHitCount").Should().Be(0);
        StatValue(stats1, "DeleteMissCount").Should().Be(0);
        StatValue(stats1, "SerializationFailureCount").Should().Be(0);
        StatValue(stats1, "PutCount").Should().Be(0);
        StatValue(stats1, "ExpiredHitCount").Should().Be(0);
        StatValue(stats1, "PurgeSeconds").Should().Be(0);

        var stats2 = provider2.GetStats();
        StatValue(stats1, "KeyCount").Should().Be(0);
        StatValue(stats1, "HitCount").Should().Be(0);
        StatValue(stats1, "MissCount").Should().Be(0);
        StatValue(stats1, "GetHitCount").Should().Be(0);
        StatValue(stats1, "DeleteHitCount").Should().Be(0);
        StatValue(stats1, "DeleteMissCount").Should().Be(0);
        StatValue(stats1, "SerializationFailureCount").Should().Be(0);
        StatValue(stats1, "PutCount").Should().Be(0);
        StatValue(stats1, "ExpiredHitCount").Should().Be(0);
        StatValue(stats1, "PurgeSeconds").Should().Be(0);
    }

    private static string GuidString() => Guid.NewGuid().ToString();

    private static int StatValue(CacheStatResult stats, string name) => int.Parse(stats.Statistics.FirstOrDefault(x => x.Name == name).Value);
}

/// <summary>
/// 
/// </summary>
public class TestObj
{
    /// <summary>Gets or sets the identifier.</summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>Gets or sets the value.</summary>
    /// <value>The value.</value>
    public string Value { get; set; }
}