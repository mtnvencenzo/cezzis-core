namespace Cezzi.Caching;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="CacheKey" /> class.
/// </remarks>
/// <param name="region">The region.</param>
/// <param name="baseKey">The base key.</param>
/// <param name="expirationSeconds">The expiration seconds.</param>
public class CacheKey(string region, string baseKey, int expirationSeconds)
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CacheKey" /> class.
    /// </summary>
    /// <param name="region">The region.</param>
    /// <param name="baseKey">The base key.</param>
    public CacheKey(string region, string baseKey) : this(region, baseKey, 0)
    {
    }

    /// <summary>Gets the region.</summary>
    /// <value>The region.</value>
    public string Region { get; private set; } = region;

    /// <summary>Gets the base key.</summary>
    /// <value>The base key.</value>
    public string BaseKey { get; private set; } = baseKey;

    /// <summary>Gets the key.</summary>
    /// <value>The key.</value>
    public string Key { get; private set; } = string.Format("{0}|{1}",
            string.IsNullOrEmpty(region) ? "_" : region,
            string.IsNullOrEmpty(baseKey) ? "_" : baseKey);

    /// <summary>Gets the expiration seconds.</summary>
    /// <value>The expiration seconds.</value>
    public int ExpirationSeconds { get; private set; } = (expirationSeconds > 0)
            ? expirationSeconds
            : 0;
}
