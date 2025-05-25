namespace Cezzi.Caching.Redis.Tests;

using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

public class ServiceBootStrapTests
{
    [Fact]
    public void servicebootstrap___throws_when_services_is_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
        {
            (null as IServiceCollection).UseStackExchangeRedisServices(null);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("services");
    }

    [Fact]
    public void servicebootstrap___throws_when_config_builder_is_null()
    {
        var ex = Assert.Throws<ArgumentNullException>(() =>
        {
            new ServiceCollection().UseStackExchangeRedisServices(null);
        });

        ex.Should().NotBeNull();
        ex.ParamName.Should().Be("configBuilder");
    }

    [Fact]
    public void servicebootstrap___retrieves_and_builds_config_when_connection_injected()
    {
        IServiceCollection services = new ServiceCollection();
        var activated = false;

        services.UseStackExchangeRedisServices((sp) =>
        {
            activated = true;

            return new RedisConfig
            {
                RetryMaxAttempts = 2,
                ReconnectErrorThreshold = 2,
                ReconnectMinFrequency = 3,
                ConnectionString = "test://redis-connection.io",
                Password = "test"
            };
        });

        var serviceProvider = services.BuildServiceProvider();
        var redisConnection = serviceProvider.GetRequiredService<RedisConnection>();
        redisConnection.Should().NotBeNull();
        activated.Should().BeTrue();

    }
}