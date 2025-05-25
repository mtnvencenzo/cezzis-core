namespace Cezzi.Http;

using Microsoft.Extensions.DependencyInjection;
using System;

/// <summary>
/// 
/// </summary>
/// <typeparam name="I"></typeparam>
/// <typeparam name="T"></typeparam>
/// <remarks>Initializes a new instance of the <see cref="ServiceClientFactory{I, T}"/> class.</remarks>
/// <param name="serviceProvider">The service provider.</param>
public class ServiceClientFactory<I, T>(IServiceProvider serviceProvider) : IServiceClientFactory<I, T>
{
    private readonly IServiceProvider serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private I instance;

    /// <summary>Gets the instance.</summary>
    /// <returns></returns>
    public I GetInstance()
    {
        if (this.instance != null)
        {
            return this.instance;
        }

        this.instance = this.serviceProvider.GetRequiredService<I>();
        return this.instance;
    }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <seealso cref="Cezzi.Http.IServiceClientFactory&lt;I, T&gt;" />
/// <remarks>Initializes a new instance of the <see cref="ServiceClientFactory{I, T}"/> class.</remarks>
/// <param name="serviceProvider">The service provider.</param>
public class ServiceClientFactory<T>(IServiceProvider serviceProvider) : IServiceClientFactory<T>
{
    private readonly IServiceProvider serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    private T instance;

    /// <summary>Gets the instance.</summary>
    /// <returns></returns>
    public T GetInstance()
    {
        if (this.instance != null)
        {
            return this.instance;
        }

        this.instance = this.serviceProvider.GetRequiredService<T>();
        return this.instance;
    }
}
