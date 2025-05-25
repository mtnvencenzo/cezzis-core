namespace Cezzi.Caching.Core;

using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Cezzi.Caching.ICacheFactory" />
/// <remarks>
/// Initializes a new instance of the <see cref="DefaultCacheFactory"/> class.
/// </remarks>
/// <param name="providers">The providers.</param>
public class DefaultCacheFactory(Dictionary<CacheLocation, ICacheProvider> providers) : ICacheFactory
{
    private readonly Dictionary<CacheLocation, ICacheProvider> providers = (providers == null || providers.Count == 0)
        ? []
        : new Dictionary<CacheLocation, ICacheProvider>(providers);

    /// <summary>
    /// Initializes a new instance of the <see cref="DefaultCacheFactory"/> class.
    /// </summary>
    public DefaultCacheFactory() : this(null)
    {
    }

    /// <summary>Adds the provider.</summary>
    /// <param name="location">The location.</param>
    /// <param name="provider">The provider.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public ICacheFactory AddProvider(CacheLocation location, ICacheProvider provider)
    {
        if (this.HasProvider(location))
        {
            throw new ArgumentException("Provider location already exists");
        }

        this.providers.Add(location, provider);
        return this;
    }

    /// <summary>Gets the provider.</summary>
    /// <param name="location">The location.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public ICacheProvider GetProvider(CacheLocation location)
    {
        return this.HasProvider(location)
            ? this.providers[location]
            : null;
    }

    /// <summary>Determines whether the specified location has provider.</summary>
    /// <param name="location">The location.</param>
    /// <returns>
    ///   <c>true</c> if the specified location has provider; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool HasProvider(CacheLocation location) => this.providers.ContainsKey(location);

    /// <summary>Removes the provider.</summary>
    /// <param name="location">The location.</param>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public ICacheFactory RemoveProvider(CacheLocation location)
    {
        if (this.HasProvider(location))
        {
            this.providers.Remove(location);
        }

        return this;
    }
}
