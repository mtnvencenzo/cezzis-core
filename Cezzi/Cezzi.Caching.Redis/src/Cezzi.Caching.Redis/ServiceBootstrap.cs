namespace Cezzi.Caching.Redis;

using Microsoft.Extensions.DependencyInjection;
using System;

/// <summary>
/// 
/// </summary>
public static class ServiceBootstrap
{
    /// <summary>Uses the stack exchange redis services.</summary>
    /// <param name="services">The services.</param>
    /// <param name="configBuilder">The configuration builder.</param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentNullException">
    /// services
    /// or
    /// configBuilder
    /// </exception>
    public static IServiceCollection UseStackExchangeRedisServices(
        this IServiceCollection services,
        Func<IServiceProvider, RedisConfig> configBuilder)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configBuilder == null)
        {
            throw new ArgumentNullException(nameof(configBuilder));
        }

        return services
            .AddSingleton((sp) =>
            {
                var config = configBuilder(sp);

                return new RedisConnection(
                    connectionString: config.GetConnectionString(),
                    reconnectMinFrequency: config.ReconnectMinFrequency,
                    reconnectErrorThreshold: config.ReconnectErrorThreshold,
                    retryMaxAttempts: config.RetryMaxAttempts);
            });
    }
}
