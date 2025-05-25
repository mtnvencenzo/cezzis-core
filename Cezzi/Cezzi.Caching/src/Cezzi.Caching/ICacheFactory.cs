namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
public interface ICacheFactory
{
    /// <summary>Gets the provider.</summary>
    /// <param name="location">The location.</param>
    /// <returns></returns>
    ICacheProvider GetProvider(CacheLocation location);

    /// <summary>Adds the provider.</summary>
    /// <param name="location">The location.</param>
    /// <param name="provider">The provider.</param>
    /// <returns></returns>
    ICacheFactory AddProvider(CacheLocation location, ICacheProvider provider);

    /// <summary>Removes the provider.</summary>
    /// <param name="location">The location.</param>
    /// <returns></returns>
    ICacheFactory RemoveProvider(CacheLocation location);

    /// <summary>Determines whether the specified location has provider.</summary>
    /// <param name="location">The location.</param>
    /// <returns>
    ///   <c>true</c> if the specified location has provider; otherwise, <c>false</c>.
    /// </returns>
    bool HasProvider(CacheLocation location);
}
