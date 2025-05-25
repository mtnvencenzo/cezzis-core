namespace Cezzi.Caching.Tests.Core;
using Cezzi.Caching.Core;

using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;

public class InjectableInProcCacheProvider_Tests
{
    [Fact]
    public void injectinproc___has_correct_location()
    {
        var provider = new InjectableInProcCacheProvider();
        provider.Location.Should().Be(CacheLocation.InProcess);
    }

    // Clear
    // -----------------

    [Fact]
    public void injectinproc___clear_new_instance()
    {
        var provider = new InjectableInProcCacheProvider();
        var result = provider.Clear();

        result.IsCleared.Should().BeTrue();
        result.Key.Should().BeNull();
        result.Location.Should().Be(CacheLocation.InProcess);
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
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___clear_verify()
    {
        var consistentKey = new CacheKey(GuidString(), GuidString(), 12);
        var provider = new InjectableInProcCacheProvider();

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
        result.Location.Should().Be(CacheLocation.InProcess);
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
        StatValue(stats, "PurgeSeconds").Should().Be(300);

        stats.Location.Should().Be(CacheLocation.InProcess);
        stats.Result.Should().Be(CacheResult.Hit);
        stats.IsHit.Should().BeTrue();
    }

    // Get
    // -----------------

    [Fact]
    public void injectinproc___get_not_exists()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Get<string>(key);
        result.IsHit.Should().BeFalse();
        result.Hits.Should().Be(0);
        result.IsMiss.Should().BeTrue();
        result.Object.Should().BeNull();
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
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
        StatValue(stats, "MissCount").Should().Be(7);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___get_exists()
    {
        var provider = new InjectableInProcCacheProvider();

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
        result.Hits.Should().Be(7);
        result.IsMiss.Should().BeFalse();
        result.Object.Should().Be("test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Hit);

        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));
        _ = provider.Get<string>(new CacheKey(GuidString(), GuidString(), 12));

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(7);
        StatValue(stats, "MissCount").Should().Be(6);
        StatValue(stats, "GetHitCount").Should().Be(7);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___getfromorput_when_not_exists_ext()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.GetFromOrPutAndReturn(
            () => key,
            () => "test");

        result.Should().Be("test");
    }

    [Fact]
    public void injectinproc___getfromorput_when_exists_ext()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        provider.Put(key, "test");

        var result = provider.GetFromOrPutAndReturn(
            () => key,
            () => "test1");

        result.Should().Be("test");
    }

    [Fact]
    public void injectinproc___get_wrong_type()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        provider.Put(key, "test");
        var result = provider.Get<SerializeableTestObj>(key);
        result.IsHit.Should().BeFalse();
        result.Hits.Should().Be(0);
        result.IsMiss.Should().BeTrue();
        result.Object.Should().BeNull();
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Miss | CacheResult.Unavailable);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(1);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___get_expired()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 1);
        _ = provider.Put(key, "test");

        Thread.Sleep(2000);

        var result = provider.Get<string>(key);
        result.IsHit.Should().BeFalse();
        result.Hits.Should().Be(0);
        result.IsMiss.Should().BeTrue();
        result.Object.Should().BeNull();
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(1);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Miss | CacheResult.Expired);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(1);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(1);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    // Delete
    // -----------------

    [Fact]
    public void injectinproc___delete_not_exists()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Miss);
        result.IsDeleted.Should().BeFalse();

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(0);
        StatValue(stats, "MissCount").Should().Be(1);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(1);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(0);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___delete_exists()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);
        _ = provider.Put(key, "test");

        var result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Deleted);
        result.IsDeleted.Should().BeTrue();

        result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Miss);
        result.IsDeleted.Should().BeFalse();

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(1);
        StatValue(stats, "MissCount").Should().Be(1);
        StatValue(stats, "GetHitCount").Should().Be(0);
        StatValue(stats, "DeleteHitCount").Should().Be(1);
        StatValue(stats, "DeleteMissCount").Should().Be(1);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    // Put
    // -----------------

    [Fact]
    public void injectinproc___put_first_time()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, "test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<string>(key);
        get.Object.Should().Be("test");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(1);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___put_first_time_with_dictionary_works()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, new Dictionary<string, int>
        {
            { "test", 1 },
            { "test2", 2 },
        });

        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<Dictionary<string, int>>(key);
        get.Object.Should().NotBeNull();
        get.Object.Should().HaveCount(2);
        get.Object["test"].Should().Be(1);
        get.Object["test2"].Should().Be(2);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(1);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___put_first_time_with_dictionary_int_keys_works()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, new Dictionary<int, string>
        {
            { 1, "test1" },
            { 2, "test2" },
        });

        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<Dictionary<int, string>>(key);
        get.Object.Should().NotBeNull();
        get.Object.Should().HaveCount(2);
        get.Object[1].Should().Be("test1");
        get.Object[2].Should().Be("test2");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(1);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___put_first_time_with_dictionary_int_keys_object_value_works()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 3600);

        var result = provider.Put(key, new Dictionary<int, TestClass1>
        {
            { 1, new TestClass1 { Prop1 = "256", Prop2 = 12 } },
            { 2, new TestClass1{ Prop1 = "257", Prop2 = 11 } },
        });

        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(3600);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<Dictionary<int, TestClass1>>(key);
        get.Object.Should().NotBeNull();
        get.Object.Should().HaveCount(2);
        get.Object[1].Prop1.Should().Be("256");
        get.Object[1].Prop2.Should().Be(12);
        get.Object[2].Prop1.Should().Be("257");
        get.Object[2].Prop2.Should().Be(11);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(1);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___put_many_time2()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, "test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<string>(key);
        get.Object.Should().Be("test");

        result = provider.Put(key, "test2");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Updated | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        get = provider.Get<string>(key);
        get.Object.Should().Be("test2");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(3);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(2);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(2);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);

    }

    [Fact]
    public void injectinproc___put_expired()
    {
        var provider = new InjectableInProcCacheProvider();

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
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Updated | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<string>(key);
        get.Object.Should().Be("test2");

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(2);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(2);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    [Fact]
    public void injectinproc___put_change_type()
    {
        var provider = new InjectableInProcCacheProvider();

        var obj = new SerializeableTestObj
        {
            Id = GuidString().GetHashCode(),
            Value = GuidString()
        };

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 1);

        _ = provider.Put(key, "test");

        Thread.Sleep(2000);

        key = new CacheKey(keyRegion, keyBase, 12);
        var result = provider.Put(key, obj);

        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Updated | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<SerializeableTestObj>(key);
        get.Object.Should().NotBeNull();
        get.Object.Id.Should().Be(obj.Id);
        get.Object.Value.Should().Be(obj.Value);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(1);
        StatValue(stats, "HitCount").Should().Be(2);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(0);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(2);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    // Round Trip
    // -----------------

    [Fact]
    public void injectinproc___roundtrip()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);
        var obj = new SerializeableTestObj
        {
            Id = 1,
            Value = GuidString()
        };

        var result = provider.Put(key, obj);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
        result.IsPut.Should().BeTrue();

        var get = provider.Get<SerializeableTestObj>(key);
        get.IsHit.Should().BeTrue();
        get.Hits.Should().Be(1);
        get.IsMiss.Should().BeFalse();
        get.Object.Should().NotBeNull();
        get.Object.Should().NotBeSameAs(obj);
        get.Object.Id.Should().Be(obj.Id);
        get.Object.Value.Should().Be(obj.Value);
        get.Key.Should().BeSameAs(key);
        get.Key.ExpirationSeconds.Should().Be(12);
        get.Key.Region.Should().Be(keyRegion);
        get.Key.BaseKey.Should().Be(keyBase);
        get.Location.Should().Be(CacheLocation.InProcess);
        get.Result.Should().Be(CacheResult.Hit);

        var delete = provider.Delete(key);
        delete.Key.Should().BeSameAs(key);
        delete.Key.ExpirationSeconds.Should().Be(12);
        delete.Key.Region.Should().Be(keyRegion);
        delete.Key.BaseKey.Should().Be(keyBase);
        delete.Location.Should().Be(CacheLocation.InProcess);
        delete.Result.Should().Be(CacheResult.Deleted);
        delete.IsDeleted.Should().BeTrue();

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(2);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(1);
        StatValue(stats, "DeleteHitCount").Should().Be(1);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(1);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    // Concurrent Tests
    // ------------------

    [Fact]
    public void injectinproc___concurrent()
    {
        var provider = new InjectableInProcCacheProvider();

        void test(int i)
        {
            var keyRegion = i.ToString() + GuidString() + GuidString();
            var keyBase = i.ToString() + GuidString() + GuidString();
            var key = new CacheKey(keyRegion, keyBase, 12);
            var obj = new SerializeableTestObj
            {
                Id = GuidString().GetHashCode(),
                Value = GuidString()
            };

            var result = provider.Put(key, obj);
            result.Key.Should().BeSameAs(key);
            result.Key.ExpirationSeconds.Should().Be(12);
            result.Key.Region.Should().Be(keyRegion);
            result.Key.BaseKey.Should().Be(keyBase);
            result.Location.Should().Be(CacheLocation.InProcess);
            result.Result.Should().Be(CacheResult.Added | CacheResult.Put);
            result.IsPut.Should().BeTrue();

            var get = provider.Get<SerializeableTestObj>(key);
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
            get.Location.Should().Be(CacheLocation.InProcess);
            get.Result.Should().Be(CacheResult.Hit);

            var delete = provider.Delete(key);
            delete.Key.Should().BeSameAs(key);
            delete.Key.ExpirationSeconds.Should().Be(12);
            delete.Key.Region.Should().Be(keyRegion);
            delete.Key.BaseKey.Should().Be(keyBase);
            delete.Location.Should().Be(CacheLocation.InProcess);
            delete.Result.Should().Be(CacheResult.Deleted);
            delete.IsDeleted.Should().BeTrue();
        }

        var items = new List<int>();
        for (var i = 0; i < 100000; i++)
        {
            items.Add(i);
        }

        items.AsParallel().ForAll(test);

        var stats = provider.GetStats();
        StatValue(stats, "KeyCount").Should().Be(0);
        StatValue(stats, "HitCount").Should().Be(200000);
        StatValue(stats, "MissCount").Should().Be(0);
        StatValue(stats, "GetHitCount").Should().Be(100000);
        StatValue(stats, "DeleteHitCount").Should().Be(100000);
        StatValue(stats, "DeleteMissCount").Should().Be(0);
        StatValue(stats, "SerializationFailureCount").Should().Be(0);
        StatValue(stats, "PutCount").Should().Be(100000);
        StatValue(stats, "ExpiredHitCount").Should().Be(0);
        StatValue(stats, "PurgeSeconds").Should().Be(300);
    }

    // GetOrPut Tests
    // ------------------

    [Fact]
    public void injectinproc___getprput()
    {
        var provider = new InjectableInProcCacheProvider();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.GetOrPut(key, () => "new-value");
        result.IsHit.Should().BeTrue();
        result.IsMiss.Should().BeFalse();
        result.Object.Should().Be("new-value");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Hit | CacheResult.Put | CacheResult.Added);

        result = provider.GetOrPut(key, () => "new-value2");
        result.IsHit.Should().BeTrue();
        result.IsMiss.Should().BeFalse();
        result.Object.Should().Be("new-value");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.InProcess);
        result.Result.Should().Be(CacheResult.Hit);
    }

    private static string GuidString() => Guid.NewGuid().ToString();

    private static int StatValue(CacheStatResult stats, string name) => int.Parse(stats.Statistics.FirstOrDefault(x => x.Name == name).Value);

    public class TestClass1
    {
        /// <summary>
        /// Gets or sets the phone code.
        /// </summary>
        public string Prop1 { get; set; }

        /// <summary>
        /// Gets or sets the phone number size.
        /// </summary>
        public int Prop2 { get; set; }
    }
}