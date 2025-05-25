namespace Cezzi.Caching.Tests.Core;
using Cezzi.Caching.Core;

using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

public class DefaultCacheFactory_Tests
{
    [Fact]
    public void factory___add_thros_when_only_allows_one_provider_per_location_same_type()
    {
        var factory = new DefaultCacheFactory();

        factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());

        var ex = Assert.Throws<ArgumentException>(() =>
        {
            factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Provider location already exists");
    }

    [Fact]
    public void factory___add_throws_only_allows_one_provider_per_location_different_type()
    {
        var factory = new DefaultCacheFactory();

        factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());

        var ex = Assert.Throws<ArgumentException>(() =>
        {
            factory.AddProvider(CacheLocation.InProcess, new DefaultNoCacheProvider());
        });

        ex.Should().NotBeNull();
        ex.Message.Should().Be("Provider location already exists");
    }

    [Fact]
    public void factory___get_returns_null_when_provider_location_doesnt_exist()
    {
        var factory = new DefaultCacheFactory();

        factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());
        factory.AddProvider(CacheLocation.InContext, new HttpContextCacheProvider(() => new Dictionary<string, object>()));

        var provider = factory.GetProvider(CacheLocation.OutOfProcess);

        provider.Should().BeNull();
    }

    [Fact]
    public void factory___get_returns_provider()
    {
        var inContextProvider = new DefaultNoCacheProvider();
        var factory = new DefaultCacheFactory();

        factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());
        factory.AddProvider(CacheLocation.OutOfProcess, inContextProvider);
        factory.AddProvider(CacheLocation.InContext, new HttpContextCacheProvider(() => new Dictionary<string, object>()));

        var provider = factory.GetProvider(CacheLocation.OutOfProcess);
        provider.Should().NotBeNull();
        provider.Should().BeOfType<DefaultNoCacheProvider>();
        provider.Should().BeSameAs(inContextProvider);
    }

    [Fact]
    public void factory___remove()
    {
        var factory = new DefaultCacheFactory();

        factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());
        factory.AddProvider(CacheLocation.InContext, new HttpContextCacheProvider(() => new Dictionary<string, object>()));

        var provider = factory.GetProvider(CacheLocation.InProcess);
        provider.Should().NotBeNull();

        provider = factory.GetProvider(CacheLocation.InContext);
        provider.Should().NotBeNull();

        factory.RemoveProvider(CacheLocation.InProcess);
        provider = factory.GetProvider(CacheLocation.InProcess);
        provider.Should().BeNull();

        provider = factory.GetProvider(CacheLocation.InContext);
        provider.Should().NotBeNull();

        factory.RemoveProvider(CacheLocation.InContext);
        provider = factory.GetProvider(CacheLocation.InContext);
        provider.Should().BeNull();
    }

    [Fact]
    public void factory___remove_when_doesnt_exist_doesnt_throw()
    {
        var factory = new DefaultCacheFactory();

        factory.AddProvider(CacheLocation.InProcess, new DefaultInProcCacheProvider());
        factory.AddProvider(CacheLocation.InContext, new HttpContextCacheProvider(() => new Dictionary<string, object>()));

        factory.RemoveProvider(CacheLocation.OutOfProcess);
    }
}