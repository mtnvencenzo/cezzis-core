namespace Cezzi.Caching.Tests.Core;

using Cezzi.Caching;
using Cezzi.Caching.Core;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class DefaultNoCacheProvider_Tests
{
    [Fact]
    public void nocache___has_correct_location()
    {
        var provider = new DefaultNoCacheProvider();
        provider.Location.Should().Be(CacheLocation.None);
    }

    // Clear
    // -----------------

    [Fact]
    public void nocache___clear()
    {
        var provider = new DefaultNoCacheProvider();
        var result = provider.Clear();

        result.IsCleared.Should().BeTrue();
        result.Key.Should().BeNull();
        result.Location.Should().Be(CacheLocation.None);
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
    public void nocache___get()
    {
        var provider = new DefaultNoCacheProvider();
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
        result.Location.Should().Be(CacheLocation.None);
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

    // Delete
    // -----------------

    [Fact]
    public void nocache___delete()
    {
        var provider = new DefaultNoCacheProvider();
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Delete(key);
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.None);
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
    public void nocache___put()
    {
        var provider = new DefaultNoCacheProvider();
        _ = provider.Clear();

        var keyRegion = GuidString();
        var keyBase = GuidString();
        var key = new CacheKey(keyRegion, keyBase, 12);

        var result = provider.Put(key, "test");
        result.Key.Should().BeSameAs(key);
        result.Key.ExpirationSeconds.Should().Be(12);
        result.Key.Region.Should().Be(keyRegion);
        result.Key.BaseKey.Should().Be(keyBase);
        result.Location.Should().Be(CacheLocation.None);
        result.Result.Should().Be(CacheResult.Miss);
        result.IsPut.Should().BeFalse();

        var get = provider.Get<string>(key);
        get.Object.Should().BeNull();

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
    public void nocache___concurrent()
    {
        var provider = new DefaultNoCacheProvider();
        _ = provider.Clear();

        static void test(int i, ICacheProvider provider)
        {
            var keyRegion = i.ToString() + GuidString();
            var keyBase = i.ToString() + GuidString();
            var key = new CacheKey(keyRegion, keyBase, 12);
            var obj = new NoTestObj
            {
                Id = GuidString().GetHashCode(),
                Value = GuidString()
            };

            var result = provider.Put(key, obj);
            result.Key.Should().BeSameAs(key);
            result.Key.ExpirationSeconds.Should().Be(12);
            result.Key.Region.Should().Be(keyRegion);
            result.Key.BaseKey.Should().Be(keyBase);
            result.Location.Should().Be(CacheLocation.None);
            result.Result.Should().Be(CacheResult.Miss);
            result.IsPut.Should().BeFalse();

            var get = provider.Get<NoTestObj>(key);
            get.IsHit.Should().BeFalse();
            get.IsMiss.Should().BeTrue();
            get.Object.Should().BeNull();
            get.Key.Should().BeSameAs(key);
            get.Key.ExpirationSeconds.Should().Be(12);
            get.Key.Region.Should().Be(keyRegion);
            get.Key.BaseKey.Should().Be(keyBase);
            get.Location.Should().Be(CacheLocation.None);
            get.Result.Should().Be(CacheResult.Miss);

            var delete = provider.Delete(key);
            delete.Key.Should().BeSameAs(key);
            delete.Key.ExpirationSeconds.Should().Be(12);
            delete.Key.Region.Should().Be(keyRegion);
            delete.Key.BaseKey.Should().Be(keyBase);
            delete.Location.Should().Be(CacheLocation.None);
            delete.Result.Should().Be(CacheResult.Miss);
            delete.IsDeleted.Should().BeFalse();
        }

        var items1 = new List<int>();
        for (var i = 0; i < 500000; i++)
        {
            items1.Add(i);
        }

        var items2 = new List<int>();
        for (var i = 0; i < 500000; i++)
        {
            items1.Add(i);
        }

        items1.AsParallel().ForAll((i) =>
        {
            test(i, provider);
        });

        var stats1 = provider.GetStats();
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
public class NoTestObj
{
    /// <summary>Gets or sets the identifier.</summary>
    /// <value>The identifier.</value>
    public int Id { get; set; }

    /// <summary>Gets or sets the value.</summary>
    /// <value>The value.</value>
    public string Value { get; set; }
}