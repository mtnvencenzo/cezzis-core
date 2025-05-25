namespace Cezzi.Http;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

/// <summary>
/// 
/// </summary>
public static class ServiceClientExtension
{
    /// <summary>Registers the service client.</summary>
    /// <typeparam name="I"></typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="services">The services.</param>
    /// <param name="configureClient">The configure client.</param>
    /// <exception cref="System.ArgumentNullException">configureClient</exception>
    public static void RegisterServiceClient<I, T>(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureClient)
        where I : class
        where T : class, I
    {
        services.AddScoped<IServiceClientFactory<I, T>, ServiceClientFactory<I, T>>();

        if (configureClient == null)
        {
            throw new ArgumentNullException(nameof(configureClient));
        }

        services.AddHttpClient<I, T>(configureClient);
    }

    /// <summary>Registers the service client.</summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="services">The services.</param>
    /// <param name="configureClient">The configure client.</param>
    /// <exception cref="System.ArgumentNullException">configureClient</exception>
    public static void RegisterServiceClient<T>(
        this IServiceCollection services,
        Action<IServiceProvider, HttpClient> configureClient)
        where T : class
    {
        services.AddScoped<IServiceClientFactory<T>, ServiceClientFactory<T>>();

        if (configureClient == null)
        {
            throw new ArgumentNullException(nameof(configureClient));
        }

        services.AddHttpClient<T>(configureClient);
    }
}
